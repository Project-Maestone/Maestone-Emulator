using System;
using System.Net;
using System.Security.Cryptography;

using DevServer.Network;
using DevServer.Packets;

namespace DevServer.Handlers
{
    public static class ZoneHandler
    {
        [Packet(HandlerType.Zone, PacketType.SPK_EXIT_RQ_LOGOUT)]
        public static void SPK_EXIT_RQ_LOGOUT(Packet clientPacket, Client packetSender)
        {
            var exitRsLogout = new DPK_EXIT_RS_LOGOUT
            {
                Unknown = 0
            };

            packetSender.SendPacket(exitRsLogout);
        }

        [Packet(HandlerType.Zone, PacketType.SPKUZ_USER_RQ_JOIN)]
        public static void SPKUZ_USER_RQ_JOIN(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUZ_USER_RQ_JOIN userRqJoin))
                return;

            var userRsJoin = new DPKUZ_USER_RS_JOIN
            {
                ErrorCode = 0
            };

            packetSender.SendPacket(userRsJoin);
        }

        [Packet(HandlerType.Zone, PacketType.SPKUZ_USER_RQ_UNKNOWN)]
        public static void SPKUZ_USER_RQ_UNKNOWN(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUZ_USER_RQ_UNKNOWN userRqUnknown))
                return;
        }

        [Packet(HandlerType.Zone, PacketType.SPK_HELO_RQ_ASK)]
        public static void SPK_HELO_RQ_ASK(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPK_HELO_RQ_ASK heloRqAsk))
                return;

            var heloRsAgree = new DPK_HELO_RS_AGREE
            {
                ServerType = 2,
                EngineVersion = 0x07151257,
                ClientVersion = 0x01020002,
                ServerVersion = 0x07041700

                /* 2015
                EngineVersion = 0x07151257,
                ClientVersion = 0x01020002,
                ServerVersion = 0x03040207 */
            };

            packetSender.SendPacket(heloRsAgree);

            packetSender.SendPacket(new DPKUL_USER_RQ_MACADDRESS());

            var userRsRsa = new DPKUL_USER_RS_RSA
            {
                RsaPublicKey = packetSender.PublicKey
            };

            packetSender.SendPacket(userRsRsa);
        }

        [Packet(HandlerType.Zone, PacketType.SPKUZ_USER_RQ_CHARLIST)]
        public static void SPKUZ_USER_RQ_CHARLIST(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUZ_USER_RQ_CHARLIST userRqCharList))
                return;

            packetSender.SendPacket(new DPKUZ_USER_RS_CHARLIST_BEGIN());

            var userRsCharListData = new DPKUZ_USER_RS_CHARLIST_DATA();

            userRsCharListData.Key = 29748865;
            userRsCharListData.Gender_JobId = 8;
            userRsCharListData.Faction = 1;
            userRsCharListData.Name = "GM_EDITz";
            userRsCharListData.Guild = "HYBR2D";
            userRsCharListData.Face_TatooId = 277;
            userRsCharListData.HairId = 6;
            userRsCharListData.HairColor = 0x70170d;
            userRsCharListData.SkinColor = 0x966d50;
            userRsCharListData.Size = 5;
            userRsCharListData.Weigth = 5;
            userRsCharListData.ModelTorsoId = 609;
            userRsCharListData.ModelHandId = 607;
            userRsCharListData.ModelShoeId = 606;
            userRsCharListData.ModelLegId = 608;
            userRsCharListData.Level = 100;
            userRsCharListData.Slot = 0;
            userRsCharListData.DeleteTime = -1;

            packetSender.SendPacket(userRsCharListData);

            userRsCharListData.Key = 29748866;
            userRsCharListData.Name = "GM_EDITz2";
            userRsCharListData.Slot = 1;

            packetSender.SendPacket(userRsCharListData);

            packetSender.SendPacket(new DPKUZ_USER_RS_CHARLIST_END());
        }

        [Packet(HandlerType.Zone, PacketType.SPKUZ_USER_RQ_PCINFO)]
        public static void SPKUZ_USER_RQ_PCINFO(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUZ_USER_RQ_PCINFO userRqPcInfo))
                return;

            // TODO

            packetSender.SendPacket(new DPKUZ_USER_RS_BEGIN_PCINFO());

            var userRsCharBaseData = new DPKUZ_USER_RS_CHAR_BASE_DATA();

            userRsCharBaseData.CharacterKey = 29748865;
            userRsCharBaseData.m_nUnknown2 = 0;
            userRsCharBaseData.Gender_JobId = 8;
            userRsCharBaseData.Faction = 0;
            userRsCharBaseData.Name = "GM_EDITz";
            userRsCharBaseData.Face_TatooId = 277;
            userRsCharBaseData.HairId = 6;
            userRsCharBaseData.HairColor = 0x70170d;
            userRsCharBaseData.SkinColor = 0x966d50;
            userRsCharBaseData.Size = 5;
            userRsCharBaseData.Weigth = 5;
            userRsCharBaseData.ModelTorsoId = 609;
            userRsCharBaseData.ModelHandId = 607;
            userRsCharBaseData.ModelShoeId = 606;
            userRsCharBaseData.ModelLegId = 608;

            userRsCharBaseData.m_nUnknown15 = 50397251;

            userRsCharBaseData.m_nUnknown16 = 16;
            userRsCharBaseData.m_nUnknown17 = 17;
            userRsCharBaseData.m_nUnknown18 = 18;

            userRsCharBaseData.m_nUnknown19 = 19;

            userRsCharBaseData.m_byUnknown = 20;

            userRsCharBaseData.m_nUnknown20 = 21;
            userRsCharBaseData.m_nUnknown21 = 22;

            userRsCharBaseData.m_nUnknown22 = 23;
            userRsCharBaseData.m_nUnknown23 = 24;

            userRsCharBaseData.m_nUnknown24 = 25;

            userRsCharBaseData.m_nUnknown25 = 26;
            userRsCharBaseData.m_nUnknown26 = 27;

            userRsCharBaseData.m_byUnknown2 = 28;

            userRsCharBaseData.m_nUnknown27 = 29;
            userRsCharBaseData.m_nUnknown28 = 30;
            userRsCharBaseData.m_nUnknown29 = 31;
            userRsCharBaseData.m_nUnknown30 = 32;
            userRsCharBaseData.m_nUnknown31 = 33;
            userRsCharBaseData.m_nUnknown32 = 34;
            userRsCharBaseData.m_nUnknown33 = 35;
            userRsCharBaseData.m_nUnknown34 = 36;
            userRsCharBaseData.m_nUnknown35 = 37;
            userRsCharBaseData.m_nUnknown36 = 38;
            userRsCharBaseData.m_nUnknown37 = 39;
            userRsCharBaseData.m_nUnknown38 = 40;
            userRsCharBaseData.m_nUnknown39 = 41;
            userRsCharBaseData.m_nUnknown40 = 42;
            userRsCharBaseData.m_nUnknown41 = 43;
            userRsCharBaseData.m_nUnknown42 = 44;
            userRsCharBaseData.m_nUnknown43 = 45;
            userRsCharBaseData.m_nUnknown44 = 46;
            userRsCharBaseData.m_nUnknown45 = 47;
            userRsCharBaseData.m_nUnknown46 = 48;
            userRsCharBaseData.m_nUnknown47 = 49;
            userRsCharBaseData.m_nUnknown48 = 50;
            userRsCharBaseData.m_nUnknown49 = 51;
            userRsCharBaseData.m_nUnknown50 = 52;
            userRsCharBaseData.m_nUnknown51 = 53;
            userRsCharBaseData.m_nUnknown52 = 54;
            userRsCharBaseData.m_nUnknown53 = 55;
            userRsCharBaseData.m_nUnknown54 = 56;
            userRsCharBaseData.m_nUnknown55 = 57;
            userRsCharBaseData.m_nUnknown56 = 58;
            userRsCharBaseData.m_nUnknown57 = 59;
            userRsCharBaseData.m_nUnknown58 = 60;
            userRsCharBaseData.m_nUnknown59 = 61;
            userRsCharBaseData.m_nUnknown60 = 62;
            userRsCharBaseData.m_nUnknown61 = 63;

            userRsCharBaseData.m_byUnknown3 = 64;

            userRsCharBaseData.m_nUnknown62 = 65;

            packetSender.SendPacket(userRsCharBaseData);

            var rsItemListStart = new DPKUZ_ITEM_RS_ITEMLIST_START
            {
                CharacterKey = 29748865
            };

            packetSender.SendPacket(rsItemListStart);

            var rsItemListNonEquip = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 49197547,
                Proto = 108330503,
                View = 2165916,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip);

            var rsItemListEnd = new DPKUZ_ITEM_RS_ITEMLIST_END
            {
                CharacterKey = 29748865
            };

            packetSender.SendPacket(rsItemListEnd);

            var userRsExtDta = new DPKUZ_USER_RS_EXT_DTA
            {
                UnknownInt = 19
            };

            packetSender.SendPacket(userRsExtDta);

            packetSender.SendPacket(new DPKUZ_SKILL_RS_BEGIN());

            var skillData = new DPKUZ_SKILL_RS_DATA
            {
                UnknownInt = 29748865,

                UnknownInt2 = 5,
                Level = 6,
                UnknownByte = 7
            };

            // packetSender.SendPacket(skillData);

            packetSender.SendPacket(new DPKUZ_SKILL_RS_END());

            var userRsEndPcInfo = new DPKUZ_USER_RS_END_PCINFO
            {
                AdminLevel = 128 // 128 or 5
            };

            packetSender.SendPacket(userRsEndPcInfo);

            packetSender.SendPacket(new DPKUZ_USER_RS_START_PERMISSION());
        }

        [Packet(HandlerType.Zone, PacketType.SPKUZ_USER_RQ_START_GAME)]
        public static void SPKUZ_USER_RQ_START_GAME(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUZ_USER_RQ_START_GAME userRqStartGame))
                return;

            var userRsStartGame = new DPKUZ_USER_RS_START_GAME
            {
                CharacterKey = userRqStartGame.CharacterKey,
                StartTime = userRqStartGame.StartTime
            };

            packetSender.SendPacket(userRsStartGame);
        }

        [Packet(HandlerType.Zone, PacketType.DPKUZ_USER_RQ_CHARSEL)]
        public static void DPKUZ_USER_RQ_CHARSEL(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is DPKUZ_USER_RQ_CHARSEL userRqCharSel))
                return;

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(packetSender.PrivateKey);

                Array.Reverse(userRqCharSel.CharPasswordBytes, 0, userRqCharSel.CharPasswordBytes.Length);

                userRqCharSel.CharPasswordBytes = rsa.Decrypt(userRqCharSel.CharPasswordBytes, false);
            }

            // TODO
            // Reject selecting character based with password check.

            var userRsCharSel = new DPKUZ_USER_RS_CHARSEL
            {
                Allow = true,
                CharacterKey = 29748865,
                IPAddress = IPAddress.Parse("127.0.0.1").GetAddressBytes()
            };

            packetSender.SendPacket(userRsCharSel);
        }

        [Packet(HandlerType.Zone, PacketType.SPKUZ_USER_RQ_CREATE_CHAR)]
        public static void SPKUZ_USER_RQ_CREATE_CHAR(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUZ_USER_RQ_CREATE_CHAR userRqCreateChar))
                return;
        }

        [Packet(HandlerType.Zone, PacketType.SPKUL_USER_RQ_MACADDRESS)]
        public static void SPKUL_USER_RQ_MACADDRESS(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUL_USER_RQ_MACADDRESS userRqMacAddress))
                return;
        }

        [Packet(HandlerType.Zone, PacketType.DPKUL_CHAR_RQ_UNKNOWN)]
        public static void DPKUL_CHAR_RQ_UNKNOWN(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is DPKUL_CHAR_RQ_UNKNOWN charRqUnknown))
                return;
        }

        [Packet(HandlerType.Zone, PacketType.DPKUZ_ITEM_RS_UNKNOWN)]
        public static void DPKUZ_ITEM_RS_UNKNOWN(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is DPKUZ_ITEM_RS_UNKNOWN itemRsUnknown))
                return;
        }
    }
}