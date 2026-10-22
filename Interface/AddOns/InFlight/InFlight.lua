-- GLOBALS -> LOCAL
local _G = getfenv(0)
local InFlight, self = InFlight, InFlight
local NumTaxiNodes, TaxiNodeGetType, TaxiNodeName, UnitOnTaxi, GetTime = NumTaxiNodes, TaxiNodeGetType, TaxiNodeName, UnitOnTaxi, GetTime
local format, strsub, floor = format, strsub, floor
local gtt = GameTooltip
local oldTakeTaxiNode
local GetGuildLevel = _G.GetGuildLevel
local IsInGuild = _G.IsInGuild

-- LIBRARIES
local smed = LibStub("LibSharedMedia-3.0")

-- LOCAL VARIABLES
local vars, db  -- addon
local source, destination, endTime  -- location data
local porttaken, takeoff, ontaxi, inworld  -- flags
local rat, endText  -- cache variables
local sb, spark, timeText, locText, bord  -- frame elements
local totalTime, startTime, elapsed, throt = 0, 0, 0, 0  -- throttle vars
local landingElapsed = 0
local pendingCheck = nil

local function HasGuildPerk()
	if IsInGuild and IsInGuild() and GetGuildLevel then
		local glvl = GetGuildLevel()
		return glvl and glvl >= 21
	end
	return false
end

-- LOCALIZATION 
local gl = GetLocale()
local L_destparse = ", (.+)"  -- removes main zone name, leaving only subzone
local L_duration = "Duration: "
local L_tooltipoption2 = " <Shift left-click> to move."
local L_tooltipoption3 = " <Right-click> for options."
local L_confirmpopup = "Take flight to |cffffff00%s%s|r?"
local L_BarOptions = "Bar Options"
local L_FillUp = "Fill Up"
local L_Texture = "Texture"
local L_Width = "Width"
local L_Height = "Height"
local L_Border = "Border"
local L_FillColor = "Fill Color"
local L_UnknownColor = "Unknown Color"
local L_BackgroundColor = "Background Color"
local L_BorderColor = "Border Color"
local L_TextOptions = "Text Options"
local L_CompactMode = "Compact Mode"
local L_ToText = "\"To\" Text"
local L_Font = "Font"
local L_FontColor = "Font Color"
local L_ConfirmFlight = "Confirm Flight"
if gl == "enUS" then
	-- do nada and skip ifelse chain
elseif gl == "deDE" then  -- provided by Brobar
	--L_destparse = ", (.+)"  -- removes main zone name, leaving only subzone
	L_duration = "Flugdauer: "
	L_tooltipoption2 = " <Shift Links-Klick> zum Bewegen."
	L_tooltipoption3 = " <Rechts-Klick> f\195\188r Optionen."
	L_confirmpopup = "Flug nehmen zu |cffffff00%s%s|r?"
	L_BarOptions = "Statusoptionen"
	L_FillUp = "F\195\188llen"
	L_Texture = "Textur"
	L_Width = "Breite"
	L_Height = "H\195\182he"
	L_Border = "Rand"
	L_FillColor = "F\195\188llfarbe"
	L_UnknownColor = "Unbekannte Farbe"
	L_BackgroundColor = "Hintergrundfarbe"
	L_BorderColor = "Randfarbe"
	L_TextOptions = "Textoptionen"
	L_CompactMode = "Kompaktmodus"
	L_ToText = "\"Nach\" Text"
	L_Font = "Schriftart"
	L_FontColor = "Schriftfarbe"
	L_ConfirmFlight = "Flug best\195\164tigen"
elseif gl == "koKR" then
	--L_destparse = ", (.+)"  -- removes main zone name, leaving only subzone
	L_duration = "지속시간: "
	L_tooltipoption2 = " <쉬프트+클릭> 하면 이동합니다."
	L_tooltipoption3 = " <오른쪽-클릭> 하면 설정을 엽니다."
	--L_confirmpopup = "Take flight to |cffffff00%s%s|r?"
	L_BarOptions = "바 설정"
	L_FillUp = "바 채우기"
	L_Texture = "바 텍스처"
	L_Width = "길이"
	L_Height = "높이"
	L_Border = "테두리"
	L_FillColor = "바 색상"
	L_UnknownColor = "모르는 경로 색상"
	L_BackgroundColor = "배경 색상"
	L_BorderColor = "테두리 색상"
	L_TextOptions = "글자 설정"
	L_CompactMode = "간단 모드"
	-- L_ToText = "\"To\" Text"
	L_Font = "글꼴"
	L_FontColor = "글꼴 색상"
	L_ConfirmFlight = "경로 확인"
