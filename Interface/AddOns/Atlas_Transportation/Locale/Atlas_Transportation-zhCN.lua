-- $Id: Atlas_Transportation-zhCN.lua 2135 2013-12-15 14:16:49Z ananhaid $
--[[

	Atlas, a World of Warcraft instance map browser
	Copyright 2005 ~ 2010 - Dan Gilbert <dan.b.gilbert@gmail.com>
	Copyright 2010 - Lothaer <lothayer@gmail.com>, Atlas Team
	Copyright 2011 ~ 2013 - Arith Hsu, Atlas Team <atlas.addon@gmail.com>

	This file is part of Atlas.

	Atlas is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Atlas is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Atlas; if not, write to the Free Software
	Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

--]]
-- Atlas Localization Data (Simplified Chinese)
-- Initial translation by DiabloHu
-- Maintained by DiabloHu, arith, Ananhaid

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Transportation", "zhCN", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	AL["The Bogpaddle Bullet"] = "沼桨弹头";
	AL["Death Knight Only"] = "死亡骑士专用";	-- Taxi node in Acherus: The Ebon Hold, which is only for Death Knight
	AL["Druid-only"] = "德鲁伊专用";			-- Taxi node in Nighthaven, Moonglade which is only for Druid
	AL["Legend"] = "图例";				-- The chart's legend, for example, the purple line means the portal's path
	AL["Gryphon"] = "狮鹫";
	AL["Only available after winning the PvP battle"] = "只有赢下 PvP 战斗后可用";
	AL["Orb of Translocation"] = "传送宝珠";	-- The Orb in Silvermonn City and Ruins of Lordaeron
	AL["Portals"] = "传送门";
	AL["Portal / Waygate Path to the destination"] = "传送门 / 界门传往目的地的路径";
	AL["Ship / Zeppelin sailing path to destination"] = "船只 / 飞船航向目的地的路径";
	AL["Requires honored faction with Sha'tari Skyguard"] = "需要沙塔尔天空卫队声望尊敬";
	AL["Seahorse"] = "海马";
	AL["South of the path along Lake Elune'ara"] = "月神湖旁小径的南方";
	AL["Special transportation"] = "特殊交通";
	AL["Taxi Nodes"] = "飞行点";
	AL["Transportation Maps"] = "交通线路图";
	AL["Transporter"] = "传送者";			-- The NPC who can transport you to other place
	AL["Transporters by the sea and on the cliff"] = "在海上或悬崖上的传送者"; -- The transporters (machine) can be found at Fuselight-by-the-Sea
	AL["West of the path to Timbermaw Hold"] = "通往木喉要塞道路的西方";
	AL["Wind Rider"] = "驭风者";
	AL["Won't be available once the Battle for Andorhal chain is finished."] = "“安多哈尔之战”任务链完成之前不可使用。"; -- After quest "Alas, Andorhal" (27206) is completed.
	AL["Zeppelin Towers"] = "飞艇塔";
	AL["Climbing Rope"] = "登山绳索";
	AL["Rappelling Rope"] = "垂降绳索";
	AL["Abandoned Kite"] = "被遗弃的风筝";
	AL["From sea level to ground level"] = "从海平面到陆地";
	AL["Whispercloud's Balloon"] = "语云的热气球";
	AL["Shado-Pan Rope"] = "影踪派绳索"; -- 66390
	AL["Require to complete \"Meet the Scout\" quest line first."] = "需要先完成“会见斥候”任务线。";
end
