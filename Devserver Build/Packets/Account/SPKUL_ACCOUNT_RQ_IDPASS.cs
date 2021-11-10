using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPKUL_ACCOUNT_RQ_IDPASS : Packet
    {
        public SPKUL_ACCOUNT_RQ_IDPASS()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPKUL_ACCOUNT_RQ_IDPASS;
        }

        public byte IdPassLength;
        public byte IdLength;
        public byte PassLength;
        public byte LanguageId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] RawIdPass;
    }
}