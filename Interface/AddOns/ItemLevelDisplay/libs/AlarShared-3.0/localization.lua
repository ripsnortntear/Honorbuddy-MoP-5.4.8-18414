local __FILE__=tostring(debugstack(1,2,0):match("(.*):1:")) -- MUST BE LINE 1
local MAJOR_VERSION = ("AlarShared-3.0.localization")
local MINOR_VERSION = 500 + tonumber(string.sub("$Revision: 48 $", 12, -3))
local lib,old=LibStub:NewLibrary(MAJOR_VERSION,MINOR_VERSION)
if not lib then return end
local me, ns = ...
--[===[@debug@
print("Loading",__FILE__," inside ",me)
--@end-debug@]===]
local l=LibStub("AceLocale-3.0")
local function getLocale(lang,default)
	local me="AlarShared"
	local L=l:GetLocale(me,lang)
	if (L) then
		L=l:NewLocale(me)
	else
		L=l:NewLocale(me,lang,default,default)
	end
	return L
end
local L=getLocale("enUS",true)
--@localization(locale="enUS", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
L=getLocale("frFR")
if (L) then
--@localization(locale="frFR", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end
L=getLocale("deDE")
if (L) then
--@localization(locale="deDE", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end
L=getLocale("koKR")
if (L) then
--@localization(locale="koKR", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end
L=getLocale("esMX")
if (L) then
--@localization(locale="esMX", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end
L=getLocale("ruRU")
if (L) then
--@localization(locale="ruRU", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end
L=getLocale("zhCN")
if (L) then
--@localization(locale="zhCN", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end
L=getLocale("esES")
if (L) then
--@localization(locale="esES", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end
L=getLocale("zhTW")
if (L) then
--@localization(locale="zhTW", format="lua_additive_table", namespace="library",  escape-non-ascii=true, same-key-is-true=true, handle-unlocalized="english" )@
end