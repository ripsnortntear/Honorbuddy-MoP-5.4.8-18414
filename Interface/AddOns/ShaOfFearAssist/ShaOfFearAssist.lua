-----------------------------------------------------------------------
-- Locals
--
-- upvalues
local min = math.min
local pi = math.pi
local cos = math.cos
local sin = math.sin
local rad = math.rad
local atan = atan
local GameTooltip = GameTooltip
local CreateFrame = CreateFrame
local GetPlayerMapPosition = GetPlayerMapPosition
local SetMapToCurrentZone = SetMapToCurrentZone
local UnitClass = UnitClass
local CUSTOM_CLASS_COLORS = CUSTOM_CLASS_COLORS
local CUSTOM_CLASS_COLORS = CUSTOM_CLASS_COLORS
local RAID_CLASS_COLORS = RAID_CLASS_COLORS
local type = type
local unpack = unpack
local tonumber = tonumber
local print = print
local UnitAffectingCombat = UnitAffectingCombat
local GetNumGroupMembers = GetNumGroupMembers
local GetRaidRosterInfo = GetRaidRosterInfo
local UIParent = UIParent
local IsInInstance = IsInInstance
local GetRealZoneText = GetRealZoneText
local GetCurrentMapAreaID = GetCurrentMapAreaID
local UnitIsUnit = UnitIsUnit
local GetPlayerFacing = GetPlayerFacing
local LibStub = LibStub
local InterfaceOptionsFrame_OpenToCategory = InterfaceOptionsFrame_OpenToCategory

local addon = CreateFrame("Frame")
--[===[@debug@
-- XXX debug
sha = addon
--@end-debug@]===]

local defaults = {
	profile = {
		posx = nil,
		posy = nil,
		lock = nil,
		width = 100,
		height = 80,
		style = "spinning",
		fullcolor = true,
		colormypie = true,
		myblipscale = 12,
		sprayedcolor = {r=1,g=0,b=0},
		sprayedcolor1 = {r=1,g=0.25,b=0},
		sprayedcolor2 = {r=1,g=0.55,b=0},
		sprayedcolor3 = {r=1,g=0.75,b=0},
		sprayedcolor4 = {r=1,g=1,b=0},
		safecolor = {r=0,g=1,b=0},
		currentpiecolor = {r=0,g=0,b=1},
		pattern = "default",
	}
}

local windowShown = nil
local range = 20
local myblip = nil
local platform = false
local dreadSprayCounter = 0
local sprayedPieVerySoon = nil
local sprayedPieSoon = nil
local suggestedSafePie = nil
local shootCounter = 0

local mapData = { 702.083984375,468.75 }

local platformPieSprayOrder = {
	[61046] = {5, 6, 8, 1, 7, 8, 2, 3, 1, 2, 4, 5, 3, 4, 6, 7}, -- Jinlun Kun (Right)
	[61038] = {7, 3, 3, 7, 6, 2, 2, 6, 5, 1, 1, 5, 4, 8, 8, 4}, -- Yang Goushi (Left)
	[61042] = {7, 2, 3, 4, 1, 4, 5, 6, 3, 6, 7, 8, 5, 8, 1, 2}, -- Cheng Kang (Back) -- looked correct
	["test"] = {1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8}, -- TEST
}

