json = require("json")
log = require("log")
clock = require("clock")
fiber = require("fiber")
msgpack = require('msgpack')
b64 = require("b64")
require("const")
require("fields")

FIGHTD_PID = 0
MAIN_PID = 0
PLAYER_CHANNELS = {
}

function log.sinfo( str ) 
    local __i = debug.getinfo(2);
    log.info(__i.short_src .. ":" .. __i.currentline .. " - " .. str)
end

function table.val_to_str ( v )
  if "string" == type( v ) then
    v = string.gsub( v, "\n", "\\n" )
    if string.match( string.gsub(v,"[^'\"]",""), '^"+$' ) then
      return "'" .. v .. "'"
    end
    return '"' .. string.gsub(v,'"', '\\"' ) .. '"'
  else
    return "table" == type( v ) and table.tostring( v ) or
      tostring( v )
  end
end

function table.key_to_str ( k )
  if "string" == type( k ) and string.match( k, "^[_%a][_%a%d]*$" ) then
    return k
  else
    return "[" .. table.val_to_str( k ) .. "]"
  end
end

function table.tostring( tbl )
  local result, done = {}, {}
  for k, v in ipairs( tbl ) do
    table.insert( result, table.val_to_str( v ) )
    done[ k ] = true
  end
  for k, v in pairs( tbl ) do
    if not done[ k ] then
      table.insert( result,
        table.key_to_str( k ) .. "=" .. table.val_to_str( v ) )
    end
  end
  return "{" .. table.concat( result, "," ) .. "}"
end


function table.contains(tbl, e) 
    local __i, __e
    for __i, __e in ipairs(tbl) do
        if __e and e == __e then
            return true
        end
    end
    return false
end

function table.clone (t) -- deep-copy a table
    local target = {}
    for k, v in pairs(t) do
        if type(v) == "table" then
            target[k] = clone(v)
        else
            target[k] = v
        end
    end
    return target
end

function table.copy (t) -- shallow-copy a table
    local target = {}
    for k, v in pairs(t) do target[k] = v end
    return target
end

local function encodeURI(str)
    if (str) then
        str = string.gsub (str, "\n", "\r\n")
        str = string.gsub (str, "([^%w ])",
            function (c) return string.format ("%%%02X", string.byte(c)) end)
        str = string.gsub (str, " ", "+")
   end
   return str
end

local function decodeURI(s)
    if(s) then
        s = string.gsub(s, "+", " ");
        s = string.gsub(s, '%%(%x%x)', function (hex) return string.char(tonumber(hex,16)) end )
    end
    return s
end

local function array_to_obj(a, ft)  -- a - array, ft - table with field ids. Somewhat expensive ( O(N*M) ), need to check luafun
    local __ret = {}
    for __ai, __av in ipairs(a) do
        for __fi, __fv in pairs(ft) do
            if __ai == __fv then
                __ret[__fi] = __av
                break
            end
        end
    end
    return __ret
end

local function obj_to_array(o, ft) -- reverse to array_to_obj
    local __ret = {}
    for __oi, __ov in pairs(o) do
        if ft[__oi] then
            __ret[ft[__oi]] = __ov
        end
   end 
end

local function queue_to_player(playerId, data) 
    local __m = msgpack.encode(data)
    local __pmt = box.space.player_msgids:update({playerId}, {{'+', PlayerMsgId.MsgId, 1}})
    local __msgId = __pmt[PlayerMsgId.MsgId] 
    local __r = box.space.player_buffers:insert({playerId, __msgId, math.floor(os.time()), __m})
    return __msgId
end

local function wakeup_player(playerId, msgId)
    local __c = PLAYER_CHANNELS[playerId]
    if __c and __c:has_readers() then
        __c:put(msgId) -- waking up
    end
end

local function get_player_id(args)
    args = args["args"]
    if (args["name"] == nil) then 
        return {}
    end
