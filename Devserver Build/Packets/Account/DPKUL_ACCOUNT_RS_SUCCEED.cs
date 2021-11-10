using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_ACCOUNT_RS_SUCCEED : Packet
    {
        public DPKUL_ACCOUNT_RS_SUCCEED()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_ACCOUNT_RS_SUCCEED;
        }
    }
}