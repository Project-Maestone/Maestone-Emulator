using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class DPK_HELO_RS_FORWARD : Packet
    {
        public DPK_HELO_RS_FORWARD()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPK_HELO_RS_FORWARD;
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] IPAddress;

        public ushort Port;
    }
}