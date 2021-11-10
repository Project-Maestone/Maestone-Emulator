using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPK_HELO_RS_AGREE : Packet
    {
        public DPK_HELO_RS_AGREE()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPK_HELO_RS_AGREE;
        }

        public byte ServerType;
        public int EngineVersion;
        public int ClientVersion;
        public int ServerVersion;
    }
}