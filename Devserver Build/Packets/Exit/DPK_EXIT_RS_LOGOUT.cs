using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPK_EXIT_RS_LOGOUT : Packet
    {
        public DPK_EXIT_RS_LOGOUT()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPK_EXIT_RS_LOGOUT;
        }

        public int Unknown;
    }
}