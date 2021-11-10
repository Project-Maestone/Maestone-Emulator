using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPK_EXIT_RQ_LOGOUT : Packet
    {
        public SPK_EXIT_RQ_LOGOUT()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPK_EXIT_RQ_LOGOUT;
        }
    }
}