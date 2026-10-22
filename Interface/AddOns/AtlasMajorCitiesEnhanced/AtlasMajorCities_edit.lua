if ( AtlasMajorCities_EditMode ) then

-- define the shop size in % of map size (this influences the shop-ID and sign-ID)
local AMC_ShopSize = {
	["TheExodar"]            = 2.0,
	["Darnassus"]            = 1.4,
	["Ironforge"]            = 1.6,
	["StormwindCity"]        = 1.0,
	["SilvermoonCity"]       = 3.0,
	["Undercity"]            = 1.6,
	["ThunderBluff"]         = 1.4,
	["Orgrimmar1_"]          = 1.2,
	["Orgrimmar2_"]          = 5.0,
	["ShattrathCity"]        = 1.4,
	["Dalaran1_"]            = 2.0,
	["Dalaran2_"]            = 3.0,
	["ShrineofTwoMoons1_"]   = 1.5,
	["ShrineofTwoMoons2_"]   = 2.0,
	["ShrineofSevenStars3_"] = 1.5,
	["ShrineofSevenStars4_"] = 2.0,
}

-- sub-commands
local AMC_MoveString     = AtlasMajorCities_loc["subMoveString"];
local AMC_DeleteString   = AtlasMajorCities_loc["subDeleteString"];
local AMC_NoPosString    = AtlasMajorCities_loc["subNoPosString"];
local AMC_DelNoPosString = AtlasMajorCities_loc["subDelNoPosString"];

-- global identifiers
local AMC_MapName = "";
local AMC_Zone = "";
local AMC_PosX = 0.0;
local AMC_PosY = 0.0;
local AMC_ShopID = "";
local AMC_ToolTipText = "";
local AMC_ToolTipSign = "";
local AMC_ShopList = {};
local AMC_DBbase;
local AMC_Pbase;

-- global strings
local AMC_ContinentText = "";
local AMC_CityText = "";
local AMC_ZoneText = "";
local AMC_ShopLabel = "";
local AMC_ShopText = "";
local AMC_NPCComment = "";

-- scan mode switch
local AMC_ScanMode = 0;


-- ******************************************
-- *** Definition of some basic functions ***
-- ******************************************

-- extract the NPCs ID (first number is the type)
local function FAMC_GetNpcID()
	local guid = UnitGUID("target");
	local type = tonumber(guid:sub(5,5), 16) % 8;
	-- check the type of target
	if ( type == 3 or type == 5 ) then
		-- get the NPC ID
		local npcid = tonumber(string.sub(guid,6,10), 16);
		local npcid0 = "NPC"..tostring(npcid);
		return npcid, npcid0;
	end
end

-- check that the displayed tooltip is originating from a sign at the world frame
function AtlasMajorCities_CheckSignToolTip()
	if ( (GetMouseFocus():GetName() == "WorldFrame") and GameTooltip:IsVisible() ) then
		if ( (GameTooltip:NumLines() == 1) and (not GameTooltipTextRight1:GetText()) ) then
			local tcolor = { GameTooltipTextLeft1:GetTextColor() };
			if ( math.floor((tcolor[1]+tcolor[2]+tcolor[3])*100.0) == 182 ) then
				local bcolor = { GameTooltip:GetBackdropColor() };
				if ( math.floor((bcolor[1]+bcolor[2]+bcolor[3])*100.0) == 53 ) then
					return true;
				end
			end
		end
	end
	return false;
end

-- set some variables for the actual map: AMC_MapName, AMC_PosX, AMC_PosY
local function FAMC_GetMapInfos()
	SetMapToCurrentZone();

	-- get the map name
	local MapName, _, _, isMicro, MicroMap = GetMapInfo();
	if ( isMicro ) then MapName = MicroMap; end
	AMC_MapName = MapName;
	local dungeonLevel = GetCurrentMapDungeonLevel();
	if ( dungeonLevel > 0 ) then
		AMC_MapName = AMC_MapName..dungeonLevel.."_";
	else
		if ( AMC_MapName == "Dalaran" ) then AMC_MapName = "Dalaran1_"; end
		if ( AMC_MapName == "Orgrimmar" ) then AMC_MapName = "Orgrimmar1_"; end
	end

	-- get player position
	local posX, posY = GetPlayerMapPosition("player");
	AMC_PosX = math.floor(posX * 1000.0 + 0.5) / 10.0;
	AMC_PosY = math.floor(posY * 1000.0 + 0.5) / 10.0;
end

-- set the base lists for the map AMC_MapName from DB
-- (in case of partial userDB create not existing city list)
local function FAMC_SetBaseList()
	local partDB = ( AtlasMajorCities_UserDB and AtlasMajorCities_Adds );
	local baseDB = ( not AtlasMajorCities_UserDB or partDB );

	if ( baseDB ) then AMC_DBbase = AtlasMajorCities_DB0;
	              else AMC_DBbase = AtlasMajorCities_DB; end
	if ( partDB ) then AMC_Pbase = AtlasMajorCities_DB; end

	if ( AMC_DBbase[AMC_MapName] ) then
		if ( partDB ) then
			if ( not AMC_Pbase[AMC_MapName] ) then
				AMC_Pbase[AMC_MapName] = {};
			end
		end
	end
end

-- compute ID string from position
local function FAMC_GetPositionID(size)
	local xpos = math.floor(AMC_PosX / size + 0.5) * math.floor(size * 10.0);
	local ypos = math.floor(AMC_PosY / size + 0.5) * math.floor(size * 10.0);
	return string.format("%03i%03i", xpos, ypos);
end

-- extract the NPC name and title from the tooltip
local function FAMC_GetNPCtext()
	local c1, c2 = ", ", "";
	if ( (GetLocale() == "zhTW") or (GetLocale() == "zhCN") ) then c1, c2 = " <", ">"; end

	local text1 = getglobal("GameTooltipTextLeft1"):GetText();
	local text2 = c1..getglobal("GameTooltipTextLeft2"):GetText();
	local text3 = "";
	local level_string = string.gsub(LEVEL_GAINED,"%%d","");
	local tt_string = string.gsub(TOOLTIP_UNIT_LEVEL_CLASS,"%%[12]$s","");
	local isLevel = string.find(text2,level_string);
	if ( not isLevel ) then isLevel = string.find(text2,tt_string); end
	if ( not isLevel ) then isLevel = string.find(text2,LEVEL); end
	if ( isLevel ) then
		text2 = "";
		c2 = "";
	else
		text3 = " "..getglobal("GameTooltipTextLeft3"):GetText();
		isLevel = string.find(text3,level_string);
		if ( not isLevel ) then isLevel = string.find(text3,tt_string); end
		if ( not isLevel ) then isLevel = string.find(text3,LEVEL); end
		if ( isLevel ) then text3 = ""; end
	end
	return text1..text2..text3..c2;
end

