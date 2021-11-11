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
                //EngineVersion = 0x07151257,
                //ClientVersion = 0x01020002,
                //ServerVersion = 0x07041700

                //  /* 2015
                EngineVersion = 0x07151257,
                ClientVersion = 0x01020002,
                ServerVersion = 0x03040207
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
            userRsCharListData.Name = "GM_Peter";
            userRsCharListData.Guild = "projectmaestone";
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


            //  userRsCharListData.Gender_Class = 83951618 ;
            userRsCharListData.Gender_JobId = 1;
            userRsCharListData.Faction = 2;
            //userRsCharListData.Faction = 268566529;


            userRsCharListData.Name = "GM_Guenther";
            userRsCharListData.Guild = "projectmaestone";
            userRsCharListData.Face_TatooId = 151;
            userRsCharListData.HairId = 4;
            userRsCharListData.HairColor = 2296857; //; //2296857; //230C19
            userRsCharListData.SkinColor = 15905905; //0xF2B471;// 15905905;
            userRsCharListData.Size = 5;
            userRsCharListData.Weigth = 5;
            userRsCharListData.ModelTorsoId = 1895825412;
            userRsCharListData.ModelHandId = 1895825410;
            userRsCharListData.ModelShoeId = 1895825409;
            userRsCharListData.ModelLegId = 1895825411;
            userRsCharListData.Level = 105;
            userRsCharListData.Slot = 1;
            userRsCharListData.DeleteTime = -1;

            packetSender.SendPacket(userRsCharListData);

            userRsCharListData.Key = 29748867;
            userRsCharListData.Gender_JobId = 1;
            userRsCharListData.Faction = 8;
            userRsCharListData.Name = "GM_Ruediger";
            userRsCharListData.Guild = "projectmaestone";
            userRsCharListData.Face_TatooId = 222;
            userRsCharListData.HairId = 97;
            userRsCharListData.HairColor = 3087666;
            userRsCharListData.SkinColor = 15249285;
            userRsCharListData.Size = 5;
            userRsCharListData.Weigth = 5;
            userRsCharListData.ModelTorsoId = 1895826088;
            userRsCharListData.ModelHandId = 1895826086;
            userRsCharListData.ModelShoeId = 1895826085;
            userRsCharListData.ModelLegId = 1895826087;
            userRsCharListData.Level = 100;
            userRsCharListData.Slot = 2;
            userRsCharListData.DeleteTime = -1;

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

            //userRsCharBaseData.CharacterKey = 29748865;
            //userRsCharBaseData.m_nUnknown2 = 0; // 0
            //userRsCharBaseData.Gender_JobId = 8; // 8 short 
            //userRsCharBaseData.Faction = 0; // 1 short  //Skill tree
            //userRsCharBaseData.Name = "GM_Peter";
            //userRsCharBaseData.Face_TatooId = 277; // 234 short
            //userRsCharBaseData.HairId = 6; // 113 short
            //userRsCharBaseData.HairColor = 0x70170d; //3087666 int 
            //userRsCharBaseData.SkinColor = 0x966d50;
            //userRsCharBaseData.Size = 5; // 8 int
            //userRsCharBaseData.Weigth = 5; // 10 8
            //userRsCharBaseData.ModelTorsoId = 609; // 1895826017 int
            //userRsCharBaseData.ModelHandId = 607; // 1895826015 int
            //userRsCharBaseData.ModelShoeId = 606; // 1895826014 int
            //userRsCharBaseData.ModelLegId = 608; // 1895826016 int

            //userRsCharBaseData.m_nUnknown15 = 50397251; // Start ZoneID
            //userRsCharBaseData.m_nUnknown16 = 2255; // X POS
            //userRsCharBaseData.m_nUnknown17 = 2350; // Y POS
            //userRsCharBaseData.m_nUnknown18 = 0; // Z Pos

            //userRsCharBaseData.m_nUnknown19 = 1; // AllianceID SG TK ??

            //userRsCharBaseData.m_byUnknown = 105; // level

            //userRsCharBaseData.m_nUnknown20 = 2;
            //userRsCharBaseData.m_nUnknown21 = 3;

            //userRsCharBaseData.m_nUnknown22 = 4;

            //userRsCharBaseData.m_nUnknown23 = 5;

            //userRsCharBaseData.m_nUnknown24 = 25;

            //userRsCharBaseData.m_nUnknown25 = 26;
            //userRsCharBaseData.m_nUnknown26 = 27;

            //userRsCharBaseData.m_byUnknown2 = 28;

            //userRsCharBaseData.m_nUnknown27 = 29;
            //userRsCharBaseData.m_nUnknown28 = 30;
            //userRsCharBaseData.m_nUnknown29 = 31;
            //userRsCharBaseData.m_nUnknown30 = 32;
            //userRsCharBaseData.m_nUnknown31 = 33;
            //userRsCharBaseData.m_nUnknown32 = 34;
            //userRsCharBaseData.m_nUnknown33 = 35;
            //userRsCharBaseData.m_nUnknown34 = 36;
            //userRsCharBaseData.m_nUnknown35 = 37;
            //userRsCharBaseData.m_nUnknown36 = 38;
            //userRsCharBaseData.m_nUnknown37 = 39;
            //userRsCharBaseData.m_nUnknown38 = 40;
            //userRsCharBaseData.m_nUnknown39 = 41;
            //userRsCharBaseData.m_nUnknown40 = 42;
            //userRsCharBaseData.m_nUnknown41 = 43;
            //userRsCharBaseData.m_nUnknown42 = 44;
            //userRsCharBaseData.m_nUnknown43 = 45;
            //userRsCharBaseData.m_nUnknown44 = 46;
            //userRsCharBaseData.m_nUnknown45 = 47;
            //userRsCharBaseData.m_nUnknown46 = 48;
            //userRsCharBaseData.m_nUnknown47 = 49;
            //userRsCharBaseData.m_nUnknown48 = 50;
            //userRsCharBaseData.m_nUnknown49 = 51;
            //userRsCharBaseData.m_nUnknown50 = 52;
            //userRsCharBaseData.m_nUnknown51 = 53;
            //userRsCharBaseData.m_nUnknown52 = 54;
            //userRsCharBaseData.m_nUnknown53 = 55;
            //userRsCharBaseData.m_nUnknown54 = 56;
            //userRsCharBaseData.m_nUnknown55 = 57;
            //userRsCharBaseData.m_nUnknown56 = 58;
            //userRsCharBaseData.m_nUnknown57 = 59;
            //userRsCharBaseData.m_nUnknown58 = 60;
            //userRsCharBaseData.m_nUnknown59 = 61;
            //userRsCharBaseData.m_nUnknown60 = 62;
            //userRsCharBaseData.m_nUnknown61 = 63;

            //userRsCharBaseData.m_byUnknown3 = 1;

            //userRsCharBaseData.m_nUnknown62 = 0;


            // Neu



            //userRsCharBaseData.CharacterKey = 29748865;
            //userRsCharBaseData.m_nUnknown2 = 4096; // 0
            //userRsCharBaseData.Gender_JobId = 8; // 8 short 
            //userRsCharBaseData.Faction = 8; // 1 short 
            //userRsCharBaseData.Name = "GM_Peter";
            //userRsCharBaseData.Face_TatooId = 277; // 234 short
            //userRsCharBaseData.HairId = 6; // 113 short
            //userRsCharBaseData.HairColor = 0x70170d; //3087666 int 
            //userRsCharBaseData.SkinColor = 0x966d50;
            //userRsCharBaseData.Size = 5; // 8 int
            //userRsCharBaseData.Weigth = 5; // 10 8
            //userRsCharBaseData.ModelTorsoId = 609; // 1895826017 int
            //userRsCharBaseData.ModelHandId = 607; // 1895826015 int
            //userRsCharBaseData.ModelShoeId = 606; // 1895826014 int
            //userRsCharBaseData.ModelLegId = 608; // 1895826016 int

            //userRsCharListData.Key = 29748866;
            //userRsCharListData.Gender_JobId = 7;
            //userRsCharListData.Faction = 1;
            //userRsCharListData.Name = "GM_Peter";
            //userRsCharListData.Guild = "HYBR2D";
            //userRsCharListData.Face_TatooId = 222;
            //userRsCharListData.HairId = 97;
            //userRsCharListData.HairColor = 3087666;
            //userRsCharListData.SkinColor = 15249285;
            //userRsCharListData.Size = 5;
            //userRsCharListData.Weigth = 5;
            //userRsCharListData.ModelTorsoId = 1895826088;
            //userRsCharListData.ModelHandId = 1895826086;
            //userRsCharListData.ModelShoeId = 1895826085;
            //userRsCharListData.ModelLegId = 1895826087;
            //userRsCharListData.Level = 100;




            //userRsCharBaseData.m_nUnknown = 29748865;
            //userRsCharBaseData.m_nUnknown2 = 1; //268435457
            //userRsCharBaseData.m_nUnknown3 = 8; // 83951624; 8
            //userRsCharBaseData.m_nUnknown4 = 8; //268435457; 8
            //userRsCharBaseData.m_strUnknown = "GM_Peter";
            //userRsCharBaseData.m_nUnknown5 = 277; //277
            //userRsCharBaseData.m_nUnknown6 = 6;
            //userRsCharBaseData.m_nUnknown7 = 0x70170d;
            //userRsCharBaseData.m_nUnknown8 = 0x966d50;
            //userRsCharBaseData.m_nUnknown9 = 8;
            //userRsCharBaseData.m_nUnknown10 = 6;
            //userRsCharBaseData.m_nUnknown11 = 609;
            //userRsCharBaseData.m_nUnknown12 = 607;
            //userRsCharBaseData.m_nUnknown13 = 606;
            //userRsCharBaseData.m_nUnknown14 = 608;

            userRsCharBaseData.m_nUnknown = 29748866;
            userRsCharBaseData.m_nUnknown2 = 22700; //268435457                        66817 Int 1 oder 83951617  oder 69632 oder  
            userRsCharBaseData.m_nUnknown3 = 83951618; // 83951624; 8            1            135168    Int 2  oder 268435458        mit faction id : 132353 oder 83951618
            userRsCharBaseData.m_nUnknown4 = 268566529; //268435457; 81
            userRsCharBaseData.m_strUnknown = "GM_Peter";
            userRsCharBaseData.m_nUnknown5 = 151; //277
            userRsCharBaseData.m_nUnknown6 = 4;
            userRsCharBaseData.m_nUnknown7 = 2296857;
            userRsCharBaseData.m_nUnknown8 = 15905905;
            userRsCharBaseData.m_nUnknown9 = 8;
            userRsCharBaseData.m_nUnknown10 = 3;
            userRsCharBaseData.m_nUnknown11 = 1895825412;
            userRsCharBaseData.m_nUnknown12 = 1895825410;
            userRsCharBaseData.m_nUnknown13 = 1895825409;
            userRsCharBaseData.m_nUnknown14 = 1895825411;


            userRsCharBaseData.m_nUnknown15 = 50397251; // ZoneID

            userRsCharBaseData.m_nUnknown16 = 2300; //PosX
            userRsCharBaseData.m_nUnknown17 = 2300; // Pos Y
            userRsCharBaseData.m_nUnknown18 = 0; //Pos Z

            userRsCharBaseData.m_nUnknown19 = 2; // AllianceID

            userRsCharBaseData.m_byUnknown = 1; // Level

            userRsCharBaseData.m_nUnknown20 = 88; //Min EXP
            userRsCharBaseData.m_nUnknown21 = 500; // Max Exp

            userRsCharBaseData.unk01 = 112; // Gold

            userRsCharBaseData.m_nUnknown22 = 91; // Stattpoints


            userRsCharBaseData.m_nUnknown23 = 13; // Skilloints in SkilLTree

            userRsCharBaseData.unk02 = 199;
            userRsCharBaseData.m_nUnknown24 = 55;

            userRsCharBaseData.m_nUnknown25 = 15; // Kill Count Total

            userRsCharBaseData.m_nUnknown26 = 216; // Cumulative Points

            userRsCharBaseData.m_byUnknown2 = 1; // DP Rank

            userRsCharBaseData.m_nUnknown27 = 99; // Ranking ??

            userRsCharBaseData.m_nUnknown28 = 27; // usable points

            userRsCharBaseData.m_nUnknown29 = 9;

            userRsCharBaseData.m_nUnknown30 = 29; // Health

            userRsCharBaseData.m_nUnknown31 = 30; // Mana
            userRsCharBaseData.m_nUnknown32 = 8;

            userRsCharBaseData.m_nUnknown33 = 32;

            userRsCharBaseData.m_nUnknown34 = 33;

            userRsCharBaseData.m_nUnknown35 = 34;

            userRsCharBaseData.m_nUnknown36 = 35; // HP regenerationRate percentage

            userRsCharBaseData.m_nUnknown37 = 36; // MP regenerationRate percantage

            userRsCharBaseData.m_nUnknown38 = 37; //Vitality
            userRsCharBaseData.m_nUnknown39 = 38; // Strength
            userRsCharBaseData.m_nUnknown40 = 39; // Agility
            userRsCharBaseData.m_nUnknown41 = 40; // Intelligence
            userRsCharBaseData.m_nUnknown42 = 41; // Wisdom

            userRsCharBaseData.m_nUnknown43 = 42; // Physical Damage

            userRsCharBaseData.m_nUnknown44 = 43; // Magic Damage

            userRsCharBaseData.m_nUnknown45 = 44; // Damange Amp

            userRsCharBaseData.m_nUnknown46 = 45; // Magical Damage percentage

            userRsCharBaseData.m_nUnknown47 = 46; // Physical Defense

            userRsCharBaseData.m_nUnknown48 = 47; // Magic Defense

            userRsCharBaseData.m_nUnknown49 = 48; // Physical Hit Rating

            userRsCharBaseData.m_nUnknown50 = 49; //Physical Dodge

            userRsCharBaseData.m_nUnknown51 = 50; // Physical CH

            userRsCharBaseData.m_nUnknown52 = 51; // Physical KT - Dodge

            userRsCharBaseData.m_nUnknown53 = 52; // Damage Through Physical KT percentage

            userRsCharBaseData.m_nUnknown54 = 53; // Magical CH

            userRsCharBaseData.m_nUnknown55 = 54; // Magical KT - Dodge

            userRsCharBaseData.m_nUnknown56 = 55; // Healing Limits percantage

            userRsCharBaseData.m_nUnknown57 = 56; // Shield blockrate in percantage

            userRsCharBaseData.m_nUnknown58 = 57; // Fire Resistance percantage

            userRsCharBaseData.m_nUnknown59 = 58; // Ice Resistance percantage

            userRsCharBaseData.m_nUnknown60 = 59; // Lighting Resitance percantage

            userRsCharBaseData.m_nUnknown61 = 60; // Poison Resistenz percantage

            userRsCharBaseData.blabool = false;

            userRsCharBaseData.m_nUnknown62 = 10004;

            userRsCharBaseData.unk2 = 200; // Faction match kill 


            packetSender.SendPacket(userRsCharBaseData);

            var rsItemListStart = new DPKUZ_ITEM_RS_ITEMLIST_START
            {
                CharacterKey = 29748866
            };

            packetSender.SendPacket(rsItemListStart);

            var rsItemListNonEquip = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 49197547,
                Proto = 108330503,
                View = 2165916,
                SlotUnk = 1,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };


            packetSender.SendPacket(rsItemListNonEquip);

            var rsItemListNonEquip2 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100841,
                Proto = 104783665,
                View = 2300904,
                SlotUnk = 2,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip2);

            var rsItemListNonEquip3 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100840,
                Proto = 104748538,
                View = 2386226,
                SlotUnk = 9,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip3);

            var rsItemListNonEquip4 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100839,
                Proto = 105936222,
                View = 3011048,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip4);

            var rsItemListNonEquip5 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100838,
                Proto = 108295501,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip5);

            var rsItemListNonEquip6 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100752,
                Proto = 108295500,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip6);


            var rsItemListNonEquip7 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100838,
                Proto = 108295501,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip7);


            var rsItemListNonEquip8 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100837,
                Proto = 104756550,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip8);



            var rsItemListNonEquip9 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100836,
                Proto = 104756551,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip9);


            var rsItemListNonEquip10 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100834,
                Proto = 106387423,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip10);

            var rsItemListNonEquip11 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100833,
                Proto = 108295499,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip11);

            var rsItemListNonEquip12 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100832,
                Proto = 104783664,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip12);


            var rsItemListNonEquip13 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100831,
                Proto = 108295508,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip13);


            var rsItemListNonEquip14 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100830,
                Proto = 105936224,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip14);

            var rsItemListNonEquip15 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100828,
                Proto = 105936220,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip15);


            var rsItemListNonEquip16 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100827,
                Proto = 108295498,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip16);

            var rsItemListNonEquip17 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100826,
                Proto = 108295504,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip17);


            var rsItemListNonEquip18 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100825,
                Proto = 109999445,
                View = 3011048,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip18);

            var rsItemListNonEquip19 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100824,
                Proto = 105936225,
                View = 3011048,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip19);

            var rsItemListNonEquip20 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100822,
                Proto = 104783665,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip20);

            var rsItemListNonEquip21 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100821,
                Proto = 104748539,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip21);


            var rsItemListNonEquip22 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100820,
                Proto = 104783648,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip22);


            var rsItemListNonEquip23 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100819,
                Proto = 104783664,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip23);


            var rsItemListNonEquip24 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100818,
                Proto = 108295492,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip24);

            var rsItemListNonEquip25 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100816,
                Proto = 108295500,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip25);

            var rsItemListNonEquip26 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100815,
                Proto = 104783665,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip26);


            var rsItemListNonEquip27 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100814,
                Proto = 105936223,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip27);

            var rsItemListNonEquip28 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100813,
                Proto = 108295502,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip28);


            var rsItemListNonEquip29 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100809,
                Proto = 108295498,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip29);

            var rsItemListNonEquip30 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100808,
                Proto = 108330496,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip30);

            var rsItemListNonEquip31 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100807,
                Proto = 108295503,
                View = 2386226,
                SlotUnk = 1,
                SlotUnk2 = 2,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip31);

            var rsItemListNonEquip32 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100806,
                Proto = 108295504,
                View = 3010703,
                SlotUnk = 1,
                SlotUnk2 = 2,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip32);

            var rsItemListNonEquip33 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100805,
                Proto = 108295500,
                View = 3010703,
                SlotUnk = 1,
                SlotUnk2 = 2,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip33);

            var rsItemListNonEquip34 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100804,
                Proto = 104756551,
                View = 2386226,
                SlotUnk = 1,
                SlotUnk2 = 2,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip34);

            var rsItemListNonEquip35 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100803,
                Proto = 104783665,
                View = 2300904,
                SlotUnk = 1,
                SlotUnk2 = 2,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip35);

            var rsItemListNonEquip36 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100801,
                Proto = 105936225,
                View = 3011048,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip36);

            var rsItemListNonEquip37 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100799,
                Proto = 104783656,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip37);

            var rsItemListNonEquip38 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100806,
                Proto = 108295504,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip38);

            var rsItemListNonEquip39 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100798,
                Proto = 104783655,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip39);

            var rsItemListNonEquip40 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100797,
                Proto = 108295499,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip40);

            var rsItemListNonEquip41 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100796,
                Proto = 108295492,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip41);

            var rsItemListNonEquip42 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100795,
                Proto = 106422958,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip42);

            var rsItemListNonEquip43 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100794,
                Proto = 108295505,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip43);

            var rsItemListNonEquip44 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100793,
                Proto = 108330487,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip44);

            var rsItemListNonEquip45 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100790,
                Proto = 108295498,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip45);

            var rsItemListNonEquip46 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100789,
                Proto = 108295499,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip46);

            var rsItemListNonEquip47 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100788,
                Proto = 108295502,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip47);

            var rsItemListNonEquip48 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100787,
                Proto = 108295504,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip48);

            var rsItemListNonEquip49 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100786,
                Proto = 104783665,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip49);

            var rsItemListNonEquip50 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100784,
                Proto = 104783664,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip50);

            var rsItemListNonEquip51 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100783,
                Proto = 104783664,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip51);

            var rsItemListNonEquip52 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100782,
                Proto = 108295492,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip52);

            var rsItemListNonEquip53 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100781,
                Proto = 108295493,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip53);

            var rsItemListNonEquip54 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100780,
                Proto = 105930020,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip54);

            var rsItemListNonEquip55 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100779,
                Proto = 108330530,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip55);

            var rsItemListNonEquip56 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100778,
                Proto = 105936221,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip56);

            var rsItemListNonEquip57 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100774,
                Proto = 108295505,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip57);

            var rsItemListNonEquip58 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100773,
                Proto = 108295506,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip58);

            var rsItemListNonEquip59 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100772,
                Proto = 108295507,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip59);

            var rsItemListNonEquip60 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100771,
                Proto = 108295505,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip60);

            var rsItemListNonEquip61 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100783,
                Proto = 104783664,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip61);


            var rsItemListNonEquip62 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100768,
                Proto = 104783662,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip62);

            var rsItemListNonEquip63 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100767,
                Proto = 104783661,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip63);

            var rsItemListNonEquip64 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100766,
                Proto = 104783665,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip64);

            var rsItemListNonEquip65 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100765,
                Proto = 106387408,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip65);

            var rsItemListNonEquip66 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100764,
                Proto = 105930000,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip66);

            var rsItemListNonEquip67 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100761,
                Proto = 106387379,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip67);

            var rsItemListNonEquip68 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100760,
                Proto = 108295508,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip68);

            var rsItemListNonEquip69 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100759,
                Proto = 108295496,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip69);

            var rsItemListNonEquip70 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100758,
                Proto = 105936223,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip70);

            var rsItemListNonEquip71 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100757,
                Proto = 108325473,
                View = 2300904,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip71);

            var rsItemListNonEquip72 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100754,
                Proto = 108295508,
                View = 2386226,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip72);

            var rsItemListNonEquip73 = new DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP
            {
                ItemHKey = 30100753,
                Proto = 108295502,
                View = 3010703,
                SlotUnk = 0,
                SlotUnk2 = 0,
                Count = 1,
                Unknown = 0,
                Unknown2 = 0,
                Unknown3 = 0,
                Unknown4 = 0
            };

            packetSender.SendPacket(rsItemListNonEquip73);




















            var rsItemListEnd = new DPKUZ_ITEM_RS_ITEMLIST_END
            {
                CharacterKey = 29748866
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
                UnknownInt = 29748866,

                UnknownInt2 = 1,
                Level = 0,
                UnknownByte = 0
            };

            packetSender.SendPacket(skillData);
            // GetSkill Data 58 |43
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
                CharacterKey = 29748866,
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
            var CHarRsUnknown = new DPKUL_CHAR_RS_UNKNOWN
            {
                UnknownInt = 1
            };



            //var charRSSync = new DPKUL_CHAR_RS_SYNC();

            //packetSender.SendPacket(charRSSync);

            //var NpcUnknown = new DPKUL_CHAR_RS_NPC();
            //packetSender.SendPacket(NpcUnknown);

            //var NpcUnknown = new DPKUL_CHAR_RS_NPC();
            //packetSender.SendPacket(NpcUnknown);
            //  packetSender.SendPacket(CHarRsUnknown);

        }

        [Packet(HandlerType.Zone, PacketType.DPKUL_CHAR_RQ_UNKNOWN_133)]
        public static void DPKUL_CHAR_RQ_UNKNOWN_133(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is DPKUL_CHAR_RQ_UNKNOWN_133 charRqUnknown133))
                return;

            //var NpcUnknown = new DPKUL_CHAR_RS_NPC();
            //packetSender.SendPacket(NpcUnknown);
            var charRSSync = new DPKUZ_QUEST_RS_EVENT_BEGIN();
            packetSender.SendPacket(charRSSync);

        }


        // To DO 
        [Packet(HandlerType.Zone, PacketType.DPKUZ_ITEM_RQ_UNKNOWN)]
        public static void DPKUZ_ITEM_RQ_UNKNOWN(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is DPKUZ_STORAGE_RS_UNK itemRQUnknown))
                return;

            //var ItemRSUnkown = new DPKUZ_ITEM_RS_UNKNOWN
            //{
            //    CharacterKey = itemRQUnknown.CharacterKey,
            //    UnkownInt = 67
            //};


            //packetSender.SendPacket(ItemRSUnkown);

        }



        // CHANNEL
        [Packet(HandlerType.Zone, PacketType.DPKUZ_CHANNEL_RQ_UNKOWN)]
        public static void DPKUZ_CHANNEL_RQ_UNKOWN(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is DPKUZ_CHANNEL_RQ_UNKOWN ChannelRQUnkown))
                return;

            var ChannelRsUnkown = new SPKUL_ADMIN_RS_UNK
            {
                //   UnknownInt = ChannelRQUnkown.
            };

            packetSender.SendPacket(ChannelRsUnkown);

        }
        // Admin CMD
        [Packet(HandlerType.Zone, PacketType.SPKUL_ADMIN_RQ_UNK)]
        public static void SPKUL_ADMIN_RQ_UNK(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUL_ADMIN_RQ_UNK AdminRQUnkown))
                return;

            var AdminRsUnkown = new SPKUL_ADMIN_RS_UNK
            {
                Unknown = AdminRQUnkown.Unknown,
                Unknown2 = AdminRQUnkown.Unknown2,
                Unknown3 = AdminRQUnkown.Unknown3,
                Unknown4 = AdminRQUnkown.Unknown4,
                Unknown5 = AdminRQUnkown.Unknown5,
                Unknown6 = AdminRQUnkown.Unknown6

            };

            packetSender.SendPacket(AdminRsUnkown);

        }

    }
}