elseif gl == "zhTW" then
	L_destparse = "，(.+)"  -- removes main zone name, leaving only subzone
	L_duration = "時間: "
	L_tooltipoption2 = " Shift-左擊: 移動"
	L_tooltipoption3 = " 右擊: 打開設定選單"
	L_confirmpopup = "你確定你要飛到|cffffff00%s%s|r?"
	L_BarOptions = "外觀"
	L_FillUp = "遞增"
	L_Texture = "時間條紋理"
	L_Width = "寬度"
	L_Height = "高度"
	L_Border = "邊框"
	L_FillColor = "時間條顏色"
	L_UnknownColor = "未知顏色"
	L_BackgroundColor = "背景顏色"
	L_BorderColor = "邊框顏色"
	L_TextOptions = "字形"
	L_CompactMode = "內嵌模式"
	L_ToText = "「到」文字"
	L_Font = "字形"
	L_FontColor = "字形顏色"
	L_ConfirmFlight = "確定飛行"
elseif gl == "esES" then
	--L_destparse = ", (.+)"  -- removes main zone name, leaving only subzone
	L_duration = "Duración: "
	L_tooltipoption2 = " <Shift+Click-Izquierdo> para mover."
	L_tooltipoption3 = " <Click-Derecho> para Opciones."
	L_confirmpopup = "¿Coger un vuelo hacia |cffffff00%s%s|r?"
	L_BarOptions = "Opciones de la Barra"
	L_FillUp = "Rellenar"
	L_Texture = "Textura"
	L_Width = "Ancho"
	L_Height = "Alto"
	L_Border = "Borde"
	L_FillColor = "Color de la Barra"
	L_UnknownColor = "Color de Desconocido"
	L_BackgroundColor = "Color de Fondo"
	L_BorderColor = "Color de Borde"
	L_TextOptions = "Opciones del Texto"
	L_CompactMode = "Modo Compacto"
	L_ToText = "Texto \"Hacia\""
	L_Font = "Fuente"
	L_FontColor = "Color"
	L_ConfirmFlight = "Confirmar Vuelo"
elseif gl == "ruRU" then  -- Translated by StingerSoft (Эритнулл ака Шептун)
	--L_destparse = ", (.+)"  -- удаляет название основной зоны, оставляя только подзону
	L_duration = "Продолжительность полета: "
	L_tooltipoption2 = " <Shift левый-клик> для перемещения."
	L_tooltipoption3 = " <Правый-клик> для опций."
	L_confirmpopup = "Лететь в |cffffff00%s%s|r?"
	L_BarOptions = "Опции полосы"
	L_FillUp = "Заполнять"
	L_Texture = "Текстура"
	L_Width = "Ширина"
	L_Height = "Высота"
	L_Border = "Края"
	L_FillColor = "Цвет заполнения"
	L_UnknownColor = "Неизвестный цвет"
	L_BackgroundColor = "Цвет фона"
	L_BorderColor = "Цвет края"
	L_TextOptions = "Опции текста"
	L_CompactMode = "Компактный режим"
	L_ToText = "\"в\" Текст"
	L_Font = "Шрифт"
	L_FontColor = "Цвет шрифта"
	L_ConfirmFlight = "Подтверждать полет"
elseif gl == "frFR" then
	L_destparse = ", (.+)" -- removes main zone name, leaving only subzone
	L_duration = "Durée: "
	L_tooltipoption2 = " <Shift clic gauche> pour se déplacer."
	L_tooltipoption3 = " <clic droit> f\195\188r Options."
	L_confirmpopup = "Prenez votre envol pour |cffffff00%s%s|r?"
	L_BarOptions = "Status Options"
	L_FillUp = "compléter"
	L_Texture = "Texture"
	L_Width = "Largeur"
	L_Height = "Hauteur"
	L_Border = "Bordure"
	L_FillColor = "Couleur de remplissage"
	L_UnknownColor = "couleur Inconnu"
	L_BackgroundColor = "Couleur de fond"
	L_BorderColor = "Couleur de la bordure"
	L_TextOptions = "Options du texte"
	L_CompactMode = "Compact Mode"
	L_ToText = "\"Après\" Text"
	L_Font = "Font"
	L_FontColor = "Couleur de font"
	L_ConfirmFlight = "Confirmer votre vol"
end

-- LOCAL FUNCTIONS
local function FormatTime(secs, f)  -- simple time format
	if not f then
		return format("%d:%02d", secs / 60, secs % 60)
	end
	f:SetFormattedText("%d:%02d / %s", secs / 60, secs % 60, endText)
end
local function ShortenName(name)  -- shorten name to lighten saved vars and display
	return gsub(name, L_destparse, "")
end
local function SetPoints(f, lp, lrt, lrp, lx, ly, rp, rrt, rrp, rx, ry)
	f:ClearAllPoints()
	f:SetPoint(lp, lrt, lrp, lx, ly)
	if rp then f:SetPoint(rp, rrt, rrp, rx, ry) end
end
local function SetToUnknown()  -- setup bar for flights with unknown time
	sb:SetValue(1)
	sb:SetStatusBarColor(db.unknowncolor.r, db.unknowncolor.g, db.unknowncolor.b, db.unknowncolor.a)
	endText = "??"
	spark:Hide()
