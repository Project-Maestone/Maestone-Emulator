using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_ITEM_RQ_UNKNOWN : Packet
    {
        public DPKUZ_ITEM_RQ_UNKNOWN()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_ITEM_RQ_UNKNOWN;
        }

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        //public byte[] CharacterKey;

      
        public int UnkInt;
        public byte UnkByte3;
        public byte UnkByte4;
        public short UnkShort2;
        public int UnkIn2;
        public byte UnkByte5;
        public byte UnkByte6;

    }
}