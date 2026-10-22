--------------------------------------------------------------------------
-- GTFO_Fail_WOD.lua 
--------------------------------------------------------------------------
--[[
GTFO Fail List - Warlords of Draenor
Author: Zensunim of Malygos
]]--

-- ***********
-- * Draenor *
-- ***********

GTFO.SpellID["158834"] = {
	--desc = "Acid Burst (Acidback)";
	sound = 3;
};

GTFO.SpellID["150790"] = {
	--desc = "Ground Slap (Gronn)";
	sound = 3;
};

GTFO.SpellID["173481"] = {
	--desc = "Cannon Blast (Gogluck)";
	sound = 3;
};

GTFO.SpellID["152750"] = {
	--desc = "Sonic Screech (Chillfang)";
	sound = 3;
};

--TODO: Colossal Slam (Drov the Ruiner) - non-tank fail for front-cone damage
--TODO: Rumbling Goren (Drov the Ruiner) - non-tank fail?
--TODO: Colossal Blow (Tarlna the Ageless) - avoidable by tanks too?
--TODO: Savage Vines (Tarlna the Ageless) - explosion fail if not targetted
--TODO: Blaze of Glory (Rukhmar) -- ?
--TODO: Loose Quills (Rukhmar) -- ?
--TODO: Solar Breath (Rukhmar) -- non-tank fail

-- **************
-- * Auchindoun *
-- **************

GTFO.SpellID["166749"] = {
	--desc = "Void Strikes (Sargerei Hoplite)";
	sound = 3;
	tankSound = 0;
};

GTFO.SpellID["157786"] = {
	--desc = "Radiant Fury (Spiteful Arbiter)";
	sound = 3;
};

GTFO.SpellID["154526"] = {
	--desc = "Hallowed Ground (Soul Construct)";
	sound = 3;
};

GTFO.SpellID["157792"] = {
	--desc = "Arcane Bomb (Sargerei Magus)";
	sound = 3;
};

GTFO.SpellID["154018"] = {
	--desc = "Conflagration (Blazing Trickster)";
	sound = 3;
	applicationOnly = true;
};

