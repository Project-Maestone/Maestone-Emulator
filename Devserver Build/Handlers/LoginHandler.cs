using System;
using System.Net;
using System.Text;
using System.Security.Cryptography;

using DevServer.Network;
using DevServer.Packets;

namespace DevServer.Handlers
{
    public static class LoginHandler
    {
        [Packet(HandlerType.Login, PacketType.SPK_HELO_RQ_ASK)]
        public static void SPK_HELO_RQ_ASK(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPK_HELO_RQ_ASK heloRqAsk))
                return;

            var heloRsAgree = new DPK_HELO_RS_AGREE
            {
                ServerType = 0,
                //EngineVersion = 0x07151257,
                //ClientVersion = 0x01020000,
                //ServerVersion = 0x07041700

                //  /* 2015
                EngineVersion = 0x07151257, // 50594311
                ClientVersion = 0x01020000,
                ServerVersion = 0x03040207
            };

            packetSender.SendPacket(heloRsAgree);

            var accountRsRsa = new DPKUL_ACCOUNT_RS_RSA
            {
                RsaPublicKey = packetSender.PublicKey
            };

            packetSender.SendPacket(accountRsRsa);
        }

        [Packet(HandlerType.Login, PacketType.SPK_CRYPT_RQ_CHANGE)]
        public static void SPK_CRYPT_RQ_CHANGE(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPK_CRYPT_RQ_CHANGE cryptRqChange))
                return;

            var cryptRsChange = new DPK_CRYPT_RS_CHANGE();

            packetSender.SendPacket(cryptRsChange);
        }

        [Packet(HandlerType.Login, PacketType.SPKUL_ACCOUNT_RQ_IDPASS)]
        public static void SPKUL_ACCOUNT_RQ_IDPASS(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUL_ACCOUNT_RQ_IDPASS accountRqIdPass))
                return;

            Array.Resize(ref accountRqIdPass.RawIdPass, accountRqIdPass.IdPassLength);
            var decryptedIdPass = accountRqIdPass.RawIdPass;

            // TODO: Check [GAME]IDPASS_ENCRYPT on Prop.ini to determine if we need to decrypt id and pass.
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportCspBlob(packetSender.PrivateKey);

                Array.Reverse(decryptedIdPass, 0, decryptedIdPass.Length);

                decryptedIdPass = rsa.Decrypt(decryptedIdPass, false);
            }

            //var id = Encoding.Unicode.GetString(decryptedIdPass, 0, accountRqIdPass.IdLength * 2);
            //var pass = Encoding.Unicode.GetString(decryptedIdPass, accountRqIdPass.IdLength * 2, accountRqIdPass.PassLength * 2);

            //  /* 2015
            var id = Encoding.UTF8.GetString(decryptedIdPass, 0, accountRqIdPass.IdLength);
            var pass = Encoding.UTF8.GetString(decryptedIdPass, accountRqIdPass.IdLength, accountRqIdPass.PassLength);

            // Example of rejecting login.
            //if (id != "Maestone" || pass != "Maestone")
            // 2015
            if (id != "authcode" || pass != "12345")
            {
                var accountRsFailed = new DPKUL_ACCOUNT_RS_FAILED
                {
                    Reason = 0x104 // IDS_ACCOUNT_MISMATCH_IDPASS
                };

                packetSender.SendPacket(accountRsFailed);

                return;
            }

            packetSender.SendPacket(new DPKUL_ACCOUNT_RS_SUCCEED());
        }

        [Packet(HandlerType.Login, PacketType.SPKUL_ACCOUNT_RQ_MACADDRESS)]
        public static void SPKUL_ACCOUNT_RQ_MACADDRESS(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUL_ACCOUNT_RQ_MACADDRESS accountRqMacAddress))
                return;

            packetSender.SendPacket(new DPKUL_ACCOUNT_RS_MACADDRESS());
        }
        [Packet(HandlerType.Login, PacketType.DPKUZ_ACCOUNT_RS_MACADDRESS)]
        public static void DPKUZ_ACCOUNT_RS_MACADDRESS(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is DPKUZ_ACCOUNT_RS_MACADDRESS accountRqMacAddress))
                return;
        }

        [Packet(HandlerType.Login, PacketType.SPKUL_SERVER_RQ_GETLIST)]
        public static void SPKUL_SERVER_RQ_GETLIST(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUL_SERVER_RQ_GETLIST serverRqGetList))
                return;

            packetSender.SendPacket(new DPKUL_SERVER_RS_LISTBEGIN());

            var serverNames = new[] { "Yotunheim", "<GE>Papyrus", "<EU>Pectus", "<GE>Delphi" };

            for (var i = 0; i < serverNames.Length; i++)
            {
                var serverRsChild = new DPKUL_SERVER_RS_CHILD
                {
                    Id = i,
                    Name = serverNames[i]
                };

                packetSender.SendPacket(serverRsChild);
            }

            packetSender.SendPacket(new DPKUL_SERVER_RS_LISTEND());

            for (var i = 0; i < serverNames.Length; i++)
            {
                var serverRsActive = new DPKUL_SERVER_RS_ACTIVE
                {
                    Id = i,
                    Active = true
                };

                packetSender.SendPacket(serverRsActive);
            }
        }

        [Packet(HandlerType.Login, PacketType.SPKUL_SERVER_RQ_SELECT)]
        public static void SPKUL_SERVER_RQ_SELECT(Packet clientPacket, Client packetSender)
        {
            if (!(clientPacket is SPKUL_SERVER_RQ_SELECT serverRqSelect))
                return;

            var serverRsSelect = new DPKUL_SERVER_RS_SELECT
            {
                Id = serverRqSelect.Id
            };

            packetSender.SendPacket(serverRsSelect);

            var heloRsForward = new DPK_HELO_RS_FORWARD
            {
                IPAddress = IPAddress.Parse("127.0.0.1").GetAddressBytes(),
                Port = 21002
            };

            packetSender.SendPacket(heloRsForward);
        }
    }
}