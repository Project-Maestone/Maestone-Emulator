using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUZ_USER_RS_CHARLIST_DATA : Packet
    {
        public DPKUZ_USER_RS_CHARLIST_DATA()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUZ_USER_RS_CHARLIST_DATA;
        }

        public int Key;

        // 1 = Male, Warrior
        // 2 = Female, Warrior
        // 3 = Male, Mage
        // 4 = Female, Mage
        // 5 = Male, Ranger
        // 6 = Female, Ranger
        // 7 = Male, Cleric
        // 8 = Female, Cleric

          public short Gender_JobId;
//        public int Gender_Class;
        // Must be 1281
          private short Unknown = 1281;

        // 1 = Superion Guardian
        // 2 = Temple Knight
       // public int Faction;
       public short Faction;

        // Must be 4096
       private short Unknown2 = 4096;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string Guild;

        public short Face_TatooId;
        public short HairId;

        // RGB
        public int HairColor;
        public int SkinColor;

        // from 0 to 10
        public int Size;

        // from 0 to 10
        public int Weigth;

        public int ModelTorsoId;
        public int ModelHandId;
        public int ModelShoeId;
        public int ModelLegId;

        public byte Level;

        // 0 - 3
        public byte Slot;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
        public byte[] UNK;

        [MarshalAs(UnmanagedType.U1)]
        public bool Locked;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 18)]
        public byte[] UNK2;

        // Minutes, -1 disables character deletion.
        public short DeleteTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] UNK3;

    }
}