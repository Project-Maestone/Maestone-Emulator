﻿using System;
using System.Runtime.InteropServices;

namespace DevServer.Packets
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public class DPKUL_ACCOUNT_RS_FAILED : Packet
    {
        public DPKUL_ACCOUNT_RS_FAILED()
        {
            Length = Convert.ToUInt16(Marshal.SizeOf(this));

            Type = PacketType.DPKUL_ACCOUNT_RS_FAILED;
        }

        // Reasons

        // 0x1		IDS_NETWORK_ERROR
        // 0x101	IDS_ACCOUNT_PROCEDURE_FAILED
        // 0x102	IDS_LOBBY_ERR_USING_ACCOUNT
        // 0x103	IDS_LOBBY_ERR_ACCOUNT_REQUIRE_CONFIRM
        // 0x104	IDS_ACCOUNT_MISMATCH_IDPASS
        // 0x105	IDS_ACCOUNT_BLOCKED
        // 0x106	IDS_ACCOUNT_DISCONNECT_PREVCONNECTION
        // 0x107	IDS_ACCOUNT_HAVE_NOT_AUTHORITY
        // 0x108	IDS_ACCOUNT_NOT_CLOSEBETA_MEMBER
        // 0x109	IDS_ACCOUNT_RETRY_LATER
        // 0x832	IDS_DONOT_CREATE_POCKETVIEW_YET
        // 0x9901	IDS_LOBBY_ERR_DENIED_ADDRESS
        // 0x9902	IDS_LOBBY_ERR_NOT_OPEN_SERVER
        // 0x9902	IDS_LOBBY_ERR_UNKNOWN
        // 0x10201	IDS_LOBBY_ERR_NO_WORLD_CONNECTED
        // 0x10202	IDS_LOBBY_ERR_INACTIVE_WORLD
        // 0x10203	IDS_LOBBY_ERR_FULL_WORLD
        // 0x10204	IDS_LOBBY_ERR_NOT_OPEN_SERVER
        // 0x220001	IDS_ALREADY_CHARACTER_BEING
        // 0x220003	IDS_CREATE_FAILED_DISABLEACCEPT
        // 0x310003	IDS_LOBBY_ERR_USING_ACCOUNT
        // 0x310010	IDS_LOBBY_ERR_FAILED_CREATE_CHAR
        // 0x310011	IDS_LOBBY_ERR_INVALID_CHAR_PROTO
        // 0x310012	IDS_LOBBY_ERR_INVALID_FACE_TYPE
        // 0x310013	IDS_LOBBY_ERR_INVALID_CHAR_HAIR
        // 0x310015	IDS_LOBBY_ERR_EXIST_CHAR_NAME
        // 0x310016	IDS_LOBBY_ERR_DATABASE_ERROR
        // 0x310017	IDS_LOBBY_ERR_INVALID_SLOT
        // 0x310018	IDS_LOBBY_ERR_DUPLICATE_SLOT
        // 0x310019	IDS_LOBBY_WRONG_NAME_KOREA
        // 0x310020	IDS_LOBBY_ERR_NOT_CREATE_ANYMORE
        // 0x310021	IDS_LOBBY_ERR_INVALID_CHAR
        // 0x310022	IDS_LOBBY_ERR_INVALID_ZONE
        // 0x310023	IDS_LOBBY_ERR_NO_ACTIVE_ZONESERVER
        // 0x310025	IDS_LOBBY_ERR_NO_ZONE_CONNECTED
        // 0x310026	IDS_LOBBY_ERR_INACTIVE_ZONE
        // 0x310028	IDS_LOBBY_ERR_FAILED_DELCHAR
        // 0x310029	IDS_CLTNET_ERR_GUILD_MASTER
        // 0x310030	IDS_LOBBY_ERR_DENY_ADDRESS
        // 0x310031	IDS_LOBBY_ERR_NOT_READY
        // 0x310032	IDS_USER_CREATE_INVALID_ALLIANCE_PROTO
        // 0x310033	IDS_ERRORCODE_USER_CHARACTER_INVALID_PASSWORD
        // 0x310034	IDS_ERRORCODE_USER_CHARACTER_ALREADY_SETTED_PASSWORD
        // 0x310035	IDS_ERRORCODE_USER_CHARACTER_DIDNT_SET_PASSWORD
        // 0x310044	IDS_USER_ALLIANCE_CREATE_CHAR_BLOCK
        // 0x310045	IDS_USER_NOT_DELETE_CHAR_CHARPET_USE
        // 0x310046	IDS_USER_NOT_DELETE_PET_REGISTERED

        public int Reason;
    }
}