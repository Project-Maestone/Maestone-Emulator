using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_QUEST_RS_EVENT_BEGIN : Packet
    {
        public DPKUZ_QUEST_RS_EVENT_BEGIN()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_QUEST_RS_EVENT_BEGIN;
        }

        //public int Id;

        //// (0 to 25 - Normal, 26 to 80 - Busy, 81 or over - Full)
        //public int Traffic;

        //[MarshalAs(UnmanagedType.U1)]
        //public bool Active;
    }
}