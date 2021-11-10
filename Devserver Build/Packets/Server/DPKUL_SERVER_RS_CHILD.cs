using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_SERVER_RS_CHILD : Packet
    {
        public DPKUL_SERVER_RS_CHILD()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_SERVER_RS_CHILD;
        }

        public int Id;

        // Format: <Flag>Name (Eg: <GE>Papyrus )

        // Flags
        // GE - Germany
        // EU - Europe
        // FR - France

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string Name;

        public int DisplayOrder;
        public byte UnknownByte;
        public byte UnknownByte2;
    }
}