--------------------------------------------------------------------------
-- GTFO_Spells_WOD.lua 
--------------------------------------------------------------------------
--[[
GTFO Spell List - Warlords of Draenor
Author: Zensunim of Malygos
]]--

-- ***********
-- * Draenor *
-- ***********

GTFO.SpellID["171406"] = {
	--desc = "Burning (Kargathar Proving Grounds)";
	sound = 1;
};

GTFO.SpellID["164177"] = {
	--desc = "Magma Pool (Blackrock Slaghauler)";
	sound = 1;
};


GTFO.SpellID["166031"] = {
	--desc = "Crush (Ogron Warcrusher)";
	sound = 1;
};

GTFO.SpellID["166534"] = {
	--desc = "Ruptured Earth (Gronn)";
	sound = 1;
};

GTFO.SpellID["161918"] = {
	--desc = "Fiery Ground (Blazing Pyreclaw)";
	sound = 1;
};

--TODO: Acid Breath (Drov the Ruiner) - avoidable?
--TODO: Noxious Spit (Tarlna the Ageless) - pool

-- **************
-- * Auchindoun *
-- **************

GTFO.SpellID["166749"] = {
	--desc = "Mind Sear (Sargerei Soulbinder)";
	sound = 4;
	negatingDebuffSpellID = 166749; -- Mind Sear?
	test = true; -- Verify negating debuff spell ID
};

GTFO.SpellID["153430"] = {
	--desc = "Sanctified Ground - Debuff (Soul Construct)";
	sound = 1;
};

GTFO.SpellID["161457"] = {
	--desc = "Sanctified Ground (Soul Construct)";
	sound = 1;
};

GTFO.SpellID["154187"] = {
	--desc = "Soul Vessel (Soulbinder Nyami)";
	sound = 1;
};

GTFO.SpellID["153616"] = {
	--desc = "Fel Pool (Azzakel)";
	sound = 1;
};