end
local function GetEstimatedTime(slot)  -- estimates flight times based on hops
	local etime, numnodes = 0, NumTaxiNodes()
	for hop = 1, GetNumRoutes(slot), 1 do
		local s, sx, sy = nil, floor(100 * TaxiGetSrcX(slot, hop)), floor(100 * TaxiGetSrcY(slot, hop))
		local d, dx, dy = nil, floor(100 * TaxiGetDestX(slot, hop)), floor(100 * TaxiGetDestY(slot, hop))
		for i = 1, numnodes, 1 do
			local ix, iy = TaxiNodePosition(i)
			ix, iy = floor(100 * ix), floor(100 * iy)
			if not s and sx == ix and sy == iy then
				s = ShortenName(TaxiNodeName(i))
			end
			if not d and dx == ix and dy == iy then
				d = ShortenName(TaxiNodeName(i))
			end
			if s and d then break end
		end
		if s and d and vars and vars[s] and vars[s][d] then
			etime = etime + vars[s][d]
		else
			return nil
		end
	end
	return etime
end
local function postTaxiNodeOnButtonEnter(button) -- adds duration info to node tooltips
	local id = button:GetID()
	if TaxiNodeGetType(id) ~= "REACHABLE" then return end
	
	local ftime = (vars and vars[source] and vars[source][ ShortenName(TaxiNodeName(id)) ]) or GetEstimatedTime(id) or 0
    if ftime > 0 then
        if HasGuildPerk() then
            ftime = ftime / 1.25
        end
        gtt:AddLine(L_duration..FormatTime(ftime), 1, 1, 1)
    else
        gtt:AddLine(L_duration.."-:--", 0.8, 0.8, 0.8)
    end
	gtt:Show()
end

----------------------------
function InFlight:LoadBulk()  -- called from InFlight_Load
----------------------------
	InFlightDB = InFlightDB or {}
	if InFlightDB.profiles then 
		InFlightDB = InFlightDB.profiles.Default
		InFlightDB.profiles = nil
	end
	db = InFlightDB.perchar and InFlightCharDB or InFlightDB

	InFlightVars = InFlightVars or { Alliance = {}, Horde = {}, Neutral = {}, }  -- flight time data
	local faction = UnitFactionGroup("player") or "Neutral"
	InFlightVars[faction] = InFlightVars[faction] or {}
	vars = InFlightVars[faction]

	if db.dbinit ~= 8 then
		db.dbinit = 8
		local function SetDefaults(db, t)  -- set saved variables
			for k,v in pairs(t) do
				if type(db[k]) == "table" then
					SetDefaults(db[k], v)
				else
					db[k] = (db[k] ~= nil and db[k]) or v
				end
			end
		end
		SetDefaults(db, {
			confirmflight = false,
			fill = true,
			inline = false,
			border = "Blizzard Dialog",
			height = 14,
			width = 230,
			font = "Friz Quadrata TT",
			fontsize = 12,
			texture = "Blizzard",
			barcolor = { r = 0.5, g = 0.5, b = 0.8, a = 1.0, },
			unknowncolor = { r = 0.2, g = 0.2, b = 0.4, a = 1.0, },
			bordercolor = { r = 0.6, g = 0.6, b = 0.6, a = 0.8, },
			backcolor = { r = 0.1, g = 0.1, b = 0.1, a = 0.6, },
			fontcolor = { r = 1.0, g = 1.0, b = 1.0, a = 1.0, },
			totext = " --> ",
		} )
		if self.LoadDefaults then  -- updates to default data
			for faction, t in pairs(self:LoadDefaults()) do
				for source, dt in pairs(t) do
					InFlightVars[faction][source] = InFlightVars[faction][source] or { }
					for destination, dtime in pairs(dt) do
						local vtime = InFlightVars[faction][source][destination]
						if not vtime or vtime > dtime then
							InFlightVars[faction][source][destination] = dtime
						end
					end
				end
			end
		end
	end
	self.LoadDefaults = nil

	oldTakeTaxiNode = TakeTaxiNode
	TakeTaxiNode = function(slot)
		if TaxiNodeGetType(slot) ~= "REACHABLE" then return end
		source = source or ShortenName(GetMinimapZoneText() or GetSubZoneText() or GetZoneText())
		destination = ShortenName(TaxiNodeName(slot))
		local t = vars and vars[source]
		if t and t[destination] and t[destination] > 0 then  -- saved variables lookup
			endTime = t[destination]
		else
			endTime = GetEstimatedTime(slot)
		end
		if endTime and HasGuildPerk() then
			endTime = endTime / 1.25
		end
		if db.confirmflight then  -- confirm flight
			StaticPopupDialogs.INFLIGHTCONFIRM = StaticPopupDialogs.INFLIGHTCONFIRM or {
				button1 = OKAY, button2 = CANCEL,
				OnAccept = function(this, data) InFlight:StartTimer(data) end,
				timeout = 0, exclusive = 1, hideOnEscape = 1,
			}
			StaticPopupDialogs.INFLIGHTCONFIRM.text = format(L_confirmpopup, destination, endTime and format(" (%s)", FormatTime(endTime)) or "")

			local dialog = StaticPopup_Show("INFLIGHTCONFIRM")
			if dialog then 
				dialog.data = slot 
			end
		else  -- just take the flight
			self:StartTimer(slot)
		end
	end

	-- function hooks to detect if a user took a summon
	hooksecurefunc("AcceptBattlefieldPort", function(index, accept) porttaken = accept and true end)
	hooksecurefunc("ConfirmSummon", function() porttaken = true end)
	hooksecurefunc("CompleteLFGRoleCheck", function(bool) porttaken = bool end)

	self:Hide()
	self.LoadBulk = nil
