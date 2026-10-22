-- ----------------------------------------------------------------------------
-- Character Achievements 
-- 		by Nasapunk88
-- ----------------------------------------------------------------------------

CharacterAchievements = {};
CharacterAchievements_Debug = false;

local function cout(msg, premsg)
	premsg = premsg or "[".."Character Achievements".."]"
	print("|cFFE8A317"..premsg.."|r "..msg);
end

local function coutBool(msg,bool)
	if bool then
		print(msg..": true");
	else
		print(msg..": false");
	end
end

local function colorizeAchievement(button,account,character)
	--Color AchievementButtons
	if CharacterAchievements.db.isAccountWide then
		if account and (not character) then
			button:SetBackdropBorderColor(0.129,0.671,0.875,1); --blue for account
		elseif (not button.accountWide) and character then
			button:SetBackdropBorderColor(0.7,0.15,0.05,1); --red for character
		end
	else
		if character then
			button:Saturate();
		else
			button:Desaturate();
			if account then
				button:SetBackdropBorderColor(0.129,0.671,0.875,1);
				button.icon:Saturate();
				button.shield:Saturate();
				button.shield.points:SetVertexColor(1, 1, 1);
				button.glow:SetVertexColor(0.10, 0.10, 0.10);
				button.titleBar:SetTexture("Interface\\AchievementFrame\\UI-Achievement-Borders");
				button.titleBar:SetTexCoord(0, 1, 0.66015625, 0.73828125);
			end
		end	
	end
end

local function updatePoints()
	--Update Achievement Pts
	if CharacterAchievements.db.isAccountWide then
		AchievementFrameHeaderPoints:SetText(GetTotalAchievementPoints());
	else
		if (IsInGuild()) then
			for i=1,GetNumGuildMembers() do 
				if Ambiguate(select(1,GetGuildRosterInfo(i)), "none") == CharacterAchievements.db.fullName then 
					AchievementFrameHeaderPoints:SetText(select(12,GetGuildRosterInfo(i)));
					break;
				end 
			end 
		else 
			AchievementFrameHeaderPoints:SetText(CharacterAchievements.db.characterPoints);
		end
	end
end

