local InFlight = CreateFrame("Frame", "InFlight")  -- no parent is intentional
local self = InFlight
InFlight:SetScript("OnEvent", function(this, event, ...) this[event](this, ...) end)
InFlight:RegisterEvent("ADDON_LOADED")

local gl = GetLocale()
local HandyNotes, Cartographer_Notes = HandyNotes, Cartographer_Notes
local UpdateTaxiIcons

local function LoadInFlight()
	if not InFlight.ShowOptions then
		LoadAddOn("InFlight")
	end
	return (InFlight.ShowOptions and true) or nil
end
----------------------------------
function InFlight:ADDON_LOADED(a1)
----------------------------------
	if a1 == "InFlight_Load" then
		self:RegisterEvent("TAXIMAP_OPENED")
		self:RegisterEvent("PLAYER_CONTROL_LOST")
		self:RegisterEvent("PLAYER_ENTERING_WORLD")
		if self.SetupInFlight then
			self:SetupInFlight()
		else
			self:UnregisterEvent("ADDON_LOADED")
		end
	elseif a1 == "InFlight" then
		self:UnregisterEvent("ADDON_LOADED")
		self:LoadBulk()
	end
end
-----------------------------------------
function InFlight:TAXIMAP_OPENED(_, misc)
-----------------------------------------
	if LoadInFlight() and not misc then
		self:InitSource()
	end
end
-----------------------------------------
function InFlight:PLAYER_CONTROL_LOST()
-----------------------------------------
	if LoadInFlight() then
		if UnitOnTaxi("player") then
			if self.StartTaxiFlight then
				self:StartTaxiFlight()
			end
		elseif self.CheckPendingTaxi then
			self:CheckPendingTaxi()
		end
	end
end
-----------------------------------------
function InFlight:PLAYER_ENTERING_WORLD()
-----------------------------------------
	if UnitOnTaxi("player") and LoadInFlight() then
		if self.StartTaxiFlight then
			self:StartTaxiFlight()
		end
	end
end


