using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPKUZ_USER_RQ_CREATE_CHAR : Packet
    {
        public SPKUZ_USER_RQ_CREATE_CHAR()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPKUZ_USER_RQ_CREATE_CHAR;
        }

        public short Gender_JobId;
        public short Unknown;

        public short Faction;
        public short Unknown2;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Name;

        public short Face_TatooId;
        public short HairId;

        public int HairColor;
        public int SkinColor;

        public int Size;
        public int Weigth;

        public int ModelTorsoId;
        public int ModelHandId;
        public int ModelShoeId;
        public int ModelLegId;

        public byte Slot;
    }
}