local function postHook_Frame()
	--Post Hook Category Display depending on toggle switch
	local orig_AchievementButton_DisplayAchievement = AchievementButton_DisplayAchievement;
	function AchievementButton_DisplayAchievement(...)
		local result = orig_AchievementButton_DisplayAchievement(...);
		if AchievementFrame.selectedTab == 1 or AchievementFrame.selectedTab == 3 then
			local button = ...; --button, category, achievement, selectionID)
			local account = select(4,GetAchievementInfo(button.id));
			local character = select(13,GetAchievementInfo(button.id));
			colorizeAchievement(button,account,character);
			updatePoints();
		end
		if AchievementFrame.selectedTab == 2 then
			-- Guild header from Blizzard: fix bug switching from stats frame
			AchievementFrameHeaderPoints:SetText(GetTotalAchievementPoints(true));
			AchievementFrameHeaderPoints:SetVertexColor(0, 1, 0);
			AchievementFrameHeaderTitle:SetText(_G["GUILD_ACHIEVEMENTS_TITLE"]);
			local shield = AchievementFrameHeaderShield;
			shield:SetTexture("Interface\\AchievementFrame\\UI-Achievement-Guild");
			shield:SetTexCoord(0.63281250, 0.67187500, 0.13085938, 0.16601563);
			shield:SetHeight(18);
		end
		return result;
	end
	
	--Post Hook Summary Display
	local orig_AchievementFrameSummary_UpdateAchievements = AchievementFrameSummary_UpdateAchievements;
	function AchievementFrameSummary_UpdateAchievements(...)
		local result = orig_AchievementFrameSummary_UpdateAchievements(...);
		if AchievementFrame.selectedTab == 1 or AchievementFrame.selectedTab == 3 then
			local buttons = AchievementFrameSummaryAchievements.buttons;
			if ( buttons ) then
				for i=1, _G["ACHIEVEMENTUI_MAX_SUMMARY_ACHIEVEMENTS"] do
					local account = select(4,GetAchievementInfo(buttons[i].id));
					local character = select(13,GetAchievementInfo(buttons[i].id));
					colorizeAchievement(buttons[i],account,character);
				end
			end
			updatePoints();
		end
		if AchievementFrame.selectedTab == 2 then
			-- Guild header from Blizzard: fix bug switching from stats frame
			AchievementFrameHeaderPoints:SetText(GetTotalAchievementPoints(true));
			AchievementFrameHeaderPoints:SetVertexColor(0, 1, 0);
			AchievementFrameHeaderTitle:SetText(_G["GUILD_ACHIEVEMENTS_TITLE"]);
			local shield = AchievementFrameHeaderShield;
			shield:SetTexture("Interface\\AchievementFrame\\UI-Achievement-Guild");
			shield:SetTexCoord(0.63281250, 0.67187500, 0.13085938, 0.16601563);
			shield:SetHeight(18);
		end
		return result;
	end
	
	--Post Hook Comparison Frame Display
	local orig_AchievementFrameComparison_DisplayAchievement = AchievementFrameComparison_DisplayAchievement;
	function AchievementFrameComparison_DisplayAchievement(...)
		local result = orig_AchievementFrameComparison_DisplayAchievement(...);
		local button, category, index = ...;
		local button = button.player;
		local account = select(4,GetAchievementInfo(category, index));
		local character = select(13,GetAchievementInfo(category, index));
		colorizeAchievement(button,account,character);
		updatePoints();
		return result;
	end
	
	--Post Hook Comparison Status Bars to update completed number 
	-- local orig_AchievementFrameComparison_UpdateStatusBars = AchievementFrameComparison_UpdateStatusBars;
	-- function AchievementFrameComparison_UpdateStatusBars(id)
		-- local result = orig_AchievementFrameComparison_UpdateStatusBars(id);
		-- CharacterAchievements.db.selectedCategory = id;
		-- local numAchievements, numCompleted, numIncomplete = GetCategoryNumAchievements(id);
		-- if not CharacterAchievements.db.isAccountWide then
			-- local earned = numAchievements - numIncomplete;
			-- AchievementFrameComparisonSummaryPlayerStatusBar:SetValue(earned);
			-- AchievementFrameComparisonSummaryPlayerStatusBar.text:SetText(earned.."/"..numAchievements);
		-- end
		-- return result;
	-- end
	------------------------------------------------------------------------------
	--      Update points hookscripts
	------------------------------------------------------------------------------
	
	--Hook Frame to update points on show
	AchievementFrame:HookScript("OnShow", function(self,...) 
		if AchievementFrame.selectedTab == 1 or AchievementFrame.selectedTab == 3 then
			updatePoints();
		end
	end);
	
	--Hook Stats Frame to update points on show
	AchievementFrameStats:HookScript("OnShow", function(self,...) 
			--Fix blizzard bug: switching from guild to stats frame keeps guild achievement pts vs characters
			-- Character header from Blizzard
			AchievementFrameHeaderPoints:SetVertexColor(1, 1, 1);
			AchievementFrameHeaderTitle:SetText(_G["ACHIEVEMENT_TITLE"]);
			local shield = AchievementFrameHeaderShield;
			shield:SetTexture("Interface\\AchievementFrame\\UI-Achievement-TinyShield");
			shield:SetTexCoord(0, 0.625, 0, 0.625);
			shield:SetHeight(20);

			updatePoints();
	end);
end

