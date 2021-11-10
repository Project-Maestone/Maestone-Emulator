﻿using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_SERVER_RS_LISTEND : Packet
    {
        public DPKUL_SERVER_RS_LISTEND()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_SERVER_RS_LISTEND;
        }
    }
}