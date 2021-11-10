using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPKUZ_USER_RQ_CHARLIST : Packet
    {
        public SPKUZ_USER_RQ_CHARLIST()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPKUZ_USER_RQ_CHARLIST;
        }

        [MarshalAs(UnmanagedType.U1)]
        public bool Allow;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] IPAddress;

        public int UnknownInt;
    }
}