local function toggleMove(bool)
	CharacterAchievements.db.isMovementEnabled = bool;
	CharacterAchievements.Interface.Movement:SetChecked(CharacterAchievements.db.isMovementEnabled);
	if bool then
		CharacterAchievements.Frame:SetMovable(true)
		CharacterAchievements.Frame:EnableMouse(true)
		CharacterAchievements.Frame:SetScript("OnMouseDown", function(self, button)
			if button == "LeftButton" and not self.isMoving then
				self:StartMoving();
				self.isMoving = true;
			end
		end)
		CharacterAchievements.Frame:SetScript("OnMouseUp", function(self, button)
			if button == "LeftButton" and self.isMoving then
				self:StopMovingOrSizing();
				self.isMoving = false;
				--Set New location relative to AchievementFrame 
				--Allows other addons to move the frame with button
				self:ClearAllPoints();
				local x = (self:GetLeft() - _G["AchievementFrameHeader"]:GetLeft());
				local y = (self:GetTop() - _G["AchievementFrameHeader"]:GetTop());
				self:SetPoint("TOPLEFT", AchievementFrameHeader, "TOPLEFT", x, y);
				CharacterAchievements.db.position = {
					point = "TOPLEFT",
					relativePoint = "TOPLEFT",
					xOffset = x,
					yOffset = y
				};
				CharacterAchievements.db.hasMoved = true;
			end
		end)
		CharacterAchievements.Frame:SetScript("OnHide", function(self)
			if self.isMoving then
				self:StopMovingOrSizing();
				self.isMoving = false;
				--Set New location relative to AchievementFrame 
				--Allows other addons to move the frame with button
				self:ClearAllPoints();
				local x = (self:GetLeft() - _G["AchievementFrameHeader"]:GetLeft());
				local y = (self:GetTop() - _G["AchievementFrameHeader"]:GetTop());
				self:SetPoint("TOPLEFT", AchievementFrameHeader, "TOPLEFT", x, y);
				CharacterAchievements.db.position = {
					point = "TOPLEFT",
					relativePoint = "TOPLEFT",
					xOffset = x,
					yOffset = y
				};
				CharacterAchievements.db.hasMoved = true;
			end
		end)
	else
		CharacterAchievements.Frame:SetMovable(false)
		CharacterAchievements.Frame:EnableMouse(false)
		CharacterAchievements.Frame:SetScript("OnMouseDown", nil)
		CharacterAchievements.Frame:SetScript("OnMouseUp", nil)
		CharacterAchievements.Frame:SetScript("OnHide", nil)
	end
end

local function toggleAccountWide(bool)
	--Checked = Account-earned, Unchecked = Character specific
	if (not IsInGuild() and not bool and CharacterAchievements.db.achievementEarned) then
		CharacterAchievements.db.accountAchievementsHidden = AreAccountAchievementsHidden();
		ShowAccountAchievements(true);
		InspectAchievements(UnitName("player"));
		CharacterAchievements.db.characterInspect = true;
	end
	CharacterAchievements.db.isAccountWide = bool;
	CharacterAchievements.CheckButton:SetChecked(CharacterAchievements.db.isAccountWide);
	CharacterAchievements.Interface.AccountWide:SetChecked(CharacterAchievements.db.isAccountWide);
	AchievementFrame_ForceUpdate();
	AchievementFrameSummary_Update();
end

local function toggleButton(bool)
	CharacterAchievements.db.isEnabled = bool;
	CharacterAchievements.Interface.EnableFrame:SetChecked(CharacterAchievements.db.isEnabled);
	if bool then
		CharacterAchievements.Frame:Show();
	else
		CharacterAchievements.Frame:Hide();
	end
end

local function resetConfig()
	CharacterAchievements.db.position = {
		point = "TOPLEFT",
		relativePoint = "TOPLEFT",
		xOffset = 109,
		yOffset = -53
	};
	if (IsInGuild()) then
		toggleAccountWide(false);
	else
		CharacterAchievements.db.achievementEarned = true;
		toggleAccountWide(true);
	end
	toggleMove(false);
	toggleButton(true);
	CharacterAchievements.Frame:ClearAllPoints();
	if AchieveIt then
		--AchieveIt's Locate button is the same place as my default position
		CharacterAchievements.Frame:SetPoint("TOPLEFT", AchievementFrameHeader, "TOPLEFT", -8.5702, -44.23);
		CharacterAchievements.db.position.xOffset = -8.5702;
		CharacterAchievements.db.position.yOffset = -44.23;
	else
		CharacterAchievements.Frame:SetPoint("TOPLEFT", AchievementFrameHeader, "TOPLEFT", 109, -53);
	end