-- delete a shop from the user DB
local function FAMC_DeleteAssignedShop()
	local oldtitle = "";
	local old_posx = AMC_PosX;
	local old_posy = AMC_PosY;
	AMC_PosX = nil;
	AMC_PosY = nil;
	local slist;

	-- remove shop from partial DB
	if ( AMC_Pbase and AMC_Pbase[AMC_MapName] ) then
		local pshopid = tonumber(AMC_ShopID) + 1000000;
		if ( pshopid == 0 ) then pshopid = tonumber(string.sub(AMC_Zone,5)) + 1000000; end
		slist = AMC_Pbase[AMC_MapName][pshopid];
		if ( slist ) then
			-- remove all NPCs at this shop from the NPC and comment list
			for key, value in pairs(slist) do
				if ( type(key) == "number" ) then
					local value0 = "NPC"..tostring(value);
					AtlasMajorCities_NPC[value0] = nil;
					AtlasMajorCities_Comment[value0] = nil;
				end
			end
			-- remove the title of this shop from the shop sign and title list
			local sid = AMC_Pbase[AMC_MapName][pshopid].sID;
			if ( sid and AtlasMajorCities_Shop[sid] ) then
				oldtitle = AtlasMajorCities_Shop[sid];
				AtlasMajorCities_Shop[sid] = nil;
			end
			local idx = AMC_MapName.."-"..AMC_ShopID;
			if ( AtlasMajorCities_Title[idx] ) then
				oldtitle = oldtitle.."."..AtlasMajorCities_Title[idx]..".";
				AtlasMajorCities_Title[idx] = nil;
			end
			-- set old shop position for status msg
			AMC_PosX = slist.PosX;
			AMC_PosY = slist.PosY;
			-- finally remove the shop
			slist = nil;
			AMC_Pbase[AMC_MapName][pshopid] = nil;
			-- remove the shop also from the AtlasMajorCities_Adds list
			if ( AtlasMajorCities_Adds[AMC_MapName] ) then
				local list = AtlasMajorCities_Adds[AMC_MapName];
				local index = 0
				for idx, id in pairs(list) do
					if ( id == pshopid ) then index = idx; end
				end
				if ( index ~= 0 ) then table.remove(list, index); end
			end
		end
	end

	-- remove shop from DB or mark as deleted in partial DB
	if ( AMC_DBbase and AMC_DBbase[AMC_MapName] ) then
		local ishopid = tonumber(AMC_ShopID);
		if ( ishopid == 0 ) then ishopid = tonumber(string.sub(AMC_Zone,5)); end
		slist = AMC_DBbase[AMC_MapName][ishopid];
		if ( slist ) then
			-- remove all NPCs at this shop from the NPC and comment list
			for key, value in pairs(slist) do
				if ( type(key) == "number" ) then
					local value0 = "NPC"..tostring(value);
					if ( AMC_Pbase ) then
						AtlasMajorCities_NPC[value0] = "deleted";
						if ( AtlasMajorCities_Comment[value0] ) then
							AtlasMajorCities_Comment[value0] = "deleted";
						end
					else
						AtlasMajorCities_NPC[value0] = nil;
						AtlasMajorCities_Comment[value0] = nil;
					end
				end
			end
			-- remove the title of this shop from the shop sign and title list
			if ( slist.sID and AtlasMajorCities_Shop[slist.sID] ) then
				if ( not oldtitle ) then oldtitle = AtlasMajorCities_Shop[slist.sID]; end
				if ( AMC_Pbase ) then
					AtlasMajorCities_Shop[slist.sID] = "deleted";
				else
					AtlasMajorCities_Shop[slist.sID] = nil;
				end
			end
			local idx = AMC_MapName.."-"..AMC_ShopID;
			if ( AtlasMajorCities_Title[idx] ) then
				oldtitle = oldtitle.."."..AtlasMajorCities_Title[idx]..".";
				if ( AMC_Pbase ) then
					AtlasMajorCities_Title[idx] = "deleted";
				else
					AtlasMajorCities_Title[idx] = nil;
				end
			end
			-- set old shop position for status msg
			if ( not AMC_PosX ) then AMC_PosX = slist.PosX; end
			if ( not AMC_PosY ) then AMC_PosY = slist.PosY; end
			-- finally remove the shop
			slist = nil;
			if ( AMC_Pbase ) then
				local pshopid = tonumber(AMC_ShopID) + 1000000;
				if ( pshopid == 0 ) then pshopid = tonumber(string.sub(AMC_Zone,5)) + 1000000; end
				if ( not AMC_Pbase[AMC_MapName] ) then
					AMC_Pbase[AMC_MapName] = {};
				end
				AMC_Pbase[AMC_MapName][pshopid] = {};
			else
				AMC_DBbase[AMC_MapName][ishopid] = nil;
			end
		end
	end

	if ( not AMC_PosX ) then AMC_PosX = old_posx; end
	if ( not AMC_PosY ) then AMC_PosY = old_posy; end

	return oldtitle;
end

-- move the shop to a new position
local function FAMC_MoveAssignedShop(new_shopID)
	local ishopid = tonumber(AMC_ShopID);
	local pshopid = tonumber(AMC_ShopID) + 1000000;
	local npshopid = tonumber(new_shopID) + 1000000;
	local newzone = tonumber(string.sub(AMC_Zone,5));

	if ( AMC_Pbase and AMC_Pbase[AMC_MapName] and AMC_Pbase[AMC_MapName][pshopid] ) then
		local islist;
		if ( AMC_DBbase[AMC_MapName] ) then islist = AMC_DBbase[AMC_MapName][ishopid]; end
		local oldshop;
		if ( AMC_Pbase[AMC_MapName][npshopid] ) then
			if ( not next(AMC_Pbase[AMC_MapName][npshopid]) ) then
				oldshop = true;
			else
				local error = AtlasMajorCities_loc["errOldShop"];
				DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
				return;
			end
		end
		-- move the shop list
		AMC_Pbase[AMC_MapName][npshopid] = AMC_Pbase[AMC_MapName][pshopid];
		AMC_Pbase[AMC_MapName][pshopid] = nil;
		if ( islist ) then AMC_Pbase[AMC_MapName][pshopid] = {}; end
		-- change the shopID
		AMC_Pbase[AMC_MapName][npshopid].ID = new_shopID;
		AMC_Pbase[AMC_MapName][npshopid].Zone = newzone;
		-- move the shop title
		local oldID = AMC_MapName.."-"..AMC_ShopID;
		local newID = AMC_MapName.."-"..new_shopID;
		AtlasMajorCities_Title[newID] = AtlasMajorCities_Title[oldID];
		AtlasMajorCities_Title[oldID] = nil;
		if ( islist and AtlasMajorCities_Title0[oldID] ) then AtlasMajorCities_Title[oldID] = "deleted"; end
		-- change assigned shop to new ID
		AMC_ShopID = new_shopID;
		-- insert the new shopID to the Adds-list and remove the old one
		if ( not AtlasMajorCities_Adds[AMC_MapName] ) then AtlasMajorCities_Adds[AMC_MapName] = {}; end
		local alist = AtlasMajorCities_Adds[AMC_MapName];
		local index = 0
		for idx, id in pairs(alist) do
			if ( id == pshopid ) then index = idx; end
		end
		if ( index ~= 0 ) then table.remove(alist, index); end
		if ( not oldshop ) then table.insert(alist, npshopid); end
	else
		local islist;
		if ( AMC_DBbase[AMC_MapName] ) then islist = AMC_DBbase[AMC_MapName][ishopid]; end
		if ( islist ) then
			if ( AMC_Pbase ) then
				-- add moved shop to partial DB
				if ( not AMC_Pbase[AMC_MapName] ) then
					AMC_Pbase[AMC_MapName] = {};
				end
				AMC_Pbase[AMC_MapName][npshopid] = islist;
				AMC_Pbase[AMC_MapName][pshopid] = {};
				-- add the new shopID to the Adds-list
				if ( not AtlasMajorCities_Adds[AMC_MapName] ) then AtlasMajorCities_Adds[AMC_MapName] = {}; end
				table.insert(AtlasMajorCities_Adds[AMC_MapName], npshopid)
			end
			-- change the shopID
			islist.ID = new_shopID;
			islist.Zone = newzone;
			-- move the shop title
			local oldID = AMC_MapName.."-"..AMC_ShopID;
			local newID = AMC_MapName.."-"..new_shopID;
			if ( AMC_Pbase ) then
				if ( AtlasMajorCities_Title[oldID] ) then
					AtlasMajorCities_Title[newID] = AtlasMajorCities_Title[oldID];
					AtlasMajorCities_Title[oldID] = nil;
				elseif ( AtlasMajorCities_Title0[oldID] ) then
					AtlasMajorCities_Title[newID] = AtlasMajorCities_Title0[oldID];
				end
				if ( AtlasMajorCities_Title0[oldID] ) then
					AtlasMajorCities_Title[oldID] = "deleted";
				end
			else
				AtlasMajorCities_Title[newID] = AtlasMajorCities_Title[oldID];
				AtlasMajorCities_Title[oldID] = nil;
			end
			-- change assigned shop to new ID
			AMC_ShopID = new_shopID;
		end
	end
end

-- show help
local function FAMC_ShowHelp()
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end

	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help01"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help02"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help03"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help04"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help05"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help06"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help07"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help08"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help09"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help10"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help11"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help12"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help13"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help14"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help15"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help16"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help17"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help18"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help19"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help20"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help21"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help22"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help23"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help24"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help25"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help26"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help27"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help28"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help29"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help30"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help31"], .9, .0, .9);
	DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["Help32"], .9, .0, .9);
end


-- *******************************************
-- *** Define some popup windows for input ***
-- *******************************************