-- TODO: Curtain of Flame (Azzakel)
-- TODO: Seed of Corruption (Teron'gor)
-- TODO: Demonic Leap (Teron'gor) (avoidable?)
-- TODO: Chaos Wave (Teron'gor)

-- ************************
-- * Bloodmaul Slag Mines *
-- ************************

GTFO.SpellID["150023"] = {
	--desc = "Slag Smash (Magmolatus)";
	sound = 3;
};

GTFO.SpellID["152843"] = {
	--desc = "Fiery Boulder (Roltall)";
	sound = 3;
};

GTFO.SpellID["164618"] = {
	--desc = "Exploding Flames (Bloodmaul Flamespeaker)";
	sound = 3;
};

-- ******************
-- * Grimrail Depot *
-- ******************

GTFO.SpellID["162513"] = {
	--desc = "VX18-B Target Eliminator (Railmaster Rocketspark)";
	sound = 3;
};

GTFO.SpellID["164188"] = {
	--desc = "Blackrock Bomb (Grimrail Bombardier)";
	sound = 3;
};

GTFO.SpellID["166404"] = {
	--desc = "Arcane Blitz (Grimrail Scout)";
	sound = 3;
};

-- **************
-- * Iron Docks *
-- **************

GTFO.SpellID["163276"] = {
	--desc = "Shredded Tendons (Neesa Nox)";
	sound = 3;
	applicationOnly = true;
};

GTFO.SpellID["161256"] = {
	--desc = "Primal Assault (Oshir)";
	sound = 3;
	tankSound = 0; -- Avoidable by tank?
};


-- *****************************
-- * Shadowmoon Burial Grounds *
-- *****************************

GTFO.SpellID["152690"] = {
	--desc = "Shadow Rune";
	sound = 3;
};

GTFO.SpellID["152688"] = {
	--desc = "Shadow Rune";
	sound = 3;
};

GTFO.SpellID["153232"] = {
	--desc = "Daggerfall (Sadana Bloodfury)";
	sound = 3;
};

GTFO.SpellID["164686"] = {
	--desc = "Dark Eclipse (Sadana Bloodfury)";
	sound = 3;
	--negatingDebuffSpellID = ???; -- White rune debuff
	test = true;
};

GTFO.SpellID["153395"] = {
	--desc = "Body Slam (Carrion Worm)";
	sound = 3;
};

GTFO.SpellID["153686"] = {
	--desc = "Body Slam (Bonemaw)";
	sound = 3;
};

GTFO.SpellID["154442"] = {
	--desc = "Malevolence (Ner'zhul)";
	sound = 3;
};

-- ************
-- * Skyreach *
-- ************

GTFO.SpellID["153563"] = {
	--desc = "Pierce";
	sound = 3;
};

-- *****************
-- * The Everbloom *
-- *****************

GTFO.SpellID["165093"] = {
	--desc = "Virulent Gasp (Verdant Mandragora)";
	sound = 3;
	tankSound = 0;
};

GTFO.SpellID["164294"] = {
	--desc = "Noxious Eruption (Twisted Abomination)";
	sound = 3;
};

GTFO.SpellID["175997"] = {
	--desc = "Noxious Eruption (Dulhu)";
	sound = 3;
	test = true; -- Avoidable?
};

GTFO.SpellID["169850"] = {
	--desc = "Frozen Snap (Infested Icecaller)";
	sound = 3;
};

GTFO.SpellID["166492"] = {
	--desc = "Firebloom (Archmage Sol)";
	sound = 3;
};

GTFO.SpellID["172643"] = {
	--desc = "Descend (Xeri'tac)";
	sound = 3;
};

GTFO.SpellID["169371"] = {
	--desc = "Swipe (Xeri'tac)";
	sound = 3;
	tankSound = 0;
};

GTFO.SpellID["169844"] = {
	--desc = "Dragon's Breath (Putrid Pyromancer)";
	sound = 3;
	tankSound = 0;
};

GTFO.SpellID["169179"] = {
	--desc = "Colossal Blow (Yalnu)";
	sound = 3;
	applicationOnly = true;
};


-- *************************
-- * Upper Blackrock Spire *
-- *************************

-- TODO: Lodestone Spike (Orebender Gor'ashan) -- Avoidable?  Couldn't see the graphic

GTFO.SpellID["155037"] = {
	--desc = "Eruption (Drakonid Monstrocity)";
	sound = 3;
	applicationOnly = true;
};

GTFO.SpellID["155033"] = {
	--desc = "Monstrous Swipe (Drakonid Monstrocity)";
	sound = 3;
	tankSound = 0;
};

GTFO.SpellID["155081"] = {
	--desc = "Fire Storm (Ragewing)";
	sound = 3;
};

GTFO.SpellID["155031"] = {
	--desc = "Engulfing Fire (Ragewing)";
	sound = 3;
	applicationOnly = true;	
};

-- TODO: Burning Breath (Emberscale Ironflight)

-- *********************
-- * Blackrock Foundry *
-- *********************

GTFO.SpellID["175752"] = {
	--desc = "Slag Breath (Ogron Hauler)";
	sound = 3;
	meleeOnly = true;
	test = true;
};

GTFO.SpellID["175765"] = {
	--desc = "Overhead Smash (Ogron Hauler)";
	sound = 3;
	tankSound = 0;
};


GTFO.SpellID["159520"] = {
	--desc = "Acidback Puddle (Darkshard Gnasher)";
	sound = 3;
};

-- Gruul
-- TODO: Inferno Slice - fail when hit while debuffed?
-- TODO: Overwhelming Blows - non-tank fail?
-- TODO: Flare (Heroic) - fail if avoidable
-- TODO: Shatter - fail if you get hit by allies

GTFO.SpellID["155301"] = {
	--desc = "Overhead Smash (Gruul)";
	sound = 3;
	test = true;
};

-- Oregorger
-- TODO: Unstable Slag Explosion - avoidable waves of death

GTFO.SpellID["156374"] = {
	--desc = "Explosive Shard - Stun (Oregorger)";
	sound = 3;
};

GTFO.SpellID["155900"] = {
	--desc = "Rolling Fury (Oregorger)";
	sound = 3;
};

GTFO.SpellID["155187"] = {
	--desc = "Bomb (Foreman Feldspar)";
	sound = 3;
	ignoreSelfInflicted = true;
};

-- Beastlord Darmac
-- TODO: Pin Down - spear impact
-- TODO: Cannonball Barrage - avoidable?
-- TODO: Heavy Smash - non-tank fail

GTFO.SpellID["163182"] = {
	--desc = "Crushing Slam (Iron Smith)";
	sound = 3;
	tankSound = 0;
};

-- Flamebender Ka'graz
-- TODO: Charring Breath - non-tank fail, tank fail if debuffed
-- TODO: Magma Monsoon - avoidable?
-- TODO: Devastating Slam - non-tank fail

GTFO.SpellID["158140"] = {
	--desc = "Pulverize (Hans'gar and Franzok)";
	sound = 1;
};

GTFO.SpellID["160050"] = {
	--desc = "Delayed Siege Bomb (Operator Thogar)";
	sound = 3;
};

GTFO.SpellID["156554"] = {
	--desc = "Moving Train (Operator Thogar)";
	sound = 3;
};

-- The Blast Furnace
-- TODO: Electrocution - when not primary target
-- TODO: Bomb - when not primary target
-- TODO: Drop Lit Bomb 
-- TODO: Slag Bomb - Avoidable?
-- TODO: Volatile Fire - Avoidable?

-- Kromog

GTFO.SpellID["156713"] = {
	--desc = "Thundering Blows (Kromog)";
	test = true; -- Not sure if this works, untested
	soundFunction = function() -- Warn only on the first hit
		if (GTFO_FindEvent("ThunderingFail")) then
			return 0;
		end
		if (GTFO_HasDebuff("player", 157059)) then -- Negate Grasping Earth debuff
			return 0;
		end
		GTFO_AddEvent("ThunderingFail", 15);
		return 3;
	end
};

GTFO.SpellID["161923"] = {
	--desc = "Rune of Crushing Earth (Kromog)";
	sound = 3;
	tankSound = 0; -- Could be off-tank's job to clear these?
};

-- TODO: Slam 156704 - fail if too close? 
-- TODO: Reverberations 157247 - avoidable?
-- TODO: Call of the Mountain

-- The Iron Maidens
-- TODO: Incendiary Device - close impact avoidable?
-- TODO: Blade Dash - fail if not first target
-- TODO: Swirling Vortex
-- TODO: Blood Ritual - avoidable impact spray? Non-Tank fail?
-- TODO: Volatile Bloodbolt - avoidable?
-- TODO: Grapeshot Blast - avoidable?

GTFO.SpellID["158601"] = {
	--desc = "Dominator Blast (Turret)";
	sound = 3;
	applicationOnly = true;
};

GTFO.SpellID["160733"] = {
	--desc = "Bomb Impact";
	sound = 3;
};

GTFO.SpellID["157884"] = {
	--desc = "Detonation Sequence";
	sound = 3;
};

GTFO.SpellID["158009"] = {
	--desc = "Bloodsoaked Heartseeker (Marak the Blooded)";
	sound = 3;
	damageMinimum = 100000; 
};

-- Blackhand
-- TODO: Demolition - distance fail?
-- TODO: Impaling Throw - non-tank fail
-- TODO: Slag Bomb - avoidable?
-- TODO: Battering Ram - non-tank fail
-- TODO: Explosive Round - avoidable?
-- TODO: Slag Eruption - ?

-- ************
-- * Highmaul *
-- ************

GTFO.SpellID["162271"] = {
	--desc = "Earth Breaker (Vul'gor)";
	sound = 3;
};

GTFO.SpellID["161634"] = {
	--desc = "Molten Bomb (Vul'gor)";
	sound = 3;
};

GTFO.SpellID["159412"] = {
	--desc = "Mauling Brew (Kargath Bladefist)";
	sound = 3;
};

GTFO.SpellID["160521"] = {
	--desc = "Vile Breath (Drunken Bileslinger)";
	sound = 3;
	--tankSound = 0; -- Avoidable by tanks?
};

-- Kargath Bladefist
-- TODO: Ravenous Bloodmaw (Kargath Bladefist, Heroic) -- Insta-death?

GTFO.SpellID["160952"] = {
	--desc = "Fire Bomb (Iron Bomber)";
	sound = 3;
};

-- The Butcher
-- TODO: Gushing Wounds (The Butcher) -- Definite Fail at 5 stacks (heroic), fail at 4, fail at 6+ on LFR?
-- TODO: Paleobomb (The Butcher, Heroic) -- Avoidable?

-- Tectus
-- TODO: Raving Assault (Tectus) 163318 -- Non-tank Avoidable?, fail if you're not the mark?2  


GTFO.SpellID["162968"] = {
	--desc = "Earthen Flechettes (Tectus)";
	sound = 3;
	tankSound = 0;
};

GTFO.SpellID["163209"] = {
	--desc = "Fracture (Tectus)";
	sound = 3;
};

GTFO.SpellID["171045"] = {
	--desc = "Earthen Pillar (Tectus)";
	sound = 3;
};


-- Brackenspore
-- TODO: Exploding Fungus (Brackenspore, Heroic) -- Avoidable?
-- TODO: Call of the Tides (Brackenspore, Heroic) -- Avoidable?

-- Twin Ogron
GTFO.SpellID["158026"] = {
	--desc = "Enfeebling Roar (Phemos)";
	sound = 3;
};

GTFO.SpellID["158159"] = {
	--desc = "Shield Charge (Pol)";
	sound = 3;
};

GTFO.SpellID["158336"] = {
	--desc = "Pulverize (Pol) - First hit";
	soundFunction = function() -- Warn only if you get hit more than once
		if (GTFO_FindEvent("PulverizeMultiHit")) then
			return 3;
		end
		GTFO_AddEvent("PulverizeMultiHit", 3);
		return 0;
	end
};

GTFO.SpellID["158417"] = {
	--desc = "Pulverize (Pol) - Second hit";
	sound = 3;
};

GTFO.SpellID["158420"] = {
	--desc = "Pulverize (Pol) - Third hit";
	sound = 3;
	damageMinimum = 50000; 
	test = true; -- Different damage amounts based on raid difficulty (50K for LFR?)
};


-- TODO: Pulverize (Pol) -- Based on damage amount & distance, 3 different types in sequence: #1 spam fail, #2 impact fail, #3 distance fail
-- TODO: Arcane Charge (Pol) -- Avoidable?
-- TODO: Arcane Volatility (Phemos, Heroic) -- Avoidable? FF damage?

-- Ko'ragh
GTFO.SpellID["172685"] = {
	--desc = "Expel Magic: Fire (Ko'ragh)";
	sound = 3;
};

GTFO.SpellID["162397"] = {
	--desc = "Expel Magic: Arcane (Ko'ragh)";
	sound = 3;
};

-- Imperator Mar'gok
-- TODO: Destructive Resonance (Imperator Mar'gok) -- impact explosion from the person that triggers the mine only, supposed to avoid?
-- TODO: Mark of Chaos (Imperator Mar'gok) -- FF damage fail
-- TODO: Nether Blast (Imperator Mar'gok) -- Fail if you're not the target?
-- TODO: Devastating Shockwave (Imperator Mar'gok) -- Non-tank fail
-- TODO: Mark of Chaos: Replication (Imperator Mar'gok) -- Fail if hit from blast? (Can get hit multiple times?)
-- TODO: Force Nova: Replication (Imperator Mar'gok) -- Fail if you're not the target of nova?



