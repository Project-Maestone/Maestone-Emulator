using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class Packet
    {
        public ushort Length;

        public byte MainId; // private
        public byte SubId;

        // Helper property to split the PacketType to the Main/Sub Id.
        protected PacketType Type
        {
            set
            {
                MainId = (byte)(ushort)((ushort)value & 0xFF);
                SubId = (byte)(ushort)((ushort)value >> 8);
            }
        }
    }
}