StaticPopupDialogs["AtlasMajorCities_ZoneManually"] = {
	text = TEXT(AtlasMajorCities_loc["dialogZoneTitle"]),
	button1 = TEXT(ACCEPT),
	button2 = TEXT(CANCEL),
	hasEditBox = 1,
	maxLetters = 30,
	OnShow = function(self)
		self.editBox:SetText("");
		self.editBox:SetFocus();
	end,
	OnAccept = function(self)
		AMC_ZoneText = self.editBox:GetText();
		AtlasMajorCities_SetZoneDB();
	end,
	EditBoxOnEnterPressed = function(self)
		local parent = self:GetParent();
		AMC_ZoneText = parent.editBox:GetText();
		AtlasMajorCities_SetZoneDB();
		parent:Hide();
	end,
	timeout = 0,
	exclusive = 1,
	whileDead = 1,
	hideOnEscape = 1
};

StaticPopupDialogs["AtlasMajorCities_LabelManually"] = {
	text = TEXT(AtlasMajorCities_loc["dialogShopLabel"]),
	button1 = TEXT(ACCEPT),
	button2 = TEXT(CANCEL),
	hasEditBox = 1,
	maxLetters = 9,
	OnShow = function(self)
		local text = "";
		if ( AMC_ShopList.Label ) then text = AMC_ShopList.Label; end
		self.editBox:SetText(text);
		self.editBox:SetFocus();
	end,
	OnAccept = function(self)
		AMC_ShopLabel = self.editBox:GetText();
		AtlasMajorCities_SetLabelDB();
	end,
	EditBoxOnEnterPressed = function(self)
		local parent = self:GetParent();
		AMC_ShopLabel = parent.editBox:GetText();
		AtlasMajorCities_SetLabelDB();
		parent:Hide();
	end,
	timeout = 0,
	exclusive = 1,
	whileDead = 1,
	hideOnEscape = 1
};

StaticPopupDialogs["AtlasMajorCities_TitleManually"] = {
	text = TEXT(AtlasMajorCities_loc["dialogShopTitle"]),
	button1 = TEXT(ACCEPT),
	button2 = TEXT(CANCEL),
	hasEditBox = 1,
	maxLetters = 150,
	OnShow = function(self)
		local SignToolTip = AtlasMajorCities_CheckSignToolTip();
		if ( SignToolTip and (AMC_ShopText == "") ) then
			AMC_ShopText = getglobal("GameTooltipTextLeft1"):GetText();
		end
		if ( (GetMinimapZoneText() ~= GetZoneText()) and (GetMinimapZoneText() ~= AMC_ZoneText) and (AMC_ShopText == "") ) then
			AMC_ShopText = GetMinimapZoneText();
		end
		self.editBox:SetText(AMC_ShopText);
		self.editBox:SetFocus();
	end,
	OnAccept = function(self)
		AMC_ShopText = self.editBox:GetText();
		AtlasMajorCities_SetTitleDB();
	end,
	EditBoxOnEnterPressed = function(self)
		local parent = self:GetParent();
		AMC_ShopText = parent.editBox:GetText();
		AtlasMajorCities_SetTitleDB();
		parent:Hide();
	end,
	timeout = 0,
	exclusive = 1,
	whileDead = 1,
	hideOnEscape = 1
};

StaticPopupDialogs["AtlasMajorCities_CommentManually"] = {
	text = TEXT(AtlasMajorCities_loc["dialogNPCComment"]),
	button1 = TEXT(ACCEPT),
	button2 = TEXT(CANCEL),
	hasEditBox = 1,
	maxLetters = 100,
	OnShow = function(self)
		if ( (not AMC_NPCComment or (AMC_NPCComment == "")) and GameTooltip:IsVisible() ) then
			AMC_NPCComment = getglobal("GameTooltipTextLeft1"):GetText();
		end
		self.editBox:SetText(AMC_NPCComment);
		self.editBox:SetFocus();
	end,
	OnAccept = function(self)
		AMC_NPCComment = self.editBox:GetText();
		AtlasMajorCities_SetCommentDB();
	end,
	EditBoxOnEnterPressed = function(self)
		local parent = self:GetParent();
		AMC_NPCComment = parent.editBox:GetText();
		AtlasMajorCities_SetCommentDB();
		parent:Hide();
	end,
	timeout = 0,
	exclusive = 1,
	whileDead = 1,
	hideOnEscape = 1
};


-- ****************************************
-- *** Definition of the main functions ***
-- ****************************************

-- activate the requested scan mode or reload the database
function AtlasMajorCities_SetScan(msg)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end

	if ( msg == "help" ) then
		FAMC_ShowHelp();
		return;
	end

	-- show label at its default positions and activate label positioning at the map
	if ( msg == AtlasMajorCities_loc["movelabel"] ) then
		if ( not AtlasMajorCities_UserDB ) then
			DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgUserOnly"], .9, .0, .0);
			return;
		end
		AtlasMajorCities_LabelAtPos = -1;
		Atlas_Refresh();
		DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgOnMove"], .9, .0, .9);
		return;
	end

	-- show label at its default position
	if ( msg == AtlasMajorCities_loc["defaultlabel"] ) then
		AtlasMajorCities_LabelAtPos = 0;
		Atlas_Refresh();
		return;
	end

	-- show label at shop position
	if ( msg == AtlasMajorCities_loc["shoplabel"] ) then
		AtlasMajorCities_LabelAtPos = 1;
		Atlas_Refresh();
		return;
	end

	-- show label at shop sign position
	if ( msg == AtlasMajorCities_loc["signlabel"] ) then
		AtlasMajorCities_LabelAtPos = 2;
		Atlas_Refresh();
		return;
	end

	-- reload DB in Atlas
	if ( msg == AtlasMajorCities_loc["reload"] ) then
		AtlasMajorCities_LabelAtPos = 0;
		AtlasMajorCities_UserDB = true;
		AtlasMajorCities_RegisterWithAtlas();
		return;
	end

	-- the following functions work only with the user DB
	if ( not AtlasMajorCities_UserDB ) then
		DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgUserOnly"], .9, .0, .0);
		return;
	end

	-- load original city data like Atlas_RegisterPlugin
	if ( msg == AtlasMajorCities_loc["revert"] ) then
		AMC_Zone = nil;
		AMC_ShopID = nil;
		AMC_ShopList = nil;
		AMC_ToolTipText = "";
		AMC_ScanMode = 0;
		AtlasMajorCities_LabelAtPos = 0;
		AtlasMajorCities_UserDB = false;
		AtlasMajorCities_RegisterWithAtlas();
		return;
	end

	-- copy internal db to user data (Attention! This overwrite all user data.)
	if ( msg == "copydbtouser" ) then
		if ( AtlasMajorCities_DB0 ) then
			AtlasMajorCities_DB        = AtlasMajorCities_DB0;
			AtlasMajorCities_Continent = AtlasMajorCities_Continent0;
			AtlasMajorCities_City      = AtlasMajorCities_City0;
			AtlasMajorCities_Zone      = AtlasMajorCities_Zone0;
			AtlasMajorCities_Shop      = AtlasMajorCities_Shop0;
			AtlasMajorCities_Title     = AtlasMajorCities_Title0;
			AtlasMajorCities_NPC       = AtlasMajorCities_NPC0;
			AtlasMajorCities_Comment   = AtlasMajorCities_Comment0;
			AtlasMajorCities_Adds      = nil;
			AtlasMajorCities_RegisterWithAtlas();
			DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgDBcopied"], .9, .0, .0);
		else
			DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgnoDB"], .9, .0, .0);
		end
		return;
	end

	-- delete all user data (Attention!)
	if ( msg == "deleteuserdb" ) then
		if ( AtlasMajorCities_DB0 ) then
			AtlasMajorCities_DB        = {};
			AtlasMajorCities_Continent = {};
			AtlasMajorCities_City      = {};
			AtlasMajorCities_Zone      = {};
			AtlasMajorCities_Shop      = {};
			AtlasMajorCities_Title     = {};
			AtlasMajorCities_NPC       = {};
			AtlasMajorCities_Comment   = {};
			AtlasMajorCities_Adds      = {};
			AtlasMajorCities_RegisterWithAtlas();
			DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgDBdeleted"], .9, .0, .0);
		else
			DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgnoDB"], .9, .0, .0);
		end
		return;
	end

	-- set the scan mode
	if ( msg == "off" ) then
		AMC_Zone = nil;
		AMC_ShopID = nil;
		AMC_ShopList = nil;
		AMC_ToolTipText = "";
		AMC_ScanMode = 0;
		AtlasMajorCities_LabelAtPos = 0;
		Atlas_Refresh();
		DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgOff"], .9, .0, .9);
	elseif ( msg == "npc" ) then
		AMC_ScanMode = 1;
		DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgOnNPC"], .9, .0, .9);
	elseif ( msg == "on" ) then
		AMC_Zone = nil;
		AMC_ShopID = nil;
		AMC_ShopList = nil;
		AMC_ToolTipText = "";
		AMC_ScanMode = 2;
		DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgOn"], .9, .0, .9);
	elseif ( msg == "all" ) then
		AMC_Zone = nil;
		AMC_ShopID = nil;
		AMC_ShopList = nil;
		AMC_ToolTipText = "";
		AMC_ScanMode = 4;
		DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgOnAll"], .9, .0, .9);
	elseif ( (msg == "sign") and (AMC_ScanMode > 1) ) then
		if ( (AMC_ScanMode == 2) or (AMC_ScanMode == 4) ) then
			AMC_ScanMode = AMC_ScanMode + 1;
			DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgOnSign"], .9, .0, .9);
		elseif ( (AMC_ScanMode == 3) or (AMC_ScanMode == 5) ) then
			AMC_ScanMode = AMC_ScanMode - 1;
			DEFAULT_CHAT_FRAME:AddMessage(AtlasMajorCities_loc["msgOffSign"], .9, .0, .9);
		end
	else
		FAMC_ShowHelp();
	end
