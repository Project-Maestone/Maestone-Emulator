﻿using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class SPKUL_SERVER_RQ_GETLIST : Packet
    {
        public SPKUL_SERVER_RQ_GETLIST()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.SPKUL_SERVER_RQ_GETLIST;
        }
    }
}