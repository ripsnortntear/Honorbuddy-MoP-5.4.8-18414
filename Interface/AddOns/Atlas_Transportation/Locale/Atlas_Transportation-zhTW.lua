-- $Id: Atlas_Transportation-zhTW.lua 2135 2013-12-15 14:16:49Z ananhaid $
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

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Transportation", "zhTW", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	AL["The Bogpaddle Bullet"] = "沼槳火箭";
	AL["Death Knight Only"] = "僅限死亡騎士";	-- Taxi node in Acherus: The Ebon Hold, which is only for Death Knight
	AL["Druid-only"] = "僅限德魯伊";		-- Taxi node in Nighthaven, Moonglade which is only for Druid
	AL["Legend"] = "圖例";				-- The chart's legend, for example, the purple line means the portal's path
	AL["Gryphon"] = "獅鷲獸";
	AL["Only available after winning the PvP battle"] = "僅限贏得戰場勝利後";
	AL["Orb of Translocation"] = "傳送之門";	-- The Orb in Silvermonn City and Ruins of Lordaeron
	AL["Portals"] = "傳送門";
	AL["Portal / Waygate Path to the destination"] = "傳送門 / 甬道之門傳往目的地的路徑";
	AL["Ship / Zeppelin sailing path to destination"] = "船隻 / 飛船航向目的地的路徑";
	AL["Requires honored faction with Sha'tari Skyguard"] = "需薩塔空防陣營榮譽";
	AL["Seahorse"] = "海馬";
	AL["South of the path along Lake Elune'ara"] = "月神湖南方小徑";
	AL["Special transportation"] = "特殊運輸工具";
	AL["Taxi Nodes"] = "航行點";
	AL["Transportation Maps"] = "交通網路地圖";
	AL["Transporter"] = "輸送者";
	AL["Transporters by the sea and on the cliff"] = "傳送器位於懸崖上與懸崖下"; -- The transporters (machine) can be found at Fuselight-by-the-Sea
	AL["West of the path to Timbermaw Hold"] = "往木喉要塞小徑西方";
	AL["Wind Rider"] = "雙足飛龍";
	AL["Won't be available once the Battle for Andorhal chain is finished."] = "安多哈爾任務線完成後飛行點將會消失"; -- After quest "Alas, Andorhal" (27206) is completed.
	AL["Zeppelin Towers"] = "飛船空塔";
	AL["Climbing Rope"] = "登山繩";
	AL["Rappelling Rope"] = "垂降繩";
	AL["Abandoned Kite"] = "被遺棄的鳶";
	AL["From sea level to ground level"] = "從海平面到地面";
	AL["Whispercloud's Balloon"] = "雲語的氣球";
	AL["Shado-Pan Rope"] = "影潘索 "; -- 66390
	AL["Require to complete \"Meet the Scout\" quest line first."] = "需要先完成“和斥候會面”任務線。";
end
