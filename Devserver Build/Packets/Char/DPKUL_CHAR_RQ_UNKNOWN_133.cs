using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_CHAR_RQ_UNKNOWN_133 : Packet
    {
        public DPKUL_CHAR_RQ_UNKNOWN_133()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_CHAR_RQ_UNKNOWN_133;
        }

        //public short UnknownShort;
        //public byte UnknownByte;

        public int UnknownInt;
        // public int UnknownInt2;
        // public int UnknownInt3;
        //// public short UnknownShort; // 8192
        // public int UnknownInt4;
        // public int UnknownInt5;
        // public short UnknownShort;
        // public byte UnknownByte;
    }
}