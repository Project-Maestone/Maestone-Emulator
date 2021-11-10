using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_USER_RS_RSA : Packet
    {
        public DPKUL_USER_RS_RSA()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_USER_RS_RSA;
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 148)]
        public byte[] RsaPublicKey;
    }
}