--    Id          = 1,
--    Name        = 2,
--    Ctime       = 3,
--    FightId     = 4,
--    Wins        = 5,
--    Losses      = 6,
--    Mtime       = 7,
--    Money       = 8,
--    EndTurn     = 9
    local __p = box.space.players:auto_increment({args["name"], math.floor(clock.time()), 0, 0, 0, math.floor(clock.time()), 50, 0 })
    box.space.player_msgids:insert({__p[Player.Id], 0})
    local __vc = box.space.vehicle_classes:select({})   
    for _, __v in pairs(__vc) do
        local __vi = box.space.vehicle_ids:update({0}, {{'+', VehicleId.Id, 1}})
        local __pv = table.copy(__v)
        __pv[Vehicle.Id] = __vi[VehicleId.Id]
        __pv[Vehicle.PlayerId] = __p[Player.Id] 
        box.space.player_vehicles:insert(__pv)
    end
    return { ['id'] = __p[Player.Id] }
end


local function get_player_id_p(args) 
    local __status, __ret = pcall(get_player_id, args)
    if __status then return __ret end
    log.info("Error calling get_player_id: " .. __ret )
    return {}
end

local function get_fight_list(args)
    args = args["args"]
    local __fightList = box.space.fights.index.fight_state:select({FIGHT_STATE.PREPARING}, {iterator = 'EQ'})
    local __ret = {}
    local __m, __fight
    for __m, __fight in pairs(__fightList)  do
        local __fightPlayers = box.space.fight_players:get({__fight[Fight.Id]})
        local __p, __playerId, __playerNames
        __playerNames = {}
        for __p, __playerId in pairs(__fightPlayers) do
            if __p > 1 then 
                local  __player = box.space.players:get({__playerId})
                table.insert(__playerNames, __player and __player[Player.Name] or __playerId)
            end
        end
        table.insert(__ret, { id = __fight[Fight.Id],  ctime = __fight[Fight.Ctime], players = __playerNames })
    end
    log.sinfo(table.tostring(__ret));
    return { fights = __ret };
end


local function get_fight_list_p(args) 
    local __status, __ret = pcall(get_fight_list, args)
    if __status then return __ret end
    log.info("Error calling get_fight_list: " .. __ret )
    return {}
end

local function create_fight(args) 
    args = args["args"]
    if (args["id"] == nil) or (tonumber(args["id"] == 0)) then 
        return {}
    end
--    Id          = 1,
--    State       = 2,
--    Ctime       = 3,
--    Mtime       = 4,
--    ActivePlayer= 5,
--    Timeouts    = 6,
--    EndTurn     = 7
    local __t = box.space.fights:auto_increment( {FIGHT_STATE.PREPARING, math.floor(clock.time()), math.floor(clock.time()), 0, 0, 0, 0} ); 
    box.space.fight_players:insert({__t[Fight.Id]})  -- t[1] - new fight id 
    return { id = __t[Fight.Id] };
end

local function end_fight(fightId, winners, losers)
    box.space.fights:update(fightId, {{'=', Fight.State, FIGHT_STATE.FINISHED}, {'=', Fight.Mtime, math.floor(clock.time()) }})
    local __fightPlayers = box.space.fight_players:get({fightId})
    if __fightPlayers then
        local __i, __playerId 
        local __data = { ["cmd"] = CMDS.END_FIGHT } 
        for __i = 2, #__fightPlayers do  
            __playerId = __fightPlayers[__i]
            local __w = table.contains(winners, __playerId) and 1 or 0
            local __l = table.contains(losers, __playerId) and 1 or 0
            box.space.players:update(__playerId, {{'+', Player.Wins, __w}, {'+', Player.Losses, __l}, {'=', Player.FightId, 0}})
            local __msgId = queue_to_player(__playerId,  __data)
            wakeup_player(__playerId, __msgId);
        end
    end
end

