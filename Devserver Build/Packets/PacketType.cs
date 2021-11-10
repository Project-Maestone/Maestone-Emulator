namespace DevServer.Packets
{
    public enum PacketType : ushort
    {
        // 1 - Helo
        SPK_HELO_RQ_ASK = 1 | 10 << 8,
        DPK_HELO_RS_AGREE = 1 | 20 << 8,
        DPK_HELO_RS_FORWARD = 1 | 30 << 8,

        // 2 - Exit
        SPK_EXIT_RQ_ASK = 2 | 10 << 8,
        SPK_EXIT_RQ_POSSIBLE = 2 | 41 << 8,
        SPK_EXIT_RQ_LOGOUT = 2 | 48 << 8,
        DPK_EXIT_RS_LOGOUT = 2 | 49 << 8,

        // 3 - Crypt
        SPK_CRYPT_RQ_CHANGE = 3 | 10 << 8,
        DPK_CRYPT_RS_CHANGE = 3 | 20 << 8,

        // 4 - Error

        // 11 - Account
        SPKUL_ACCOUNT_RQ_IDPASS = 11 | 1 << 8,
        DPKUL_ACCOUNT_RS_SUCCEED = 11 | 2 << 8,
        DPKUL_ACCOUNT_RS_FAILED = 11 | 3 << 8,
        SPKUL_ACCOUNT_RQ_MACADDRESS = 11 | 11 << 8,
        DPKUL_ACCOUNT_RS_MACADDRESS = 11 | 12 << 8,
        DPKUL_ACCOUNT_RS_RSA = 11 | 13 << 8,
        DPKUZ_ACCOUNT_RS_MACADDRESS = 11 | 20 << 8,

        // 12 - Server
        SPKUL_SERVER_RQ_GETLIST = 12 | 1 << 8,
        DPKUL_SERVER_RS_LISTBEGIN = 12 | 2 << 8,
        DPKUL_SERVER_RS_CHILD = 12 | 3 << 8,
        DPKUL_SERVER_RS_ACTIVE = 12 | 4 << 8,
        DPKUL_SERVER_RS_LISTEND = 12 | 5 << 8,
        SPKUL_SERVER_RQ_SELECT = 12 | 10 << 8,
        DPKUL_SERVER_RS_SELECT = 12 | 11 << 8,

        // 51 - User
        SPKUZ_USER_RQ_JOIN = 51 | 1 << 8,
        DPKUZ_USER_RS_JOIN = 51 | 2 << 8,
        SPKUZ_USER_RQ_UNKNOWN = 51 | 3 << 8,
        SPKUZ_USER_RQ_CHARLIST = 51 | 11 << 8,
        DPKUZ_USER_RS_CHARLIST_BEGIN = 51 | 12 << 8,
        DPKUZ_USER_RS_CHARLIST_DATA = 51 | 13 << 8,
        DPKUZ_USER_RS_CHARLIST_END = 51 | 14 << 8,
        SPKUZ_USER_RQ_PCINFO = 51 | 21 << 8,
        DPKUZ_USER_RS_BEGIN_PCINFO = 51 | 22 << 8,
        DPKUZ_USER_RS_END_PCINFO = 51 | 23 << 8,
        DPKUZ_USER_RS_CHAR_BASE_DATA = 51 | 24 << 8,
        SPKUZ_USER_RQ_START_GAME = 51 | 25 << 8,
        DPKUZ_USER_RS_START_GAME = 51 | 26 << 8,
        DPKUZ_USER_RS_EXT_DTA = 51 | 27 << 8,
        DPKUZ_USER_RQ_CHARSEL = 51 | 31 << 8,
        DPKUZ_USER_RS_CHARSEL = 51 | 32 << 8,
        DPKUZ_USER_RS_CHARDEL = 51 | 34 << 8,
        DPKUZ_USER_RS_START_PERMISSION = 51 | 35 << 8,
        DPKUZ_USER_PENALTY = 51 | 36 << 8,
        SPKUZ_USER_RQ_CREATE_CHAR = 51 | 41 << 8,
        DPKUL_USER_RS_RSA = 51 | 47 << 8,
        DPKUZ_USER_RS_WAIT_DELETE_CHAR = 51 | 52 << 8,
        DPKUL_USER_RQ_MACADDRESS = 51 | 251 << 8,
        SPKUL_USER_RQ_MACADDRESS = 51 | 252 << 8,

        // 52 - Char
        DPKUL_CHAR_RS_UNKNOWN = 52 | 235 <<8,
        DPKUL_CHAR_RS_SYNC = 52 | 4 << 8,

       

        DPKUL_CHAR_RQ_UNKNOWN = 52 | 231 << 8,

        DPKUL_CHAR_RQ_UNKNOWN2_248 = 52 | 248 << 8,
        DPKUL_CHAR_RQ_UNKNOWN_133 = 52 | 133 << 8,

        DPKUL_CHAR_RQ_SKILLTREECLOSE = 52 | 55 << 8,
        DPKUL_CHAR_RS_NPC = 52 | 56 << 8,
        DPKUL_CHAR_RS_MOVE_Player = 52 | 58 << 8,
        DPKUL_CHAR_RQ_DELETEITEMINPOECKET = 52 | 54 << 8,
        SPKUZ_CHARACTER_RQ_RESERVE_AUTO_ATTACK_CANCEL = 52 | 38 << 8,
        SPKUZ_CHARACTER_RQ_RESERVE_AUTO_ATTACK = 52 | 37 << 8,



        //53  Automove? Target ? 

        // 54 - Item
        DPKUZ_ITEM_RS_ITEMLIST_START = 54 | 1 << 8,
        DPKUZ_ITEM_RS_ITEMLIST_NONEQUIP = 54 | 3 << 8,
        DPKUZ_ITEM_RS_ITEMLIST_END = 54 | 5 << 8,
        DPKUZ_ITEM_RQ_UNKNOWN = 54 | 11 << 8,
        DPKUZ_ITEM_RS_UNKNOWN = 54 | 6 << 8,




        // 58 - Skill
        DPKUZ_SKILL_RS_BEGIN = 58 | 1 << 8,
        DPKUZ_SKILL_RS_DATA = 58 | 2 << 8,
        DPKUZ_SKILL_RS_END = 58 | 3 << 8,
        SPKUZ_SKILL_RQ_RESERVE_CAST = 58 | 11 << 8,


        // 62 - Quest
        DPKUZ_QUEST_RS_EVENT_BEGIN = 62 | 1,
        DPKUZ_QUEST_RS_EVENT_END = 62 | 2,
        DPKUZ_QUEST_RS_PROMPT = 62 | 3,
        DPKUZ_QUEST_RS_USER_SELECT_START = 62 | 5,
        DPKUZ_QUEST_RS_USER_SELECT = 62 | 6,
        DPKUZ_QUEST_RS_USER_SELECT_END = 62 | 7,
        DPKUZ_QUEST_RS_QUEST_START_AGREE = 62 | 10,
        DPKUZ_QUEST_RS_QUEST_STEP = 62 | 12,
        DPKUZ_QUEST_RS_QUEST_COMPLETE_AGREE = 62 | 13,
        DPKUZ_QUEST_RS_QUEST_LIST_BEGIN = 62 | 15,
        DPKUZ_QUEST_RS_QUEST_LIST_ITEM = 62 | 16,
        DPKUZ_QUEST_RS_QUEST_LIST_END = 62 | 17,
        DPKUZ_QUEST_RS_OBJ_SAY = 62 | 19,
        DPKUZ_QUEST_RS_PC_SAY = 62 | 21,
        DPKUZ_QUEST_RS_CAMERA_MODE = 62 | 22,
        DPKUZ_QUEST_RS_CHANGE_BGM = 62 | 23,
        DPKUZ_QUEST_RS_SYSTEM_MESSAGE = 62 |24,
        DPKUZ_QUEST_RS_APPEAR_MARK = 62 | 26,
        DPKUZ_QUEST_RS_QUEST_TIMER = 62 | 27,
        DPKUZ_QUEST_RS_COLLECTIONSET_INFO = 62 | 28,
        DPKUZ_QUEST_RS_NEW_QUEST_INFO = 62 | 29,
        DPKUZ_QUEST_RS_APPEAR_MARK_BY_PRAYER = 62 | 31,
        DPKUZ_QUEST_RS_PLAY_SOUND_EFFECT = 62 | 33,
        DPKUZ_QUEST_RS_CLEAR_TEXT = 62 | 34,
        DPKUZ_QUEST_RS_ERROR = 62 | 255,

        // 66 - Storage
         DPKUZ_STORAGE_RS_UNK= 66 | 1 << 8, 

        // 67 Admin CMD
        SPKUL_ADMIN_RQ_UNK = 67 | 1 << 8,
        SPKUL_ADMIN_RS_UNK = 67 | 2 << 8, // 31

        // 94 - Channel
        DPKUZ_CHANNEL_RQ_UNKOWN = 94 | 21 << 8,
        DPKUZ_CHANNEL_RS_UNKOWN = 94 | 22 << 8,


        // 96 -  Chatroom
        DPKUZ_ONChatRoomMsg_RS = 96 |11 << 8,


       

    }
}