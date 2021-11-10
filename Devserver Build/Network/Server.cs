using System;
using System.Net;
using System.Reflection;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using DevServer.Packets;
using DevServer.Handlers;

namespace DevServer.Network
{
    public class Server : TcpListener
    {
        public HandlerType HandlerType { get; }

        private readonly Dictionary<PacketType, Action<Packet, Client>> _packetHandlers;

        public Server(int serverPort, HandlerType handlerType) : base(IPAddress.Any, serverPort)
        {
            HandlerType = handlerType;

            _packetHandlers = new Dictionary<PacketType, Action<Packet, Client>>();
        }

        public void Initialize()
        {
            Start();
            BeginAcceptTcpClient(OnAccept, this);
        }

        private void OnAccept(IAsyncResult asyncResult)
        {
            var tcpClient = EndAcceptTcpClient(asyncResult);

            Log.WriteInfo($"[{HandlerType}] Accepted client connection from {tcpClient.Client.RemoteEndPoint}.");

            var client = new Client(tcpClient, this);

            BeginAcceptTcpClient(OnAccept, this);
        }

        public void AddPacketHandler(MethodInfo methodInfo, PacketType packetType)
        {
            var packetHandler = methodInfo.CreateDelegate(typeof(Action<Packet, Client>)) as Action<Packet, Client>;

            _packetHandlers.Add(packetType, packetHandler);
        }

        public void Handle(Client packetSender, byte[] packetData)
        {
            var packetLength = BitConverter.ToUInt16(packetData, 0) & 0x1FFF;

            var packetMainId = packetData[2];
            var packetSubId = packetData[3];

            var packetRawType = BitConverter.ToUInt16(packetData, 2);

            if (!Enum.IsDefined(typeof(PacketType), packetRawType))
            {
                Log.WriteWarning($"[{HandlerType}] Received unknown packet. ( Length: {packetLength}, MainId: {packetMainId}, SubId: {packetSubId} )");
                return;
            }

            if (!Enum.TryParse(packetRawType.ToString(), out PacketType packetType))
            {
                Log.WriteError($"[{HandlerType}] Failed to get packet type. ( Length: {packetLength}, MainId: {packetMainId}, SubId: {packetSubId} )");
                return;
            }

            var packetStruct = Type.GetType($"{typeof(Packet).Namespace}.{packetType}");

            if (packetStruct == null)
            {
                Log.WriteWarning($"[{HandlerType}] Packet {packetType} is unmapped.");
                return;
            }

            var fixedLength = Marshal.SizeOf(packetStruct);

            if (packetData.Length > fixedLength)
                Array.Resize(ref packetData, fixedLength);

            var arrayHandle = GCHandle.Alloc(packetData, GCHandleType.Pinned);

            var clientPacket = Marshal.PtrToStructure(arrayHandle.AddrOfPinnedObject(), packetStruct) as Packet;

            arrayHandle.Free();

            if (!_packetHandlers.TryGetValue(packetType, out var packetHandler))
            {
                Log.WriteWarning($"[{HandlerType}] Packet {packetType} is unhandled.");
                return;
            }

            Log.WriteSuccess($"[{HandlerType}] Received {packetHandler.Method.Name}.");

            packetHandler(clientPacket, packetSender);
        }
    }
}