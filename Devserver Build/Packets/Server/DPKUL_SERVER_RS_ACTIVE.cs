using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_SERVER_RS_ACTIVE : Packet
    {
        public DPKUL_SERVER_RS_ACTIVE()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_SERVER_RS_ACTIVE;
        }

        public int Id;

        // (0 to 25 - Normal, 26 to 80 - Busy, 81 or over - Full)
        public int Traffic;

        [MarshalAs(UnmanagedType.U1)]
        public bool Active;
    }
}