local function leave_fight(fightId, playerId) 
    local __fightPlayers = box.space.fight_players:get({fightId})
    if __fightPlayers then
        local __i, __playerId 
        for __i, __playerId in pairs(__fightPlayers) do
            if __i > 1  then
                if __playerId == playerId then -- hId, fight[1]...n - playerIds
                    local __fight = box.space.fights:get({fightId})
                    if  __fight then
                        if __fight[Fight.State] == FIGHT_STATE.RUNNING then -- leaving an already running fight
                            end_fight(fightId, {} , { playerId })
                        elseif __fight[Fight.State] == FIGHT_STATE.READY then
                            box.space.fights:update(fightId, {{'=', Fight.State, FIGHT_STATE.PREPARING}})
                            box.space.fight_players:update( fightId, {{ '#', __i, 1}})
                        elseif __fight[Fight.State] == FIGHT_STATE.PREPARING then
                            box.space.fight_players:update( fightId, {{ '#', __i, 1}})
                        end
                    end
                    break
                end
            end
        end
    end
    box.space.players:update( playerId, {{ '=', Player.FightId, 0}} ) -- player[4] - fight id 
    return { id = 0 }
end

local function join_fight(args) 
    args = args["args"]
    local __playerId = args["player_id"] and tonumber(args["player_id"]) or 0
    local __fightId = args["fight_id"] and tonumber(args["fight_id"]) or 0
    if __playerId == 0 or __fightId == 0 then 
        return { id = 0 }
    end
    local __player = box.space.players:get({__playerId})
    local __fight = box.space.fights:get({__fightId})
    if not __player or not __fight then
        return { id = 0 }
    end
    if __player[Player.FightId] > 0 then
        leave_fight(__player[Player.FightId], __playerId)
    end
    box.space.players:update(__playerId, {{'=', Player.FightId, __fightId}})
    box.space.fight_players:update(__fightId, {{'!', -1, __playerId}})
    local __fightPlayers = box.space.fight_players:get({__fightId})
    if #__fightPlayers > 2 then
        box.space.fights:update(__fightId, {{'=',Fight.State, FIGHT_STATE.READY}, {'=', Fight.Mtime, math.floor(clock.time()) }})
    end
    return { id = __fightId };
end

local function find_free_cell(field, playerPos) 
    local __minX = 1 
    local __maxX = 2 
    local __minY = 3
    local __maxY = 4
    local __bounds = { { 1, 5, 1, 5}, { -5, -1, -5, -1 }, {1, 5, -5, -1}, {-5, -1, 1, 5} }
    local __bo = __bounds[playerPos + 1]
    for __b = __minX, __maxY do
        __bo[__b] = __bo[__b] > 0 and __bo[__b] or PConst.Map_Size + __bo[__b];  
    end
    local __x = 0
    local __y = 0
    repeat
        __x = math.random(__bo[__minX], __bo[__maxX]);
        __y = math.random(__bo[__minY], __bo[__maxY]);
        if not field[__y] or not field[__y][__x] then log.sinfo(table.tostring(field)) end
        if field[__y][__x]['v'] == 0 then 
            if (((field[__y][__x]['t'] >= PConst.TType_Desert) and (field[__y][__x]['t'] <= PConst.TType_Marsh)) or 
                 (field[__y][__x]['t'] == PConst.TType_Plain)) then
                return __x, __y
            end
        end   
    until false 
    return -1, -1
end

local function generate_field()
    local __f = {}
    local __h = 0
    local __w = 0
    local __t = 0
    for  __h = 0, PConst.Map_Size - 1 do 
        __f[__h] = {}
        for __w = 0, PConst.Map_Size - 1 do
            local __t
            if      math.random(0, 100) < 30 then
                __t = math.random(PConst.TType_Desert, PConst.TType_Marsh + 1)
            elseif  math.random(0, 100) < 20 then 
                __t = PConst.TType_Mountain
            else
                __t = PConst.TType_Plain
            end
           __f[__h][__w] = { ['t'] = __t, ['v'] = 0 }
        end
    end
    return __f
