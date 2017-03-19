log = require("log")
local function drop_space(spaceName)
    local ret = box.space[spaceName] ~= nil and box.space[spaceName]:drop() or false
end
local function bootstrap()
    log.info('players')
    drop_space('players')
    local players = box.schema.space.create('players')
    players:create_index('primary', {parts = {1, 'NUM'}})
    players:create_index('player_name', {parts = {2, 'STR'}})
    log.info('fights')
    drop_space('fights')
    local fights = box.schema.space.create('fights')
    fights:create_index('primary', {parts = {1, 'NUM'}})
    fights:create_index('fight_state', {parts = {2, 'NUM'}, unique = false})
    log.info('fight_players')
    drop_space('fight_players')
    local fight_players = box.schema.space.create('fight_players')
    fight_players:create_index('primary', {parts = {1, 'NUM'}})
    log.info('shards')
    drop_space('shards')
    local shards = box.schema.create_space('shards')
    shards:create_index('primary', {parts = {1, 'NUM'}})
    log.info('player_buffers')
    drop_space('player_buffers')
    local player_buffers = box.schema.create_space('player_buffers')
    player_buffers:create_index('primary', { parts = { 1, 'NUM', 2, 'NUM' }})
    log.info('player_msgids')
    drop_space('player_msgids')
    local player_msgids = box.schema.create_space('player_msgids')
    player_msgids:create_index('primary', { parts = { 1, 'NUM' }})
    log.info('fight_steps')
    drop_space('fight_steps')
    local fight_steps = box.schema.space.create('fight_steps')
    fight_steps:create_index('primary', { parts = { 1, 'NUM'}})
    fight_steps:create_index('fight_player', { parts = { 2, 'NUM', 3, 'NUM'} , unique = false })
    log.info('fight_field')
    drop_space('fight_fields')
    local fight_fields = box.schema.space.create('fight_fields')
    fight_fields:create_index('primary', { parts = { 1, 'NUM'}})
    log.info('vehicle_classes')
    drop_space('vehicle_classes')
    local vehicle_classes = box.schema.space.create('vehicle_classes')
    vehicle_classes:create_index('primary', { parts = { 3, 'NUM'}})
    log.info('player_vehicles')
    drop_space('player_vehicles')
    local player_vehicles = box.schema.space.create('player_vehicles')
    player_vehicles:create_index('primary', { parts = { 1, 'NUM'}}) -- id
    player_vehicles:create_index('player_id', { parts = { 2, 'NUM'}, unique = false}) -- player id
    log.info('vehicle_ids')
    drop_space('vehicle_ids')
    local vehicle_ids = box.schema.create_space('vehicle_ids')
    vehicle_ids:create_index('primary', {parts = {1, 'NUM'}})
    log.info('fight_vehicles')
    drop_space('fight_vehicles')
    local fight_vehicles = box.schema.space.create('fight_vehicles')
    fight_vehicles:create_index('primary', { parts = { 1, 'NUM' } })
end
local function fill_vehicle_classes() 
-- PConst.VType_Light: MaxTime= 10; MaxDamage = 2; MaxArmor = 4; Price = 2;    ShotTU = 2; MaxDistance = 4; break;
-- PConst.VType_LightRanged: MaxTime = 10;    MaxDamage = 2; MaxArmor = 4; Price = 2;    ShotTU = 2; MaxDistance = 4; break;
-- PConst.VType_Medium: MaxTime = 5; MaxDamage = 4; MaxArmor = 8; Price = 5; ShotTU = 3; MaxDistance = 6; break;
-- PConst.VType_MediumRanged: MaxTime = 5; MaxDamage = 8; MaxArmor = 10; Price = 8; ShotTU = 3; MaxDistance = 6; break;
   box.space.vehicle_classes:insert({   
        0,                          --  Id          =   1,
        0,                          --  PlayerId    =   2,
        VEHICLE_TYPE.VType_Light,   --  Type        =   3
        10, -- MaxTime     =   4,
        2,  -- MaxDamage   =   5,
        4,  -- MaxArmor    =   6,
        4,  -- MaxDistance =   7,
        0,  -- Time        =   8,
        0,  -- Damage      =   9,
        0,  -- Armor       =   10,
        2,  -- ShotTU      =   11,
        0,  -- Distance    =   12,
        2,  -- Price       =   13,
        0,  -- X           =   14,
        0,  -- Y           =   15
   })
box.space.vehicle_classes:insert({   
        0,                          --  Id          =   1,
        0,                          --  PlayerId    =   2,
        VEHICLE_TYPE.VType_LightRanged,   --  Type        =   3
        10, -- MaxTime     =   4,
        2,  -- MaxDamage   =   5,
        4,  -- MaxArmor    =   6,
        4,  -- MaxDistance =   7,
        0,  -- Time        =   8,
        0,  -- Damage      =   9,
        0,  -- Armor       =   10,
        2,  -- ShotTU      =   11,
        0,  -- Distance    =   12,
        2,  -- Price       =   13,
        0,  -- X           =   14,
        0,  -- Y           =   15
   })
box.space.vehicle_classes:insert({   
        0,                          --  Id          =   1,
        0,                          --  PlayerId    =   2,
        VEHICLE_TYPE.VType_Medium,   --  Type        =   3
        5,  -- MaxTime     =   4,
        4,  -- MaxDamage   =   5,
        8,  -- MaxArmor    =   6,
        6,  -- MaxDistance =   7,
        0,  -- Time        =   8,
        0,  -- Damage      =   9,
        0,  -- Armor       =   10,
        3,  -- ShotTU      =   11,
        0,  -- Distance    =   12,
        5,  -- Price       =   13,
        0,  -- X           =   14,
        0,  -- Y           =   15
   })
box.space.vehicle_classes:insert({   
        0,                          --  Id          =   1,
        0,                          --  PlayerId    =   2,
        VEHICLE_TYPE.VType_MediumRanged,   --  Type        =   3
        5,  -- MaxTime     =   4,
        8,  -- MaxDamage   =   5,
        10, -- MaxArmor    =   6,
        6,  -- MaxDistance =   7,
        0,  -- Time        =   8,
        0,  -- Damage      =   9,
        0,  -- Armor       =   10,
        3,  -- ShotTU      =   11,
        0,  -- Distance    =   12,
        8,  -- Price       =   13,
        0,  -- X           =   14,
        0,  -- Y           =   15
   })

end

local function init_counters()
    log.info("vehicle_ids")
    box.space.vehicle_ids:insert({0, 0})
end
log.info("Recreating schema")
bootstrap()
log.info("Filling vehicle data")
fill_vehicle_classes()
log.info("Initializing counters")
init_counters()
log.info("Finished")
