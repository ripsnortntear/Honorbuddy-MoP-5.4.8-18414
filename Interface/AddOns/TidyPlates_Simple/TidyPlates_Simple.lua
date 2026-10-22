if not TidyPlatesThemeList then TidyPlatesThemeList = {} end
local path = "Interface\\Addons\\TidyPlates_Simple\\Media" 

local simpleBar = {}
simpleBar.healthborder = {
	texture 				= path.."\\empty.tga",
	width = 10,
	height = 10,
	x = -10,
	y = 1,
	anchor = "LEFT",
	show = false,
}

simpleBar.threatborder = {
	texture 				= path.."\\empty.tga",
	width = 110,
	height = 18,
	x = 0,
	y = -8,
	anchor = "BOTTOM",
	show = false,
}

simpleBar.name = {
	typeface 				= path.."\\neuropol x cd rg.ttf",
	size = 8,
	width = 106,
	height = 9,
	x = 0,
	y = 1,
	align = "CENTER",
	anchor = "TOP",
	shadow = false,
	vertical = "BOTTOM",
	show = true,
}

simpleBar.level = {
	typeface 				= path.."\\neuropol x cd rg.ttf",
	size = 7,
	width = 30,
	height = 9,
	x = -60,
 	y = -6,
	align = "CENTER",
	anchor = "TOP",
   	shadow = false,
	vertical = "BOTTOM",
	show = true,
}

simpleBar.healthbar = {
	texture 				= path.."\\barhealth.tga",
	height = 18,
	width = 110,
	x = 0,
	y = 0,
	anchor = "BOTTOM",
	orientation = "HORIZONTAL",
	show = true,
}

simpleBar.castbar = {
	texture 				= path.."\\barcast.tga",
	height = 18,
	width = 110,
	x = 0,
	y = -20,
	anchor = "BOTTOM",
	orientation = "HORIZONTAL",
	show = true,
}

simpleBar.castnostop = {
	texture 				= path.."\\empty.tga",
	height = 18,
	width = 110,
	x = 0,
	y = -20,
	anchor = "BOTTOM",
	orientation = "HORIZONTAL",
	show = true,
}
  
simpleBar.castborder = {
	texture 				= path.."\\empty.tga",
	height = 20,
	width = 112,
	x = 0,
	y = -20,
	anchor = "BOTTOM",
	orientation = "HORIZONTAL",
	show = true,
}  

simpleBar.specialText = {
	typeface 				= path.."\\neuropol x cd rg.ttf",
	size = 10,
	width = 74,
	height = 9,
	x = 0,
	y = 2,
	align = "RIGHT",
	anchor = "BOTTOMRIGHT",
	vertical = "TOP",
	show = false,
}

simpleBar.spellicon = {
	width = 10,
	height = 10,
	x = 52,
 	y = -6,
	anchor = "TOP",
	show = true,
}

simpleBar.eliteicon = {
	texture 				= path.."eliteicon.tga",
	width = 10,
	height = 10,
	x = -21,
	y = 2,
	anchor = "RIGHT",
	show = false,
}

simpleBar.skullicon = {
	width = 14,
	height = 14,
	x = -7,
	y = 2,
	anchor = "RIGHT",
	show = false,
}

simpleBar.raidicon = {
	width = 14,
	height = 14,
	x = 0,
	y = 14,
	anchor = "TOP",
	show = true,
}

simpleBar.frame = {
	width = 96,
	height = 16,
	x = 0,
	y = -5,
}

local simpleSmall = {}
simpleSmall.healthborder = {
	texture 	        	= path.."\\blankborder",
	width = 64,
	height = 8,
	x = 0,
	y = 0,
}

simpleSmall.threatborder = {
	texture 				= path.."\\empty",
	width = 64,
	height = 32,
	x = 0,
	y = 25,
	anchor = "CENTER",
    }

simpleSmall.healthbar = {
	texture 				= path.."\\blank",
	height = 8,
	width = 64,
	x = 0,
	y = 0,
	anchor = "CENTER",
}

simpleSmall.castbar = {
	texture 				= path.."\\empty",
	height = 7,
	width = 114,
	x = 17,
	y = -20,
	anchor = "CENTER",
}

simpleSmall.spellicon = {
	size = 16,
	x = -53,
	y = -15,
}

simpleSmall.name = {
	typeface 				= path.."\\neuropol x cd rg.ttf",
	size = 8,
	width = 70,
	height = 8,
	x = 0,
	y = 5,
	align = "CENTER",
	anchor = "TOP",
	shadow = true,
}