if select(4, GetAddOnInfo("InFlight")) then  -- maybe this stuff gets garbage collected if InFlight isn't loadable
	local t
	do
	-- LOCALIZATION
	local nighthaven = "Nighthaven"					--Nighthaven, Moonglade
	local druidgossip = "I'd like to fly to (.+)."	--Druid gossip option
	local expedition = "Expedition Point"			--Expedition Point
	local hellfire = "Hellfire Peninsula"			--Another for Shatter Point (aka Honor Point)
	local shatter = "Shatter Point"
	local honorpoint = "Honor Point"
	local hellgossip = "Send me to (.+)!"			--Hellfire special flightpath gossip option (Alliance)
	local skyguard = "Skyguard Outpost"
	local blackwind = "Blackwind Landing"
	local sssa = "Shattered Sun Staging Area"		-- Shattered Sun Offensive bombing run
	local srharbor = "Sun's Reach Harbor"
	local sssagossip = "Speaking of action"			-- shattered sun gossip air strike gossip
	local sssagossip2 = "I need to intercept"		-- dawnblade reinforcements gossip
	local thesinloren = "The Sin'loren"				-- The Sin'loren dragonhawk
	local sinlorengossip = "<Ride the dragonhawk"	-- The Sin'loren gossip
	local swcity = "Stormwind City"
	local swharborscenic = "flight around Stormwind Harbor"  -- flight tour around Stormwind Harbor
	local cavernsoftime = "Caverns of Time"
	local cavernsentrance = "Please take me"		-- dragon flight at Caverns of Time entrance
	local oldhillsbrad = "Old Hillsbrad Foothills"
	local oldhillsbradgossip = "I'm ready to go"
	local durnholdekeep = "Durnholde Keep"
	local amberledge = "Amber Ledge"
	local transitusshield = "Transitus Shield (Scenic)"  -- gossip flightpath, long way
	local amberledgegossip = "I'd like passage"
	local valgarde = "Valgarde"								-- Valgarde to Explorer's League Outpost
	local explorersleague = "Explorers' League Outpost"
	local valgardegossip = "Take me to the Explorers"
	local argenttournament = "Argent Tournament Grounds"	-- "Get Kraken" daily quest
	local argenttournamentgossip = "Mount the Hippogryph"
	local fin = "Return"
	local ashenvalesaber = "one of your nightsabers to (.+)."
	local astranaar, maestrapost = "Astranaar", "Maestra's Post"
	local darkshoregossip = "like to travel to (.+)."
	local lordanei, ruinsofmathystra, shatterspearvale, bashalaran = "Lor'danei", "Ruins of Mathystra", "Shatterspear Vale", "Bashal'Aran"
	local feralasgossip1, feralasgossip2 = "Take me to the battle", "I must return to"
	local feathermoonstronghold, ruinsoffeathermoon = "Feathermoon Stronghold", "Ruins of Feathermoon"
	local terrorwinggossip = "head down to the Swamp of Sorrows"
	local terrorwing, bogpaddle = "Terror Wing Path", "Bogpaddle"

	if gl == "koKR" then
		nighthaven = "나이트헤이븐"
		druidgossip = "(.+)|1으로;로; 가고 싶습니다."
		expedition = "원정대 거점"
		hellfire = "지옥불 반도"
		shatter = "징검다리 거점"
		honorpoint = "명예 거점"
		hellgossip = "(.+)|1으로;로; 보내 주십시오!"
		skyguard = "하늘경비대 전초기지"
		blackwind = "검은바람 비행기지"
	elseif gl == "zhCN" then
		nighthaven = "永夜港"
		druidgossip = "我想飞往(.+)。"
		expedition = "远征军岗哨"
		hellfire = "地狱火半岛"
		shatter = "破碎岗哨"
		honorpoint = "荣耀岗哨"
		hellgossip = "送我到(.+)去！"
		skyguard = "天空卫队哨站"
		blackwind = "黑风码头"
		sssa = "破碎残阳基地"
		srharbor = "阳湾港口"
		sssagossip = "说到行动"
		sssagossip2 = "我必须阻止"
		thesinloren = "辛洛雷号"
		sinlorengossip = "<骑上龙鹰"
	elseif gl == "zhTW" then
		nighthaven = "永夜港"				--Nighthaven, Moonglade
		druidgossip = "我想飛往(.+)。"	--Druid gossip option
		expedition = "遠征隊哨塔"			--Expedition Point
		hellfire = "地獄火半島"			--Another for Shatter Point (aka Honor Point)
		shatter = "破碎崗哨"
		honorpoint = "榮譽崗哨"
		hellgossip = "送我去(.+)!"			--Hellfire special flightpath gossip option (Alliance)
		skyguard = "禦天者崗哨"
		blackwind = "黑風平臺"
	elseif gl == "deDE" then
		nighthaven = "Nachthafen"
		druidgossip = "Wollt Ihr jetzt nach %s? %s fliegen?"   --Druid gossip option
		expedition = "Expeditionsr\195\188stlager"
		hellfire = "H\195\182llenfeuerhalbinsel"
		shatter = "Tr\195\188mmerposten"
		honorpoint = "Ehrenpunkt"
		hellgossip = "Bringt mich zum (.+)!"
		skyguard = "Au\195\159enposten der Himmelswache"
		blackwind = "Schattenwindlager"
	elseif gl == "esES" then
		nighthaven = "Asilo de la noche"		--Nighthaven, Moonglade
		druidgossip = "Me gustaría volar a (.+)."	--Druid gossip option
		expedition = "Punto de Expedición"		--Expedition Point
		hellfire = "Península del Fuego Infernal"	--Another for Shatter Point (aka Honor Point)
		shatter = "Punto de Añicos"
		honorpoint = "Punto de Honor"
		hellgossip = "Envíame a (.+)!"			--Hellfire special flightpath gossip option (Alliance)
		skyguard = "Puesto avanzado del Protector del Cielo"
		blackwind = "Aterrizaje del VientoNegro"
		sssa = "Zona de Espera del Sol Roto"		-- Shattered Sun Offensive bombing run
		srharbor = "Puerto del Alcance del Sol"
		sssagossip = "Hablando de acción"		-- shattered sun gossip air strike gossip
		sssagossip2 = "Necesito interceptar"		-- dawnblade reinforcements gossip
		thesinloren = "El Sin'loren"			-- The Sin'loren dragonhawk
		sinlorengossip = "<Monte el HalcónDragón"	-- The Sin'loren gossip
	elseif gl == "ruRU" then
		nighthaven = "Ночная Гавань"					--Nighthaven, Moonglade
		druidgossip = "I'd like to fly to (.+)."	--Druid gossip option
		expedition = "Лагерь экспедиции"			--Expedition Point
		hellfire = "Полуостров Адского Пламени"			--Another for Shatter Point (aka Honor Point)
		shatter = "Парящая застава"
		honorpoint = "Honor Point" -- Оплот Чести
		hellgossip = "Send me to (.+)!"			--Hellfire special flightpath gossip option (Alliance)
		skyguard = "Застава Стражи Небес"
		blackwind = "Лагерь Черного Ветра"
		sssa = "П.П. Расколотого Солнца"		-- Shattered Sun Offensive bombing run
		srharbor = "Гавань Солнечного Края"
		sssagossip = "Speaking of action"			-- shattered sun gossip air strike gossip
		sssagossip2 = "I need to intercept"		-- dawnblade reinforcements gossip
		thesinloren = "Син'лорен"				-- The Sin'loren dragonhawk
		sinlorengossip = "<Ride the dragonhawk"	-- The Sin'loren gossip
		swcity = "Штормград"
		swharborscenic = "flight around Stormwind Harbor"  -- flight tour around Stormwind Harbor
		cavernsoftime = "Пещеры Времени"
		cavernsentrance = "Please take me"		-- dragon flight at Caverns of Time entrance
		oldhillsbrad = "Старые предгорья Хилсбрада"
		oldhillsbradgossip = "I'm ready to go"
		durnholdekeep = "Крепость Дарнхольд"
		amberledge = "Янтарная гряда"
		transitusshield = "Маскировочный щит"  -- gossip flightpath, long way
		amberledgegossip = "Мне хотелось бы"
		valgarde = "Валгард"								-- Valgarde to Explorer's League Outpost
		explorersleague = "Лагерь Лиги Исследователей"
		valgardegossip = "Take me to the Explorers"
		argenttournament = "Ристалище Серебряного турнира"	-- "Get Kraken" daily quest
		argenttournamentgossip = "Забраться на гиппогрифа"
		fin = "Return"
	end
	t = {
		[nighthaven] = { match = druidgossip, },
		[expedition] = { match = hellgossip, },
		[hellfire] = { match = hellgossip, s = honorpoint },
		[shatter] = { match = hellgossip, },
		[skyguard] = { find = blackwind, d = blackwind, }, 
		[blackwind] = { find = skyguard, d = skyguard, },
		[sssa] = { find = sssagossip, find2 = sssagossip2, s = sssa, d = sssa, s2 = sssa, d2 = thesinloren, },
		[thesinloren] = { find = sinlorengossip, d = sssa, },
		[swcity] = { find = swharborscenic, },
		[cavernsoftime] = { find = cavernsentrance, },
		[oldhillsbrad] = { find = oldhillsbradgossip, d = durnholdekeep, },
		[amberledge] = { find = amberledgegossip, d = transitusshield, },
		[valgarde] = { find = valgardegossip, d = explorersleague, },
		[argenttournament] = { find = argenttournamentgossip, d = fin, },
		[astranaar] = { match = ashenvalesaber, },
		[maestrapost] = { match = ashenvalesaber, },
		[lordanei] = { match = darkshoregossip, },
		[feathermoonstronghold] = { find = feralasgossip1, d = ruinsoffeathermoon, }, 
		[ruinsoffeathermoon] = { find = feralasgossip2, d = feathermoonstronghold, },
		[terrorwing] = { find = terrorwinggossip, d = bogpaddle, },
	}
	t[srharbor] = t[sssa]
	t[ruinsofmathystra], t[shatterspearvale], t[bashalaran] = t[lordanei], t[lordanei], t[lordanei]
	end

	---------------------------------
	function InFlight:SetupInFlight()
	---------------------------------
	  	SlashCmdList.INFLIGHT = function()
	  		if LoadInFlight() then
	  			self:ShowOptions()
	  		end
	  	end
	   	SLASH_INFLIGHT1 = "/inflight"

		local panel = CreateFrame("Frame")
		panel.name = "InFlight"
		panel:SetScript("OnShow", function(this)
			if LoadInFlight() and InFlight.SetLayout then
				InFlight:SetLayout(this)
			end
		end)
		InterfaceOptions_AddCategory(panel)
		InFlight.SetupInFlight = nil
	end

	-- support for flightpaths that are started by gossip options
	local strmatch = strmatch
	hooksecurefunc("GossipTitleButton_OnClick", function(this, button)
		if this.type ~= "Gossip" then return end
		
		local subzone = GetMinimapZoneText()
		self.lastGossipSource = subzone or GetZoneText()
		local tsz = t[subzone]
		if not tsz then return end
		
		local text = this:GetText()
		if not text or text == "" then return end
		
		local source, destination
		if tsz.match then  -- destination is in the text
			destination = strmatch(text, tsz.match)
			if not destination then return end
			source = tsz.s or subzone
		elseif tsz.find and strmatch(text, tsz.find) then  -- destination already known 1
			source = tsz.s or subzone
			destination = tsz.d or subzone
		elseif tsz.find2 and strmatch(text, tsz.find2) then  -- destination already known 2
			source = tsz.s2 or subzone
			destination = tsz.d2 or subzone
		end
		if source and LoadInFlight() then
			self:StartMiscFlight(source, destination)
		end
	end)
end