end

local function field_to_string(f)
    local __fs = "" 
    local __h
    local __w
    local __r
    local __c -- height, width, row, cell
    for __h, __r in pairs(f) do 
        for __w, __c in pairs(__r) do
            __fs = __fs .. string.format("%02X", __c['t'])
        end
    end
    return __fs
end

local function setup_fight(fight, players)
    local __f, __ret, __p, __playerId
    log.sinfo("Setting up new fight")
    __f = generate_field()
    box.space.fight_fields:insert({fight[Fight.Id], msgpack.encode(__f)})
    box.space.fight_vehicles:insert({fight[Fight.Id]})
    local __playerPos = 0;
    for __p = 2, #players do
        __playerId = players[__p];
        local  __plr = box.space.players:get({__playerId})
        if __plr then
            local  __pv = box.space.player_vehicles.index.player_id:select({__playerId}, {iterator= box.index.EQ})
            for _, __v in pairs(__pv) do
                local __x, __y = find_free_cell(__f, __playerPos);
                if __x ~= -1 and __y ~= -1 then
                    __f[__y][ __x]['v'] = __v[Vehicle.Type]
                    box.space.player_vehicles:update(__v[Vehicle.Id], {{'=', Vehicle.X, __x}, {'=', Vehicle.Y, __y}, {'=', Vehicle.Armor, __v[Vehicle.MaxArmor]}})
                    box.space.fight_vehicles:update(fight[Fight.Id], {{'!', -1, __v[Vehicle.Id]}});
                else
                    log.warn("Cannot find free cell for vehicle #" .. __v[Vehicle.Id] .. " for player at position " .. __playerPos)
                end
            end
            __playerPos = __playerPos  + 1
        end
    end
    box.space.fights:update(fight[Fight.Id], {{'=', Fight.State, FIGHT_STATE.RUNNING}, {'=', Fight.Mtime, math.floor(clock.time()) }})
    for __p = 2, #players do
        __playerId = players[__p]
        local __msgId = queue_to_player(__playerId,  { ["cmd"] = CMDS.START_FIGHT, ["fight_id"] = fight[Fight.Id] })
        wakeup_player(__playerId, __msgId);
    end
end

local function get_fight_data(args)
    args = args["args"]
    local __playerId = args["player_id"] and tonumber(args["player_id"]) or 0
    if __playerId == 0 then 
        return { id = 0 }
    end
    local __player = box.space.players:get({__playerId})
    local __fightId = __player and __player[Player.FightId] or 0
    local __fight = __fightId > 0 and box.space.fights:get({__fightId}) or nil
    if not __player or not __fight then
        return 0
    end
    local __players = box.space.fight_players:get(__fightId)
    local __f = box.space.fight_fields:get(__fightId)
    local __fs = field_to_string(msgpack.decode(__f[FightField.Field]))
    local __ret = { ['field'] = __fs,  ['size'] = PConst.Map_Size,  ['players'] = {}, ['active_player'] = __fight[Fight.ActivePlayer] }
    local __playerPos = 0;
    local __players = box.space.fight_players:get({__fightId}) 
    for __p = 2, #__players do
        local  __plr = box.space.players:get({__players[__p]})
        if __plr then
            table.insert(__ret["players"], { 
                ["id"] = __plr[Player.Id],
                ["name"] = __plr[Player.Name] or "Unnamed Player",
                ["money"] = __plr[Player.Money] or 50,
                ["pos"] = __playerPos
            })
            __playerPos = __playerPos  + 1
        end
    end
    return __ret;
end

