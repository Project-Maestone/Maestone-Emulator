using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_ITEM_RS_ITEMLIST_END : Packet
    {
        public DPKUZ_ITEM_RS_ITEMLIST_END()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_ITEM_RS_ITEMLIST_END;
        }

        public int CharacterKey;
    }
}