end

-- get the zone of the city by coordinates
-- set: AMC_MapName, AMC_Zone, AMC_PosX, AMC_PosY
local function FAMC_GetZone(x, y)
	-- set AMC_PosX, AMC_PosY, and AMC_MapName
	FAMC_GetMapInfos();
	if ( x and y ) then
		AMC_PosX = x;
		AMC_PosY = y;
	end

	-- set AMC_DBbase and AMC_Pbase
	FAMC_SetBaseList();

	-- search for the zone name by coordinates
	AMC_Zone = nil;
	local zonelist = AtlasMajorCities_ZoneList[AMC_MapName];
	if ( zonelist ) then
		local x0 = AMC_PosX;
		local y0 = AMC_PosY;
		for idx = 1, table.getn(zonelist) do
			local x1 = zonelist[idx]["x1"];
			local x2 = zonelist[idx]["x2"];
			local y1 = zonelist[idx]["y1"];
			local y2 = zonelist[idx]["y2"];
			if ( (x0 >= x1) and (x0 <= x2) and (y0 >= y1) and (y0 <= y2) ) then
				AMC_Zone = zonelist[idx]["Zone"];
			end
		end
	end

	-- error if no zones defined yet for this map
	if ( not AMC_Zone ) then
		local error = AtlasMajorCities_loc["errNoZoneDef"]..AMC_MapName;
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
		return;
	end
end

-- get the continent, city, and zone title
function AtlasMajorCities_SetZone(msg)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode < 2 ) then return; end

	-- get AMC_MapName, AMC_Zone, AMC_PosX, AMC_PosY
	FAMC_GetZone();
	if ( not AMC_Zone ) then return; end

	-- get continent
	local continentID = GetCurrentMapContinent();
	local continentNames = { GetMapContinents() };
	for key, val in pairs(continentNames) do
		if ( key == continentID ) then AMC_ContinentText = val; end
	end

	-- get the name of the city
	CityName = GetZoneText();
	local nlevels = GetNumDungeonMapLevels();
	if ( nlevels > 1 ) then
		SetMapToCurrentZone();
		local level = GetCurrentMapDungeonLevel();
		local mapname = strupper(GetMapInfo());
		if ( not _G["DUNGEON_FLOOR_"..mapname..nlevels] ) then level = level - 1; end
		CityName = _G["DUNGEON_FLOOR_"..mapname..level];
	end

	-- get faction of city
	local CityFaction, CityRace;
	local pvpType, isFFA, faction = GetZonePVPInfo();
	if ( pvpType == "friendly" or pvpType == "hostile" ) then
		CityFaction = faction;
	elseif ( pvpType == "sanctuary" ) then
		CityFaction = FACTION_STANDING_LABEL4; -- "Neutral" faction standing
	end

	-- get race of city (from a player target belonging to this city)
	CityRace = UnitRace("target");

	-- create city text
	if ( CityFaction and CityRace) then
		AMC_CityText = CityName.." ("..CityFaction.." - "..CityRace..")";
	elseif ( CityFaction) then
		AMC_CityText = CityName.." ("..CityFaction..")";
	else
		AMC_CityText = CityName;
	end

	-- get (MinimapZoneText) and set zone title
	if ( msg and msg ~= "" ) then
		AMC_ZoneText = msg;
		if ( msg == AMC_DeleteString ) then AMC_ZoneText = nil; end
		AtlasMajorCities_SetZoneDB();
	else
		AMC_ZoneText = GetMinimapZoneText();
		if ( AMC_ZoneText == GetZoneText() ) then
			StaticPopup_Show("AtlasMajorCities_ZoneManually");
		else
			AtlasMajorCities_SetZoneDB();
		end
	end
end

-- set DB entry for city and zone
function AtlasMajorCities_SetZoneDB()
	-- create city entry in DB
	if ( not AtlasMajorCities_DB[AMC_MapName] ) then
		if ( AMC_Pbase or (AMC_ScanMode > 3) ) then
			AtlasMajorCities_DB[AMC_MapName] = {};
		else
			local error = AtlasMajorCities_loc["errNoCityDef"]..AMC_MapName;
			DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
			return;
		end
	end

	-- set continent, city, and zone name
	AtlasMajorCities_Continent[AMC_MapName] = AMC_ContinentText;
	AtlasMajorCities_City[AMC_MapName] = AMC_CityText;
	AtlasMajorCities_Zone[AMC_MapName.."-"..AMC_Zone] = AMC_ZoneText;

	-- reload DB (include Atlas_Refresh)
	AtlasMajorCities_RegisterWithAtlas();

	-- show status msg
	local text;
	if ( AMC_ZoneText ) then
		text = "*** "..AMC_ContinentText.." - "..AMC_CityText.." ("..AMC_MapName..") - "
		text = text..AMC_ZoneText.." ("..AMC_Zone..")";
	else
		text = AtlasMajorCities_loc["msgDelZone"];
		AMC_ZoneText = "";
	end
	DEFAULT_CHAT_FRAME:AddMessage(text, .9, .0, .9);
end

-- set shop assignment
function AtlasMajorCities_AssignShop(msg, ID)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode < 2 ) then return; end

	-- save location for the check only mode
	local old_zone = AMC_Zone;
	local old_map = AMC_MapName;

	-- get AMC_MapName, AMC_Zone, AMC_PosX, AMC_PosY
	if ( ID and tonumber(ID) ) then
		local nid = tonumber(ID);
		local x = math.floor(nid / 1000.0) / 10.0;
		local y = (nid-math.floor(nid / 1000.0) * 1000) / 10.0;
		FAMC_GetZone(x, y);
	else
		FAMC_GetZone();
	end

	if ( not AMC_Zone ) then
		-- in case of check only mode reset to saved location
		if ( (msg == "check") ) then
			AMC_Zone = old_zone;
			AMC_MapName = old_map;
		end
		return;
	end

	-- get shop ID from position
	local sid, psid;
	if ( msg == AMC_NoPosString ) then
		-- set position to zero to assign shop with no position
		AMC_PosX = 0.0;
		AMC_PosY = 0.0;
		sid = "000000";
		isid = tonumber(string.sub(AMC_Zone,5));
	else
		sid = FAMC_GetPositionID(AMC_ShopSize[AMC_MapName]);
		isid = tonumber(sid);
	end
	psid = isid + 1000000;

	-- get shop list from shop ID
	local slist, empty;
	-- first check partial DB
	if ( AMC_Pbase and AMC_Pbase[AMC_MapName] ) then
		slist = AMC_Pbase[AMC_MapName][psid];
		if ( slist and not next(slist) ) then slist = nil; empty = true; end
	end
	-- else take from full DB
	if ( not slist and not empty and AMC_DBbase[AMC_MapName] ) then
		slist = AMC_DBbase[AMC_MapName][isid];
		-- copy DB list entry to partial list
		if ( AMC_Pbase and slist ) then
			if ( not AMC_Pbase[AMC_MapName] ) then
				AMC_Pbase[AMC_MapName] = {};
			end
			AMC_Pbase[AMC_MapName][psid] = slist;
			slist = AMC_Pbase[AMC_MapName][psid];
		end
	end

	-- in case of check only mode reset to saved location
	if ( (msg == "check") ) then
		AMC_Zone = old_zone;
		AMC_MapName = old_map;
		if ( not slist ) then return false; else return true; end
	end

	-- assign shop list
	if ( not slist ) then
		AMC_ShopID = nil;
		AMC_ShopList = nil;
	else
		AMC_ShopList = slist;
		AMC_ShopID = sid;
	end

	-- show status msg
	if ( not slist ) then
		local error = AtlasMajorCities_loc["errNoShopList"];
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
	else
		local title = AtlasMajorCities_GetShopTitleText(slist.sID, AMC_MapName, sid);
		if ( not title ) then title = " "; end
		local text = AtlasMajorCities_loc["msgIsShop"]..AMC_ShopID.." - "..title;
		DEFAULT_CHAT_FRAME:AddMessage(text, .9, .0, .9);
	end