-- TODO: Rain of Fire (Teron'gor)

-- ************************
-- * Bloodmaul Slag Mines *
-- ************************

GTFO.SpellID["151638"] = {
	--desc = "Suppression Field (Bloodmaul Overseer?)";
	sound = 1;
};

GTFO.SpellID["150011"] = {
	--desc = "Magma Barrage (Forgemaster Gog'duh)";
	sound = 1;
};

GTFO.SpellID["149996"] = {
	--desc = "Firestorm (Forgemaster Gog'duh)";
	sound = 1;
};

GTFO.SpellID["153227"] = {
	--desc = "Burning Slag (Roltall)";
	sound = 1;
};

GTFO.SpellID["164616"] = {
	--desc = "Channel Flames (Bloodmaul Flamespeaker)";
	sound = 1;
};

GTFO.SpellID["150784"] = {
	--desc = "Magma Eruption (Gug'rokk)";
	sound = 1;
};

-- ******************
-- * Grimrail Depot *
-- ******************

GTFO.SpellID["161220"] = {
	--desc = "Slag Tanker";
	sound = 1;
	test = true; -- Spammy?
};

GTFO.SpellID["167038"] = {
	--desc = "Slag Tanker";
	applicationOnly = true;
	sound = 1;
};

GTFO.SpellID["166340"] = {
	--desc = "Thunder Zone (Iron Horde Far Seer)";
	applicationOnly = true;
	sound = 1;
};

GTFO.SpellID["171902"] = {
	--desc = "Thunderous Breath (Rakun)";
	sound = 1;
};

GTFO.SpellID["161588"] = {
	--desc = "Diffused Energy (Skylord Tov'osh)";
	applicationOnly = true;
	sound = 1;
};

-- **************
-- * Iron Docks *
-- **************

-- Lava Sweep (Makogg Emberblade) - Fail?
-- TODO: Shattering Blade (Koramar) - avoidable?

-- Barbed Arrow Barrage (Fleshrender Nok'gar)
-- Burning Arrows (Fleshrender Nok'gar) - fire
-- Shredding Swipes (Fleshrender Nok'gar) - Fail?

GTFO.SpellID["168540"] = {
	--desc = "Cannon Barrage (Skulloc)";
	sound = 1;
};

GTFO.SpellID["168514"] = {
	--desc = "Cannon Barrage (Skulloc)";
	sound = 1;
};

GTFO.SpellID["173105"] = {
	--desc = "Whirling Chains";
	sound = 1;
};

GTFO.SpellID["173149"] = {
	--desc = "Flaming Arrows";
	sound = 1;
};

GTFO.SpellID["173489"] = {
	--desc = "Lava Barrage (Ironwing Flamespitter)";
	sound = 1;
};

GTFO.SpellID["172963"] = {
	--desc = "Gatecrasher (Siegemaster Rokra)";
	sound = 1;
};

GTFO.SpellID["178156"] = {
	--desc = "Acid Splash (Rylak Skyterror)";
	sound = 1;
};

GTFO.SpellID["168390"] = {
	--desc = "Cannon Barrage (Skulloc)";
	sound = 1;
};

GTFO.SpellID["168348"] = {
	--desc = "Rapid Fire (Zoggosh)";
	negatingDebuffSpellID = 168398; -- Rapid Fire Targetting
	negatingIgnoreTime = 7;
	sound = 4;
	test = true; -- A little weird at times
};


-- *****************************
-- * Shadowmoon Burial Grounds *
-- *****************************

GTFO.SpellID["152854"] = {
	--desc = "Void Sphere (Shadowmoon Loyalist)";
	sound = 1;
};

GTFO.SpellID["158061"] = {
	--desc = "Blessed Waters of Purity";
	sound = 2;
};

GTFO.SpellID["153224"] = {
	--desc = "Shadow Burn (Sadana Bloodfury)";
	sound = 1;
};

GTFO.SpellID["153070"] = {
	--desc = "Void Devastation (Nhallish)";
	sound = 1;
};

GTFO.SpellID["153501"] = {
	--desc = "Void Blast (Nhallish)";
	sound = 1;
};

GTFO.SpellID["153692"] = {
	--desc = "Necrotic Pitch (Bonemaw)";
	sound = 1;
};

GTFO.SpellID["154469"] = {
	--desc = "Ritual of Bones (Ner'zhul)";
	sound = 1;
};


-- ************
-- * Skyreach *
-- ************

GTFO.SpellID["153139"] = {
	--desc = "Four Winds (Ranjit)";
	sound = 1;
};

GTFO.SpellID["153759"] = {
	--desc = "Windwall (Ranjit)";
	sound = 1;
};

GTFO.SpellID["159226"] = {
	--desc = "Solar Storm (Skyreach Arcanologist)";
	sound = 1;
};

GTFO.SpellID["154043"] = {
	--desc = "Lens Flare (High Sage Viryx)";
	sound = 1;
};

-- *****************
-- * The Everbloom *
-- *****************

GTFO.SpellID["172579"] = {
	--desc = "Bounding Whirl (Melded Berserker)";
	sound = 1;
};

GTFO.SpellID["169495"] = {
	--desc = "Living Leaves (Witherbark)";
	sound = 1;
};

GTFO.SpellID["164294"] = {
	--desc = "Unchecked Growth (Witherbark)";
	sound = 1;
};

GTFO.SpellID["167977"] = {
	--desc = "Bramble Patch (Earthshaper Telu)";
	sound = 1;
};

GTFO.SpellID["166726"] = {
	--desc = "Frozen Rain (Archmage Sol)";
	sound = 1;
};

GTFO.SpellID["169223"] = {
	--desc = "Toxic Gas (Xeri'tac)";
	sound = 1;
};

-- *************************
-- * Upper Blackrock Spire *
-- *************************

GTFO.SpellID["154345"] = {
	--desc = "Electric Pulse (Orebender Gor'ashan)";
	sound = 1;
};

GTFO.SpellID["161288"] = {
	--desc = "Vileblood pool (Kyrak)";
	sound = 1;
};

GTFO.SpellID["161772"] = {
	--desc = "Incinerating Breath (Ironbarb Skyreaver)";
	sound = 1;
};

GTFO.SpellID["161833"] = {
	--desc = "Noxious Spit (Ironbarb Skyreaver)";
	sound = 1;
};

GTFO.SpellID["162097"] = {
	--desc = "Imbued Iron Axe (Tharbek)";
	sound = 1;
};

GTFO.SpellID["155057"] = {
	--desc = "Magma Pool (Ragewing)";
	sound = 1;
};

-- TODO: Black Iron Cyclone (Warlord Zaela)


-- *********************
-- * Blackrock Foundry *
-- *********************

GTFO.SpellID["175643"] = {
	--desc = "Spinning Blade (Workshop Guardian)";
	sound = 1;
};

GTFO.SpellID["159686"] = {
	--desc = "Acidback Puddle (Darkshard Acidback)";
	sound = 1;
};

GTFO.SpellID["159520"] = {
	--desc = "Gripping Slag (Iron Slag-Shaper)";
	sound = 4;
	ignoreSelfInflicted = true;
};

GTFO.SpellID["175605"] = {
	--desc = "Gripping Slag (Iron Slag-Shaper)";
	sound = 4;
	ignoreSelfInflicted = true;
};

GTFO.SpellID["173192"] = {
	--desc = "Cave In (Gruul)";
	sound = 1;
};

GTFO.SpellID["156203"] = {
	--desc = "Retched Blackrock (Oregorger)";
	sound = 1;
};

GTFO.SpellID["156388"] = {
	--desc = "Explosive Shard - Initial Hit (Oregorger)";
	sound = 3;
};

GTFO.SpellID["156932"] = {
	--desc = "Rupture (Foreman Feldspar)";
	sound = 1;
};

GTFO.SpellID["155743"] = {
	--desc = "Slag Pool (Heart of the Mountain)";
	sound = 1;
};

GTFO.SpellID["155223"] = {
	--desc = "Melt (Heart of the Mountain)";
	sound = 1;
};

GTFO.SpellID["160260"] = {
	--desc = "Fire Bomb (Blackrock Enforcer)";
	sound = 1;
};

GTFO.SpellID["177806"] = {
	--desc = "Furnace Flame";
	sound = 1;
};

GTFO.SpellID["162663"] = {
	--desc = "Electrical Storm (Thunderlord Beast-Tender)";
	sound = 1;
};

-- Beastlord Darmac
-- TODO: Epicenter - avoidable?

GTFO.SpellID["154989"] = {
	--desc = "Inferno Breath (Beastlord Darmac)";
	sound = 1;
	tankSound = 0; -- Avoidable by tank?
	applicationOnly = true;
};

GTFO.SpellID["156824"] = {
	--desc = "Inferno Pyre (Beastlord Darmac)";
	sound = 1;
};

GTFO.SpellID["155718"] = {
	--desc = "Conflagration (Beastlord Darmac)";
	sound = 4;
	ignoreSelfInflicted = true;	
};

GTFO.SpellID["155499"] = {
	--desc = "Superheated Shrapnel (Beastlord Darmac)";
	sound = 1;
	applicationOnly = true;
};

GTFO.SpellID["156823"] = {
	--desc = "Superheated Scrap (Beastlord Darmac)";
	sound = 1;
};

-- Flamebender Ka'graz
-- TODO: Singe - too many stacks?

GTFO.SpellID["155314"] = {
	--desc = "Lava Slash (Flamebender Ka'graz)";
	sound = 1;
};

GTFO.SpellID["156713"] = {
	--desc = "Unquenchable Flame (Flamebender Ka'graz)";
	sound = 1;
	test = true;
};

GTFO.SpellID["155484"] = {
	--desc = "Blazing Radiance (Flamebender Ka'graz)";
	sound = 4;
	ignoreSelfInflicted = true;	
};

GTFO.SpellID["155818"] = {
	--desc = "Scorching Burns (Hans'gar and Franzok)";
	sound = 1;
};

GTFO.SpellID["161570"] = {
	--desc = "Searing Plates (Hans'gar and Franzok)";
	sound = 1;
};

-- Operator Thogar
-- TODO: Lava Shock - avoidable?
-- TODO: Obliteration - avoidable?
-- TODO: Heat Blast - avoidable?

GTFO.SpellID["156932"] = {
	--desc = "Rupture (Foreman Feldspar)";
	sound = 1;
};

GTFO.SpellID["155223"] = {
	--desc = "Melt (Foreman Feldspar)";
	sound = 1;
};

-- The Iron Maidens
-- TODO: Convulsive Shadows, Lingering Shadows?

GTFO.SpellID["156637"] = {
	--desc = "Rapid Fire (Admiral Gar'an)";
	sound = 1;
};

GTFO.SpellID["158683"] = {
	--desc = "Corrupted Blood (The Iron Maidens)";
	sound = 1;
};

-- Blackhand
GTFO.SpellID["156401"] = {
	--desc = "Molten Slag (Blackhand)";
	sound = 1;
};

-- ************
-- * Highmaul *
-- ************

GTFO.SpellID["161635"] = {
	--desc = "Molten Bomb (Vul'gor)";
	sound = 1;
};

GTFO.SpellID["159413"] = {
	--desc = "Mauling Brew (Kargath Bladefist)";
	sound = 1;
};

GTFO.SpellID["159311"] = {
	--desc = "Flame Jet (Kargath Bladefist)";
	sound = 1;
};

GTFO.SpellID["159002"] = {
	--desc = "Berserker Rush (Kargath Bladefist)";
	sound = 1;
};

GTFO.SpellID["156138"] = {
	--desc = "Heavy Handed (The Butcher)";
	sound = 1;
	tankSound = 0;
};

-- The Butcher
GTFO.SpellID["163046"] = {
	--desc = "Pale Vitorl (The Butcher)";
	sound = 1;
};

-- Tectus
-- TODO: Petrification (Tectus) -- How many stacks is too many?

GTFO.SpellID["162370"] = {
	--desc = "Crystalline Barrage (Tectus)";
	sound = 1;
	test = true; -- Avoidable if you're marked?
};

GTFO.SpellID["173232"] = {
	--desc = "Flamethrower (Iron Flame Technician)";
	sound = 1;
};

GTFO.SpellID["163590"] = {
	--desc = "Creeping Moss (Brackenspore)";
	sound = 1;
};

GTFO.SpellID["159220"] = {
	--desc = "Necrotic Breath (Brackenspore)";
	sound = 1;
	tankSound = 0;
};

GTFO.SpellID["164642"] = {
	--desc = "Infested Waters (Brackenspore)";
	sound = 1;
};

GTFO.SpellID["160179"] = {
	--desc = "Mind Fungus (Brackenspore)";
	sound = 2;
	casterOnly = true;
	test = true;
};

GTFO.SpellID["157944"] = {
	--desc = "Whirlwind (Phemos)";
	sound = 1;
	tankSound = 0;
};

GTFO.SpellID["158241"] = {
	--desc = "Blaze (Phemos)";
	sound = 1;
	applicationOnly = true;
};

-- Ko'ragh
-- TODO: Expel Magic: Fel (Ko'ragh, Heroic) -- fire?

GTFO.SpellID["159220"] = {
	--desc = "Suppression Field (Ko'ragh)";
	sound = 1;
	tankSound = 0;
	test = true; -- How does this mechanic work strat-wise?  Tanks may have to drag adds into the field to prevent explosions, but does DPS ever need to stand there?
};

-- Imperator Mar'gok
-- TODO: Force Nova (Imperator Mar'gok) -- avoidable?

