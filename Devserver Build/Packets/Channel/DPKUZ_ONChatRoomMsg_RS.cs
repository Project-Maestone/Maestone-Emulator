using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_ONChatRoomMsg_RS : Packet
    {
        public DPKUZ_ONChatRoomMsg_RS()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_ONChatRoomMsg_RS;
        }
    }
}