end

------------------------------
function InFlight:InitSource()  -- cache source location and hook tooltips
------------------------------
	for i = 1, NumTaxiNodes(), 1 do
		local tb = _G["TaxiButton"..i]
		if tb and not tb.inflighted then
			tb:HookScript("OnEnter", postTaxiNodeOnButtonEnter)
			tb.inflighted = true
		end
		if TaxiNodeGetType(i) == "CURRENT" then
			source = ShortenName(TaxiNodeName(i))
		end
	end
	source = source or ShortenName(GetMinimapZoneText() or GetSubZoneText() or GetZoneText())
end

----------------------------------
function InFlight:StartTimer(slot)  -- lift off
----------------------------------
	if slot then
		Dismount()
		if CanExitVehicle and CanExitVehicle() == 1 then
			VehicleExit()
		end
	end
	if not sb then  -- create the timer bar
		self:CreateBar()
	end
	locText:SetFormattedText("%s%s%s", not db.inline and source or "", not db.inline and db.totext or "", destination or "")
	if endTime then  -- start the timers and setup statusbar
		sb:SetValue((db.fill and 0) or 1)
		sb:SetStatusBarColor(db.barcolor.r, db.barcolor.g, db.barcolor.b, db.barcolor.a)
		spark:Show()
		rat = sb:GetWidth() / endTime
		endText = FormatTime(endTime)
	else
		SetToUnknown()
	end
	porttaken = nil
	elapsed, totalTime, startTime = 0, 0, GetTime()
	takeoff, inworld = true, true
	ontaxi = nil
	landingElapsed = 0
	pendingCheck = nil
	throt = min(0.2, (endTime or 50) * 0.004)  -- increases updates for short flights
	self:RegisterEvent("PLAYER_ENTERING_WORLD")
	self:RegisterEvent("PLAYER_LEAVING_WORLD")
	self:RegisterEvent("PLAYER_CONTROL_GAINED")
	self:Show()
	
	if slot then
		oldTakeTaxiNode(slot)
	end
end

---------------------------------------
function InFlight:StartMiscFlight(s, d)  -- called from InFlight_Load for special flights
---------------------------------------
	self:TAXIMAP_OPENED(nil, true)
	endTime = vars and vars[s] and vars[s][d]
	source, destination = s, d
	self:StartTimer()
end

----------------------------------
function InFlight:StartTaxiFlight()  -- fallback when on flight but timer not active
----------------------------------
	if ontaxi or takeoff or (sb and sb:IsShown()) then return end
	if not UnitOnTaxi("player") then return end

	source = source or self.lastGossipSource or ShortenName(GetMinimapZoneText() or GetZoneText() or "Taxi")
	destination = destination or "..."
	local t = vars and vars[source]
	if t and t[destination] and t[destination] > 0 then
		endTime = t[destination]
	else
		endTime = nil
	end
	if endTime and HasGuildPerk() then
		endTime = endTime / 1.25
	end

	self:StartTimer()
end

------------------------------------
function InFlight:CheckPendingTaxi()  -- checks for taxi state after control lost
------------------------------------
	if ontaxi or takeoff or (sb and sb:IsShown()) then return end
	if UnitOnTaxi("player") then
		self:StartTaxiFlight()
		return
	end
	if not sb then
		self:CreateBar()
	end
	pendingCheck = GetTime() + 3.0
	inworld = true
	self:Show()
end

