using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_USER_RQ_CHARSEL : Packet
    {
        public DPKUZ_USER_RQ_CHARSEL()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_USER_RQ_CHARSEL;
        }

        public int CharId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
        public byte[] CharPasswordBytes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 520)]
        public byte[] UnknownString2;

        [MarshalAs(UnmanagedType.U1)]
        public bool UnknownBool;
    }
}