local platformSuggestedSafeZone = {
	[61046] = {
		["default"] = 			{4, 4, 4, 4, 4, 4, 4, 4, 8, 8, 8, 8, 8, 8, 8, 8}, -- Jinlun Kun (Right)
		["minimalMovement"] = 	{4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
		["globePickup"] = {
			{2,3,4}, 	-- 1
			{2,3,4,5}, 	-- 2
			{2,3,4,5,6},-- 3
			{3,4,5,6}, 	-- 4
			{4,5,6}, 	-- 5
			{4,5,6,7},	-- 6
			{4,5,6,7,8},-- 7
			{5,6,7,8},	-- 8
			{6,7,8},	-- 9
			{1,6,7,8},	-- 10
			{1,2,6,7,8},-- 11
			{1,2,7,8},	-- 12
			{1,2,8},	-- 13
			{1,2,8},	-- 14
			{1,2,8},	-- 15
			{1,2,8},	-- 16
		},
		["globePickupBack"] = { -- Same as globePickup
			{2,3,4}, 	-- 1
			{2,3,4,5}, 	-- 2
			{2,3,4,5,6},-- 3
			{3,4,5,6}, 	-- 4
			{4,5,6}, 	-- 5
			{4,5,6,7},	-- 6
			{4,5,6,7,8},-- 7
			{5,6,7,8},	-- 8
			{6,7,8},	-- 9
			{1,6,7,8},	-- 10
			{1,2,6,7,8},-- 11
			{1,2,7,8},	-- 12
			{1,2,8},	-- 13
			{1,2,8},	-- 14
			{1,2,8},	-- 15
			{1,2,8},	-- 16
		},
		["globePickupFront"] = { -- Same as globePickup
			{2,3,4}, 	-- 1
			{2,3,4,5}, 	-- 2
			{2,3,4,5,6},-- 3
			{3,4,5,6}, 	-- 4
			{4,5,6}, 	-- 5
			{4,5,6,7},	-- 6
			{4,5,6,7,8},-- 7
			{5,6,7,8},	-- 8
			{6,7,8},	-- 9
			{1,6,7,8},	-- 10
			{1,2,6,7,8},-- 11
			{1,2,7,8},	-- 12
			{1,2,8},	-- 13
			{1,2,8},	-- 14
			{1,2,8},	-- 15
			{1,2,8},	-- 16
		},
	},
	[61038] = {
		["default"] = 			{4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3}, -- Yang Goushi (Left)
		["minimalMovement"] = 	{4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3},
		["globePickup"] = {
			{1,4,5,8}, 		-- 1
			{1,4,5,8}, 		-- 2
			{1,4,5,8},		-- 3
			{1,3,4,5,8}, 	-- 4
			{1,3,4,5,7,8}, 	-- 5
			{1,3,4,7,8},	-- 6
			{3,4,7,8},		-- 7
			{2,3,4,7,8},	-- 8
			{2,3,4,6,7,8},	-- 9
			{2,3,6,7,8},	-- 10
			{2,3,6,7},		-- 11
			{1,2,3,6,7},	-- 12
			{1,2,3,5,6,7},	-- 13
			{1,2,3,5,6,7},	-- 14
			{1,2,3,5,6,7},	-- 15
			{1,2,3,5,6,7},	-- 16
		},
		["globePickupBack"] = {
			{1,8}, 		-- 1
			{1,8}, 		-- 2
			{1,8},		-- 3
			{1,8}, 		-- 4
			{1,7,8},	-- 5
			{1,7,8},	-- 6
			{7,8},		-- 7
			{7,8},		-- 8
			{6,7,8},	-- 9
			{6,7,8},	-- 10
			{6,7},		-- 11
			{6,7},		-- 12
			{5,6,7},	-- 13
			{5,6,7},	-- 14
			{5,6,7},	-- 15
			{5,6,7},	-- 16
		},
		["globePickupFront"] = {
			{4,5}, 		-- 1
			{4,5}, 		-- 2
			{4,5},		-- 3
			{3,4,5}, 	-- 4
			{3,4,5}, 	-- 5
			{3,4},	-- 6
			{3,4},		-- 7
			{2,3,4},	-- 8
			{2,3,4},	-- 9
			{2,3},	-- 10
			{2,3},		-- 11
			{1,2,3},	-- 12
			{1,2,3},	-- 13
			{1,2,3},	-- 14
			{1,2,3},	-- 15
			{1,2,3},	-- 16
		},
	},
	[61042] = {
		["default"] = 			{8, 8, 8, 8, 8, 8, 8, 8, 4, 4, 4, 4, 4, 4, 4, 4}, -- Cheng Kang (Back)
		["minimalMovement"] = 	{8, 8, 8, 8, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
		["globePickup"] = {
			{5,6}, 		-- 1
			{5,6,7,8}, 	-- 2
			{5,6,7,8},	-- 3
			{6,7,8}, 	-- 4
			{7,8}, 		-- 5
			{1,2,7,8},	-- 6
			{1,2,7,8},	-- 7
			{1,2,8},	-- 8
			{1,2},		-- 9
			{1,2,3,4},	-- 10
			{1,2,3,4},	-- 11
			{2,3,4},	-- 12
			{3,4},		-- 13
			{3,4},		-- 14
			{3,4},		-- 15
			{3,4},		-- 16
		},
		["globePickupBack"] = { -- Same as globePickup
			{5,6}, 		-- 1
			{5,6,7,8}, 	-- 2
			{5,6,7,8},	-- 3
			{6,7,8}, 	-- 4
			{7,8}, 		-- 5
			{1,2,7,8},	-- 6
			{1,2,7,8},	-- 7
			{1,2,8},	-- 8
			{1,2},		-- 9
			{1,2,3,4},	-- 10
			{1,2,3,4},	-- 11
			{2,3,4},	-- 12
			{3,4},		-- 13
			{3,4},		-- 14
			{3,4},		-- 15
			{3,4},		-- 16
		},
		["globePickupFront"] = { -- Same as globePickup
			{5,6}, 		-- 1
			{5,6,7,8}, 	-- 2
			{5,6,7,8},	-- 3
			{6,7,8}, 	-- 4
			{7,8}, 		-- 5
			{1,2,7,8},	-- 6
			{1,2,7,8},	-- 7
			{1,2,8},	-- 8
			{1,2},		-- 9
			{1,2,3,4},	-- 10
			{1,2,3,4},	-- 11
			{2,3,4},	-- 12
			{3,4},		-- 13
			{3,4},		-- 14
			{3,4},		-- 15
			{3,4},		-- 16
		},
	},
	["test"] = {
		["default"] = 			{2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},  -- TEST
		["minimalMovement"] = { -- Multiple value test
			{1,2},
			{1,2,4},
			{1,2,3,4,5,6},
			8,
			7,
			6,
			{1,2},
			{1,2,4},
			{1,2,3,4,5,6},
			8,
			7,
			6,
			{1,2},
			{2,4,6,8},
			{3,6},
			{2,4,6,8},
		},
	},
}

-- it is assumed that all pies are equal in size and they don't overlap
-- this is the biggest source of inaccuraccy (the only unless mapdata us fucked)
local platformData = { -- this data is based on a few point of reference then generated by drawing shit then looking up coordinates of points on a picture
-- reference points were: boss in middle of platform, the position where you enter, edge of platform
-- points are meant to be roughly as the hours are on the clock when facing towards north
	["test"] = {
		[10] = { 0.56441903114319, 0.46709984540939 },
		[11] = { 0.57575696706772, 0.45102655887604 },
		[1] = { 0.6021317243576, 0.45170402526855 },
		[2] = { 0.6135094165802, 0.46685117483139 },
		[4] = { 0.61374545097351, 0.50771379470825 },
		[5] = { 0.60232824087143, 0.52308487892151 },
		[7] = { 0.57563626766205, 0.52302396297455 },
		[8] = { 0.56478691101074, 0.50713908672333 },
		["boss"] = { 0.58890211582184, 0.48720079660416 },
		--[1] = { 0.43257997299335, 0.45531824631467 },
		--[2] = { 0.46318117380143, 0.40879055908267 },
		--[4] = { 0.50678537421238, 0.40829993572267 },
		--[5] = { 0.5378498250265, 0.454133776768 },
		--[7] = { 0.53817739223372, 0.519443239744 },
		--[8] = { 0.50757619142564, 0.565970926976 },
		--[10] = { 0.4639719910147, 0.56646155031467 },
		--[11] = { 0.43290754020057, 0.52062770926933 },
		--["boss"] = { 0.48537868261337, 0.48738074302673 },
	},
	[61046] = { -- Jinlun Kun (Right)
		-- right of enterance: 11
		-- left of enterance: 1
		[1] = { 0.36511522073559, 0.0073186877653333 },
		[2] = { 0.40097359641469, 0.044480834197333 },
		[4] = { 0.40878491776948, 0.10873568008533 },
		[5] = { 0.38397341866441, 0.162443608128 },
		[7] = { 0.34107333880173, 0.17414324251733 },
		[8] = { 0.30521496310838, 0.136981096064 },
		[10] = { 0.29740364176784, 0.072726250197333 },
		[11] = { 0.32221514085866, 0.019018322133333 },
		["boss"] = { 0.35309427976608, 0.090730965137482 }, -- might want to double check
	},
	[61038] = { -- Yang Goushi (Left)
		-- right of enterance: 4
		-- left of enterance: 5
		[1] = { 0.50592280328998, 0.847146740736 },
		[2] = { 0.52219793842239, 0.90773840740267 },
		[4] = { 0.50510065916927, 0.96782001546667 },
		[5] = { 0.46464631982797, 0.99219657380267 },
		[7] = { 0.42453252374833, 0.96658862513067 },
		[8] = { 0.40825738861592, 0.905996958464 },
		[10] = { 0.42535466786904, 0.8459153504 },
		[11] = { 0.46580900721034, 0.821538792064 },
		["boss"] = { 0.465227663517, 0.90686768293381 },
	},
	[61042] = { -- Cheng Kang (Back)
		-- right of enterance: 8
		-- left of enterance: 10
		[1] = { 0.12637842469656, 0.525899113792 },
		[2] = { 0.16179988858616, 0.563989657856 },
		[4] = { 0.16886396688786, 0.62843825115733 },
		[5] = { 0.14343261832079, 0.681491781824 },
		[7] = { 0.10040318198221, 0.692072211136 },
		[8] = { 0.064981718078377, 0.653981667072 },
		[10] = { 0.057917639790913, 0.58953307377067 },
		[11] = { 0.083348988357986, 0.53647954308267 },
		["boss"] = { 0.1133908033371, 0.60898566246033 },
	},
}

for k, v in pairs(platformData) do
	for r, s in pairs(v) do
		s[1], s[2] = s[1]*mapData[1], s[2]*mapData[2]
	end
end

local function getPiePoints(index, platform)
	local pies = {
		-- pies are indexed from north going east, south, west (aka clockwise)
		[1] = { 11, 1 },
		[2] = { 1, 2 },
		[3] = { 2, 4 },
		[4] = { 4, 5 },
		[5] = { 5, 7 },
		[6] = { 7, 8 },
		[7] = { 8, 10 },
		[8] = { 10, 11 },
	}
	return platformData[platform]["boss"], platformData[platform][pies[index][1]], platformData[platform][pies[index][2]]
end

--- XXX MAKE THIS LOCAL AFTER DEBUGGING
local display = nil

local unlock = "Interface\\AddOns\\ShaOfFearAssist\\Textures\\icons\\lock"
local lock = "Interface\\AddOns\\ShaOfFearAssist\\Textures\\icons\\un_lock"

local window = nil

local L = LibStub("AceLocale-3.0"):GetLocale("ShaAssist")

-----------------------------------------------------------------------
-- Display Window
--

-- Mostly ripped from the Atramedes addon, which ripped it from BigWigs proximity display
local function onDragStart(self) self:StartMoving() end
local function onDragStop(self)
	self:StopMovingOrSizing()
	local s = self:GetEffectiveScale()
	addon.db.profile.posx = self:GetLeft() * s
	addon.db.profile.posy = self:GetTop() * s
end
local function OnDragHandleMouseDown(self) self.frame:StartSizing("BOTTOMRIGHT") end
local function OnDragHandleMouseUp(self) self.frame:StopMovingOrSizing() end
local function onResize(self, width, height)
	addon.db.profile.width = width
	addon.db.profile.height = height
	local width, height = display:GetWidth(), display:GetHeight()
	local ppy = min(width, height) / (range*3) -- pixel per yard
	display.rangeCircle:SetSize(range*2*ppy, range*2*ppy)
	display.boss:SetSize(range*0.2*ppy, range*0.2*ppy)
	local pieX = "pie%d"
	for i=1, 8 do
		display[pieX:format(i)]:SetSize(ppy * range * 2, ppy * range * 2)
	end
end

local locked = nil
local function lockDisplay()
	if locked then return end
	window:EnableMouse(false)
	window:SetMovable(false)
	window:SetResizable(false)
	window:RegisterForDrag()
	window:SetScript("OnSizeChanged", nil)
	window:SetScript("OnDragStart", nil)
	window:SetScript("OnDragStop", nil)
	window.drag:Hide()
	locked = true
end

local function unlockDisplay()
	if not locked then return end
	window:EnableMouse(true)
	window:SetMovable(true)
	window:SetResizable(true)
	window:RegisterForDrag("LeftButton")
	window:SetScript("OnSizeChanged", onResize)
	window:SetScript("OnDragStart", onDragStart)
	window:SetScript("OnDragStop", onDragStop)
	window.drag:Show()
	locked = nil
end

local function updateLockButton()
	if not window then return end
	window.lock:SetNormalTexture(addon.db.profile.lock and unlock or lock)
end

local function toggleLock()
	if addon.db.profile.lock then
		unlockDisplay()
	else
		lockDisplay()
	end
	addon.db.profile.lock = not addon.db.profile.lock
	updateLockButton()
end

local function onControlEnter(self)
	GameTooltip:ClearLines()
	GameTooltip:SetOwner(self, "ANCHOR_TOPLEFT")
	GameTooltip:AddLine(self.tooltipHeader)
	GameTooltip:AddLine(self.tooltipText, 1, 1, 1, 1)
	GameTooltip:Show()
end
local function onControlLeave() GameTooltip:Hide() end
local function closeWindow() if window then window:Hide() windowShown = false end end

local function ensureDisplay()
	if window then return end

	display = CreateFrame("Frame", "ShaAssistAnchor", UIParent)
	display:SetWidth(addon.db.profile.width)
	display:SetHeight(addon.db.profile.height)
	display:SetMinResize(100, 30)
	display:SetClampedToScreen(true)
	local bg = display:CreateTexture(nil, "PARENT")
	bg:SetAllPoints(display)
	bg:SetBlendMode("BLEND")
	bg:SetTexture(0, 0, 0, 0.3)

	local close = CreateFrame("Button", nil, display)
	close:SetPoint("BOTTOMRIGHT", display, "TOPRIGHT", -2, 2)
	close:SetHeight(16)
	close:SetWidth(16)
	close.tooltipHeader = L["Close"]
	close.tooltipText = L["Closes the Sha Assist display."]
	close:SetNormalTexture("Interface\\AddOns\\ShaOfFearAssist\\Textures\\icons\\close")
	close:SetScript("OnEnter", onControlEnter)
	close:SetScript("OnLeave", onControlLeave)
	close:SetScript("OnClick", closeWindow)

	local lock = CreateFrame("Button", nil, display)
	lock:SetPoint("BOTTOMLEFT", display, "TOPLEFT", 2, 2)
	lock:SetHeight(16)
	lock:SetWidth(16)
	lock.tooltipHeader = L["Toggle lock"]
	lock.tooltipText = L["Toggle whether or not the Sha Assist window should be locked or not."]
	lock:SetScript("OnEnter", onControlEnter)
	lock:SetScript("OnLeave", onControlLeave)
	lock:SetScript("OnClick", toggleLock)
	display.lock = lock

	local header = display:CreateFontString(nil, "OVERLAY", "GameFontNormal")
	header:SetText("Sha Assist")
	header:SetPoint("BOTTOM", display, "TOP", 0, 4)

	local rangeCircle = display:CreateTexture(nil, "OVERLAY")
	rangeCircle:SetPoint("CENTER")
	rangeCircle:SetTexture([[Interface\AddOns\ShaOfFearAssist\Textures\alert_circle]])
	rangeCircle:SetBlendMode("ADD")
	display.rangeCircle = rangeCircle
	display.rangeCircle:SetAlpha(0.5)


	local pieX = "pie%d"
	for i=1, 8 do
		display[pieX:format(i)] = display:CreateTexture(nil, "OVERLAY")
		display[pieX:format(i)]:SetPoint("CENTER")
		display[pieX:format(i)]:SetTexture(([[Interface\AddOns\ShaOfFearAssist\Textures\%d]]):format(i))
		display[pieX:format(i)]:SetBlendMode("ADD")
		display[pieX:format(i)]:SetAlpha(1)
	end

	local boss = display:CreateTexture(nil, "OVERLAY")
	boss:SetPoint("CENTER")
	boss:SetTexture([[Interface\TargetingFrame\UI-RaidTargetingIcon_8]])
	boss:SetBlendMode("ADD")
	display.boss = boss

	local drag = CreateFrame("Frame", nil, display)
	drag.frame = display
	drag:SetFrameLevel(display:GetFrameLevel() + 10) -- place this above everything
	drag:SetWidth(16)
	drag:SetHeight(16)
	drag:SetPoint("BOTTOMRIGHT", display, -1, 1)
	drag:EnableMouse(true)
	drag:SetScript("OnMouseDown", OnDragHandleMouseDown)
	drag:SetScript("OnMouseUp", OnDragHandleMouseUp)
	drag:SetAlpha(0.5)
	display.drag = drag

	local tex = drag:CreateTexture(nil, "BACKGROUND")
	tex:SetTexture("Interface\\AddOns\\ShaOfFearAssist\\Textures\\draghandle")
	tex:SetWidth(16)
	tex:SetHeight(16)
	tex:SetBlendMode("ADD")
	tex:SetPoint("CENTER", drag)

	window = display

	local x = addon.db.profile.posx
	local y = addon.db.profile.posy
	if x and y then
		local s = display:GetEffectiveScale()
		display:ClearAllPoints()
		display:SetPoint("TOPLEFT", UIParent, "BOTTOMLEFT", x / s, y / s)
	else
		display:ClearAllPoints()
		display:SetPoint("CENTER", UIParent)
	end

	updateLockButton()
	if addon.db.profile.lock then
		locked = nil
		lockDisplay()
	else
		locked = true
		unlockDisplay()
	end

	myblip = display:CreateTexture(nil, "OVERLAY", nil, 7)
	myblip:SetSize(56, 56) -- this gets changed on onupdate anyways
	myblip:SetTexture([[Interface\Minimap\MinimapArrow]])
end

local function resetWindow()
	window:ClearAllPoints()
	window:SetPoint("CENTER", UIParent)
	window:SetWidth(defaults.profile.width)
	window:SetHeight(defaults.profile.height)
	addon.db.profile.posx = nil
	addon.db.profile.posy = nil
	addon.db.profile.width = nil
	addon.db.profile.height = nil
end

-------------------------------------------------------------------------------
-- Texture Updater
--

local function rotateTextureAroundCenterPoint(texture, hAngle)
	local s = sin(hAngle)
	local c = cos(hAngle)
	texture:SetTexCoord(
	0.5 - s, 0.5 + c,
	0.5 + c, 0.5 + s,
	0.5 - c, 0.5 - s,
	0.5 + s, 0.5 - c
	)
end

function addon:setPlatformOrientation(a)
	local width, height = display:GetWidth(), display:GetHeight()
	--local range = activeRange and activeRange or 10
	-- range * 3, so we have 3x radius space
	local pixperyard = min(width, height) / (range * 3)
	-- left platform is rotated compared to the other two
	-- tho we should probably not be rotating this much in onupdate
	local angle = nil
	if addon.db.profile.style == "fixed" then
		if platform == 61038 then
			angle = 112.5
			local pieX = "pie%d"
			for i=1, 8 do
				rotateTextureAroundCenterPoint(display[pieX:format(i)], rad(angle))
				display[pieX:format(i)]:SetSize(pixperyard * range * 3, pixperyard * range * 3)
			end
			rotateTextureAroundCenterPoint(display.rangeCircle, rad(angle))
			display.rangeCircle:SetSize(pixperyard * range * 3, pixperyard * range * 3)
		else
			angle = a or 135
			local pieX = "pie%d"
			for i=1, 8 do
				rotateTextureAroundCenterPoint(display[pieX:format(i)], rad(angle))
				display[pieX:format(i)]:SetSize(pixperyard * range * 3, pixperyard * range * 3)
			end
			rotateTextureAroundCenterPoint(display.rangeCircle, rad(angle))
			display.rangeCircle:SetSize(pixperyard * range * 3, pixperyard * range * 3)
		end
	else
		if not a then return end
		if platform == 61038 then
			angle = 112.5 + a
			local pieX = "pie%d"
			for i=1, 8 do
				rotateTextureAroundCenterPoint(display[pieX:format(i)], rad(angle))
				display[pieX:format(i)]:SetSize(pixperyard * range * 3, pixperyard * range * 3)
			end
			rotateTextureAroundCenterPoint(display.rangeCircle, rad(angle))
			display.rangeCircle:SetSize(pixperyard * range * 3, pixperyard * range * 3)
		else
			angle = 135 + a
			local pieX = "pie%d"
			for i=1, 8 do
				rotateTextureAroundCenterPoint(display[pieX:format(i)], rad(angle))
				display[pieX:format(i)]:SetSize(pixperyard * range * 3, pixperyard * range * 3)
			end
			rotateTextureAroundCenterPoint(display.rangeCircle, rad(angle))
			display.rangeCircle:SetSize(pixperyard * range * 3, pixperyard * range * 3)
		end
	end
end

local function sign(p1, p2, p3)
	return (p1[1] - p3[1]) * (p2[2] - p3[2]) - (p2[1] - p3[1]) * (p1[2] - p3[2]);
end

local function pointInTriangle(pt, v1, v2, v3)
	local b1, b2, b3;

	b1 = sign(pt, v1, v2) < 0;
	b2 = sign(pt, v2, v3) < 0;
	b3 = sign(pt, v3, v1) < 0;

	return ((b1 == b2) and (b2 == b3));
end

do
	-- dx and dy are in yards
	-- facing is radians with 0 being north, counting up clockwise
	local setDot = function(dx, dy, blip)
		local width, height = display:GetWidth(), display:GetHeight()
		--local range = activeRange and activeRange or 10
		-- range * 3, so we have 3x radius space
		local pixperyard = min(width, height) / (range * 3)

		-- rotate relative to player facing
		local rotangle = 0
		local x = (dx * cos(rotangle)) - (-1 * dy * sin(rotangle))
		local y = (dx * sin(rotangle)) + (-1 * dy * cos(rotangle))
		local above0 = nil
		if y > 0 then
			above0 = true
		else
			above0 = false
		end

		if addon.db.profile.style == "spinning" then
			addon:setPlatformOrientation(above0 and -1*atan(dx/dy)+180 or -1*atan(dx/dy))
			x = 0
			y = -1*(dx^2 + dy^2)^0.5
		end

		x = x * pixperyard
		y = y * pixperyard

		blip:ClearAllPoints()
		-- Clamp to frame if out-of-bounds, mainly for reverse proximity
		if x < -(width / 2) then
			x = -(width / 2)
		elseif x > (width / 2) then
			x = (width / 2)
		end
		if y < -(height / 2) then
			y = -(height / 2)
		elseif y > (height / 2) then
			y = (height / 2)
		end

		blip:SetPoint("CENTER", display, "CENTER", x, y)
		if not blip.isShown then
			blip.isShown = true
			blip:Show()
		end

		blip:SetSize(addon.db.profile.myblipscale*pixperyard, addon.db.profile.myblipscale*pixperyard)

		-- do some rotation
		local bearing = GetPlayerFacing()
		local hAngle = bearing - rad(225)
		if addon.db.profile.style == "spinning" then
			local adjustingAngle = nil
			if above0 then
				adjustingAngle = rad(atan(dx/dy)+180)
			else
				adjustingAngle = rad(atan(dx/dy))
			end
			hAngle = hAngle-adjustingAngle
		end
		rotateTextureAroundCenterPoint(blip, hAngle)
	end
	function addon:setMyDot(srcX, srcY)
		if not platform then platform = "test" end -- XXX debug
		local bossX, bossY = platformData[platform]["boss"][1], platformData[platform]["boss"][2]
		if not bossX or not bossY then return end

		local dx = (srcX - bossX)
		local dy = (srcY - bossY)

		setDot(dx, dy, myblip)
	end

	function addon:updateData()
		if not platform then platform = "test" end -- XXX debug
		--if platform == "test" and dreadSprayCounter == 0 then dreadSprayCounter = 1 end -- XXX debug
		if type(platform) == "number" or platform == "test" then
			local srcX, srcY = GetPlayerMapPosition("player")
			if srcX == 0 and srcY == 0 then
				SetMapToCurrentZone()
				srcX, srcY = GetPlayerMapPosition("player")
			end
			srcX, srcY = srcX*mapData[1], srcY*mapData[2]

			addon:setMyDot(srcX, srcY)

			--if platform == "test" then
			--	sprayedPieVerySoon, sprayedPieSoon, suggestedSafePie = 1, 2, 3 --XXX debug
			--end

			if addon.db.profile.fullcolor then
				for i=1, 8 do
					if type(platform) == "number" or platform == "test" then
						addon:setPieColor(i, addon.db.profile.sprayedcolor4)
					end
				end
				for i=1, 8 do
					if type(platform) == "number" or platform == "test" then
						if platformPieSprayOrder[platform][dreadSprayCounter+3] then
							if i == platformPieSprayOrder[platform][dreadSprayCounter+3] then
								addon:setPieColor(i, addon.db.profile.sprayedcolor3)
							end
						end
						if platformPieSprayOrder[platform][dreadSprayCounter+2] then
							if i == platformPieSprayOrder[platform][dreadSprayCounter+2] then
								addon:setPieColor(i, addon.db.profile.sprayedcolor2)
							end
						end
						if platformPieSprayOrder[platform][dreadSprayCounter+1] then
							if i == platformPieSprayOrder[platform][dreadSprayCounter+1] then
								addon:setPieColor(i, addon.db.profile.sprayedcolor1)
							end
						end
						if i == platformPieSprayOrder[platform][dreadSprayCounter] then
							addon:setPieColor(i, addon.db.profile.sprayedcolor)
						end
					end
				end
			end
			if addon.db.profile.colormypie then
				for i=1, 8 do
					if type(platform) == "number" or platform == "test" then
						local a, b, c = getPiePoints(i, platform)
						if pointInTriangle({srcX, srcY}, a, b, c) then
							addon:setPieColor(i, addon.db.profile.currentpiecolor)
						else
							if not addon.db.profile.fullcolor then
								addon:setPieColor(i, {r=0.5,g=0.5,b=0.5})
								addon:setPieColor(sprayedPieVerySoon, addon.db.profile.sprayedcolor)
								addon:setPieColor(sprayedPieSoon, addon.db.profile.sprayedcolor4)
							end
						end
					end
				end
			end
			if suggestedSafePie then addon:setPieColor(suggestedSafePie, addon.db.profile.safecolor) end
		else
			addon:clearPieColors()
		end
	end
end

-----------------------------------------------------------------------
-- Utility
--
function addon:getc(s)
	--print(platform, s, GetPlayerMapPosition("player"))
end

local function newSpraySim()
	dreadSprayCounter = dreadSprayCounter + 1

	suggestedSafePie = platformSuggestedSafeZone[platform][platformSuggestedSafeZone[platform][addon.db.profile.pattern] and addon.db.profile.pattern or 'default'][dreadSprayCounter]
	sprayedPieVerySoon = platformPieSprayOrder[platform][dreadSprayCounter]
	if platformPieSprayOrder[platform][dreadSprayCounter+1] then
		sprayedPieSoon = platformPieSprayOrder[platform][dreadSprayCounter+1]
	end
	if dreadSprayCounter > 15 then
		sprayedPieVerySoon, sprayedPieSoon = nil, nil
		if not addon.db.profile.fullcolor then
			addon:clearPieColors()
		end
	end
	if dreadSprayCounter < 17 then
		addon:ScheduleTimer(newSpraySim, 0.5)
	end
end

function addon:sim()
	platform = "test"
	dreadSprayCounter = 0
	shootCounter = 0

	addon:ScheduleTimer(newSpraySim, 0.5)
end

function addon:clearPieColors()
	for i=1, 8 do
		addon:setPieColor(i, {r=0.5,g=0.5,b=0.5})
	end
end

function addon:setPieColor(pie, color)
	-- Table value for multiple pies at once
	if type(pie) ~= "number" and type(pie) ~= "table" then return end
	local pieIndex
	local t = { color.r, color.g, color.b, 1 }
	if type(pie) == "number" then
		pieIndex = ("pie%d"):format(pie)
		display[pieIndex]:SetVertexColor(unpack(t))
	else -- table
		for i,v in pairs(pie) do
			pieIndex = ("pie%d"):format(v)
			display[pieIndex]:SetVertexColor(unpack(t))
		end
	end
end

local function updateDisplay()
	local width, height = display:GetWidth(), display:GetHeight()
	local ppy = min(width, height) / (range * 3)
	display.rangeCircle:SetSize(ppy * range * 2, ppy * range * 2)
	display.boss:SetSize(ppy * range * 0.2, ppy * range * 0.2)
	local pieX = "pie%d"
	for i=1, 8 do
		display[pieX:format(i)]:SetSize(ppy * range * 2, ppy * range * 2)
	end
	addon:clearPieColors()
end

local function checkForWipe()
	local w = true
	local num = GetNumGroupMembers()
	for i = 1, num do
		local name = GetRaidRosterInfo(i)
		if name then
			if UnitAffectingCombat(name) then
				w = false
			end
		end
	end
	if w and windowShown then
		platform = false
		dreadSprayCounter = 0
		updateDisplay()
		closeWindow()
	end
	if not w then addon:ScheduleTimer(checkForWipe, 2) end
end

local function openWindow()
	-- Make sure the window is there
	ensureDisplay()
	-- Start the show!
	window:Show()
	windowShown = true
	updateDisplay()
	addon:clearPieColors()
end

-----------------------------------------------------------------------
-- Slash command
--

local function slashCommand(input)
	input = input:trim()
	if input == "reset" then
		resetWindow()
	elseif input == "lock" or input == "unlock" then
		toggleLock()
	elseif input == "style" then
		if addon.db.profile.style == "spinning" then
			addon.db.profile.style = "fixed"
		else
			addon.db.profile.style = "spinning"
		end
		addon:setPlatformOrientation()
		print("Sha of Fear Assist: <style> = "..addon.db.profile.style)
	elseif input == "fullcolor" then
		addon.db.profile.fullcolor = not addon.db.profile.fullcolor
		print("Sha of Fear Assist: <fullcolor> = "..addon.db.profile.fullcolor and "True" or "False")
	elseif input == "config" then
		InterfaceOptionsFrame_OpenToCategory("ShaOfFearAssist")
	else
		openWindow()
	end
end

-----------------------------------------------------------------------
-- Event handler
--

local function registerEvents()
	addon:RegisterEvent("PLAYER_REGEN_ENABLED")
	addon:RegisterEvent("COMBAT_LOG_EVENT_UNFILTERED")
end

local function unregisterEvents()
	addon:UnregisterEvent("PLAYER_REGEN_ENABLED")
	addon:UnregisterEvent("COMBAT_LOG_EVENT_UNFILTERED")
end

local function detectInstanceChange()
	if IsInInstance() then
		SetMapToCurrentZone()
	end
	local zone = GetRealZoneText()
	if zone == nil or zone == "" then
		-- zone hasn't been loaded yet, try again in 5 secs.
		addon:ScheduleTimer(detectInstanceChange, 5)
		return
	elseif GetCurrentMapAreaID() == 886 and IsInInstance() then
		registerEvents()
	else
		unregisterEvents()
	end
end

local function CLEU(...)
	local timestamp, etype, hideCaster, sourceGUID, sourceName, sourceFlags, sourceRaidFlags, destGUID, player, destFlags, destRaidFlags, spellId, spellName, spellSchool, missType,  amountMissed = ...
	if etype == "SPELL_AURA_APPLIED" and spellId == 118977 then               --Fearless
		if UnitIsUnit("player", player) then
			--addon:getc("Fearless")-- XXX DEBUG
			closeWindow()
			platform = false
		end
	elseif etype == "SPELL_AURA_APPLIED" and spellId == 129378 then     --FadingLight
			closeWindow()
	elseif etype == "SPELL_AURA_REMOVED" and spellId == 129147 then   --OminousCackleRemoved
		if UnitIsUnit("player", player) then
			openWindow()
			platform = true
			shootCounter = 0
			sprayedPieVerySoon, sprayedPieSoon, suggestedSafePie = nil, nil, nil
			addon:clearPieColors()
		end
	elseif etype == "SPELL_CAST_SUCCESS" and spellId == 120047 then        --DreadSprayStart
		if not platform then return end
		--addon:getc("DreadSprayStart")-- XXX DEBUG
		dreadSprayCounter = 0
		shootCounter = 0
	elseif etype == "SPELL_CAST_SUCCESS" and spellId == 119983 then            --DreadSpray
		if not platform then return end
		dreadSprayCounter = dreadSprayCounter + 1
		--addon:getc(("%s %d"):format("dreadSprayCounter", dreadSprayCounter)) -- XXX DEBUG
		if platform == 61046 or platform == 61038 or platform == 61042 then
			suggestedSafePie = platformSuggestedSafeZone[platform][platformSuggestedSafeZone[platform][addon.db.profile.pattern] and addon.db.profile.pattern or 'default'][dreadSprayCounter]
			sprayedPieVerySoon = platformPieSprayOrder[platform][dreadSprayCounter]
			if platformPieSprayOrder[platform][dreadSprayCounter+1] then
				sprayedPieSoon = platformPieSprayOrder[platform][dreadSprayCounter+1]
			end
		end
		if dreadSprayCounter > 15 then
			sprayedPieVerySoon, sprayedPieSoon = nil, nil
			addon:clearPieColors()
		end
	elseif etype == "SPELL_CAST_START" and spellId == 119862 then                  --Shoot
		if not platform then return end -- this prevents people to get warning if they log back into a platform, so need to find a better platform detection
		shootCounter = shootCounter + 1
		local mobId = tonumber(sourceGUID:sub(7, 10), 16)
		if not mobId then return end
		if mobId == 61046 then-- Jinlun Kun
			platform = 61046
		elseif mobId == 61038 then-- Yang Goushi
			platform = 61038
		elseif mobId == 61042 then-- Cheng Kang
			platform = 61042
		end
		addon:setPlatformOrientation()
		--addon:getc(("%s %d"):format("shootCounter", shootCounter)) -- XXX DEBUG
		-- if you are delayed to get to the platform this counter can get out of sync of realizy hence why we check on multiple numbers
		-- another thing that a better platform detection could solve
		if shootCounter == 3 or shootCounter == 5 or shootCounter == 7 then
			dreadSprayCounter = 0
			if platform == 61046 or platform == 61038 or platform == 61042 then
				suggestedSafePie = platformSuggestedSafeZone[platform][platformSuggestedSafeZone[platform][addon.db.profile.pattern] and addon.db.profile.pattern or 'default'][1]
				sprayedPieVerySoon, sprayedPieSoon = platformPieSprayOrder[platform][1], platformPieSprayOrder[platform][2]
			end
		end
	end
end

local LIST_VALUES = {
	['style'] = {
		fixed = L['fixed'],
		spinning = L['spinning'],
	},
	['patterns'] = {
		default = L['defaultPattern'],
		minimalMovement = L['minimalMovement'],
		globePickup = L['globePickup'],
		globePickupBack = L['globePickupBack'],
		globePickupFront = L['globePickupFront'],
	},
}

local options = {
	type = "group",
	handler = addon,
	get = function(info) return addon.db.profile[info[1]] end,
	set = function(info, v) addon.db.profile[info[1]] = v end,
	args = {
		desc = {
			type = "description",
			name = L["Assistance to the dance during Dread Spray"],
			order = 1,
			fontSize = "medium",
		},
		general_header = {
			type = "header",
			name = L["General"],
			order = 70,
		},
		style = {
			order = 100,
			type = "select",
			name = L["Display style"],
			desc = L["Change the style of the Sha of Fear Assist display"],
			values = LIST_VALUES['style'],
		},
		pattern = {
			order = 100,
			type = "select",
			name = L["Pattern"],
			desc = L["Change the display pattern utilized by Sha of Fear Assist."],
			values = LIST_VALUES['patterns'],
		},
		fullcolor = {
			type = "toggle",
			name = L["Full color"],
			desc = L["Toggle if every pie should get colored, or just the very relevant ones"],
			order = 110,
		},
		colormypie = {
			type = "toggle",
			name = L["Color my pie"],
			desc = L["Color the pie you are currently in differently"] ,
			order = 110,
		},
		resetbutton = {
			order = 200,
			type = "execute",
			name = L["Reset display"],
			desc = L["Reset the scale and position of the display"],
			func = resetWindow,
		},
		test = {
			order = 200,
			type = "execute",
			name = L["Test"],
			func = function() openWindow() addon:sim() end,
		},
		myblipscale = {
			order = 300,
			type = "range",
			name = L["My blip scale"],
			desc = L["Set the scale of the arrow indicating your own position on the display"],
			min = 10, max = 50, step = 2,
		},
		color_header = {
			type = "header",
			name = L["Colors"],
			order = 400,
		},
		sprayedcolor = {
			type = "color",
			name = L["Sprayed area color"],
			order = 500,
			hasAlpha = false,
			get = function ()
					local color = addon.db.profile.sprayedcolor
					return color.r, color.g, color.b
				end,
			set = function ( _, r, g, b)
					local color = addon.db.profile.sprayedcolor
					color.r = r
					color.g = g
					color.b = b
				end,
		},
		sprayedcolor1 = {
			type = "color",
			name = L["Next sprayed area color"],
			order = 510,
			hasAlpha = false,
			get = function ()
					local color = addon.db.profile.sprayedcolor1
					return color.r, color.g, color.b
				end,
			set = function ( _, r, g, b)
					local color = addon.db.profile.sprayedcolor1
					color.r = r
					color.g = g
					color.b = b
				end,
		},
		sprayedcolor2 = {
			type = "color",
			name = L["Second next sprayed area color"],
			order = 520,
			hasAlpha = false,
			get = function ()
					local color = addon.db.profile.sprayedcolor2
					return color.r, color.g, color.b
				end,
			set = function ( _, r, g, b)
					local color = addon.db.profile.sprayedcolor2
					color.r = r
					color.g = g
					color.b = b
				end,
		},
		sprayedcolor3 = {
			type = "color",
			name = L["Third next sprayed area color"],
			order = 530,
			hasAlpha = false,
			get = function ()
					local color = addon.db.profile.sprayedcolor3
					return color.r, color.g, color.b
				end,
			set = function ( _, r, g, b)
					local color = addon.db.profile.sprayedcolor3
					color.r = r
					color.g = g
					color.b = b
				end,
		},
		sprayedcolor4 = {
			type = "color",
			name = L["Fourth next sprayed area color"],
			order = 540,
			hasAlpha = false,
			get = function ()
					local color = addon.db.profile.sprayedcolor4
					return color.r, color.g, color.b
				end,
			set = function ( _, r, g, b)
					local color = addon.db.profile.sprayedcolor4
					color.r = r
					color.g = g
					color.b = b
				end,
		},
		newline1 = {
			type = "description",
			name = "\n",
			order = 550,
		},
		safecolor = {
			type = "color",
			name = L["Suggested safe area color"],
			order = 600,
			hasAlpha = false,
			get = function ()
					local color = addon.db.profile.safecolor
					return color.r, color.g, color.b
				end,
			set = function ( _, r, g, b)
					local color = addon.db.profile.safecolor
					color.r = r
					color.g = g
					color.b = b
				end,
		},
		currentpiecolor = {
			type = "color",
			name = L["Current pie color"],
			desc = L["The color of the pie you are currently inside"],
			order = 700,
			hasAlpha = false,
			get = function ()
					local color = addon.db.profile.currentpiecolor
					return color.r, color.g, color.b
				end,
			set = function ( _, r, g, b)
					local color = addon.db.profile.currentpiecolor
					color.r = r
					color.g = g
					color.b = b
				end,
		},
	},
}

addon:RegisterEvent("ZONE_CHANGED_NEW_AREA")
addon:RegisterEvent("ADDON_LOADED")
addon:SetScript("OnEvent", function(self, event, ...)
	if event == "ADDON_LOADED" then
		if (...) == "ShaOfFearAssist" then
			LibStub("AceTimer-3.0"):Embed(addon)
			self.db = LibStub("AceDB-3.0"):New("ShaAssistDB", defaults, true)
			LibStub("AceConfig-3.0"):RegisterOptionsTable("ShaOfFearAssist", options)
			LibStub("AceConfigDialog-3.0"):AddToBlizOptions("ShaOfFearAssist", "ShaOfFearAssist")
			SlashCmdList.ShaAssist = slashCommand
			SLASH_ShaAssist1 = "/sha"
			detectInstanceChange()
			self:UnregisterEvent("ADDON_LOADED")
		end
	elseif event == "COMBAT_LOG_EVENT_UNFILTERED" then
		CLEU(...)
	elseif event == "ZONE_CHANGED_NEW_AREA" then
		detectInstanceChange()
	elseif event == "PLAYER_REGEN_ENABLED" and windowShown then
		checkForWipe()
	end
end)

local total = 0
addon:SetScript("OnUpdate", function(self, elapsed)
	total = total + elapsed
	if total > 0.05 then
		if windowShown then
			addon:updateData()
		end
		total = 0
	end
end)