local function get_fight_vehicles(args)
    args = args["args"]
    local __playerId = args["player_id"] and tonumber(args["player_id"]) or 0
    if __playerId == 0 then 
        return { id = 0 }
    end
    local __player = box.space.players:get({__playerId})
    local __fightId = __player and __player[Player.FightId] or 0
    local __fightVehicles = __fightId > 0 and box.space.fight_vehicles:get({__fightId}) or nil
    if not __player or not __fightVehicles then
        return 0
    end
    local __ret = {}
    for __i = 2, #__fightVehicles do
        local __vhId = __fightVehicles[__i]
        local __vh = box.space.player_vehicles:get({__vhId});
        local __vho = array_to_obj(__vh, Vehicle)
        table.insert(__ret, __vho)
    end
    return __ret;
end

local function next_turn(fight, fightPlayers)
    box.space.fights:update(fight[Fight.Id], {{'=', Fight.Mtime, math.floor(clock.time())},{'=', Fight.ActivePlayer, 0}, {'=', Fight.EndTurn, 0}})
    local __data = { ["cmd"] = CMDS.NEXT_TURN, ["active_player"] = 0 } 
    for __i = 2, #fightPlayers do
        box.space.players:update(fightPlayers[__i], {{'=', Player.EndTurn, 0}})
        local __msgId = queue_to_player(fightPlayers[__i], __data)
        wakeup_player(fightPlayers[__i], __msgId)
    end
end

local function pass_the_move(fight) 
    log.sinfo("Fight #".. fight[Fight.Id] .. " passing the move ")
    local __fightPlayers = fight and box.space.fight_players:get({fight[Fight.Id]}) or nil
    if not __fightPlayers then
        return 0
    end
    local __endTurnUsers = 0;
    local __active = fight[Fight.ActivePlayer]
    local __totalPlayers = #__fightPlayers - 1
    repeat
        __active = (__active + 1) % __totalPlayers
        local __player = box.space.players:get({__fightPlayers[__active + 2]})
        log.sinfo("Total players " .. __totalPlayers .. " active " ..  __active .. " Player #" .. __player[Player.Id] .. " Player.EndTurn " .. __player[Player.EndTurn] .. " EndTurnUsers " .. __endTurnUsers)
        if __player and (__player[Player.EndTurn] == 0) then
            break
        end
        __endTurnUsers = __endTurnUsers +  1
    until __endTurnUsers >= __totalPlayers
    if __endTurnUsers == __totalPlayers then
        next_turn(fight, __fightPlayers)
        return 0
    end
    box.space.fights:update(fight[Fight.Id], {{'=', Fight.Mtime, math.floor(clock.time())}, {'=', Fight.ActivePlayer, __active}})
    local __data = { ["cmd"] = CMDS.PASS_MOVE, ["active_player"] = __active }
    for __i = 2, #__fightPlayers do
      local __msgId = queue_to_player(__fightPlayers[__i], __data)
      wakeup_player(__fightPlayers[__i], __msgId)
    end
end

