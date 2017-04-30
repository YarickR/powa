-- field constants
Fight =  {  
-- fields for fights space
    Id          = 1,
    State       = 2,
    Ctime       = 3,
    Mtime       = 4,
    ActivePlayer= 5,
    Timeouts    = 6,
    EndTurn     = 7
}

Player = { 
-- fields for players space
    Id          = 1,
    Name        = 2,
    Ctime       = 3,
    FightId     = 4,
    Wins        = 5,
    Losses      = 6,
    Mtime       = 7,
    Money       = 8,
    EndTurn     = 9
}

FightPlayer = { 
-- fields for fight_players space
    Id          = 1 -- == Fight.Id
}

PlayerBuffer =  { 
-- fields for player_buffers space
    PlayerId    = 1,
    Id          = 2,
    Ctime       = 3,
    Data        = 4
}

FightStep =  { 
-- fields for fight_steps space
    Id          = 1,
    FightId     = 2,
    PlayerId    = 3,
    Ctime       = 4,
    Data        = 5
}


FightField = {
    Id      =   1,
    Field   =   2
}

Vehicle = {
    Id          =   1,
    PlayerId    =   2,
    Type        =   3,
    MaxTime     =   4,
    MaxDamage   =   5,
    MaxArmor    =   6,
    MaxDistance =   7,
    Time        =   8,
    Damage      =   9,
    Armor       =   10,
    ShotTU      =   11,
    Distance    =   12,
    Price       =   13,
    X           =   14,
    Y           =   15
}

VehicleId = {
    PK      = 1,
    Id      = 2
}

PlayerMsgId = {
    PlayerId    = 1,
    MsgId       = 2
}

FightResults = {
    FightId     = 1,
    PlayerId    = 2,
    Place       = 3,
    Kills       = 4,
    Deaths      = 5, 
    LastKill    = 6,
    LastDeath   = 7
}

