using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_SKILL_RS_END : Packet
    {
        public DPKUZ_SKILL_RS_END()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_SKILL_RS_END;
        }
    }
}