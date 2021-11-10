using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_CHANNEL_RS_UNKOWN : Packet
    {
        public DPKUZ_CHANNEL_RS_UNKOWN()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_CHANNEL_RS_UNKOWN;
        }
    }
}