end

------------------------------------------------
-- Interface Options
------------------------------------------------

local function interfaceOptions_OnShow(self)
	if not (CharacterAchievements.Interface.AccountWide == nil) then
		CharacterAchievements.Interface.AccountWide:SetChecked(CharacterAchievements.db.isAccountWide);
	end
	if not (CharacterAchievements.Interface.EnableFrame == nil) then
		CharacterAchievements.Interface.EnableFrame:SetChecked(CharacterAchievements.db.isEnabled);
	end
	if not (CharacterAchievements.Interface.Movement == nil) then
		CharacterAchievements.Interface.Movement:SetChecked(CharacterAchievements.db.isMovementEnabled);
	end
end

local function interfaceOptions_Okay(self)
	--Settings save as they are changed automatically
end

local function interfaceOptions_Cancel(self)
	--Settings save as they are changed automatically
end

local function interfaceOptions_Default(self)
	resetConfig();
end

local function setupInterfaceOptions()
	CharacterAchievements.Interface = {};
	CharacterAchievements.InterfacePanel = CreateFrame("Frame", "CharacterAchievements_InterfacePanel", InterfaceOptionsFramePanelContainer);
	CharacterAchievements.InterfacePanel.name = "Character Achievements";
	CharacterAchievements.InterfacePanel:Hide();
	CharacterAchievements.InterfacePanel.okay = function(self) interfaceOptions_Okay(self) end
	CharacterAchievements.InterfacePanel.cancel = function(self) interfaceOptions_Cancel(self) end
	CharacterAchievements.InterfacePanel.default = function(self) interfaceOptions_Default(self) end
	CharacterAchievements.InterfacePanel:SetScript("OnShow", function(self) interfaceOptions_OnShow(self) end);
	
	--Title
	CharacterAchievements.Interface.ConfigTitle = CharacterAchievements.InterfacePanel:CreateFontString("CharacterAchievements_ConfigTitle", "ARTWORK", "GameFontNormalLarge");
    CharacterAchievements.Interface.ConfigTitle:SetPoint("TOPLEFT", 16, -16);
    CharacterAchievements.Interface.ConfigTitle:SetText("Character Achievements");
	
	--Description
	CharacterAchievements.Interface.ConfigSubtitle = CharacterAchievements.InterfacePanel:CreateFontString("CharacterAchievements_ConfigSubtitle", "ARTWORK", "GameFontHighlight");
    CharacterAchievements.Interface.ConfigSubtitle:SetHeight(22);
    CharacterAchievements.Interface.ConfigSubtitle:SetPoint("TOPLEFT", CharacterAchievements_ConfigTitle, "BOTTOMLEFT", 0, -8);
    CharacterAchievements.Interface.ConfigSubtitle:SetPoint("RIGHT", CharacterAchievements_InterfacePanel, -32, 0);
    CharacterAchievements.Interface.ConfigSubtitle:SetNonSpaceWrap(false);
	CharacterAchievements.Interface.ConfigSubtitle:SetWordWrap(true);
    CharacterAchievements.Interface.ConfigSubtitle:SetJustifyH("LEFT");
    CharacterAchievements.Interface.ConfigSubtitle:SetJustifyV("TOP");
    CharacterAchievements.Interface.ConfigSubtitle:SetText("Toggle Achievement display between highlighting Account-earned or Character Achievements");
	
	--Account Wide CheckButton
	CharacterAchievements.Interface.AccountWide = CreateFrame("CheckButton", "CharacterAchievements_AccountWide", CharacterAchievements.InterfacePanel, "InterfaceOptionsCheckButtonTemplate");
    CharacterAchievements.Interface.AccountWide:SetPoint("TOPLEFT", CharacterAchievements_ConfigSubtitle, "BOTTOMLEFT", 16, -6);
	_G[CharacterAchievements.Interface.AccountWide:GetName().."Text"]:SetText("Display Account-earned Achievements");
	CharacterAchievements.Interface.AccountWide:SetScript("OnClick", function(self, button, down)  
		PlaySound(self:GetChecked() and "igMainMenuOptionCheckBoxOn" or "igMainMenuOptionCheckBoxOff");
		toggleAccountWide(self:GetChecked() and true or false);
		end);
	CharacterAchievements.Interface.AccountWide.tooltipText = "Account-earned";
	CharacterAchievements.Interface.AccountWide.tooltipRequirement = "Enable Account-earned Style";
	
	--Enable EnableFrame CheckButton
	CharacterAchievements.Interface.EnableFrame = CreateFrame("CheckButton", "CharacterAchievements_EnableFrame", CharacterAchievements.InterfacePanel, "InterfaceOptionsCheckButtonTemplate");
    CharacterAchievements.Interface.EnableFrame:SetPoint("TOPLEFT", CharacterAchievements_AccountWide, "BOTTOMLEFT", 0, -15);
	_G[CharacterAchievements.Interface.EnableFrame:GetName().."Text"]:SetText("Enable Button on Achievement Frame");
	CharacterAchievements.Interface.EnableFrame:SetScript("OnClick", function(self, button, down)	
		PlaySound(self:GetChecked() and "igMainMenuOptionCheckBoxOn" or "igMainMenuOptionCheckBoxOff");
		toggleButton(self:GetChecked() and true or false);
		end);
	CharacterAchievements.Interface.EnableFrame.tooltipText = "Button";
	CharacterAchievements.Interface.EnableFrame.tooltipRequirement = "Enable Display of the Button on the Achievement Frame";
	
	--Enable Movement CheckButton
	CharacterAchievements.Interface.Movement = CreateFrame("CheckButton", "CharacterAchievements_Move", CharacterAchievements.InterfacePanel, "InterfaceOptionsCheckButtonTemplate");
    CharacterAchievements.Interface.Movement:SetPoint("TOPLEFT", CharacterAchievements_EnableFrame, "BOTTOMLEFT", 0, 0);
	_G[CharacterAchievements.Interface.Movement:GetName().."Text"]:SetText("Enable Movement of Button");
	CharacterAchievements.Interface.Movement:SetScript("OnClick", function(self, button, down)	
		PlaySound(self:GetChecked() and "igMainMenuOptionCheckBoxOn" or "igMainMenuOptionCheckBoxOff");
		toggleMove(self:GetChecked() and true or false);
		end);
	CharacterAchievements.Interface.Movement.tooltipText = "Movement";
	CharacterAchievements.Interface.Movement.tooltipRequirement = "Enable Movement of the Achievement Display Button";
	
	--Reset Settings
	CharacterAchievements.Interface.ResetUI = CreateFrame("Button", "CharacterAchievements_ResetUI", CharacterAchievements.InterfacePanel, "UIPanelButtonTemplate");
	CharacterAchievements.Interface.ResetUI:SetText("Reset Settings and Position");
	CharacterAchievements.Interface.ResetUI:SetWidth(177);
	CharacterAchievements.Interface.ResetUI:SetHeight(24);
	CharacterAchievements.Interface.ResetUI:SetPoint("TOPLEFT", CharacterAchievements_Move, "BOTTOMLEFT",0,-40);
	CharacterAchievements.Interface.ResetUI:SetScript("OnClick", function()
		resetConfig();
	end);
	CharacterAchievements.Interface.ResetUI.tooltipText = "Reset's all settings to default values";
	
	--Note: Not in guild
	CharacterAchievements.Interface.ConfigSubtitle = CharacterAchievements.InterfacePanel:CreateFontString("CharacterAchievements_ConfigSubtitle", "ARTWORK", "GameFontHighlight");
    CharacterAchievements.Interface.ConfigSubtitle:SetHeight(18);
    CharacterAchievements.Interface.ConfigSubtitle:SetPoint("TOPLEFT", CharacterAchievements_ResetUI, "BOTTOMLEFT", 0, -40);
    CharacterAchievements.Interface.ConfigSubtitle:SetPoint("RIGHT", CharacterAchievements_InterfacePanel, -32, 0);
    CharacterAchievements.Interface.ConfigSubtitle:SetNonSpaceWrap(false);
	CharacterAchievements.Interface.ConfigSubtitle:SetWordWrap(true);
    CharacterAchievements.Interface.ConfigSubtitle:SetJustifyH("LEFT");
    CharacterAchievements.Interface.ConfigSubtitle:SetJustifyV("TOP");
    CharacterAchievements.Interface.ConfigSubtitle:SetText("Note: You must be in a guild in order to view your Character Achievement Points.");
	
	InterfaceOptions_AddCategory(CharacterAchievements.InterfacePanel);
