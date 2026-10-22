--[[

	Want to help translate this addon?

	Head over to curseforge and submit your translations using the localization page:

	http://wow.curseforge.com/addons/taxitimer/localization/

]]

local addonName, ns = ...

ns.L = {}

ns.L["DEBUG_TEXT"] = "TaxiMap issue! Report this message to the author:"
ns.L["FLIGHT_TIME"] = "flight time"
ns.L["CHAT_TEXT"] = "TaxiTimer estimates that it takes |cffFFFFFF%s|r to fly from |cffFFFFFF%s|r to |cffFFFFFF%s|r and that the price for the whole flight is |cffFFFFFF%s|r."

local locale = GetLocale()

if locale == "deDE" then

elseif locale == "esES" then

elseif locale == "esMX" then

elseif locale == "frFR" then
ns.L["Acherus: The Ebon Hold"] = "Achérus : le fort d'Ébène" -- Needs review

elseif locale == "itIT" then

elseif locale == "koKR" then

elseif locale == "ptBR" then

elseif locale == "ruRU" then

elseif locale == "zhCN" then

elseif locale == "zhTW" then

end
