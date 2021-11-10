using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_USER_RS_CHARSEL : Packet
    {
        public DPKUZ_USER_RS_CHARSEL()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_USER_RS_CHARSEL;
        }

        [MarshalAs(UnmanagedType.U1)]
        public bool Allow;

        public int CharacterKey;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] IPAddress;
    }
}