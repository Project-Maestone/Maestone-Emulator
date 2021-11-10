using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPK_CRYPT_RS_CHANGE : Packet
    {
        public DPK_CRYPT_RS_CHANGE()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPK_CRYPT_RS_CHANGE;
        }

        public byte UnknownArraySize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] UnknownArray;
    }
}