end

local function openInterfaceOptions()
	InterfaceOptionsFrame_OpenToCategory(CharacterAchievements.InterfacePanel)
end

CharacterAchievements.cout = cout;
CharacterAchievements.toggleMove = toggleMove;
CharacterAchievements.postHook_Frame = postHook_Frame;
CharacterAchievements.toggleAccountWide = toggleAccountWide;
CharacterAchievements.toggleButton = toggleButton;
CharacterAchievements.setupInterfaceOptions = setupInterfaceOptions;
CharacterAchievements.openInterfaceOptions = openInterfaceOptions;
CharacterAchievements.resetConfig = resetConfig;
------------------------------------------------
-- Global Functions
------------------------------------------------

function CharacterAchievements.OnEvent(self,event,...)
	if event == "PLAYER_LOGIN" then
		CharacterAchievements.Frame:UnregisterEvent("PLAYER_LOGIN");
		--Added to fix conflict with SLASH_CharacterAchievement1 addon
		if not AchievementFrame then  
			AchievementFrame_LoadUI();  
		end
		CharacterAchievements.db = CharacterAchievementsDB;

		if CharacterAchievements.db == nil or CharacterAchievements.db == "" then
			CharacterAchievements.db = {"","",};
			resetConfig();
		else
			if (IsInGuild()) then
				toggleAccountWide(CharacterAchievements.db.isAccountWide);
			else
				--Force Character Point Update on login
				CharacterAchievements.db.characterPoints = GetTotalAchievementPoints();
				CharacterAchievements.db.achievementEarned = true;
				toggleAccountWide(true);
			end
			toggleMove(CharacterAchievements.db.isMovementEnabled);
			toggleButton(CharacterAchievements.db.isEnabled);
		end
		if not AchievementFrame then  
			AchievementFrame_LoadUI();  
		end
		--Update Full name
		CharacterAchievements.db.fullName = UnitName("player"); --.."-"..GetRealmName();
		--Post Hook Achievement Frame to enable toggle switch
		postHook_Frame();
		
	elseif (event == "PLAYER_LOGOUT") then
		-- Store to CharacterAchievementsDB
		CharacterAchievementsDB = CharacterAchievements.db;
	elseif (event == "INSPECT_ACHIEVEMENT_READY") then
		if (CharacterAchievements.db.characterInspect) then
			CharacterAchievements.db.characterPoints = GetComparisonAchievementPoints();
			AchievementFrameHeaderPoints:SetText(CharacterAchievements.db.characterPoints);
			CharacterAchievements.db.characterInspect = false;
			CharacterAchievements.db.achievementEarned = false;
			ShowAccountAchievements(CharacterAchievements.db.accountAchievementsHidden);
		end
	elseif (event == "ACHIEVEMENT_EARNED") then
		--Force Character Point Update
		if (not IsInGuild()) then
			CharacterAchievements.db.achievementEarned = true;
			toggleAccountWide(true);
		end
	end
