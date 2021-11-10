using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_ACCOUNT_RS_MACADDRESS : Packet
    {
        public DPKUZ_ACCOUNT_RS_MACADDRESS()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_ACCOUNT_RS_MACADDRESS;
        }

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        //public byte[] MacAddress;
        public int unk;

    }
}