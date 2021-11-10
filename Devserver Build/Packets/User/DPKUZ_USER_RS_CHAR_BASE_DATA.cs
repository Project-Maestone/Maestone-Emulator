using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_USER_RS_CHAR_BASE_DATA : Packet
    {
        public DPKUZ_USER_RS_CHAR_BASE_DATA()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_USER_RS_CHAR_BASE_DATA;
        }

        //public int CharacterKey;
        //public int m_nUnknown2;
        //public short Gender_JobId;
        //// Must be 1281
        //private short Unknown = 1281; // For loading map
        //public short Faction;
        //// Must be 4096
        //private short Unknown2 = 4096; // For loading Map
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        //public string Name;
        //public short Face_TatooId;
        //public short HairId;
        //public int HairColor;
        //public int SkinColor;
        //public int Size;
        //public int Weigth;
        //public int ModelTorsoId;
        //public int ModelHandId;
        //public int ModelShoeId;
        //public int ModelLegId;
        //public int m_nUnknown15;

        //public short m_nUnknown16; // position
        //public short m_nUnknown17; // position
        //public short m_nUnknown18; // position
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //private byte[] GAP;
        //public short m_nUnknown19;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        //private byte[] GAP_2;
        //public byte m_byUnknown;
        //public int m_nUnknown20;
        //public int m_nUnknown21;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //private byte[] GAP_3;
        //public short m_nUnknown22;
        //public short m_nUnknown23;
        //public int m_nUnknown24;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        //private byte[] GAP_4;
        //public int m_nUnknown25;
        //public int m_nUnknown26;
        //public byte m_byUnknown2;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        //private byte[] GAP_5;
        //public int m_nUnknown27;
        //public int m_nUnknown28;
        //public int m_nUnknown29;
        //public int m_nUnknown30;
        //public int m_nUnknown31;
        //public int m_nUnknown32;
        //public int m_nUnknown33;
        //public int m_nUnknown34;
        //public int m_nUnknown35;
        //public int m_nUnknown36;
        //public int m_nUnknown37;
        //public int m_nUnknown38;
        //public int m_nUnknown39;
        //public int m_nUnknown40;
        //public int m_nUnknown41;
        //public int m_nUnknown42;
        //public int m_nUnknown43;
        //public int m_nUnknown44;
        //public int m_nUnknown45;
        //public int m_nUnknown46;
        //public int m_nUnknown47;
        //public int m_nUnknown48;
        //public int m_nUnknown49;
        //public int m_nUnknown50;
        //public int m_nUnknown51;
        //public int m_nUnknown52;
        //public int m_nUnknown53;
        //public int m_nUnknown54;
        //public int m_nUnknown55;
        //public int m_nUnknown56;
        //public int m_nUnknown57;
        //public int m_nUnknown58;
        //public int m_nUnknown59;
        //public int m_nUnknown60;
        //public int m_nUnknown61;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //private byte[] GAP_6;
        //public byte m_byUnknown3;
        //public short m_nUnknown62;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        //private byte[] GAP_7;


        // New Char Base Data
        //#########
        public int m_nUnknown;
        public int m_nUnknown2;
       
    //    public short m_nUnknown3;
        // For loading map
      //  private short Unknown = 1281;
        public int m_nUnknown3;
        public int m_nUnknown4;

       // public short m_nUnknown4;
     //   private short Unknown2 = 4096;
        // Must be 4096
        // For loading Map

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string m_strUnknown;
        public short m_nUnknown5;
        public short m_nUnknown6;
        public int m_nUnknown7;
        public int m_nUnknown8;
        public int m_nUnknown9;
        public int m_nUnknown10;
        public int m_nUnknown11;
        public int m_nUnknown12;
        public int m_nUnknown13;
        public int m_nUnknown14;
        public int m_nUnknown15;
        public ushort m_nUnknown16;
        public ushort m_nUnknown17;
        public ushort m_nUnknown18;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] GAP;
        public ushort m_nUnknown19;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        private byte[] GAP_2;
        public byte m_byUnknown;
        public int m_nUnknown20;
        public int m_nUnknown21;
        public int unk01;
        public ushort m_nUnknown22;
        public ushort m_nUnknown23;
        public int unk02;
        public ushort m_nUnknown24;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        private byte[] GAP_52;
        public int m_nUnknown25;
        public int m_nUnknown26;
        public byte m_byUnknown2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private byte[] GAP_5;
        public int m_nUnknown27;
        public int m_nUnknown28;
        public int m_nUnknown29;
        public int m_nUnknown30;
        public int m_nUnknown31;
        public int m_nUnknown32;
        public int m_nUnknown33;
        public int m_nUnknown34;
        public int m_nUnknown35;
        public int m_nUnknown36;
        public int m_nUnknown37;
        public int m_nUnknown38;
        public int m_nUnknown39;
        public int m_nUnknown40;
        public int m_nUnknown41;
        public int m_nUnknown42;
        public int m_nUnknown43;
        public int m_nUnknown44;
        public int m_nUnknown45;
        public int m_nUnknown46;
        public int m_nUnknown47;
        public int m_nUnknown48;
        public int m_nUnknown49;
        public int m_nUnknown50;
        public int m_nUnknown51;
        public int m_nUnknown52;
        public int m_nUnknown53;
        public int m_nUnknown54;
        public int m_nUnknown55;
        public int m_nUnknown56;
        public int m_nUnknown57;
        public int m_nUnknown58;
        public int m_nUnknown59;
        public int m_nUnknown60;
        public int m_nUnknown61;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private byte[] GAP__6;
        public bool blabool;
        public ushort m_nUnknown62;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        private byte[] GAP_7;

        public int unk2;


    }
}