end

------------------------------------------------
-- Slash Commands
------------------------------------------------
local function slashHandler(msg)
	msg = msg:lower() or "";
	if (msg == "account" or msg == "a") then
		CharacterAchievements.toggleAccountWide(true);
	elseif (msg == "character" or msg == "c") then
		CharacterAchievements.toggleAccountWide(false);
	elseif (msg == "options" or msg == "gui") then
		CharacterAchievements.openInterfaceOptions();
	elseif (msg == "lock") then
		CharacterAchievements.toggleMove(false);
	elseif (msg == "unlock") then
		CharacterAchievements.toggleMove(true);
	elseif (msg == "hide") then
		CharacterAchievements.toggleButton(false);
	elseif (msg == "show") then
		CharacterAchievements.toggleButton(true);
	elseif (msg == "reset") then
		resetConfig();
	else
		print("|cff33ff99Character Achievements|r: Arguments to |cffffff78/ca|r :");
		print("  |cffffff78 options|r - Display the options");
		print("  |cffffff78 account|r - Display Account-earned style");
		print("  |cffffff78 character|r - Display Character style");
		print("  |cffffff78 hide|r - Hide button");
		print("  |cffffff78 show|r - Show button");
		print("  |cffffff78 unlock|r - Unlock button");
		print("  |cffffff78 lock|r - Lock button");
		print("  |cffffff78 reset|r - Restores default settings and button position");
	end