end

-- set shop-position, move the shop or create a new shop entry
function AtlasMajorCities_SetPosition(msg)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode < 4 ) then return; end

	-- delete only assigned shops
	if ( not AMC_ShopID and (msg == AMC_DeleteString) ) then
		local error = AtlasMajorCities_loc["errNoAssignment"];
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
		return;
	end

	-- assign the no position shop
	if ( msg == AMC_DelNoPosString ) then AMC_ShopID = "000000"; end

	-- get AMC_MapName, AMC_Zone, AMC_PosX, AMC_PosY
	local old_map = AMC_MapName;
	local old_zone = AMC_Zone;
	FAMC_GetZone();

	-- check if there is a zone defined
	if ( not AMC_Zone ) then
		AMC_MapName = old_map;
		AMC_Zone = old_zone;
		return;
	end

	-- check for a city change
	if ( (msg == AMC_MoveString) or (msg == AMC_DeleteString) or (msg == AMC_DelNoPosString) ) then
		if ( old_map ~= AMC_MapName ) then
			AMC_MapName = old_map;
			AMC_Zone = old_zone;
			local error = AtlasMajorCities_loc["errNoSameCity"];
			DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
			return;
		end
	end

	-- correct the no position shop ID with the zone number
	if ( AMC_ShopID == "000000" ) then
		AMC_ShopID = "00000"..string.sub(AMC_Zone,5);
	end

	local oldtitle = "";
	local slist, setlabel;
	if ( (msg == AMC_DeleteString) or (msg == AMC_DelNoPosString) ) then
		-- delete the actual shop from the user DB
		AMC_Zone = old_zone;
		oldtitle = FAMC_DeleteAssignedShop();
	else
		-- set position to zero to assign shop with no position
		if ( msg == AMC_NoPosString ) then
			if ( msg == AMC_MoveString ) then
				local error = AtlasMajorCities_loc["errNoMoveNopos"];
				DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
				return;
			end
			AMC_PosX = 0.0;
			AMC_PosY = 0.0;
		end

		local old_shopID = AMC_ShopID;
		-- try to assign a shop to check if there is already a shop at the actual position
		if ( msg == AMC_MoveString ) then
			local ShopFound = AtlasMajorCities_AssignShop("check");
			if ( ShopFound ) then
				local error = AtlasMajorCities_loc["errOldShop"];
				DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
				return;
			end
		end

		-- get shop ID from position
		AMC_ShopID = FAMC_GetPositionID(AMC_ShopSize[AMC_MapName]);
		local new_shopID = AMC_ShopID;
		if ( msg == AMC_MoveString ) then AMC_ShopID = old_shopID; end

		-- move the actual shop to its new position
		if ( msg == AMC_MoveString ) then
			FAMC_MoveAssignedShop(new_shopID);
		end

		local zoneid = tonumber(string.sub(AMC_Zone,5));
		local ishopid = tonumber(AMC_ShopID);
		if ( AMC_ShopID == "000000" ) then ishopid = zoneid; end
		local pshopid = ishopid + 1000000;

		-- set new shop coordinates
		if ( AMC_Pbase and AMC_Pbase[AMC_MapName] and AMC_Pbase[AMC_MapName][pshopid] ) then
			slist = AMC_Pbase[AMC_MapName][pshopid];
			if ( next(slist) ) then
				if ( msg ~= AMC_NoPosString ) then
					slist.PosX = AMC_PosX;
					slist.PosY = AMC_PosY;
				end
			else
				-- this is a deleted shop
				slist = nil;
			end
		elseif (AMC_DBbase[AMC_MapName] and AMC_DBbase[AMC_MapName][ishopid] ) then
			slist = AMC_DBbase[AMC_MapName][ishopid];
			if ( AMC_Pbase ) then
				if ( not AMC_Pbase[AMC_MapName] ) then
					AMC_Pbase[AMC_MapName] = {};
				end
				AMC_Pbase[AMC_MapName][pshopid] = slist;
				slist = AMC_Pbase[AMC_MapName][pshopid];
			end
			if ( msg ~= AMC_NoPosString ) then
				slist.PosX = AMC_PosX;
				slist.PosY = AMC_PosY;
			end
		end

		-- create a new shop and set its position
		setlabel = false;
		if ( not slist and (msg ~= AMC_MoveString) ) then
			slist = {};
			slist.ID = AMC_ShopID;
			slist.Zone = zoneid;
			if ( msg ~= AMC_NoPosString ) then
				slist.PosX = AMC_PosX;
				slist.PosY = AMC_PosY;
			end
			if ( AMC_Pbase ) then
				if ( not AMC_Pbase[AMC_MapName] ) then
					AMC_Pbase[AMC_MapName] = {};
				end
				local old_shop = false;
				if ( AMC_Pbase[AMC_MapName][pshopid] ) then old_shop = true; end
				AMC_Pbase[AMC_MapName][pshopid] = slist;
				if ( not old_shop ) then
					if ( not AtlasMajorCities_Adds[AMC_MapName] ) then AtlasMajorCities_Adds[AMC_MapName] = {}; end
					table.insert(AtlasMajorCities_Adds[AMC_MapName], pshopid);
				end
			else
				AMC_DBbase[AMC_MapName][ishopid] = slist;
			end
			-- set the label
			if ( msg ~= AMC_NoPosString ) then
				setlabel = true;
			end
		end
	end

	-- assign the shop
	if ( not slist ) then
		AMC_ShopID = nil;
		AMC_ShopList = nil;
	else
		if ( msg == AMC_NoPosString ) then
			AtlasMajorCities_AssignShop(AMC_NoPosString);
		else
			AtlasMajorCities_AssignShop();
		end
	end

	-- show status msg
	local text;
	if ( not slist ) then
		if ( msg == AMC_MoveString ) then
			text = AtlasMajorCities_loc["errNoAssignment"];
		else
			text = AtlasMajorCities_loc["msgDelShop"].." - "..oldtitle.." - "..AMC_PosX..", "..AMC_PosY;
		end
	else
		if ( setlabel ) then
			text = AtlasMajorCities_loc["msgCreatedShop"]..AMC_ShopID.." - "..AMC_MapName.." - "..AMC_PosX..", "..AMC_PosY;
		else
			text = AtlasMajorCities_loc["msgChangedShop"]..AMC_ShopID.." - "..AMC_MapName.." - "..AMC_PosX..", "..AMC_PosY;
		end
	end
	DEFAULT_CHAT_FRAME:AddMessage(text, .9, .0, .9);

	if ( setlabel ) then
		-- set the label
		AtlasMajorCities_SetLabel();
	else
		-- reload DB
		Atlas_Refresh();
	end
end

-- get shop label
function AtlasMajorCities_SetLabel(msg)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode < 4 ) then return; end

	if ( not AMC_ShopList ) then
		local error = AtlasMajorCities_loc["errNoAssignment"];
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
		return;
	end

	if ( msg and msg ~= "" ) then
		AMC_ShopLabel = msg;
		if ( msg == AMC_DeleteString ) then AMC_ShopLabel = nil; end
		AtlasMajorCities_SetLabelDB();
	else
		StaticPopup_Show("AtlasMajorCities_LabelManually");
	end
end

-- set DB entry for shop label
function AtlasMajorCities_SetLabelDB()
	if ( AMC_ShopLabel and (AMC_ShopLabel == "") ) then AMC_ShopLabel = nil; end

	AMC_ShopList.Label = AMC_ShopLabel;

	-- show status msg
	local text;
	if ( AMC_ShopLabel ) then
		text = AtlasMajorCities_loc["msgIsLabel"]..AMC_ShopLabel;
	else
		text = AtlasMajorCities_loc["msgDelLabel"];
		AMC_ShopLabel = "";
	end
	DEFAULT_CHAT_FRAME:AddMessage(text, .9, .0, .9);

	-- reload DB
	Atlas_Refresh();
