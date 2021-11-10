using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP : Packet
    {
        public DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP;
        }

        public int ItemHKey;
        public int Proto;
        public int View;
        public byte SlotUnk;
        public byte SlotUnk2;
        public short Count;
        public int Unknown;
        public int Unknown2;
        public byte Unknown3;
        public short Unknown4;
    }
}