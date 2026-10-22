local __FILE__=tostring(debugstack(1,2,0):match("(.*):1:")) -- MUST BE LINE 1
local MAJOR_VERSION = ("AlarAddon-3.0.lua"):gsub(".lua","")
local MINOR_VERSION = 500 + tonumber(string.sub("$Revision: 48 $", 12, -3))
local pp=print
--[[
Name: AlarAddon-3.0.lua
Revision: $Rev: 48 $
Author: Alar of Daggerspine
Email: alar@aspide.it
Website: http://www.curse.com
SVN: $HeadUrl:$
Description: Generic library
Dependencies: Ace3
License: LGPL v2.1
--]]
local me, ns = ...
--[===[@debug@
print("Loading",__FILE__," inside ",me)
--@end-debug@]===]
if (LibDebug) then LibDebug() end
local function debug(...) 
--[===[@debug@
	print(...)
--@end-debug@]===]
end
local print=_G.print
local notify=_G.print
local error=_G.error
local function dump() end
local function debugEnable() end
if (LibStub("AlarLoader-3.0",true)) then
	local rc=LibStub("AlarLoader-3.0"):GetPrintFunctions(MAJOR_VERSION)
	print=rc.print
	--[===[@debug@
	debug=rc.debug
	dump=rc.dump
	--@end-debug@]===]
	notify=rc.notify
	error=rc.error
	debugEnable=rc.debugEnable
else
	debug("Missing AlarLoader-3.0")
end
local _,_,_,toc=GetBuildInfo()
if (not LibStub) then
    error("Couldn't find LibStub. Please reinstall " .. MAJOR_VERSION )
end
local lib,old=LibStub:NewLibrary(MAJOR_VERSION,MINOR_VERSION)
if (not lib) then
    debug("Already loaded a newer or equal version of " .. MAJOR_VERSION .. ' r' .. MINOR_VERSION)
    return -- Already loaded
end
if (old) then
    debug(format("Upgrading %s from %s to %s",MAJOR_VERSION,old,MINOR_VERSION))
end
debugEnable(false)
local L=LibStub("AceLocale-3.0"):GetLocale('AlarShared',true)
--[[ Standard prologue end --]]
debug(MAJOR_VERSION .. " from file " .. __FILE__)
debug("Loading " .. MAJOR_VERSION .. " r" .. MINOR_VERSION)


-- I hate having to do the same thing a lot of times... so here is my stub
-- It calls LibStub and returns an addon with all available ace3 embeddable
-- components mixed in
function lib:new(name,...)
    local mixins={}
    for i,k in  LibStub:IterateLibraries() do
        if (i:match("Ace%w*-3%.0") and k.Embed) then
            table.insert(mixins,i)
        end
    end
    table.insert(mixins,"AlarCore-3.0")
    for i=1,select('#',...) do
        table.insert(mixins,(select(i,...)))
    end
    return LibStub("AceAddon-3.0"):NewAddon(name,unpack(mixins))
    
end