end

-- create a list entry for a shop sign at the actual position
function AtlasMajorCities_SetSign(msg)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode < 4 ) then return; end

	if ( not AMC_ShopList ) then
		local error = AtlasMajorCities_loc["errNoAssignment"];
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
		return;
	end

	-- get AMC_MapName, AMC_PosX, AMC_PosY
	FAMC_GetMapInfos();

	-- check and get shop sign ID from position
	local signID = AMC_ShopList.sID;
	if ( not signID ) then
		if ( msg == AMC_DeleteString ) then
			local error = AtlasMajorCities_loc["errNoShopSign"];
			DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
			return;
		end
		local posID = FAMC_GetPositionID(AMC_ShopSize[AMC_MapName]/2.0);
		signID = AMC_MapName..posID;
	end

	-- check and create a new sign for this shop
	local oldsign = AtlasMajorCities_Shop[signID];
	if ( not oldsign and AMC_Pbase ) then
		oldsign = AtlasMajorCities_Shop0[signID];
	end
	if ( oldsign == "deleted" ) then oldsign = nil; end
	if ( not oldsign ) then
		AMC_ShopList.sID = signID;
		AtlasMajorCities_Shop[signID] = "-";
	end

	-- remove sign if requested
	if ( msg == AMC_DeleteString ) then
		if ( AMC_Pbase and AtlasMajorCities_Shop0[signID] ) then
			AtlasMajorCities_Shop[signID] = "deleted";
		else
			AtlasMajorCities_Shop[signID] = nil;
		end
		AMC_ShopList.sID = nil;
	end

	-- reload DB
	Atlas_Refresh();

	-- show status msg
	if ( oldsign ) then
		if ( msg == AMC_DeleteString ) then
			text = AtlasMajorCities_loc["msgDelSign"]..signID;
			DEFAULT_CHAT_FRAME:AddMessage(text, .9, .0, .9);
		else
			local error = AtlasMajorCities_loc["errOldShopSign"]..oldsign;
			DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
		end
	else
		text = AtlasMajorCities_loc["msgCreatedSign"]..signID;
		DEFAULT_CHAT_FRAME:AddMessage(text, .9, .0, .9);
	end
end

-- get shop comment
function AtlasMajorCities_SetTitle(msg)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode < 2 ) then return; end

	-- check that a shop list is assigned
	if ( not AMC_ShopList ) then
		local error = AtlasMajorCities_loc["errNoAssignment"];
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
		return;
	end

	-- set shop comment
	if ( msg and msg ~= "" ) then
		AMC_ShopText = msg;
		if ( msg == AMC_DeleteString ) then AMC_ShopText = nil; end
		AtlasMajorCities_SetTitleDB();
	else
		AMC_ShopText = AtlasMajorCities_Title[AMC_MapName.."-"..AMC_ShopID];
		if ( AMC_ShopText == "deleted" ) then AMC_ShopText = nil; end
		if ( not AMC_ShopText ) then AMC_ShopText = AtlasMajorCities_Title0[AMC_MapName.."-"..AMC_ShopID]; end
		if ( not AMC_ShopText ) then AMC_ShopText = ""; end
		StaticPopup_Show("AtlasMajorCities_TitleManually");
	end
end

-- set DB entry for shop comment
function AtlasMajorCities_SetTitleDB()
	if ( AMC_ShopText and (AMC_ShopText == "") ) then AMC_ShopText = nil; end

	-- write shop comment in DB
	local idx = AMC_MapName.."-"..AMC_ShopID;
	AtlasMajorCities_Title[idx] = AMC_ShopText;
	if ( not AMC_ShopText and AtlasMajorCities_Title0[idx] ) then AtlasMajorCities_Title[idx] = "deleted"; end

	-- show status msg
	local text;
	if ( AMC_ShopText ) then
		text = AtlasMajorCities_loc["msgIsShopComment"]..AMC_ShopText;
	else
		text = AtlasMajorCities_loc["msgDelTitle"];
		AMC_ShopText = "";
	end
	DEFAULT_CHAT_FRAME:AddMessage(text, .9, .0, .9);

	-- reload DB
	Atlas_Refresh();
end

-- add comment to the target NPC
function AtlasMajorCities_SetComment(msg)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode < 2 ) then return; end
	
	AMC_NPCComment = "";
	local exist;
	if ( TargetFrame:IsVisible() ) then
		-- get npcid
		local _, npcid0 = FAMC_GetNpcID();
		if ( npcid0 ) then
			-- get old NPC-comment
			if ( AtlasMajorCities_Comment[npcid0] ) then
				AMC_NPCComment = AtlasMajorCities_Comment[npcid0];
			elseif ( AMC_Pbase and AtlasMajorCities_Comment0[npcid0] ) then
				AMC_NPCComment = AtlasMajorCities_Comment0[npcid0];
			end

			-- check if NPC exist
			if ( AtlasMajorCities_NPC[npcid0] and (AtlasMajorCities_NPC[npcid0] ~= "deleted") ) then
				exist = true;
			elseif ( AMC_Pbase and AtlasMajorCities_NPC0[npcid0] ) then
				exist = true;
			end
		end
	end

	if ( not exist ) then
		local error = AtlasMajorCities_loc["errNoNPC"];
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
		return;
	end

	-- set NPC comment
	if ( msg and msg ~= "" ) then
		AMC_NPCComment = msg;
		AtlasMajorCities_SetCommentDB();
	else
		StaticPopup_Show("AtlasMajorCities_CommentManually");
	end
end

-- show status msg for NPC comment
function AtlasMajorCities_SetCommentDB(msg)
	if ( AMC_NPCComment and (AMC_NPCComment == "") ) then AMC_NPCComment = nil; end

	-- get npcid
	local _, npcid0 = FAMC_GetNpcID();

	-- add comment to NPC name
	if ( not AMC_NPCComment ) then
		if ( AMC_Pbase and AtlasMajorCities_Comment0[npcid0] ) then
			AtlasMajorCities_Comment[npcid0] = "deleted";
		else
			AtlasMajorCities_Comment[npcid0] = nil;
		end
	else
		AtlasMajorCities_Comment[npcid0] = AMC_NPCComment;
	end

	-- reload DB
	Atlas_Refresh();

	-- get NPC
	local npc = AtlasMajorCities_NPC[npcid0];
	if ( not npc ) then npc = AtlasMajorCities_NPC0[npcid0]; end

	-- show status msg
	local msg;
	if ( AMC_NPCComment ) then
		msg = AtlasMajorCities_loc["msgIsNPCComment"]..npc.." ("..AMC_NPCComment..")";
	else
		msg = AtlasMajorCities_loc["msgDelComment"];
	end
	DEFAULT_CHAT_FRAME:AddMessage(msg, .9, .0, .9);
end

-- set NPC name
local function FAMC_SetNPC(text, npcid)
	if ( not text or not npcid ) then return; end
	local npcid0 = "NPC"..tostring(npcid);

	local new_npc = false;
	-- add NPC to shop list
	if ( not AtlasMajorCities_NPC[npcid0] or (AtlasMajorCities_NPC[npcid0] == "deleted") ) then
		if ( not AMC_Pbase or (AtlasMajorCities_NPC[npcid0] == "deleted") or (AMC_Pbase and not AtlasMajorCities_NPC0[npcid0]) ) then
			if ( AMC_ScanMode > 3 ) then
				table.insert(AMC_ShopList, npcid);
				table.sort(AMC_ShopList);
				new_npc = true;
			else
				local error = AtlasMajorCities_loc["errNoNPCDef"];
				DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
				return;
			end
		end
	end

	-- set NPC name in DB
	AtlasMajorCities_NPC[npcid0] = text;

	-- reload DB
	Atlas_Refresh();

	-- get comment
	local comment = AtlasMajorCities_Comment[npcid0];
	if ( not comment ) then comment = "";
			   else comment = " ("..comment..")";
	end

	-- show status msg
	local msg;
	if ( new_npc ) then
		local label = AMC_ShopList.Label;
		if ( not label ) then label = "-"; end
		msg = AtlasMajorCities_loc["msgCreatedNPC"]..label.." - #"..npcid.." - "..text..comment;
	else
		msg = AtlasMajorCities_loc["msgChangedNPC"]..npcid.." - "..text..comment;
	end
	DEFAULT_CHAT_FRAME:AddMessage(msg, .9, .0, .9);
