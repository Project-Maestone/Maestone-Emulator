using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_USER_RQ_MACADDRESS : Packet
    {
        public DPKUL_USER_RQ_MACADDRESS()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_USER_RQ_MACADDRESS;
        }
    }
}