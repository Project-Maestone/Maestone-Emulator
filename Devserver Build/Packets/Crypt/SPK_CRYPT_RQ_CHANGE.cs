using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPK_CRYPT_RQ_CHANGE : Packet
    {
        public SPK_CRYPT_RQ_CHANGE()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPK_CRYPT_RQ_CHANGE;
        }

        public int UnknownInt;
    }
}