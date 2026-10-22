-- $Id: Atlas_Transportation-esMX.lua 1891 2013-02-17 15:41:01Z arithmandar $
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

--[[

-- Datos de Atlas (Español)
-- Traducido por --> maqjav|Marosth de Tyrande<--
-- maqjav@hotmail.com
-- Úlltima Actualización (last update): 28/10/2012

--]]
local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_Transportation", "esMX", false);
-- Localize file must set above to false, for example:
--    local AL = AceLocale:NewLocale("Atlas", "deDE", false);

if AL then
	AL["The Bogpaddle Bullet"] = "La bala de Chapaleos";
	AL["Death Knight Only"] = "Solo caballeros de la muerte";		-- Taxi node in Acherus: The Ebon Hold, which is only for Death Knight
	AL["Druid-only"] = "Solo druidas";			-- Taxi node in Nighthaven, Moonglade which is only for Druid
	AL["Legend"] = "Leyenda";				-- The chart's legend, for example, the purple line means the portal's path
	AL["Gryphon"] = "Grifo";
	AL["Only available after winning the PvP battle"] = "Disponible únicamente despues de haber ganado la batalla JcJ";
	AL["Orb of Translocation"] = "Orbe de traslado";	-- The Orb in Silvermonn City and Ruins of Lordaeron
	AL["Portals"] = "Portales";				
	AL["Portal / Waygate Path to the destination"] = "Portal / Puerta con destino";
	AL["Ship / Zeppelin sailing path to destination"] = "Barco / Velero Zepelín con destino";
	AL["Requires honored faction with Sha'tari Skyguard"] = "Requiere honorable con la facción Guardia del cielo Sha'tari";
	AL["Seahorse"] = "Cabllito de mar";				
	AL["South of the path along Lake Elune'ara"] = "Sur del camino a Lago Elune'ara";
	AL["Special transportation"] = "Transporte especial";
	AL["Taxi Nodes"] = "Paradas de Taxi";			
	AL["Transportation Maps"] = "Mapas de Transportes";
	AL["Transporter"] = "Transportador";			-- The NPC who can transport you to other place
	AL["Transporters by the sea and on the cliff"] = "Transportes por mar y por acantilados"; -- The transporters (machine) can be found at Fuselight-by-the-Sea
	AL["West of the path to Timbermaw Hold"] = "Oeste del camino al Puesto Vigóa del Cubil";
	AL["Wind Rider"] = "Jinete del viento";
	AL["Won't be available once Thassarian departed"] = "No estará disponible una vez se marche Thassarian"; -- After quest "Alas, Andorhal" (27206) is completed.
	AL["Zeppelin Towers"] = "Aeropuertos Zepelín";		
	AL["Climbing Rope"] = "Cuerda de escalar";
	AL["Rappelling Rope"] = "Soga de rápel";
	AL["Abandoned Kite"] = "Cometa abandonada";
	AL["From sea level to ground level"] = "Del nivel del mar al nivel de tierra";
	AL["Whispercloud's Balloon"] = "Globo de la Nube Susurrante";
	AL["Shado-Pan Rope"] = "Cuerda del Shadopan"; -- 66390
end