local __FILE__=tostring(debugstack(1,2,0):match("(.*):1:")) -- MUST BE LINE 1
local MAJOR_VERSION = ("AlarLoader-3.0.lua"):gsub(".lua","")
local MINOR_VERSION = 500 + tonumber(string.sub("$Revision: 48 $", 12, -3))
local me, ns = ...
local pp=print
--[[
Name: AlarLoader-3.0.lua
Revision: $Rev: 48 $
Author: Alar of Daggerspine
Email: alar@aspide.it
Website: http://www.curse.com
SVN: $HeadUrl:$
Description: Generic library
Dependencies: Ace3
License: LGPL v2.1
--]]
--[===[@debug@
print("Loading",__FILE__,"inside", me)
--@end-debug@]===]
if (LibDebug) then LibDebug() end
LoadAddOn("Blizzard_DebugTools")
local function debug (...) 
	--[===[@debug@
	print(...)
	--@end-debug@]===]
end
local print=_G.print
local notify=_G.print
local error=_G.error
local function dump() end
local _,_,_,toc=GetBuildInfo()
-- End prologue ---------------------------------------------
--[===[@debug@
local oprint=print
if (LibDebug) then LibDebug() end
local dump=print

if (_G.AlarDebug) then  AlarDebug() end
--@end-debug@]===]
local lib,old=LibStub:NewLibrary(MAJOR_VERSION,MINOR_VERSION)
if (not lib) then return end
local function xformat(fmt,...)
		if tostring(fmt):find("%%") and select('#', ...) >= 1 then
		-- Format string
		local success, text = pcall(string.format, fmt, ...)
		if success then
			return text
		end
	end
	return tostringall(fmt,...)
end
lib.debugs=lib.debugs or {}
local function GetChatFrame(chat)
	if (chat) then
		for i=1,10 do
			local frame=_G["ChatFrame" .. i]
			if (not frame) then break end
			if (frame.name==chat) then return frame end
		end
	end
	return DEFAULT_CHAT_FRAME
end
function lib:GetPrintFunctions(caller,skip)
	local result={}
	do
		local c=GetChatFrame
		local caller=tostring(caller) or '' 
		local skip=tonumber(skip) or 1
		local prefixp=caller .. ':|r|cff20ff20'
		local prefixp=caller .. ':|r|cff20ff20'
		local prefixd='|cffff1010DBG:' .. caller .. ':|r|cff00ff00'
		local prefixn=caller .. ':|r|cffff9900'
		local prefixe=caller .. '-Error:|r|cffff0000'
		local xformat=xformat
		local debugs=self.debugs
		local oprint = pp
		debugs[caller]=true
		function result.print(...) oprint(prefixp,xformat(select(skip,...))) end
		function result.dump(...)
			local desc,value=select(skip,...)
			if (type(value)=="nil")  then
				value=desc
				desc="Data dump"
			end
			if (debugs[caller]) then 
				oprint(prefixp ,desc)
				DevTools_Dump(value) 
				if (_G.DOVEDIAVOLOSTA) then
					oprint(tostring(debugstack(2,1,0)))
				end			
			end 
		end
		function result.debug(...) 
			if (debugs[caller]) then 
				c("ADebug"):AddMessage(strjoin(' ',date("%X"),prefixd,xformat(select(skip,...))))
				if (_G.DOVEDIAVOLOSTA) then
					c("ADebug"):AddMessage(tostring(debugstack(2,1,0)))
				end
			end 
		end
		function result.notify(...) oprint(prefixn,xformat(select(skip,...))) end
		function result.error(...) oprint(prefixe,xformat(select(skip,...))) end
		function result.debugEnable(...) if (select(skip,...)) then debugs[caller] = true else debugs[caller] =false end end
	end
	return result
end
function lib:CreateAddon(addon,force,...)
    local stub
    if type(force)~="boolean" then force = true end
    if (force) then
    	stub=LibStub("AlarAddon-3.0"):new(addon,...)
    else
    	stub=LibStub("AceAddon-3.0"):NewAddon(addon,...)
    end
    if (stub) then
    	self.debugs[addon]=false
    	do
    		local debugs=self.debugs
    		local addon=addon
    		function stub:EnableDebug(status)
    			debugs[addon]=status
    		end
    		
    	end
        return stub
    else 
    	error("Unable to create stub")
    end
end
function lib:SetDebug(addon,status)
	if (addon=='*') then
		for addon,_ in pairs(self.debugs) do
			self.debugs[addon]=status
		end
	else
		self.debugs[addon]=status
	end
end
function lib:GetDebug(addon)
	if (addon=='*') then
		self:ShowDebug()
	else
		return self.debugs[addon]
	end
end
function lib:ShowDebug()
	print("AlarShared debug status")
	for addon,status in pairs(self.debugs) do
		print(addon,"debug:",(status and "|cffff1010ON|r" or "|cff20ff20OFF|r"))
	end
end
