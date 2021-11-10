using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPKUL_USER_RQ_MACADDRESS : Packet
    {
        public SPKUL_USER_RQ_MACADDRESS()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPKUL_USER_RQ_MACADDRESS;
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] MacAddress;
    }
}