local function poll(args)
    args = args["args"]
    local __playerId, __lastMsgId
    __playerId = args["player_id"] and tonumber(args["player_id"]) or nil
    __lastMsgId  = args["last_msg_id"] and tonumber(args["last_msg_id"]) or nil
    if not __playerId or not __lastMsgId then
        return {}
    end
    local __cnt = 0;
    box.space.players:update(__playerId, {{'=', Player.Mtime, math.floor(clock.time())}})
    log.sinfo('Player #' .. __playerId  .. " polls the server")
    repeat
        local __msgs = {}
        for __i, __msg in box.space.player_buffers.index.primary:pairs(__playerId, {iterator = box.index.EQ}) do
            if __msg[PlayerBuffer.Id] > __lastMsgId then
                local __umsg = { ["msg_id"] = __msg[PlayerBuffer.Id], ["ctime"] = __msg[PlayerBuffer.Ctime], ["data"] = msgpack.decode(__msg[PlayerBuffer.Data]) }
                table.insert(__msgs, __umsg);
            end
        end
        if #__msgs > 0 then
            log.sinfo("Returning " .. #__msgs .. " message(s) to player #" .. __playerId .. ", " ..table.tostring(__msgs))
            return __msgs
        end
        local __c = PLAYER_CHANNELS[__playerId]
        local __w = nil
        if not __c then
            __c = fiber.channel()
            PLAYER_CHANNELS[__playerId] = __c
        end
        __w = __c:get(10)
        __cnt = __cnt + (__w and 1 or 2) -- exiting without rereading message buffer on timeout 
    until __cnt > 1
    log.sinfo("Breaking long poll for player #" .. __playerId )
    return {}
end

local function poll_p(args) 
  local __status, __ret = pcall(poll, args)
    if __status then return __ret end
    log.info("Error calling poll: " .. __ret )
    return {}
end

local function fight_cmd(args)
    args = args["args"]
    local __playerId, __cmdId, __data
    __playerId = args["player_id"] and tonumber(args["player_id"]) or 0
    __cmdId  = args["cmd_id"] and tonumber(args["cmd_id"]) or 0
    __data = args["data"] and decodeURI(args["data"]) or ""
    if (__playerId == 0) or (__cmdId == 0) or (__data == "") then
        log.sinfo("bad fight command arguments: player id:*" .. __playerId .. "*, cmd id:*" .. __cmdId .. "*, data: *"..__data.."*")  
        return 0
    end
    log.sinfo("data received from player #" .. __playerId .. " : " .. __data)
    __data = json.decode(__data)
    local __player = box.space.players:get({__playerId})
    if not __player or (__player[Player.FightId] == 0) then
        log.sinfo("player not found or not in the fight ")
        return 0
    end
    local __fightId = __player[Player.FightId];
    local __fightPlayers  = box.space.fight_players:get({__fightId})
    local __fight = box.space.fights:get({__fightId})
    local __playerPos = -1
    for __i = 2, #__fightPlayers do
        if __fightPlayers[__i] == __playerId then
            __playerPos = __i - 2
            break
        end    
    end
    if __playerPos ~= __fight[Fight.ActivePlayer] then
        if __data["cmd"] ~= CMDS.END_TURN then
            log.sinfo("Not his move (" .. __playerPos .. " vs " .. __fight[Fight.ActivePlayer] .. ") , though")
            return 0
        end
    end
    -- ok, player is alive and sending something
    box.space.fights:update(__fightId, {{'=', Fight.Timeouts, 0}})
    box.space.players:update(__playerId, {{'=', Player.EndTurn, 0}})
    for __i = 2, #__fightPlayers do
        if __fightPlayers[__i] ~= __playerId then
            local __msgId = queue_to_player(__fightPlayers[__i], __data)
            wakeup_player(__fightPlayers[__i], __msgId)
        end
    end
    if __data["cmd"] == CMDS.END_TURN then
        box.space.players:update(__playerId, {{'=', Player.EndTurn, __data["state"] or 0}})
        if __data["state"] ~= 0 then
            pass_the_move(__fight)    
        end
    else
        pass_the_move(__fight)
    end    
    box.space.fight_steps:auto_increment({__fightId, __playerId, math.floor(clock.time()), msgpack.encode(__data)})
    return __cmdId
end


local function fight_cmd_p(args)
  local __status, __ret = pcall(fight_cmd, args)
    if __status then return __ret end
    log.info("Error calling fight_cmd: " .. __ret )
    return {}
end

local function fight_player_timeout(fight, fightPlayers, playerPos)
    local __playerId = fightPlayers[playerPos + 2] -- first element on fightPlayers is fightId
    local __player = box.space.players:get({__playerId})
    if not __player then return 0 end
    box.space.players:update(__playerId, {{'=', Player.EndTurn, PConst.EOT_Soft }})
    box.space.fights:update(fight[Fight.Id], {{'+', Fight.Timeouts, 1}})
    if fight[Fight.Timeouts] >= ((#fightPlayers) - 1) * 3 then -- three full timeouts
        return 1
    end
    local __data = { ["cmd"] = CMDS.END_TURN, ["player"] = __playerId, ["state"] = PConst.EOT_Soft }
    for __i = 2, #fightPlayers do
        local __msgId = queue_to_player(fightPlayers[__i], __data)
    end
    pass_the_move(fight)
    return 0
end

local function next_turn(fight, fightPlayers)
    box.space.fights:update(fight[Fight.Id], {{'=', Fight.Mtime, math.floor(clock.time())},{'=', Fight.ActivePlayer, 0}, {'=', Fight.EndTurn, 0}})
    
    local __data = { ["cmd"] = CMDS.NEXT_TURN, ["active_player"] = 0 } 
    for __i = 2, #fightPlayers do
        box.space.players:update(fightPlayers[__i], {{'=', Player.EndTurn, 0}})
        local __msgId = queue_to_player(fightPlayers[__i], __data)
        wakeup_player(fightPlayers[__i], __msgId)
    end

end

local function fightd_main()
    FIGHTD_PID = fiber.self():id()
   -- main fightd loop goes here
    log.sinfo("FightD fiber started with PID " .. FIGHTD_PID)
    while true do
        local __readyFights = box.space.fights.index.fight_state:select({FIGHT_STATE.READY}, { iterator = 'EQ'})
        if #__readyFights > 0 then
            log.sinfo("We have " .. #__readyFights .. " ready fight(es)")
        end
        local __fight
        for _, __fight in pairs(__readyFights) do
            local __fightPlayers = box.space.fight_players:get({__fight[Fight.Id]})
            setup_fight(__fight, __fightPlayers)
        end
        local __currTS = math.floor(clock.time())
        for _, __fight in box.space.fights.index.fight_state:pairs(FIGHT_STATE.RUNNING, {iterator = 'EQ'}) do
          if ((__currTS - __fight[Fight.Mtime]) >= PConst.Move_Time) then
            -- 30 seconds to pass the move
            local __fightPlayers = box.space.fight_players:get({__fight[Fight.Id]})
            local __activePlayer = __fight[Fight.ActivePlayer]
            log.sinfo("Fight " .. __fight[Fight.Id] .. " player " .. __fight[Fight.ActivePlayer] .. "(#".. __fightPlayers[__activePlayer + 2] .. ") times out")
            
            if  fight_player_timeout(__fight, __fightPlayers, __fight[Fight.ActivePlayer]) == 1 then
                log.sinfo("Finishing fight")
                end_fight(__fight[Fight.Id], {}, {}) -- no winners, no losers in timeout fight 
            end
          end
        end
        fiber.sleep(0.1)
        fiber.testcancel()
    end
end

local function deinit()
    if FIGHTD_PID and (FIGHTD_PID > 0) then
        local __fightdFiber = fiber.find(FIGHTD_PID)
        if __fightdFiber ~= nil  then
            log.sinfo("Stopping thread " .. __fightdFiber:id() )
            local __status, __err = pcall(__fightdFiber:cancel())
            if not __status then
                log.sinfo("Error '".. __err .. "' stopping fightd thread ")
            else 
                log.sinfo("Successfully stopped fightd thread")
            end
        else 
            log.sinfo("Can't find fightd fiber #" .. FIGHTD_PID)
        end
    end
    for __i, __c in ipairs(PLAYER_CHANNELS) do
        log.sinfo("Closing another channel")
        __c:close()
    end
    log.sinfo("Deinit complete")
end

MAIN_PID = fiber.self():id()

return {
    get_player_id       = get_player_id_p,
    get_fight_list      = get_fight_list_p,
    create_fight        = create_fight,
    join_fight          = join_fight,
    get_fight_data      = get_fight_data,
    get_fight_vehicles  = get_fight_vehicles,
    fightd_main         = fightd_main,
    poll                = poll_p,
    fight_cmd           = fight_cmd_p,
    deinit              = deinit
}