do  -- timer bar
	local bdrop = { edgeSize = 16, insets = {}, }
	local bdi = bdrop.insets

	local function EndFlight()
		if not self:IsShown() and not (sb and sb:IsShown()) then return end

		self:UnregisterEvent("PLAYER_CONTROL_GAINED")

		if ontaxi and not porttaken and source then
			if destination == "..." then
				local landZone = ShortenName(GetMinimapZoneText() or GetSubZoneText() or GetZoneText())
				if landZone and landZone ~= "" and landZone ~= source then
					destination = landZone
				end
			end

			if destination and destination ~= "..." and totalTime >= 3 then
				local duration = totalTime
				if HasGuildPerk() then
					duration = duration * 1.25
				end
				if vars then
					vars[source] = vars[source] or {}
					vars[source][destination] = floor(duration + 0.5)
				end
			end
		end

		takeoff, ontaxi = nil, nil
		endTime = nil
		landingElapsed = 0
		pendingCheck = nil

		if sb then
			sb:Hide()
		end
		self:Hide()
	end
	self.EndFlight = EndFlight

	-----------------------------
	function InFlight:CreateBar()
	-----------------------------
		sb = CreateFrame("StatusBar", "InFlightBar", UIParent)
		sb:Hide()
		sb:SetPoint(db.p or "TOP", UIParent, db.rp or "BOTTOMLEFT", db.x or (GetScreenWidth()/2), db.y or (GetScreenHeight()-200))
		sb:SetMovable(true)
		sb:EnableMouse(true)
		sb:SetScript("OnMouseUp", function(this, a1) if a1 == "RightButton" then InFlight:ShowOptions() end end)
		sb:RegisterForDrag("LeftButton")
		sb:SetScript("OnDragStart", function(this) if IsShiftKeyDown() then this:StartMoving() end end)
		sb:SetScript("OnDragStop", function(this) 
			this:StopMovingOrSizing() 
			local a,b,c,d,e = this:GetPoint()
			db.p, db.rp, db.x, db.y = a, c, floor(d + 0.5), floor(e + 0.5)
		end)
		sb:SetScript("OnEnter", function(this)
			gtt:SetOwner(this, "ANCHOR_RIGHT")
			gtt:SetText("InFlight", 1, 1, 1)
			gtt:AddLine(L_tooltipoption2, 0, 1, 0)
			gtt:AddLine(L_tooltipoption3, 0, 1, 0)
			gtt:Show()
		end)
		sb:SetScript("OnLeave", function() gtt:Hide() end)
		sb:SetMinMaxValues(0, 1)

		timeText = sb:CreateFontString(nil, "OVERLAY")
		locText = sb:CreateFontString(nil, "OVERLAY")

		spark = sb:CreateTexture(nil, "OVERLAY")
		spark:SetTexture("Interface\\CastingBar\\UI-CastingBar-Spark")
		spark:SetWidth(16)
		spark:SetBlendMode("ADD")

		bord = CreateFrame("Frame", nil, sb)  -- border/background
		SetPoints(bord, "TOPLEFT", sb, "TOPLEFT", -5, 5, "BOTTOMRIGHT", sb, "BOTTOMRIGHT", 5, -5)
		bord:SetFrameStrata("LOW")

		InFlight:UpdateLook()

		local function onupdate(this, a1)
			if not inworld then return end

			elapsed = elapsed + a1
			if elapsed < throt then return end
			elapsed = 0

			if pendingCheck then
				if UnitOnTaxi("player") then
					pendingCheck = nil
					self:StartTaxiFlight()
					return
				elseif GetTime() > pendingCheck then
					pendingCheck = nil
					if not ontaxi and not takeoff then
						this:Hide()
					end
					return
				end
			end

			totalTime = GetTime() - startTime

			if takeoff then  -- check if actually in flight after take off (doesn't happen immediately)
				if UnitOnTaxi("player") then
					takeoff, ontaxi = nil, true
					landingElapsed = 0
					sb:Show()
					totalTime = 0
					startTime = GetTime()
				elseif totalTime > 10 then  -- allow up to 10s for taxi takeoff on laggy servers
					EndFlight()
				end
				return
			end

			if ontaxi then
				if not UnitOnTaxi("player") then
					landingElapsed = landingElapsed + throt
					if landingElapsed >= 0.4 then  -- player has landed
						EndFlight()
						return
					end
				else
					landingElapsed = 0
				end

				if endTime then  -- update statusbar if destination time is known
					if totalTime - 2 > endTime then   -- in case the flight is longer than expected
						SetToUnknown()
						endTime = nil
					else
						local value = (db.fill and totalTime) or (endTime - totalTime)
						sb:SetValue(value / endTime)
						spark:SetPoint("CENTER", sb, "LEFT", value * rat, 0)
						FormatTime(value, timeText)
					end
				else  -- destination time is unknown, so show that it's timing
					FormatTime(totalTime, timeText)
				end
			end
		end

		function self:PLAYER_LEAVING_WORLD()
			inworld = nil
		end
		function self:PLAYER_ENTERING_WORLD()
			inworld = true
			if not UnitOnTaxi("player") and ontaxi then
				EndFlight()
			elseif UnitOnTaxi("player") and not ontaxi and not takeoff then
				self:StartTaxiFlight()
			end
		end
		function self:PLAYER_CONTROL_GAINED()
			if not inworld then return end
			if not self:IsShown() then return end
			if takeoff then return end  -- ignore control events while still preparing for takeoff

			if ontaxi and not UnitOnTaxi("player") then
				EndFlight()
			end
		end
		function self:PLAYER_CONTROL_LOST()
			if not inworld then return end
			if ontaxi or takeoff or (sb and sb:IsShown()) then return end
			if UnitOnTaxi("player") then
				self:StartTaxiFlight()
			else
				self:CheckPendingTaxi()
			end
		end

		self:SetScript("OnUpdate", onupdate)
		self.CreateBar = nil
	end
	------------------------------
	function InFlight:UpdateLook()
	------------------------------
		if not sb then return end

		sb:SetWidth(db.width)
		sb:SetHeight(db.height)
		
		local texture = smed:Fetch("statusbar", db.texture)
		local inset = (db.border=="Textured" and 2) or 4
		bdrop.bgFile = texture
		bdrop.edgeFile = smed:Fetch("border", db.border)
		bdi.left, bdi.right, bdi.top, bdi.bottom = inset, inset, inset, inset
		bord:SetBackdrop(bdrop)
		bord:SetBackdropColor(db.backcolor.r, db.backcolor.g, db.backcolor.b, db.backcolor.a)
		bord:SetBackdropBorderColor(db.bordercolor.r, db.bordercolor.g, db.bordercolor.b, db.bordercolor.a)
		sb:SetStatusBarTexture(texture)
		if sb:GetStatusBarTexture() then
			sb:GetStatusBarTexture():SetHorizTile(false)
			sb:GetStatusBarTexture():SetVertTile(false)
		end
		if endTime then  -- in case we're in flight
			rat = db.width / endTime
			sb:SetStatusBarColor(db.barcolor.r, db.barcolor.g, db.barcolor.b, db.barcolor.a)
		else
			SetToUnknown()
		end
		spark:SetHeight(db.height*2.4)
		
		locText:SetFont(smed:Fetch("font", db.font), db.fontsize, db.outline and "OUTLINE" or nil)
		locText:SetShadowColor(0, 0, 0, db.fontcolor.a)
		locText:SetShadowOffset(1, -1)
		locText:SetTextColor(db.fontcolor.r, db.fontcolor.g, db.fontcolor.b, db.fontcolor.a)
		
		timeText:SetFont(smed:Fetch("font", db.font), db.fontsize, db.outlinetime and "OUTLINE" or nil)
		timeText:SetShadowColor(0, 0, 0, db.fontcolor.a)
		timeText:SetShadowOffset(1, -1)
		timeText:SetTextColor(db.fontcolor.r, db.fontcolor.g, db.fontcolor.b, db.fontcolor.a)

		if db.inline then
			timeText:SetJustifyH("RIGHT")
			timeText:SetJustifyV("CENTER")
			SetPoints(timeText, "RIGHT", sb, "RIGHT", -4, 0)
			locText:SetJustifyH("LEFT")
			locText:SetJustifyV("CENTER")
			SetPoints(locText, "LEFT", sb, "LEFT", 4, 0, "RIGHT", timeText, "LEFT", -2, 0)
		else
			timeText:SetJustifyH("CENTER")
			timeText:SetJustifyV("CENTER")
			SetPoints(timeText, "CENTER", sb, "CENTER", 0, 0)
			locText:SetJustifyH("CENTER")
			locText:SetJustifyV("BOTTOM")
			SetPoints(locText, "TOPLEFT", sb, "TOPLEFT", -24, db.fontsize*2.5, "BOTTOMRIGHT", sb, "TOPRIGHT", 24, (db.border=="None" and 1) or 3)
		end	
		locText:SetFormattedText("%s%s%s", not db.inline and source or "", not db.inline and db.totext or "", destination or "")
	end
end

---------------------------------
function InFlight:SetLayout(this)  -- setups the options in the default interface options
---------------------------------
	local t1 = this:CreateFontString(nil, "ARTWORK")
	t1:SetFontObject(GameFontNormalLarge)
	t1:SetJustifyH("LEFT")
	t1:SetJustifyV("TOP")
	t1:SetPoint("TOPLEFT", 16, -16)
	t1:SetText("InFlight")
	this.tl = t1
	
	local t2 = this:CreateFontString(nil, "ARTWORK")
	t2:SetFontObject(GameFontHighlightSmall)
	t2:SetJustifyH("LEFT")
	t2:SetJustifyV("TOP")
	t2:SetHeight(43)
	SetPoints(t2, "TOPLEFT", t1, "BOTTOMLEFT", 0, -8, "RIGHT", this, "RIGHT", -32, 0)
	t2:SetNonSpaceWrap(true)
	local function GetInfo(field)
		return GetAddOnMetadata("InFlight", field) or "N/A"
	end
	t2:SetFormattedText("Notes: %s\nAuthor: %s\nVersion: %s", GetInfo("Notes"), GetInfo("Author"), GetInfo("Version"))

	local b = CreateFrame("Button", nil, this, "UIPanelButtonTemplate")
	b:SetWidth(120)
	b:SetHeight(20)
	b:SetText(_G.GAMEOPTIONS_MENU)
	b:SetScript("OnClick", InFlight.ShowOptions)
	b:SetPoint("TOPLEFT", t2, "BOTTOMLEFT", -2, -8)
	this:SetScript("OnShow", nil)
	self.SetLayout = nil
end

-- options table
smed:Register("border", "Textured", "\\Interface\\None")  -- dummy border
local InFlightDD, offsetvalue, offsetcount, lastb
local info = { }
-------------------------------
function InFlight.ShowOptions()
-------------------------------
	if not InFlightDD then
		InFlightDD = CreateFrame("Frame", "InFlightDD", InFlight)
		InFlightDD.displayMode = "MENU"
		local function HideCheck(b)
			if b and b.GetName and _G[b:GetName().."Check"] then
				_G[b:GetName().."Check"]:Hide()
			end
		end
		hooksecurefunc("ToggleDropDownMenu", function(...) lastb = select(8, ...) end)
		local function Exec(b, k, value)
			HideCheck(b)
			if k == "totext" then
				StaticPopupDialogs["InFlightToText"] = StaticPopupDialogs["InFlightToText"] or {
					text = "Set your busy message.",
					button1 = ACCEPT, button2 = CANCEL,
					hasEditBox = 1, maxLetters = 12,
					OnAccept = function(self)
						db.totext = self.editBox:GetText()
						InFlight:UpdateLook()
					end,
					OnShow = function(self)
						self.editBox:SetText(db.totext)
						self.editBox:SetFocus()
					end,
					OnHide = function(self)
						self.editBox:SetText("")
					end,
					EditBoxOnEnterPressed = function(self)
						local parent = self:GetParent()
						db.totext = parent.editBox:GetText()
						parent:Hide()
						InFlight:UpdateLook()
					end,
					EditBoxOnEscapePressed = function(self)
						self:GetParent():Hide()
					end,
					timeout = 0, exclusive = 1, whileDead = 1, hideOnEscape = 1,
				}
				StaticPopup_Show("InFlightToText")
			elseif (k == "less" or k == "more") and lastb then
				local off = (k == "less" and -8) or 8
				if offsetvalue == value then
					offsetcount = offsetcount + off
				else
					offsetvalue, offsetcount = value, off
				end
				local tb = _G[gsub(lastb:GetName(), "ExpandArrow", "")]
				CloseDropDownMenus(b:GetParent():GetID())
				ToggleDropDownMenu(b:GetParent():GetID(), tb.value, nil, nil, nil, nil, tb.menuList, tb)
			elseif k == "resetall" then
				InFlightDB, InFlightCharDB = nil, nil
				ReloadUI()
			end
		end
		local function Set(b, k)
			if not k then return end
			db[k] = not db[k]
			if k == "perchar" then
				if db[k] then
					InFlightCharDB = db
				else
					InFlightCharDB = nil
				end
				ReloadUI()
			end
			InFlight:UpdateLook()
		end
		local function SetSelect(b, a1)
			db[a1] = tonumber(b.value) or b.value
			local level, num = strmatch(b:GetName(), "DropDownList(%d+)Button(%d+)")
			level, num = tonumber(level) or 0, tonumber(num) or 0
			for i = 1, UIDROPDOWNMENU_MAXBUTTONS, 1 do
				local b = _G["DropDownList"..level.."Button"..i.."Check"]
				if b then
					b[i == num and "Show" or "Hide"](b)
				end
			end
			InFlight:UpdateLook()
		end
		local function SetColor(a1)
			local dbc = db[UIDROPDOWNMENU_MENU_VALUE]
			if not dbc then return end
			if a1 then
				local pv = ColorPickerFrame.previousValues
				dbc.r, dbc.g, dbc.b, dbc.a = pv.r, pv.g, pv.b, 1 - pv.opacity
			else
				dbc.r, dbc.g, dbc.b = ColorPickerFrame:GetColorRGB()
				dbc.a = 1 - OpacitySliderFrame:GetValue()
			end
			InFlight:UpdateLook()
		end
		local function AddButton(lvl, text, keepshown)
			info.text = text
			info.keepShownOnClick = keepshown
			UIDropDownMenu_AddButton(info, lvl)
			wipe(info)
		end
		local function AddToggle(lvl, text, value)
			info.arg1 = value
			info.func = Set
			info.checked = db[value]
			info.isNotRadio = true
			AddButton(lvl, text, 1)
		end
		local function AddExecute(lvl, text, arg1, arg2)
			info.arg1 = arg1
			info.arg2 = arg2
			info.func = Exec
			info.notCheckable = 1
			AddButton(lvl, text, 1)
		end
		local function AddColor(lvl, text, value)
			local dbc = db[value]
			if not dbc then return end
			info.hasColorSwatch = true
			info.hasOpacity = 1
			info.r, info.g, info.b, info.opacity = dbc.r, dbc.g, dbc.b, 1 - dbc.a
			info.swatchFunc, info.opacityFunc, info.cancelFunc = SetColor, SetColor, SetColor
			info.value = value
			info.notCheckable = 1
			info.func = UIDropDownMenuButton_OpenColorPicker
			AddButton(lvl, text, nil)
		end
		local function AddList(lvl, text, value)
			info.value = value
			info.hasArrow = true
			info.func = HideCheck
			info.notCheckable = 1
			AddButton(lvl, text, 1)
		end
		local function AddSelect(lvl, text, arg1, value)
			info.arg1 = arg1
			info.func = SetSelect
			info.value = value
			if tonumber(value) and tonumber(db[arg1] or "blah") then
				if floor(100 * tonumber(value)) == floor(100 * tonumber(db[arg1])) then
					info.checked = true
				end
			else
				info.checked = (db[arg1] == value)
			end
			AddButton(lvl, text, 1)
		end
		local function AddFakeSlider(lvl, value, minv, maxv, step, tbl)
			local cvalue = 0
			local dbv = db[value]
			if type(dbv) == "string" and tbl then
				for i, v in ipairs(tbl) do
					if dbv == v then
						cvalue = i
						break
					end
				end
			else
				cvalue = dbv or ((maxv - minv) / 2)
			end
			local adj = (offsetvalue == value and offsetcount) or 0
			local starti = max(minv, cvalue - (7 - adj) * step)
			local endi = min(maxv, cvalue + (8 + adj) * step)
			if starti == minv then
				endi = min(maxv, starti + 16 * step)
			elseif endi == maxv then
				starti = max(minv, endi - 16 * step)
			end
			if starti > minv then
				AddExecute(lvl, "--", "less", value)
			end
			if tbl then
				for i = starti, endi, step do
					AddSelect(lvl, tbl[i], value, tbl[i])
				end
			else
				local fstring = (step >= 1 and "%d") or (step >= 0.1 and "%.1f") or "%.2f"
				for i = starti, endi, step do
					AddSelect(lvl, format(fstring, i), value, i)
				end
			end
			if endi < maxv then
				AddExecute(lvl, "++", "more", value)
			end
		end
		InFlightDD.initialize = function(self, lvl)
			if lvl == 1 then
				info.isTitle = true
				info.notCheckable = 1
				AddButton(lvl, "|cffaaaaffIn|r|cffddddffFlight|r")
				AddList(lvl, L_BarOptions, "frame")
				AddList(lvl, L_TextOptions, "text")
				AddList(lvl, _G.OTHER, "other")
			elseif lvl == 2 then
				local sub = UIDROPDOWNMENU_MENU_VALUE
				if sub == "frame" then
					AddToggle(lvl, L_FillUp, "fill")
					AddList(lvl, L_Texture, "texture")
					AddList(lvl, L_Width, "width")
					AddList(lvl, L_Height, "height")
					AddList(lvl, L_Border, "border")
					AddColor(lvl, L_FillColor, "barcolor")
					AddColor(lvl, L_UnknownColor, "unknowncolor")
					AddColor(lvl, L_BackgroundColor, "backcolor")
					AddColor(lvl, L_BorderColor, "bordercolor")
				elseif sub == "text" then
					AddToggle(lvl, L_CompactMode, "inline")
					AddExecute(lvl, L_ToText, "totext")
					AddList(lvl, L_Font, "font")
					AddList(lvl, _G.FONT_SIZE, "fontsize")
					AddColor(lvl, L_FontColor, "fontcolor")
					AddToggle(lvl, "Outline Info", "outline")
					AddToggle(lvl, "Outline Time", "outlinetime")
				elseif sub == "other" then
					AddToggle(lvl, L_ConfirmFlight, "confirmflight")
					AddToggle(lvl, _G.CHARACTER.." ".._G.SAVE, "perchar")
					AddExecute(lvl, _G.RESET_TO_DEFAULT, "resetall")
				end
			elseif lvl == 3 then
				local sub = UIDROPDOWNMENU_MENU_VALUE
				if sub == "texture" or sub == "border" or sub == "font" then
					local t = smed:List(sub == "texture" and "statusbar" or sub)
					AddFakeSlider(lvl, sub, 1, #t, 1, t)
				elseif sub == "width" then
					AddFakeSlider(lvl, sub, 40, 500, 5)
				elseif sub == "height" then
					AddFakeSlider(lvl, sub, 4, 100, 1)
				elseif sub == "fontsize" then
					AddFakeSlider(lvl, sub, 4, 30, 1)
				end
			end
		end
	end
	ToggleDropDownMenu(1, nil, InFlightDD, "cursor")
end

--[[
function inflightupdate()
	local updates = {}
	--for table, updates in pairs(updates) do
	for faction, t in pairs(updates) do
		for source, dt in pairs(t) do
			if not InFlightVars[faction][source] then
				InFlightVars[faction][source] = dt
				ChatFrame1:AddMessage("New source: "..faction.." "..source)
			end
			for destination, time in pairs(dt) do
				local vtime = InFlightVars[faction][source][destination]
				if not vtime or vtime > time then
					ChatFrame1:AddMessage("Update time: "..faction.." "..source.." "..destination.." "..time)
					InFlightVars[faction][source][destination] = time
				end
			end
		end
	end
	--end
end
]]--