end

SlashCmdList.CharacterAchievement = function(msg) slashHandler(msg) end;
SLASH_CharacterAchievement1 = "/ca";
SLASH_CharacterAchievement2 = "/characterachievements";
SLASH_CharacterAchievement3 = "/characterachievement";

------------------------------------------------
-- Character Achievement Frame Setup
------------------------------------------------
CharacterAchievements.Frame = CreateFrame("Frame", "CharacterAchievementsFrame", AchievementFrameHeader,"AchievementCategoryTemplate");
if AchieveIt then
	--AchieveIt Locate button is same place as my default position
	CharacterAchievements.Frame:SetPoint("TOPLEFT", AchievementFrameHeader, "TOPLEFT", -8.5702, -44.23);
else
	CharacterAchievements.Frame:SetPoint("TOPLEFT", AchievementFrameHeader, "TOPLEFT", 109, -53);
end
CharacterAchievements.Frame:SetWidth(120);
CharacterAchievements.Frame:SetHeight(20);
CharacterAchievements.Frame.label:SetText("");
CharacterAchievements.Frame:SetFrameStrata("HIGH");
CharacterAchievements.Frame:SetClampedToScreen(true);
CharacterAchievements.Frame:SetScript("OnEvent", function(self,event,...) CharacterAchievements.OnEvent(self,event,...) end);
CharacterAchievements.Frame:SetScript("OnShow", function(self)
	if not (CharacterAchievements.db == nil) and not (CharacterAchievements.db.hasMoved == nil) and CharacterAchievements.db.hasMoved then
		CharacterAchievements.Frame:ClearAllPoints();
		CharacterAchievements.Frame:SetPoint("TOPLEFT", AchievementFrameHeader, "TOPLEFT", CharacterAchievements.db.position.xOffset, CharacterAchievements.db.position.yOffset);
	end
	if not (CharacterAchievements.CheckButton == nil) then
		CharacterAchievements.CheckButton:SetChecked(CharacterAchievements.db.isAccountWide);
	end
end);
CharacterAchievements.Frame:RegisterEvent("PLAYER_LOGIN");
CharacterAchievements.Frame:RegisterEvent("PLAYER_LOGOUT");
CharacterAchievements.Frame:RegisterEvent("INSPECT_ACHIEVEMENT_READY");
CharacterAchievements.Frame:RegisterEvent("ACHIEVEMENT_EARNED");

-- Implement the check button ontop of frame
CharacterAchievements.CheckButton = CreateFrame("CheckButton", "CharacterAchievements_CheckButton", CharacterAchievements.Frame, "UICheckButtonTemplate");
CharacterAchievements.CheckButton:SetPoint("LEFT", CharacterAchievements.Frame, "LEFT", 0,-4);
CharacterAchievements.CheckButton:SetWidth(28);
CharacterAchievements.CheckButton:SetHeight(28);
CharacterAchievements.CheckButton.tooltip = "This toggles the display of Account-earned or Character Achievements";
CharacterAchievements.CheckButton:SetScript("OnClick", function(self, button, down)
	CharacterAchievements.toggleAccountWide(self:GetChecked() and true or false);
end);
_G[CharacterAchievements.CheckButton:GetName() .. "Text"]:SetText("Account-earned")

--Implement Interface Options
CharacterAchievements.setupInterfaceOptions();