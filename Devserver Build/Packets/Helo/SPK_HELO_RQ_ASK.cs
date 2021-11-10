using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SPK_HELO_RQ_ASK : Packet
    {
        public SPK_HELO_RQ_ASK()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPK_HELO_RQ_ASK;
        }

        public int UnknownInt;
        public int UnknownInt2;
        public int UnknownInt3;
    }
}