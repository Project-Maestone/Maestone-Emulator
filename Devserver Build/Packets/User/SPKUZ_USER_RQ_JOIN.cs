using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPKUZ_USER_RQ_JOIN : Packet
    {
        public SPKUZ_USER_RQ_JOIN()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPKUZ_USER_RQ_JOIN;
        }
    }
}