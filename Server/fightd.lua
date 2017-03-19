fightd_pid = 0
fiber = require("fiber")
local function fightd_main
    fightd_pid = fiber.self():id();

   -- main fightd loop goes here
    while true  do
        local __readyMatches = box.space.matches:select({Match.State, MATCH_STATE.READY})
        local __i, __match
        for __i, __match in pairs(__readyMatches) do
            setup_match(__match)
            start_match(__match)
        end
        __readyMatches = box.space.matches:select({Match.State, MATCH_STATE.RUNNING})
        local __currTS = clock.time()
        for __i, __match in pairs(__readyMatches) do
            if __currTS - __match[Match.Mtime] > 30 then
                
            end
        end
        fiber.sleep(0.5)
        fiber.testcancel()
    end
end

fiber.create(fightd_main)