simpleSmall.level = {
	typeface 				= path.."\\neuropol x cd rg.ttf",
	size = 8,
	width = 30,
	height = 9,
	x = -60,
	y = -8,
	align = "RIGHT",
	anchor = "TOP",
	show = false,
}

simpleSmall.raidicon = {
	width = 18,
	height = 18,
	x = 0,
	y = 18,
	anchor = "TOP",
}

simpleSmall.frame = {
	width = 64,
	height = 8,
	x = 0,
	y = 0,
}

local simpleElite = CopyTable(simpleBar)
simpleElite.healthborder.texture = path.."\\eliteicon.tga"
simpleElite.healthborder.show = true

TidyPlatesThemeList["Simple"] = {}
TidyPlatesThemeList["Simple"]["Bar"] = {}
TidyPlatesThemeList["Simple"]["Bar"] = simpleBar
TidyPlatesThemeList["Simple"]["Small"] = {}
TidyPlatesThemeList["Simple"]["Small"] = simpleSmall
TidyPlatesThemeList["Simple"]["Elite"] = {}
TidyPlatesThemeList["Simple"]["Elite"] = simpleElite

local IsTotem = TidyPlatesUtility.IsTotem

local function StyleDelegate(unit)
	if IsTotem(unit.name) then return "Small"
	elseif tonumber(unit.level) == NULL or tonumber(unit.level) > 10 then 
		if unit.isElite then return "Elite"
		else return "Bar" end
	else return "Small" end
end

local function AlphaDelegate(unit)
	if unit.reaction == "FRIENDLY" then
		if unit.health < unit.healthmax then
			return 1, true
		else
			return 0, false
		end
	else
		return 1, false
	end
end

local function HealthColorDelegate(unit)
	local inInstance, instanceType = IsInInstance()
	if instanceType == "pvp" or instanceType == "arena" then
		--if unit.reaction == "HOSTILE" then
		if unit.type == "PLAYER" then
			if unit.class == "DEATHKNIGHT" then
				return 0.77, 0.12, 0.23
			elseif unit.class == "DRUID" then
				return 1.00, 0.49, 0.04
			elseif unit.class == "MAGE" then
				return 0.41, 0.80, 0.94
			elseif unit.class == "HUNTER" then
				return 0.67, 0.83, 0.45
			elseif unit.class == "WARLOCK" then
				return 0.58, 0.51, 0.79
			elseif unit.class == "PRIEST" then
				return 1.00, 1.00, 1.00
			elseif unit.class == "SHAMAN" then
				return 0.00, 0.44, 0.87
			elseif unit.class == "ROGUE" then
				return 1.00, 0.96, 0.41
			elseif unit.class == "PALADIN" then
				return 0.96, 0.55, 0.73
			elseif unit.class == "WARRIOR" then
				return 0.78, 0.61, 0.43
			else
				return 0.31, 0.45, 0.63
				--return 0.69, 0.31, 0.31
			end
		elseif unit.reaction == "FRIENDLY" then
			return 0.33, 0.59, 0.33
		elseif unit.reaction == "NEUTRAL" then
			return 0.65, 0.63, 0.35
		else
			return 0.69, 0.31, 0.31
		end
	elseif unit.threatSituation ~= "HIGH" then
		if unit.reaction == "FRIENDLY" and unit.type == "NPC" then
			return 0.33, 0.59, 0.33
		elseif unit.reaction == "FRIENDLY" and unit.type == "PLAYER" then
			return 0.31, 0.45, 0.63
		elseif unit.reaction == "NEUTRAL" then
			return 0.65, 0.63, 0.35
		elseif unit.reaction == "HOSTILE" then
			return 0.69, 0.31, 0.31
		end
	else
		return 1, 0, 0
	end
end

local function ThreatColorDelegate(unit)
	if unit.threatSituation == "LOW" then
		return 1, 1, 1, 0
	elseif unit.threatSituation == "MEDIUM" then
		return 1, 1, 0
	elseif unit.threatSituation == "HIGH" then
		return 1, 0, 0
	end
end

TidyPlatesThemeList["Simple"].SetStyle = StyleDelegate
TidyPlatesThemeList["Simple"].SetHealthbarColor = HealthColorDelegate
TidyPlatesThemeList["Simple"].SetThreatColor = ThreatColorDelegate
TidyPlatesThemeList["Simple"].SetAlpha = AlphaDelegate


-- Automatically turn on Threat Display
SetCVar("threatWarning", 3)