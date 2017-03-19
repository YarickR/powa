 -- other constants
 -- should be synchronised with C# code

FIGHT_STATE = {
    LOBBY       = 1,
    PREPARING   = 2,
    READY       = 3,
    RUNNING     = 4,
    FINISHED    = 5
}

CMDS = {
    START_FIGHT     = 1,
    END_FIGHT       = 2,
    VEHICLE_POS     = 3,
    VEHICLE_MOVE    = 4,
    VEHICLE_SHOOT   = 5,
    SETUP_FIELD     = 6,
    PASS_MOVE       = 7,
    END_TURN        = 8,
    NEXT_TURN       = 9
}


VEHICLE_TYPE = {
    VType_Invalid       = 0,
    VType_Light         = 1,
    VType_LightRanged   = 2,
    VType_Medium        = 3,
    VType_MediumRanged  = 4
}

PConst = {
    Side_Invalid            = 0,
    Side_User               = 1,
    Side_Enemy              = 2,
    VType_Invalid           = 0, -- Vehicle type
    VType_Light             = 1,
    VType_LightRanged       = 2,
    VType_Medium            = 3,
    VType_MediumRanged      = 4,
    TType_Base              = 0, -- Terrain type
    TType_Desert            = 1,
    TType_Dirt              = 2,
    TType_Forest            = 3,
    TType_Hill              = 4,
    TType_Marsh             = 5,
    TType_Mountain          = 6,
    TType_Plain             = 7,
    TType_Void              = 8,
    TType_Water             = 9,
    BType_Howitzer          = 1,
    BType_Cannon            = 2,
    Max_Players             = 2,
    Map_Size               = 12,
    EOT_None                = 0,
    EOT_Soft                = 1,
    EOT_Hard                = 2,
    Move_Time               = 15,
    Server_URL              = "http://powa.ext.terrhq.ru"
}


