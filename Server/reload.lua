log=require("log")
function reload() 
    log.info("Reloading, current powa table: " )
    log.info(powa)    
    if powa then
        powa.deinit()
    end
    package.loaded["powa"] = nil
    powa = require("powa")
    log.info("Reloaded, new powa table: ")
    log.info(powa)
    fiber.create(powa.fightd_main)
    log.info("New main thread id: " .. MAIN_PID .. ", new fightd thread id : " .. FIGHTD_PID)
end
reload()
