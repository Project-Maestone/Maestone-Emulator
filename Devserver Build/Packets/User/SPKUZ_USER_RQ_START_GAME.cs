using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPKUZ_USER_RQ_START_GAME : Packet
    {
        public SPKUZ_USER_RQ_START_GAME()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPKUZ_USER_RQ_START_GAME;
        }

        public int CharacterKey;

        public int StartTime;
    }
}