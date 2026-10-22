-- Pawn by Vger-Azjol-Nerub
-- www.vgermods.com
-- © 2006-2013 Green Eclipse.  This mod is released under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 license.
-- See Readme.htm for more information.
--
-- Wowhead scales
------------------------------------------------------------

local ScaleProviderName = "Wowhead"

function PawnWowheadScaleProvider_AddScales()




------------------------------------------------------------
-- Warrior
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"WarriorArms",
	PawnLocal.Wowhead.WarriorArms,
	"c79c6e",
	{
		["Dps"] = 459, ["Strength"] = 100, ["HitRating"] = 78, ["ExpertiseRating"] = 67, ["CritRating"] = 56, ["Ap"] = 49, ["MasteryRating"] = 44, ["HasteRating"] = 33, ["Stamina"] = .1, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	1 -- hide 1H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"WarriorFury",
	PawnLocal.Wowhead.WarriorFury,
	"c79c6e",
	{
		["Dps"] = 136, ["ExpertiseRating"] = 100, ["HitRating"] = 100, ["CritRating"] = 73, ["Strength"] = 51, ["MasteryRating"] = 34, ["HasteRating"] = 24, ["Ap"] = 24, ["Stamina"] = .1, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"WarriorTank",
	PawnLocal.Wowhead.WarriorTank,
	"c79c6e",
	{
		["ParryRating"] = 100, ["DodgeRating"] = 100, ["Strength"] = 98, ["Stamina"] = 70, ["MasteryRating"] = 50, ["ExpertiseRating"] = 30, ["HitRating"] = 30, ["Armor"] = 25, ["HasteRating"] = 20, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	2 -- hide 2H upgrades
)

------------------------------------------------------------
-- Paladin
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"PaladinHoly",
	PawnLocal.Wowhead.PaladinHoly,
	"f58cba",
	{
		["Intellect"] = 100, ["Spirit"] = 75, ["SpellPower"] = 75, ["HasteRating"] = 55, ["CritRating"] = 50, ["MasteryRating"] = 50, ["Stamina"] = .1, ["IsDagger"] = -1000000, ["IsFist"] = -1000000, ["IsStaff"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	2 -- hide 2H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"PaladinTank",
	PawnLocal.Wowhead.PaladinTank,
	"f58cba",
	{
		["MasteryRating"] = 100, ["HitRating"] = 63, ["Armor"] = 62, ["ExpertiseRating"] = 41, ["HasteRating"] = 39, ["Strength"] = 38, ["DodgeRating"] = 34, ["ParryRating"] = 34, ["Stamina"] = 29, ["IsDagger"] = -1000000, ["IsFist"] = -1000000, ["IsStaff"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	2 -- hide 2H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"PaladinRetribution",
	PawnLocal.Wowhead.PaladinRetribution,
	"f58cba",
	{
		["Speed"] = 100, ["MeleeDps"] = 98, ["Strength"] = 58, ["ExpertiseRating"] = 41, ["HitRating"] = 41, ["HasteRating"] = 32, ["MasteryRating"] = 26, ["Ap"] = 24, ["CritRating"] = 24, ["Stamina"] = .1, ["IsDagger"] = -1000000, ["IsFist"] = -1000000, ["IsStaff"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	1 -- hide 1H upgrades
)

------------------------------------------------------------
-- Hunter
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"HunterBeastMastery",
	PawnLocal.Wowhead.HunterBeastMastery,
	"abd473",
	{
		["Agility"] = 100, ["RangedDps"] = 90, ["Speed"] = 74, ["HitRating"] = 64, ["ExpertiseRating"] = 64, ["Ap"] = 38, ["HasteRating"] = 37, ["CritRating"] = 31, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsShield"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsOffHand"] = -1000000, ["IsDagger"] = -1000000, ["IsPolearm"] = -1000000, ["IsStaff"] = -1000000, ["IsFist"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"HunterMarksman",
	PawnLocal.Wowhead.HunterMarksman,
	"abd473",
	{
		["Speed"] = 100, ["RangedDps"] = 83, ["Agility"] = 42, ["HitRating"] = 40, ["ExpertiseRating"] = 40, ["HasteRating"] = 18, ["CritRating"] = 16, ["Ap"] = 15, ["MasteryRating"] = 11, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsShield"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsOffHand"] = -1000000, ["IsDagger"] = -1000000, ["IsPolearm"] = -1000000, ["IsStaff"] = -1000000, ["IsFist"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"HunterSurvival",
	PawnLocal.Wowhead.HunterSurvival,
	"abd473",
	{
		["Agility"] = 100, ["RangedDps"] = 78, ["Speed"] = 75, ["ExpertiseRating"] = 67, ["HitRating"] = 67, ["Ap"] = 39, ["CritRating"] = 30, ["HasteRating"] = 28, ["MasteryRating"] = 17, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsShield"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsOffHand"] = -1000000, ["IsDagger"] = -1000000, ["IsPolearm"] = -1000000, ["IsStaff"] = -1000000, ["IsFist"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

------------------------------------------------------------
-- Rogue
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"RogueAssassination",
	PawnLocal.Wowhead.RogueAssassination,
	"fff569",
	{
		["MeleeDps"] = 137, ["Speed"] = 100, ["Agility"] = 80, ["ExpertiseRating"] = 78, ["HitRating"] = 78, ["HasteRating"] = 44, ["MasteryRating"] = 34, ["CritRating"] = 31, ["Strength"] = 29, ["Ap"] = 29, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsPolearm"] = -1000000, ["IsStaff"] = -1000000, ["Is2HAxe"] = -1000000, ["Is2HMace"] = -1000000, ["Is2HSword"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsAxe"] = -1000000, ["IsFist"] = -1000000, ["IsMace"] = -1000000, ["IsSword"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	2 -- hide 2H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"RogueCombat",
	PawnLocal.Wowhead.RogueCombat,
	"fff569",
	{
		["Agility"] = 100, ["MeleeDps"] = 92, ["HitRating"] = 66, ["ExpertiseRating"] = 45, ["MasteryRating"] = 39, ["Strength"] = 38, ["HasteRating"] = 38, ["CritRating"] = 37, ["Ap"] = 36, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsPolearm"] = -1000000, ["IsStaff"] = -1000000, ["Is2HAxe"] = -1000000, ["Is2HMace"] = -1000000, ["Is2HSword"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	2 -- hide 2H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"RogueSubtlety",
	PawnLocal.Wowhead.RogueSubtlety,
	"fff569",
	{
		["MeleeDps"] = 123, ["Agility"] = 100, ["HitRating"] = 46, ["ExpertiseRating"] = 39, ["HasteRating"] = 38, ["CritRating"] = 34, ["MasteryRating"] = 29, ["Strength"] = 28, ["Ap"] = 27, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsPolearm"] = -1000000, ["IsStaff"] = -1000000, ["Is2HAxe"] = -1000000, ["Is2HMace"] = -1000000, ["Is2HSword"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	2 -- hide 2H upgrades
)

------------------------------------------------------------
-- Priest
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"PriestDiscipline",
	PawnLocal.Wowhead.PriestDiscipline,
	"e0e0e0",
	{
		["Intellect"] = 100, ["SpellPower"] = 81, ["Spirit"] = 80, ["CritRating"] = 60, ["MasteryRating"] = 55, ["HasteRating"] = 50, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsPolearm"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["Is2HMace"] = -1000000, ["IsOffHand"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"PriestHoly",
	PawnLocal.Wowhead.PriestHoly,
	"e0e0e0",
	{
		["Intellect"] = 100, ["SpellPower"] = 85, ["Spirit"] = 81, ["HasteRating"] = 62, ["CritRating"] = 43, ["MasteryRating"] = 33, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsPolearm"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["Is2HMace"] = -1000000, ["IsOffHand"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"PriestShadow",
	PawnLocal.Wowhead.PriestShadow,
	"e0e0e0",
	{
		["Intellect"] = 100, ["SpellPower"] = 81, ["Spirit"] = 61, ["HitRating"] = 61, ["HasteRating"] = 60, ["CritRating"] = 44, ["MasteryRating"] = 38, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsPolearm"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["Is2HMace"] = -1000000, ["IsOffHand"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

------------------------------------------------------------
-- DK
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"DeathKnightBloodTank",
	PawnLocal.Wowhead.DeathKnightBloodTank,
	"ff4d6b",
	{
		["MasteryRating"] = 100, ["Strength"] = 92, ["ParryRating"] = 77, ["DodgeRating"] = 77, ["Stamina"] = 50, ["Armor"] = 31, ["HitRating"] = 15, ["ExpertiseRating"] = 15, ["IsShield"] = -1000000, ["IsDagger"] = -1000000, ["IsFist"] = -1000000, ["IsStaff"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	1 -- hide 1H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"DeathKnightFrostDps",
	PawnLocal.Wowhead.DeathKnightFrostDps,
	"ff4d6b",
	{
		["MeleeDps"] = 335, ["Strength"] = 100, ["ExpertiseRating"] = 90, ["HitRating"] = 81, ["HasteRating"] = 56, ["CritRating"] = 48, ["Ap"] = 42, ["MasteryRating"] = 36, ["Stamina"] = .1, ["IsShield"] = -1000000, ["IsDagger"] = -1000000, ["IsFist"] = -1000000, ["IsStaff"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"DeathKnightUnholyDps",
	PawnLocal.Wowhead.DeathKnightUnholyDps,
	"ff4d6b",
	{
		["MeleeDps"] = 137, ["Strength"] = 100, ["HitRating"] = 65, ["ExpertiseRating"] = 65, ["HasteRating"] = 49, ["CritRating"] = 44, ["Ap"] = 40, ["MasteryRating"] = 35, ["Stamina"] = .1, ["IsShield"] = -1000000, ["IsDagger"] = -1000000, ["IsFist"] = -1000000, ["IsStaff"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	1 -- hide 1H upgrades
)

------------------------------------------------------------
-- Shaman
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"ShamanElemental",
	PawnLocal.Wowhead.ShamanElemental,
	"6e95ff",
	{
		["HitRating"] = 100, ["Spirit"] = 100, ["Intellect"] = 83, ["SpellPower"] = 70, ["HasteRating"] = 37, ["MasteryRating"] = 33, ["CritRating"] = 31, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsPolearm"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"ShamanEnhancement",
	PawnLocal.Wowhead.ShamanEnhancement,
	"6e95ff",
	{
		["Agility"] = 100, ["HitRating"] = 99, ["ExpertiseRating"] = 99, ["MeleeDps"] = 91, ["MasteryRating"] = 45, ["Strength"] = 42, ["CritRating"] = 41, ["Ap"] = 40, ["HasteRating"] = 37, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsPolearm"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	2 -- hide 2H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"ShamanRestoration",
	PawnLocal.Wowhead.ShamanRestoration,
	"6e95ff",
	{
		["Intellect"] = 100, ["SpellPower"] = 75, ["Spirit"] = 65, ["HasteRating"] = 60, ["MasteryRating"] = 55, ["CritRating"] = 40, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsPolearm"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

------------------------------------------------------------
-- Mage
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"MageArcane",
	PawnLocal.Wowhead.MageArcane,
	"69ccf0",
	{
		["Intellect"] = 100, ["SpellPower"] = 83, ["HitRating"] = 72, ["MasteryRating"] = 56, ["HasteRating"] = 50, ["CritRating"] = 38, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsPolearm"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"MageFire",
	PawnLocal.Wowhead.MageFire,
	"69ccf0",
	{
		["Intellect"] = 100, ["HitRating"] = 79, ["SpellPower"] = 75, ["CritRating"] = 65, ["HasteRating"] = 55, ["MasteryRating"] = 46, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsPolearm"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"MageFrost",
	PawnLocal.Wowhead.MageFrost,
	"69ccf0",
	{
		["Intellect"] = 100, ["SpellPower"] = 81, ["HitRating"] = 68, ["HasteRating"] = 44, ["CritRating"] = 41, ["MasteryRating"] = 37, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsPolearm"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

------------------------------------------------------------
-- Warlock
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"WarlockAffliction",
	PawnLocal.Wowhead.WarlockAffliction,
	"bca5ff",
	{
		["Intellect"] = 100, ["SpellPower"] = 82, ["MasteryRating"] = 62, ["HasteRating"] = 57, ["HitRating"] = 47, ["CritRating"] = 39, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsPolearm"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"WarlockDemonology",
	PawnLocal.Wowhead.WarlockDemonology,
	"bca5ff",
	{
		["Intellect"] = 100, ["SpellPower"] = 83, ["HitRating"] = 73, ["MasteryRating"] = 45, ["HasteRating"] = 42, ["CritRating"] = 41, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsPolearm"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"WarlockDestruction",
	PawnLocal.Wowhead.WarlockDestruction,
	"bca5ff",
	{
		["Intellect"] = 100, ["SpellPower"] = 81, ["HitRating"] = 68, ["HasteRating"] = 46, ["CritRating"] = 43, ["MasteryRating"] = 41, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsLeather"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsFist"] = -1000000, ["IsMace"] = -1000000, ["Is2HMace"] = -1000000, ["IsPolearm"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

------------------------------------------------------------
-- Monk
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"MonkBrewmaster",
	PawnLocal.Wowhead.MonkBrewmaster,
	"00ff96",
	{
		["Agility"] = 100, ["HitRating"] = 65, ["ExpertiseRating"] = 65, ["HasteRating"] = 62, ["CritRating"] = 54, ["ParryRating"] = 50, ["Armor"] = 46, ["DodgeRating"] = 42, ["Ap"] = 35, ["MasteryRating"] = 31, ["Stamina"] = 27, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsDagger"] = -1000000, ["Is2HSword"] = -1000000, ["Is2HAxe"] = -1000000, ["Is2HMace"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"MonkMistweaver",
	PawnLocal.Wowhead.MonkMistweaver,
	"00ff96",
	{
		["Intellect"] = 100, ["SpellPower"] = 85, ["Spirit"] = 75, ["HasteRating"] = 60, ["CritRating"] = 50, ["MasteryRating"] = 40, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsDagger"] = -1000000, ["Is2HSword"] = -1000000, ["Is2HAxe"] = -1000000, ["Is2HMace"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"MonkWindwalker",
	PawnLocal.Wowhead.MonkWindwalker,
	"00ff96",
	{
		["HitRating"] = 100, ["Agility"] = 86, ["ExpertiseRating"] = 79, ["HasteRating"] = 46, ["CritRating"] = 37, ["Strength"] = 33, ["Ap"] = 31, ["MasteryRating"] = 21, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsDagger"] = -1000000, ["Is2HSword"] = -1000000, ["Is2HAxe"] = -1000000, ["Is2HMace"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsFrill"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

------------------------------------------------------------
-- Druid
------------------------------------------------------------

PawnAddPluginScale(
	ScaleProviderName,
	"DruidBalance",
	PawnLocal.Wowhead.DruidBalance,
	"ff7d0a",
	{
		["Intellect"] = 100, ["HitRating"] = 90, ["Spirit"] = 90, ["SpellPower"] = 83, ["CritRating"] = 52, ["HasteRating"] = 50, ["MasteryRating"] = 45, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

PawnAddPluginScale(
	ScaleProviderName,
	"DruidFeralDps",
	PawnLocal.Wowhead.DruidFeralDps,
	"ff7d0a",
	{
		["Agility"] = 100, ["MeleeDps"] = 96, ["Strength"] = 41, ["MasteryRating"] = 41, ["Ap"] = 39, ["HitRating"] = 32, ["ExpertiseRating"] = 32, ["CritRating"] = 31, ["HasteRating"] = 29, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	1 -- hide 1H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"DruidFeralTank",
	PawnLocal.Wowhead.DruidFeralTank,
	"ff7d0a",
	{
		["Agility"] = 100, ["DodgeRating"] = 82, ["MasteryRating"] = 44, ["CritRating"] = 41, ["HasteRating"] = 24, ["HitRating"] = 21, ["ExpertiseRating"] = 21, ["Armor"] = 18, ["Stamina"] = 18, ["Ap"] = 3, ["Strength"] = 3, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1, -- normalize values
	1 -- hide 1H upgrades
)

PawnAddPluginScale(
	ScaleProviderName,
	"DruidRestoration",
	PawnLocal.Wowhead.DruidRestoration,
	"ff7d0a",
	{
		["Intellect"] = 100, ["SpellPower"] = 85, ["HasteRating"] = 80, ["Spirit"] = 75, ["MasteryRating"] = 65, ["CritRating"] = 60, ["Stamina"] = .1, ["IsPlate"] = -1000000, ["IsMail"] = -1000000, ["IsShield"] = -1000000, ["IsAxe"] = -1000000, ["Is2HAxe"] = -1000000, ["IsSword"] = -1000000, ["Is2HSword"] = -1000000, ["IsBow"] = -1000000, ["IsCrossbow"] = -1000000, ["IsGun"] = -1000000, ["IsWand"] = -1000000, ["IsOffHand"] = -1000000, ["MetaSocketEffect"] = 16000
	},
	1 -- normalize values
)

------------------------------------------------------------

-- PawnWowheadScaleProviderOptions.LastAdded keeps track of the last time that we tried to automatically enable scales for this character.
if not PawnWowheadScaleProviderOptions then PawnWowheadScaleProviderOptions = { } end
if not PawnWowheadScaleProviderOptions.LastAdded then PawnWowheadScaleProviderOptions.LastAdded = 0 end

local _, Class = UnitClass("player")
if PawnWowheadScaleProviderOptions.LastClass ~= nil and Class ~= PawnWowheadScaleProviderOptions.LastClass then
	-- If the character has changed class since last time, let's start over.
	PawnSetAllScaleProviderScalesVisible(ScaleProviderName, false)
	PawnWowheadScaleProviderOptions.LastAdded = 0
end
PawnWowheadScaleProviderOptions.LastClass = Class

if PawnWowheadScaleProviderOptions.LastAdded < 1 then
	if Class == "WARRIOR" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "WarriorFury"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "WarriorTank"), true)
	elseif Class == "PALADIN" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "PaladinHoly"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "PaladinTank"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "PaladinRetribution"), true)
	elseif Class == "HUNTER" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "HunterBeastMastery"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "HunterMarksman"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "HunterSurvival"), true)
	elseif Class == "ROGUE" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "RogueAssassination"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "RogueCombat"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "RogueSubtlety"), true)
	elseif Class == "PRIEST" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "PriestDiscipline"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "PriestHoly"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "PriestShadow"), true)
	elseif Class == "DEATHKNIGHT" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "DeathKnightBloodTank"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "DeathKnightFrostDps"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "DeathKnightUnholyDps"), true)
	elseif Class == "SHAMAN" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "ShamanElemental"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "ShamanEnhancement"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "ShamanRestoration"), true)
	elseif Class == "MAGE" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "MageArcane"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "MageFire"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "MageFrost"), true)
	elseif Class == "WARLOCK" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "WarlockAffliction"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "WarlockDemonology"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "WarlockDestruction"), true)
	elseif Class == "DRUID" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "DruidBalance"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "DruidFeralDps"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "DruidFeralTank"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "DruidRestoration"), true)
	end
end

if PawnWowheadScaleProviderOptions.LastAdded < 2 then
	if Class == "WARRIOR" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "WarriorArms"), true)
	end
end

if PawnWowheadScaleProviderOptions.LastAdded < 3 then
	if Class == "MONK" then
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "MonkBrewmaster"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "MonkMistweaver"), true)
		PawnSetScaleVisible(PawnGetProviderScaleName(ScaleProviderName, "MonkWindwalker"), true)
	end
end

-- Don't reenable those scales again after the user has disabled them previously.
PawnWowheadScaleProviderOptions.LastAdded = 3

-- After this function terminates there's no need for it anymore, so cause it to self-destruct to save memory.
PawnWowheadScaleProvider_AddScales = nil

end -- PawnWowheadScaleProvider_AddScales

------------------------------------------------------------

PawnAddPluginScaleProvider(ScaleProviderName, PawnLocal.Wowhead.Provider, PawnWowheadScaleProvider_AddScales)
