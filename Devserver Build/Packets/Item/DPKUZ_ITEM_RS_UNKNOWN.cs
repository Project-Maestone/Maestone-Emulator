using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_ITEM_RS_UNKNOWN : Packet
    {
        public DPKUZ_ITEM_RS_UNKNOWN()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_ITEM_RS_UNKNOWN;
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        public byte[] CharacterKey;


        public int UnkownInt;

    
    }
}