end

-- remove this NPC from the assigned shop (AMC_ShopList)
function AtlasMajorCities_DeleteNPC(npcid)
	if ( not AMC_ShopList ) then return; end
	if ( not npcid ) then return; end
	local npcid0 = "NPC"..tostring(npcid);

	local comment = AtlasMajorCities_Comment[npcid0];
	if ( comment ) then comment = " ("..comment..")"; else comment = ""; end
	local npc = AtlasMajorCities_NPC[npcid0];
	if ( not npc ) then npc = AtlasMajorCities_NPC0[npcid0]; end
	npc = npc..comment;

	-- remove NPC from shop list
	local pos;
	local index = 0;
	for key, entry in pairs(AMC_ShopList) do
		index = index + 1;
		if ( entry == npcid ) then pos = index; end
	end
	table.remove(AMC_ShopList, pos);

	-- remove NPC name in DB
	if ( AMC_Pbase and AtlasMajorCities_NPC0[npcid0] ) then
		AtlasMajorCities_NPC[npcid0] = "deleted";
	else
		AtlasMajorCities_NPC[npcid0] = nil;
	end

	-- remove comment
	if ( AMC_Pbase and AtlasMajorCities_Comment0[npcid0] ) then
		AtlasMajorCities_Comment[npcid0] = "deleted";
	else
		AtlasMajorCities_Comment[npcid0] = nil;
	end

	-- reload DB
	Atlas_Refresh();

	-- show status msg
	local msg = AtlasMajorCities_loc["msgDelNPC"]..npcid.." - "..npc;
	DEFAULT_CHAT_FRAME:AddMessage(msg, .9, .0, .9);
end

-- get shop sign text and set DB entry
local function FAMC_SetShopSign(SignText)
	-- get actual viewing  direction (degree)
	local angle = math.deg(GetPlayerFacing());

	-- get mouse position relativ to screen center (= viewing direction)
	local xmouse = GetCursorPosition() - UIParent:GetCenter() * UIParent:GetEffectiveScale();

	-- get mouse angle (scale = 0.069 degree per xmouse)
	local amouse = angle - xmouse * 0.069;

	-- set AMC_PosX, AMC_PosY, and AMC_MapName
	FAMC_GetMapInfos();

	-- save actual player position
	local Xpos = AMC_PosX;
	local Ypos = AMC_PosY;

	-- get pre-defined field size of this map
	local size = AMC_ShopSize[AMC_MapName] / 2.0;

	-- scan  field IDs in direction of the mouse pointer for a sign (up to 4x size)
	local oldtext = "";
	local found;
	for i = 0, 20 do
		if ( not found ) then
			AMC_PosX = math.floor((Xpos - math.sin(math.rad(amouse)) * size / 5.0 * i) * 10.0 + 0.5) / 10.0;
			AMC_PosY = math.floor((Ypos - math.cos(math.rad(amouse)) * size / 5.0 * i) * 10.0 + 0.5) / 10.0;
			local signID = AMC_MapName..FAMC_GetPositionID(size);
			-- change sign text
			if ( AtlasMajorCities_Shop[signID] ) then
				oldtext = AtlasMajorCities_Shop[signID];
				AtlasMajorCities_Shop[signID] = SignText;
				found = signID;
			elseif ( AMC_Pbase and AtlasMajorCities_Shop0[signID] ) then
				oldtext = AtlasMajorCities_Shop0[signID];
				AtlasMajorCities_Shop[signID] = SignText;
				found = signID;
			end
		end
	end

	-- show status msg
	if ( found ) then
		local msg = AtlasMajorCities_loc["msgIsShopSign"]..found.." - "..oldtext.." -> "..SignText.." - "..AMC_PosX..", "..AMC_PosY;
		DEFAULT_CHAT_FRAME:AddMessage(msg, .9, .0, .9);
		-- reload DB
		Atlas_Refresh();
	else
		local error = AtlasMajorCities_loc["errNoShopSign"];
		DEFAULT_CHAT_FRAME:AddMessage(error, .9, .0, .0);
	end
end

-- define a scan interval
local AMC_UpdateInterval = 1.0;
local AMC_TimeSinceLastUpdate = 0;

-- scan for NPCs and shop signs
function AtlasMajorCities_Edit_OnUpdate(self, elapsed)
	if ( not AtlasMajorCities_VariablesLoaded ) then return; end
	if ( AMC_ScanMode == 0 ) then return; end
	if ( not AMC_ShopList and (AMC_ScanMode ~= 1) ) then return; end

	AMC_TimeSinceLastUpdate = AMC_TimeSinceLastUpdate + elapsed;
	if (AMC_TimeSinceLastUpdate < AMC_UpdateInterval) then return; end

	-- fix the camera to the player
	if ( (AMC_ScanMode == 3) or (AMC_ScanMode == 5) ) then SetView(1); end

	-- scan shop sign
	if ( IsControlKeyDown() and IsShiftKeyDown() and ( (AMC_ScanMode == 3) or (AMC_ScanMode == 5) ) ) then
		local SignToolTip = AtlasMajorCities_CheckSignToolTip();
		if ( SignToolTip ) then
			local SignText = getglobal("GameTooltipTextLeft1"):GetText();
			if ( SignText ~= AMC_ToolTipSign ) then
				AMC_ToolTipSign = SignText;
				-- set shop sign if there is one
				FAMC_SetShopSign(SignText);
			end
		end
	else
		AMC_ToolTipSign = "";
	end

	-- scan NPC
	if ( TargetFrame:IsVisible() and MouseIsOver(TargetFrame) and GameTooltip:IsVisible() ) then
		-- get npcid
		local npcid = FAMC_GetNpcID();
		if ( npcid and (GameTooltip:NumLines() >= 2) ) then
			-- get NPC name and title from tooltip of NPC target
			local text = FAMC_GetNPCtext();
			if ( text ~= AMC_ToolTipText ) then
				AMC_ToolTipText = text;
				-- add NPC to shop list
				FAMC_SetNPC(text, npcid);
			end
		end
	end

	AMC_TimeSinceLastUpdate = 0;
end

-- functions to setup a pull-down menu for the display mode of the labels

function AtlasMajorCities_LabelMode_PullDown_OnShow(self)
	local labelMode = "";
	if ( AtlasMajorCities_LabelAtPos == -1 ) then labelMode = AtlasMajorCities_loc["movelabel"]; end
	if ( AtlasMajorCities_LabelAtPos ==  0 ) then labelMode = AtlasMajorCities_loc["defaultlabel"]; end
	if ( AtlasMajorCities_LabelAtPos ==  1 ) then labelMode = AtlasMajorCities_loc["shoplabel"]; end
	if ( AtlasMajorCities_LabelAtPos ==  2 ) then labelMode = AtlasMajorCities_loc["signlabel"]; end
	UIDropDownMenu_Initialize(self, AtlasMajorCities_LabelMode_PullDown_Init);
	UIDropDownMenu_SetText(self, labelMode);
end

function AtlasMajorCities_LabelMode_PullDown_Init(self)
	local info = { };
	info.notCheckable = 1;
	info.func = AtlasMajorCities_LabelMode_PullDown_OnClick;

	info.text = AtlasMajorCities_loc["movelabel"];
	info.value = AtlasMajorCities_loc["movelabel"];
	info.notClickable = false;
	UIDropDownMenu_AddButton(info);
	info.text = AtlasMajorCities_loc["defaultlabel"];
	info.value = AtlasMajorCities_loc["defaultlabel"];
	UIDropDownMenu_AddButton(info);
	info.text = AtlasMajorCities_loc["shoplabel"];
	info.value = AtlasMajorCities_loc["shoplabel"];
	UIDropDownMenu_AddButton(info);
	info.text = AtlasMajorCities_loc["signlabel"];
	info.value = AtlasMajorCities_loc["signlabel"];
	UIDropDownMenu_AddButton(info);
	info.text = "";
	info.value = "";
	info.disabled = true;
	UIDropDownMenu_AddButton(info);
	info.disabled = nil;
	info.text = AtlasMajorCities_loc["reload"];
	info.value = AtlasMajorCities_loc["reload"];
	UIDropDownMenu_AddButton(info);
	info.text = AtlasMajorCities_loc["revert"];
	info.value = AtlasMajorCities_loc["revert"];
	UIDropDownMenu_AddButton(info);
end

