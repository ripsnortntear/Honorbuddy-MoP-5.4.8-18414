do
	local _detalhes = _G._detalhes

	_detalhes.PotionList = {}

	--import potion list from the framework
	for spellID, _ in pairs(DetailsFramework.PotionIDs) do
		_detalhes.PotionList[spellID] = true
	end

	_detalhes.SpecSpellList = { --~spec
		-- Unholy Death Knight:
		[49206] = 252, -- Summon Gargoyle
		[55090] = 252, -- Scourge Strike (Physical)
		[70890] = 252, -- Scourge Strike (Shadow)
		[85948] = 252, -- Festering Strike
		[49016] = 252, -- Unholy Frenzy
		[63560] = 252, -- Dark Transformation

		-- Frost Death Knight:
		[49143] = 251, -- Frost Strike
		[66196] = 251, -- Frost Strike Off-Hand
		[49184] = 251, -- Howling Blast
		[49020] = 251, -- Obliterate
		[51271] = 251, -- Pillar of Frost

		-- Blood Death Knight:
		[55233] = 250, -- Vampiric Blood
		[55050] = 250, -- Heart Strike
		[48982] = 250, -- Rune Tap
		[56815] = 250, -- Rune Strike
		[49028] = 250, -- Dancing Rune Weapon
		[49222] = 250, -- Bone Shield

		-- Balance Druid:
		[50288] = 102,  -- Starfall
		[93399] = 102,  -- Shooting Stars
		[78675] = 102,  -- Solar Beam
		[78674] = 102,  -- Starsurge
		[2912] = 102,   -- Starfire
		[24858] = 102,  -- Moonkin Form
		[93402] = 102,  -- Sunfire
		[112071] = 102, -- Celestial Alignment
		[88747] = 102,  -- Wild Mushroom

		-- Feral Druid:
		[52610] = 103,  -- Savage Roar
		[5217] = 103,   -- Tiger's Fury
		[5221] = 103,   -- Shred
		[135700] = 103, -- Omen of Clarity
		[69369] = 103,  -- Predatory Swiftness

		-- Guardian Druid:
		[62606] = 104,  -- Savage Defense
		[5229] = 104,   -- Enrage
		[102795] = 104, -- Bear Hug
		[135286] = 104, -- Tooth and Claw

		-- Restoration Druid:
		[100977] = 105, -- Harmony
		[48438] = 105,  -- Wild Growth
		[18562] = 105,  -- Swiftmend
		[50464] = 105,  -- Nourish
		[8936] = 105,   -- Regrowth
		[33763] = 105,  -- Lifebloom
		[102342] = 105, -- Ironbark
		[102791] = 105, -- Wild Mushroom: Bloom
		[145518] = 105, -- Genesis

		-- Beast Mastery Hunter:
		[19574] = 253, -- Bestial Wrath
		[53257] = 253, -- Cobra Strikes
		[34026] = 253, -- Kill Command
		[82692] = 253, -- Focus Fire

		-- Marksmanship Hunter:
		[53209] = 254, -- Chimera Shot
		[19434] = 254, -- Aimed Shot
		[82921] = 254, -- Bombardment
		[34487] = 254, -- Master Marksman
		[53224] = 254, -- Steady Focus
		[53232] = 254, -- Rapid Recuperation
		[76659] = 254, -- Mastery: Wild Quiver

		-- Survival Hunter:
		[3674] = 255,  -- Black Arrow
		[53301] = 255, -- Explosive Shot
		[56453] = 255, -- Lock and Load
		[82834] = 255, -- Improved Serpent Sting

		-- Arcane Mage:
		[12042] = 62, -- Arcane Power
		[31589] = 62, -- Slow
		[44425] = 62, -- Arcane Barrage
		[82930] = 62, -- Arcane Tactics

		-- Fire Mage:
		[31661] = 63,  -- Dragon's Breath
		[11366] = 63,  -- Pyroblast
		[133] = 63,    -- Fireball
		[108853] = 63, -- Inferno Blast
		[2948] = 63,   -- Scorch
		[11129] = 63,  -- Combustion

		-- Frost Mage:
		[44544] = 64,  -- Fingers of Frost
		[44572] = 64,  -- Deep Freeze
		[116] = 64,	   -- Frostbolt
		[31687] = 64,  -- Summon Water Elemental
		[12472] = 64,  -- Icy Veins
		[131078] = 64, -- Icy Veins Glyphed
		[84714] = 64,  -- Frozen Orb

		-- Holy Paladin:
		[31842] = 65, -- Divine Favor
		[25914] = 65, -- Holy Shock
		[54149] = 65, -- Infusion of Light
		[20053] = 65, -- Conviction
		[86273] = 65, -- Mastery: Illuminated Healing
		[85222] = 65, -- Light of Dawn

		-- Protection Paladin:
		[53600] = 66, -- Shield of the Righteous
		[31935] = 66, -- Avenger's Shield
		[20925] = 66, -- Holy Shield
		[31850] = 66, -- Ardent Defender

		-- Retribution Paladin:
		[53385] = 70,  -- Divine Storm
		[85256] = 70,  -- Templar's Verdict
		[84963] = 70,  -- Inquisition
		[879] = 70,    -- Exorcism
		[121783] = 70, -- Emancipate
		
		-- Discipline Priest:
		[89485] = 256,  -- Inner Focus
		[33206] = 256,  -- Pain Suppression
		[47750] = 256,  -- Penance
		[47753] = 256,  -- Divine Aegis
		[59888] = 256,  -- Borrowed Time
		[47755] = 256,  -- Rapture
		[114908] = 256, -- Spirit Shell

		-- Holy Priest:
		[34861] = 257,  -- Circle of Healing
		[47788] = 257,  -- Guardian Spirit
		[81206] = 257,  -- Chakra: Sanctuary
		[81208] = 257,  -- Chakra: Serenity
		[81200] = 257,  -- Chakra: Chastise
		[126135] = 257, -- Lightwell
		[95649] = 257,  -- Rapid Renewal
		[64843] = 257,  -- Divine Hymn
		[88625] = 257,  -- Holy Word: Chastise
		[88684] = 257,  -- Holy Word: Serenity
		[88685] = 257,  -- Holy Word: Sanctuary

		-- Shadow Priest:
		[15286] = 258, -- Vampiric Embrace
		[34914] = 258, -- Vampiric Touch
		[15407] = 258, -- Mind Flay
		[78203] = 258, -- Shadowy Apparitions
		[2944] = 258,  -- Devouring Plague
		[8092] = 258,  -- Mind Blast
		[15473] = 258, -- Shadowform
		[73510] = 258, -- Mind Spike
		[15487] = 258, -- Silence
		[47585] = 258, -- Dispersion
		[64044] = 258, -- Psychic Horror

		-- Assassination Rogue:
		[32645] = 259,  -- Envenom
		[1329] = 259,   -- Mutilate
		[5374] = 259,   -- Mutilate
		[27576] = 259,  -- Mutilate Off-Hand
		[111240] = 259, -- Dispatch
		[79140] = 259,  -- Vendetta

		-- Combat Rogue:
		[51690] = 260, -- Killing Spree
		[13750] = 260, -- Adrenaline Rush
		[13877] = 260, -- Blade Flurry
		[84617] = 260, -- Revealing Strike

		-- Subtlety Rogue:
		[16511] = 261, -- Hemorrhage
		[14183] = 261, -- Premeditation
		[51713] = 261, -- Shadow Dance
		[53] = 261,    -- Backstab
		[91023] = 261, -- Find Weakness

		-- Elemental Shaman:
		[51490] = 262,  -- Thunderstorm
		[88767] = 262,  -- Fulmination
		[77756] = 262,  -- Lava Surge
		[61882] = 262,  -- Earthquake
		[45284] = 262,  -- Lightning Bolt Overload
		[77451] = 262,  -- Lava Burst Overload
		[120588] = 262, -- Elemental Blast Overload
		[16246] = 262,  -- Clearcasting

		-- Enhancement Shaman:
		[51533] = 263, -- Feral Spirit
		[51530] = 263, -- Maelstrom Weapon
		[60103] = 263, -- Lava Lash
		[17364] = 263, -- Stormstrike
		[1535] = 263,  -- Fire Nova
		[58875] = 263, -- Spirit Walk

		-- Restoration Shaman:
		[53390] = 264, -- Tidal Waves
		[974]   = 264, -- Earth Shield
		[61295] = 264, -- Riptide
		[331] = 264,   -- Healing Wave
		[77472] = 264, -- Greater Healing Wave
		[16190] = 264, -- Mana Tide Totem
		[51730] = 264, -- Earthliving Weapon Imbue
		[51945] = 264, -- Earthliving Weapon HoT
		[98008] = 264, -- Spirit Link Totem

		-- Affliction :
		[48181] = 265,  -- Haunt
		[30108] = 265,  -- Unstable Affliction
		[1120] = 265,   -- Drain Soul
		[74434] = 265,  -- Soulburn
		[980] = 265,    -- Agony
		[103103] = 265, -- Malefic Grasp
		[108558] = 265, -- Nightfall
		[27243] = 265,  -- Seed of Corruption
		[86121] = 265,  -- Soul Swap
		[113860] = 265, -- Dark Soul: Misery

		-- Demonology Warlock:
		[30146] = 266,  -- Summon Felguard
		[103958] = 266, -- Metamorphosis
		[105174] = 266, -- Hand of Gul'dan
		[6353] = 266,   -- Soul Fire
		[1949] = 266,   -- Hellfire
		[113861] = 266, -- Dark Soul: Knowledge

		-- Destruction Warlock:
		[116858] = 267, -- Chaos Bolt
		[17962] = 267,  -- Conflagrate
		[80240] = 267,  -- Havoc
		[29722] = 267,  -- Incinerate
		[348] = 267,    -- Immolate
		[114635] = 267, -- Ember Tap
		[17877] = 267,  -- Shadowburn
		[108683] = 267, -- Fire and Brimstone
		[113858] = 267, -- Dark Soul: Instability

		-- Arms Warrior:
		[12328] = 71,  -- Sweeping Strikes
		[60503] = 71,  -- Taste for Blood
		[12294] = 71,  -- Mortal Strike
		[76858] = 71,  -- Opportunity Strike (mastery)
		[1464] = 71,   -- Slam
		[118038] = 71, -- Die by the Sword

		-- Fury Warrior:
		[23881] = 72,  -- Bloodthirst
		[100130] = 72, -- Wild Strike
		[85288] = 72,  -- Raging Blow
		[85739] = 72,  -- Meat Cleaver

		-- Protection Warrior:
		[12975] = 73,  -- Last Stand
		[6572] = 73,   -- Revenge
		[20243] = 73,  -- Devastate
		[2565] = 73,   -- Shield Block
		[23922] = 73,  -- Shield Slam
		[46953] = 73,  -- Sword and Board
		[50227] = 73,  -- Sword and Board Trigger
		[1160] = 73,   -- Demoralizing Shout
		[112048] = 73, -- Shield Barrier

		-- Windwalker Monk:
		[113656] = 269, -- Fists of Fury
		[115288] = 269, -- Energizing Brew
		[107428] = 269, -- Rising Sun Kick
		[101545] = 269, -- Flying Serpent Kick
		[116740] = 269, -- Tigereye Brew
		[122470] = 269, -- Touch of Karma
		[115073] = 269, -- Spinning Fire Blossom
		[137639] = 269, -- Storm, Earth, and Fire

		-- Brewmaster Monk:
		[115181] = 268, -- Breath of Fire
		[115308] = 268, -- Elusive Brew
		[115295] = 268, -- Guard
		[118604] = 268, -- Guard 2?
		[121253] = 268, -- Keg Smash
		[115180] = 268, -- Dizzying Haze
		[119582] = 268, -- Purifying Brew
		[115315] = 268, -- Black Ox Statue
		[122057] = 268, -- Clash
		[115213] = 268, -- Avert Harm
		[118636] = 268, -- Power Guard
		[115307] = 268, -- Shuffle

		-- Mistweaver Monk:
		[124682] = 270, -- Enveloping Mist
		[116849] = 270, -- Life Cocoon
		[115151] = 270, -- Renewing Mist
		[115294] = 270, -- Mana Tea
		[115175] = 270, -- Soothing Mist
		[116670] = 270, -- Uplift
		[116694] = 270, -- Surging Mist
		[127722] = 270, -- Serpent's Zeal
		[115313] = 270, -- Jade Serpent Statue
		[116680] = 270, -- Thunder Focus Tea
		[115310] = 270, -- Revival
		[139598] = 270, -- Muscle Memory
	}

	_detalhes.ClassSpellList = {
		[47568] = "DEATHKNIGHT", -- Empower Rune Weapon
		[48982] = "DEATHKNIGHT", -- Rune Tap
		[55050] = "DEATHKNIGHT", -- Heart Strike
		[48707] = "DEATHKNIGHT", -- Anti-Magic Shell
		[73975] = "DEATHKNIGHT", -- Necrotic Strike
		[49576] = "DEATHKNIGHT", -- Death Grip
		[45524] = "DEATHKNIGHT", -- Chains of Ice
		[43265] = "DEATHKNIGHT", -- Death and Decay
		[55095] = "DEATHKNIGHT", -- Frost Fever
		[49222] = "DEATHKNIGHT", -- Bone Shield
		[48721] = "DEATHKNIGHT", -- Blood Boil
		[48263] = "DEATHKNIGHT", -- Blood Presence
		[49184] = "DEATHKNIGHT", -- Howling Blast
		[52212] = "DEATHKNIGHT", -- Death and Decay
		[66196] = "DEATHKNIGHT", -- Frost Strike Off-Hand
		[48266] = "DEATHKNIGHT", -- Frost Presence
		[85948] = "DEATHKNIGHT", -- Festering Strike
		[55078] = "DEATHKNIGHT", -- Blood Plague
		[49143] = "DEATHKNIGHT", -- Frost Strike
		[45529] = "DEATHKNIGHT", -- Blood Tap
		[48743] = "DEATHKNIGHT", -- Death Pact
		[45470] = "DEATHKNIGHT", -- Death Strike
		[61999] = "DEATHKNIGHT", -- Raise Ally
		[48265] = "DEATHKNIGHT", -- Unholy Presence
		[45902] = "DEATHKNIGHT", -- Blood Strike
		[57330] = "DEATHKNIGHT", -- Horn of Winter
		[48792] = "DEATHKNIGHT", -- Icebound Fortitude
		[49206] = "DEATHKNIGHT", -- Summon Gargoyle
		[49016] = "DEATHKNIGHT", -- Unholy Frenzy
		[51271] = "DEATHKNIGHT", -- Pillar of Frost
		[47541] = "DEATHKNIGHT", -- Death Coil
		[55090] = "DEATHKNIGHT", -- Scourge Strike
		[96268] = "DEATHKNIGHT", -- Death's Advance
		[53365] = "DEATHKNIGHT", -- Unholy Strength
		[50401] = "DEATHKNIGHT", -- Razor Frost
		[47632] = "DEATHKNIGHT", -- Death Coil
		[55233] = "DEATHKNIGHT", -- Vampiric Blood
		[42650] = "DEATHKNIGHT", -- Army of the Dead
		[77606] = "DEATHKNIGHT", -- Dark Simulacrum
		[3714]  = "DEATHKNIGHT", -- Path of Frost
		[50842] = "DEATHKNIGHT", -- Pestilence
		[50977] = "DEATHKNIGHT", -- Death Gate
		[49039] = "DEATHKNIGHT", -- Lichborne
		[63560] = "DEATHKNIGHT", -- Dark Transformation
		[45477] = "DEATHKNIGHT", -- Icy Touch
		[77575] = "DEATHKNIGHT", -- Outbreak
		[66198] = "DEATHKNIGHT", -- Obliterate Off-Hand
		[49020] = "DEATHKNIGHT", -- Obliterate
		[49998] = "DEATHKNIGHT", -- Death Strike
		[77535] = "DEATHKNIGHT", -- Blood Shield
		[49028] = "DEATHKNIGHT", -- Dancing Rune Weapon
		[56222] = "DEATHKNIGHT", -- Dark Command
		[66216] = "DEATHKNIGHT", -- Plague Strike Off-Hand
		[47528] = "DEATHKNIGHT", -- Mind Freeze
		[45462] = "DEATHKNIGHT", -- Plague Strike
		[56815] = "DEATHKNIGHT", -- Rune Strike

		[24858] = "DRUID", -- Moonkin Form
		[16914] = "DRUID", -- Hurricane
		[33763] = "DRUID", -- Lifebloom
		[8936]  = "DRUID", -- Regrowth
		[17057] = "DRUID", -- Furor
		[48438] = "DRUID", -- Wild Growth
		[48503] = "DRUID", -- Living Seed
		[1126]  = "DRUID", -- Mark of the Wild
		[33891] = "DRUID", -- Tree of Life
		[52610] = "DRUID", -- Savage Roar
		[20484] = "DRUID", -- Rebirth
		[62078] = "DRUID", -- Swipe
		[22842] = "DRUID", -- Frenzied Regeneration
		[18562] = "DRUID", -- Swiftmend
		[33876] = "DRUID", -- Mangle
		[22570] = "DRUID", -- Maim
		[2912]  = "DRUID", -- Starfire
		[48505] = "DRUID", -- Starfall
		[16979] = "DRUID", -- Feral Charge
		[29166] = "DRUID", -- Innervate
		[16959] = "DRUID", -- Primal Fury
		[6807]  = "DRUID", -- Maul
		[5211]  = "DRUID", -- Bash
		[61391] = "DRUID", -- Typhoon
		[2908]  = "DRUID", -- Soothe
		[42231] = "DRUID", -- Hurricane
		[5185]  = "DRUID", -- Healing Touch
		[33831] = "DRUID", -- Force of Nature
		[33878] = "DRUID", -- Mangle
		[22568] = "DRUID", -- Ferocious Bite
		[9005]  = "DRUID", -- Pounce
		[5215]  = "DRUID", -- Prowl
		[5221]  = "DRUID", -- Shred
		[8921]  = "DRUID", -- Moonfire
		[5217]  = "DRUID", -- Tiger's Fury
		[5229]  = "DRUID", -- Enrage
		[5225]  = "DRUID", -- Track Humanoids
		[16953] = "DRUID", -- Primal Fury
		[62606] = "DRUID", -- Savage Defense
		[61336] = "DRUID", -- Survival Instincts
		[44203] = "DRUID", -- Tranquility
		[770]   = "DRUID", -- Faerie Fire
		[50464] = "DRUID", -- Nourish
		[1079]  = "DRUID", -- Rip
		[50334] = "DRUID", -- Berserk
		[68285] = "DRUID", -- Leader of the Pack
		[50288] = "DRUID", -- Starfall
		[774]   = "DRUID", -- Rejuvenation
		[779]   = "DRUID", -- Swipe
		[33917] = "DRUID", -- Mangle
		[6795]  = "DRUID", -- Growl
		[6785]  = "DRUID", -- Ravage
		[49376] = "DRUID", -- Feral Charge
		[1822]  = "DRUID", -- Rake
		[5176]  = "DRUID", -- Wrath
		[1850]  = "DRUID", -- Dash
		[33778] = "DRUID", -- Lifebloom
		[22812] = "DRUID", -- Barkskin
		[768]   = "DRUID", -- Cat Form
		[16689] = "DRUID", -- Nature's Grasp
		[33745] = "DRUID", -- Lacerate

		[781]   = "HUNTER", -- Disengage
		[53353] = "HUNTER", -- Chimera Shot
		[53271] = "HUNTER", -- Master's Call
		[19574] = "HUNTER", -- Bestial Wrath
		[34026] = "HUNTER", -- Kill Command
		[77767] = "HUNTER", -- Cobra Shot
		[53209] = "HUNTER", -- Chimera Shot
		[53301] = "HUNTER", -- Explosive Shot
		[56641] = "HUNTER", -- Steady Shot
		[19386] = "HUNTER", -- Wyvern Sting
		[53351] = "HUNTER", -- Kill Shot
		[13165] = "HUNTER", -- Aspect of the Hawk
		[34477] = "HUNTER", -- Misdirection
		[19263] = "HUNTER", -- Deterrence
		[19503] = "HUNTER", -- Scatter Shot
		[20736] = "HUNTER", -- Distracting Shot
		[5118]  = "HUNTER", -- Aspect of the Cheetah
		[883]   = "HUNTER", -- Call Pet 1
		[19577] = "HUNTER", -- Intimidation
		[13159] = "HUNTER", -- Aspect of the Pack
		[3044]  = "HUNTER", -- Arcane Shot
		[13812] = "HUNTER", -- Explosive Trap
		[51753] = "HUNTER", -- Camouflage
		[1543]  = "HUNTER", -- Flare
		[13813] = "HUNTER", -- Explosive Trap
		[1978]  = "HUNTER", -- Serpent Sting
		[19434] = "HUNTER", -- Aimed Shot
		[3674]  = "HUNTER", -- Black Arrow
		[5116]  = "HUNTER", -- Concussive Shot
		[60192] = "HUNTER", -- Freezing Trap
		[34490] = "HUNTER", -- Silencing Shot
		[1130]  = "HUNTER", -- Hunter's Mark
		[2643]  = "HUNTER", -- Multi-Shot
		[19801] = "HUNTER", -- Tranquilizing Shot
		[2641]  = "HUNTER", -- Dismiss Pet
		[3045]  = "HUNTER", -- Rapid Fire

		[12043] = "MAGE", -- Presence of Mind
		[30449] = "MAGE", -- Spellsteal
		[31687] = "MAGE", -- Summon Water Elemental
		[61316] = "MAGE", -- Dalaran Brilliance
		[1953]  = "MAGE", -- Blink
		[55342] = "MAGE", -- Mirror Image
		[42956] = "MAGE", -- Conjure Refreshment
		[12051] = "MAGE", -- Evocation
		[11129] = "MAGE", -- Combustion
		[45438] = "MAGE", -- Ice Block
		[116]   = "MAGE", -- Frostbolt
		[11958] = "MAGE", -- Cold Snap
		[12042] = "MAGE", -- Arcane Power
		[12472] = "MAGE", -- Icy Veins
		[2139]  = "MAGE", -- Counterspell
		[44572] = "MAGE", -- Deep Freeze

		--monk
		[116995]	=	"MONK", -- "Surging Mist"
		[162530]	=	"MONK", -- "Rushing Jade Wind"
		[157675]	=	"MONK", -- "Chi Explosion"
		[157590]	=	"MONK", -- "Breath of the Serpent"
		[128591]	=	"MONK", -- "Blackout Kick"
		[122281]	=	"MONK", -- "Healing Elixirs"
		[124101]	=	"MONK", -- "Zen Sphere: Detonate"
		[119031]	=	"MONK", -- "Gift of the Serpent"
		[137562]	=	"MONK", -- "Nimble Brew"
		[157535]	=	"MONK", -- "Breath of the Serpent"
		[152173]	=	"MONK", -- "Serenity"
		[152175]	=	"MONK", -- "Hurricane Strike"
		[148187]	=	"MONK", -- "Rushing Jade Wind"
		[124098]	=	"MONK", -- "Zen Sphere"
		[125033]	=	"MONK", -- "Zen Sphere: Detonate"
		[158221]	=	"MONK", -- "Hurricane Strike"
		[115129]	=	"MONK", -- "Expel Harm"
		[152174]	=	"MONK", -- "Chi Explosion"
		[123586]	=	"MONK", -- "Flying Serpent Kick"
		[115176]	=	"MONK", -- Zen Meditation cooldown
		[115203]	=	"MONK", -- Fortifying Brew
		
		[124081]	=	"MONK", -- Zen Sphere
		[125355]	=	"MONK", -- Healing Sphere
		[122278]	=	"MONK", -- Dampen Harm
		[115450]	=	"MONK", -- Detox
		
		[121827]	=	"MONK", -- Roll
		[115315]	=	"MONK", -- Summon Black Ox Statue
		[115399]	=	"MONK", -- Chi Brew
		[101643]	=	"MONK", -- Transcendence
		[115546]	=	"MONK", -- Provoke
		[115294]	=	"MONK", -- Mana Tea
		[116680]	=	"MONK", -- Thunder Focus Tea
		[115070]	=	"MONK", -- Stance of the Wise Serpent
		[115069]	=	"MONK", -- Stance of the Sturdy Ox
		
		[119381]	=	"MONK", -- Leg Sweep
		[115695]	=	"MONK", -- Jab
		[137639]	=	"MONK", -- Storm, Earth, and Fire
		[115008]	=	"MONK", -- Chi Torpedo
		[121828]	=	"MONK", -- --Chi Torpedo 
		[115180]	=	"MONK", -- Dizzying Haze
		[123986]	=	"MONK", -- Chi Burst
		[130654]	=	"MONK", -- Chi Burst
		[148135]	=	"MONK", -- Chi Burst
		[119392]	=	"MONK", -- Charging Ox Wave
		[116095]	=	"MONK", -- Disable
		[115687]	=	"MONK", -- Jab		
		[117993]	=	"MONK", -- Chi Torpedo
		[100780]	=	"MONK", -- Jab
		[116740]	=	"MONK", -- Tigereye Brew
		[124682]	=	"MONK", -- Enveloping Mist
		[101545]	=	"MONK", -- Flying Serpent Kick
		[109132]	=	"MONK", -- Roll
		[122470]	=	"MONK", -- Touch of Karma
		[117418]	=	"MONK", -- Fists of Fury
		[113656]	=	"MONK", -- Fists of Fury
		[115698]	=	"MONK", -- Jab
		[115460]	=	"MONK", -- Healing Sphere
		[115098]	=	"MONK", -- Chi Wave
		[115151]	=	"MONK", -- Renewing Mist
		[117952]	=	"MONK", -- Crackling Jade Lightning
		[122783]	=	"MONK", -- Diffuse Magic
		[115078]	=	"MONK", -- Paralysis
		[116705]	=	"MONK", -- Spear Hand Strike
		[123904]	=	"MONK", -- Invoke Xuen, the White Tiger
		[147489]	=	"MONK", -- Expel Harm
		[101546]	=	"MONK", -- Spinning Crane Kick
		[115313]	=	"MONK", -- Summon Jade Serpent Statue
		[135920]	=	"MONK", -- Gift of the Serpent
		[116841]	=	"MONK", -- Tiger's Lust
		[116694]	=	"MONK", -- Surging Mist
		[116847]	=	"MONK", -- Rushing Jade Wind
		[108557]	=	"MONK", -- Jab
		[115181]	=	"MONK", -- Breath of Fire
		[121253]	=	"MONK", -- Keg Smash
		[124506]	=	"MONK", -- Gift of the Ox
		[124503]	=	"MONK", -- Gift of the Ox
		[115288]	=	"MONK", -- Energizing Brew
		[115308]	=	"MONK", -- Elusive Brew
		[116781]	=	"MONK", -- Legacy of the White Tiger
		[115921]	=	"MONK", -- Legacy of the Emperor
		[115693]	=	"MONK", -- Jab
		[124507]	=	"MONK", -- Gift of the Ox
		[119582]	=	"MONK", -- Purifying Brew
		[115080]	=	"MONK", -- Touch of Death
		[126892]	=	"MONK", -- Zen Pilgrimage
		[116849]	=	"MONK", -- Life Cocoon
		[116844]	=	"MONK", -- Ring of Peace
		[107428]	=	"MONK", --rising sun kick
		[100784]	=	"MONK", --blackout kick
		[132467]	=	"MONK", --Chi wave	
		[107270]	=	"MONK", --spinning crane kick
		[100787]	=	"MONK", --tiger palm
		[123761]	=	"MONK", --mana tea
		[119611]	=	"MONK", --renewing mist
		[116670]	=	"MONK", -- Uplift
		[115310]	=	"MONK", --revival
		[115175]	=	"MONK", --soothing mist
		[124041]	=	"MONK", --gift of the serpent
		[124040]	=	"MONK", -- shi torpedo
		[132120]	=	"MONK", -- enveloping mist
		[132463]	=	"MONK", -- shi wave
		[117895]	=	"MONK", --eminence (statue)
		[115295]	=	"MONK", --guard
		[115072]	=	"MONK", --expel harm

		[31821] = "PALADIN", -- Aura Mastery
		[53652] = "PALADIN", -- Beacon of Light
		[642]   = "PALADIN", -- Divine Shield
		[31803] = "PALADIN", -- Censure
		[62124] = "PALADIN", -- Hand of Reckoning
		[20164] = "PALADIN", -- Seal of Justice
		[53563] = "PALADIN", -- Beacon of Light
		[53595] = "PALADIN", -- Hammer of the Righteous
		[633]   = "PALADIN", -- Lay on Hands
		[31884] = "PALADIN", -- Avenging Wrath
		[66235] = "PALADIN", -- Ardent Defender
		[31801] = "PALADIN", -- Seal of Truth
		[17233] = "PALADIN", -- Lay on Hands
		[54428] = "PALADIN", -- Divine Plea
		[35395] = "PALADIN", -- Crusader Strike
		[1044]  = "PALADIN", -- Hand of Freedom
		[54172] = "PALADIN", -- Divine Storm
		[31842] = "PALADIN", -- Divine Favor
		[4987]  = "PALADIN", -- Cleanse
		[20271] = "PALADIN", -- Judgement
		[20167] = "PALADIN", -- Seal of Insight
		[63148] = "PALADIN", -- Divine Shield
		[25780] = "PALADIN", -- Righteous Fury
		[20217] = "PALADIN", -- Blessing of Kings
		[42463] = "PALADIN", -- Seal of Truth
		[20154] = "PALADIN", -- Seal of Righteousness
		[53385] = "PALADIN", -- Divine Storm
		[20165] = "PALADIN", -- Seal of Insight

		[33110] = "PRIEST", -- Prayer of Mending
		[56160] = "PRIEST", -- Glyph of Power Word: Shield
		[15473] = "PRIEST", -- Shadowform
		[33206] = "PRIEST", -- Pain Suppression
		[32375] = "PRIEST", -- Mass Dispel
		[34919] = "PRIEST", -- Vampiric Touch
		[64843] = "PRIEST", -- Divine Hymn
		[64901] = "PRIEST", -- Hymn of Hope
		[34433] = "PRIEST", -- Shadowfiend
		[1706]  = "PRIEST", -- Levitate
		[64844] = "PRIEST", -- Divine Hymn
		[34650] = "PRIEST", -- Mana Leech
		[15286] = "PRIEST", -- Vampiric Embrace
		[47585] = "PRIEST", -- Dispersion
		[15290] = "PRIEST", -- Vampiric Embrace
		[586]   = "PRIEST", -- Fade
		[64904] = "PRIEST", -- Hymn of Hope
		[10060] = "PRIEST", -- Power Infusion
		[15487] = "PRIEST", -- Silence
		[47753] = "PRIEST", -- Divine Aegis
		[47788] = "PRIEST", -- Guardian Spirit
		[114908] = "PRIEST", -- Spirit Shell absorb

		[31224] = "ROGUE", -- Cloak of Shadows
		[35546] = "ROGUE", -- Combat Potency
		[13877] = "ROGUE", -- Blade Flurry
		[3408]  = "ROGUE", -- Crippling Poison
		[51637] = "ROGUE", -- Venomous Vim
		[22482] = "ROGUE", -- Blade Flurry
		[8679]  = "ROGUE", -- Instant Poison
		[57841] = "ROGUE", -- Killing Spree
		[57934] = "ROGUE", -- Tricks of the Trade
		[2094]  = "ROGUE", -- Blind
		[2823]  = "ROGUE", -- Deadly Poison
		[2818]  = "ROGUE", -- Deadly Poison
		[13750] = "ROGUE", -- Adrenaline Rush
		[14185] = "ROGUE", -- Preparation
		[1766]  = "ROGUE", -- Kick
		[8680]  = "ROGUE", -- Instant Poison
		[5277]  = "ROGUE", -- Evasion
		[51713] = "ROGUE", -- Shadow Dance
		[51690] = "ROGUE", -- Killing Spree
		[1833]  = "ROGUE", -- Cheap Shot
		[36554] = "ROGUE",   -- Shadowstep
		[5761]  = "ROGUE",   -- Mind-numbing Poison
		[14183] = "ROGUE",   -- Premeditation
		[51723] = "ROGUE",   -- Fan of Knives
		[57842] = "ROGUE",   -- Killing Spree Off-Hand

		[51533] = "SHAMAN",  -- Feral Spirit
		[51522] = "SHAMAN",  -- Primal Wisdom
		[30823] = "SHAMAN",  -- Shamanistic Rage
		[60103] = "SHAMAN",  -- Lava Lash
		[2894]  = "SHAMAN",  -- Fire Elemental Totem
		[2062]  = "SHAMAN",  -- Earth Elemental Totem
		[17364] = "SHAMAN",  -- Stormstrike
		[16166] = "SHAMAN",  -- Elemental Mastery
		[57994] = "SHAMAN",  -- Wind Shear
		[2825]  = "SHAMAN",  -- Bloodlust
		[8143]  = "SHAMAN",  -- Tremor Totem
		[55533] = "SHAMAN",  -- Glyph of Healing Wave
		[36936] = "SHAMAN",  -- Totemic Recall
		[8177]  = "SHAMAN",  -- Grounding Totem
		[52042] = "SHAMAN",  -- Healing Stream Totem
		[32175] = "SHAMAN",  -- Stormstrike
		[51505] = "SHAMAN",  -- Lava Burst
		[61882] = "SHAMAN",  -- Earthquake
		[51886] = "SHAMAN",  -- Cleanse Spirit
		[32176] = "SHAMAN",  -- Stormstrike Off-Hand
		[16188] = "SHAMAN",  -- Nature's Swiftness
		[25504] = "SHAMAN",  -- Windfury Attack
		[2645]  = "SHAMAN",  -- Ghost Wolf
		[10444] = "SHAMAN",  -- Flametongue Attack
		[58875] = "SHAMAN",  -- Spirit Walk
		[51514] = "SHAMAN",  -- Hex
		[63375] = "SHAMAN",  -- Primal Wisdom
		[21169] = "SHAMAN",  -- Reincarnation

		[48018] = "WARLOCK", -- Demonic Circle: Summon
		[17962] = "WARLOCK", -- Conflagrate
		[688]   = "WARLOCK", -- Summon Imp
		[1122]  = "WARLOCK", -- Summon Infernal
		[20707] = "WARLOCK", -- Soulstone Resurrection
		[63106] = "WARLOCK", -- Siphon Life
		[22703] = "WARLOCK", -- Infernal Awakening
		[697]   = "WARLOCK", -- Summon Voidwalker
		[48020] = "WARLOCK", -- Demonic Circle: Teleport
		[29858] = "WARLOCK", -- Soulshatter
		[47231] = "WARLOCK", -- Fel Synergy
		[18540] = "WARLOCK", -- Summon Doomguard
		[30146] = "WARLOCK", -- Summon Felguard
		[47230] = "WARLOCK", -- Fel Synergy

		[18499] = "WARRIOR", -- Berserker Rage
		[1680]  = "WARRIOR", -- Whirlwind
		[1715]  = "WARRIOR", -- Hamstring
		[12323] = "WARRIOR", -- Piercing Howl
		[871]   = "WARRIOR", -- Shield Wall
		[57755] = "WARRIOR", -- Heroic Throw
		[55694] = "WARRIOR", -- Enraged Regeneration
		[12292] = "WARRIOR", -- Death Wish
		[2565]  = "WARRIOR", -- Shield Block
		[34428] = "WARRIOR", -- Victory Rush
		[7384]  = "WARRIOR", -- Overpower
		[1719]  = "WARRIOR", -- Recklessness
		[355]   = "WARRIOR", -- Taunt
		[2457]  = "WARRIOR", -- Battle Stance
		[52174] = "WARRIOR", -- Heroic Leap
		[29842] = "WARRIOR", -- Second Wind
		[23881] = "WARRIOR", -- Bloodthirst
		[5246]  = "WARRIOR", -- Intimidating Shout
		[23920] = "WARRIOR", -- Spell Reflection
		[12975] = "WARRIOR", -- Last Stand
		[44949] = "WARRIOR", -- Whirlwind Off-Hand
		[64382] = "WARRIOR", -- Shattering Throw
		[3411]  = "WARRIOR", -- Intervene
		[12723] = "WARRIOR", -- Sweeping Strikes
		[29841] = "WARRIOR", -- Second Wind
		[50622] = "WARRIOR", -- Whirlwind
		[12328] = "WARRIOR", -- Sweeping Strikes
		[6552]  = "WARRIOR", -- Pummel
		[46924] = "WARRIOR", -- Bladestorm
		[46968] = "WARRIOR", -- Shockwave
	}

	_detalhes.HardCrowdControlSpells = {
		--> death knight

		--> druid
		[33786] = true, -- Cyclone

		--> hunter

		--> mage

		--> paladin

		--> priest

		--> rogue

		--> shaman

		--> warlock

		--> warrior

	}

	_detalhes.CrowdControlSpells = {
		--Racials
		[28730]	= true, -- Arcane Torrent(be)
		[25046]	= true, -- Arcane Torrent(be)
		[50613]	= true, -- Arcane Torrent(be)
		[69179]	= true, -- Arcane Torrent(be)
		[80483]	= true, -- Arcane Torrent(be)
		[20549]	= true, -- War Stomp(tauren)

		--death knight
		[47481]	= true, -- Gnaw
		[47528]	= true, -- Mind Freeze
		[47476]	= true, -- Strangulate
		[49203]	= true, -- Hungering Cold

		--druid
		[33786]	= true, -- Cyclone
		[339]	= true, -- Entangling Toots
		[45334]	= true, -- Immobilized(from Wild Charge)
		[99]	= true, -- Incapacitating Roar
		[22570]	= true, -- Maim
		[5211]	= true, -- Mighty Bash(talent)
		[16979]	= true, -- Wild Charge(talent)
		[78675]	= true, -- Solar Beam

		--hunter
		[64803]	= true, -- Entrapment
		[3355]	= true, -- Freezing trap
		[24394]	= true, -- Intimidation(pet)
		[24335]	= true, -- Wyvern sting
		[19386]	= true, -- Wyvern sting

		--mage
		[2139]	= true, -- Counterspell
		[44572]	= true, -- Deep Freeze
		[58534]	= true, -- Deep Freeze
		[31661]	= true, -- Dragon's Breath
		[33395]	= true, -- Freeze(pet)
		[122]	= true, -- Frost Nova
		[118]	= true, -- Polymorph sheep
		[28272]	= true, -- Polymorph pig
		[61305]	= true, -- Polymorph black cat
		[61721]	= true, -- Polymorph rabbit
		[61780]	= true, -- Polymorph turkey
		[28271]	= true, -- Polymorph turtle

		--paladin
		[31935]	= true, -- Avenger's Shield
		[853]	= true, -- Hammer of Justice
		[20066]	= true, -- Repentance

		--priest
		[605]	= true, -- Dominate Mind
		[64044]	= true, -- Psychic Horror
		[8122]	= true, -- Psychic scream
		[9484]	= true, -- Shackle undead
		[15487]	= true, -- Silence

		--rogue
		[2094]	= true, -- Blind
		[1833]	= true, -- Cheap shot
		[1330]	= true, -- Garrote
		[1776]	= true, -- Gouge
		[1766]	= true, -- Kick
		[408]	= true, -- Kidney shot
		[6770]	= true, -- Sap

		--shaman
		[64695]	= true, -- Earthgrab(earthgrab totem)
		[51514]	= true, -- Hex
		[51490]	= true, -- Thunderstorm
		[57994]	= true, -- Wind Shear

		--warlock
		[5782]	= true, -- Fear
		[5484]	= true, -- Howl of terror
		[6789]	= true, -- Mortal Coil
		[6358]	= true, -- Seduction(succubus)
		[30283]	= true, -- Shadowfury
		[19647]	= true, -- Spell Lock(Felhunters)
		[31117]	= true, -- Unstable Affliction

		--warrior
		[100]	= true, -- Charge
		[5246]	= true, -- Intimidating shout
		[6552]	= true, -- Pummel
		[7922]	= true, -- Warbringer
	}

	_detalhes.MageWardSpells = {
		[543] = 30 , -- Mage Ward (Mage)
	}

	_detalhes.WarlockShadowWardSpells = {
		[6229] = 30, -- Shadow Ward (warlock) Rank 1
	}

	_detalhes.MageIceBarrierSpells = {
		[11426] = 60, -- Ice Barrier (Mage)
	}

	_detalhes.WarlockSacrificeSpells = {
		[7812] = 30, -- Sacrifice (warlock)
	}

	_detalhes.AbsorbSpells = {
		-- Death Knight
		[48707] = 5, -- Anti-Magic Shell
		[51052] = 10, -- Anti-Magic Zone
			-- Does DK Spell Deflection show absorbs in the CL?
		[77535] = 10, -- Blood Shield
		
		[116888]	=	3, -- Shroud of Purgatory (Talent)
		[115635]	=	12, -- Death Coil (Glyph)

		-- Druid
		
		-- Mage
		[11426] = 60, -- Ice Barrier (Talent)
		[1463] = 8, -- Encanter's Ward (Talent)

		[140468] = 86400, -- Flame Glow Passive (Talent)

		-- Monk
		[118604] = 30, -- Guard Statue
		[115295] = 30, -- Guard
		[116849] = 12, -- Life Cocoon

		[145051] = 30, -- Brewmaster T16 2p
		[145441] = 15, -- Mistweaver T16 2p
		[122470] = 10, -- Touch of Karma

		-- Paladin
		[86273] = 15, -- Illuminated Healing
		[65148] = 6, -- Sacred Shield

		-- Priest
		[17] = 15, -- Power Word: Shield
		[47753] = 15, -- Divine Aegis
		[47788] = 10, -- Guardian Spirit
		[114908] = 15, -- Spirit Shell

		[114214] = 20, -- Angelic Bulwark (Talent)

		-- Shaman
		[145379] = 15, -- Resto T16 2p

		[114893]	=	10, -- Stone Bulwark (Talent)

		-- Warlock
		[108366] = 15, -- Soul Leech (Talent)
		[7812] = 30, -- Sacrifice (Voidwalker?)
		[6229] = 30, -- Twilight Ward

		[108416] = 20, --  Sacrifical Pact (Talent)
		[110913] = 8, -- Dark Bargain (Talent)

		--Warrior
		[112048] = 15, -- Shield Barrier

		-- Consumables
		[29674] = 86400, -- Lesser Ward of Shielding
		[29719] = 86400, -- Greater Ward of Shielding (these have infinite duration, set for a day here :P)
		[29701] = 86400,
		[28538] = 120, -- Major Holy Protection Potion
		[28537] = 120, -- Major Shadow
		[28536] = 120, --  Major Arcane
		[28513] = 120, -- Major Nature
		[28512] = 120, -- Major Frost
		[28511] = 120, -- Major Fire
		[7233] = 120, -- Fire
		[7239] = 120, -- Frost
		[7242] = 120, -- Shadow Protection Potion
		[7245] = 120, -- Holy
		[6052] = 120, -- Nature Protection Potion
		[53915] = 120, -- Mighty Shadow Protection Potion
		[53914] = 120, -- Mighty Nature Protection Potion
		[53913] = 120, -- Mighty Frost Protection Potion
		[53911] = 120, -- Mighty Fire
		[53910] = 120, -- Mighty Arcane
		[17548] = 120, --  Greater Shadow
		[17546] = 120, -- Greater Nature
		[17545] = 120, -- Greater Holy
		[17544] = 120, -- Greater Frost
		[17543] = 120, -- Greater Fire
		[17549] = 120, -- Greater Arcane
		[28527] = 15, -- Fel Blossom
		[29432] = 3600, -- Frozen Rune usage (Naxx classic)

		-- Item usage
		[36481] = 4, -- Arcane Barrier (TK Kael'Thas) Shield
		[57350] = 6, -- Darkmoon Card: Illusion
		[17252] = 30, -- Mark of the Dragon Lord (LBRS epic ring) usage
		[25750] = 15, -- Defiler's Talisman/Talisman of Arathor Rank 1
		[25747] = 15,
		[25746] = 15,
		[23991] = 15,
		[31000] = 300, -- Pendant of Shadow's End Usage
		[30997] = 300, -- Pendant of Frozen Flame Usage
		[31002] = 300, -- Pendant of the Null Rune
		[30999] = 300, -- Pendant of Withering
		[30994] = 300, -- Pendant of Thawing
		[23506]= 20, -- Arena Grand Master Usage (Aura of Protection)
		[12561] = 60, -- Goblin Construction Helmet usage
		[31771] = 20, -- Runed Fungalcap usage
		[21956] = 10, -- Mark of Resolution usage
		[29506] = 20, -- The Burrower's Shell
		[4057] = 60, -- Flame Deflector
		[4077] = 60, -- Ice Deflector
		[39228] = 20, -- Argussian Compass (may not be an actual absorb)

		-- Item procs
		[27779] = 30, -- Divine Protection - Priest dungeon set 1/2  Proc
		[11657] = 20, -- Jang'thraze (Zul Farrak) proc
		[10368] = 15, -- Uther's Strength proc
		[37515] = 15, -- Warbringer Armor Proc
		[42137] = 86400, -- Greater Rune of Warding Proc
		[26467] = 30, -- Scarab Brooch proc
		[27539] = 6, -- Thick Obsidian Breatplate proc
		[28810] = 30, -- Faith Set Proc Armor of Faith
		[54808] = 12, -- Noise Machine proc Sonic Shield 
		[55019] = 12, -- Sonic Shield (one of these too ought to be wrong)
		[64411] = 15, -- Blessing of the Ancient (Val'anyr Hammer of Ancient Kings equip effect)
		[64413] = 8, -- Val'anyr, Hammer of Ancient Kings proc Protection of Ancient Kings
		[105909] = 6, -- Shield of Fury (Warrior T13 Protection 2P Bonus)
		[105801] = 6, -- Delayed Judgement (Paladin T13 Protection 2P Bonus)

		[116631]	=	10, -- enchant "Enchant Weapon - Colossus"
		[140380]	=	15, -- trinket "Inscribed Bag of Hydra-Spawn"
		[138925]	=	1000, -- trinket "Stolen Relic of Zuldazar"

		-- Misc
		[40322] = 30, -- Teron's Vengeful Spirit Ghost - Spirit Shield

		-- Boss abilities
		[65874] = 15, -- Twin Val'kyr's Shield of Darkness 175000
		[67257] = 15, -- 300000
		[67256] = 15, -- 700000
		[67258] = 15, -- 1200000
		[65858] = 15, -- Twin Val'kyr's Shield of Lights 175000
		[67260] = 15, -- 300000
		[67259] = 15, -- 700000
		[67261] = 15, -- 1200000
		[65686] = 86400, -- Twin Val'kyr Light Essence
		[65684] = 86400, -- Twin Val'kyr Dark Essence
	}

	-- ~cooldown duration self
	_detalhes.DefensiveCooldownSpellsNoBuff = {
		[20594]	= {120, 8, 1}, -- racial stoneform

--		[6262] = {120, 1, 1}, -- healthstone

--		["DEATHKNIGHT"] = {},
		[48743]	= {120, 0, 1}, -- Death Pact
		[51052]	= {120, 3, 0}, -- Anti-Magic Zone
		[48982]	= {30, 0, 1}, -- "Blood Tap"

--		["DRUID"] = {},
		[740]	= {480, 8, 0}, -- Tranquility
		[22842] = {1.5, 0, 1}, -- Frenzied Regeneration
		[108238] = {120, 0, 1}, -- Renewal

--		["HUNTER"] = {},
		[5384] = {30, 0, 1}, -- Feign Death
		[109304] = {120, 0, 1}, -- Exhilaration

--		["MAGE"] = {},
		[55342] = {180, 0, 1}, -- Mirror Image
--		["PALADIN"] = {},
		[633]	= {600, 0, 0}, -- Lay on Hands
		[31821]	= {180, 6, 0}, -- Devotion Aura
		[66235] = {180, 0, 1}, -- Ardent Defender Proc
		[114039] = {30, 6, 0}, -- Hand of Purity

--		["PRIEST"] = {},
		[62618]	= {180, 10, 0}, -- Power Word: Barrier
		[109964] = {60, 10, 0}, --Spirit Shell
		[64843] = {180, 8, 0}, --Divine Hymn
		[108968] = {300, 0, 0}, --Void Shift holy disc
		[142723] = {600, 0, 0}, --Void Shift shadow

--		["ROGUE"] = {},
		[76577] = {180, 5, 0}, -- Smoke Bomb

--		["SHAMAN"] = {},

--		["WARLOCK"] = {108416, 6229},
		[6229]	= {30, 30, 1}, -- Twilight Ward 1 = self

--		["WARRIOR"] = {},
		[2565]	= {12, 6, 1}, -- Shield Block
		[871]	= {180, 12, 1}, -- Shield Wall
		[12975]	= {180, 20, 1}, -- Last Stand
		[23920]	= {25, 5, 1}, -- Spell Reflection
		[114030] = {120, 12, 0}, -- Vigilance
		[114029] = {30, 6, 0}, -- Safeguard
		[3411] = {30, 0, 0}, -- Intervene
		[114203] = {180, 15, 0}, -- Demoralizing Banner
		[1160] = {60, 10, 1}, -- Demoralizing Shout
		[97462] = {180, 10, 0}, -- Rallying Cry
	}

	--> spellid = {cooldown, duration}
	_detalhes.DefensiveCooldownSpells = {
		-- Death Knight
		[55233] = {60, 10}, -- Vampiric Blood
		[49222] = {60, 300}, -- Bone Shield
		[48792] = {180, 12}, -- Icebound Fortitude
		[48743] = {120, 0}, -- Death Pact
		[49039] = {12, 10}, -- Lichborne
		[48707] = {45, 7}, -- Anti-Magic Shell
		[49028] = {12, 10}, -- Dancing Rune Weapon
		[51052] = {120, 10}, -- Anti-Magic Zone
		["DEATHKNIGHT"] = {55233, 49222, 48792, 48743, 49039, 48707, 49028, 51052, 48982},

		-- Druid
		[62606] = {1.5, 6}, -- Savage Defense
		[61336] = {180, 12}, -- Survival Instincts
		[22812] = {60, 12}, -- Barkskin
		[106922] = {180, 20}, -- Might of Ursoc
		["DRUID"] = {62606, 61336, 22812, 740, 22842, 108238},

		-- Hunter
		[19263] = {120, 5}, -- Deterrence
		["HUNTER"] = {19263, 5384, 109304},

		-- Mage
		[45438] = {300, 12}, -- Ice Block
		[11426] = {25, 60}, -- Ice Barrier
		[1463] = {25, 8}, -- Incanter's Ward
		[110959] = {60, 3}, -- Greater Invisibility
		[115610] = {25, 4}, -- Temporal Shield
		[66] = {300, 3}, -- Invisibility
		[108978] = {180, 6}, -- Alter Time
		["MAGE"] = {45438, 11426, 1463, 110959, 115610, 66, 108978, 55342},

		-- Paladin
		[498] = {60, 10}, -- Divine Protection
		[642] = {300, 8}, -- Divine Shield
		[6940] = {120, 12}, -- Hand of Sacrifice
		[1022] = {300, 10}, -- Hand of Protection
		[1038] = {120, 10}, -- Hand of Salvation
		[114039] = {30, 6}, -- Hand of Purity
		[31850] = {180, 10}, -- Ardent Defender
		[86659] = {180, 12}, -- Guardian of Ancient Kings
		["PALADIN"] = {86659, 31850, 498, 642, 6940, 1022, 1038, 114039, 31850, 86659, 633, 31821, 66235},

		-- Priest
		[47788] = {180, 10}, -- Guardian Spirit
		[47585] = {120, 6}, -- Dispersion
		[33206] = {180, 8}, -- Pain Suppression
		[112833] = {30, 6}, -- Spectral Guise
		[114214] = {90, 20}, -- Angelic Bulwark
		["PRIEST"] = {47788, 47585, 33206, 112833, 114214, 62618, 109964, 64843},

		-- Rogue
		[1966] = {1.5, 5}, -- Feint
		[31224] = {60, 5}, -- Cloak of Shadows
		[5277] = {180, 15}, -- Evasion
		[1856] = {120, 0}, -- Vanish
		[74001] = {120, 20}, -- Combat Readiness
		[45182] = {360, 3}, -- Cheat Death
		["ROGUE"] = {1966, 31224, 5277, 1856, 74001, 45182, 76577},

		-- Shaman
		[30823] = {60, 15}, -- Shamanistic Rage
		[108270] = {60, 30}, -- Stone Bulwark Totem
		[108271] = {60, 6}, -- Astral Shift
		[31616] = {30, 10}, -- Nature's Guardian
		["SHAMAN"] = {30823, 108270, 108271, 31616},

		-- Warlock
		[104773] = {180, 8}, -- Unending Resolve
		[108359] = {120, 12}, -- Dark Regeneration
		[110913] = {180, 8}, -- Dark Bargain
		[108416] = {60, 20}, -- Sacrificial Pact
		[132413] = {120, 20}, -- Shadow Bulwark
		["WARLOCK"] = {104773, 108359, 110913, 108416, 132413, 6229},

		-- Warrior
		[871] = {180, 12}, -- Shield Wall
		[12975] = {180, 20}, -- Last Stand
		[23920] = {25, 5}, -- Spell Reflection
		[118038] = {120, 8}, -- Die by the Sword
		["WARRIOR"] = {871, 12975, 23920, 118038, 2565, 114030, 114029, 3411, 114203, 1160, 97462}
	}


	local Loc = LibStub("AceLocale-3.0"):GetLocale( "Details" )
	_detalhes.SpellOverwrite = {
		[124464] = {name = GetSpellInfo(124464).." ("..Loc["STRING_MASTERY"]..")"}, --> Shadow Word: Pain mastery proc (Shadow Priest)
		[124465] = {name = GetSpellInfo(124465).." ("..Loc["STRING_MASTERY"]..")"}, --> Vampiric Touch mastery proc (Shadow Priest)
		[124468] = {name = GetSpellInfo(124468).." ("..Loc["STRING_MASTERY"]..")"}, --> Mind Flay mastery proc (Shadow Priest)
		[124467] = {name = GetSpellInfo(124467).." ("..Loc["STRING_MASTERY"]..")"}, --> Devouring Plague mastery proc (Shadow Priest)
		[124469] = {name = GetSpellInfo(124469).." ("..Loc["STRING_MASTERY"]..")"}, --> Mind Sear mastery proc (Shadow Priest)
	}

	function _detalhes:IsCooldown(spellid)
		return _detalhes.DefensiveCooldownSpellsNoBuff[spellid] or _detalhes.DefensiveCooldownSpells[spellid]
	end

	_detalhes.spells_school = {
		[1] = {name = STRING_SCHOOL_PHYSICAL, formated = "|cFFFFFF00"..STRING_SCHOOL_PHYSICAL.."|r", hex = "FFFFFF00", rgb = {255, 255, 0}, decimals = {1.00, 1.00, 0.00}},
		[2] = {name = STRING_SCHOOL_HOLY, formated = "|cFFFFE680"..STRING_SCHOOL_HOLY.."|r", hex = "FFFFE680", rgb = {255, 230, 128}, decimals = {1.00, 0.90, 0.50}},
		[4] = {name = STRING_SCHOOL_FIRE, formated = "|cFFFF8000"..STRING_SCHOOL_FIRE.."|r", hex = "FFFF8000", rgb = {255, 128, 0}, decimals = {1.00, 0.50, 0.00}},
		[8] = {name = STRING_SCHOOL_NATURE, formated = "|cFFbeffbe"..STRING_SCHOOL_NATURE.."|r", hex = "FFbeffbe", rgb = {190, 190, 190}, decimals = {0.7451, 1.0000, 0.7451}},
		[16] = {name = STRING_SCHOOL_FROST, formated = "|cFF80FFFF"..STRING_SCHOOL_FROST.."|r", hex = "FF80FFFF", rgb = {128, 255, 255}, decimals = {0.50, 1.00, 1.00}},
		[32] = {name = STRING_SCHOOL_SHADOW, formated = "|cFF8080FF"..STRING_SCHOOL_SHADOW.."|r", hex = "FF8080FF", rgb = {128, 128, 255}, decimals = {0.50, 0.50, 1.00}},
		[64] = {name = STRING_SCHOOL_ARCANE, formated = "|cFFFF80FF"..STRING_SCHOOL_ARCANE.."|r", hex = "FFFF80FF", rgb = {255, 128, 255}, decimals = {1.00, 0.50, 1.00}},
		[3] = {name = STRING_SCHOOL_HOLYSTRIKE, formated = "|cFFFFF240"..STRING_SCHOOL_HOLYSTRIKE.."|r", hex = "FFFFF240", rgb = {255, 64, 64}, decimals = {1.0000, 0.9490, 0.2510}}, --#FFF240
		[5] = {name = STRING_SCHOOL_FLAMESTRIKE, formated = "|cFFFFB900"..STRING_SCHOOL_FLAMESTRIKE.."|r", hex = "FFFFB900", rgb = {255, 0, 0}, decimals = {1.0000, 0.7255, 0.0000}}, --#FFB900
		[6] = {name = STRING_SCHOOL_HOLYFIRE, formated = "|cFFFFD266"..STRING_SCHOOL_HOLYFIRE.."|r", hex = "FFFFD266", rgb = {255, 102, 102}, decimals = {1.0000, 0.8235, 0.4000}}, --#FFD266
		[9] = {name = STRING_SCHOOL_STORMSTRIKE, formated = "|cFFAFFF23"..STRING_SCHOOL_STORMSTRIKE.."|r", hex = "FFAFFF23", rgb = {175, 35, 35}, decimals = {0.6863, 1.0000, 0.1373}}, --#AFFF23
		[10] = {name = STRING_SCHOOL_HOLYSTORM, formated = "|cFFC1EF6E"..STRING_SCHOOL_HOLYSTORM.."|r", hex = "FFC1EF6E", rgb = {193, 110, 110}, decimals = {0.7569, 0.9373, 0.4314}}, --#C1EF6E
		[12] = {name = STRING_SCHOOL_FIRESTORM, formated = "|cFFAFB923"..STRING_SCHOOL_FIRESTORM.."|r", hex = "FFAFB923", rgb = {175, 35, 35}, decimals = {0.6863, 0.7255, 0.1373}}, --#AFB923
		[17] = {name = STRING_SCHOOL_FROSTSTRIKE, formated = "|cFFB3FF99"..STRING_SCHOOL_FROSTSTRIKE.."|r", hex = "FFB3FF99", rgb = {179, 153, 153}, decimals = {0.7020, 1.0000, 0.6000}},--#B3FF99
		[18] = {name = STRING_SCHOOL_HOLYFROST, formated = "|cFFCCF0B3"..STRING_SCHOOL_HOLYFROST.."|r", hex = "FFCCF0B3", rgb = {204, 179, 179}, decimals = {0.8000, 0.9412, 0.7020}},--#CCF0B3
		[20] = {name = STRING_SCHOOL_FROSTFIRE, formated = "|cFFC0C080"..STRING_SCHOOL_FROSTFIRE.."|r", hex = "FFC0C080", rgb = {192, 128, 128}, decimals = {0.7529, 0.7529, 0.5020}}, --#C0C080
		[24] = {name = STRING_SCHOOL_FROSTSTORM, formated = "|cFF69FFAF"..STRING_SCHOOL_FROSTSTORM.."|r", hex = "FF69FFAF", rgb = {105, 175, 175}, decimals = {0.4118, 1.0000, 0.6863}}, --#69FFAF
		[33] = {name = STRING_SCHOOL_SHADOWSTRIKE, formated = "|cFFC6C673"..STRING_SCHOOL_SHADOWSTRIKE.."|r", hex = "FFC6C673", rgb = {198, 115, 115}, decimals = {0.7765, 0.7765, 0.4510}},--#C6C673
		[34] = {name = STRING_SCHOOL_SHADOWHOLY, formated = "|cFFD3C2AC"..STRING_SCHOOL_SHADOWHOLY.."|r", hex = "FFD3C2AC", rgb = {211, 172, 172}, decimals = {0.8275, 0.7608, 0.6745}},--#D3C2AC
		[36] = {name = STRING_SCHOOL_SHADOWFLAME, formated = "|cFFB38099"..STRING_SCHOOL_SHADOWFLAME.."|r", hex = "FFB38099", rgb = {179, 153, 153}, decimals = {0.7020, 0.5020, 0.6000}}, -- #B38099
		[40] = {name = STRING_SCHOOL_SHADOWSTORM, formated = "|cFF6CB3B8"..STRING_SCHOOL_SHADOWSTORM.."|r", hex = "FF6CB3B8", rgb = {108, 184, 184}, decimals = {0.4235, 0.7020, 0.7216}}, --#6CB3B8
		[48] = {name = STRING_SCHOOL_SHADOWFROST, formated = "|cFF80C6FF"..STRING_SCHOOL_SHADOWFROST.."|r", hex = "FF80C6FF", rgb = {128, 255, 255}, decimals = {0.5020, 0.7765, 1.0000}},--#80C6FF
		[65] = {name = STRING_SCHOOL_SPELLSTRIKE, formated = "|cFFFFCC66"..STRING_SCHOOL_SPELLSTRIKE.."|r", hex = "FFFFCC66", rgb = {255, 102, 102}, decimals = {1.0000, 0.8000, 0.4000}},--#FFCC66
		[66] = {name = STRING_SCHOOL_DIVINE, formated = "|cFFFFBDB3"..STRING_SCHOOL_DIVINE.."|r", hex = "FFFFBDB3", rgb = {255, 179, 179}, decimals = {1.0000, 0.7412, 0.7020}},--#FFBDB3
		[68] = {name = STRING_SCHOOL_SPELLFIRE, formated = "|cFFFF808C"..STRING_SCHOOL_SPELLFIRE.."|r", hex = "FFFF808C", rgb = {255, 140, 140}, decimals = {1.0000, 0.5020, 0.5490}}, --#FF808C
		[72] = {name = STRING_SCHOOL_SPELLSTORM, formated = "|cFFAFB9AF"..STRING_SCHOOL_SPELLSTORM.."|r", hex = "FFAFB9AF", rgb = {175, 175, 175}, decimals = {0.6863, 0.7255, 0.6863}}, --#AFB9AF
		[80] = {name = STRING_SCHOOL_SPELLFROST, formated = "|cFFC0C0FF"..STRING_SCHOOL_SPELLFROST.."|r", hex = "FFC0C0FF", rgb = {192, 255, 255}, decimals = {0.7529, 0.7529, 1.0000}},--#C0C0FF
		[96] = {name = STRING_SCHOOL_SPELLSHADOW, formated = "|cFFB980FF"..STRING_SCHOOL_SPELLSHADOW.."|r", hex = "FFB980FF", rgb = {185, 255, 255}, decimals = {0.7255, 0.5020, 1.0000}},--#B980FF

		[28] = {name = STRING_SCHOOL_ELEMENTAL, formated = "|cFF0070DE"..STRING_SCHOOL_ELEMENTAL.."|r", hex = "FF0070DE", rgb = {0, 222, 222}, decimals = {0.0000, 0.4392, 0.8706}},
		[124] = {name = STRING_SCHOOL_CHROMATIC, formated = "|cFFC0C0C0"..STRING_SCHOOL_CHROMATIC.."|r", hex = "FFC0C0C0", rgb = {192, 192, 192}, decimals = {0.7529, 0.7529, 0.7529}},
		[126] = {name = STRING_SCHOOL_MAGIC, formated = "|cFF1111FF"..STRING_SCHOOL_MAGIC.."|r", hex = "FF1111FF", rgb = {17, 255, 255}, decimals = {0.0667, 0.0667, 1.0000}},
		[127] = {name = STRING_SCHOOL_CHAOS, formated = "|cFFFF1111"..STRING_SCHOOL_CHAOS.."|r", hex = "FFFF1111", rgb = {255, 17, 17}, decimals = {1.0000, 0.0667, 0.0667}},
	}

	_detalhes.OverrideSpellSchool = {
		--[196917] = 126, --light of the martyr - from holy to fire
	}

	function _detalhes:GetSpellSchoolName(school)
		return _detalhes.spells_school[school] and _detalhes.spells_school[school].name or ""
	end

	function _detalhes:GetSpellSchoolFormatedName(school)
		return _detalhes.spells_school[school] and _detalhes.spells_school[school].formated or ""
	end

	local default_school_color = {145/255, 180/255, 228/255}
	function _detalhes:GetSpellSchoolColor(school)
		return unpack(_detalhes.spells_school[school] and _detalhes.spells_school[school].decimals or default_school_color)
	end

	function _detalhes:GetCooldownList(class)
		class = class or select(2, UnitClass("player"))
		return _detalhes.DefensiveCooldownSpells[class]
	end
end