function AtlasMajorCities_LabelMode_PullDown_OnClick(self)
	local labelMode = self.value;
	AtlasMajorCities_SetScan(labelMode);
	if ( AtlasMajorCities_LabelAtPos == -1 ) then labelMode = AtlasMajorCities_loc["movelabel"]; end
	if ( AtlasMajorCities_LabelAtPos ==  0 ) then labelMode = AtlasMajorCities_loc["defaultlabel"]; end
	if ( AtlasMajorCities_LabelAtPos ==  1 ) then labelMode = AtlasMajorCities_loc["shoplabel"]; end
	if ( AtlasMajorCities_LabelAtPos ==  2 ) then labelMode = AtlasMajorCities_loc["signlabel"]; end
	UIDropDownMenu_SetText(AMC_LabelMode_PullDown, labelMode);
end

-- init slash commands and the label mode pull-down menu
function AtlasMajorCities_Edit_OnLoad(self)
	SlashCmdList["AtlasMajorCitiesHelp"] = AtlasMajorCities_SetScan;
	SLASH_AtlasMajorCitiesHelp1 = "/amc";

	SlashCmdList["AtlasMajorCitiesZone"] = AtlasMajorCities_SetZone;
	SLASH_AtlasMajorCitiesZone1 = "/amcz";

	SlashCmdList["AtlasMajorCitiesPosition"] = AtlasMajorCities_SetPosition;
	SLASH_AtlasMajorCitiesPosition1 = "/amcp";

	SlashCmdList["AtlasMajorCitiesAssign"] = AtlasMajorCities_AssignShop;
	SLASH_AtlasMajorCitiesAssign1 = "/amca";

	SlashCmdList["AtlasMajorCitiesLabel"] = AtlasMajorCities_SetLabel;
	SLASH_AtlasMajorCitiesLabel1 = "/amcl";

	SlashCmdList["AtlasMajorCitiesScan"] = AtlasMajorCities_SetSign;
	SLASH_AtlasMajorCitiesScan1 = "/amcs";

	SlashCmdList["AtlasMajorCitiesTitle"] = AtlasMajorCities_SetTitle;
	SLASH_AtlasMajorCitiesTitle1 = "/amct";

	SlashCmdList["AtlasMajorCitiesComment"] = AtlasMajorCities_SetComment;
	SLASH_AtlasMajorCitiesComment1 = "/amcc";

	-- create pull-down menu for the display mode of the labels
	AMC_LabelMode_TextFrame = CreateFrame("FRAME", "AMC_LabelMode_TextFrame", AtlasFrame);
	AMC_LabelMode_TextString = AMC_LabelMode_TextFrame:CreateFontString("AMC_LabelMode_TextString", "MEDIUM", "GameFontNormalSmall");
	AMC_LabelMode_TextString:SetPoint("CENTER", AMC_LabelMode_TextFrame, "CENTER", 0, 0);
	AMC_LabelMode_TextString:SetText(AtlasMajorCities_loc["labelPullDown"]);
	AMC_LabelMode_TextFrame:SetWidth(AMC_LabelMode_TextString:GetWidth());
	AMC_LabelMode_TextFrame:SetHeight(30);
	AMC_LabelMode_TextFrame:SetPoint("TOPLEFT", AtlasFrame, "TOPLEFT", 80, -9);

	AMC_LabelMode_PullDown = CreateFrame("FRAME", "AMC_LabelMode_PullDown", AtlasFrame, "UIDropDownMenuTemplate");
	AMC_LabelMode_PullDown:SetWidth(160);
	AMC_LabelMode_PullDown:SetHeight(40);
	AMC_LabelMode_PullDown:SetPoint("TOPLEFT", AMC_LabelMode_TextFrame, "TOPRIGHT", -10, 0);
	AMC_LabelMode_PullDown:SetScript("OnLoad", function(self) getglobal(self:GetName().."Text"):SetJustifyH("LEFT"); end)
	AMC_LabelMode_PullDown:SetScript("OnShow", function(self) AtlasMajorCities_LabelMode_PullDown_OnShow(self); end)

	AMC_LabelMode_TextFrame:Hide();
	AMC_LabelMode_PullDown:Hide();
end


-- functions called from normal mode function (needs allways to be defined)

-- write the new label position to the UserDB
function AtlasMajorCities_Label_SetNewPos(self)
	-- works only for the UserDB
	if ( not AtlasMajorCities_UserDB or (AtlasMajorCities_LabelAtPos ~= -1) ) then return; end

	-- is the mouse inside the image ?
	local newx = self.x1 + self.x3 - self.x2;
	local newy = self.y1 + self.y3 - self.y2;
	if (newx<0 or newx>512 or newy>0 or newy<-512) then return; end

	local city = AtlasMajorCities_ShownCity;
	local ID = self:GetID();
	local shop = string.sub(tostring(ID + 1000000),2);
	FAMC_SetBaseList();

	-- get the shop
	local slist = AMC_DBbase[city][ID];
	if ( AMC_Pbase ) then
		if ( not AMC_Pbase[city] ) then
			AMC_Pbase[city] = {};
		end
		if ( not AMC_Pbase[city][ID + 1000000] ) then
			AMC_Pbase[city][ID + 1000000] = slist;
		end
		slist = AMC_Pbase[city][ID + 1000000];
	end

	-- shop position
	local sx = tonumber(string.sub(shop,1,3)) / 10.0;
	local sy = tonumber(string.sub(shop,4,6)) / 10.0;
	-- new label position
	local x1 = AtlasMajorCities_XMapCoordSkip[AtlasMajorCities_ShownCity.."1"];
	local x2 = AtlasMajorCities_XMapCoordSkip[AtlasMajorCities_ShownCity.."2"];
	local wmap = AtlasMap:GetWidth();
	local hmap = AtlasMap:GetHeight();
	local px = math.floor((newx / wmap * (100.0 - x1 - x2) + x1) * 10.0) / 10.0;
	local py = math.floor((-newy / hmap * 100.0) * 10.0) / 10.0;
	-- allow label a max. distance of 5% of map size to its shop position
	-- (consider the different scale of the x-axis -> 1/3 larger then y-axis)
	if ( (math.abs(sx-px) < 5.0/0.66) and (math.abs(sy-py) < 5.0) ) then
		slist.PosX = px;
		slist.PosY = py;
		self.x1 = newx;
		self.y1 = newy;
	end
end

-- create the menu table for a label
function AtlasMajorCities_Label_Menu(self)
	-- works only for UserDB
	if ( not AtlasMajorCities_UserDB or (AtlasMajorCities_LabelAtPos ~= 1) ) then return; end

	-- assign works only for actual city
	FAMC_GetZone();
	if ( (AtlasMajorCities_ShownCity ~= AMC_MapName) or not AMC_Zone ) then return; end

	-- only to be used in the all scan mode
	if ( AMC_ScanMode < 4 ) then return; end

	local city = AtlasMajorCities_ShownCity;
	local ID = self:GetID();
	FAMC_SetBaseList();

	-- get the shop
	local slist;
	if ( AMC_Pbase and AMC_Pbase[city] ) then slist = AMC_Pbase[city][ID + 1000000]; end
	if ( not slist ) then slist = AMC_DBbase[city][ID]; end

	local shop = string.sub(tostring(ID + 1000000),2);
	local shopname = AtlasMajorCities_GetShopTitleText(slist.sID, city, shop);
	local menu = {};
	if ( shopname ) then menu = { { text = shopname, isTitle = true, notCheckable = true } }; end
	if ( ID ~= tonumber(AMC_ShopID) ) then
		-- allow assigning to this shop
		local entry = { text = "Assign to this shop", value = ID, func = function(self)
			AtlasMajorCities_AssignShop(nil, self.value);
			end, notCheckable = true };
		table.insert(menu, entry);
	else
		-- get NPCs
		local skey, sval;
		for skey, sval in pairs(slist) do
			if ( (type(skey) == "number") and AtlasMajorCities_GetNPCText(sval) ) then
				local npc = "DELETE: "..AtlasMajorCities_GetNPCText(sval);
				entry = { text = npc, value = sval, func = function(self)
					AtlasMajorCities_DeleteNPC(self.value);
					end, notCheckable = true };
				table.insert(menu, entry)
			end
		end
	end

	return menu;
end

else -- AtlasMajorCities_EditMode

-- write the new label position to the UserDB
function AtlasMajorCities_Label_SetNewPos(self)
end

-- create the menu table for a label
function AtlasMajorCities_Label_Menu(self)
	local menu = {};
	return menu;
end

end -- AtlasMajorCities_EditMode
