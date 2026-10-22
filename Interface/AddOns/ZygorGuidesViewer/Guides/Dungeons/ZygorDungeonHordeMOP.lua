local ZygorGuidesViewer=ZygorGuidesViewer
if not ZygorGuidesViewer then return end
if UnitFactionGroup("player")~="Horde" then return end
if ZGV:DoMutex("DungeonHMOP") then return end
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\End Time (ET)\\End Time (ET) Tank Gear",[[
description This will help you see what gear you can use in the End Time Dungeon
step
.from Echo of Baine##54431, Echo of Jaina##54445, Echo of Sylvanas##54123, Echo of Tyrande##54544 |only DeathKnight, Paladin, Warrior, Druid
.item Axe of the Tauren Chieftains##72814 |tip 2H Axe |only DeathKnight
.item Gauntlets of Temporal Interference##72800 |tip Hands |only DeathKnight, Paladin, Warrior
.item Girdle of Lost Heroes##72803 |tip Waist |only DeathKnight, Paladin, Warrior
.item Bloodhoof Legguards##72815 |tip Legs |only DeathKnight, Paladin, Warrior
.item Gloves of the Hollow##72805 |tip Hands |only Druid
'_
.from Murozond##54432 |only DeathKnight, Paladin, Warrior
.item Breastplate of Tarnished Bronze##72818 |tip Chest |only DeathKnight, Paladin, Warrior, Druid
.item Time Twisted Tunic##72824 |tip Chest |only Druid
.item Chrono Boots##72819 |tip Feet |only Druid
.' There are no items for your class |only Warlock, Mage, Hunter, Priest, Rogue, Shaman
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\End Time (ET)\\End Time (ET) Dps Gear",[[
dungeon 820
description This will help you see what gear you can use in the End Time Dungeon
step
from Echo of Jaina##54445 |only Druid, Priest, Warlock, Shaman, Mage, Paladin
item Jaina's Staff##72808 |tip Two-Hand Staff |only Druid, Priest, Warlock, Shaman, Mage
item Ward of Incantations##72809 |tip Shield |only Paladin, Shaman
'_ |only Druid, Priest, Warlock, Shaman, Mage, Paladin
from Echo of Tyrande##54544 |only Shaman, Paladin, Warlock, Priest, Mage
item Crescent Moon##72812 |tip Main Hand Axe |only Shaman, Paladin
item Whisperwind Robes##72813 |tip Chest |only Warlock, Priest, Mage
'_ |only Shaman, Paladin, Warlock, Priest, Mage
from Echo of Sylvanas##54123 |only Rogue, Hunter, Druid, Shaman
item Windrunner's Bow##72810 |tip Bow |only Rogue, Hunter
item Cloak of the Banshee Queen##72811 |tip Cloak |only Druid, Rogue, Hunter, Shaman
'_ |only Rogue, Hunter, Druid, Shaman
from Echo of Jaina##54445, Echo of Tyrande##54544, Echo of Sylvanas##54123, Echo of Baine##54431 |only DeathKnight, Paladin, Warrior, Shaman, Druid, Rogue, Warlock, Priest, Mage, Hunter
item Axe of the Tauren Chieftains##72814 |tip Two-Hand Axe |only DeathKnight, Paladin, Warrior
item Dragonshrine Scepter##72804 |tip One-Hand Weapon |only DeathKnight, Paladin, Warrior, Shaman
item Echoing Headguard##72806 |tip Head |only Shaman
item Breastplate of Sorrow##72801 |tip Chest |only DeathKnight, Paladin, Warrior
item Gloves of the Hollow##72805 |tip Hands |only Druid, Rogue
item Cord of Lost Hope##72798 |tip Waist |only Druid
item Echoing Headguard##72806 |tip Head |only Shaman
item Time Traveler's Leggings##72802 |tip Legs |only Warlock, Priest, Mage
item Dead End Boots##72799 |tip Feet |only Shaman, Hunter
'_ |only DeathKnight, Paladin, Warrior, Shaman, Druid, Rogue, Warlock, Priest, Mage, Hunter
from Murozond##54432 |only Rogue, Shaman, Hunter, DeathKnight, Paladin, Warrior, Priest, Mage, Warlock, Druid
item Jagged Edge of Time##72822 |tip One-Hand Dagger |only Rogue, Shaman, Hunter
item Crown of Epochs##72820 |tip Head |only DeathKnight, Paladin, Warrior
item Timeway Headgear##72823 |tip Head |only Druid
item Mantle of Time##72825 |tip Shoulder |only Mage, Warlock, Priest
item Temporal Pauldrons##72821 |tip Shoulder |only DeathKnight, Paladin, Warrior
item Robes of Fate##72826 |tip Chest |only Mage, Warlock, Priest
item Time Twisted Tunic##72824 |tip Chest |only Rogue, Druid
item Distortion Greaves##72816 |tip Hands  |only Shaman
item Arrow of Time##72897 |tip Trinket  |only Rogue, Shaman, Hunter, Druid
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\End Time (ET)\\End Time (ET) Healer Gear",[[
description This will help you see what gear you can use in the End Time Dungeon
step
.from Echo of Baine##54431, Echo of Jaina##54445, Echo of Sylvanas##54123, Echo of Tyrande##54544 |only Priest, Paladin, Druid, Shaman
.item Time Traveler's Leggings##72802 |tip Legs |only Priest
.item Waistguard of Lost Time##72807 |tip Waist |only Paladin
.item Cord of Lost Hope##72798 |tip Waist |only Druid
.item Echoing Headguard##72806 |tip Head |only Shaman
'_
.from Echo of Jaina##54445 |only Druid, Priest, Shaman
.item Jaina's Staff##72808 |tip 2h Staff |only Druid,Priest,Shaman
.item Ward of Incantations##72809 |tip Wand |only Shaman,Paladin
'_
.from Echo of Tyrande##54544 |only Shaman, Priest, Paladin, Druid
.item Crescent Moon##72812 |tip Main Hand Axe |only Shaman,Priest,Paladin,Druid
.item Whisperwind Robes##72813 |tip Chest |only Priest
'_
.from Murozond##54432 |only Druid, Priest, Shaman, Paladin
.item Timeway Headgear##72823 |tip Head |only Druid
.item Mantle of Time##72825 |tip Shoulder |only Priest
.item Distortion Greaves##72816 |tip Legs |only Shaman
.item Time Altered Legguards##72817 |tip Legs |only Paladin
.' There are no items for your class |only Warlock, Rogue, Warrior, DeathKnight, Mage
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\Well of Eternity (WoE)\\Well of Eternity (WoE) Tank Gear",[[
description This will help you see what gear you can use in the End Time Dungeon
step
.from Peroth'arn##55085 |only DeathKnight, Paladin, Warrior, Druid
.item Gavel of Peroth'arn##72827 |tip 1h Mace |only DeathKnight, Paladin, Warrior
.item Peroth'arn's Belt##72830 |tip Waist |only Druid
'_  |only DeathKnight, Paladin, Warrior, Druid
.from Queen Azshara##54853 |only DeathKnight, Paladin, Warrior, Druid
.item Queen's Boon##72837 |tip Finger |only DeathKnight, Paladin, Warrior
.item Puppeteer's Pantaloons##72835 |tip Legs |only Druid
'_ |only DeathKnight, Paladin, Warrior, Druid
.from Mannoroth##54969, Varo'then##55419 |only Druid, Paladin, Warrior
.item Annihilan Helm##72842 |tip Head |only DeathKnight, Paladin, Warrior
.item Mannoroth's Signet##72845 |tip Finger |only Druid
.item Thornwood Staff##72846 |tip 2h Staff |only Druid
.item Helm of Thorns##72847 |tip Head |only Druid
.' There are no items for your class |only Warlock, Mage, Hunter, Priest, Rogue, Shaman
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\Well of Eternity (WoE)\\Well of Eternity (WoE) Dps Gear",[[
dungeon 816
description This will help you see what gear you can use in the End Time Dungeon
step
from Peroth'arn##55085 |only Warrior, Paladin, Hunter, Rogue, DeathKnight, Shaman, Priest, Mage, Warlock, Druid
item Trickster's Edge##72828 |tip One-Hand Axe |only Warrior, Paladin, Hunter, Rogue, DeathKnight, Shaman
item Orb of the First Satyrs##72829 |tip Held In Off Hand |only Paladin, Priest, Shaman, Mage, Warlock, Druid
item Girdle of the Queen's Champion##72832 |tip Waist |only Warrior, Paladin, DeathKnight
item Peroth'arn's Belt##72830 |tip Waist  |only Druid, Rogue
item Horned Band##72831 |tip Finger |only Paladin, Priest, Shaman, Mage, Warlock, Druid
'_ |only Warrior, Paladin, Hunter, Rogue, DeathKnight, Shaman, Priest, Mage, Warlock, Druid
from Queen Azshara##54853 |only Paladin, Priest, Shaman, Druid, Warrior, Hunter, Rogue, DeathKnight, Mage, Warlock
item Scepter of Azshara##72833 |tip Main Hand Mace |only Paladin, Priest, Shaman, Druid
item Cloak of the Royal Protector##72838 |tip Cloak |only Warrior, Paladin, Hunter, Rogue, DeathKnight, Shaman, Druid
item Breastplate of the Queen's Guard##72834 |tip Chest |only Hunter, Shaman
item Puppeteer's Pantaloons##72835 |tip Legs |only Rogue, Druid
item Slipper's of Wizardry##72836 |tip Feet |only Priest, Mage, Warlock
'_ |only Paladin, Priest, Shaman, Druid, Warrior, Hunter, Rogue, DeathKnight, Mage, Warlock
from Mannoroth##54969 |only Warrior, Paladin, DeathKnight, Shaman, Druid, Hunter, Priest, Mage, Warlock, Rogue
item Pit Lord's Destroyer##72844 |tip Two-Hand Mace |only Warrior, Paladin, DeathKnight, Shaman, Druid
item Thornwood Staff##72846 |tip Two-Hand Staff |only Hunter, Druid
item Cowl of Highborne Sorcerors##72839 |tip Head |only Priest, Mage, Warlock
item Helm of Thorns##72847 |tip Head |only Rogue, Druid
item Spaulders of Eternity##72840 |tip Shoulders |only Druid
item Legguards of the Legion##72848 |tip Legs |only Hunter, Shaman
item Mannoroth's Signet##72845 |tip Finger |only Warrior, Paladin, Hunter, Rogue, DeathKnight, Shaman, Druid
item Varo'then's Brooch##72899 |tip Trinket |only Warrior, Paladin, Hunter, Rogue, DeathKnight, Shaman, Druid
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\Well of Eternity (WoE)\\Well of Eternity (WoE) Healer Gear",[[
description This will help you see what gear you can use in the End Time Dungeon
step
.from Peroth'arn##55085 |only Paladin,Druid,Priest,Shaman
.item Horned Band##72831 |tip Finger |only Paladin,Druid,Priest,Shaman
'_
.from Queen Azshara##54853 |only Paladin,Druid,Priest,Shaman
.item Scepter of Azshara##72833 |tip Main Hand, Mace |only Paladin,Druid,Priest,Shaman
'_
.from Mannoroth##54969, Varo'then##55419 |only Priest, Druid, Shaman, Paladin
.item Cowl of Highborne Sorcerors##72839 |tip Head |only Priest
.item Helm of Power##72843 |tip Head |only Paladin
.item Spaulders of Eternity##72840 |tip Shouders |only Druid
.item Demonsbane Chestguard##72841 |tip Chest |only Shaman
.item Foul Gift of the Demon Lord##72841 |tip Trinket |only Shaman,Paladin,Priest,Druid
.' There are no items for your class |only Warlock, Rogue, Warrior, DeathKnight, Mage
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\Hour of Twilight (HoT)\\Hour of Twilight (HoT) Tank Gear",[[
description This will help you see what gear you can use in the End Time Dungeon
step
.from Arcurion##54590 |only DeathKnight, Paladin, Warrior, Druid
.item Iceward Cloak##72854 |tip Back |only DeathKnight, Paladin, Warrior
.item Wayfinder Boots##72849 |tip Feet |tip This item drops from Arcurion |only Druid
'_ |only DeathKnight, Paladin, Warrior, Druid
.from Asira Dawnslayer##54968 |only Paladin, Warrior
item Corrupted Carapace##72855 |tip Shield |only Paladin, Warrior
'_ |only Paladin, Warrior
.from Archbishop Benedictus##54938 |only Druid, DeathKnight, Paladin, Warrior
.item Desecrated Shoulderguards##72868 |tip Shoulder |only Druid
.item Pauldrons of the Dragonblight##72861 |tip Shoulder |only DeathKnight, Paladin, Warrior
.item Veil of Lies##72900 |tip Trinket |only Druid, Paladin, DeathKnight, Warrior
.' There are no items for your class |only Warlock, Mage, Hunter, Priest, Rogue, Shaman
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\Hour of Twilight (HoT)\\Hour of Twilight (HoT) Dps Gear",[[
dungeon 819
description This will help you see what gear you can use in the End Time Dungeon
step
from Arcurion##54590 |only Druid, Warlock, Priest, Mage, Paladin, DeathKnight, Warrior, Shaman, Rogue
item Evergreen Wristbands##76150 |tip Wrist |only Druid
item Chillbane Belt##72851 |tip Waist |only Warlock,Priest,Mage
item Arcurion Legguards##72853 |tip Legs |only Paladin,DeathKnight,Warrior
item Surestride Boots##72850 |tip Feet |only Shaman
item Wayfinder Boots##72849 |tip Feet |only Rogue,Druid
'_|only Druid, Warlock, Priest, Mage, Paladin, DeathKnight, Warrior, Shaman, Rogue
from Asira Dawnslayer##54968 |only Rogue, Shaman, Druid, Priest
item Mandible of the Old Ones##72860 |tip Off Hand Dagger |only Rogue,Shaman
item Dawnslayer Helm##72859 |tip Head |only Rogue,Shaman
item Pauldrons of Midnight Whispers##72856 |tip Shoulder |only Druid
item Cloak of Subtle Light##76151 |tip Cloak |only Priest,Druid,Shaman
item Leggings of Blinding Speed##72857 |tip Legs |only Druid
'_ |only Rogue, Shaman, Druid, Priest
from Archbishop Benedictus##54938 |only Druid, Shaman, Priest, Mage, Warlock, Rogue, Warrior, DeathKnight, Hunter, Paladin
item Stalk of Corruption##72863 |tip Two-Hand Staff |only Druid,Shaman,Priest,Mage,Warlock
item Clattering Claw##72867 |tip Main Hand Fist Weapon |only Rogue,Shaman
item Fanged Tentacle##72862 |tip Main Hand Dagger |only Mage,Shaman,Warlock,Priest,Druid
item Treachery's Bite##72866 |tip One Hand Sword |only Warrior,DeathKnight
item Dragonsmaw Blaster##72869 |tip Gun |only Hunter
item Betrayer's Pauldrons##72870 |tip Shoulder |only Hunter,Shaman
item Desecrated Shoulderguards##72868 |tip Shoulder. |only Rogue,Druid
item Mantle of False Virtue##72865 |tip Shoulder |only Mage,Pirest,Warlock
item Rosary of Light##72901 |tip Trinket |only Warrior,Paladin,DeathKnight
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Gear Guides\\Cataclysm Dungeons\\Hour of Twilight (HoT)\\Hour of Twilight (HoT) Healer Gear",[[
description This will help you see what gear you can use in the End Time Dungeon
step
.from Arcurion##54590 |only Shaman, Druid
.item Evergreen Wristbands##76150 |tip Wrist |only Druid
.item Surestride Boots##72850 |tip Feet |only Shaman
'_
.from Asira Dawnslayer##54968 |only Druid, Paladin, Druid, Shaman
.item Pauldrons of Midnight Whispers##72856 |tip Shoulder |only Shaman
.item Cloak of Subtle Light##76151 |tip Back |only Priest,Paladin,Druid,Shaman
.item Leggings of Blinding Speed##72857 |tip Legs |only Druid
'_
.from Archbishop Benedictus##54938 |only Druid, Paladin, Druid, Shaman
.item Stalk of Corruption##72863 |tip 2h Staff |only Druid,Shaman,Priest
.item Pauldrons of Conviction##72864 |tip Shoulder |only Paladin
.' There are no items for your class |only Warlock, Mage, Hunter, Priest, Rogue, Shaman
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\End Time (ET)\\End Time (ET) Tank Guide",[[
description This Guides will walk you through tanking the End of Time dungeon and questing.
author support@zygorguides.com
step
.' Press _I_ on your keyboard to open your Dungeon Finder and queue for End of Time specfic Cataclysm dungeon.
.' This guide is for a Tank role, so choose _Tank_ for your choice.
|confirm
step
goto End Time 79.6,45.1
.talk 57864
..accept 30097
step
goto 79.6,45.0
.talk 54476
..accept 30096
step
label	"transit"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Emerald"
'In this section, you will need to start the trash by aggroing a _Nightsaber_. Once you have started, you will have to fight until the boss appears.
.' There are _Nightsabers_ and _Sentinel's_ which keep spawning, that you will have to fight constantly (they do not hit hard) and stack together to keep aggro.
.' _Run to the Moonlight_ and stay in anytime it appears, this helps with killing the mobs. When one dissappears, run to the next one that spawns.
.' _Repeat until Tyrande_ show up.
.from Time-Twisted Nightsaber##54688+
.from Time-Twisted Sentinel##54512+
|confirm
step
label	"pre_Tyrande"
goto End Time/5 51.8,42.9
.from Echo of Tyrande##54544
.' Absorb or Reflect _Moonbolt_ when possible.
.' Dodge _Moonlance_.
.' Use a Damage Modifer when Tyrande _30% Health_.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight. |confirm |next "Tyrande" |or
.' or
|confirm |next "next_1" |or
step
label	"Tyrande"
goto End Time/5 51.8,42.9
.from Echo of Tyrande##54544
.' As a tank you can either Spell Reflect or use a Spell Damage modifier for when _Tyrande_ casts _Moonbolt_, a direct damage spell she casts often.
.' _Tyrande_ throws arcane lances appear to be a streak of blue then splits into three and need to be dodged by ranged. This cannot be dodged by the Tank and will stun if it hits you.
.' When _Tyrande_ is at 30% health she will be surrounded by _Tears of Elune_, this looks like fireworks and damages anyone within 4 yards of it.
.kill 2 Echos in the End Time instance |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Tyrande" |or
|confirm |or
step
label	"next_1"
.' Click Time Transit Device and choose one of the destinations:
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Azure"
'Clearing the trash mobs in this room is not very difficult. You want to start killing the _Priest_, then the _Riflemen_. Make sure to have the Dps kill the Fountain of Light that are summoned also.
.' If you can, _Control the Sorceress_ by using _Shackle_ or _Turn Undead_. If you cannot, kill Sorceress 2nd.
.' Below is the
.from Time-Twisted Priest##54690+
.from Time-Twisted Sorceress##54691+
.from Time-Twisted Rifleman##54693+
.from Time-Twisted Footman##54687+
|modelnpc Fountain of Light##54795+
'To summon the Echo of Jaina, you will need to go around and _click 16 Fragments of Jaina's Staff_. Once you have clicked all 16, she will appear in the middle of the map, in the _blue swirling circle_.
|confirm
step
label	"pre_Jaina"
.from Echo of Jaina##54445
.' Absorb or Reflect _Pyroblast_ when possible.
.' Immediately touch _Flarecore_.
.' Dodge _Frost Blades_.
.kill 2 Echos in the End Time instance |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight. |confirm  |next "Jaina" |or
.' or
|confirm |next "next_2" |or
step
label	"Jaina"
.from Echo of Jaina##54445
.' _Jaina_ will _Pyroblast_ the tank throughout the whole fight, this is just a fireball that causes medium damage. This cannot be silenced but can be reflected.
.' _Jaina_ randomly creates a bomb called _Flarecore_, in a random area that will explode and hit all party members if not touched before 10 seconds. Running into this will cause less damage, the faster someone gets to it.
.' _Jaina_ will blink at random times and throw 3 _Frost Blades_ that can only be dodged, not jumped over.
.' Rinse and repeat the fight.
.' Kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Jaina" |or
|confirm |or
step
label	"next_2"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Ruby"
.from Time-Twisted Geist##54511+
.' Target the _Geists_ and kill the group with AoE, kill all _Geists_ first, they cause the most damage.
.' You will take massive damage after a few seconds from Bleed effects and Disease. Save any cooldowns for about 10 seconds.
'|modelnpc 54507
|confirm
step
label	"pre_Sylvanas"
goto End Time/3 55.8,39.0
.from Echo of Sylvanas##54123
.' Spread out 10 yards apart.
.' Avoid _Blighted Ground_.
.' Kill _Risen Ghoul_.
.kill 2 Echos in the End Time instance |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight |confirm  |next "Sylvanas" |or
.' or
|confirm |next "next_3" |or
step
label	"Sylvanas"
goto End Time/3,55.8,39.0
.from Echo of Sylvanas##54123
.' _Sylvanas_ has two parts to her fight.
.' _PART ONE_ Sylvanas will blight the ground, which looks like purple fog, don't stand in this as it will deal 35k damage per second and slow movement by 50%. _Sylvanas_ will jump up and shoot any player in this area for 50k damage and hit anyone within 10 yards of them.
.' _PART TWO_ (this can happen multiple times) _Sylvanas_ will pull everyone toward her as she hovers in the air. You will see circles appear in a wide radius on the ground which will spawn _Risen Ghouls_.
.' All party members must target the same _Risen Ghoul_ and kill it before it reaches Sylvanas, or this will deal AoE damage and wipe party. Once you kill the ghoul, run through the opening in the shadows and wait until Sylvanas is on the ground again.
.' Rinse and reapeat fight.
.' Kill 2 Echos in the End Time instance |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Sylvanas" |or
.' or
|confirm  |or
step
label	"next_3"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Obsidian"
'If you can, try and Control the _Twisted Breaker_, this mob can hit very hard.
.' If any of these mobs step into the lava, they will get a buff that will do extra fire damage. Make sure to pull a group away from the lava as you fight them.
.' Below is the _kill order_ for these mobs.
.from Time-Twisted Drake##54543+
.from Time-Twisted Seer##54553+
.from Time-Twisted Breaker##54552+
'The _Dragonkin_ can either be _polymorphed_ by a mage _or sapped_ by a rogue for good control, however depending on your gear, you can simply tank through these fights.
|confirm
step
label	"pre_Baine"
.from Echo of Baine##54431
.' Run to other rocks as they explode.
.' Use damage reduction ability during _Motlen Axe_.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight. |confirm  |next "Baine" |or
.' or
|confirm |next "next_4" |or
step
label	"Baine"
.from Echo of Baine##54431
.' You will need to start tanking _Baine_ on the right or left rocks floating in lava that you see.
.' _Baine_ has a few abilities to watch out for. When he is in the Lava, he will gain a 10 second buff _Molten Axe_ that will do additional 10k damage per hit, make sure to use cooldowns at this time as a tank.
.' If you step in the Lava briefly, you will gain a debuff that makes you take additional fire damage, and a 20 second buff that will give you additional 10k damage per melee hit.
.' Baine will also periodically break the rock most of the group is standing on, so make sure to use each platform in a circular path and leave the last rock next to the mainland to retreat to if you cannot kill him.
.kill 2 Echos in the End Time instance |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Baine" |or
.' or
|confirm |or
step
label	"next_4"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	 "pre_Muronzond"
.from Muronzond##54432
.' Face boss away from group.
.' Use damage reduction ability during _Infinite Breath_.
.' Rotate Boss away from _Distortion Bombs_.
.' Click _Hourglass_ to reset fight.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight. |confirm  |next "Muronzond" |or
.' or
|confirm |next "next_5" |or
step
label	"Muronzond"
.' _Muronzond_
.' As a tank you will need to face Boss aways from group. _Muronzond_ has _Tail Swipe_ so group cannot be directly behind boss.
.' Tank and Melee cannot avoid _Temporal Blast_, a small AoE that deals 25k damage, this will need to be healed through.
.' Tank will be hit with _Infinite Breath_. This is shadow damage that is dealt in a cone in front of _Muronzond_ and will follow tank if he moves left or right. This will need to be healed through.
.' When tanking, you will have to pay attention to _Distortion Bombs_, which will be cast at ranged players and leave a circle or arcane on the ground. Move the boss slightly in a circle to allow ranged to avoid standing directly behind the boss in order to avoid these.
.' You will see five hourglass symbols on your screen. By clicking the large hourglass in the middle of the room, you will use 1 hourglass, this resets everyone's revives all health and clears the _Distortion Bombs_ on the ground.
.kill Murozond##54432 |q 30096/1 |only if havequest(30096)
.from Muronzond##54432 |only if not havequest(30096)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Muronzond" |or
.' or
|confirm |or
step
label	"next_5"
.' After you kill Murozond wait for Alurmi
.talk 57864
..turnin 30097
step
goto 52.1,44.5
.talk 54751
..turnin 30096
step
goto 52.1,44.5
.talk 54751
..accept 30098
step
goto 52.1,44.5
.talk 54751
.' Tell him
.' <Take me to the Well of Eternity> |goto Well of Eternity |noway |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\End Time (ET)\\End Time (ET) Damage Guide",[[
dungeon 820
description This Guides will walk you through tanking the End of Time dungeon and questing.
author support@zygorguides.com
step
.' Press _I_ on your keyboard to open your Dungeon Finder and queue for End of Time specfic Cataclysm dungeon.
.' This guide is for a Damage role, so choose _Damage_ for your choice.
|confirm
step
goto End Time,79.6,45.1
.talk 57864
..accept 30097
step
goto 79.6,45.0
.talk 54476
..accept 30096
step
goto 79.7,45.4
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Emerald"
'For this section, the tank should start by attacking the _Nightsabers_ and _Sentinel's_
'When the Moonlight rays randonly appear on the map you need to run over to them.
.'AoE the rounded up mobs if you can, otherwise just kill them while they are standing in the beam of light.
.'_Repeat until Tyrande_ show up.
.from Time-Twisted Nightsaber##54688+
.from Time-Twisted Sentinel##54512+
|confirm
step
.from Echo of Tyrande##54544
.' Tyrande will cast _Eyes of the Goddess_ which move around the outside of the pool causing damage and silencing players.
.' It is best to remain in the pool to avoid these, but the pool will reduce your casting speed so use any speed increasing abilities you have to counteract this.
.' Tyrande will will send out waves of _Moonlance_ which you will need to avoid.
.' When _Tyrande_ is at 30% health she will be surrounded by _Tears of Elune_, this looks like fireworks and damages anyone within 4 yards of it.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
|confirm
step
.' Click Time Transit Device and choose one of the destinations:
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Azure"
'Your party will need to make their way around the map clearing trash and collecting fragments,
.'The kill order should be _Priest_ first, then the _Riflemen_. As a DPS you will need to kill the Fountain of Light that appear also.
.' Below is the mobs you will encounter:
.from Time-Twisted Priest##54690+
.from Time-Twisted Sorceress##54691+
.from Time-Twisted Rifleman##54693+
.from Time-Twisted Footman##54687+
|modelnpc Fountain of Light##54795+
'To summon the Echo of Jaina, you will need to go around and _click 16 Fragments of Jaina's Staff_. Once you have clicked all 16, she will appear in the middle of the map, in the _blue swirling circle_.
|confirm
step
.from Echo of Jaina##54445
.' _Jaina_ periodically creates a bomb called _Flarecore_, in a random area that will explode and hit all party members if not touched before 10 seconds. Running into this will cause less damage, the faster someone gets to it.
.' _Jaina_ will blink at random times and throw 3 _Frost Blades_ that can only be dodged, not jumped over.
.' Rinse and repeat the fight.
.' Kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
|confirm
step
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Ruby"
.from Time-Twisted Geist##54511+
.' Target the and kill all _Geists_ first, they cause the most damage.
'|modelnpc 54507
|confirm
step
.from Echo of Sylvanas##54123
.' _Sylvanas_ has two parts to her fight.
.' _PART ONE_ Sylvanas will blight the ground, which looks like purple fog, don't stand in this as it will deal 35k damage per second and slow movement by 50%. _Sylvanas_ will jump up and shoot any player in this area for 50k damage.
.' _PART TWO_ (this can happen multiple times) _Sylvanas_ will say "watch as Death surrounds you" and pull everyone toward her as she hovers in the air. You will see circles appear in a wide radius on the ground which will spawn _Risen Ghouls_.
.' All party members must target the same _Risen Ghoul_ and kill it before it reaches Sylvanas, or this will deal AoE damage and wipe the party. Once you kill the ghoul, run through the opening in the shadows and wait until Sylvanas is on the ground again.
.' Rinse and reapeat fight.
.' Kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
|confirm
step
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Obsidian"
'Clear the dragons as a group.
.' If any of these mobs step into the lava, they will get a buff that will do extra fire damage. The tank should pull a group away from the lava as you fight them.
.' Below is the _kill order_ for these mobs.
.from Time-Twisted Drake##54543+
.from Time-Twisted Seer##54553+
.from Time-Twisted Breaker##54552+
'The _Dragonkin_ can either be _polymorphed_ by a mage _or sapped_ by a rogue for extra crowd control if needed.
|confirm
step
.from Echo of Baine##54431
.' The tank should start attacking _Baine_ on the right or left rocks floating in lava that you see.
.' Attack _Baine_ and keep him on the same platform he is standing on.
.' Baine will also periodically break the rock most of the group is standing on, so make sure to use each platform in a circular path and leave the last rock next to the mainland to retreat to if you cannot kill him.
.' Falling into the Lava will give you a debuff that makes you take additional fire damage, and a 20 second buff that will give you additional 10k damage per melee hit. This is unavoidable when moving platforms but try to stay out as much as possible.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
|confirm
step
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Bronze"
.' Muronzond has a frontal attack called _Infinite Breath_ that you will need to avoid. The tank should keep him turned in a way that prevents this from happening.
.' When tanking, you will have to pay attention to _Distortion Bombs_, which will gradually fill the area with temporal rifts you will want to stay away from.
.' You will see five hourglass symbols on your screen. By clicking the large hourglass in the middle of the room, you will use 1 hourglass, which will revive everyones health and clear the _Distortion Bombs_ on the ground.
.from Murozond##54432 |only if not havequest(30096)
|confirm |only if not havequest(30096)
.kill 2 Echos in the End Time instance |q 30097/1 |only if havequest(30097)
.kill Murozond##54432 |q 30096/1 |only if havequest(30096)
step
.' After you kill Murozond wait for Alurmi
.talk 57864
..turnin 30097
step
goto 52.1,44.5
.talk 54751
..turnin 30096
step
goto 52.1,44.5
.talk 54751
..accept 30098
step
goto 52.1,44.5
.talk 54751
.' Tell him
.' <Take me to the Well of Eternity> |goto Well of Eternity |noway |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\End Time (ET)\\End Time (ET) Healer Guide",[[
description This Guides will walk you through tanking the End of Time dungeon and questing.
author support@zygorguides.com
step
.' Press _I_ on your keyboard to open your Dungeon Finder and queue for End of Time specfic Cataclysm dungeon.
.' This guide is for a Healer role, so choose _Healer_ for your choice.
|confirm
step
goto End Time,79.6,45.1
.talk 57864
..accept 30097
step
goto 79.6,45.0
.talk 54476
..accept 30096
step
label	"transit"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Emerald"
'Follow your tank as they pull mobs.
.' _Run to the Moonlight_ and stay in anytime it appears, this helps with killing the mobs. When one dissappears, run to the next one that spawns.
.' _Repeat until Tyrande_ shows up.
|confirm
step
label	"pre_Tyrande"
goto End Time/5,51.8,42.9
.from Echo of Tyrande##54544
.' Dodge _Moonlances_
.' Avoid _Eyes of the Goddess_.
.' At 30% health, she uses _Tears of Elune_. Extra heals may be needed.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight |confirm |next "Tyrande" |or
.' or
|confirm |next "next_1" |or
step
label	"Tyrande"
goto End Time/5 51.8,42.9
.from Echo of Tyrande##54544
.' As a Healer you may need to use AoE heals. _Moonlance_ can be dispelled if a party member is hit by it and appear to be a streak of blue that splits into three and need to be dodged by ranged.
.' _Piercing Gaze of Elune_ can also be dispelled.
.' When _Tyrande_ is at 30% health she will be surrounded by _Tears of Elune_, this looks like fireworks and damages anyone within 4 yards of it.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm  |next "pre_Tyrande" |or
|confirm |or
step
label	"next_1"
.' Click Time Transit Device and choose one of the destinations:
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Azure"
'The mobs in this room don't hit very hard, though the fights can be lengthy depending on your groups damage.
'To summon the Echo of Jaina, you will need to go around and _click 16 Fragments of Jaina's Staff_. Once you have clicked all 16, she will appear in the middle of the map, in the _blue swirling circle_.
|confirm
step
label	"pre_Jaina"
.from Echo of Jaina##54445
.' Immediately touch _Flarecore_.
.' Dodge _Frost Blades_	.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight |confirm  |next "Jaina" |or
.' or
|confirm |next "next_2" |or
step
label	"Jaina"
.from Echo of Jaina##54445
.' _Jaina_ randomly creates a bomb called _Flarecore_, in a random area that will explode and hit all party members if not touched before 10 seconds. Running into this will cause less damage, the faster someone gets to it.
.' _Jaina_ will blink at random times and throw 3 _Frost Blades_ that can only be dodged, not jumped over.
.' Use _Resistance Auras_ if you have them.
.' Kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Jaina" |or
|confirm |or
step
label	"next_2"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Ruby"
.from Time-Twisted Geist##54511+
.' _Dispel diseases_ from group members.
'|modelnpc 54507
|confirm
step
label	"pre_Sylvanas"
goto End Time/3,55.8,39.0
.from Echo of Sylvanas##54123
.' Spread out 10 yards apart.
.' Avoid _Blighted Ground_.
.' Dispel _Shriek of the Highborne_ if you can dispel magic.
.' _Unholy Shot_ deals out moderate damage, heal it.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight |confirm  |next "Sylvanas" |or
.' or
|confirm |next "next_3" |or
step
label	"Sylvanas"
goto End Time/3,55.8,39.0
.from Echo of Sylvanas##54123
.' _Sylvanas_ has two parts to her fight.
.' _PART ONE_ Sylvanas will blight the ground, which looks like purple fog, don't stand in this as it will deal 35k damage per second and slow movement by 50%. _Sylvanas_ will jump up and shoot any player in this area for 50k damage and hit anyone within 10 yards of them.
.' _PART TWO_ (this can happen multiple times) _Sylvanas_ will pull everyone toward her as she hovers in the air. You will see circles appear in a wide radius on the ground which will spawn _Risen Ghouls_.
.' All party members must target the same _Risen Ghoul_ and kill it before it reaches Sylvanas, or this will deal AoE damage and wipe party. Once you kill the ghoul, run through the opening in the shadows and wait until Sylvanas is on the ground again.
.' You can use the time to recooperate your mana if need be.
.' Kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Sylvanas" |or
.' or
|confirm |or
step
label	"next_3"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Obsidian Dragonshrine> |goto End Time/4 29.2,16.0 <5 |noway |c |next "Obsidian" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	"Obsidian"
'If you can, try and Control the _Twisted Breaker_, this mob can hit very hard.
.' If any of these mobs step into the lava, they will get a buff that will do extra fire damage. Make sure to pull a group away from the lava as you fight them.
.' Below is the _kill order_ for these mobs.
.from Time-Twisted Drake##54543+
.from Time-Twisted Seer##54553+
.from Time-Twisted Breaker##54552+
'The _Dragonkin_ can either be _polymorphed_ by a mage _or sapped_ by a rogue for good control, however depending on your gear, you can simply tank through these fights.
|confirm
step
label	"pre_Baine"
.from Echo of Baine##54431
.' Run to other rocks as they explode.
.' _Molten Axe_ increases Baines damage output.
.' Use _resistance aura's_ if you have them.
.kill 2 Echos in the End Time instance |q 30097/1 |only if havequest(30097)
.' Click here for more details of the boss fight |confirm  |next "Baine" |or
.' or
|confirm |next "next_4" |or
step
label	"Baine"
.from Echo of Baine##54431
.' _Baine_ will gain _Molten Axe_ when dragged through lava. Extra heals will be needed.
.' Baine will also periodically break the rock most of the group is standing on, so make sure to use each platform in a circular path and leave the last rock next to the mainland to retreat to if you cannot kill him.
.kill 2 Echos in the End Time instance. |q 30097/1 |only if havequest(30097)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Baine" |or
.' or
|confirm |or
step
label	"next_4"
.' Click Time Transit Device and choose one of the destinations:
.' <Emerald Dragonshrine> |goto End Time/5 66.5,79.5 <5 |noway |c |next "Emerald" |or
.' or
.' <Azure Dragonshrine> |goto End Time/2 |noway |c |next "Azure" |or
.' or
.' <Ruby Dragonshrine> |goto End Time/3 34.3,43.2 <5 |noway |c |next "Ruby" |or
.' or
.' <Bronze Dragonshrine> |goto End Time/6 36.5,77.5 <5 |noway |c |next "Bronze" |or
step
label	 "pre_Muronzond"
.from Muronzond##54432
.' Don't be in front of behind the boss.
.' Avoid _Distortion Bombs_.
.' Click _Hourglass_ to reset fight.
.' Click here for more details of the boss fight |confirm |next "Muronzond" |or
.' or
|confirm |next "next_5" |or
step
label	"Muronzond"
.' _Muronzond_
.' _Muronzond_ has _Tail Swipe_ so group cannot be directly behind boss.
.' Tank and Melee cannot avoid _Temporal Blast_, a small AoE that deals 25k damage, this will need to be healed through.
.' Tank will be hit with _Infinite Breath_. This is shadow damage that is dealt in a cone in front of _Muronzond_ and will follow tank if he moves left or right. This will need to be healed through.
.' _Distortion Bombs_ will be cast at ranged players and leave a circle or arcane on the ground. Move the boss slightly in a circle to allow ranged to avoid standing directly behind the boss in order to avoid these.
.' You will see five hourglass symbols on your screen. By clicking the large hourglass in the middle of the room, you will use 1 hourglass, this resets everyone's revives all health and clears the _Distortion Bombs_ on the ground.
.kill Murozond##54432 |q 30096/1 |only if havequest(30096)
.from Muronzond##54432 |only if not havequest(30096)
.' Click here to go back to the short version of the boss fight. |confirm |next "pre_Muronzond" |or
.' or
|confirm |only if not havequest (30096) |or
step
label	"next_5"
.' After you kill Murozond wait for Alurmi
.talk 57864
..turnin 30097
step
goto 52.1,44.5
.talk 54751
..turnin 30096
..accept 30098
step
goto 52.1,44.5
.talk 54751
.' Tell him
.' <Take me to the Well of Eternity> |goto Well of Eternity |noway |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Well of Eternity (WoE)\\Hour of Twilight (HoT) Tank Guide",[[
step
goto Well of Eternity,27.3,64.2
.talk 55624
..turnin 30098
step
goto Well of Eternity,27.3,64.2
.talk 55624
..accept 30099
step
goto 26.0,63.0
.talk 57864
..accept 30104
step
goto Well of Eternity 26.2,65.6
.from Legion Demon##55503
.' He will use 2 abilities. He will leap to a random party causing damage to anyone nearby.
.' He also uses an ability that will cause 25k Shadow damage to the group which applies a damage debuff by 10%, this can stack multiple times.
.' Archive a Legion Demon |q 30104/1 |only if havequest(30104)
|confirm |only if not havequest(30104)
step
goto Well of Eternity 19.6,68.8
.talk 55500
.' <I am ready to be hidden by your shadowcloak>.
|confirm
step
goto 27.1,56.7
'Wait for Illidan to throw down smoke bombs and then run through the gap in the line of demons.
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable the First portal. |q 30099/1 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 18.7,38.4
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable Second portal. |q 30099/2 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 15.9,35.1
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable Third portal |q 30099/3 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
label	"pre_Peroth"
goto 19.5,52.0
.' Tanks take 20% increased damage during this fight.
.' At _60%_ Peroth'arn will vanish, _avoid_ the _Eyes of Peroth'arn_ at all costs.
.kill Peroth'arn##55085 |q 30099/4 |only if havequest(30099)
.from Peroth'arn##55085 |only if not havequest(30099)
.' Click here for a more detailed boss fight. |confirm |next "Peroth" |or
.' or
|confirm |next "part2" |or
step
label	"Peroth"
goto 19.5,52.0
.' Tanks will receive a debuff that increases their damage taken by 20%, this is unavoidable.
.' _Fel Decay_ will target a random player, then leave an AoE circle on the ground. Stay out of this.
.' At _60% health_, Peroth'arn will _stun the group_, and deal shadow damage for 4 seconds. He will vanish after this and the group can recover before the eyes spawn.
.' While he is stealthed, _avoid detection_ by the _Eyes of Peroth'arn_. If any party member is detected, Peroth'arn will ambush that person and deal massive damage until the tank can taunt.
.kill Peroth'arn##55085 |q 30099/4 |only if havequest(30099)
.from Peroth'arn##55085 |only if not havequest(30099)
.' Click here for a less detailed boss fight. |confirm |next "pre_Peroth" |or
.' or
|confirm |or
step
label	"part2"
goto 29.4,40.0
.talk 55624
..turnin 30099
step
goto 29.4,40.0
.talk 55624
..accept 30100
step
goto 33.6,35.1
.' Walking along this path you will encounter groups of 3.
.' Kill spellcasters first as they do damage to the whole group.
.from Enchanted Highmistress##56579
.from Enchanted Magus##54882
.from Eternal Champion##54612
|confirm
step
goto 31.5,29.4
.from Eye of the Legion##54747
.' Tank these facing away from your group, they deal massive channeled fire damage called _Fel Flames_ in a frontal cone. This can be interrupted.
|confirm
step
goto 43.7,27.3
.' The handmaidens are very easy to kill. They throw bottles of nature damage and can put members to sleep, however if you kill them fast enough, you won't have any trouble.
.from Royal Handmaiden##54645+
.' Archive Royal Handmaiden |q 30104/2
|only if havequest(30104)
step
label	"pre_Queen"
goto 47.2,36.9
.' Kill all the commanded _Magus'_.
.' Interrupt Queen Azhara's _Total Obedience_.
.from Queen Azshara##54853
.' Click here for a more detailed boss fight. |confirm |next "Queen" |or
.' or
|confirm |next "part3" |or
step
label	"Queen"
goto 47.2,36.9
.' Tank can start the fight by _hitting or taunting Queen Azshara_: |tip *NOTE* You will not attack the Queen, killing all guards will end the fight.
.' The _Queen_ will _summon 2 Magus'_ to life to attack the group. These will be summoned about _every 30 seconds_. Pick these adds up.
.' The Queen will often use the spell _Total Obedience_ and charm your entire party. _Interrupt_ this or it will kill your group. It is best to have a _ranged DPS, or a healer_ that can _interrupt_ this at all times.
.' There are 3 types of Magus' that will be summoned in pairs.Each of these use basic spells and keeping aggro will allow your group to survive.
.from Queen Azshara##54853
.' Defeat Queen Azshara. |q 30100/1 |only if havequest(30100)
.' Click here for a less detailed boss fight. |confirm |next "pre_Queen" |or
.' or
|confirm
step
label	"part3"
goto 43.9,32.9
.' Click the Time Transit Device.
.' Teleport to The Well of Eternity. |goto 73.1,84.0 <5 |noway |c
step
goto 71.7,80.6
.talk 55624
..accept 30101
step
goto Well of Eternity 73.2,77.7
.' Wait for _Tyrande_ to cast Light of Elune. Once you see this light, aggro a demon and they will all come to fight. Stay in the light to help kill them all fast.
.from Doomguard Annihilator##55519+
|confirm
step
goto 83.8,61.2
.' When fighting the Doombringer, he will constantly cast Abyssal Flames which damages the whole group for 40k fire damage. Kill him fast to keep the group from dying.
.from Abyssal Doombringer##55510
|confirm
step
label	"pre_Mannoroth"
goto 84.7,55.0
.from Captain Varo'then##55419
.' Avoid _Fel Firestorm_
.' Keep threat on Demons from open Portal.
.' Once in Demon form, kill Mannoroth.
.' Archive Captain Varo'then. |q 30104/3 |only if havequest(30104)
.kill Mannoroth##54969 |q 30101/1 |only if havequest(30101)
.from Mannoroth##54969 |only if not havequest(30101)
.' Click here for a more detailed boss fight. |confirm |next "Mannoroth" |or
.' or
|confirm |next "part4" |only if not havequest(30101) or not havequest(30104)|or
step
label	"Mannoroth"
goto 84.7,55.0
.' _PART ONE_
.from Captain Varo'then##55419
.' When coming up to fight Mannoroth, you will first face Captain Varo'then.
.' Captain Varo'then will attack with _Magistrike_ dealing _30k fire damage_ and jumps to any player nearby.
.' _Fel Firestorm_ will fall from the sky often. This is a rapid _Fel Flame_ that flows _along the ground_. Stay out of it.
.' _PART TWO_
.' After Varo'then is dead, click the sword that falls and start attacking Mannoroth, you will not have to hold threat. After a short time, a portal will open releasing lots of Doomguards and Demons.
.' Stand near the portal and aoe any demon that comes out. _Tyrande Whisperwind_ will cast _Light of Elune_, bringing all demons in this light to kill much faster.
.' Continue picking up demons and killing them until Mannoroth is weak. When he gets to 40%, your entire party will be turned into demons, at this point the fight is over. Fight until Mannoroth retreats.
.' Archive Captain Varo'then. |q 30104/3 |only if havequest(30104)
.kill Mannoroth##54969 |q 30101/1 |only if havequest(30101)
.' Click here for a less detailed boss fight. |confirm always |next "pre_Mannoroth"
.' or
|confirm
step
label	"part4"
goto 87.8,49.9
.talk 57864
..turnin 30104
step
goto 84.6,55.9
.talk 55532
..turnin 30101
step
goto 87.3,49.3
.talk 57913
..accept 30102
step
goto 87.3,49.3
.talk 57913
.' <I am ready to help Thrall, back in my present time.>  |goto Hour of Twilight |noway |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Well of Eternity (WoE)\\Hour of Twilight (HoT) Damage Guide",[[
dungeon 816
step
goto Well of Eternity,27.3,64.2
.talk 55624
..turnin 30098
step
goto Well of Eternity,27.3,64.2
.talk 55624
..accept 30099
step
goto 26.0,63.0
.talk 57864
..accept 30104
step
goto Well of Eternity,26.2,65.6
.from Legion Demon##55503
.' He will use 2 abilities. He will leap to a random party causing damage to anyone nearby.
.' He also uses an ability that will cause 25k Shadow damage to the group which applies a damage debuff by 10%, this can stack multiple times.
.' Archive a Legion Demon |q 30104/1 |only if havequest(30104)
|confirm |only if not havequest(30104)
step
goto Well of Eternity 19.6,68.8
.talk 55500
.' <I am ready to be hidden by your shadowcloak>.
|confirm
step
goto 27.1,56.7
'Wait for Illidan to throw down smoke bombs and then run through the gap in the line of demons.
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable the First portal. |q 30099/1 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 18.7,38.4
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable Second portal. |q 30099/2 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 15.9,35.1
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable Third portal |q 30099/3 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 19.5,52.0
.' _Fel Decay_ will target a random player, then leave an AoE circle on the ground. Stay out of this.
.' At _60% health_, Peroth'arn will _stun the group_, and deal shadow damage for 4 seconds. He will vanish after this, giving you time to recover.
.' While he is stealthed, _avoid detection_ by the _Eyes of Peroth'arn_. If any party member is detected, Peroth'arn will ambush that person and deal massive damage until the tank can taunt.
.' He will return if the group remains undetected by the _Eyes of Peroth'arn_ for 40 seconds.
.kill Peroth'arn##55085 |q 30099/4 |only if havequest(30099)
.from Peroth'arn##55085 |only if not havequest(30099)
|confirm |only if not havequest(30099)
step
goto 29.4,40.0
.talk 55624
..turnin 30099
step
goto 29.4,40.0
.talk 55624
..accept 30100
step
goto 33.6,35.1
.' Walking along this path you will encounter groups of 3.
.' Kill spellcasters first as they do damage to the whole group.
.from Enchanted Highmistress##56579
.from Enchanted Magus##54882
.from Eternal Champion##54612
|confirm
step
goto 31.5,29.4
.from Eye of the Legion##54747
.' Tank these facing away from your group, they deal massive channeled fire damage called _Fel Flames_ in a frontal cone. This can be interrupted.
|confirm
step
goto 43.7,27.3
.' The handmaidens are very easy to kill. They throw bottles of nature damage and can put members to sleep, however if you kill them fast enough, you won't have any trouble.
.from Royal Handmaiden##54645+
.' Archive Royal Handmaiden |q 30104/2
|only if havequest(30104)
step
goto 47.2,36.9
.' *NOTE* You will not attack the Queen, killing all guards will end the fight.
.' The _Queen_ will _summon 2 Magus'_ to life to attack the group. These will be summoned about _every 30 seconds_.
.' The Frost Magus unleashes a very powerful attack you will want to dodge called _Coldflame_. This looks like a blue line of energy on the ground. You will know it's coming by the spinning blue circles that appear before it hits.
.' The Queen will often use the spell _Total Obedience_ and charm your entire party. _Interrupt_ this or it will kill your group. It is best to have a _ranged DPS, or a healer_ that can _interrupt_ this at all times.
.' She will also cast _Servant of the Queen_ which mind controls a player. Kill the puppet strings above that character to release them.
.from Queen Azshara##54853
.' Defeat Queen Azshara |q 30100/1 |only if havequest(30100)
|confirm
step
goto 43.9,32.9
.' Click the Time Transit Device
.' Teleport to The Well of Eternity. |goto 73.1,84.0 <5 |noway |c
step
goto 71.7,80.6
.talk 55624
..accept 30101
step
goto Well of Eternity,73.2,77.7
.' Wait for _Tyrande_ to cast Light of Elune. Once you see this light, aggro a demon and they will all come to fight. Stay in the light to help kill them all fast.
.from Doomguard Annihilator##55519+
|confirm always
step
goto 83.8,61.2
.' When fighting the Doombringer, he will constantly cast Abyssal Flames which damages the whole group for 40k fire damage. Kill him fast to keep the group from dying.
.from Abyssal Doombringer##55510
|confirm
step
goto 84.7,55.0
.' _PART ONE_
.from Captain Varo'then##55419
.' When coming up to fight Mannoroth, you will first face Captain Varo'then.
.' Captain Varo'then will attack with _Magistrike_ dealing _30k fire damage_. This jumps to any player nearby.
.' _Fel Firestorm_ will fall from the sky often. This is a rapid _Fel Flame_ that flows _along the ground_. Stay out of it.
.' _PART TWO_
.' After Varo'then is dead, click the sword that falls and start attacking Mannoroth. After a short time, a portal will open releasing lots of Doomguards and Demons.
.' Stand near the portal and aoe any demon that comes out until Tyrande Whisperwind is free.
.' Attack Mannoroth and avoid the fire that rains from the sky. If you attract any adds run to your tank so that he may pick them up.
.' Attack Mannoroth until he gets to 40%. At this point, your entire party will be turned into demons and the fight is over. Fight until Mannoroth retreats.
.' Archive Captain Varo'then. |q 30104/3 |only if havequest(30104)
.kill Mannoroth##54969 |q 30101/1 |only if havequest(30101)
|confirm |only if not havequest(30101)
step
goto 87.8,49.9
.talk 57864
..turnin 30104
step
goto 84.6,55.9
.talk 55532
..turnin 30101
step
goto 87.3,49.3
.talk 57913
..accept 30102
step
goto 87.3,49.3
.talk 57913
.' <I am ready to help Thrall, back in my present time.>  |goto Hour of Twilight |noway |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Well of Eternity (WoE)\\Hour of Twilight (HoT) Healer Guide",[[
description This guide will walk you through the Well of Eternity
description instance with quests involved from a Healers perspective.
step
goto Well of Eternity,27.3,64.2
.talk 55624
..turnin 30098
step
goto Well of Eternity,27.3,64.2
.talk 55624
..accept 30099
step
goto 26.0,63.0
.talk 57864
..accept 30104
step
goto Well of Eternity,26.2,65.6
.from Legion Demon##55503
.' He will use 2 abilities. He will leap to a random party causing damage to anyone nearby
.' He also uses an ability that will cause 25k Shadow damage to the group which applies a damage debuff by 10%, this can stack multiple times.
.' Archive a Legion Demon |q 30104/1 |only if havequest(30104)
|confirm |only if not havequest(30104)
step
goto Well of Eternity 19.6,68.8
.talk 55500
.' <I am ready to be hidden by your shadowcloak>.
|confirm
step
goto 27.1,56.7
'Wait for Illidan to throw down smoke bombs and then run through the gap in the line of demons.
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable the First portal. |q 30099/1 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 18.7,38.4
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable Second portal |q 30099/2 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 15.9,35.1
.' Take out the Dreadlords first and try to interrupt their _Carrion Swarm_, this causes big shadow damage to anyone in front of them.
.from Dreadlord Defender##55656+, Corrupted Arcanist##55654
.click Portal Energy Focus##6905
.' Disable Third portal |q 30099/3 |only if havequest(30099)
|confirm |only if not havequest(30099)
step
goto 19.5,52.0
.' Tanks take 20% increased damage during this fight.
.' For this fight, be careful of healing those with the Fel Decay debuff. You will take equal damage that your heals do to that target.
.' He randomly places a DoT called _Fel Flames_ on a party member, so be ready to heal through it.
.' When he hits 60%, he will deal massive AoE damage, so be sure to heal up the party before hand.
.' He will return if the group remains undetected by the _Eyes of Peroth'arn_ for 40 seconds. If any party member is detected, Peroth'arn will ambush that person and deal massive damage until the tank can taunt.
.kill Peroth'arn##55085 |q 30099/4 |only if havequest(30099)
.from Peroth'arn##55085 |only if not havequest(30099)
|confirm |only if not havequest(30099)
step
goto 29.4,40.0
.talk 55624
..turnin 30099
step
goto 29.4,40.0
.talk 55624
..accept 30100
step
goto 33.6,35.1
.' Walking along this path you will encounter groups of 3.
.' Kill spellcasters first as they do damage to the whole group.
.from Enchanted Highmistress##56579
.from Enchanted Magus##54882
.from Eternal Champion##54612
|confirm
step
goto 31.5,29.4
.from Eye of the Legion##54747
.' Tank these facing away from your group, they deal massive channeled fire damage called _Fel Flames_ in a frontal cone. This can be interrupted.
|confirm
step
goto 43.7,27.3
.' The handmaidens are very easy to kill. They throw bottles of nature damage and can put members to sleep, however if you kill them fast enough, you won't have any trouble.
.from Royal Handmaiden##54645+
.' Archive Royal Handmaiden |q 30104/2
|only if havequest(30104)
step
goto 47.2,36.9
.' The adds Queen Azshara summons will place random AoE spells on the ground, avoid them if at all possible.
.' If you have any form of interrupt, use it to interrupt _Total Obedience_, which Queen Azshara casts herself. It will be a wipe if you don't.
.from Queen Azshara##54853
.' Defeat Queen Azshara. |q 30100/1 |only if havequest(30100)
|confirm |only if not havequest(30100)
step
goto 43.9,32.9
.' Click the Time Transit Device.
.' Teleport to The Well of Eternity. |goto 73.1,84.0 <5 |noway |c
step
goto 71.7,80.6
.talk 55624
..accept 30101
step
goto Well of Eternity 73.2,77.7
.' Wait for _Tyrande_ to cast Light of Elune. Once you see this light, aggro a demon and they will all come to fight. Stay in the light to help kill them all fast.
.from Doomguard Annihilator##55519+
|confirm
step
goto 83.8,61.2
.' When fighting the Doombringer, he will constantly cast Abyssal Flames which damages the whole group for 40k fire damage. Kill him fast to keep the group from dying.
.from Abyssal Doombringer##55510
|confirm
step
goto 84.7,55.0
.' _PART ONE_
.from Captain Varo'then##55419
.' When coming up to fight Mannoroth, you will first face Captain Varo'then.
.' Captain Varo'then will attack with _Magistrike_ dealing _30k fire damage_. This jumps to any player nearby.
.' _Fel Firestorm_ will fall from the sky often. This is a rapid _Fel Flame_ that flows _along the ground_. Stay out of it.
.' _PART TWO_
.' After Varo'then is dead, click the sword that falls.
.' Your tank will be picking up a lot of adds so this is where the majority of your attention should be.
.' When Mannoroth reaches 40% your entire group will be turned into demons, at this point the fight is over.
.' Archive Captain Varo'then |q 30104/3 |only if havequest(30104)
.kill Mannoroth##54969 |q 30101/1 |only if havequest(30101)
.from Mannoroth##54969  |only if not havequest(30101)
|confirm |only if not havequest(30101) or not havequest(30104)
step
goto 87.8,49.9
.talk 57864
..turnin 30104
step
goto 84.6,55.9
.talk 55532
..turnin 30101
step
goto 87.3,49.3
.talk 57913
..accept 30102
step
goto 87.3,49.3
.talk 57913
.' Tell Chromie
.' <I am ready to help Thrall, back in my present time.>  |goto Hour of Twilight |noway |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Hour of Twilight (HoT)\\Hour of Twilight (HoT) Tank Guide",[[
step
goto Hour of Twilight,48.7,19.7
.talk 54548
..turnin 30102
..accept 30103
step
goto Hour of Twilight,48.7,19.7
.talk 54548
.' <Yes Thrall>
|confirm always
step
map Hour of Twilight
path follow strict;loop off;ants straight
path	50.9,20.3	52.3,21.6	53.9,23.1
.' Follow this path and kill the groups.
.' Talk to Thrall when you are ready to face the boss.
|confirm always
step
label	"pre_Arcurion"
goto 56.7,27.4
.from Arcurion##54590
.' Avoid _Icy Boulders_.
.' Interrupt or use damage reduction ability on _Hand of Frost_.
.' At 30% use damage modifiers as needed.
.' Click here for a more detailed boss fight. |confirm |next "Arcurion" |or
.' or
|confirm |next "part2"
step
label	"Arcurion"
goto 56.7,27.4
'Arcurion will cast _Hand of Frost_ dealing _50k damage_ to his target.
.' Arcurion will cast _chains of frost_, freezing all targets (65yds) and _dealing 10k damage_.
.' You will see blue _circles mark the ground_ for a few seconds, do not stand in this. _Frozen Servitors_ on top of cliffs are throwing _Icy Boulders_ and you will freeze if caught in it.
.' Keep _Thrall out of the Ice block_ by damaging it when he is frozen, he will keep the Frozen Servitors at a minimum for less circles.
.' At _30% health_, boss will do a _massive aoe_ in which all damage needs to be directed at boss for final blows.
.from Arcurion##54590
.' Click here for a less detailed boss fight. |confirm |next "pre_Arcurion" |or
.' or
|confirm
step
label "part2"
.talk 54548
.' <Lead the Way>
|confirm always
step
goto 43.7,45.3
.talk 54548
.' <Yes Thrall lets do this>
|confirm always
step
map Hour of Twilight
path follow loose;loop off;ants straight
path	45.5,47.1	45.6,49.0
path	43.0,50.0	40.4,48.7
.' _Follow Thrall_ through this canyon.
.' You will have to fight a _few groups_ through this area.
.from Twilight Assassin##55106+
.from Twilight Thug##55111+
.from Twilight Bruiser##55112+
.from Twilight Ranger##55107+
.from Twilight Shadow-Walker##55109+
'|modelnpc 55474
|confirm always
step
label	"pre_Asira"
goto Hour of Twilight,39.5,52.4
.from Asira Dawnslayer##54968
.' Avoid _Choking Smoke Bomb_.
.' Stan in _Thrall's Fire Totem_.
.' Click here for a more detailed boss fight. |confirm |next "Asira" |or
.' or
|confirm |next "part3"
step
label	"Asira"
goto Hour of Twilight,39.5,52.4
.' Asira will _mark_ a random _spellcaster_ and if this spellcaster casts a spell, boss will throw a knife, hitting anyone between the player and the boss. This will silence whoever it hits.
.' Asira will create _Choking Smoke Bomb_ which will not allow players to target into this cloud, while also inflicting _5k Nature damage_ every .5 seconds.
.' Make sure to stand near _Thall's Fire Totem_, this will _increase health and damage_ of party members nearby.
.from Asira Dawnslayer##54968
.' Click here for a less detailed boss fight |confirm |next "pre_Asira" |or
.' or
|confirm |next "part3"
step
label	"part3"
goto Hour of Twilight 39.5,52.4
.from Asira Dawnslayer##54968
.collect Urgent Twilight Missive##77957 |n
.' Click the Urgent Twilight Missive in your bags. |use Urgent Twilight Missive##77957
..accept 30105
step
goto 40.1,50.8
.clicknpc Life Warden##55549
|invehicle |c
step
.' You will land next to Thrall on the road to Wyrmrest Temple.
|outvehicle |c
step
map Hour of Twilight
path follow loose; loop off; straight
path	49.8,70.2	49.7,71.7	49.4,75.4
path	49.4,81.0
path	49.6,82.8
.' _Follow Thrall_ through this canyon.
.' Kill the _Shadow Borer's first_, they interrupt and stop healing.
.from Faceless Shadow Weaver##54633+
.from Faceless Brute##54632+
.from Shadow Borer##54686+
.' Enter Wyrmrest Temple here. |goto Hour of Twilight 49.6,82.8 <5|noway |c
step
label	"pre_Bishop"
goto Hour of Twilight/2 47.2,50.9
.from Archbishop Benedictus##54938
.' Spread out 10 yards.
.' Avoid _Wave of Virtue_ stand in blue shell.
.' Avoid _Wave of Twilight_ run to the side.
.' Unmask the Twilight Prophet. |q 30105/1 |only if havequest(30105)
.' Escort Thrall to Wyrmrest Temple. |q 30103/1 |only if havequest(30103)
.' Click here for a more detailed boss fight |confirm |next "Bishop" |or
.' or
|confirm |next "part4"
step
label	"Bishop"
goto Hour of Twilight/2 47.2,50.9
.' Archbishop has a lot of spells that will affect nearby players, _spread out_ before fight starts.
.' Thrall will help with the first part of the fight, he will put a blue shell on the ground that you can use for protection and a damage buff. Stand in his _Water Shell_ whenever you see it.
.' _Wave of Virtue_ and _Wave of Twilight_ are waves of Holy or Shadow that will sweep across the stage. These _knock_ anyone _back_ and cause _100k damage_. Run to the side to avoid or stand in the _Water Shell_ for the first part of fight.
.' Archbishop will target a random member and cast _Twilight Shear_ or _Purifying Blast_ dealing damage to the target and nearby friendly players.
.' There are _2 stages_ of this fight, he will switch from _holy to shadow_ at _60% health_, the only difference is that Thrall will be trapped and damage will be shadow instead of holy.
.from Archbishop Benedictus##54938
.' Unmask the Twilight Prophet. |q 30105/1 |only if havequest(30105)
.' Escort Thrall to Wyrmrest Temple. |q 30103/1 |only if havequest(30103)
.' Click here for a less detailed boss fight |confirm |next "pre_Bishop" |or
.' or |only if havequest(30103) or havequest(30105)
|confirm |only if havequest(30103) or havequest(30105)
step
label	"part4"
goto 46.9,45.0
.talk 54971
..turnin 30103
..turnin 30105
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Hour of Twilight (HoT)\\Hour of Twilight (HoT) DPS Guide",[[
dungeon 819
step
goto Hour of Twilight,48.7,19.7
.talk 54548
..turnin 30102
..accept 30103
step
goto Hour of Twilight,48.7,19.7
.talk 54548
.' <Yes Thrall>
|confirm always
step
map Hour of Twilight
path follow strict;loop off;ants straight
path	50.9,20.3	52.3,21.6	53.9,23.1
.' Follow this path and kill the groups.
.' Talk to Thrall when you are ready to face the boss.
|confirm always
step
goto 56.7,27.4
.' Arcurion will cast _Chains of Frost_, freezing all targets in place and _dealing 10k damage_.
.' You will see blue _circles mark the ground_ for a few seconds, do not stand in this. _Frozen Servitors_ on top of cliffs are _throwing boulders_ and you will freeze if caught in it.
.' Keep _Thrall out of the Ice block_ by damaging it when he is frozen, he will keep the Frozen Servitors at a minimum for less circles.
.' At _30% health_, Arcurion will do a _massive aoe_, burn him down.
.from Arcurion##54590
|confirm always
step
.talk 54548
.' <Lead the Way>
|confirm always
step
goto 43.7,45.3
.talk 54548
.' <Yes Thrall lets do this>
|confirm always
step
map Hour of Twilight
path follow loose;loop off;ants straight
path	45.5,47.1	45.6,49.0
path	43.0,50.0	40.4,48.7
.' _Follow Thrall_ through this canyon.
.' You will have to fight a _few groups_ through this area.
.from Twilight Assassin##55106+
.from Twilight Thug##55111+
.from Twilight Bruiser##55112+
.from Twilight Ranger##55107+
.from Twilight Shadow-Walker##55109+
'|modelnpc 55474
|confirm always
step
goto Hour of Twilight,39.5,52.4
.' Asira will use _Mark of Silence_ on a random _spellcaster_ and if this spellcaster casts a spell, boss will throw a knife, hitting anyone between the player and the boss. This will silence whoever it hits.
.' Boss will create _Choking Smoke Bomb_ which will not allow players to target while in this cloud. It will also inflict _5k Nature damage_ every .5 seconds.
.' Make sure to stand near _Thall's Fire Totem_ if you can, this will _increase health and damage_ of party members nearby.
.from Asira Dawnslayer##54968
|confirm always
step
goto Hour of Twilight,39.5,52.4
.from Asira Dawnslayer##54968
.collect Urgent Twilight Missive##77957 |n
.' Click the Urgent Twilight Missive in your bags |use Urgent Twilight Missive##77957
..accept 30105
step
goto 40.1,50.8
.clicknpc Life Warden##55549
|invehicle |c
step
.' You will land next to Thrall on the road to Wyrmrest Temple
|outvehicle |c
step
map Hour of Twilight
path follow loose; loop off; straight
path	49.8,70.2	49.7,71.7	49.4,75.4
path	49.4,81.0
path	49.6,82.8
.' _Follow Thrall_ through this canyon.
.' As Damage you should kill the _Shadow Borer's first_, as they interrupt and stop healing.
.from Faceless Shadow Weaver##54633+
.from Faceless Brute##54632+
.from Shadow Borer##54686+
.' Enter Wyrmrest Temple here |goto Hour of Twilight,49.6,82.8,1|noway |c
step
goto Hour of Twilight/2,47.2,50.9
.' Archbishop has a lot of spells that will affect nearby players, _spread out_ before fight starts.
.' _Wave of Virtue_ and _Wave of Twilight_ are waves of Holy or Shadow that will sweep across the stage. These _knock_ anyone _back_ and cause _100k damage_. Run over to Thrall's blue _Water shield_ when you see this coming.
.' There are _2 stages_ of this fight, he will switch from _holy to shadow_ at _60% health_, the only difference in the second stage is Thrall will be trapped and damage will be shadow instead of holy.
.from Archbishop Benedictus##54938
|confirm always
step
goto 46.9,45.0
.talk 54971
..turnin 30103
..turnin 30105
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Hour of Twilight (HoT)\\Hour of Twilight (HoT) Healer Guide",[[
description This guide will walk you through the Hour of Twilight
description instance with quests involved from a Healers perspective.
step
goto Hour of Twilight,48.7,19.7
.talk 54548
..turnin 30102
..accept 30103
step
goto Hour of Twilight,48.7,19.7
.talk 54548
.' <Yes Thrall>
|confirm always
step
map Hour of Twilight
path follow strict;loop off;ants straight
path	50.9,20.3	52.3,21.6	53.9,23.1
.' Follow this path and kill the groups.
.' Talk to Thrall when you are ready to face the boss.
|confirm always
step
label	"pre_Arcurion"
goto 56.7,27.4
.from Arcurion##54590
.' Avoid _Icy Boulders_
.' Dispel _Chains of Ice_. Mass Dispel is extremely effective here.
.' At 30% use Area of Effect healsa long with resistance aura's if you have them.
.' Click here for a more detailed boss fight. |confirm |next "Arcurion" |or
.' or
|confirm |next "part2"
step
label	"Arcurion"
goto 56.7,27.4
'Arcurion will cast _Hand of Frost_ dealing _50k damage_ to his target.
.' Arcurion will cast _chains of frost_, freezing all targets (65yds) and _dealing 10k damage_.
.' You will see blue _circles mark the ground_ for a few seconds, do not stand in this. _Frozen Servitors_ on top of cliffs are throwing _Icy Boulders_ and you will freeze if caught in it.
.' At _30% health_, boss will do a _massive aoe_ in which you will need to use _Resistance Auras_, _Area of Effect Heals_ or _Big Heals_ as needed.
.from Arcurion##54590
.' Click here for a less detailed boss fight |confirm |next "pre_Arcurion" |or
.' or
|confirm
step
label "part2"
.talk 54548
.' <Lead the Way>
|confirm always
step
goto 43.7,45.3
.talk 54548
.' <Yes Thrall lets do this>
|confirm always
step
map Hour of Twilight
path follow loose;loop off;ants straight
path	45.5,47.1	45.6,49.0
path	43.0,50.0	40.4,48.7
.' _Follow Thrall_ through this canyon.
.' You will have to fight a _few groups_ through this area.
.from Twilight Assassin##55106+
.from Twilight Thug##55111+
.from Twilight Bruiser##55112+
.from Twilight Ranger##55107+
.from Twilight Shadow-Walker##55109+
'|modelnpc 55474
|confirm always
step
label	"pre_Asira"
goto Hour of Twilight,39.5,52.4
.from Asira Dawnslayer##54968
.' Avoide _Choking Smoke Bomb_.
.' Stand in _Thrall's Fire Totem_.
.' Use _Large or Area of Effect Heals_ as needed.
.' Click here for a more detailed boss fight. |confirm |next "Asira" |or
.' or
|confirm |next "part3"
step
label	"Asira"
goto Hour of Twilight,39.5,52.4
.' Asira will _mark spellcasters_ and if this spellcaster casts a spell, boss will throw a knife, hitting anyone between the player and the boss. This will silence whoever it hits. Use _Big Heals_.
.' Asira will create _Choking Smoke Bomb_ which will not allow players to target into this cloud, while also inflicting _5k Nature damage_ every .5 seconds.
.' Make sure to stand near _Thall's Fire Totem_, this will _increase health and damage_ of party members nearby.
.from Asira Dawnslayer##54968
.' Click here for a less detailed boss fight |confirm |next "pre_Asira" |or
.' or
|confirm |next "part3"
step
label	"part3"
goto Hour of Twilight 39.5,52.4
.from Asira Dawnslayer##54968
.collect Urgent Twilight Missive##77957 |n
.' Click the Urgent Twilight Missive in your bags. |use Urgent Twilight Missive##77957
..accept 30105
step
goto 40.1,50.8
.clicknpc Life Warden##55549
|invehicle |c
step
.' You will land next to Thrall on the road to Wyrmrest Temple.
|outvehicle |c
step
map Hour of Twilight
path follow loose; loop off; straight
path	49.8,70.2	49.7,71.7	49.4,75.4
path	49.4,81.0
path	49.6,82.8
.' _Follow Thrall_ through this canyon.
.' Kill the _Shadow Borer's first_, they interrupt and stop healing.
.from Faceless Shadow Weaver##54633+
.from Faceless Brute##54632+
.from Shadow Borer##54686+
.' Enter Wyrmrest Temple here. |goto Hour of Twilight 49.6,82.8 |noway |c
step
label	"pre_Bishop"
goto Hour of Twilight/2,47.2,50.9
.from Archbishop Benedictus##54938
.' Spread out 10 yards.
.' Avoid _Wave of Virtue_ stand in blue shell.
.' Avoid _Wave of Twilight_ run to the side.
.' Dispel _Righteous Shear_ as well as _Twilight Shear_.
.' Unmask the Twilight Prophet. |q 30105/1 |only if havequest(30105)
.' Escort Thrall to Wyrmrest Temple. |q 30103/1 |only if havequest(30103)
.' Click here for a more detailed boss fight. |confirm |next "Bishop" |or
.' or
|confirm |next "part4"
step
label	"Bishop"
goto Hour of Twilight/2,47.2,50.9
.' Archbishop has a lot of spells that will affect nearby players, _spread out_ before fight starts.
.' Thrall will help with the first part of the fight, he will put a blue shell on the ground that you can use for protection and a damage buff. Stand in his _Water Shell_ whenever you see it.
.' _Wave of Virtue_ and _Wave of Twilight_ are waves of Holy or Shadow that will sweep across the stage. These _knock_ anyone _back_ and cause _100k damage_. Run to the side to avoid or stand in the _Water Shell_ for the first part of fight.
.' Archbishop will target a random member and cast _Twilight Shear_ or _Purifying Blast_ dealing damage to the target and nearby friendly players.
.' There are _2 stages_ of this fight, he will switch from _holy to shadow_ at _60% health_, the only difference is that Thrall will be trapped and damage will be shadow instead of holy.
.' Area of Effect Heals will be need during some phases.
.from Archbishop Benedictus##54938
.' Unmask the Twilight Prophet. |q 30105/1 |only if havequest(30105)
.' Escort Thrall to Wyrmrest Temple. |q 30103/1 |only if havequest(30103)
.' Click here for a less detailed boss fight. |confirm |next "pre_Bishop" |or
.' or |only if havequest(30103) or havequest(30105)
|confirm |only if havequest(30103) or havequest(30105)
step
label	"part4"
goto 46.9,45.0
.talk 54971
..turnin 30103
..turnin 30105
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Ragefire Chasm (RFC) 15-18",[[
dungeon 680
description This guide will walk you through the Ragefire Chasm dungeon.
achieveid 629
author support@zygorguides.com
step
label	"start"
goto Ragefire Chasm 67.7,11.3
.talk 61716
..accept 30969
..accept 30983
step
goto 68.2,12.2
.talk 61724
..accept 30982
..accept 30984
step
'Follow this path down, but look out for rising lava. If you see lava rising up, wait for it to go back down before crossing |goto 65.9,21.6 <5
step
'_As you go through this Dungeon:_
.from Corrupted Houndmaster##61666+, Corrupted Flamecaller##61705+, Dark Shaman Acolyte##61672+, Corrupted Reaver##61678+
.get 5 Corrupted Insignia |q 30969/1 |n
|confirm
|only if not completedq(30969)
step
goto 68.7,64.2
.from Adarogg##61408 |only if not havequest(30982)
.kill Adarogg##61408 |q 30982/1 |only if havequest(30982)
|confirm |only if not havequest(30982)
step
goto 65.0,70.8
.talk 61780
.' Tell him:
.' <It's Safe to come out now.>
|confirm
|only if not completedq(30984)
step
'Follow this path up, but look out for rising lava. If you see lava rising up, wait for it to go back down before crossing. |goto 61.5,69.1 <5
step
goto 53.0,26.1
.talk 61790
.' Tell him:
.' <It's Safe to come out now.>
|confirm
|only if not completedq(30984)
step
goto 51.8,31.7
.from Dark Shaman Koranthal##61412 |only if not havequest(30983)
.kill Dark Shaman Koranthal##61412 |q 30983/1 |only if havequest(30983)
|confirm |only if not havequest(30983)
step
goto 38.6,58.6
.from Slagmaw##61463 |only if not havequest(30982)
.kill Slagmaw##61463 |q 30982/2 |only if havequest(30982)
|confirm |only if not havequest(30982)
step
goto 42.8,66.5
.clicknpc Kor'kron Scout##61680
|confirm
|only if not completedq(30984)
step
goto 33.7,81.4
.from Lava Guard Gordoth##61528 |only if not havequest(30983)
.kill Lava Guard Gordoth##61528 |q 30983/2 |only if havequest(30983)
|confirm |only if not havequest(30983)
step
goto 36.1,84.3
.click Scout Cages
.' Rescue 5 Kor'kron Scouts |q 30984/1
|modelnpc  Kor'kron Scout##6168
|only if havequest(30984)
step
'_Make sure you have:_
.from Corrupted Houndmaster##61666+, Corrupted Flamecaller##61705+, Dark Shaman Acolyte##61672+, Corrupted Reaver##61678+
.get 5 Corrupted Insignia |q 30969/1
|only if havequest(30969)
step
goto 33.5,80.6
.talk 61716
..turnin 30969
..turnin 30983
step
goto 31.7,76.5
.talk 61724
..turnin 30982
..turnin 30984
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Deadmines (DM) 15-18",[[
dungeon 756
description This guide will walk you through the Deadmines dungeon.
achieveid 628
step
label "start"
goto The Deadmines,29.3,27.1
.talk 46889
..accept 27842
step
goto 28.8,27.3
.talk 46906
..accept 27844
step
goto 29.6,24.8
.talk 46902
..accept 27847
step
goto 37.7,61.2
.from Glubtok##47162 |only if not havequest(27842)
.kill Glubtok##47162 |q 27842/1 |only if havequest(27842)
|confirm |only if not havequest(27842)
|only if not heroic_dung()
step
goto 37.7,61.2
.' _Glubtok_
.' Phase 1:
.' He will _Blink_ occasionally, dropping threat of all enemies. The tank will need to provoke immediately after.
.' PHase 2:
.' When he casts _Fire Blossum_, Fire Blossom creatures spawn and need to be picked up by the tank.
.' When he casts _Frost Blossom_, Frost Blossom creatures spawn and need to be picked up by the tank.
.from Glubtok##47162 |only if not havequest(27842)
.kill Glubtok##47162 |q 27842/1 |only if havequest(27842)
|confirm |only if not havequest(27842)
|only if heroic_dung()
step
.' Click the Quest Completion Box that pops up:
..turnin 27842
step
.' Click the Quest Completion Box that pops up
..accept 27848
step
goto 43.8,79.6 |n
.click Heavy Door##400
.' Enter the room. |goto 43.9,82.3 <5 |noway |c
step
goto 49.0,87.4 |n
.from Lumbering Oaf##47297
.from Helix Gearbreaker##47296 |only if not havequest(27844)
.kill Helix Gearbreaker##47296 |q 27844/1 |only if havequest(27844)
|confirm |only if not havequest(27844)
|only if not heroic_dung()
step
goto 49.0,87.4 |n
.from Lumbering Oaf##47297
.' Phase 1 (Lumbering Oaf):
.' Avoid _Sticky Bombs_ on the ground.
.' The Lumbering Oaf will periodically use _Oaf Smash_, picking up a player and slamming them, dealing 100% of the players health as damage.
.' The Lumbering Oaf will use _Throw Helix_ onto a player for 10 seconds,
.' After Throw Helix, there will be a _Chest Bomb_ on the player, dealing 46k-57k fire damage.
.' _Helix' Crew_ throws bombs at random players throughout the entire fight.
.' Phase 2:
.' Helix will use _Leap_ to random players, attacking for 10 seconds before moving to the next.
.' He will still use _Chest Bomb_ on the player, dealing 46k-57k fire damage.
.from Helix Gearbreaker##47296 |only if not havequest(27844)
.kill Helix Gearbreaker##47296 |q 27844/1 |only if havequest(27844)
|confirm |only if not havequest(27844)
|only if heroic_dung()
step
goto 61.0,75.0
.click Heavy Door##400
|confirm
step
.' Clear the Goblin Foundry of Defias Watchers.
|confirm
'|modelnpc 47404
step
goto The Deadmines/2 10.0,82.7
.from Foe Reaper 5000##43778 |only if not havequest(27847)
.kill Foe Reaper 5000##43778 |q 27847/1 |only if havequest(27847)
|confirm |only if not havequest(27847)
|only if not heroic_dung()
step
goto The Deadmines/2 10.0,82.7
.' _Foe Reaper 5000_
.' If you're the _Prototype Reaper_ pilot, click here. |confirm |next "Proto_Reap"
.' The Foe Reaper 5000 will use _Overdrive_ which will cause it to move around randomly, attacking anyone near him as well as knocking them back.
.' It will also use _Harvest_ which targets a random player, the move to where they are. It attacks anyone in front of him within a 60 degree cone. Once at the target location, it will preform a sweep attack, dealing damage to anyone within 10 yards.
.' At 30% of his total health, he will use _Safety Restrictions Off-line_ which increases his damage by 100% for the duration of the fight.
.' Avoid being in it's path when it uses it's abilities and you should be fine.
.from Foe Reaper 5000##43778 |only if not havequest(27847)
.kill Foe Reaper 5000##43778 |q 27847/1 |only if havequest(27847)
|confirm |next "canon_blast" |only if not havequest(27847)
|only if heroic_dung()
step
label "Proto_Reap"
.' You will spend your time at the bottom of the ramp, waiting for _Molten Slag_ to spawn.
.' You will have 2 abilities to use to control the adds.
.' Use _Reaper Strike_ twice in a row, then use _Reaper Charge_ to stun them.
.' Repeat this until they are defeated.
.from Molten Slag##49229+
|confirm
|only if heroic_dung()
step
label "canon_blast"
goto The Deadmines/2,27.9,52.7
.click Defias Cannon##245
|confirm
step
'Proceed along the docks avoiding the cannonballs that are being shot at the ground. |goto 53.0,20.2 <5 |c
step
goto 63.9,39.5
.from Admiral Ripsnarl##47626 |only if not havequest(27848)
.kill Admiral Ripsnarl##47626 |q 27848/1 |only if havequest(27848)
|confirm |only if not havequest(27848)
|only if not heroic_dung()
step
goto 63.9,39.5
.' _Admiral Ripsnarl_
.' Has a _Swipe_ ability, so attack him from behind.
.' Has the _Thirst for Blood_ ability, moving faster and gaining attack speed each time he hits, stacking up to 20 times)
.' He will use _Go For the Throat_ through the entire fight. It damages then stuns targets for 2 seconds.
.' At 75%, 50% and 25% He will summon a fog.
.' During the fog, he will _Summon Vapors_ which need to be killed as soon as possible. They will grow larger before they explode, potentially killing anyone nearby.
.from Admiral Ripsnarl##47626 |only if not havequest(27848)
.kill Admiral Ripsnarl##47626 |q 27848/1 |only if havequest(27848)
|confirm |only if not havequest(27848)
|only if heroic_dung()
step
'Click the Quest Complete Box in the top right corner:
..turnin 27848
step
'Click the Quest Complete Box in the top right corner:
..accept 27850
step
goto 60.6,44.5
.from "Captain" Cookie##47739 |only if not havequest(27850)
.kill "Captain" Cookie##47739 |q 27850/1 |only if havequest(27850)
|confirm |only if not havequest(27850)
|only if not heroic_dung()
step
goto 60.6,44.5
.' _"Captain" Cookie_
.' He will throw food on the ground. If it does not say _Rotten_ then it's safe to click. You will get a haste buff each time you consume food.
.from "Captain" Cookie##47739 |only if not havequest(27850)
.kill "Captain" Cookie##47739 |q 27850/1 |only if havequest(27850)
|confirm |only if not havequest(27850)
|only if heroic_dung()
step
.' A note will appear on the ground
.' Don't click it until your group is ready, it will start the _Vanessa VanCleef_ event.
.' Once you click it, Vanessa will come out and poison you.
.' When you awaken you'll be suspended above a lava pool. |goto 12.7,80.1 <8 |c
|only if heroic_dung()
step
goto 12.4,71.8
.' Click the _Vent Lever_ to avoid being lowered into the Lava. There will be green arrows indicating where the levers are.
.' Once you click it, you will land at the top of the spiral stairs.
.' You will be dazed and fires will appear. _Avoid the fires at all cost_ they deal out massive damage.
.' Make your way down the ramp. |goto 12.4,71.8 <5 |c
|only if heroic_dung()
step
.from Glubtok##49670
.' After you defeat Glubtok, _Helix Gearbreaker_  will appear at the door.
.' The group needs to collapse at the door, as spiders will spawn and fill the room.
.' Avoid aggroing the spiders if possible.
.from Helix Gearbreaker##49674
|confirm
|only if heroic_dung()
step
goto 29.5,50.2
.' Once the doors open there will be several rotating chains of sparks that you have to avoid as you progress.
.' Most of the time, if you touch them you will be killed, so avoid them while making your way to the coordinates.
.from Foe Reaper 5000##49681
.' Note that you should let your tank go first, since the boss will aggro.
|confirm
|only if heroic_dung()
step
goto 51.6,44.7
.' Follow the deck onto the ship.
.' You will need to save _Emma Harrington_, _Erik Harrington_ and _Calissa Harrington_ from the worgen.
.' Kill any packs of Worgen that you see.
.' Once you're at  _Calissa Harrington_, burn down _Admiral Ripsnarl_ fast.
|confirm
|only if heroic_dung()
step
goto 58.9,38.7
.' Make your way back up to the main part of the ship. |goto 58.9,38.7 <5 |noway|c
|only if heroic_dung()
step
.' _Vanessa VanCleef_
.' She will use _Deflection_, which deflects all ranged, melee and spell attacks for 10 seconds. She will use the ability when her health is higher thatn 25%.
.' She will use _Deadly Blades_, randomly attack party members.
.' She will use _Backslash_ dealing 9k to 10k damage.
.' At 50%, she will use _Fiery Blaze_ which deals 46k to 53k fire damage every second. When this happens, ropes will appear at the end of the deck. Click them to avoid the damage.
.' There will also be adds that need to be picked up by the tank.
.' When Vanessa is at 1% of her total health, she will use _Powder Explosion_, Move as far away from her as possible to avoid death.
.from Vanessa VanCleef##49541
|only if heroic_dung()
step
goto 57.7,39.7
.talk 46906
..turnin 27844
|only if havequest(27844)
step
goto 61.4,38.6
.talk 46902
..turnin 27847
|only if havequest(27847)
step
goto 64.2,39.9
.talk 46889
..turnin 27850
|only if havequest(27850)
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Shadowfang Keep (SFK) 18-23",[[
dungeon 764
description This guide will walk you through the Shadowfang Keep dungeon.
achieveid 631
author support@zygorguides.com
step
label	"start"
goto Shadowfang Keep,64.4,58.0
.talk 47293
..accept 27974
step
map Shadowfang Keep
path follow loose; loop off
path	72.8,50.5	61.1,44.4	60.7,54.9
path	58.5,63.8
.' Follow this path and kill any mobs you come in contact with. |goto 58.5,63.8 <5 |noway |c
step
goto 67.5,72.7
.from Baron Ashbury##46962 |only if not havequest(27974)
.kill Baron Ashbury##46962 |q 27974/1 |only if havequest(27974)
|confirm |only if not havequest(27974)
|only if not heroic_dung()
step
goto 67.5,72.7
.' _Baron Ashbury_
.' He will commonly cast _Pain and Suffering_, which needs to be interrupted fast each time. An interrupt rotation should be established.
.' Note that any class that can dispel magic effect can remove Pain and Suffering.
.' He will cast _Asphyxiate_, which will drop the groups health to 1.
.' Immediately after, _Stay of Execution_ which heals the party for 10% health per tick, as well as giving him 5% health each tick. The trick is to get your health up by 20% before interrupting.
.' As the fight progresses,_Wracking Pains_ stacks will begin to appear on players. It increases your shadow damage vulnerability by 20% as well as inflicting shadow damage. This effect stacks.
.' At 25% health, he will cast _Calamity_, dealing out 5,000 shadow damage as well as increasing shadow damage enemies take every second.
.from Baron Ashbury##46962 |only if not havequest(27974)
.kill Baron Ashbury##46962 |q 27974/1 |only if havequest(27974)
|confirm |only if not havequest(27974)
|only if heroic_dung()
step
goto 67.8,73.1
.talk 47293
..turnin 27974
step
goto 67.8,73.1
.talk 47293
..accept 27988
step
goto Shadowfang Keep,55.2,65.8 |n
.' Go out into the Courtyard. |goto Shadowfang Keep 55.2,65.8 <5 |noway |c
step
goto 36.8,39.6 |n
.' Enter the Dining Hall here. |goto 36.8,39.6 <5 |noway |c
step
goto Shadowfang Keep/2,31.2,73.3
.from Baron Silverlaine##3887 |only if not havequest(27988)
.kill Baron Silverlaine##3887 |q 27988/1 |only if havequest(27988)
|confirm |only if not havequest(27988)
|only if not heroic_dung()
step
goto Shadowfang Keep/2,31.2,73.3
.' _Baron Silverlaine_
.' He will cast _Veil of Shadow_, which reduces healing taken by 50%.
.' It can be removed by anyone who can remove Curses.
.' He will also cast _Cursed Veil_ affecting all party members within 40 yards, dealing 24k to 25k shadow damage, along with reducing healing taken by 75%.
.' Cursed Veil can be removed by anyone who can remove magic.
.' At 90%, 60% and 30% he will cast _Summon Worgen Spirits_.
.' _Odo the Blindwatcher_ just deals damage.
.' _Razorclaw the Butcher_ stuns targets furthest away from them. He also drains health based on his damage done.
.' _Rethilgore_ drains health from enemy targets, stunning them. Heals for twice the amount that he drains.
.' _Wolf Master Nandos_ summons Lupine Spirits with low health.
.from Baron Silverlaine##3887 |only if not havequest(27988)
.kill Baron Silverlaine##3887 |q 27988/1 |only if havequest(27988)
|confirm |only if not havequest(27988)
|only if heroic_dung()
step
'Run up the stairs and follow the hallway to the _left_. |goto 27.4,89.5 <5 |c
step
goto Shadowfang Keep 28.4,58.6
.from Commander Springvale##4278 |only if not havequest(27988)
.kill Commander Springvale##4278 |q 27988/2 |only if havequest(27988)
|confirm |only if not havequest(27988)
|only if not heroic_dung()
step
goto Shadowfang Keep 28.4,58.6
_Commander Springvale_
.' He will cast _Word of Shame_ on random party members, causing 5% of the players health every 3 seconds until he or the player is defeated.
.' During the fight, he will summon 2 _Wailing Guardsmans_ and _Tormented Officers_ to assist him. The tank should pick them up.
.' He will often use _Shield of the Perfidious_ which shoots a cone attack in front of him. It expands to 150 degrees and should be avoided.
.from Commander Springvale##4278 |only if not havequest(27988)
.kill Commander Springvale##4278 |q 27988/2 |only if havequest(27988)
|confirm |only if not havequest(27988)
|only if heroic_dung()
step
goto Shadowfang Keep,28.0,60.2
.talk 47293
..turnin 27988
step
goto Shadowfang Keep,28.0,60.2
.talk 47293
..accept 27996
step
goto 35.6,65.4 |n
'Follow the stairs down and go outside. |goto Shadowfang Keep/7 |noway |c
step
map Shadowfang Keep/7
path follow loose; loop off
path	57.4,90.6	68.2,82.3	71.4,58.6
path	43.5,46.8	45.9,39.9	44.4,30.3
.' Follow this path through the dungeon, kill any mobs you come in contact with. |goto Shadowfang Keep/3 |noway |c
step
map Shadowfang Keep/3
path follow strict; loop off
path	48.8,46.5	57.6,10.1	64.7,38.2
path	51.6,81.1
.' Follow this path through the dungeons, kill any mobs you come in contact with. |goto Shadowfang Keep/4 |noway |c
step
goto Shadowfang Keep/4,54.2,54.3
.from Lord Walden##46963 |only if not havequest(27996)
.kill Lord Walden##46963n |q 27996/1 |only if havequest(27996)
|confirm |only if not havequest(27996)
|only if not heroic_dung()
step
goto Shadowfang Keep/4,54.2,54.3
.' _Lord Walden_
.' He will fire _Ice Shards_ randomly around the room, damaging enemies within 2 yards of where it lands.
.' He will use _Conjure Poisonous Mixture_, which will reduce your movement speed by 40% as well as leave a dot.
.' He will use _Toxic Coagulent_, which will be explained below:
.' When he has a _Red Aura_ around him, _do not move!_ You will take extra damage for each step you take.
.' When he has a _Green Aura_ around him, _Move around constantly!_ You will take extra damage the longer you don't move.
.' These effects stack up to 3 times and by the third stack you are stunned for 5 seconds.
.' At 35% of his total health, he will use _Toxic Catalyst_, dealing out 13k to 16k Fire damage every two seconds and increasing the targets critical strike rating to 100%.
.from Lord Walden##46963 |only if not havequest(27996)
.kill Lord Walden##46963 |q 27996/1 |only if havequest(27996)
|confirm |only if not havequest(27996)
|only if heroic_dung()
step
goto Shadowfang Keep/4,59.9,41.8
.talk 47293
..turnin 27996
..accept 27998
step
map Shadowfang Keep/4
path follow strict; loop off
path	58.6,27.3	36.8,65.2
.' Follow the path up the stairs. |goto Shadowfang Keep/5 |noway |c
step
map Shadowfang Keep/5
path follow strict; loop off
path	49.2,74.7	60.4,31.7	37.3,65.2
.' Follow the path up the stairs. |goto Shadowfang Keep/6 |noway |c
step
goto Shadowfang Keep/6,61.6,23.8
.from Lord Godfrey##46964 |only if not havequest(27998)
.kill Lord Godfrey##46964 |q 27998/1 |only if havequest(27998)
|only if not heroic_dung()
step
goto Shadowfang Keep/6,61.6,23.8
.' _Lord Godfrey_
.' He will often cast _Cursed Bullets_ on a random target. It can be _interrupted_.
.' If it doesn't get interrupted it will deal out significant shadow damage every 3 seconds, increasing over time. Anyone who can remove curses can dispel it.
.' He will use _Pistol Barrage_ often, an attack that deals out 30k damage to anyone within a 60 degree cone in front of him. Avoid at all costs.
.' He will place _Mortal Wound_ on the tank, reducing healing taken by 5%, stacking 10 times.
.' Throughout the fight, he will _Summon Bloodthirsty Ghouls_ that have low health. They should be picked up and killed quickly.
.from Lord Godfrey##46964 |only if not havequest(27998)
.kill Lord Godfrey##46964 |q 27998/1 |only if havequest(27998)
|only if heroic_dung()
step
goto Shadowfang Keep/6,58.8,52.3
.talk 47293
..turnin 27998
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Wailing Caverns (WC) 18-23",[[
dungeon 749
description This guide will walk you through the Wailing Caverns dungeon.
achieveid 630
step
label "start"
goto Wailing Caverns,46.6,59.2
.talk 5768
..accept 26870
step
goto 46.6,58.4
.talk 5767
..accept 26872
..accept 26873
step
' Do the following as you move through the instance
.from Deviate Ravager##3636+, Deviate Guardian##3637+, Deviate Crocolisk##5053+
..get 10 Deviate Hide |q 26872/1
.click Serpentbloom##390+
..get 5 Serpentbloom |q 26873/1
|confirm
|only if havequest(26873) or havequest(26872)
step
goto 29.7,43.9
.from Lady Anacondra##3671 |only if not havequest(26870)
.kill Lady Anacondra##3671 |q 26870/2 |only if havequest(26870)
|confirm |only if not havequest(26870)
step
.from Druid of the Fang##3840
.' Make sure to kill any Druid of the Fang you come across.
|confirm
step
map Wailing Caverns
path loop off; strict
path	28.2,40.1	19.6,45.2	9.2,33.5
path	13.7,33.9	19.8,41.6
.' Jump in the water and follow this path
.from Lord Pythas##3670 |only if  not havequest(26870)
..kill Lord Pythas##3670 |q 26870/3 |only if havequest(26870)
|confirm |only if not havequest(26870)
step
map Wailing Caverns
path loop off; strict
path	17.4,28.3	15.4,24.1	5.7,27.8
path	15.6,58.4
.' Follow this path and kill Lord Cobrahn.
.from Lord Cobrahn##3669 |only if not havequest(26870)
..kill Lord Cobrahn##3669 |q 26870/1 |only if havequest(26870)
|confirm |only if not havequest(26870)
step
goto 15.8,51.8 |n
.' Jump down here and follow the path to the right. |goto 15.8,51.8 <5 |noway |c
step
.' If you haven't already killed Kresh, then you will find him walking in this little river.
.from Kresh##3653
|confirm
step
goto 60.7,72.1
.from Skum##3674
|confirm
step
goto 55.0,87.6 |n
.' Follow this path up and around. |goto 55.0,87.6 <5 |noway |c
step
map Wailing Caverns
path follow strict; loop off
path	54.8,83.1	55.2,71.7	50.7,61.8
path	72.8,67.7	69.2,82.9	54.4,76.3
path	54.6,60.4	62.2,53.8
.from Lord Serpentis##3673 |only if not havequest(26870)
.kill Lord Serpentis##3673 |q 26870/4 |only if havequest(26870)
|confirm |only if not havequest(26870)
step
goto 56.4,47.5
.from Verdan the Everliving##5775
|confirm
step
goto 54.5,39.5 |n
.' Jump down this hole. |goto 54.5,39.5 <5 |noway |c
step
goto Wailing Caverns,46.6,59.2
.talk 5768
..turnin 26870
step
goto 46.6,58.4
.talk 5767
..turnin 26872
..turnin 26873
step
goto 46.5,56.0
.talk 3678
.' Tell him:
.' <Let the event begin!>
|confirm
step
goto 37.1,20.9 |n
.' Escort and Protect Muyoh until you get to the boss. |goto 37.1,20.9 <5 |noway |c
step
goto 34.0,15.6 |n
.' Protect Muyoh.
.' There will be 2 waves of mobs that attack him, then the boss will appear.
.from Mutanus the Devourer##3654
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Blackfathom Deeps (BFD) 22-27",[[
dungeon 688
description This guide will walk you through the Blackfathom Deeps dungeon.
achieveid 632
author support@zygorguides.com
step
label	"start"
goto Blackfathom Deeps 48.4,11.7
.talk 12736
..accept 26888
..accept 26891
step
goto 48.2,13.1
.talk 44375
..accept 26892
step
goto Blackfathom Deeps 33.5,59.4
.from Ghamoo-Ra##4887 |only if not havequest(26892)
.kill Ghamoo-Ra##4887 |q 26892/1 |only if havequest(26892)
|confirm |only if not havequest(26892)
step
goto Blackfathom Deeps 23.7,45.5 |n
.' Swim under the water here. |goto Blackfathom Deeps 23.7,45.5 <5 |noway |c
step
goto Blackfathom Deeps 11.6,40.4
.from Lady Sarevess##4831 |only if not havequest(26892)
.kill Lady Sarevess##4831 |q 26892/2 |only if havequest(26892)
|confirm |only if not havequest(26892)
step
goto Blackfathom Deeps 24.4,46.5 |n
.' Swim under the water here. |goto Blackfathom Deeps 24.4,46.5 <5 |noway |c
step
goto Blackfathom Deeps,32.5,90.2 |n
.' Follow this hallway. |goto Blackfathom Deeps 32.5,90.2 <5 |noway |c
step
goto Blackfathom Deeps,52.3,55.4
.from Gelihast##6243 |only if not havequest(26892)
.kill Gelihast##6243 |q 26892/3 |only if havequest(26892)
|confirm |only if not havequest(26892)
step
goto 59.5,71.8 |n
.' Follow this hallway. |goto Blackfathom Deeps/2 |noway |c
step
goto 31.8,66.9
.talk 44387
..accept 26899 |only if not havequest(26899)
|only if not completedq(26899)
step
goto 41.5,75.3
.' Jump in the water
.click Fathom Stone##01027
.get Fathom Core |q 26891/1
|only if havequest(26891)
step
goto 34.0,68.0 |n
.' Jump out of the water here. |goto 34.0,68.0 <5 |noway |c
|only if havequest(26891)
step
goto 40.8,65.8 |n
.' Follow these stairs to the right and go down the path. |goto 40.8,65.8 <5|noway |c
step
goto 51.5,81.6
.from Twilight Lord Kelris##4832
|confirm |only if not havequest(26899)
.get Head of Kelris |q 26899/1 |only if havequest(26899)
step
goto 52.3,80.8
.click Fire of Aku'mai##00524
.' Each time you click a Fire, you will have adds come from the right and left side of the room.
.' Click all 4 of them before continuing.
|confirm
step
goto 79.1,84.2
.from Aku'mai##4829 |only if not havequest(26888)
.kill Aku'mai##4829 |q 26888/1 |only if havequest(26888)
step
goto 32.0,67.2
.talk 44387
..turnin 26899
|only if havequest(26899)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
|only if havequest(26888) or havequest(26891) or havequest(26892)
step
goto Blackfathom Deeps,48.4,11.7
.talk 12736
..turnin 26888
..turnin 26891
|only if havequest(26888) or havequest(26891)
step
goto 48.2,13.1
.talk 44375
..turnin 26892
|only if havequest(26892)
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\The Stockade (STOCKS) 22-27",[[
dungeon 690
description This guide will walk you through the Stockade dungeon.
achieveid 633
author support@zygorguides.com
step
label	"start"
goto The Stockade 50.8,19.2
.from Randolph Moloch##46383
|confirm
step
goto 81.0,48.0
.from Lord Overheat##46264
|confirm
step
goto 22.1,27.0
.from Hogger##46254
|confirm
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Gnomeregan (Gnomer) 26-31",[[
dungeon 691
description This guide will walk you through the Gnomeregan dungeon.
achieveid 634
author support@zygorguides.com
step
label	"start"
goto Gnomeregan 81.9,65.1
.talk 7998
.' Tell her:
.' <I am ready to begin>
|confirm
step
goto 76.3,64.3
.' Get ready for the wall to break and kill lots of mobs.
.' There will be 2 waves, about 15-20 enemies on each side of the hallway.
.from Grubbis##7361
|confirm
|modelnpc Caverndeep Ambusher##6207+, Caverndeep Burrower##6206+, Chomper##6215
step
goto 57.6,51.4 |n
.' Jump down from this ledge, onto the big Bolt below. |goto Gnomeregan/2 |noway |c
step
'Walking all around this area
.from Viscous Fallout##7079
|confirm
step
goto 39.3,67.5 |n
.' Follow the path down to this area, run around the ramp clockwise. |goto 39.3,67.5 <5 |noway |c
step
goto 14.1,64.8 |n
.' Follow this ramp up. |goto 14.1,64.8 <5 |noway |c
step
goto 23.7,68.3
.from Electrocutioner 6000##6235
|confirm
step
goto 23.4,72.9 |n
.' Jump down here to turn in your quest. |goto Gnomeregan/3 |noway |c
step
goto Gnomeregan/4,28.8,45.4 |n
.' Click the Big door and go into the room. |goto 29.3,42.9 <5 |noway |c
step
goto 31.3,29.9
.from Mekgineer Thermaplugg##7800
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Scarlet Halls (SH) 28-31",[[
dungeon 871
dungeondifficulty Normal
description This guide will walk you through the Scarlet Halls dungeon.
achieveid 7413
author support@zygorguides.com
step
goto Scarlet Halls/1 31.9,84.4
.talk 64738
..accept 31490
..accept 31493
step
.from Vigilant Watchman##58898+, Master Archer##59175+, Scarlet Evangelist##58685+, Scarlet Myrmidon##58683+, Scarlet Cannoneer##59293+, Scarlet Scourge Hewer##58684+, Scarlet Defender##58676+, Scarlet Evoker##58756+, Scarlet Defender##58998+, Scarlet Treasurer##59241+, Scarlet Hall Guardian##59240+, Scarlet Pupil##59373+, Scarlet Scholar##59372+
.' Kill 50 Scarlet Crusaders. |q 31490/1
|only if havequest(31490)
step
'Prepare to cross the training ground. |goto 48.2,78.8 <5 |c
step
'In order to cross the training ground you must click on an archery target and use it as a shield. Avoid the fire arrows that leave patches of flame on the ground.
.from Commander Lindon##59191
|confirm
step
' Go through the tunnel |goto 49.8,36.9 <5 |c
step
'_Houndmaster Braun_
.' Houndmaster Braun will use _Piercing Throw_. This will target a player and deal damage to every player in between Braun and his target.
.' _Death Blossom_ is an AoE whirlwind, spread out to reduce its effectiveness.
.' Both of these abilities will cause _Bloody Mess_. This is DoT can stack and become deadly.
.' At _90%, 80%, 70%, and 60%_ Braun will summon an Obedient Hound to his side. You can either kill them or just have the tank hold onto them.
.' If the hounds remain alive at _50%_ Braun will begin to lose control of them and the will aid you in battle. When this happens, Braun will enter a _Bloody Rage_, gaining _25% increased damage_ and _50% increased attack speed_.
.from Houndmaster Braun##59303
|confirm
step
goto 55.3,21.1 |n
'Allow the remaining dogs to kill the door guards and procede to the Athenaeum. |goto Scarlet Halls/2 |noway |c
step
map Scarlet Halls/2
path follow strict;loop off;ants straight
path	47.6,88.3	56.8,79.4	61.7,69.5
path	61.7,55.5
'While following this path keep an eye out for Scarlet Cannoneers. They will spawn randomly and attack using the cannons along this path.
|goto 61.7,55.5 <5 |noway |c
|modelnpc Scarlet Cannoneer##59293
step
'_Armsmaster Harlan_
.' _Dragon's Reach_ is a cleave ability, avoid being in front of this boss.
.' Harlan will periodically summon two Scarlet Defenders. You can either kill these adds or you can wait for them to be caught in Harlan's _Blades of Light_ ability.
.' He will use _Heroic Leap_ to the center of the pit and begin casting _Blades of Light_. This whirlwind quickly moves around the entire area and does massive damage to anybody caught within it. Jump between the different levels of this area to avoid this ability easily.
.from Armsmaster Harlan##58632
|confirm
step
map Scarlet Halls/2
path follow strict;loop off;ants straight
path	48.6,58.1	48.7,50.5	39.1,46.8
path	39.1,23.2
.' Follow the path to the final boss.
step
'_Flameweave Koegler_
.' Koegler will try to light the book cases around the room on fire with _Book Burner_. You can avoid this mechanic by staying on the red carpet circle inside this room.
.' He will move to the center of the room and begin using _Greater Dragon's Breath_. Watch where he is facing and move around the room clockwise to avoid being hit.
.' _Quickened Mind_ will cause Koegler's next three spells to be cast nearly instantly. This can be dispelled off of him.
.from Flameweaver Koegler##59150
.get Codex of the Crusade |q 31493/1 |only if havequest(31493)
|confirm |only if not havequest(31493)
step
goto 39.3,17.0
.talk 64738
..turnin 31490
..turnin 31493
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Scarlet Monastery (SM) 30-33",[[
dungeon 874
dungeondifficulty Normal
description This guide will walk you through the Scarlet Monastery dungeon.
achieveid 637
author support@zygorguides.com
step
goto Scarlet Monastery/1 72.7,47.0
.talk 64827
..accept 31513
step
goto 30.8,46.2
.' Clear the trash up to this point. The Piles of Corpses should be killed first or you will be fighting zombies much longer than is necessary.
|confirm
step
'_Thalnos the Soulrender_
.' Thalnos will _Raise Fallen Crusaders_. This ability summons zombies that the tank needs to pick up. When the Fallen Crusaders hit you, they leave behind _Mind Rot_ which is a DoT that can stack.
.' He also summons _Spirit Gales_ often. These are puddles on the ground that should not be stepped in. This ability is interruptable.
.' _Evict Soul_ is a debuff that Thalnos throws on a random party member. This DoT needs to be dispelled as quickly as possible.
.' He will also use _Summon Empowering Spirit_. This ability will summon an add that DPS should kill as quickly as they can. If this add reaches a Fallen Crusader's corpse, it will spawn an _Empowered Zombie_.
.' _Empowered Zombies_ are large zombies which need to be tanked. They cannot be killed without great focus.
.from Thalnos the Soulrender##59789
|confirm
step
goto 20.6,45.9
.click Blade of the Anointed
.get Blade of the Anointed |q 31513/1
step
map Scarlet Monastery/1
path follow strict;loop off;ants straight
path	34.5,52.5	34.8,78.2	39.4,81.7
path	48.5,90.3
.' Follow the path. |goto Scarlet Monastery/2 |noway |c
step
goto Scarlet Monastery/2 49.1,24.5
.talk 64855
..turnin 31513
..accept 31514
step
map Scarlet Monastery/2
path follow strict;loop off;ants straight
path	45.7,31.9	43.9,36.3	49.2,43.3
'_Brother Korloff_
.' Brother Korloff loves to play with fire.
.' _Flying Kick_ will hit a random party member knocking them back, make sure they aren't knocked into any uncleared trash.
.' _Blazing Fists_ is a series of punches Korloff makes in front of him, have the tank take a step back and make sure all party members are behind Korloff.
.' _Firestorm Kick_ is whirlwind of flame, do not stand in it.
.' _Scorched Earth_ is a fire trail that Korloff leaves behind him, do not stand in it.
.' Every 10%, Korloff wil gain a stack of _Rising Flame_. These increase his fire damage done by 10% per stack.
.from Brother Korloff##59223
|tip The boss will patrol between two groups. Pull each group carefully, keeping in mind the boss' patrol.
|confirm
step
'Enter the cathedral here |goto 49.1,57.1 <5 |c
step
'Before encountering the boss, make sure you kill the trash within the cathedral. This includes the main room and both wings.
|confirm
step
'_Phase 1_
.' Commander Durand will fight you alone.
.' He uses _Flash of Steel_ and _Dashing Strike_. Both of these abilities cause him to jump to party members quickly, but neither do much damage as long as the group is spread out.
.' When Durand dies, phase 2 begins.
'_Phase 2_
.' High Inquisitor Whitemane will come and and begin to fight.
.' She will cast _Power Word: Shield, Smite and Heal_. Make sure to interrupt Smite and Heal.
.' Whitemane will also try casting _Mass Resurrection_. _DO NOT LET THIS CAST FINISH!!_ If this cast finishes, all corpses within 100 yards will resurrect and aggro.
.' After you weaken her she will cast _Deep Sleep_. This puts the entire party to sleep for 10 seconds, during this time she will resurrect Durand.
'_Phase 3_
.' Now you will have to fight the two together.
.' Kill Whitemane first, make sure to interrupt _Smite and Heal_
.' Stay spread out for Durand.
.from Commander Durand##60040
.from High Inquisitor Whitemane##3977
.' Use the Blades of the Anointed on High Inquisitor Whitemane's corpse. |use Blades of the Anointed##87390 |only if havequest(31514)
.' Blades of the Anointed thrust into Whitemane's corpse. |q 31514/1 |only if havequest(31514)
|confirm |only if not havequest(31514)
step
.talk 64842
..turnin 31514
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Razorfen Kraul (RFK) 32-37",[[
dungeon 761
description This guide will walk you through the Razorfen Kraul dungeon.
achieveid 635
author support@zygorguides.com
step
label	"start"
goto Razorfen Kraul,68.3,83.0
.talk 44402
..accept 26901
..accept 26906
..accept 26907
step
map Razorfen Kraul
path loop off
path	62.5,71.8	78.7,64.2	84.4,47.0
path	75.3,38.7
.' Follow the path to the right. |goto 75.3,38.7 <5 |noway |c
step
goto Razorfen Kraul,87.7,41.2
.from Death Speaker Jargba##4428 |only if not havequest(26907)
.kill Death Speaker Jargba##4428 |q 26907/3 |only if havequest(26907)
|confirm |only if not havequest(26907)
step
goto 81.0,53.9
.from Aggem Thorncurse##4424 |only if not havequest(26907)
.kill Aggem Thorncurse##4424 |q 26907/4 |only if havequest(26907)
|confirm |only if not havequest(26907)
step
goto 56.9,29.8
.from Overlord Ramtusk##4420 |only if not havequest(26907)
.kill Overlord Ramtusk##4420 |q 26907/2 |only if havequest(26907)
|confirm |only if not havequest(26907)
step
goto 56.0,44.2 |n
.' Follow this path to the right. |goto 56.0,44.2 <10 |noway |c
step
goto Razorfen Kraul 14.2,54.6
.from Kraul Bat##4538+
.get Kraul Guano |q 26901/1
|only if havequest(26901)
step
goto 7.2,66.9
.from Agathelos the Raging##4422 |only if not havequest(26907)
.kill Agathelos the Raging##4422 |q 26907/1 |only if havequest(26907)
|confirm |only if not havequest(26907)
step
goto 19.9,31.9
.talk 44415
..turnin 26906
..accept 26905
step
goto 26.7,32.5
.from Charlga Razorflank##4421
.get Razorflank's Heart |q 26905/1 |only if havequest(26905)
|confirm |only if not havequest(26905)
step
goto 20.0,31.9
.talk 44415
..turnin 26905
step
goto Razorfen Kraul 68.2,82.9
.talk 44402
..turnin 26901
..turnin 26907
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Maraudon: The Wicked Grotto (Mara) 33-38",[[
dungeon 750
description This guide will walk you through the Maraudon: The Wicked Grotto dungeon.
author support@zygorguides.com
step
label	"start"
goto Maraudon 76.7,64.7
.' Click the Quest Accept box in the top right cornner:
..accept 27697
step
map Maraudon
path loose; loop off
path	70.8,65.9	65.5,60.5	57.2,49.1
path	48.9,56.1
.' Follow this path.
.from Tinkerer Gizlock##13601
|confirm
step
map Maraudon
path loose; loop off
path	50.7,58.8	49.5,71.5	43.7,87.0
path	35.3,79.9	41.9,76.4	37.5,75.3
.' Follow this path
.from Lord Vyletongue##12236 |only if not havequest(27697)
.kill Lord Vyletongue##12236 |q 27697/1 |only if havequest(27697)
step
'Click the Complete Quest box in the top right corner:
..turnin 27697
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Maraudon: Foulspore Cavern (Mara) 35-40",[[
dungeon 750
description This guide will walk you through the Maraudon: Foulspore Cavern dungeon.
author support@zygorguides.com
step
label	"start"
goto Maraudon 60.8,21.2
.' Click the Accept Quest box on the right side of your screen
..accept 27698
step
goto Maraudon,54.2,25.8 |n
.' Follow the left side of this path. |goto Maraudon 54.2,25.8 <5 |noway |c
step
map Maraudon
path follow strict; loop off
path	45.3,34.0	40.0,42.5	50.3,43.2
path	52.7,33.1	45.9,25.7	34.5,10.9
.' Follow this path
.from Noxxion##13282 |only if not havequest(27698)
.kill Noxxion##13282 |q 27698/1 |only if havequest(27698)
|confirm |only if not havequest(27698)
step
map Maraudon
path follow loose; loop off
path	36.2,27.6	32.1,40.3	24.6,36.4
path	16.4,34.1
.' Follow this path:
.from Razorlash##12258 |only if not havequest(27698)
.kill Razorlash##12258 |q 27698/2 |only if havequest(27698)
step
'Click the Complete Quest box on the right side of your screen
..turnin 27698
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Maraudon: Earth Song Falls (Mara) 36-41",[[
dungeon 750
description This guide will walk you through the Maraudon: Earth Song Falls dungeon.
achieveid 640
author support@zygorguides.com
step
label	"start"
goto Maraudon/2 28.2,37.3 |n
.' Jump down the waterfall here. |goto 28.2,37.3 <5 |noway |c
step
goto 29.9,50.0
..accept 27692
step
goto 33.2,60.8 |n
.' Follow the path through this doorway. |goto 33.2,60.8 <5 |noway |c
step
map Maraudon/2
path loop off
path	35.4,57.2	41.8,61.4	44.9,54.1
path	40.1,64.2	36.6,55.4	40.6,48.7
.from Landslide##12203
|confirm
step
goto 44.3,67.1 |n
.' Run down the ramp and follow this path. |goto 44.3,67.1 <5 |noway |c
step
map Maraudon/2
path loop off
path	46.1,68.1	44.4,73.8	36.6,79.7
path	29.4,78.2
.' Follow this path. |goto 29.4,78.2 <5 |noway |c
step
goto 26.6,78.8
.from Princess Theradras##12201 |only if not havequest(27692)
.kill Princess Theradras##12201 |q 27692/1 |only if havequest(27692)
|confirm |only if not havequest(27692)
step
'Click the Quest Complete box in the top right corner
..turnin 27692
step
goto 31.3,75.6 |n
.' Jump down the waterfall here. |goto 31.3,75.6 <5 |noway |c
step
goto 41.6,79.5
.from Rotgrip##13596
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Dire Maul: Warpwood Quarter (DM) 38-43",[[
dungeon 699
description This guide will walk you through the Dire Maul: Warpwood Quarter dungeon.
achieveid 644
author support@zygorguides.com
step
label	"start"
goto Dire Maul/5 7.1,36.3
.talk 44971
..accept 27107
..accept 27108
step
goto 12.5,30.9
.talk 14354
.' Tell him:
.' <Game? Are you crazy?>
|confirm
step
goto 11.2,38.4
.talk 44969
..accept 27103
..accept 27104
..accept 27105
step
goto 22.1,75.3
.talk 14354
.' Tell him:
.' <Why you little..>
|confirm
step
goto 32.5,26.9
.talk 14354
.' Tell him:
.' <Mark my words, I will catch you, imp. And when I do!>
|confirm
step
goto 42.8,49.1
.from Lethtendris##14327+
.get Lethtendris' Web |q 27108/1 |only if havequest(27108)
|confirm |only if not havequest(27108)
step
goto 66.0,38.0
.talk 14354
.' Tell him:
.' <DIE!>
|confirm
step
goto 76.3,45.5
.talk 14354
.' Tell him:
.' <Prepare to meet your maker.>
.from Pusillin##14354
.get Book of Incantations |q 27107/1 |only if havequest(27107)
|confirm |only if not havequest(27107)
step
goto 46.8,63.2 |n
.' Go to the ramp here. |goto 46.8,63.2 <5 |noway|c
step
goto Dire Maul/6,55.5,71.4
.from Hydrospawn##13280
.get Hydrospawn Essence |q 27105/1 |only if havequest(27105)
|confirm |only if not havequest(27105)
step
goto 57.8,56.1 |n
.' Go up the ramp here. |goto 57.8,56.1 <5 |noway|c
step
goto 57.6,74.0
.from Zevrim Thornhoof##11490 |only if not havequest(27104)
.kill Zevrim Thornhoof##11490 |q 27104/1 |only if havequest(27104)
|confirm |only if not havequest(27104)
step
goto 52.8,88.5 |n
.' Go through this hallway and to the right. |goto 52.8,88.5 <5 |noway |c
step
goto 39.8,78.2 |n
.talk 11491
.' Tell him to open the Door.
.' Follow him and go through the door he opens. |goto 40.2,48.7 <5 |c
step
goto Dire Maul/6,57.7,28.9
.from Alzzin the Wildshaper##11492 |only if not havequest(27104)
.kill Alzzin the Wildshaper##11492 |q 27104/2 |only if havequest(27104)
|confirm |only if not havequest(27104)
step
goto 55.0,27.1
.click Felvine Shard##5746
.' Click the Reliquary of Purity in your bags |use Reliquary of Purity##18539
.collect Felvine Shard##18501 |n
.get Sealed Reliquary of Purity |q 27103/1
|only if havequest(27103)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Dire Maul 7.0,36.2
.talk 44971
..turnin 27107
..turnin 27108
step
goto 11.2,38.4
.talk 44969
..turnin 27103
..turnin 27104
..turnin 27105
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Scholomance (Scholo) 40-43",[[
dungeon 898
dungeondifficulty Normal
description This guide will walk you through the Scholomance dungeon.
achieveid 645
author support@zygorguides.com
step
goto Scholomance/1 27.0,58.8
.talk 64562
..accept 31440
..accept 31447
step
'Go through the gate and down these steps |goto 48.6,43.2 <5 |c
step
'Clear this room of trash and be careful not to aggro Instructor Chillheart.
|confirm
step
'_Instructor Chillheart_
'_Phase 1_
.' _Wrack Soul_ will deal damage to a party member and jump to a nearby party member when it expires. Spread out.
.' _Touch of the Grave_ causes Chillheart's next 10 basic attacks to deal additional damage.
.' _Frigid Grasp_ will summon ice at a player's feet, step out of this before it explodes.
.' _Ice Wall_ will start at the back of the room and slowly creep toward the boss. If this touches any player then they will die instantly. This will act as the enrage timer of the fight.
.' Kill Chillheart's physical body and Phase 2 will start.
.from Instructor Chillheart##58633
'_Phase 2_
.' _The Anarchist Arcanist_ will float around creating _Arcane Bombs_. These explode on impact so avoid them.
.' _Antonidas' Self Help Guide to Standing in Fire_ will cast _Burn_. This does a small AoE of fire damage around the book.
.' _Wander's Colossal Book of Shadow Puppets_ will cast _Shadow Bolt_ at the party.
.from Instructor Chillheart's Phylactery##58664
|confirm
step
goto Scholomance/1 74.7,57.5
.click In the Shadow of the Light
.' Destroy In the Shadow of the Light |q 31440/1
|only if havequest(31440)
step
map Scholomance/1
path follow strict;loop off;ants straight
path	79.3,52.0	80.6,23.8
'Follow the path downstairs. |goto Scholomance/2 77.0,26.5 <5 |c
step
goto Scholomance/2 73.9,8.6
.click Kel'Thuzad's Deep Knowledge
.' Destroy Kel'Thuzad's Deep Knowledge |q 31440/2
|only if havequest(31440)
step
'Clear all of the trash in this room before fighting Jandice Barov.
|confirm
step
'_Jandice Barov_
.' _Wondrous Rapidity_ is a large cone attack in front of Jandice, make sure nobody is in front of her.
.' _Gravity Flux_ will create a gravity distortion field on the ground. If you step into this ability you will be knocked back as well as damaged heavily.
.' At _66%_ and _33%_ Jandice will cast _Phantasmal Images_. She will clone herself throughout the room, destroy the clones until she returns to battle.
.' While the clones are alive they will cast _Whirl of Illusion_, this does AoE damage to everybody in the group every second.
.from Jandice Barov##59184
|confirm |next
step
'Enter the Chamber of Summoning |goto 56.4,26.8 <5 |c
step
Go around the perimeter of this room and kill the Boneweavers. When all of them are dead, the boss wil initiate you.
.from Boneweaver##59193
|confirm
step
'_Rattlegore_
.' With each successful attack Rattlegore will gain a stack of _Rusting_. This increases his attack damage but lowers his attack and movement speeds. If the tank is getting hit too hard, kite Rattlegore around the room.
.' _Bone Spike_ will target a random party member and do massive damage to them. This can be prevented if every party member keeps _Bone Armor_ active.
.' _Bone Armor_ is granted by clicking on the _Bone Piles_ around the room.
.' Periodically a section of the room will be engulfed in _Soulflame_. Avoid the fire and be wary because it grows over time.
.from Rattlegore##59153
|confirm
step
'Enter the Chamber of Summoning |goto 40.3,26.6 <5 |c
step
goto 42.0,46.8
'When fighting the Flesh Horrors, aim for the Meat Grafts attached to their bodies. If all of the Grafts die then the Horrors will die with them.
.from Flesh Horror##59359+, Meat Graft##59982+
|confirm
step
goto 42.5,51.6
.click Forbidden Rites and other Rituals Necromantic
.' Destroy Forbidden Rites and other Rituals Necromantic |q 31440/3
|only if havequest(31440)
step
'Go down the steps and prepare to fight Lilian Voss. |goto 47.0,47.2 <5 |c
step
'_Lilian Voss_
'_Phase 1_
.' Lilian will _Shadow Shiv_ a random player after jumping onto them. When she latches onto you, run away from your allies to avoid AoE damage.
.' _Death's Grasp_ will pull every party member to Lilian and will be followed by _Dark Blaze_. _Dark Blaze_ causes every player to start leaving a trail of fire behind them, run to the outside of the room to trail your fire, this way the center is still open.
.' At _60%_ Phase 2 begins.
'_Phase 2_
.' Lilian's Soul will begin to attack you. She will periodically cast _Blazing Soul_ which does fire damage to all players.
.' For every melee attack Lilian lan ds she will unleash another _Blazing Soul_. You must kite her aroud.
.' _Fixate Anger_ will determine who is in charge of kiting. If she Fixates on your do not stop running.
.from Lilian's Soul##58791
'_Phase 3_
.' Phase 3 is a combination of Phases 1 and 2.
.' Be watchful of who Lilian's Soul is _Fixating_ on, this is the highest source of damage during the fight.
.' You will continued to get pulled in by _Death's Grasp_, be extra careful not to put a path of _Dark Blaze_ in somebodies way if they need to kite the Soul.
.from Lilian Voss##58722
|confirm
step
goto 54.4,56.6 |n
.' Go through this hallway.
step
'Clear the _Bored Students_ out of this room before you engage _Professor Slate_
|confirm
step
'_Professor Slate_
.' He casts _Toxic Potion_, which deals nature damage to everyone standing in the green pool that spawns. Run away to avoid being killed by this.
.' Occasionally he will use a _Fire Breath Potion_, which deals heavy fire damage to everyone in front of him.
.' At 30% health, he will use a _Potion of Brutish Force_, which increases his size and physical damage done by 50% for 2 minutes.
.from Professor Slate##59613
|confirm
step
goto Scholomance/2 47.1,73.1
.click The Dark Grimoire
.' Destroy The Dark Grimoire |q 31440/4
|only if havequest(31440)
step
goto 47.8,84.8 |n
'Go through this doorway and follow the stairs down. |goto Scholomance/3 |c
step
'_Darkmaster Gandling_
.' _Immolate_ will be used on a random party member, heal through this DoT.
.' Darkmaster Gandling will cast _Rise!_. This summons many _Failed Students_ that need to be killed. During this cast Darkmaster Gandling takes reduced damage.
.' Periodically a random party member will be teleported to one of Gandling's study rooms for a _Harsh Lesson_.
.' This party member must kill the _Expired Test Subjects_ and make their way back to the bosses room. If the healer gets trapped in one of these rooms, the _Fresh Test Subjects_ around the room will serve as useful bombs. Dispel the _Explosive Pain_ off of them to deal AoE damage to the Expired Test Subjects.
.kill Darkmaster Gandling##59080 |q 31447/1 |only if havequest(31447)
.from Darkmaster Gandling##59080 |only if not havequest(31447)
|confirm |only if not havequest(31447)
step
'Go back up the stairs. |goto Scholomance/3 49.1,21.5 <5 |c
step
goto Scholomance/3 49.2,21.2
.talk 64562
..turnin 31440
..turnin 31447
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Uldaman (Uld) 40-45",[[
dungeon 692
description This guide will walk you through the Uldaman dungeon.
achieveid 638
author support@zygorguides.com
step
label	"start"
goto Uldaman 64.0,72.2
.talk 46241
..accept 27682
step
goto 64.0,72.5
.talk 46236
..accept 27679
step
goto 64.0,73.0
.talk 46235
..accept 27681
step
map Uldaman
path loop off
path	65.5,77.3	61.8,80.5	56.8,75.3
path	56.7,85.1	58.6,94.0
.from Baelog##6906, Olaf##6908, Eric "The Swift"##6907
|confirm
step
map Uldaman
path loop off
path	58.9,86.8	53.4,81.3	52.7,72.2
.' Follow this path:
.from Revelosh##6910
|confirm
step
goto 45.4,73.5
.' Clear the trash in the room before using the Keystone.
.click the Keystone##1369
|confirm
step
goto 42.6,73.5
.from Ironaya##7228 |only if not havequest(27679)
.kill Ironaya##7228 |q 27679/1 |only if havequest(27679)
|confirm |only if not havequest(27679)
step
'Click the Complete Quest box in the top right corner.
..turnin 27679
step
'Click the Complete Quest box in the top right corner.
..accept 27680
step
map Uldaman
path loop off
path	41.6,66.2	36.0,63.8	35.1,56.4
path	31.1,56.3	28.6,61.4
.' Follow this path:
.from Obsidian Sentinel##7023
.get Obsidian Power Core |q 27682/1 |only if havequest(27682)
|confirm |only if not havequest(27682) |only if not havequest(27682)
step
map Uldaman
path loop off
path	33.0,54.5	37.8,58.0	43.6,55.1
path	48.9,54.1	47.6,46.1	47.7,41.3
.' Follow this path:
.from Ancient Stone Keeper##7206
.get Titan Power Core |q 27682/2 |only if havequest(27682)
|confirm |only if not havequest(27682) |only if not havequest(27682)
step
map Uldaman
path loop off
path	44.7,41.3	41.1,36.7	33.7,38.0
path	29.3,34.5	25.9,35.4
.' Follow this path:
.from Galgann Firehammer##7291 |only if not havequest(27681)
.kill Galgann Firehammer##7291 |q 27681/1 |only if havequest(27681)
|confirm |only if not havequest(27681)
step
map Uldaman
path loop off
path	29.3,34.7	32.5,28.0	27.9,21.1
path	23.8,27.6	21.2,24.4
.' Follow this path:
.from Grimlok##4854
|confirm
step
map Uldaman
path loop off
path	24.7,26.4	29.9,24.3	38.5,30.1
path	43.1,33.4	44.6,26.2	41.7,16.7
.' Follow this path:
.click Altar of the Keepers
.from 4 Stone Keeper##4857+
|confirm
step
goto Uldaman/2 58.7,61.4 |n
.click Temple Door##1370
.' Follow the Hallway down, click the Temple Door and go inside. |goto 57.6,58.1 <5 |noway |c
step
goto 56.0,53.1
.click Altar of Archaedas
.from Archaedas##2748 |only if not havequest(27680)
.kill Archaedas##2748 |q 27680/1 |only if havequest(27680)
|confirm |only if not havequest(27680)
step
'Click the Quest Complete box in the top right corner
..turnin 27680
step
goto 50.0,33.6
.click The Discs of Norgannon##131474
..accept 2278
step
goto 48.0,37.2
.talk 7172
'Ask him about the Earthen.
.' Learn the lore that the stone watcher has to offer. |q 2278/1
|only if havequest(2278)
step
goto 50.0,33.6
.click The Discs of Norgannon##131474
..turnin 2278
..accept 2280
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Uldaman 64.0,72.1
.talk 46241
..turnin 27682
step
goto 64.1,73.0
.talk 46235
..turnin 27681
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Dire Maul: Capital Gardens (DM) 41-46",[[
dungeon 699
description This guide will walk you through the Dire Maul: Capital Gardens dungeon.
author support@zygorguides.com
step
label	"start"
goto Dire Maul/2,87.3,52.6
.talk 44999
..accept 27109
step
goto Dire Maul/2,87.3,52.6
.talk 44999
..accept 27109
step
goto 90.6,44.9
.talk 44991
..accept 27112
..accept 27113
step
goto 73.2,53.8
.from Mana Remnant##11483+
.' Kill all the Remnants around the crystals to deactivate them.
|confirm
step
goto 32.1,71.6 |n
.' Enter this Hallway. |goto 32.1,71.6 <5 |noway |c
step
goto Dire Maul/3,30.6,45.8
.from Magister Kalendris##11487 |only if not havequest(27112)
.kill Magister Kalendris##11487 |q 27112/1 |only if havequest(27112)
|confirm |only if not havequest(27112)
step
goto 49.4,12.7 |n
.' Follow this ramp up. |goto 49.4,12.7 <5 |noway |c
step
goto Dire Maul/2,42.0,22.4
.from Mana Remnant##11483+
.' Kill all the Remnants around the crystals to deactivate them.
|confirm
step
goto 48.4,53.8
.talk 14358
..turnin 27113
step
goto 48.4,53.8
.talk 14358
..accept 27110
step
goto 39.5,83.4
.from Mana Remnant##11483+
.' Kill all the Remnants around the crystals to deactivate them.
|confirm
step
goto 21.7,75.8
.from Illyanna Ravenoak##11488 |only if not havequest(27112)
.kill Illyanna Ravenoak##11488 |q 27112/2 |only if havequest(27112)
|confirm |only if not havequest(27112)
step
.' Follow this ramp down. |goto 20.8,19.6 <5 |c
step
.' Go through this doorway. |goto Dire Maul/3 56.3,26.8 <5 |c
step
goto Dire Maul/2 34.6,53.3
.from Tendris Warpwood##11489 |only if not havequest(27109)
.kill Tendris Warpwood##11489 |q 27109/1 |only if havequest(27109)
|confirm |only if not havequest(27109)
step
goto 29.1,42.8 |n
.' Go through this doorway and down the Hallway. |goto Dire Maul/4 76.0,39.9 <5 |noway |c
step
goto 46.5,81.6
.from Mana Remnant##11483+, Arcane Aberration##11480+
.' Kill all the Remnants around the crystal to deactivate it.
|confirm
step
goto 44.7,34.1
.from Mana Remnant##11483+, Arcane Aberration##11480+
.' Kill all the Remnants around the crystal to deactivate it.
|confirm
step
goto 35.5,57.0
.from Immol'thar##11496 |only if not havequest(27110)
.kill Immol'thar##11496 |q 27110/1 |only if havequest(27110)
|confirm |only if not havequest(27110)
step
.' Go through this doorway. |goto 59.1,33.5 <5 |c
step
.' Go down this ramp. |goto 67.1,24.2 <5 |c
step
goto 60.4,23.5
.from Prince Tortheldrin##11486 |only if not havequest(27110)
.kill Prince Tortheldrin##11486 |q 27110/2 |only if havequest(27110)
|confirm |only if not havequest(27110)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Dire Maul/2,87.3,52.4
.talk 44999
..turnin 27109
step
goto 90.5,44.8
.talk 44991
..turnin 27112
step
goto 39.9,34.4 |n
.' Enter this room here. |goto 39.9,34.4 <5 |noway |c
step
goto Dire Maul/3,48.1,12.4 |n
.' Follow this ramp up. |goto 48.1,12.4 <5 |noway |c
step
goto Dire Maul/2,48.4,53.8
.talk 14358
..turnin 27110
step
goto Dire Maul/2,48.4,53.8
.talk 14358
..accept 27111
step
goto 20.8,19.6 |n
.' Follow this ramp down. |goto 20.8,19.6 <5 |noway |c
|only if havequest(27111)
step
goto Dire Maul/3,56.3,26.8 |n
.' Go through this doorway. |goto 56.3,26.8 <5 |noway |c
|only if havequest(27111)
step
goto Dire Maul/2,29.1,42.8 |n
.' Go through this doorway and down the Hallway. |goto Dire Maul/4 |noway |c
|only if havequest(27111)
step
goto 59.1,33.5 |n
.' Go through this doorway. |goto 59.1,33.5 <5 |noway |c
|only if havequest(27111)
step
goto 70.6,24.1
.click Treasure of the Shen'dralar##1387
..turnin 27111
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Razorfen Downs (RFD) 42-47",[[
dungeon 760
description This guide will walk you through the Razorfen Downs dungeon.
achieveid 636
author support@zygorguides.com
step
label	"start"
goto Razorfen Downs 24.6,23.7
.talk 44837
..accept 27009
..accept 27024
..accept 27029
step
'As you go through the dungeon, make sure to loot mobs for your quest item _Withered Tusk_.
|confirm
step
goto 31.0,18.2 |n
.' Follow the left route of the dungeon. |goto 31.0,18.2 <5 |noway |c
step
goto 59.6,28.0
.' Clear this room then click the Gong.
.' You will fight 2 small waves of spiders, then the boss will show up.
.click the Gong after each wave to summon the next group.
.from Tuten'kash##7355 |only if not havequest(27024)
.kill Tuten'kash##7355 |q 27024/3 |only if havequest(27024)
|confirm |only if not havequest(27024)
step
goto 68.9,37.9 |n
.' Follow the path to the left. |goto 68.9,37.9 <5 |noway |c
step
goto Razorfen Downs 78.3,16.6 |n
.click the Holding Pin
.talk 8516
..accept 27019
step
goto Razorfen Downs 78.3,16.6 |n
.click the Holding Pin
..turnin 27019
step
goto Razorfen Downs 78.3,16.6 |n
.click the Holding Pin
..accept 27022
step
goto 46.7,22.6
.' There will be about 6 waves of mobs, and a boss to kill here for 5 minutes
.' Protect Belnistrasz while he performs the ritual to shut down the idol |q 27022/1
|only if havequest(27022)
step
goto 46.7,22.6
click Brazier##759
..turnin 27022
step
goto 68.6,36.8 |n
.' Go up the ramp here. |goto Razorfen Downs 68.6,36.8 <5 |noway|c
step
goto 76.3,34.4 |n
.' Cross the bridge here. |goto 75.5,34.4 <5 |noway
step
goto 85.2,46.4
.from Mordresh Fire Eye##7357 |only if not havequest(27024)
.kill Mordresh Fire Eye##7357 |q 27024/2 |only if havequest(27024)
|confirm |only if not havequest(27024)
step
goto 75.1,40.2 |n
.' Take this ramp up and continue following the path. |goto 75.1,40.2 <5 |noway |c
step
goto Razorfen Downs 36.6,70.8
.from Glutton##8567 |only if not havequest(27024)
.kill Glutton##8567 |q 27024/1 |only if havequest(27024)
|confirm |only if not havequest(27024)
step
goto 44.6,59.4
.' Follow this path up and around
.from Amnennar the Coldbringer##7358 |only if not havequest(27009)
.kill Amnennar the Coldbringer##7358 |q 27009/1 |only if havequest(27009)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Razorfen Downs 24.6,23.7
.talk 44837
..turnin 27009
..turnin 27024
..turnin 27029
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Dire Maul: Gordock Commons (DM)  43-48",[[
dungeon 699
description This guide will walk you through the Dire Maul: Gordock Commons dungeon.
author support@zygorguides.com
step
label	"start"
.' Click here to do a Tribute run, which skips killing any bosses and gives you treasure and quests at the end. |confirm always |next "tribute" |or
.' or
.' Click here to run the dungeon normally |confirm always |next "normal" |or
step
label	"normal"
goto Dire Maul 71.7,92.2
.talk 45052
..accept 27128
step
goto 69.8,75.5
.from Guard Mol'dar##14326+
.' He will call out for help, so you may need to clear some of the area around him first.
|confirm
step
goto Dire Maul 62.2,66.0
.from Stomper Kreeg##14322 |tip Up the ramp, in the corner.
|confirm
step
goto 49.3,78.7
.click Fengus's Chest##41
.from Guard Fengus##14321 |tip He will walk around this area.
|confirm
step
goto 40.5,78.2 |n
.' Go through this doorway. |goto 40.5,78.2 <5 |noway |c
step
goto 26.5,58.8
.' He will call out for help, so you may need to clear some of the area around him first.
.from Guard Slip'kik##14323
|confirm
step
goto 23.3,66.7 |n
.' Follow this ramp up. |goto 23.3,66.7 <5 |noway |c
step
goto 29.0,65.7 |n
.' Go up the ramp through this doorway. |goto 29.0,65.7 <5 |noway |c
step
goto 31.9,50.8
.from Captain Kromcrush##14325
|confirm
step
goto 31.8,26.4
.from King Gordok##11501 |only if not havequest(27128)
.kill King Gordok##11501 |q 27128/1 |only if havequest(27128)
|confirm |only if not havequest(27128)
step
goto 31.7,27.4
.talk 14353
.' Tell him:
.' <I'm the new king? What are you talking about?
.' Talk to him a second time, about the Tribute, and he will spawn a Chest for you to loot as well.
.click Gordok Tribute##1387
|confirm
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm always |next "end"
step
label	"tribute"
goto Dire Maul 71.7,92.2
.talk 45052
..accept 27128
step
goto 57.7,85.4 |n
.' Follow this hallway to the left. |goto 57.7,85.4 <5 |noway |c
step
goto 40.5,78.2 |n
.' Follow the path, avoid Guard Fengus, _DO NOT_ kill him. Go through this doorway. |goto 40.5,78.2 <5 |noway |c
step
goto Dire Maul 28.6,55.3
.talk 14338
..accept 27119
step
goto 26.5,57.9
.click Broken Trap##5712
..accept 27118 |instant |repeatable
.' Wait for Slip'kik to get frozen before moving on |tip You may have to aggro him and walk over the trap.
|confirm
step
goto 23.7,55.6 |n
.' Up the ramp:
.click Ogre Tannin Basket##468
.collect Ogre Tannin##18240
.get Ogre Tannin |q 27119/1
|only if havequest(27119)
step
goto 28.6,55.3
.' Jump down or go down the ramp.
.talk 14338
..turnin 27119
..accept 27120 |instant |repeatable
step
goto 23.2,66.6 |n
.' Follow this ramp up. |goto 23.2,66.6 <5 |noway |c
step
goto 29.1,65.8 |n
.' Go through this doorway. |goto 29.1,65.8 <5 |noway |c
step
.' Before running down this hallway:
.' Put on your ogre disguise. |havebuff Interface\Icons\Ability_Rogue_Disguise |use Gordok Ogre Suit##18258
step
goto 31.8,26.4
.from King Gordok##11501 |only if not havequest(27128)
.kill King Gordok##11501 |q 27128/1 |only if havequest(27128)
.' Do not kill Cho'Rush the Observer
|confirm |only if not havequest(27128)
step
goto 31.7,27.4
.talk 14353
.' Tell him:
.' <I'm the new king? What are you talking about?
.' Talk to him a second time, about the Tribute, and he will spawn a Chest for you to loot as well.
.click Gordok Tribute##1387
|confirm
step
goto 31.8,50.0
.talk 14325
..accept 27124
step
goto 62.0,65.8
.talk 14322
..accept 27114 |instant
step
goto 69.9,75.5
.talk 14326
..turnin 27124
step
label	"end"
goto Dire Maul 71.7,92.3
.talk 45052
..turnin 27128
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Stratholme: Main Gate (Strat) 44-49",[[
dungeon 765
description This guide will walk you through the Stratholme: Main Gate dungeon.
author support@zygorguides.com
step
label	"start"
goto Stratholme 67.0,89.2
.talk 45323
..accept 27192
step
goto 65.3,87.2
.talk 45201
..accept 27185
step
goto 66.3,86.7
.talk 45200
..accept 27208
..accept 27305
..accept 27223
step
goto 54.5,70.8
.from Stratholme Courier##11082
.click Premium Siabi Tobacco##336
.from Fras Siabi##11058 |tip He will show up after you click the box.
.get Siabi's Premium Tobacco |q 27192/1
|only if havequest(27192)
step
goto 73.6,20.4
.kill The Unforgiven##10516 |q 27223/1 |only if havequest(27223)
|confirm |only if not havequest(27223)
step
goto Stratholme 50.1,17.4
.kill Timmy the Cruel##10808 |q 27223/2 |only if havequest(27223)
|confirm |only if not havequest(27223)
step
goto 32.0,34.7 |n
.' Go through this doorway. |goto 32.0,34.7 <5 |noway |c
step
goto 30.6,39.9
.from Commander Malor##11032
|confirm
step
goto 19.8,51.3 |n
.' Go through this doorway. |goto 19.8,51.3 <5 |noway |c
|only if havequest(27185)
step
goto 12.5,47.5 |n
.click Blacksmithing Plans##525
.kill Risen Hammersmith##11120 |q 27185/1
|only if havequest(27185)
step
goto 22.3,56.7 |n
.' Go through this doorway. |goto 22.3,56.7 <5 |noway |c
step
goto 27.4,75.2
.from Instructor Galford##10811
.click Unfinished Painting##4733 |tip This is on the wall next to you.
.get Of Love and Family |q 27305/1
|only if havequest(27305)
step
goto 19.3,83.1
.from Balnazzar##10813 |only if not havequest(27208)
.kill Balnazzar |q 27208/1 |only if havequest(27208)
step
goto 21.6,80.8
.talk 45200
..turnin 27208
..turnin 27305
..turnin 27223
step
goto 21.6,79.8
.talk 45323
..turnin 27192
step
goto 24.2,76.3
.talk 45201
..turnin 27185
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Zul'Farrak (ZF) 46-51",[[
dungeon 686
description
achieveid 639
step
label "start"
goto Zul'Farrak 56.7,89.2
.talk 40712
..accept 27068
..accept 27071
step
goto 58.2,89.1
.talk 7407
..accept 27070
step
goto 58.3,88.7
.talk 44929
..accept 27076
step
map Zul'Farrak
path follow strict; loop off
path 57.5,79.9	59.1,68.2	57.3,55.7
path 53.0,38.9	59.8,42.0	55.6,30.7
'Follow the path. |goto 55.5,30.7 <5 |noway|c
step
goto 54.8,29.3
..kill Theka the Martyr##7272 |q 27071/1
|only if havequest(27071)
step
goto 63.3,27.9
.' Clear the trash in this area.
.from Sandfury Shadowhunter##7246+, Sul'lithuz Sandcrawler##8095+, Sandfury Witch Doctor##5650+
|confirm
step
goto Zul'Farrak 66.0,26.0 |tip Moving to this location will cause the boss to chase your party until he aggros.
.from Antu'sul##8127 |only if not havequest(27071)
.kill Antu'sul##8127 |q 27071/3 |only if havequest(27071)
.from Servant of Antu'sul##8156+, Sul'lithuz Broodling##8138+
.' He will summon Sul'lithuz Broodlings to aid him in battle. Tanks try to pick them up quickly.
|confirm |only if not havequest(27071)
step
goto 44.1,15.8
'Witch Doctore Zum'rah will be friendly until you approach him.
.from Witch Doctor Zum'rah##7271 |only if not havequest(27071)
..kill Witch Doctor Zum'rah##7271 |q 27071/2 |only if havequest(27071)
|confirm |only if not havequest(27071)
step
goto 30.8,15.9
.from Sandfury Witch Doctor##5650+, Sandfury Shadowhunter##7246+
.' Clear the trash around the area.
|confirm
step
goto 25.1,17.9
.from Sandfury Executioner##7274+
|confirm
step
'Once you kill the Executioner, click the cages behind him.
.click Troll Cage##1787
|confirm
step
'There will be an event when you release the prisoners.
.from Sandfury Drudge##7788+, Sandfury Slave##7787+, Sandfury Cretin##7789+
.' Defend Sergeant Bly from the attacking trolls.
.' Eventually Shadowpriest Sezz'zis will appear.
.from Shadowpriest Sezz'ziz##7275 |only if not havequest(27076)
..kill Shadowpriest Sezz'ziz##7275 |q 27076/2 |only if havequest(27076)
|confirm |only if not havequest(27076)
step
goto Zul'Farrak 30.5,18.1
.talk 7604
.' Tell him you're tired of taking orders from him.
.from Sergeant Bly##7604, Murta Grimgut##7608, Raven##7605, Oro Eyegouge##7606
|confirm
step
goto 43.5,34.6
.from Ruuzlu##7797,  Chief Ukorz Sandscalp##7267 |only if not havequest(27071) or not havequest(27068)
..kill Ruuzlu |q 27071/4 |only if havequest(27071)
..kill Chief Ukorz Sandscalp |q 27068/1 |only if havequest(27068)
|confirm |only if not havequest(27071) or not havequest(27068)
step
goto 29.7,38.8
.from Hydromancer Velratha##7795 |tip She patrols around the room a bit, but often stops here.
..get Tiara of the Deep |q 27070/1
|only if havequest(27070)
step
goto Zul'Farrak 32.9,43.6 |n
.' Kill trash while making your way to the _Gong of Zul'Farrak_. |goto Zul'Farrak 32.9,43.6 <5 |noway|c
step
goto 32.8,43.5
.click Gong of Zul'Farrak##1827 |tip This will cause Gahz'rilla to spawn.
.from Gahz'rilla##7273
..get Gahz'rilla's Electrified Scale |q 27070/2 |only if havequest(27070)
|confirm
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Zul'Farrak 56.7,89.2
.talk 40712
..turnin 27068
..turnin 27071
step
goto 58.2,89.1
.talk 7407
..turnin 27070
step
goto 58.3,88.7
.talk 44929
..turnin 27076
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Stratholme: Service Entrance (Strat) 48-53",[[
dungeon 765
description This guide will walk you through the Stratholme: Service Entrance dungeon.
achieveid 646
author support@zygorguides.com
step
label	"start"
goto Stratholme 67.8,82.4
.talk 45328
..accept 27352
step
goto Stratholme/2 67.1,72.6
.talk 45330
..accept 27359
step
goto 67.9,72.1
.talk 45206
..accept 27228
step
goto 68.0,72.7
.talk 45329
..accept 27227
step
goto 68.6,73.1
.talk 45331
..accept 27230
step
goto 63.4,66.2 |n
.click Gauntlet Gate##3614
.' Go through this gate.  |goto 63.2,65.4 <5 |noway |c
step
goto 62.4,57.9 |n
.click Gauntlet Gate##3614
.' Go through this gate. |goto 62.9,57.5 <5 |noway |c
step
.' While running through this area:
.from Wailing Banshee##10464+
.get 4 Banshee Essence |q 27359/1 |future
|confirm
|only if havequest(27359)
step
goto 68.6,54.6
.click Supply Crate##336 |tip They will be along the walls throughout the entire instance.
.get Stratholme Holy Water |q 27352/1
|only if havequest(27352)
step
goto 72.7,52.4
.click Blacksmithing Plans##525
.kill Black Guard Swordsmith##11121 |q 27230/1
|only if havequest(27230)
step
goto 74.8,46.8
.from Baroness Anastari##10436
.' After you kill this boss, make sure to go inside the doors and kill the Acolyte's.
.from Thuzadin Acolyte##10399+
|confirm
step
goto 68.1,20.0
.from Maleki the Pallid##10438
.' After you kill this boss, make sure to go inside the doors and kill the Acolyte's.
.from Thuzadin Acolyte##10399+
|confirm
step
goto 57.0,15.1
.from Magistrate Barthilas##10435
|confirm
step
goto 56.5,46.8
.from Nerub'enkan##10437
.' After you kill this boss, make sure to go inside the doors and kill the Acolyte's.
.from Thuzadin Acolyte##10399+
|confirm
step
goto 66.5,49.3
.from Wailing Banshee##10464+
.get 4 Banshee Essence |q 27359/1
|only if havequest(27359)
step
goto 47.3,19.9
.' Clear this room of Abominations.
|confirm
step
goto 47.3,19.9
.from Ramstein the Gorger##10439 |only if not havequest(27228)
.kill Ramstein the Gorger##10439 |q 27228/1 |only if havequest(27228)
|confirm |only if not havequest(27228)
step
.' Before you kill the next boss, you will have a wave a mobs come from the left gate.
.' Then, you will have a wave of elite mobs come from the doorway.
.' Clear these 2 waves.
|confirm
step
goto 37.4,19.9
.from Lord Aurius Rivendare##45412 |only if not havequest(27227)
.kill Lord Aurius Rivendare##45412 |q 27227/1 |only if havequest(27227)
step
goto 44.8,20.4
.talk 45329
..turnin 27227
step
goto 43.9,14.1
.talk 45206
..turnin 27228
step
goto 42.5,12.7
.talk 45330
..turnin 27359
step
goto 49.8,18.4
.talk 45331
..turnin 27230
step
goto 49.7,22.4
.talk 45328
..turnin 27352
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Sunken Temple (ST) 52-57",[[
description This guide will walk you through the Sunken Temple dungeon.
achieveid 641
step
goto The Temple of Atal'Hakkar 50.0,17.2
.talk 46077
..accept 27604
..accept 27605
..accept 27633
step
goto 76.2,54.9
' Clear the trash here. The mobs will fear you around the room, so be careful.
|confirm
step
goto 76.1,38.7
.from Jammal'an the Prophet##5710
..get Head of Jammal'an |q 27604/1 |only if havequest(27604)
|confirm |only if not havequest(27604)
step
goto 24.3,45.7
.' After clearing the trash, use your Egg of Hakkar here. |use Egg of Hakkar##10465
..from Avatar of Hakkar##8443 |only if not havequest(27633)
..kill Avatar of Hakkar##8443 |q 27633/1 |only if havequest(27633)
|confirm |only if not havequest(27633)
step
goto 49.4,81.3
.from Hazzas##5722, Morphaz##5719
|confirm
step
goto 68.5,87.7
.from Shade of Eranikus##5709 |only if not havequest(27605)
.kill Shade of Eranikus##5709 |q 27605/1 |only if havequest(27605)
'Note that you will not be able to attack him if you haven't defeated the other bosses.
step
goto The Temple of Atal'Hakkar 50.0,17.2
.talk 46077
..turnin 27604
..turnin 27605
..turnin 27633
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Blackrock Depths: Detention Block (BRD) 53-58",[[
dungeon 704
description This guide will walk you through the Blackrock Depths: Detention Block dungeon.
achieveid 642
author support@zygorguides.com
step
label "start"
goto Blackrock Depths 36.1,77.3
.talk 45821
..accept 27579
step
goto 37.3,80.1
.talk 45849
..accept 27603
step
goto 36.7,74.6
.talk 45839
..accept 27582
step
goto 36.1,73.7 |n
.' Go through this doorway. |goto 36.1,73.7 <5 |noway |c
step
goto 24.2,51.8
.from Bael'Gar##9016 |only if not havequest(27603)
.kill Bael'Gar##9016 |q 27603/1 |only if havequest(27603)
|confirm |only if not havequest(27603)
step
goto Blackrock Depths 56.6,31.1
.from Lord Incendius##9017
|confirm
step
goto 59.8,26.2 |n
.' Go through this door and up the ramp. |goto 59.8,26.2 <5 |noway |c
step
goto Blackrock Depths/2,64.2,58.2
.from Fineous Darkvire##9056+
|confirm
step
goto Blackrock Depths/2,59.8,60.0 |n
.' Enter the next room here. |goto Blackrock Depths/2 59.8,60.0 <5 |noway |c
step
goto Blackrock Depths 59.0,35.0 |n
.' Enter this room. |goto 59.0,35.0 <5 |noway |c
step
goto Blackrock Depths/2 60.7,67.4
.from Warder Stilgiss##9041
|confirm
step
goto 58.6,64.1 |n
.' Leave this room and go to the left. |goto 58.6,64.1 <5
step
goto Blackrock Depths 54.8,36.0 |n
.' Go through this doorway. |goto 54.8,36.0 <5 |noway |c
step
goto Blackrock Depths/2,54.3,69.3 |n
.click Abandoned Mole Machine##8005
.' <Hit the button labeled 'Entrance'> |goto Blackrock Depths 35.9,79.2 <5 |noway |c
step
goto 37.3,80.1
.talk 45849
..turnin 27603
step
goto 37.7,66.0 |n
.' Go through this doorway. |goto 37.7,66.0 <5 |noway |c
step
goto 40.9,67.1
.talk 45820
..turnin 27582
step
goto 40.9,67.1
.talk 45820
..accept 27585
step
goto Blackrock Depths 44.3,77.8
.talk 45818
..turnin 27579
step
goto Blackrock Depths 44.3,77.8
.talk 45818
..accept 27581
step
goto 47.5,93.4
.from High Interrogator Gerstahn##9018 |only if not havequest(27581)
.kill High Interrogator Gerstahn##9018 |q 27581/1 |only if havequest(27581)
|confirm |only if not havequest(27581)
step
goto Blackrock Depths 44.3,77.8
.talk 45818
..turnin 27581
step
goto 51.4,62.1
.from Houndmaster Grebmar##9319
|confirm
step
goto Blackrock Depths 55.2,58.9
.from Lord Roccor##9025 |tip He walks around this area. Some searching may be required.
|confirm
step
goto Blackrock Depths 51.4,67.5 |n
.' Enter the Ring of the Law here. |goto Blackrock Depths 51.4,67.5 <5 |noway |c
step
goto 50.5,63.0
.' Wait for the gates to be opened and kill all the mobs.
.' There will be a couple waves of mobs and then a boss to kill.
|confirm
step
goto 48.0,63.9 |n
.' Enter the Hallway here. |goto 48.0,63.9 <5 |noway |c
step
goto Blackrock Depths/2 41.4,88.3
.click The Shadowforge Lock##2851
|confirm
step
goto 45.0,87.1 |n
.' Go back up the stairs and follow this hallway. |goto 45.0,87.1 <5 |noway |c
step
goto 36.6,84.4
.from General Angerforge##9033 |only if not havequest(27585)
.kill General Angerforge##9033 |q 27585/1 |only if havequest(27585)
|confirm |only if not havequest(27585)
step
goto 36.8,64.8
.from Golem Lord Argelmach##8983 |only if not havequest(27585)
.kill Golem Lord Argelmach##8983 |q 27585/2 |only if havequest(27585)
|confirm |only if not havequest(27585)
step
goto 41.2,68.3 |n
.' Follow this Hallway. |goto Blackrock Depths 41.2,68.3 |noway |c
step
goto Blackrock Depths/2,46.9,66.2
.talk 45817
..turnin 27585
step
goto Blackrock Depths/2,46.9,66.2
.talk 45817
..accept 27589
step
goto 47.9,58.3
.click 3 Thunderbrew Lager Keg##4631+
.' A group of Dwarves will enter the room to fight, be ready for them.
.from Hurley Blackbreath##9537+, Blackbreath Crony##9541+, Blackbreath Crony##9541+
|confirm
|only if not havequest(27589)
step
goto 47.9,58.3
.click 3 Thunderbrew Lager Keg##4631+
.' A group of Dwarves will enter the room to fight, be ready for them.
.from Blackbreath Crony##9541, Blackbreath Crony##9541
.kill Hurley Blackbreath##9537 |q 27589/1
|only if havequest(27589)
step
goto 49.1,61.8
.talk 9543
.' Tell him:
.' <You're good for nothing, Ribbly. It's time to pay for your wickedness!
.from Ribbly Screwspigot##9543 |only if not havequest(27589)
.kill Ribbly Screwspigot##9543 |q 27589/3 |only if havequest(27589)
|confirm |only if not havequest(27589)
step
goto 49.6,61.3
.from Plugger Spazzring##9499
.' This will bring the guards in through the back door and all the Bar Patrons will start attacking, be ready to fight.
|confirm
|only if not havequest(27589)
step
goto 49.6,61.3
.kill Plugger Spazzring##9499 |q 27589/2
.' This will bring the guards in through the back door and all the Bar Patrons will start attacking, be ready to fight.
|only if havequest(27589)
step
goto 53.0,62.8
.from Phalanx##9502
|confirm
step
goto Blackrock Depths,53.9,27.0 |n
.' Go through the Grim Guzzler's back doorway and down the small ramp to the left. |goto Blackrock Depths 53.9,27.0 <5 |noway |c
step
goto Blackrock Depths/2,50.8,52.7
.talk 45824
..turnin 27589
step
goto Blackrock Depths/2,50.8,52.7
.talk 45824
..accept 27591
step
goto 54.0,48.6
.from Ambassador Flamelash##9156 |only if not havequest(27591)
.kill Ambassador Flamelash##9156 |q 27591/1 |only if havequest(27591)
|confirm |only if not havequest(27591)
step
goto 49.5,42.1 |n
.' Follow the hallway and go through the next doorway. |goto 49.5,42.1 <5 |noway |c
step
goto 56.7,21.9
.talk 9039
.' Tell him:
.' <Your bondage is at an end, Doom'rel. I challenge you!>
.' Once you talk to him, you will have to fight Dwarves before you will fight Doom'rel
.from Vile'rel##9036, Hate'rel##9034, Gloom'rel##9037, Anger'rel##9035
.from Doom'rel##9039 |only if not havequest(27591)
.kill Doom'rel##9039 |q 27591/2 |only if havequest(27591)
|confirm |only if not havequest(27591)
step
goto 56.9,23.2
.click Chest of The Seven##1387
.' Loot the Armor and Weapons from the Chest.
|confirm
step
goto 58.1,22.3
.talk 45840
..turnin 27591
step
goto 58.1,22.3
.talk 45840
..accept 27593
step
goto 60.7,18.8 |n
.' Go through this big Doorway. |goto 60.7,18.8 |noway |c
step
'All around this room look for Flame Keepers
.from Shadowforge Flame Keeper##9956+
.collect 2 Shadowforge Torch##11885
step
goto 71.8,16.7
.' Use the Torches in your bags to light the Shadowforge Brazier.
.click Shadowforge Brazier##3411
|confirm
step
goto 71.8,7.0
.' Use the Torches in your bags to light the Shadowforge Brazier.
.click Shadowforge Brazier##3411
|confirm
step
goto 74.6,11.9 |n
.' Enter the hallway here. |goto 74.6,11.9 |noway |c
step
goto 82.3,12.0
.from Magmus##9938
|confirm
step
goto 93.3,12.0
.from Emperor Dagran Thaurissan##9019 |only if not havequest(27593)
.kill Emperor Dagran Thaurissan##9019 |q 27593/1 |only if havequest(27593)
step
goto 74.6,11.9 |n
.' Go back through these doors. |goto 74.6,11.9 |noway |c
step
goto 58.1,22.3
.talk 45840
..turnin 27593
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Lower Blackrock Spire (LBRS) 57-60",[[
dungeon 721
description This guide will walk you through the Lower Blackrock Spire dungeon.
achieveid 643
author support@zygorguides.com
step
label	"start"
goto Blackrock Spire/4 30.5,39.8
.talk 10299
..accept 27440
step
goto 37.9,40.1 |n
.' Go through this doorway below the ramp. |goto 37.9,40.1 <5 |noway |c
step
goto 39.7,48.3 |n
.' Follow this winding hallway. |goto 39.7,48.3 <5 |noway |c
step
goto 49.0,41.5 |n
.' Follow this ramp down. |goto Blackrock Spire/3 |noway |c
step
map Blackrock Spire/3
path follow loose; loop off
path	50.0,40.7	59.2,42.8	66.5,44.6
path	65.0,57.8	57.1,57.7	36.1,55.3
.' Follow the path
.from Highlord Omokk##9196 |only if not havequest(27440)
.kill Highlord Omokk##9196 |q 27440/2 |only if havequest(27440)
|confirm |only if not havequest(27440)
step
goto 56.6,57.8 |n
.' Go out this doorway to the left. |goto 56.6,57.8 <5 |noway |c
step
goto 54.8,51.2 |n
.' Follow this ramp down and to the left. |goto 54.8,51.2 <5 |noway |c
step
goto Blackrock Spire/2,55.4,61.7 |n
.' Follow this ramp up. |goto Blackrock Spire/2 55.4,61.7 <5 |noway |c
step
goto 55.6,71.8
.from Shadow Hunter Vosh'gajin##9236
|confirm
step
map Blackrock Spire/2
path follow loose; loop off
path	56.8,58.2	59.6,59.1	59.4,62.5
.' Follow this path all the way down. |goto 59.4,62.5 <5 |noway |c
step
goto Blackrock Spire,53.0,52.9
.from War Master Voone##9237 |only if not havequest(27440)
.kill War Master Voone##9237 |q 27440/1 |only if havequest(27440)
|confirm |only if not havequest(27440)
step
goto 66.0,50.9 |n
.' Go through this doorway to the left. |goto Blackrock Spire/3 |noway |c
step
goto 57.0,47.9 |n
.' Go through the path with the rocks. |goto Blackrock Spire |noway |c
step
map Blackrock Spire
path follow loose; loop off
path	56.4,48.0	51.3,49.9	48.4,62.7
path	55.8,70.2	64.6,70.7
.' Follow this winding path. |goto Blackrock Spire/2 |noway |c
step
goto 62.5,76.0
.from Mother Smolderweb##10596
|confirm
step
goto 50.7,74.4 |n
.' Go through this doorway. |goto Blackrock Spire/3 |noway |c
step
goto 45.7,62.8 |n
.' Follow this ramp up and to the left. |goto 45.7,62.8 <5 |noway |c
step
goto Blackrock Spire/4 42.8,73.9 |n
.' Go through this doorway. |goto 42.8,73.9 <5 |noway |c
step
goto Blackrock Spire/5,37.7,85.4
.from Halycon##10220
.' After you kill Halycon, a white wolf will come in the door to fight.
.from Gizrul the Slavener##10268
|confirm
step
goto 40.0,78.2 |n
.' Follow this ramp up. |goto 40.0,78.2 <5 |noway |c
step
goto 40.6,60.4 |n
.' Continue up this ramp. |goto 40.6,60.4 <5 |noway |c
step
goto Blackrock Spire/6 56.1,55.6
.from Overlord Wyrmthalak##9568 |only if not havequest(27440)
.kill Overlord Wyrmthalak##9568 |q 27440/3 |only if havequest(27440)
|confirm
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Blackrock Spire/4,30.5,39.6
.talk 10299
..turnin 27440
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Classic Dungeons\\Upper Blackrock Spire (UBRS) 57-60",[[
dungeon 721
description This guide will walk you through the Upper Blackrock Spire dungeon.
achieveid 1307
author support@zygorguides.com
step
label	"start"
goto Blackrock Spire/4 30.5,39.8
.talk 10299
..accept 27444
..accept 27445
step
goto 37.1,33.1 |n
.' Go up this ramp and through the doorway on the left. |goto 37.1,33.1 <5 |noway |c
step
goto Blackrock Spire/5,33.4,15.0
.from Blackhand Dreadweaver##9817+, Blackhand Summoner##9818+, Blackhand Veteran##9819+, Rage Talon Dragonspawn##9096+
.' You will need to clear this entire room before you can continue.
.' Go through this doorway when you are finished. |goto 33.2,13.7 <5 |noway |c
step
goto 30.4,15.1 |n
.' Follow the ramp up. |goto 30.4,15.1 <5 |noway |c
step
goto Blackrock Spire/6,32.9,27.0
.click Blackrock Altar##4076
.' This will force the Blachand mobs to attack you. Once you fight them, you will have about 30 seconds, then you will fight the boss.
.from Pyroguard Emberseer##9816, Blackhand Incarcerator##10316+
|confirm
step
goto 30.4,31.9 |n
.' Go through this doorway. |goto 30.4,31.9 <5 |noway |c
step
goto 38.9,38.6
.click Father Flame##1667
.' Loot the items inside.
.' Stay and kill the mobs that come to attack you in waves of 2.
.from Rookery Guardian##10258+, Rookery Hatcher##10683+
.' The boss will come out shortly.
.from Solakar Flamewreath##10264
|confirm
step
goto 29.6,41.6 |n
.' Follow this path up. |goto 29.6,41.6 <5 |noway |c
step
goto Blackrock Spire/7,30.5,32.4 |n
.' Follow this ramp up and to the right. |goto 30.5,32.4 <5 |noway |c
step
goto 29.1,26.4
.from Goraluk Anvilcrack##10899
|confirm
step
goto 42.4,30.1 |n
.' Follow this hallway and turn left. |goto 42.4,30.1 <5 |noway |c
step
goto 48.6,33.4 |n
.' Enter the Stadium here. |goto 48.6,33.4 <5 |noway |c
step
goto 48.6,20.0 |n
.' Run here but stay out of the gate.
.' Fight the waves of enemies until the boss shows up, then kill him.
.from Warchief Rend Blackhand##10429 |only if not havequest(10429)
.kill Warchief Rend Blackhand##10429 |q 27444/1 |only if havequest(27444)
|confirm |only if not havequest(10429)
step
goto 50.4,35.6 |n
.' Follow this ramp up and to the right. |goto 50.4,35.6 <5 |noway |c
step
goto 58.9,47.0 |n
.' Jump over these fires and run to the back corner. |goto 58.9,47.0 <5 |noway |c
step
goto 63.5,36.3
.from The Beast##10430
|confirm
step
goto 43.2,45.9 |n
.' Go through this doorway. |goto 43.2,45.9 <5 |noway |c
step
goto 33.5,47.5
.' Clear this room from enemies, the boss can knock your group into them during the fight.
.from General Drakkisath##10363 |only if not havequest(27445)
.kill General Drakkisath##10363 |q 27445/1 |only if havequest(27445)
|confirm |only if not havequest(27445)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Blackrock Spire/4,30.5,39.7
.talk 10299
..turnin 27444
..turnin 27445
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Hellfire Citadel: Hellfire Ramparts (Ramps) 59-63",[[
dungeon 797
description This guide will walk you through the Hellfire Citadel: Hellfire Ramparts dungeon.
achieveid 647
author support@zygorguides.com
step
label	"entrance"
goto Hellfire Peninsula 47.6,53.6 |n
.' Enter Hellfire Ramparts here. |goto Hellfire Ramparts |noway |c
step
label	"start"
goto Hellfire Ramparts 48.5,68.1
.talk 54606
..accept 29527
..accept 29530
..accept 29593
step
goto 45.8,50.8
.click Hellfire Supplies##7001
.get 1 Hellfire Supplies |q 29593/1
|only if havequest(29593)
step
goto 51.7,45.7
.click Hellfire Supplies##7001
.get 2 Hellfire Supplies |q 29593/1
|only if havequest(29593)
step
goto 66.8,52.0
.click Hellfire Supplies##7001
.get 3 Hellfire Supplies |q 29593/1
|only if havequest(29593)
step
goto Hellfire Ramparts 76.9,34.4
.from Watchkeeper Gargolmar##17306
.get Gargolmar's Hand |q 29527/1 |only if havequest(29527)
|confirm |only if not havequest(29527)
step
goto 39.3,21.2
.from Omor the Unscarred##17308
.get Omor's Hoof |q 29530/1 |only if havequest(29530)
|confirm |only if not havequest(29530)
step
goto 36.2,79.3
.from Nazan##17307
.get Nazan's Head |q 29527/2 |only if havequest(29527)
|confirm |only if not havequest(29527)
step
goto 31.3,86.4
.click Reinforced Fel Iron Chest##5744
.' Loot your items for clearing this dungeon
|confirm |only if havequest(29527) or havequest(29530) or havequest(29593)
step
goto 51.1,63.8 |n
.' Jump down here to turn in your quests. |goto 49.1,67.3 <5 |noway |c
step
goto Hellfire Ramparts 48.5,68.1
.talk 54606
..turnin 29527
..turnin 29530
..turnin 29593
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Hellfire Citadel: The Blood Furnace (BF) 60-64",[[
dungeon 725
description This guide will walk you through the Hellfire Citadel dungeon.
achieveid 648
author support@zygorguides.com
step
label	"start"
goto The Blood Furnace 51.4,90.0
.talk 54636
..accept 29535
..accept 29536
..accept 29537
step
'You can collect Blood Vials from all Orcs in this instance
.from Laughing Skull Enforcer##17370+, Shadowmoon Adept##17397+, Laughing Skull Rogue##17491+, Shadowmoon Warlock##17371+
..collect Fel Orc Blood Vial##23894 |n
|confirm
|only if havequest(29535)
step
goto The Blood Furnace 36.6,40.7
.from The Maker##17381
.get Fel Infusion Rod |q 29535/2 |only if havequest(29535)
|confirm |only if not havequest(29535)
step
goto 43.4,21.9
.click Cell Door Lever##6899
.' This will start the boss fight. You will have 4 cell doors open and have to fight Orcs until all 4 waves are complete.
.' Once you have killed the Orcs, the Boss will come in.
.' Try not to stand in the greem mist from the boss, this will deal lots of damage to you.
.from Broggok##17380
|confirm
step
'Make sure you have collected Fel Orc Blood.
.get 10 Fel Orc Blood Vial |q 29535/1
|only if havequest(29535)
step
goto The Blood Furnace 64.3,41.4
.'Investigate The Blood Furnace. |q 29536/1
|only if havequest(29536)
step
goto 59.2,42.5
.from Keli'dan the Breaker##17377 |only if not havequest(29537)
.kill Keli'dan the Breaker##17377 |q 29537/1 |only if havequest(29537)
|confirm
step
goto 62.5,50.9 |n
.' Follow this hallway. |goto 62.5,50.9 <5 |noway |c
step
goto 51.4,90.1
.talk 54636
..turnin 29535
..turnin 29536
..turnin 29537
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Coilfang Reservoir: Slave Pens (SP) 61-65",[[
dungeon 728
description This guide will walk you through the Coilfang Reservoir: Slave Pens dungeon.
achieveid 649
author support@zygorguides.com
step
label	"start"
goto The Slave Pens 18.9,11.3
.talk 54667
..accept 29563
..accept 29565
step
goto 17.9,12.1
.talk 54668
..accept 29564
step
goto 25.0,51.2 |n
.' Follow the path and go straight through this main room. |goto 25.0,51.2 <5 |noway |c
step
goto 40.2,35.5 |n
.' Follow this hallway. |goto 40.2,35.5 <5 |noway |c
step
goto 48.8,24.3
.' You can either stand back and pull Mennu from the ramp, or clear the enemies first so they won't aggro.
.from Mennu the Betrayer##17941 |only if not havequest(29564)
.kill Mennu the Betrayer##17941 |q 29564/1 |only if havequest(29564)
|confirm |only if not havequest(17941)
step
goto 48.9,83.4
.talk 17890
.' Discover Weeder Greenthumb |q 29563/2
|only if havequest(29563)
step
goto 57.3,45.4
.from Rokmar the Crackler##17991
.' Rokmar will put a bleed affect on you. This will not go away until you are fully healed to 100%, so bring some food or a healer.
.get The Invader's Claw |q 29565/1 |only if havequest(29565)
|confirm |only if not havequest(29565)
step
goto 74.9,51.4 |n
.' Follow this path up. |goto 74.9,51.4 <5 |noway |c
step
goto 95.7,66.2
.talk 17893
.' Discover Naturalist Bite. |q 29563/1
|only if havequest(29563)
step
goto 95.7,66.2
.' Be ready to fight a couple mobs
.from Coilfang Champion##17957+, Coilfang Enchantress##17961+
.talk 17893
.' Tell him
.' <Naturalist, please grant me your boon |havebuff Interface\Icons\Ability_Druid_FerociousBite
step
goto The Slave Pens 86.2,67.9
.from Quagmirran##17942
.get The Slave Master's Eye |q 29565/2 |only if havequest(29565)
|confirm |only if not havequest(29565)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto The Slave Pens 17.9,12.2
.talk 54668
..turnin 29564
step
goto 18.8,11.3
.talk 54667
..turnin 29563
..turnin 29565
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Coilfang Reservoir: Underbog (UB) 62-66",[[
dungeon 726
description This guide will walk you through the Coilfang Reservoir: Underbog dungeon.
achieveid 650
author support@zygorguides.com
step
label	"start"
goto The Underbog 29.5,64.7
.talk 54678
..accept 29568
step
goto 30.9,65.3
.talk 54675
..accept 29570
step
goto 31.5,65.5
.talk 54674
..accept 29567
..accept 29691
step
'As you go through the dungeon, look for Sanguine Hibiscus
.click Sanguine Hibiscus##5031
.collect 5 Sanguine Hibiscus##24246 |n
|confirm
|only if havequest(29691)
step
goto 42.5,62.0 |n
.' Keep following this straight path. |goto 42.5,62.0 <5 |noway |c
step
goto 57.4,82.9 |n
.' Follow this path up and around. |goto 57.4,82.9 <5 |noway |c
step
goto 70.0,90.0
.' He will summon mushrooms, these will explode to deal damage to player, and heal the boss.
.' Keep moving him so that doesn't happen.
.from Hungarfen##17770 |only if not havequest(29568)
.kill Hungarfen##17770 |q 29568/1 |only if havequest(29568)
|confirm |only if not havequest(29568)
step
goto 77.2,43.8
.' Follow the path around until you get here
.from Ghaz'an##18105
|confirm
step
goto 78.9,24.5 |n
.' Jump down in the water here.  |goto 78.9,24.5 <5 |noway |c
step
goto 79.6,15.2 |n
.' Swim across the water and jump down here. |goto 79.6,15.2 <5 |noway |c
step
goto 67.4,21.3
.talk 17885
.' Discover Earthbinder Rayge. |q 29570/1
|only if havequest(29570)
step
goto 67.0,14.0 |n
.' Follow this path down and to the right. |goto 67.0,14.0 <5 |noway |c
step
goto 41.5,25.4
.from Swamplord Musel'ek##17826, Claw##17827
.talk 17827 |only if havequest(29570)
.' Discover Windcaller Claw |q 29570/2 |only if havequest(29570)
|confirm |only if not havequest(29570)
step
goto The Underbog 25.9,45.5
.from The Black Stalker##17882
.get Black Stalker's Brain |q 29567/1 |only if havequest(29567)
|confirm |only if not havequest(29567)
step
'Make sure you have picked up 5 Sanguine Hibiscus:
.click Sanguine Hibiscus##5031
.get 5 Sanguine Hibiscus##24246 |q 29691/1
|only if havequest(29691)
step
goto The Underbog 29.5,64.5
.talk 54678
..turnin 29568
step
goto 30.8,65.3
.talk 54675
..turnin 29570
step
goto 31.6,65.5
.talk 54674
..turnin 29567
..turnin 29691
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Auchindoun: Mana-Tombs (MT) 63-67",[[
dungeon 732
description This guide will walk you through the Auchindoun: Mana-Tombs dungeon.
achieveid 651
author support@zygorguides.com
step
label	"start"
goto Mana-Tombs 32.7,19.4
.talk 54694
..accept 29574
step
goto 34.4,19.4
.talk 54692
..accept 29573
..accept 29575
step
goto 48.2,27.7
.from Pandemonius##18341
.get Pandemonius' Essence |q 29574/1 |only if havequest(29574)
|confirm |only if not havequest(29574)
step
goto 61.3,74.0
.from Tavarok##18343
.get Tavarok's Heart |q 29574/2 |only if havequest(29574)
|confirm |only if not havequest(29574)
step
goto 32.4,50.7
.from Nexus-Prince Shaffar##18344
.get Shaffar's Wrappings. |q 29575/1 |only if havequest(29575)
|confirm |only if not havequest(29575)
step
goto 32.6,44.3 |n
.' Follow this Hallway |goto 32.6,44.3,0.5 |noway |c
step
goto Mana-Tombs 34.3,19.6
.talk 54692
..turnin 29573
..turnin 29575
step
goto 32.8,19.4
.talk 54694
..turnin 29574
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Auchindoun: Auchenai Crypts (AC) 64-68",[[
dungeon 722
description This guide will walk you through the Auchindoun: Auchenai Crypts dungeon.
achieveid 666
author support@zygorguides.com
step
label	"start"
goto Auchenai Crypts 44.3,72.6
.talk 54725
..accept 29590
..accept 29596
step
goto Auchenai Crypts 44.0,66.3 |n
.' Enter the main room of Auchenai Crypts here. |goto Auchenai Crypts 44.0,66.3 <5 |noway |c
step
.from Auchenai Monk##18497+, Auchenai Soulpriest##18493+, Auchenai Vindicator##18495+
.' This room can be a little rough for low level groups. Pull the _Soulpriest's_ first with a silence or charge them, they will cast spells the whole fight.
.' Kill the _Vindicators Soulpriest's first_, then deal with the _Monks_ afterward.
.' You will have to also fight summoned adds, this is unavoidable.
|confirm
step
goto Auchenai Crypts 42.4,16.9 |n
.' Follow the stairs up. |goto Auchenai Crypts 42.4,16.9 <5 |noway |c
step
goto Auchenai Crypts/2,46.3,23.1
.talk 54698
..accept 29591
step
goto 46.2,68.7
.from Shirrak the Dead Watcher##18371 |only if not havequest(29590)
.kill Shirrak the Dead Watcher##18371 |q 29590/1 |only if havequest(29590)
|confirm |only if not havequest(29590)
step
goto 58.1,88.1
.kill 5 Raging Skeleton##18524+ |q 29591/2
|only if havequest(29591)
step
goto 73.9,70.6
.kill 3 Auchenai Necromancer##18702+ |q 29591/3
|only if havequest(29591)
step
goto 74.2,49.9
.from Exarch Maladaar##18373 |only if not havequest(29596)
.kill Exarch Maladaar##18373 |q 29596/1 |only if havequest(29596)
|confirm |only if not havequest(29596)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Auchenai Crypts 44.3,72.6
.talk 54725
..turnin 29590
..turnin 29596
step
goto Auchenai Crypts/2 46.3,23.2
.talk 54698
..turnin 29591
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Caverns of Time: Escape from Durnholde Keep (DH or Durn) 65-69",[[
dungeon 734
description This guide will walk you through the Caverns of Time: Escape from Durnholde Keep dungeon.
achieveid 652
author support@zygorguides.com
step
label	"start"
goto Old Hillsbrad Foothills 28.9,48.3
.talk 18723
..accept 29598
..collect Pack of Incendiary Bombs##25853
step
goto 29.2,47.9
.talk 18725
.' Tell him:
.' <I'm ready to go to Durnhold Keep.> |goto 69.3,71.5 <5 |noway |c
step
goto 70.3,67.5 |n
.' Enter Durnhold Keep here. |goto 70.3,67.5 <5 |noway |c
step
goto 74.5,63.1 |n
.' Go across the Bridge and follow this path to the right. |goto 74.5,63.1 <5 |noway |c
step
goto 77.3,61.5 |n
.' Jump down here. |goto 78.0,62.1 <5 |noway |c
step
goto 77.6,66.0
.click Barrel##334
.' You will need to click the Barrels in each building, until you have 5 Bombs set.
.' You can find more Barrels here:
.' [76.7,67.4]
.' [73.5,67.8]
.' [68.9,62.7]
.' [67.9,59.7]
|confirm |only if not havequest(29598)
.' Set 5 Internment Lodges on Fire. |q 29598/1 |only if havequest(29598)
step
goto 75.7,64.8
.' Lieutenant Drake will come down these stairs when the building are on fire.
.from Lieutenant Drake##17848
|confirm
step
goto 73.2,61.2 |n
.' Go up the stairs and into this building. |goto 73.2,61.2 <5 |noway |c
step
goto 74.5,57.7
.talk 17876
..turnin 29598
step
goto 74.5,57.7
.talk 17876
..accept 29599
step
goto 74.5,57.7
.talk 17876
.' _CAUTION_ |tip Make sure everyone in your group is ready before talking to Thrall.
.' Tell him:
.' <We are ready to get you out of here Thrall. Let's go!>
|confirm
step
goto 69.6,68.4
.from Captain Skarloc##17862
.' After you kill the boss, talk to Thrall.
.talk 17876
.' Tell him
.' <Taretha cannot see you, Thrall.>
|confirm
step
goto 52.8,41.2 |n
.' Follow Thrall until he stops near Tarren Mill, you will have to fight guards along the way.
.talk 17876
.' Tell him
.' < We're ready, Thrall.>
|confirm
step
goto 51.3,29.8 |n
.' Follow Thrall until he comes upstairs here.
.talk 18887
.' Tell her:
.' <Strange wizard?>
|confirm
step
.' Follow Thrall outside.
.' You will have to fight a few waves of dragonkin.
.from Infinite Defiler##18171+, Infinite Saboteur##18172+
.' The Boss will come down to fight once you have killed enough mobs.
.from Epoch Hunter##18096
|confirm |only if not havequest(29599)
.' Fulfill Thrall's Destiny |q 29599/1 |only if havequest(29599)
step
goto 50.3,30.9
.talk 18723
..turnin 29599
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Auchindoun: Sethekk Halls (AS or Seth) 67-69",[[
dungeon 732
description This guide will walk you through the Auchindoun: Sethekk Halls dungeon.
achieveid 653
author support@zygorguides.com
step
label	"start"
goto Sethekk Halls 72.2,35.5
.talk 54840
..accept 29605
..accept 29606
step
goto 64.7,27.8 |n
.' When entering this room, you will fight lots of Arakkoa to fight.
.from Time-Lost Controller##18327+ |tip Kill these first, they will put down a mind control totem, and turn a party member against you group.
.' Go through into the main room through this doorway. |goto 64.7,27.8 <5 |noway |c
step
goto 48.6,67.5
.from Darkweaver Syth##18472 |tip He will summon 4 elementals periodically through the fight, they will attack anyone that pulls aggro.
|confirm |only if not havequest(29606)
.get Terokk's Mask |q 29606/1 |only if havequest(29606)
step
goto Sethekk Halls 48.3,67.7
.talk 54847
..accept 29607
step
.' As you run through the next part of this dungeon:
.from Cobalt Serpent##19428+
.get 3 Cobalt Eye |q 29607/1 |n
|confirm
|only if havequest(29607)
step
goto 50.8,70.8
.talk 18956
.' Tell him
.' <You are free to go>
.' Free Lakka |q 29605/1
|only if havequest(29605)
step
goto Sethekk Halls 51.1,95.2 |n
.' Go up these stairs. |goto Sethekk Halls/2 |noway |c
step
goto 32.5,66.6 |n
.from Cobalt Serpent##19428+
.get 3 Cobalt Eye |q 29607/1
|only if havequest(29607)
step
goto 32.4,56.1
.from Anzu##23035
|confirm
|only if heroic_dung()
step
goto 32.2,29.5
.from Talon King Ikiss##18473
|confirm |only if not havequest(29606)
.get Terokk's Quill |q 29606/2 |only if havequest(29606)
step
goto 49.9,27.3 |n
.' Run down these stairs. |goto 49.9,27.3 <5 |noway |c
step
goto Sethekk Halls 72.2,35.5
.talk 54840
..turnin 29605
..turnin 29606
step
goto 48.1,67.7
.talk 54847
..turnin 29607
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Auchindoun: Shadow Labyrinth (SL or Slabs) 68-70",[[
dungeon 732
description This guide will walk you through the Auchindoun: Shadow Labyrinth dungeon.
achieveid 654
author support@zygorguides.com
step
label	"start"
goto Shadow Labyrinth 22.6,12.5
.talk 54891
..accept 29640
step
goto 21.3,13.1
.talk 54890
..accept 29643
..accept 29645
step
goto 34.3,22.3 |n
.' Follow this hallway up. |goto 34.3,22.3 <5 |noway |c
step
goto 29.5,39.1
.' To activate this boss, kill all the Ritualists in this room. Once you kill the final mob, the Boss will become unleashed.
.kill 12 Cabal Ritualist##18794+ |q 29645/1
step
goto Shadow Labyrinth 21.4,39.7
.' To activate this boss, kill all the Ritualists in this room. Once you kill the final mob, the Boss will become unleashed.
.from Ambassador Hellmaw##18731 |only if not havequest(29645)
.kill Ambassador Hellmaw##18731 |q 29645/2 |only if havequest(29645)
|confirm |only if not havequest(29645)
step
goto Shadow Labyrinth 17.0,22.4
.talk 18891
..turnin 29640
step
goto Shadow Labyrinth 17.0,22.4
.talk 18891
..accept 29641
step
goto 19.2,63.0
.click Soul Device##7010
.get 5 Soul Devices |q 29641/1
|only if havequest(29641)
step
goto 26.2,68.2
.from Blackheart the Inciter##18667 |tip If you do not clear the room first, all the mobs will attack you when you fight the boss.
|confirm
step
goto Shadow Labyrinth 53.3,56.5
.from Grandmaster Vorpil##18732
|confirm
step
goto 53.3,59.0
.click Codex of Blood##6892
..turnin 29643
step
goto 53.3,59.0
.click Codex of Blood##6892
..accept 29644
step
goto 80.8,39.1
..kill Murmur##18708 |q 29644/1
|confirm |only if not havequest(29644)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto 21.3,13.1
.talk 54890
..turnin 29644
..turnin 29645
step
goto 22.6,12.5
.talk 54891
..turnin 29641
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Tempest Keep: The Mechanar (Mech) 68-70",[[
dungeon 730
description This guide will walk you through the Tempest Keep: The Mechanar dungeon.
achieveid 658
author support@zygorguides.com
step
label	"start"
goto The Mechanar/1 49.4,83.5
.' Click the Accept Quest box on the right side of your screen.
..accept 29657
step
.' Take the left passage. |goto 43.5,70.6 <5 |noway|c
step
goto 46.4,58.4
.from Gatewatcher Gyro-Kill##19218
|confirm
step
.' Click the Accept Quest box on the right side of your screen.
..accept 29659
step
goto The Mechanar 51.1,30.4
.from Mechano-Lord Capacitus##19219 |only if not havequest(29657)
.kill Mechano-Lord Capacitus##19219 |q 29657/1 |only if havequest(29657)
|confirm |only if not havequest(29657)
step
.' Click the Turnin Quest box on the right side of your screen.
..turnin 29657
step
.' Click the Turnin Quest box on the right side of your screen.
..accept 29658
step
goto 60.2,36.8
.from Gatewatcher Iron-Hand##19710
|confirm
step
goto 39.4,28.3
.click Cache of the Legion##7216
.' Loot the items inside.
.get Blinding Fury |q 29659/1 |only if havequest(29659)
|confirm |only if not havequest(29659)
step
.' Click the Quest Complete Box in the top right corner.
..turnin 29659
step
goto 41.8,25.7 |n
.' Ride the elevator up to the Calculation Chamber. |goto The Mechanar/2 |noway|c
step
goto 53.1,27.4 |n
.' Clear this whole room before pulling this boss.
|confirm
step
goto 47.6,20.8 |n
.from Nethermancer Sepethrea##19221
.' Do not kill the _Raging Flames_ that she summons, try to avoid them and their _AoE_ spells. They are not hard to avoid.
|modelnpc Raging Flames##20481+
|confirm
step
map The Mechanar/2
path follow loose; loop off
path	52.8,36.4	52.6,50.9	49.6,58.1
path	38.5,60.9
.' Follow this long hallway, and kill the mobs that spawn in groups.
.from Bloodwarder Centurion##19510+, Sunseeker Engineer##20988+, Sunseeker Astromage##19168+, Bloodwarder Physician##20990+, Tempest-Forge Destroyer##19735+
|confirm
step
goto 27.8,61.4
.from Pathaleon the Calculator##19220 |only if not havequest(29658)
.kill Pathaleon the Calculator##19220 |q 29658/1 |only if havequest(29658)
step
.' Click the Quest Complete Box in the top right corner.
..turnin 29658
step
goto 27.3,74.6 |n
' You can use this portal to exit The Mechanar. |goto Netherstorm |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Tempest Keep: Botanica (Bot) 68-70",[[
dungeon 729
description This guide will walk you through the Tempest Keep: Botanica dungeon.
achieveid 659
author support@zygorguides.com
step
label	"start"
goto The Botanica 85.6,46.9
' Click the Accept Quest box on the right side of your screen.
..accept 29660
step
map The Botanica
path follow loose; loop off
path	79.3,49.4	53.0,51.0	47.8,43.3
.' Follow the path through the halls until you reach this room. |goto 47.8,34.5 <5 |noway|c
step
goto 47.4,18.2
.from Commander Sarannis##17976 |only if not havequest(29660)
.kill Commander Sarannis##17976 |q 29660/1 |only if havequest(29660)
|confirm |only if not havequest(29660)
step
goto 42.0,16.2
.' Click the Accept Quest box on the right side of your screen.
..accept 29667
step
goto 25.9,25.5
.from High Botanist Freywinn##17975 |only if not havequest(29660)
.kill High Botanist Freywinn##17975 |q 29660/2 |only if havequest(29660)
|confirm |only if not havequest(29660)
step
goto 8.2,47.6
.from Thorngrin the Tender##17978 |only if not havequest(29660)
.kill Thorngrin the Tender##17978 |q 29660/3 |only if havequest(29660)
|confirm |only if not havequest(29660)
step
.' Click the Complete Quest Box in the top right corner.
..turnin 29660
step
goto 16.5,76.7
..kill 6 Mutate Fleshlasher##19598 |q 29667/1
..kill 6 Mutate Fear-Shrieker##19513 |q 29667/2
..kill 6 Mutate Horror##19865 |q 29667/3
.' You can find more at [29.1,81.9]
.' You may have to run the dungeon more than once to get all of them.
|confirm |only if not completedq(29667)
step
goto 34.1,89.3
.from Laj##17980
.get Rapidly Evolving Frond |q 29667/4 |only if havequest(29667)
|confirm |only if not havequest(29667)
step
'Click the Quest complete box at the top right of the screen
..turnin 29667
step
'Click the Quest Accept box at the top right of the screen
..accept 29669
step
goto 34.1,35.8
.from Warp Splinter##17977 |only if not havequest(29669)
.kill Warp Splinter##17977 |q 29669/1 |only if havequest(29669)
|confirm |only if not havequest(29669)
step
.' Click the Complete Quest box at the top right of the screen.
..turnin 29669
step
goto 34.1,18.0 |n
' You can use this portal to exit The Botanica. |goto Netherstorm |noway|c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Tempest Keep: The Arcatraz (Arca) 68-70",[[
dungeon 731
description This guide will walk you through the Tempest Keep: The Arcatraz dungeon.
achieveid 660
author support@zygorguides.com
step
label	"start"
goto The Arcatraz/1 41.3,75.8
.' Click the Accept Quest box in the top right of the screen
..accept 29674
step
goto 59.7,25.0
.from Zereketh the Unbound##20870 |only if not havequest(29674)
.kill Zereketh the Unbound##20870 |q 29674/1 |only if havequest(29674)
|confirm |only if not havequest(29674)
step
goto 59.7,25.0
.' Click the Quest Complete Box in the top right of the screen.
..turnin 29674
step
goto 65.0,35.6
.' Go up the stairs:
.' Click the Quest Accept Box that pops up.
..accept 29675
step
goto 65.0,35.7 |n
.' Follow this ramp up to the 2nd floor. |goto The Arcatraz/2 |noway|c
step
goto The Arcatraz/2,21.4,77.0
.from Wrath-Scryer Soccothrates##20886 |only if not havequest(29675)
.kill Wrath-Scryer Soccothrates##20886 |q 29675/1 |only if havequest(29675)
|confirm |only if not havequest(29675)
step
goto 35.7,78.4
.from Dalliah the Doomsayer##20885 |only if not havequest(29675)
.kill Dalliah the Doomsayer##20885 |q 29675/2 |only if havequest(29675)
|confirm |only if not havequest(29675)
step
.' Click the Quest Turnin Box that pops up.
..turnin 29675
step
.' Click the Quest Turnin Box that pops up.
..accept 29681
step
goto 32.1,57.1 |n
.' Go through this passage and up the stairs. |goto 32.1,57.1 <5 |c
step
goto The Arcatraz/3,58.2,30.5
.' Attack Warden Mellichar to get the fight stared.
.' He will unleash several monsters and then Harbinger Skyriss.
.from Harbinger Skyriss##20912 |only if not havequest(29681)
..kill Harbinger Skyriss##20912 |q 29681/1 |only if havequest(29681)
|confirm |only if not havequest(29681)
|modelnpc Warden Mellichar##20904
step
.' Click the Quest Turnin Box that pops up.
..turnin 29681
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Caverns of Time: Opening the Dark Portal (CoT:Dark Portal or BM) (Level 68-70)",[[
dungeon 733
description This guide will walk you through the Cavern's of Time: Opening the Dark Portal dungeon.
achieveid 655
author support@zygorguides.com
step
label	"start"
goto The Black Morass 49.3,6.4
.talk 20201
..accept 10297
..collect Chrono-Beacon##24289
step
goto 48.9,71.8
.' Protect Medivh while he attempts to open the Dark Portal. If his shield gets too low you will fail.
.' Monsters will appear randomly in the swamp out of yellow portals. To stop the flow of monsters you have to kill the Rift Lords summmoning the portals.
.from Rift Lord##17839+
.' At several stages of the battle a boss will spawn. Simply kill these like you did the Rift Lords.
.from Chrono Lord Deja##17879+
.from Temporus##17880+
.' Once you have sealed all the rifts a final boss will appear near Medivh
.from Aeonus##17881+
.' Open the Dark Portal |q 10297/1 |only if havequest(10297)
step
goto 49.3,6.5
.talk 20201
..turnin 10297
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Hellfire Citadel: Shattered Halls (SH or Shatt Halls) 67-75",[[
dungeon 710
description This guide will walk you through the Hellfire Citadel: Shattered Halls dungeon.
achieveid 657
author support@zygorguides.com
step
label	"start"
goto The Shattered Halls 58.7,88.7
.talk 54932
..accept 29651
step
goto 58.7,88.7
.talk 54931
..accept 29650
..accept 29655
..accept 29653
step
goto 34.9,69.7
.kill 4 Shattered Hand Legionnaire##16700 |q 29651/1 |only if havequest(29651)
.' Jump down into the sewers here. |goto 34.9,69.7 <5 |noway |c
step
goto 37.0,61.8 |n
.' Follow the sewer and turn left. Enter the Throne of the Damned here. |goto 37.0,61.8 <5 |noway |c
step
goto 34.6,62.1
.from Grand Warlock Nethekurse##16807
.collect Grand Warlock's Amulet##23735 |only if havequest(29650)
|confirm |only if not havequest(29650)
step
goto 31.9,60.6
.' Use your Grand Warlock's Amulet next to this fire. |use Grand Warlock's Amulet##23735
.get Fel Ember |q 29650/1
|only if havequest(29650)
step
goto The Shattered Halls 29.4,14.9
.' Run down this hallway and kill all the Orcs. Watch out for flaming arrows shot at the ground.
.from Shattered Hand Blood Guard##17461+, Shattered Hand Archer##17427+
|confirm
step
goto 53.8,26.4
.kill 2 Shattered Hand Champion##17671 |q 29651/3
.kill 2 Shattered Hand Centurion##17465 |q 29651/2 |tip There will be 4 on the sides, surrounded by Shattered Hand Gladiators as you pass through.
|only if havequest(29651)
step
goto 53.3,33.6
.from Warbringer O'mrogg##16809 |tip He will fear random party members, this can make the fight healing intensive.
.get O'mrogg's Warcloth |q 29655/1 |only if havequest(29655)
|confirm |only if not havequest(29655)
step
goto 67.8,55.3
.from Warchief Kargath Bladefist##16808 |tip This boss will call in Reaver Guards to help during the fight. Tank needs to pick up aggro on them.
.get Warchief Kargath's Fist |q 29653/1 |only if havequest(29653)
.from Reaver Guard##17623
step
.from Shattered Hand Executioner##17301
|only if heroic_dung()
step
goto 67.2,51.8
.talk 55136
..turnin 29651
..turnin 29650
..turnin 29655
..turnin 29653
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Coilfang Reservoir: The Steamvault (SV) 68-70",[[
dungeon 727
description This guide will walk you through the Coilfang Reservoir: The Steamvault dungeon.
achieveid 656
author support@zygorguides.com
step
label	"start"
goto The Steamvault 17.9,27.3
.talk 54848
..turnin 29616
step
goto The Steamvault 17.9,27.3
.talk 54848
..accept 29613
step
goto 17.5,25.9
.talk 54849
..accept 29614
step
goto 18.9,27.8
.talk 54851
..accept 29615
step
goto 33.7,22.7
.kill 3 Bog Overlord##21694 |q 29614/1
|only if havequest(29614)
step
goto The Steamvault 54.3,12.4
.from Hydromancer Thespia##17797 |only if not havequest(17797)
.kill Hydromancer Thespia##17797 |q 29615/1 |only if havequest(29615)
|confirm |only if not havequest(17797)
step
goto 54.2,10.6
.click Main Chambers Access Panel##7147
.' You hear a faint echo...
|confirm
step
.' Go through the opening on your left. |goto 42.1,32.9 <5 |c
step
.' Go up the ramp. |goto 49.3,42.8 <5 |c
step
.' Go up to the bridge on the right side. |goto 55.8,47.3 <5 |c
step
.' Cross the bridge and go through the hallway. |goto 39.7,69.0 <5 |c
step
goto The Steamvault 33.6,81.2
.from Mekgineer Steamrigger##17796
.get Irradiated Gear |q 29614/3 |only if havequest(29614)
.kill 6 Coilfang Leper##21338+ |q 29614/2 |only if havequest(29614)
|confirm |only if not havequest(29614)
step
goto 31.8,84.0
.click Main Chambers Access Panel##7147
.' You hear a faint echo... and You hear a loud rumble of metal grinding on stone....
|confirm
step
goto 54.0,51.4 |n
.' Follow the tunnel back to the bridge. |goto 54.0,51.4 <5 |c
step
.' Take a right after crossing the bridge, and go up a ramp. |goto 69.8,43.4 <5 |c
step
goto 78.7,43.3 |n
.from Warlord Kalithresh##17798 |only if not havequest(29613)
.kill Warlord Kalithresh##17798 |q 29613/1 |only if havequest(29613)
step
.' Follow the path down to the ramp. |goto 49.1,42.6 <5 |c
step
.' Enter this doorway. |goto 42.5,33.0 <5 |c
step
goto 18.8,27.6
.talk 54851
..turnin 29615
step
goto 18.0,27.2
.talk 54848
..turnin 29613
step
goto 17.6,25.9
.talk 54849
..turnin 29614
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Outland Dungeons\\Magisters' Terrace (MT) 70",[[
dungeon 798
description This guide will walk you through the Magisters' Terrace dungeon.
achieveid 661
author support@zygorguides.com
step
label start
goto Magisters' Terrace/2 42.6,87.2
.talk 55007
..accept 29684
..accept 29686
..accept 29687
step
map Magisters' Terrace/2
path follow loose;loop off;ants straight
path	42.6,74.5	42.6,31.2	42.6,23.5
.' Follow this path and kill Selin Fireheart.
.from Selin Fireheart##24723 |only if not havequest(29687)
.kill Selin Fireheart##24723 |q 29687/1 |only if havequest(29687)
|confirm |only if not havequest(29687)
step
goto 60.8,23.7
.talk 24822
..turnin 29684
..accept 11490
step
goto 81.6,26.8
.from Vexallus##24744
.get Volatile Essence |q 29686/1 |only if havequest(29686)
|confirm |only if not havequest(29686)
step
goto 94.2,26.8
.click Scrying Orb##4891
.' Activate the Scrying Orb |q 11490/1
|only if havequest(11490)
step
goto 94.1,35.9
.talk 24848
..turnin 11490
..accept 29685
step
goto 83.2,46.2 |n
.' Follow this ramp down and go through the doorway. |goto Magisters' Terrace 79.8,56.3 <5 |noway |c
step
goto 39.0,56.1
.' Kill Priestess Delrissa and her four companions.
.from Priestess Delrissa##24560 |only if not havequest(29687)
.kill Priestess Delrissa##24560 |q 29687/2 |only if havequest(29687)
|confirm |only if not havequest(29687)
|modelnpc Kagani Nightstrike##24557
|modelnpc Ellrys Duskhallow##24558
|modelnpc Eramas Brightblaze##24554
|modelnpc Yazzai##24561
|modelnpc Warlord Salaris##24559
|modelnpc Apoko##24553
|modelnpc Zelfan##24556
step
map Magisters' Terrace
path follow loose;loop off;ants straight
path	31.0,56.1	19.2,49.7	7.7,50.1
.' Follow the path and kill Kael'thas Sunstrider.
.from Kael'thas Sunstrider##24664
.get Head of Kael'thas |q 29685/1 |only if havequest(29685)
|confirm |only if not havequest(29685)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
|only if havequest(29685)
step
goto 13.4,50.0
.talk 55007
..turnin 29686
..turnin 29687
..turnin 29685
step
goto 4.3,49.6
.click Escape to the Isle of Quel'Danas##7161 |goto Isle of Quel'Danas 47.3,30.6 |noway |c
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Utgarde Keep: Utgarde Keep (UK) 69-72",[[
dungeon 523
description This guide will walk you through the Utgarde Keep: Utgarde Keep dungeon.
achieveid 477
step
label "lfg"
goto Utgarde Keep,68.2,67.7
.talk 24137
.accept 30112
.accept 13206
.accept 11262
step
.' While going through the instance, look for _Vrykul Weapons_ along the walls.
.click Vrykul Weapons##8388
|confirm
|only if havequest(13206)
step
map Utgarde Keep
path follow loose; loop off
path	65.1,32.5	58.3,25.7	50.2,28.5
.' Kill the groups at each of the waypoints.  It will deactivate the flame walls blocking you. |goto 50.2,28.5 <5 |noway|c
step
goto 29.5,67.0
.from Prince Keleseth##23953 |only if not havequest(30112)
.kill Prince Keleseth##23953 |q 30112/1 |only if havequest(30112)
|confirm |only if not havequest(30112)
step
map Utgarde Keep
path follow loose; loop off
path	22.1,74.3	27.2,90.0	40.3,86.5
path	48.6,84.4
.' Go up the steps here. |goto Utgarde Keep/2 |noway|c
step
goto 58.5,65.4
.' There will be two bosses, _Skarvald & Dalronn the Controller_.
.' Skarvald - He has two abilities:
.' He will _Charge_ random party member, stunning them for 2 second, as well as using _Stone Strike_ on the tank, knocking them back.
.' Dalronn - Uses two abilities:
.' His basic attack is _Shadow Bolt_, and he will randomly cast _Debilitate_ slowing the targets melee, moving, spell casting and ranged attack speed by 50% as well as putting a dot on the target.
.' If you can try to kill them at the same time. When one dies, a ghost with the same abilities will spawn, but won't be targetable to control in any way.
.from Dalronn##24201
.from Skarvald the Constructor##24200
|confirm
step
goto 53.6,26.5 |n
.' Go up the steps. |goto Utgarde Keep/3 |noway|c
step
goto Utgarde Keep/3,72.7,38.8
.' There are two phases to this fight.
.' Phase 1:
.' He will often use _Staggering Roar_, which will interrupt any spell casts. Be sure to stop casting if you see him using this.
.' He will use _Smash_, which is a cone attack that attacks whoever is in front of him. It can be avoided by side stepping.
.' Phase 2:
.' He will often use _Dreadful Roar_, which will interrupt any spell casts. Be sure to stop casting if you see him using this.
.' He will use _Woe Strike_, which will leave a debuff that will hurt anyone who heals the target afflicted by this.
.' He will use _Dark Smash_, which will do massive cone damage and can be easily avoided by side stepping. Unlike Smash, this attack will additionally hit everyone in the group, knocking them down.
.' He will also use _Shadow Axe_, which is cast at a random target.  You will have to run away from it.
.from Ingvar the Plunderer##23954
.get Ingvar's Head |q 11262/1 |only if havequest(11262)
|confirm only if not havequest(11262)
step
'Make sure you have gotten all your Vykrul Weapons.
.' Run through these halls and click Vykrul Weapons.
.click Vrykul Weapons##8388
.get 5 Vrykul Weapons |q 13206/1
|only if havequest(13206)
step
goto 48.1,55.6 |n
.' Jump down into the water below. Then go to [50.2,54.5] |n
.' Jump down again. |goto Utgarde Keep |noway|c
step
goto 68.2,67.7
.talk 24137
.turnin 30112
.turnin 13206
.turnin 11262
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\The Nexus (Nexus) 70-73",[[
dungeon 520
description This guide will walk you through the The Nexus dungeon.
achieveid 478
author support@zygorguides.com
step
label "lfg"
goto The Nexus 36.2,83.0
.talk 55537
..accept 13095
step
goto 37.2,81.4
.talk 55536
..accept 11911
step
goto 36.2,81.4
.talk 55531
..accept 11973
step
goto 35.1,81.4
.talk 55535
..accept 11905
step
map The Nexus
path follow loose; loop off
path	32.8,76.3	26.8,68.8	21.8,66.4
path	21.6,56.2	19.0,51.1
.' Follow this path
.from Alliance Commander##27949
|confirm |only if not havequest(13095)
.click Berinand's Research##8128 |only if havequest(13095)
.get Berinand's Research |q 13095/1 |only if havequest(13095)
step
goto 27.6,38.6
.from Grand Magus Telestra##26731
.' Follow the path left and you will reach her.
|confirm
step
map The Nexus
path follow loose; loop off
path	27.4,30.3	31.6,22.0	47.3,21.7
path	61.5,21.9
.from Anomalus##26763
|confirm
step
goto 65.1,21.8
.' Use your Interdimensional Refabricator here |use Interdimensional Refabricator##35479
.' Stall the Interdimensional Rift |q 11905/1
|only if havequest(11905)
step
map The Nexus
path follow loose; loop off
path	53.9,35.8	56.4,53.0	64.2,52.7
path	60.3,64.8
.' Follow this path to the next boss |goto 60.3,64.8 <5 |only if not havequest(11911)
.from Crystalline Protector##26792
.get 5 Arcane Splinter |q 11911/1 |only if havequest(11911)
.' You can find more _Protectors here_ [53.3,55.6] |only if havequest(11911)
step
goto 56.9,70.3
.from Ormorok the Tree-Shaper##26794
.' Once you kill him, you should be able to engage _Keristrasza_.
.' Go up the ramp on the right and jump down.
|confirm
step
goto 38.3,67.2
.' Kill the Azure Magus in the room, then look for the Containment Spheres around the room.
.click Telestra's Containment Sphere##7800
.from Keristrasza##26723
.' Keristrasza Laid to Rest. |q 11973/1 |only if havequest(11973)
.' Make sure that you move often during the fight.  If you don't you will get a debuff that lowers your attack and casting speed.
step
goto 36.2,83.0
.talk 55537
..turnin 13095
step
goto 37.3,81.4
.talk 55536
..turnin 11911
step
goto 36.3,81.4
.talk 55531
..turnin 11973
|only if havequest(11973)
step
goto 35.2,81.4
.talk 55535
..turnin 11905
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Azjol-Nerub (AN) 71-74",[[
dungeon 533
description This guide will walk you through the Azjol-Nerub dungeon.
achieveid 480
step
label "lfg"
goto Azjol-Nerub/3,15.2,83.8
.talk 55564
.accept 29807
.accept 29808
.accept 29811
step
goto 34.6,48.7
.click Nerubian Scourge Egg##7803
.' Destroy 6 Nerubian Scourge Eggs. |q 29808/1
|only if not completedq(29808)
step
goto 46.3,45.2
.from Krik'thir the Gatewatcher##28684
.get The Gatewatcher's Talisman |q 29811/1 |only if havequest(29811)
|confirm |only if not havequest(28684)
step
goto 87.4,53.5
.' Pull the mobs that are standing in the middle of the room.
.' Once you pull them, it will trigger an event.  Clear all the mobs that come at you until _Hadronox_ engages you.
.from Hadronox##28921
|confirm
step
goto 72.6,30.0 |n
.' Follow this path to the right. |goto Azjol-Nerub/2 |noway |c
step
goto Azjol-Nerub/2 49.3,65.3 |n
.' Jump down the hole here. |goto Azjol-Nerub |noway |c
step
goto Azjol-Nerub,87.4,53.5
.from Anub'arak##29120
.get Anub'arak's Broken Husk |q 29807/1 |only if havequest(29807)
|confirm |only if not havequest(29807)
step
goto 62.2,48.5
.talk 55564
.turnin 29807
.turnin 29808
.turnin 29811
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Ahn'kahet: The Old Kingdom (OK) 72-75",[[
dungeon 522
description This guide will walk you through the Ahn'kahet: The Old Kingdom dungeon.
achieveid 481
step
label "lfg"
goto Ahn'kahet: The Old Kingdom 87.2,72.6
.talk 55658
.accept 29825
step
.' Note that as you're killing trash, target the _Ahn'kahar Spell Flingers_ first.
.' If possible interrupt the  _Shadow Blast_ ability every chance you get.
|confirm
step
goto 70.4,28.6
.from Elder Nadox##29309 |only if not havequest(29825)
.kill Elder Nadox##29309 |q 29825/1 |only if havequest(29825)
|confirm |only if not havequest(29825)
step
.' Click the Complete Quest Box in the top right corner of your screen.
.turnin 29825
.accept 29826
step
goto 56.7,24.4
.' Kill the mobs around the Ancient Nerubian Device, then click it.
.click Ancient Nerubian Device##8393
|confirm
step
goto 50.1,29.9
.kill 3 Eye of Taldaram##30285+ |q 29826/1 |only if havequest(29826)
.click Ancient Nerubian Device##8393
|confirm
step
goto 56.9,38.2
.kill Bonegrinder##30284 |q 29826/2
|only if havequest(29826)
step
goto 63.1,50.4
.from Prince Taldaram##29308 |only if not havequest(29826)
.kill Prince Taldaram##29308 |q 29826/3 |only if havequest(29826)
|confirm |only if not havequest(29826)
step
.' Click the Complete Quest Box in the top right corner of your screen
.turnin 29826
.accept 13187
step
goto 49.3,72.3
.from Jedoga Shadowseeker##29310
.' The boss here is option, click here to skip it. |confirm
step
goto 32.4,50.1
.from 3 Forgotten One##30414+ |only if not havequest(13187)
.kill 3 Forgotten Ones##30414+ |q 13187/1 |only if havequest(13187)
.' Avoid Shadow Crash, an AoE attack that targets the ground.
|confirm |only if not havequest(13187)
step
.' Herald Volazj casts a spell called _Insanity_, which makes you kill a mirror image of each of your party members.
.' Note that if you're by yourself, he will not use _Insanity_.
.from Herald Volazj##29311 |only if not havequest(13187)
.kill Herald Volazj##29311 |q 13187/2 |only if havequest(13187)
|confirm |only if not havequest(13187)
step
goto 26.9,50.4
.talk 55658
.turnin 13187
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Drak'Tharon Keep (DTK) 73-76",[[
dungeon 534
description This guide will walk you through the Drak'Tharon Keep dungeon.
achieveid 482
step
label "lfg"
goto Drak'Tharon Keep,35.5,83.2
.talk 55677
.accept 13129
.accept 29828
step
goto 37.0,80.9
.talk 58149
.accept 30120
step
map Drak'Tharon Keep
path follow loose; loop off
path	47.1,83.2	47.9,74.0	47.4,54.4
path	47.3,43.2	54.6,35.0
.' Clear the trash at each of these points.
|confirm
step
goto Drak'Tharon Keep/1 56.8,19.3
.' Trollgore will use the _Consume_ ability, eating his fallen allies and gaining a damage buff. You can avoid this by kiting him away from the bodies.
.' He will also use _Corpse Explode_ on his fallen allies, damaging those around.
.' He'll put an _Infected Wound_ debuff on the tank, making him take extra damage.
.from Trollgore##26630
|confirm
step
goto Drak'Tharon Keep 73.3,47.6
.' Attack Novos the Summoner to start the event.
.' Kill the mobs that come down the steps here.
.' 4 Crystal Handler will come one at a time from the waypoints on each of the summoners side.
.' Once they are dead, Novos will become vulnerable to attack.
.' He will cast _Blizzard_ on random party members.
.' He will put _Wrath of Misery_ on random party members, which leaves a dot.
.from Novos the Summoner##26631
.' Use Kurzel's Blouse Scrap on Novos the Summoners corpse. |use Kurzel's Blouse Scrap##43214 |only if havequest(13129)
.get Ichor-Stained Cloth |q 13129/1 |only if havequest(13129)
'|goto Drak'Tharon Keep,67.4,55.9
'|goto Drak'Tharon Keep,67.2,38.0
|confirm |only if not havequest(13129)
step
goto Drak'Tharon Keep 67.8,80.2
.' Clear the trash before engaging King Dred.
.' If you didn't clear the trash he will use an ability called _Raptor Call_, calling all raptors in the area to assist him.
.' Stand at max range if you're a caster / healer. He uses _Bellowing Roar_ which fears everyone within 35 yards. It can be countered with Fear Ward or Tremor Totem.
.' He will hit the tank with _Grievous Bite_ which deals damage until the target is fully healed.
.' He will also use _Mangling Slash_, which increases the damage the bleed does.
.' He will use _Piercing Slash_ which reduces your armor by 75%, all damage reduction and avoidance abilities should be used at this time.
.from King Dred##27483
.get King Dred's Tooth |q 29828/1 |only if havequest(29828)
|confirm |only if not havequest(29828)
step
goto 63.7,71.8 |n
.' Go up the stairs here. |goto Drak'Tharon Keep/2 |noway|c
step
.' Kill trolls in this area and collect _Enduring Mojo_.
.from Drakkari Commander##27431+,Drakkari Guardian##26620+,Drakkari Shaman##26639+
.collect 5 Enduring Mojo##38303 |q 30120
'|goto Drak'Tharon Keep/2,37.1,67.2
'|goto Drak'Tharon Keep/2,37.3,29.2
|only if havequest(30120)
step
goto 51.3,13.2
.' The Prophet Tharon'ja has two Phases.
.' Skeletal:
.' He will cast _Shadowbolt Volley_, an AoE Shadow attack.
.' He will cast _Rain of Fire_ at grouped up allies.
.' He will use _Decay Flesh_, which turns you into a skeleton and puts you into the second phase where he is a serpent.
.' Serpent:
.' He will use _Lightning Breath_, an AoE Nature attack that jumps to nearby targets.
.' He will cast _Poison Cloud_ on the ground.
.' You will have 4 attacks as a skeleton.
.' Tanks should use all four abilities on cooldown.
.' DPS and Heals should use everything but _Taunt_.
.' Repeat the actions until he is defeated.
.from The Prophet Tharon'ja##26632
|confirm
step
goto 37.9,13.2
.' Use Drakuru's Elixer at the giant brazier here. |use Drakuru's Elixir##35797
.' Follow Drakuru.
.turnin 30120
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Drak'Tharon Keep 35.5,83.2
.talk 55677
..turnin 13129
..turnin 29828
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\The Violet Hold (VH) 73-76",[[
dungeon 536
description This guide will walk you through the The Violet Hold dungeon.
achieveid 483
step
label "lfg"
goto The Violet Hold 47.7,89.7
.talk 30658
.accept 29830
step
goto 47.5,90.3
.talk 30658
.' Once you've accepted the quest, tell Lieutenant Sinclari that you will activate the crystals when in trouble.
|confirm
step
.' In this instance, _Blue Portals_ will randomly pop up at each of the large pink shields in the room.
.' You will need to go to it and defeat the monsters that come out until it closes.
.' On waves 6, 12 and 18
|confirm
step
label "the_list"
.' Click here for the Boss Erekem. |next "Erekem" |confirm
.' Click here for the Boss Moragg. |next "Moragg" |confirm
.' Click here for the Boss Ichoron. |next "Ichoron" |confirm
.' Click here for the Boss Xevozz. |next "Xevozz" |confirm
.' Click here for the Boss Lavanthor. |next "Lavanthor" |confirm
.' Click here for the Boss Zuramat the Obliterator. |next "Zuramat_the_Obliterator" |confirm
.' Click here for the Boss Cyanigosa. |next "final_boss" |confirm
|modelnpc Erekem##29315, Moragg##29316, Ichoron##29313, Xevozz##29266, Lavanthor##29312, Cyanigosa##31134
step
label "Erekem"
goto 28.1,61.9
.' He comes with 2 friends.  Attack him first.
.' Interrupt _Chain Heal_ when he casts it.
.' Dispel, Purge, or Spellsteal when he casts _Earth Shield_.
.' Purge or Spellsteal when he uses _Bloodlust_.
.from Erekem##29315
.' Click here to go back to the list of bosses. |confirm |next "the_list"
step
label "Moragg"
goto 65.0,55.3
.' Moragg puts _Corrosive Saliva_ which reduces the targets armor by 5% per application (Stacking effect).
.' It will cast _Optic Link_ on a random party member. The damage slowly builds, getting higher the longer he channels it.
.' It will put _Ray of Suffering_ on a random party member, dealing out 8% of the targets total health each tick.
.from Moragg##29316
.' Click here to go back to the list of bosses. |confirm |next "the_list"
step
label "Ichoron"
goto 53.8,42.5.
.' Ichoron has a _Protective Bubble_, preventing 99% of the damage you do to him. This shield has a charge, and every time he is hit a charge is taken away.
.' Each time you remove his Protective Bubble, he splits into many smaller water elementals.  When they start appearing, go to one of the waypoints on the map and click the crystal on the wall.
.' He will use _Water Blast_ to knock your tank around.
.' He often casts _Water Bolt Volley_, an AoE frost based attack.
.' You will need to destroy the _Protective Bubble_ 3 times before he is _Drained_ and you are able to kill him.
.' When his shield is gone, he will gain a _Frenzy_ effect, increasing his caster speed and damage output.
.from Ichoron##29313
.' Click here to go back to the list of bosses. |confirm |next "the_list"
'|goto The Violet Hold,31.0,75.6
'|goto The Violet Hold,46.5,39.8
'|goto The Violet Hold,55.5,79.9
step
label "Xevozz"
.' Xevozzs will need to be kited around the room.
.' He spawns _Ethereal Spheres_ which deal massive damage when you come into contact with them.  They will give the boss a huge damage buff, and give the player a huge arcane resistance debuff.
.' The orbs also cast a constant _Arcane Explosion_ so stay away from them.
.' He will put _Arcane Buffet_ on a random party member. It can be dispelled (Magic Effect).
.' He often spams _Arcane Barrage Volley_.
.from Xevozz##29266
.' Click here to go back to the list of bosses. |confirm |next "the_list"
step
label "Lavanthor"
goto 59.4,73.8
.' Lavanthor uses a cone attack, _Flame Breath_ so don't be in front of him.
.' He will cast _Lava Burn_ at random party members.
.' He casts _Firebolt_ which can be interrupted.
.' He places _Lava Burn_ on the ground, which deals damage if you stand in it.
.from Lavanthor##29312
.' Click here to go back to the list of bosses. |confirm |next "the_list"
step
label "Zuramat_the_Obliterator"
.' This boss will _Summon Void Sentries_ which spam Shadowbolt Volley.
.' He will cast _Void Shift_ on a random party member, this will allow them to see the Void Sentries. Whoever gets this debuff is responsible for clearing Void Sentries.
.' _Shroud of Darkness_ is a buff Zuramat throws on himself that reduces the healing the group can receive. It is a magic effect and needs to be dispelled.
.from Zuramat the Obliterator##29314
.' Click here to go back to the list of bosses. |confirm |next "the_list"
step
label "final_boss"
goto 45.1,54.6
.' DPS and Heals should stand on either of her sides, to avoid _Uncontrollable Energy_ and_Tail Swipe_.
.' She will use _Arcane Grip_, which pulls all party members to her and drops aggro. The tank should _Taunt_ immediately.
.' She will cast _Blizzard_ on a random party member.
.from Cyanigosa##31134
|confirm |only if not havequest(29830)
.' Cyanigosa Slain |q 29830/1 |only if havequest(29830)
step
goto The Violet Hold 45.9,91.2
.talk 30658
.turnin 29830
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Gundrak (GD) 75-78",[[
dungeon 530
description This guide will walk you through the Gundrak dungeon.
achieveid 484
step
label "lfg"
goto Gundrak 58.3,34.2
.talk 55738
.accept 29834
.accept 29839
.accept 29844
step
.' There will be Drakkari History Tablets throughout the dungeon. you will need to collect 6 to complete the quest.
.click Drakkari History Tablet##7356
|confirm
step
goto Gundrak,58.8,40.0
.' Follow the ramp down.
|confirm
step
goto 56.7,48.7
.click Drakkari History Tablet##7356
.' Slad'ran will have several abilities.
.' When he is casting _Poison Nova_ move away  from him as quickly as you can.
.' He will randomly cast _Venom Bolt_ on party members.
.' He summons _Slad'ran Constrictors_ which could be AoE'd down. If they aren't killed, they will use _Grip of Slad'ran_ which will incapacitate whoever get attacked with it.
.from Slad'ran##29304 |only if not havequest(29834)
.kill Slad'ran##29304 |q 29834/1 |only if havequest(29834)
|confirm |only if not havequest(29834)
step
goto 53.8,48.7
.click Altar of Slad'ran##8278
|confirm
step
map Gundrak
path follow loose; loop off
path	58.8,56.9	57.8,67.7	46.8,73.8
.' Follow this path
.' Kill the trash mobs in this area.
|confirm
step
goto 46.9,66.3
.' The Drakkari Colossus splits into two bosses.
.' The Drakkari Colossus uses _Mortal Strike_ reducing healing taken by 50% on the tank.
.' At 50% health, the Drakkari Elemental will Emerge. Swtich DPS to it.
.' The Drakkari Elemental will put purple puddles on the ground. Move out of them.
.' It will charge forward in a straight line and damage anyone in its path.
.' At 50% health, it will merge back with the Drakkari Colossus.
.' Repeat until it is defeated.
.from Drakkari Colossus##29307
.from Drakkari Elemental##29573
.get Drakkari Colossus Fragment |q 29839/1 |only if havequest(29839)
|confirm |only if not havequest(29839)
step
goto 46.5,62.5
.click Altar of the Drakkari Colossus##8278
|confirm
step
goto 41.2,72.4 |n
.' Jump down into the water here
|confirm
step
goto 37.4,59.3 |n
.' Swim to land. |goto 37.4,59.3 <5 |noway|c
step
goto 37.4,49.1
.' Moorabi uses several abilities:
.' _Ground Tremor_ is an ability that damages melee.
.' He will also use _Mojo Frenzy_ which increases his casting speed, as well as his damage taken.
.' He will use _Numbing Shout_ which reduces spell casting speed by 25%.
.' He will occassionally cast _Transformation_ which can be interrupted. Each time he casts it, it is faster.
.from Moorabi##29305 |only if not havequest(29834)
.kill Moorabi##29305 |q 29834/2 |only if havequest(29834)
|confirm |only if not havequest(29834)
step
goto 32.0,49.2 |n
.' Pass through the new opening here. |goto 32.0,49.2 <5 |noway |c
|only if heroic_dung()
step
goto 25.1,66.2
.' Clear the Ruins Dwellers in the area. |goto 25.1,66.2 <5 |noway|c
.from Ruins Dweller##29920+
|only if heroic_dung()
step
.from Eck the Ferocious##29932
.' Eck will appear once all the Ruin Dwellers have been defeated.
|confirm
|only if heroic_dung()
step
goto 39.6,49.2
.click Altar of Moorabi##8278
|confirm
step
goto 40.7,49.6
.' Jump down and swim across. |goto 44.6,54.3 |c
step
goto 46.5,28.4
.' He will also randomly throw a spear at a party member, giving them a bleed effect for 2 minutes called _Puncture_.
.' He will use _Stampede_ on a random target, knocking them into the air.
.' He has the ability _Whirling Slash_ which deals out massive damage if you're in melee range of him.
.' Eventually he will transform into a Rhino.
.' While transformed, he will use different abilities, listed below:
.' _Enrage_ increases his attack speed by 100%.
.' Gal'darah will use _Impaling Charge_ throughout the fight, randomly knocking back a party member.
.' He will use _Stomp_ to knock melee away from him.
.from Gal'darah##29306 |only if not havequest(29834)
.kill Gal'darah##29306 |q 29834/3 |only if havequest(29834)
|confirm |only if not havequest(29834)
step
.get 6 Drakkari History Tablet |q 29844/1
.' If you haven't collected all 6, search through the instance for them.
.' They are near walls.
|only if havequest(29844)
step
goto Gundrak 51.7,20.9
.talk 55738
.turnin 29834
.turnin 29839
.turnin 29844
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Halls of Stone (HoS) 76-79",[[
dungeon 526
description This guide will walk you through the Halls of Stone dungeon.
achieveid 485
step
label  "lfg"
goto Halls of Stone 35.8,35.0
.talk 55835
.accept 29848
.accept 29850
step
goto 49.8,41.2
.kill 3 Dark Rune Giant##27969 |q 29850/1
|only if havequest(29850)
step
map Halls of Stone
path follow strict;loop off;ants straight
path	42.6,53.6	39.1,52.9	31.9,51.7
path	34.0,68.0	38.1,65.5
.' Clear trash following this path.  It will lead you to _Krystallus_.
|confirm
step
goto 39.6,61.8
.' It is important to spread out on this fight.
.' Krystallus will use _Boulder Toss_ on a random player, knocking them down and dealing damage to those around the target.
.' He will also use _Stomp_, knocking the group backwards.
.' He will turn the group to stone, followed by using the ability _Shatter_ which will damage the entire group.
.from Krystallus##27977 |only if not havequest(29850)
.kill Krystallus##27977 |q 29850/2 |only if havequest(29850)
|confirm |only if not havequest(29850)
step
.' Jump down here. |goto 41.0,57.5 <5 |c
step
goto 50.0,85.4
.' The _Maiden of Grief_ will cast Pillar of Woe on random party members. It causes shadow damage over time and can be dispelled. It is a magic effect.
.' Throughout the fight she will place _Storm of Grief_ on the ground, which is a large black circle that deals shadow damage. Avoid when possible.
.' She will use the ability _Shock of Sorrow_, which will damage the entire group and incapacitate them for 6 seconds. To avoid the incapacitating effect, stand in _Storm of Grief_ before she finishes casting Shock of Sorrow.
.from Maiden of Grief##27975
.get Crystal Tear of Grief |q 29848/1 |only if havequest(29848)
|confirm |only if not havequest(29848)
step
goto 59.2,53.6
.kill 3 Dark Rune Giant##27969 |q 29850/1
|only if havequest(29850)
step
goto 70.9,48.6
.talk 28070
.accept 13207
.' When you're ready to go, tell Brann that it would be our honor!
|confirm
step
goto 81.7,71.1
.' Once here, you will be at the next event.
.' You will need to _protect Brann Bronzebeard_ from enemies that attack.
.' Let Brann know you're ready to move.
.' Click here to continue when the event is over. |confirm always
step
goto 84.2,74.8
.talk 28070
.' Tell him we need to get moving.
|confirm
step
goto 49.9,28.7
.talk 28070
.' Tell Brann to open the door.
|confirm
step
Sjonnir The Ironshaper
.' He has a _Lightning Shield_ that can be _Purged_ or _Dispelled_.
.' He places _Static Charge_ on a random party member. The afflicted member should move away from the group.
.' He channels _Lightning Ring_ which you should move away from.
.from Sjonnir The Ironshaper##27978
|confirm
step
goto Halls of Stone 61.8,39.7
.talk 28070
..turnin 13207
step
goto Halls of Stone 35.8,34.9
.talk 55835
.turnin 29848
.turnin 29850
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Trial of The Champion (ToC) 78-80",[[
dungeon 542
description This guide will walk you through the Trial of the Champion dungeon.
achieveid 3778
step
label "lfg"
goto Trial of the Champion 49.6,30.3
.accept 29851
.click Lance Rack##130
.collect Argent Lance##46106
|confirm
step
goto 57.9,33.6
.' Equip your _Argent Lance_. |equipped Argent Lance##46106 |use Argent Lance##46106
.clicknpc Argent Battleworg##36558
.' Ride an Argent Battleworg |invehicle
step
.' Your Argent Battleworg has 4 abilities:
.' _Thrust_ Is your primary attack, and is assigned to 1.
.' _Shield Break_ is assigned to 2, and should be used to remove and enemies _Defend_ buff.
.' _Charge_ deals out massive damage, as well as removing 1 layer of _Defend_ from the enemy. It is assigned to 3.
.' _Defend_ Reduces damage you take by 30%, stacking 3 times. This should be maintained at 3 at all times.
'|clicknpc Argent Battleworg##36558
|confirm
step
goto 51.0,53.1
.talk 35005
.' Let him know that you're ready.
.' When the even starts there will be 3 enemies.
.' Mark a target and focus them down one by one.
.' There will be several waves of jousting enemies up until the 3 champions come out.
.' Repeat the same strategy with them and complete the event.
|confirm
|modelnpc Marshal Jacob Alerius##34705, Ambrose Boltspark##34702, Jaelyne Evensong##34657
step
.' The first boss encounter will consist of the last group of jousters your defeated.
.' _Colosos_ should be your first target if he is there. He has _Healing Wave_ and _Earth Shield_ which will make the fight last longer than it has to. Interrupt Healing Wave at all costs.
.' _Ambrose Boltspark_ should be second if applicable. He will _Polymorph_ random party members, as well as use _Blast Wave_ to AoE your group down.
.' _Lana Stouthammer_ should be third, if applicable. She will use _Poison Bottle_ leaving a green puddle on the ground that deals nature damage as well as _Fan of Knives_ as AoE.
.' _Marshal Jacob Alerius_ should be fourth, if applicable. He will use _Mortal Strike_ on the tank reducing healing done by 50% as well as a _Bladestorm_ ability, hitting everyone near him.
.' _Jaelyne Evensong_ should be fifth, if applicable. She will use _Multishot_ and _Lightning Arrow_ which can be dispelled.
.' _Kill the Champions of the Alliance_
|modelnpc Colosos##34701, Ambrose Boltspark##34702, Lana Stouthammer##34703, Marshal Jacob Alerius##34705, Jaelyne Evensong##34657
|confirm
step
goto 54.2,48.4
.' Once you kill the boss, trash will come out.
.' There will be 3 groups of enemies that come out.
.' Each group will have an _Argent Priestess_. Be sure to kill her first.
.' Interrupt her heals and kill _Fountain of Light_ when she puts one out.
|confirm
|modelnpc Argent Priestess##35307, Argent Monk##35305, Argent Lightwielder##35309
'|goto 38.4,51.4
step
.' There will be one of two bosses at this point:
.' Click here for _Eadric_. |next "Eadric" |confirm
.' Click here for _Argent Confessor Paletress_. |next "Paletress" |confirm
|modelnpc Eadric##35119, Paletress##34928
step
label "Eadric"
goto 39.0,53.1
.' Eadric will use _Hammer of Justice_ on a random party member.
.' When Eadric casts _Radiance_, turn away from him so you aren't blinded. It will render you unable to act for 2 seconds.
.' He will also cast _Hammer of the Righteous_ at a random party member. If the party member isn't affected by Hammer of Justice, you can catch it and throw it back at him.
.from Eadric##35119
|confirm |next "Black_Knight"
step
label "Paletress"
goto 39.0,53.1
.' She will cast _Holy Fire_ on random party members. It will leave a DoT.
.' She will often cast _Holy Nova_ before Confess, which will spawn a Shadow that you will have to kill before finishing her off.
.from Argent Confessor Paletress##34928
|confirm
step
label	"Black_Knight"
goto 51.0,53.1
.talk 35005
.' Let him know you're ready for the finals.
.' He will get killed and _The Black Knight_ will appear.
|confirm
step
.' There are 3 phases to this fight.
.' _Phase 1:_
.' He will summon Ghouls which leap around and drop aggro often.  They also _Explode_ often, dealing out AoE Damage.
.' He will also random use _Death's Respite_ on random party members, stunning them for 2 seconds and knocking them back, dealing massive damage.
.' _Phase 2:_
.' He will use _Army of the Dead_, summoning several ghouls that leap around and explode.
.' He will leave _Desecration_ on the ground under a random target. It deals out damage every 2 seconds so move out of it.
.' _Phase 3:_
.' He has _Death's Bite_ aura, which deals out shadow and frost damage that leaves an increase magic damage taken of 5% debuff that stacks.
.' He will cast _Marked for Death_ which leaves a debuff that increases magic damage taken by 200% for 10 seconds.
.from The Black Knight##35451
|confirm |only if not havequest(29851)
.' Defeat the Black Knight |q 29851/1 |only if havequest(29851)
step
'Click the Complete Quest Box in the top right corner of your screen
..turnin 29851
|only if havequest(29851)
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Caverns of Time: The Culling of Stratholme (CoS) 78-80",[[
dungeon 521
description This guide will walk you through the Caverns of Time: The Culling of Stratholme dungeon.
achieveid 479
author support@zygorguides.com
step
label  "start"
goto The Culling of Stratholme 86.5,61.5
.talk 26527
.accept 13149
step
goto 86.5,61.5
.talk 26527
.collect Arcane Disruptor##37888
|only if not havequest(13149)
step
map The Culling of Stratholme
path follow loose; loop off
path	83.8,59.2	81.2,59.8	77.8,55.0
path	73.5,54.9	70.2,51.2
.' Use your Arcane Disruptor on the 5 Suspicious Grain Crates at the waypoints. |use Arcane Disruptor##37888
.' Click here when you have all 5 Dispelled|confirm |only if not havequest(13149)
.' Plague Grain Crates Dispelled |q 13149/1 |only if havequest(13149)
'|click Plagued Grain Crate##336
step
goto 47.6,39.8
.talk 27915
.turnin 13149
step
goto 47.6,39.8
.talk 27915
..accept 13151
step
goto 47.6,39.8
.talk 27915
.' Ask Chromie what they're up to.
|confirm
step
goto 47.3,24.9 |n
.' Cross the bridge into Stratholme. |goto The Culling of Stratholme/2 |noway|c
step
goto The Culling of Stratholme/2,50.5,79.2
.talk 26499
.' Let him know that you are ready.
|confirm
step
.' Move between these waypoints in order to find the mobs that spawn.
.' Click here when _Meathook_ spawns. |confirm
'|goto 45.1,64.9
'|goto 46.6,58.5
'|goto 53.1,62.4
'|goto 61.5,52.9
'|goto 59.7,39.0
step
goto 52.9,62.4
.' Meathook walks around this area.
.' He has two main attacks:
.' He will use _Constricting Chains_ on a random party member, stunning them for 5 seconds and dealing damage.
.' He will use _Disease Expulsion_, which will deal out damage and interrupt spell casts to players nearby.
.from Meathook##26529
|confirm
'|goto 47.2,59.8
'|goto 61.2,53.0
'|goto 60.2,44.4
step
.' Move between these waypoints in order to find the mobs that spawn.
.' Click here when _Salramm the Fleshcrafter_ spawns. |confirm
'|goto 45.1,64.9
'|goto 46.6,58.5
'|goto 53.1,62.4
'|goto 61.5,52.9
'|goto 59.7,39.0
step
goto 52.9,62.4
.' Salramm the Fleshcrafter walks around one of these locations.
.' He will cast _Steal Flesh_ on a random group member, lowering their damage by 75% and increases his by 75%
.' He casts _Summon Ghouls_ throughout the fight. He will randomly cast _Explode Ghoul_ as well, blowing them up to do AoE damage.
.from Salramm the Fleshcrafter##26530
|confirm
'|goto 47.2,59.8
'|goto 61.2,53.0
'|goto 60.2,44.4
step
goto 58.8,36.7
.talk 26499
.' Tell _Arthas_ to lead the way.
|confirm
step
.' Fight the mobs that spawn at these points.
|confirm
'|goto 58.1,32.7
'|goto 59.0,27.0
'|goto 61.7,31.4
'|goto 65.8,28.4
step
goto 66.1,26.0
.' Chrono-Lord Epoch will come out of a portal here.
.' He will use _Time Warp_ and uses an AoE Charge.
.' He will use _Wounding Strike_ on the tank, reducing healing received by 25% as well as leaving a DoT.
.from Chrono-Lord Epoch##26532
|confirm
step
goto 65.5,28.8
.talk 26499
.' Tell Arthas that you're ready. |confirm
step
.' Follow Arthas down the steps. |goto The Culling of Stratholme/2 64.9,14.2 <5 |c
step
goto 64.9,14.2
.talk 26499
.' Tell him "For Lordaeron!"
|confirm
step
.' Follow and protect Arthas as he goes through the tunnel. |goto 40.1,37.2 <5 |c
step
goto 40.1,37.2
.talk 26499
.' Tell him you're ready to battle the dreadlord.
|confirm
step
goto 33.5,44.9
.' Mal'Ganis has a few attacks to watch out for:
.' He will randomly cast _Sleep_ on a random party memeber, incapacitating them for 10 seconds.
.' He will cast _Carrion Swarm_ which is a cone attack in front of him.
.' He will cast _Mind Blast_ on random party members.
.' He will buff himself with _Vampiric Touch_ causing him to heal for half the damage he does. It can be _Purged_ or removed with _Dispel Magic_.
.from Mal'Ganis##26533
.' Defeat Mal'Ganis |q 13151/1 |only if havequest(13151)
|confirm |only if not havequest(13151)
step
goto 32.0,44.7
.talk 30997
.turnin 13151
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\The Oculus (OC) 78-80",[[
dungeon 528
description This guide will walk you through the Oculus dungeon.
achieveid 487
step
label "lfg"
goto The Oculus 63.3,42.0
.accept 13124
step
goto 38.3,50.5 |n
.' Follow this ledge around.
.' Click the portal. |goto 47.9,69.4 <5 |noway|c
step
goto 48.9,76.1
.from Drakos the Interrogator##27654
|confirm
step
goto 50.1,81.2
.talk 27658
.turnin 13124
step
goto 50.1,81.2
.talk 27658
..accept 13126
step
.' Talk to either Verdisa, Belgaristrasz or Eternos and attain a dragon.
.collect Ruby Essence##37860 |or |tip Talk to Belgaristrasz for the Ruby Essence.
.collect Amber Essence##37859 |or |tip Talk to Eternos for the Amber Essence.
.collect Emerald Essence##37815 |or |tip Talk to Verdisa for the Emerald Essence.
'|talk Verdisa##27657
'|talk Belgaristrasz##27658
'|talk Eternos##27659
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence|invehicle
step
goto 47.0,54.6 |n
.' Fly up and over to this platform. |goto The Oculus/2 |noway |c
step
goto The Oculus/2,41.0,66.3
.from 1 Centrifuge Construct##27641 |only if not havequest(13126)
|confirm |only if not havequest(13126)
.kill 1 Centrifuge Construct##27641 |q 13126/1 |only if havequest(13126)
step
goto 40.2,55.5
.from 1 Centrifuge Construct##27641 |only if not havequest(13126)
|confirm |only if not havequest(13126)
.kill 2 Centrifuge Construct##27641 |q 13126/1 |only if havequest(13126)
step
goto 57.7,54.3
.from 1 Centrifuge Construct##27641 |only if not havequest(13126)
|confirm |only if not havequest(13126)
.kill 3 Centrifuge Construct##27641 |q 13126/1 |only if havequest(13126)
step
goto 57.4,66.6
.from 1 Centrifuge Construct##27641 |only if not havequest(13126)
|confirm |only if not havequest(13126)
.kill 4 Centrifuge Construct##27641 |q 13126/1 |only if havequest(13126)
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence |invehicle
step
goto 71.0,76.7
.from 3 Centrifuge Construct##27641 |only if not havequest(13126)
|confirm |only if not havequest(13126)
.kill 7 Centrifuge Construct##27641 |q 13126/1 |only if havequest(13126)
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence |invehicle
step
goto 29.3,76.3
.from 3 Centrifuge Construct##27641 |only if not havequest(13126)
|confirm |only if not havequest(13126)
.kill 10 Centrifuge Construct##27641 |q 13126/1 |only if havequest(13126)
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence |invehicle
step
goto 45.5,26.0
.from Varos Cloudstrider##27447 |only if not havequest(13126)
|confirm |only if not havequest(13126)
.kill Varos Cloudstrider##27447 |q 13126/2 |only if havequest(13126)
step
goto 45.7,20.4
.talk 28012
.turnin 13126
step
goto 45.7,20.4
.talk 28012
.accept 13127
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence |invehicle
step
goto 57.4,33.0 |n
.' Fly up to this platform with your drake. |goto The Oculus/3 |noway |c
step
goto The Oculus/3,70.8,27.2
.' Attack Mage-Lord Urom.  When he vanishes, kill the mobs he summons.
.from Mage-Lord Urom##27655+, Phantasmal Naga##27648+, Phantasmal Ogre##27647+, Phantasmal Murloc##27649
|confirm
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence |invehicle
step
goto 50.4,87.8
.' Attack Mage-Lord Urom.  When he vanishes, kill the mobs he summons.
.from Mage-Lord Urom##27655+, Phantasmal Naga##27648+, Phantasmal Ogre##27647+, Phantasmal Murloc##27649
|confirm
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence |invehicle
step
goto 25.6,31.1
.' Attack Mage-Lord Urom.  When he vanishes, kill the mobs he summons.
.from Mage-Lord Urom##27655+, Phantasmal Naga##27648+, Phantasmal Ogre##27647+, Phantasmal Murloc##27649
|confirm
step
.' Use your Ruby Essence. |use Ruby Essence##37860 |or
.' Use your Amber Essence. |use Amber Essence##37859 |or
.' Use your Emerald Essence. |use Emerald Essence##37815 |or
.' Ride your Essence |invehicle
step
goto 39.3,36.1
.' For this fight, you will want to stay out of the _Frost Bombs_ he throws on the ground.
.from Mage-Lord Urom##27655 |only if not havequest(13127)
|confirm |only if not havequest(13127)
.kill Mage-Lord Urom##27655 |q 13127/1 |only if havequest(13127)
.turnin 13127
step
goto 39.3,36.1
.accept 13128
step
goto The Oculus/4,47.1,85.2
.' For Ley-Guardian Eregos, each Drake plays a vital role in this encounter.
.' To see what you will be doing based on the Essence you picked, _Click_ below:
.' Emerald Essence: Heals |confirm |next "emerald"
.' Ruby Essence: Tank |confirm |next "ruby"
.' Amber Essence:: Damage |confirm |next "amber"
step
label "emerald"
.' As the Emerald Drake, your job will be to heal your group.
.' You 3 abilities that you will be using:
.' _Leeching Poison_ puts a DoT on your target, siphoning health from that target to you.  It will be important that you keep 3 stacks up on the boss at all times at the very least.
.' _Touch the Nightmare_ damages you in order to reduce your enemies damage output.  It should be used on _Ley-Guardian Eregos_ only, however _DO NOT SPAM_ this ability.
.' _Dream Funnel_ transfers 5% of your health to a friendly target.  This along with Leeching Poison is what you should be using the most.
.from Ley-Guardian Eregos##27656 |only if not havequest(13128)
|confirm |only if not havequest(13128)
.kill Ley-Guardian Eregos##27656 |q 13128/1 |only if havequest(13128)
|next "turnin"
step
label "ruby"
.' As the Ruby Drake, your job will be to Tank the boss as well as the whelplings as best you can at all times.
.' _Searing Wrath_ is your only attack, and jumps to nearby opponents when used.
.' _Evasive Aura_ is an ability that gives you a charge whenever you're damage. The charge is used for yoru 3rd ability.
.' _Evasive Maneuvers_ is an ability that allows you to dodge all incoming damage.
.from Ley-Guardian Eregos##27656 |only if not havequest(13128)
|confirm |only if not havequest(13128)
.kill Ley-Guardian Eregos##27656 |q 13128/1 |only if havequest(13128)
|next "turnin"
step
label "amber"
.' As the Amber Drake, your job will be to damage Ley-Guardian Eregos.
.' _Shock Lance_ Will be your primary attack.
.' _Stop Time_ is an important ability, freezing all enemy targets when used.  It is important to use this when Ley-Guardian Eregos uses _Enraged Assault_.
.' _Temporal Rift_ is a channeled ability, which should be used until you have _10 Shock Charges_, at which point you will want to use _Shock Lance_.
.from Ley-Guardian Eregos##27656 |only if not havequest(13128)
|confirm |only if not havequest(13128)
.kill Ley-Guardian Eregos##27656 |q 13128/1 |only if havequest(13128)
|next "turnin"
step
label "turnin"
goto The Oculus/4,47.1,85.2
.talk 27658
.turnin 13128
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Halls of Lightning (HoL) 78-80",[[
dungeon 525
description This guide will walk you through the Halls of Lightning dungeon.
achieveid 486
step
label "lfg"
goto Halls of Lightning 11.9,53.8
.talk 56027
.accept 29860
.accept 29861
.accept 29862
step
map Halls of Lightning
path follow loose; loop off; ants straight
path	32.6,53.7	32.5,72.5	54.4,72.2
.' Go to each point and kill the mobs as you continue
.' Follow the path to this spot |goto 54.4,72.2 <5 |noway |c |only if not havequest(29862)
.kill 3 Stormforged Tactician##28581 |q 29862/2 |only if havequest(29862)
.kill 3 Stormforged Mender##28582 |q 29862/1 |only if havequest(29862)
step
goto 54.9,71.9
.' Wait for General Bjarngrim here. Don't engage him if he has the _Temporary Electric Charge_ buff.
.' Clear trash before engaging. Avoid him if there is trash around.
.' Clear trash up to this point if he is patting further away. [54.9,53.7]
.' Kill the _Stormforged Lieutenants_ before killing the boss.
.' If you get the _Arch Weld_ debuff, you _SHOULD NOT_ move.
.' Be sure you spread out for the fight.
.from General Bjarngrim##28586 |only if not havequest(28586)
|confirm |only if not havequest(28586)
.kill General Bjarngrim##28586 |q 29862/3 |only if havequest(28586)
step
goto 69.5,53.7
.' Once here, you will see an area full of _Slags_. Once you start going across, do not stop until you reach [88.0,54.1].
.' _Do not_ attack them until you're at the other side.
.' _Do not_ stand near them when you kill them.
.' Once cleared and up the steps, click here to proceed. |confirm
step
goto Halls of Lightning/2,41.8,23.1
.' _Volkhan_ will create _Molten Golems_ throughout the fight.  The Tank should pick them up.
.' When _Volkhan_ uses his _Shattering Stomp_ ability, the giants will explode and deal damage to anyone near them.
.' The more damage the _Molten Golems_ take, the slower the move, and at 1 Health they will eventually stop.
.from Volkhan##28587 |only if not havequest(29860)
|confirm |only if not havequest(29860)
.kill Volkhan##28587 |q 29860/1 |only if havequest(29860)
step
map Halls of Lightning/2
path follow loose;loop off;ants straight
path	47.2,41.3	47.0,53.5	51.4,54.2
.' Follow the path through the hallway, stopping at each waypoint.
.' At each of the waypoints, enemies will spawn.
.' Kill the _Titanium Siegebreakers_ first, they use a fear ability.
|confirm
step
goto 61.1,75.0
.' Ionar has 3 main abilities.
.' He will use _Ball of Lightning_ at random, which throws a ball that falls at moderate speed and can be avoided.
.' _Static Overload_ is placed on a random group member, damaging all party members around the target. Stay away from allies if you get this debuff.
.' When Ionar uses _Disperse_, run to [60.8,53.9].
.from Ionar##28546
|confirm
step
map Halls of Lightning/2
path follow loose;loop off;ants straight
path	47.2,69.3	40.5,65.3	29.4,75.8
.' Follow the path, killing trash at each waypoint.
.' Kill _Stormforged Runeshapers_ first, they do an Area of Effect attack that deals out massive damage.
.' There will be a _Titanium Vanguard_ patrol around the second waypoint. Wait before pulling.
.' Click here once you're safely at the last waypoint. |confirm
|modelnpc Titanium Vanguard##28838, Stormforged Runeshaper##28836+
step
goto 19.2,59.8
.' Loken has two main abilities to look out for:
.' The first is that he has an aura that deals nature damage.  The closer you are to him, the less damage it does. _Get as close to him as possible._
.' The Second ability, he will cast _Lightning Nova_. You will need to move 20 yards away from him to avoid the damage.
.from Loken##28923
.get Loken's Tongue |q 29861/1 |only if havequest(29861)
|confirm
step
goto 19.1,62.8
.talk 56027
.turnin 29860
.turnin 29861
.turnin 29862
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Utgarde Keep: Utgarde Pinnacle (UP) 78-80",[[
dungeon 524
description This guide will walk you through the Utgarde Keep: Utgarde Pinnacle dungeon.
achieveid 488
step
label "lfg"
goto Utgarde Pinnacle/2,45.1,18.1
.talk 56072
.accept 29864
step
goto 44.0,18.2
.talk 30871
.accept 13131
.accept 13132
step
.' As you go through the instance look in corners and on ledges for the following:
.click Golden Goblet##8367
.click Shiny Bauble##7554
.click Untarnished Silver Bar##6457
.click Jade Statue##8368
|confirm
step
map Utgarde Pinnacle/2
path follow loose;loop off;ants straight
path	43.0,35.7	34.0,37.5	35.3,68.9
path	42.4,76.7
.' Follow this path until you get downstairs. |goto Utgarde Pinnacle |noway |c
step
goto Utgarde Pinnacle,33.8,74.2
.' _Svala Sorrowgrave_ will use the ability _Ritual Preparation_ on a random party member. 3 Ghosts will appear, the DPS needs to kill the Ghosts to free the targeted member.
.' During Ritual Preparation, she will summon _Ritual of the Sword_, which will crash on the altor onto the trapped party member if you don't free them.
.from Svala Sorrowgrave##26668 |only if not havequest(29864)
.kill Svala Sorrowgrave##26668 |q 29864/1 |only if havequest(29864)
|confirm |only if not havequest(29864)
step
goto 46.1,81.5 |n
.' Go up the stairs. |goto Utgarde Pinnacle/2 |noway|c
step
goto Utgarde Pinnacle/2 61.8,84.1
.click Stasis Generator##7811
.' This will trigger an event where 3 out of 5 statues come to life, the third being the actual boss.
.' _Jormunger_ - leaves poison on the ground and summons non-elite adds.
.' _Rhino_ - Has a heavy stomp damage and randomly charges victims, leaving a dot.
.' _Furlbog_ - Casts Chain Lightning and random changes aggro, as well as an AoE Fear.
.' _Worgen_ -  Uses Mortal Strike on the tank.
.' When _Gortok Palehoof_ comes to life, he has 3 abilities to look out for:
.' _Arcing Smash_ Does cone damage wherever he is facing.
.' _Impale_ leaves a DoT on a random party member.
.' _Withering Roar_ decreases your maximum health by 500 and stacks.  The longer the fight lasts the harder it will become to heal.
.from Gortok Palehoof##26687
|confirm
step
goto 68.7,65.1
.' Aggroing the boss will trigger an event. Ymirjar mobs will come to fight you and you will have to battle them to [69.2,36.5].
.' The boss will be flying overhead, covering the ground with frost as you move along the path, avoid it.
.' When you kill Harpooners, they will drop their weapons.
.click Harpoon##5994
.' When you get the message _"Skadi the Rutheless is within range of the harpoon launchers!"_ click them to shoot him down.
.click Harpoon Gun##8245
.' You will need to shoot him 3 times before he comes down.
.' When _Skadi the Ruthless_ jumps down, _Taunt_ him immediately if you're the tank.
.' Tanks will also need to kite him around when he uses _Whirlwind_.
.' Skadi will randomly throw _Poisoned Spears_ at random party memebers.  If possible, it should be dispelled immediately (Poison).
.from Skadi the Ruthless##26693
|confirm
step
goto 60.3,37.9 |n
.' Go down the steps. |goto Utgarde Pinnacle |noway|c
step
goto Utgarde Pinnacle,48.7,44.3
.' Go up the ramp here. |goto Utgarde Pinnacle |noway|c
step
goto Utgarde Pinnacle/2 42.6,53.5
.' At 80% and 30%, _King Ymiron_ will use _Screams of the Dead_ Stunning the entire party for 8 seconds.
.' He will often use _Dark Slash_ damaging targets with shadow damage, dealing out half the targets current health.
.' He will use _Bane_ which will trigger when he is hit with the buff, damaging the group with AoE Damage.
.' He also places _Fetid Rot_ on random party members, which should be dispelled immediately if possible (Disease).
.from King Ymiron##26861 |only if not havequest(13132)
.kill King Ymiron##26861 |q 13132/1 |only if havequest(13132)
|confirm |only if not havequest(13132)
step
'Make sure you have collected the following:
.click Golden Goblet##8367
.get 5 Untarnished Silver Bar |q 13131/1
.click Shiny Bauble##7554
.get 3 Shiny Bauble |q 13131/2
.click Untarnished Silver Bar##6457
.get 2 Golden Goblet |q 13131/3
.click Jade Statue##8368
.get Jade Statue |q 13131/4
|only if havequest(13131)
step
goto 45.1,18.1
.talk 56072
.turnin 29864
step
goto 44.0,18.2
.talk 30871
.turnin 13131
.turnin 13132
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Forge of Souls (FoS) 80",[[
dungeon 601
description This guide will walk you through the Forge of Souls dungeon.
achieveid 4516
step
label "lfg"
goto The Forge of Souls 64.0,91.2
.talk 37596
.turnin 24506
step
goto The Forge of Souls 64.0,91.2
.talk 37596
.accept 24511
step
map The Forge of Souls
path follow loose; loop off
path	61.9,83.5	69.7,73.7	69.3,63.8
path	58.9,60.2	54.2,62.8	48.9,56.6
path	43.6,53.0
.' Follow this path and kill any enemies you come in contact with. |goto 43.6,53.0 <5 |noway|c
step
goto 43.3,49.8
.' _Bronjahm's_ Has two phases.
.' He casts _Shadow Bolt_ if you're out of range.
.' He uses an ability called _Corrupt Soul_ which after 4 seconds creates an orb.  If it reaches Bronjahm, it will heal him.
.' When he reaches 30% health, he'll cast _Soulstorm_. You'll want to move into melee range or you will take damage from it.
.' Once he's in the _Soulstorm_ phase, he'll cast _Fear_ on a random target.
.from Bronjahm##36497 |only if not havequest(36497)
.kill Bronjahm##36497 |q 24511/1 |only if havequest(36497)
|confirm |only if not havequest(36497)
step
map The Forge of Souls
path follow loose; loop off
path	42.9,45.1	48.3,41.8	43.9,31.0
path	49.4,19.4
.' Follow this path and kill any enemies you come in contact with. |goto 49.4,19.4 <5 |noway |c
step
goto The Forge of Souls 45.6,14.9
.' The _Devourer of Souls_ will use an ability called _Phantom Blast_ which should be interrupted.
.' He will put a debuff on a party member called _Mirrored Soul_ which dishes out a percentage of the damage done to him to the target.
.' He will randomly leap at a party member with _Well of Souls_ which dishes out shadow damage to any who stand in it's radius.
.' He will occasionally cast _Wailing Souls_, shooting out a pink beam.  Stay out of it.
.from Deourer of Souls##36502 |only if not havequest(24511)
.kill Deourer of Souls##36502 |q 24511/2 |only if havequest(24511)
|confirm |only if not havequest(24511)
step
goto 44.0,15.3
.talk 38161
.turnin 24511
step
goto 44.0,15.3
.talk 38161
.accept 24682
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Pit of Saron (PoS) 80",[[
dungeon 602
description This guide will walk you through the Pit of Saron dungeon.
achieveid 4517
step
label "lfg"
goto Pit of Saron,40.8,79.8
.talk 36990
.turnin 24682
step
goto Pit of Saron,40.8,79.8
.talk 36990
.accept 24507
step
map Pit of Saron
path follow strict;loop off;ants straight
path	48.8,69.4	53.5,65.3	56.6,65.7
path	55.8,62.7	58.4,61.3	63.5,59.3
path	62.2,55.0	63.0,49.5	59.9,49.7
.click Ball and Chain##181
.' 15 Horde Slaves Freed. |q 24507/1
|only if havequest(24507)
step
map Pit of Saron
path follow loose; loop off
path	43.9,70.5	50.1,67.6	54.6,63.0
path	64.2,55.0
.' Follow this path, killing any mob you come in contact with. |goto 64.2,55.0 <5 |noway |c
|only if not havequest(24507)
step
goto 66.4,55.5
.' _Forgemaster Garfrost_
.' _Chilling Wave_ is an attack that he shoots at a party member, dealing AoE Damage to anyone in it's radius.
.'	_Deep Freeze_ is randomly casted at party members, slowing their movement and inflicting frost damage.
.' If you stand within 40 yards of him you will get a debuff called _Permafrost_, increasing frost damage taken. This effect stacks.
.' He will occassionwally _Throw Sarnoite_ at a random party target.
.from Forgemaster Garfrost##36494 |only if not havequest(24507)
.kill Forgemaster Garfrost##36494 |q 24507/2
|confirm |only if not havequest(24507)
step
goto 65.6,55.0
.talk 37592
.turnin 24507
step
goto 65.6,55.0
.talk 37592
.accept 24712
step
goto 49.4,44.8
.from Plagueborn Horror##36879+
.' Clear them, then click here to continue. |confirm
step
goto 47.4,41.5
.' _Ick_ will cast _Explosive Barrage_ randomly. Players will need to move continueously to avoid damage.
.' When Ick uses _Poison Nova_, run away from him.
.' Ick will randomly use _Pursuit_ on a target during the fight. You will need to run away from him if you're the target.
.' _Krick_ will use _Shadow Bolt_ on random party members.
.' He tosses _Toxis Waste_ on the ground, that you shouldn't stand in.
.from Ick##36476
|confirm
step
map Pit of Saron
path follow strict;loop;ants straight
path	50.9,36.6	49.9,33.3	52.8,30.5
path	56.7,32.0
.' There will be monsters that need to be cleared at each waypoint on the map.
.' Make sure that the _casters die first_.
.' Click here once you have cleared them. |confirm
step
goto 60.0,31.0
.' Once you get to this point, you will have reached a tunnel entrace.
.' Let your tank go in before anyone else.
.' Collect the mobs as you go along.
.' Stop at [60.9,19.8] to clear the mobs.
.' Avoid blue circles that appear on the ground.
.' Go to  [47.5,19.4] next.
|confirm
step
.' _Scourgelord Tyrannus_
.' He will be flying high up in the air. Enter the room to start the event.
.' _Make sure you DO NO HIT HIM_ before he dismounts.  He may bug if you do so.
.' Randomly through the fight, he will mark a party member with _Mark of Rimefang_. That member will have to move away from the rest of the group.
.' He will randomly place _Overlord's Brand_ on a DPS in the group.  All damage that DPS does is transfered to the Tank, so don't dps if you get this debuff.
.' When he _Roars and Swells with Dark Might_ his movement speed will be slowed.  You should kite him through this until it wears off.
.from Scourgelord Tyrannus##36658 |only if not havequest(24712)
.kill Scourgelord Tyrannus##36658 |q 24712/1 |only if havequest(24712)
|confirm |only if not havequest(24712)
step
goto Pit of Saron 48.2,18.9
.talk 36990 |tip you will have to wait for her to show up.
.turnin 24712
step
goto Pit of Saron 48.2,18.9
.talk 36990
.accept 24713
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Northrend Dungeons\\Halls of Reflection (HoR) 80",[[
dungeon 603
description This guide will walk you through the Halls of Reflection dungeon.
achieveid 4518
step
goto Halls of Reflection 45.3,76.8
.talk 37223
.turnin 24713 |only if havequest(24713)
step
goto 36.2,76.3
.' For this trash, have your group Line of sight at the wall here for casters.
.' Groups of enemies will attack in waves.
.' Kill priority for these groups should be Ghostly Priests, Shadowy Mercenary, Phantom Mage, Tortured Rifleman then Spectral Footman when applicable.
.' At wave 5, you will face _Falric_. Click here to move onto his strategy. |confirm
|modelnpc Ghostly Priest##38175
|modelnpc Shadowy Mercenary##38177
|modelnpc Phantom Mage##38172
|modelnpc Tortured Rifleman##38176
|modelnpc Spectral Footman##38173
step
.' Falric uses the ability _Defiling Horror_ which is a fear effect that damages you as well.
.' Every time he fears, he gives players a stack of _Hopelessness_, which reduces the healing received.  The longer the fight, the harder healing will become.
.' He will cast _Impending Despair_, which will stun you for 6 seconds if not dispelled within 6 seconds. It is a magic effect.
.' He will use _Quivering Strike_ on the tank, dropping their dodge by 20%. The strike is unavoidable.
.from Falric##38112
|confirm
step
.' After _Falric_ is defeated, the waves will continue.
.' Again, kill priority for these groups should be Ghostly Priests, Shadowy Mercenary, Phantom Mage, Tortured Rifleman then Spectral Footman when applicable.
.' At wave 10, you will face _Marwyn_. Click here to move onto his strategy. |confirm
|modelnpc Ghostly Priest##38175, Shadowy Mercenary##38177, Phantom Mage##38172, Tortured Rifleman##38176, Spectral Footman##38173
step
.' Marwyn has the ability _Corrupted Flesh_, reducing the groups maximum health by 25% for 8 seconds.
.' Everytime he uses Corrupted Flesh, he will also use _Shared Suffering_ which deals out massive shadow damage.  It should be dispelled, but note that when it is, it splits the damage between the group. Be sure everyone is at a reasonable amount of health before dispelling.
.' Throughout the fight, he will toss random _Well of Corruptions_ on the ground, which will increase the shadow damage you take.  Simply move out of them.
.from Marwyn##38113
|confirm
step
goto 26.1,51.1
.from Frostsworn General##36723
.' During the fight, he will spawn mirror images of your group.
.' Be sure not to stand close as you kill them. They explode when they are killed.
|confirm
step
goto 12.9,23.2
.talk 37554
.accept 24802 |only if not completedq(24802) and not havequest(24802) and completedq(24713)
.' Find Sylvanas Windrunner. |q 24802/1 |only if havequest(24802)
.' When your group is ready, talk to Lady Sylvanas and tell her Let's go! Click to proceed.
|confirm
step
goto 27.5,27.5
.' Fight the undead as The Lich King summons them.
.' Generally, he will send _Raging Ghouls_ followed by _Risen Witch Doctors_ and _Lumbering Abomination_
.' Kill _Risen Witch Doctors_ first, as they will damage your group more than any of the others.
.' Face _Lumbering Abominations_ away from the group.
.' At each of the waypoints, there will be more and more mobs. You will need to DPS the enemies down fast, or the Lich King will kill you.
.' next [40.5,36.1]
.' next [51.1,44.9]
.' next [66.6,65.0]
.' next [77.3,77.6]
.' Fight your way to the ship |goto 77.3,77.6 |only if not havequest(24802)
.' Escape the Lich King |q 24802/2 |only if havequest(24802)
step
goto 75.9,76.8
.talk 37554
.turnin 24802
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Abyssal Maw: Throne of the Tides (TotT) 80-82",[[
dungeon 767
description This guide will walk you through the Abyssal Maw: Throne of the Tides dungeon.
achieveid 4839
step
label	"start"
goto Throne of the Tides,49.9,90.6
.talk 50272
.accept 28833
.accept 28843
step
goto 50.0,39.4 |n
.' Enter this elevator room. |goto Throne of the Tides/2 |noway |c
step
goto 50.1,84.4
.' Ride the Elevator up.
.' When it's at the bottom you will have to jump to get on it.
|confirm
step
.' Run to the other side of the tunnel.
.' Combat won't end so you will need to fight you way through the waves of monsters. |goto 50.5,52.1 <5 |c
step
goto 50.6,21.4
.' _Lady Naz'jar_
.' This boss will use 4 abilities.
.' She will cast _Summon Geyser_ on the ground under a random players feet. Move out and away from the blue circle on the ground.
.' She will often cast _Shock Blast_ is a spell that can be interrupted.
.' She will cast _Fungal Spores_ on the ground that will leave a damage over time effect on any players caught with it.
.' At 66% and 33% she will cast _Waterspout_ which makes her immune to damage.  At this point she will summon 3 mobs, 2 casters and 1 melee. Control the casters and kill the melee.
.kill Lady Naz'jar##40586 |q 28843/1 |only if havequest(28843)
.from Lady Naz'jar##40586 |only if not havequest(28843)
|confirm |only if not havequest(28843)
|only if not heroic_dung()
step
goto 50.6,21.4
.' _Lady Naz'jar_
.' This boss will use 4 abilities.
.' She will cast _Summon Geyser_ on the ground under a random players feet. Move out and away from the blue circle on the ground.
.' She will often cast _Shock Blast_ is a spell that can be interrupted.
.' She will cast _Fungal Spores_ on the ground that will leave a damage over time effect on any players caught with it.
.' At 66% and 33% she will cast _Waterspout_ which makes her immune to damage.  At this point she will summon 3 mobs, 2 casters and 1 melee. Control the casters and kill the melee.
.' While she is in the _Waterspout_, she will send out _Water Vortex's_ which will move from Lady Naz'jar, towards a party member. These will suck anyone in that is close enough while stunning and causing damage to them for 5 seconds.
.kill Lady Naz'jar##40586 |q 28843/1 |only if havequest(28843)
.from Lady Naz'jar##40586 |only if not havequest(28843)
|confirm |only if not havequest(28843)
|only if heroic_dung()
step
goto Throne of the Tides/2 50.8,33.1
.click Throne of Tides Defense System##7147
|confirm
step
goto 50.6,41.8
.' _Commander Ulthok_
.' He uses 4 main abilities.
.' _Squeeze_ will be used on a random party member, restricting them from any actions.
.' He will use _Dark Fissure_ where the tank is standing.  It's important to move away from him when he does this as this attack may one-shot you.
.' Note that when he uses _Dark Fissure_ it leaves a circle of shadow on the ground that slowly expands the longer the fight carries on.
.' He casts _Curse of Fatigue_ at a random target, slowing their movement and haste by 50%
.' He will Enrage increasing all damage done by 50%.
.from Commander Ulthok##40765
|confirm
step
goto 51.2,52.7 |n
.clicknpc Throne of the Tides Teleporter##51391
.' Teleport to the first floor. |goto Throne of the Tides |noway |c
step
map Throne of the Tides
path follow loose; loop off; ants straight
path	57.3,44.6	61.9,43.7	66.2,38.6
path	67.1,28.8
.' Kill the trash at these points above. |goto 67.1,28.8 <5 |noway |c
step
goto Throne of the Tides 67.2,20.4
.' _Erunak Stonespeaker & Mindbender Ghur'sha_
.' This fight will have 2 phases:
.' Phase 1:
.' Erunak will cast _Earth Shards_ at a random party member. It will cause damage in a line, so move out of it.
.' He will cast _Lava Bolt_ at a random target.  This spell can be interrupted.
.' He will use _Magma Splash_ in a cone in front of him.
.' Phase 2:
.' _Mindbender Ghur'sha_ will put _Mind Fog_ on the ground, rendering you unable to cast or attack, as well as causing shadow damage.
.' He will randomly _Enslave_ a party member. You will have to beat the member down until they reach 50% helath.
.' Note that if you don't free the player, they will die. |only if heroic_dung()
.' The most important ability to watch out for is _Absorb Magic_. All spells cast at him while this ability is up will heal him.
.from Mindbender Ghur'sha##40788
.' Rescue Erunak Stonespeaker |q 28833/1 |only if havequest(26193)
|confirm |only if not havequest(28833)
step
goto 67.2,20.4
.talk 40825
.turnin 28833
step
map Throne of the Tides
path follow loose; loop off
path	44.3,44.5	37.1,43.5	33.0,37.3
path	32.6,29.6
.' As you run through this hallway, kill the Elite Water mobs. Once you ahve killed all the Elites, you can continue through the gate.
.' Lots of non-elite mobs will also appear and start fighting here.
.' Run through the gate after it opens. |goto 32.6,29.6 <5 |noway |c
step
goto 32.5,22.5
.talk 40792
.' Let him know you're ready.
|confirm
step
.' Phase 1:
.' _Deep Murloc Invaders_ will spawn in groups. Often, and should be AoE'd down.
.' There will be 3 _Vicious Mindlashers_ that spawn, and need to be tanked.
.' There will be 1 _Unyielding Behemoth_ that spawns and needs to be tanked.
.' After the 3 Vicious Mindlashers and Unyielding Behemoth are killed, 3 _Faceless Sappers_ will spawn.
..' Phase 2:
..' Kill the _Faceless Sappers_ 1 at a time, focusing them down quickly.
..' The tank should look for _Blight Beasts_, kiting them around the room as they spawn.
..' Once the 3 Faceless Sappers are defeated, Ozumat will appear at [36.4,17.4].
...' Phase 3:
...' You will gain a buff that will increase your damage, healing, health and size.
...' Kill any _Blight Beasts_ that have spawned.
...' Burn down _Ozumat_ as quickly as possible, he will lay _Blight of Ozumat_ on the ground which deals 200 shadow damage per second and stacks 200 times.
|modelnpc 3 Faceless Sapper##44752, Vicious Mindlasher##44715, Unyielding Behemoth##44648, Blight Beast##44841, Ozumat##44566
|confirm
step
goto Throne of the Tides,49.9,90.6
.talk 50272
.turnin 28843
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Blackrock Caverns (BRC) 80-82",[[
dungeon 753
description This guide will walk you through Blackrock Caverns dungeon.
achieveid 4833
step
label	"start"
goto Blackrock Caverns,33.1,66.0
.talk 49476
.accept 28735
step
.' Clear the trash at these coordinates.
.' _Rom'ogg Bonecrusher_ patrols nearby, if you aggro him, he will do a shout attracting the nearby groups.
'|model 39665
'|goto 49.3,74.1
'|goto 51.1,70.3
.' Click here when you've cleared the groups. |confirm
step
.' _Rom'ogg Bonecrusher_
.' Pull _Rom'ogg Bonecrusher_ when he's at [50.0,67.3].
.' For this fight, he will use _Call for Help_ when you engage  him. Any mob nearby that you didnt kill will come to his aid.
.' He will often use _Quake_, which can be avoid by moving to the side when you see him casting it.
.' He will also summon _Angered Earths_ each time he uses Quake, which need to be picked up by your tank. |only if heroic_dung()
.' At 66% and 33% he will use _Chains of Woe_ followed by _The Skullcracker_.
.' Players will need to _kill the Chains of Woe_, then move away from the boss in order to avoid massive. It takes 12 seconds for him to wind up for the damage. |only if not heroic_dung()
.' Players will need to _kill the Chains of Woe_, then move away from the boss in order to avoid massive. It takes 8 seconds for him to wind up for the damage. |only if heroic_dung()
.' He will also use _Wounding Strike_ on the tank, reducing healing received by 25%.
.from Rom'ogg Bonecrusher##39665 |only if not havequest(28735)
.kill Rom'ogg Bonecrusher##39665 |q 28735/1 |only if havequest(28735)
|confirm |only if not havequest(28735)
step
'Click the Quest Complete Box at the top right of your screen
.turnin 28735
step
map Blackrock Caverns
path follow strict;loop off;ants straight
path	50.1,55.2	50.1,39.2	53.9,21.5
.' Follow this path. |goto 53.9,21.5 <5 |noway |c
.' Click the Quest Complete Box at the top right of your screen.
.accept 28737
step
goto 54.0,20.4
.' There will be an _Evolved Twilight Zealot_ patrolling here.
.' Interrupt _Shadow Stike_ whenever he uses it.
.from Evolved Twilight Zealot##39987
|confirm
step
goto 51.9,20.0 |n
.' Jump down the ledge here. |goto Blackrock Caverns/2 |noway|c
step
goto 30.3,16.6
.' There will be a group of 5 patrolling the hallway here.
.' Once they are defeated, there will be another _Evolved Twilight Zealot_ on the ramp at [29.6,14]
.' Interrupt _Shadow Stike_ whenever he uses it.
.from Evolved Twilight Zealot##39987
|confirm
step
goto 27.4,16.6
.' _Corla, Herald of Twilight_
.' You will see two beams on each side of Corla. You should have _Ranged DPS_ or _Healers_ step into the beams during the encounter. |only if not heroic_dung()
.' You will see three beams on each side of Corla. You should have _Ranged DPS_ or _Healers_ step into the beams during the encounter. |only if heroic_dung()
.' This will give whoever interrupts the beams stacks of _Evolution_, when you reach 70, step out of the beams.  You will need to move back in when your stack falls back off.
.' Failing to interrupt the beams, or to move out when you reach 70 will result in _Twilight Evolution_, which will enrage either a player or the minions into attack your party members, and likely wipe you.
.' Corla, Herald of Twilight will cast _Dark Command_ on a random party member. It will be important to interrupt this, as it can ruin the _Evolution_ rotations.
.from Corla, Herald of Twilight##39679 |only if not havequest(28737)
.kill Corla, Herald of Twilight##39679 |q 28737/1 |only if havequest(28737)
|confirm |only if not havequest(28737)
step
'Click the Quest Complete Box at the top right of your screen.
.turnin 28737
step
map Blackrock Caverns/2
path follow strict;loop off;ants straight
path	34.2,13.3	37.5,13.2	39.2,16.0
.' Hug the wall, following the way points.
.' There will be trash at the last waypoint.
.' Follow the ramp down and click here to proceed. |confirm
step
goto 43.5,57.7
.accept 28738
.' There will be 2 fire elementals patrolling around the room with _Karsh Steelbender_.
.from Conflagration##39994
.' Pull one at a time back to the ramp.
|confirm
step
goto 45.6,65.6
.' _Karsh Steelbender_
.' For this fight, there will be a large pillar of flame at the center of the room.
.' The tank will need to run through the center, giving the boss a _Superheated Quicksilver Armor_ debuff, increasing the damage the boss takes.
.' Each time you drag Karsh through the flames, 3 _Bound Flames_ will spawn, and attack nearby players. |only if heroic_dung()
.' When the debuff has 5 seconds left, the tank will have to _Run_ through the pillar of flame again, renewing and adding another debuff to the boss.
.' If you let _Superheated Quicksilver Armor_ fall off the boss,  he will spawn adds, as well as his _Quicksilver Armor_ buff.
.from Karsh Steelbender##39698 |only if not havequest(28738)
.kill Karsh Steelbender##39698 |q 28738/1 |only if havequest(28738)
|confirm |only if not havequest(28738)
|only if not heroic_dung()
step
'Click the Quest Complete Box in the top right of your screen.
.turnin 28738
step
map Blackrock Caverns/2
path follow loose;loop off;ants straight
path	53.5,67.2	55.7,68.3	58.5,64.9
path	61.6,71.8
.' Kill trash at the waypoints.
.' The second group should be 2 Defiled Earth Ragers. Everyone in the group needs to stand close, as they do an AoE that does more damage the further away you are.
.' Be sure you hug the wall on the left up until the last waypoint.
|confirm
step
goto 66.1,71.6
.' Click the Quest Complete Box at the top right of your screen
..accept 28740
step
goto 66.3,77.5
.' There will be 3 Corehound Pups. You can pull them without pulling the boss behind them.
.from Lucky##40008 |n
.from Buster##40013 |n
.from Spot##40011 |n
|confirm
|only if not heroic_dung()
step
.' _Beauty_
.' You will need to use crowd control on _Lucky, Buster and Spot_ or kill them before engaging the boss. |only if heroic_dung()
.' _DO NOT_ kiill _Runty_, or the boss will enrage and deal incredible damage. |only if heroic_dung()
.' She will randomly cast _Terrifying Roar_, fearing the entire party. Tremor Totem and Fear Ward can be used to avoid this.
.' She will _Berserker Charge_ an enemy, knocking them back.
.' She will also use _Flamebreak_ on melee knocking them back.
.' She casts _Magma Spit_ on party members, causing AoE damage when it is dispelled.
.from Beauty##39700 |only if not havequest(28740)
|confirm |only if not havequest(28740)
.kill Beauty##39700 |q 28740/1 |only if havequest(28740)
|only if not heroic_dung()
step
.' _Beauty_
.' You will need to use crowd control on _Lucky, Buster and Spot_ or kill them before engaging the boss.
.' _DO NOT_ kill _Runty_, or the boss will enrage and deal incredible damage.
.' She will randomly cast _Terrifying Roar_, fearing the entire party. Tremor Totem and Fear Ward can be used to avoid this.
.' She will _Berserker Charge_ an enemy, knocking them back.
.' She will also use _Flamebreak_ on melee knocking them back.
.' She casts _Magma Spit_ on party members, causing AoE damage when it is dispelled.
.from Beauty##39700 |only if not havequest(28740)
.kill Beauty |q 28740/1 |only if havequest(28740)
|confirm |only if not havequest(28740)
|only if heroic_dung()
step
'Click the Quest Complete Box in the top right of your screen.
.turnin 28740
step
goto 69.6,72.2
.' Wait here for the elemental patrol
|confirm
step
goto 70.0,63.4
.' Click the Complete Quest Box in the top right of your screen
.accept 28741
|only if not completedq(28741) and not havequest(28741)
step
goto 68.8,56.0
.' _Ascendant Lord Obsidius_
.' When you get within aggro radius, he will be attacked. Raz will be killed then it will be time to face the boss.
.' Ascendant Lord Obsidius will split into 3 forms.
.' During the fight, he will switch bodies with one of the other two clones and you will have to switch dps.
.from Ascendant Lord Obsidius##39705
|confirm
|only if not havequest(28741)or not heroic_dung()
step
goto 68.8,56.0
.' _Ascendant Lord Obsidius_
.' When you get within aggro radius, he will be attacked. Raz will be killed then it will be time to face the boss.
.' Ascendant Lord Obsidius will split into 3 forms.
.' There will be a big one and two little ones. The big one needs to be tanked.
.' The other two need to be kited by a ranged dps. |only if heroic_dung()
.' During the fight, he will switch bodies with one of the other two clones and you will have to switch dps.
.from Ascendant Lord Obsidius##39705 |only if not havequest(28741)
.kill Ascendant Lord Obsidius##39705 |q 28741/1 |only if havequest(28741)
step
goto 68.8,56.0
.talk 49476
..turnin 28741
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\The Stonecore (SC) 82-84",[[
dungeon 768
description This guide will walk you through the Stonecore dungeon.
achieveid 4846
step
label	"start"
goto The Stonecore 53.7,94.9
.talk 50048
..accept 28815
step
goto The Stonecore 54.6,88.4
.from Stonecore Earthshaper##43537+
.' Make sure to kill him first, if not killed fast enough, he will transform and do big group damage.
.' You will have to fight him in each group as you continue forward.
|confirm
step
goto 63.5,65.2
.click Twilight Documents##183
.get Twilight Documents |q 28815/1
|only if havequest(28815)
step
'Click the Quest Complete Box in the top right of your screen
..turnin 28815
step
'Click the Quest Complete Box in the top right of your screen
..accept 28814
step
goto The Stonecore 62.4,61.2
.from Corborus##43438
.' Corborus has 2 phases to his fight.
.' _Phase 1_ Corborus will attack the Tank and use _Crystal Barrage_. These crystals will do damage to anyone standing in them. |tip In horoic mode, these will also spawn Crystal Shards to attack. Make sure to AoE these down quickly.
.' _Crystal Barrage_ will spawn adds that need to be picked up quickly. |only if heroic_dung()
.' Additionally, he will randomly use _Dampening Wave_, which will reduce the healing the party takes, as well as dealing damage. Remove this from the tank as soon as possible. (Magic Effect)
.' _Phase 2_ Corborus will burrow. During this time avoid the ares with rubble when he is underground, he will jump out of these areas and hurt anyone standing in it.
|confirm
|only if not heroic_dung()
step
goto 37.7,44.9
.from Slabhide##43214
.' This is a pretty basic fight. Tank the boss in the middle of the room, and face him away from group.
.' He will spawn _Lava Fissure_ under some players feet, move from this to not be hurt by fire. |tip In heroice mode, this will leave a pool of lava.
.' He will drops _Stalactite's_ from the ceiling, move fromt he shadow to not get hurt.
.' After the Stalactite's, cast _Crystal Storm_, move behind the rocks out of his line of site to avoid damage. |only if heroic_dung()
|confirm
step
goto 38.7,20.6
.' As you run through this room, you will want to kill the _Stonecore Sentrys_, they will aggro big groups if they are close enough.
.' _ Stonecore Bruisers_ Patrol this area. When you can, kill them away from groups. |tip If you stay to the right side, you will not have to kill groups on the left side.
|modelnpc Stonecore Bruiser##42692+, Stonecore Sentry##42695+
|confirm
step
goto 47.7,20.0
.' _Ozruk_
.' Face Ozruk away from the group. He will use _Ground Slam_ that everyone, including the tank need to avoid. This causes massive damage.
.' After Ground Slam, he will use _Rupture_, which will cause 3 lines to appear from the ground. They will cause a pillar of spikes to shoot up and should be avoided at all costs. |only if heroic_dung()
.' Ozruk will cast _Shatter_ shortly after Ground slam. Run 15 yards away from him or you will take damage up to 45k. |tip In heroic this will cause up to 200k damage.
.' He will cast _Paralyze_, which needs to be dispelled.  You can also cast a DoT when Ozruk has_Elementium Bulwark_ up, or simply melee him keeping the _Elementium Spike Shield_ debuff on you at all times to breka the effect. |only if heroic_dung()
.from Ozruk##42188
|confirm
step
goto 57.64,38.24
.' There are 2 stages to this fight.
.' _Phase One_ She will need to be tanked and moved when she creates a _Gravity Well_ (purple circle with that deals shadow damage).
.' Gravity Well will shrink as it kills units, disappearing after it kills 4 units.
.' During this phase, boss will need to be tanked on the left of the Gravity Well, so the mobs will run in and die.
.' She will also use an ability called _Force Grip_ which deals massive damage to the tank and should be interrupted each time it is used. |only if heroic_dung()
.' _Phase Two_ She will float up to her altar and Tank will need to get aggro on all the _Devout Followers_ that run in.
.' She will use _Seismic Shard_, which throws a huge rock at a random party member. More often than not it will kill that party member, so be on the look out for a shadow on the ground. |only if heroic_dung()
.from High Priestess Azil##42333 |only if not havequest(28814)
.kill High Priestess Azil##42333 |q 28814/1 |only if havequest(28814)
.kill 101 Devout Follower##42428 |q 28814/2 |only if havequest(28814)
|confirm |only if not havequest(28814)
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto 53.6,95.0
.talk 50048
..turnin 28814
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Vortex Pinnacle (VP) 82-84",[[
dungeon 769
description This guide will walk you through the Vortex Pinnacle dungeon.
achieveid 4847
author support@zygorguides.com
step
label	"start"
goto The Vortex Pinnacle,54.4,16.6
.talk 49943
..accept 28779
..accept 28760
step
goto 55,44.6
.' _Grand Vizier Ertan_
.' When engaged, she will summon whirlwinds at the outside of the room.
.' Move in towards her to avoid them for the duration of the fight.
.' As the fight progresses, Grand Vizier Ertan will pull the whirlwinds to form a wall around her.  _Move outward, away from her, avoiding the whirlwinds!_
.' If you're hit by the whirlwinds you will receive a debuff, decreasing movement, casting and attack speed as well as take nature damage.
.from Grand Vizier Ertan##43878
.get Grand Vizier Etran's Heart##65660 |q 28779/1 |only if havequest(28779)
|confirm |only if not havequest(28779)
|only if not heroic_dung()
step
goto 55,44.6
.' _Grand Vizier Ertan_
.' When engaged, she will summon whirlwinds at the outside of the room.
.' Move in towards her to avoid them for the duration of the fight.
.' As the fight progresses, Grand Vizier Ertan will pull the whirlwinds to form a wall around her.  _Move outward, away from her, avoiding the whirlwinds!_
.' If you're hit by the whirlwinds you will receive a debuff, decreasing movement, casting and attack speed as well as take nature damage.
.from Grand Vizier Ertan##43878
.get Grand Vizier Etran's Heart##65660 |q 28779/1 |only if havequest(28779)
|confirm |only if not havequest(28779)
|only if heroic_dung()
step
goto The Vortex Pinnacle 57.6,48.9  |n
' Ride the Slipstream across the gap. |goto 63.6,59.1 <5 |noway|c
step
goto 60.6,65.3 |n
.kill Young Storm Dragon##45919 |n
.' When you pull these, move it away from the circle or it will heal for all the damage you do. |confirm
step
goto 58.4,68.9 |n
.' There will be _Howling Gales_ that you will need to hit to cross the bridge.
.' Cross the bridge. |goto 55.2,73.6 <5 |noway|c
step
goto 60.6,65.3 |n
.kill Young Storm Dragon##45919 |n
.' When you pull these, move it away from the circle or it will heal for all the damage you do. |confirm
step
goto 53.0,77.7
.' For this trash, move out of _Hurricane_.
.' Interrupt _Cloudburst_ when it's cast.
.' Kill Empyrean Assassins first.
'|modelnpc Empyrean Assassin##45922
'|modelnpc Turbulent Squall##45924
|confirm
step
goto 52,80.6
.' _Altairus_
.' When engaged, Altairus will summon a wind which will affect your haste.
.' He will also summon _Twisting Winds_, which will toss you up in the air and deal damage if you're hit by them. |only if heroic_dung()
.' Using the wind current to your advantage will be the deciding factor in this fight.
.' You will need to navigate around the boss. When you have the _Upwind of Altairus_ buff, stop moving. You gain a 100% haste buff for spells and attacks.
.' If you have the _Downwind of Altairus_ debuff, circle the boss until you have the Upwind buff.
.' Do not stand in front of Altairus. He uses a cone breath ability.
.from Altairus##43873
|confirm
step
goto 49.4,80.7
' Ride the Slipstream across the gap. |goto 31.4,80.4 <8 |noway|c
step
goto 30.8,76.2
.' For these trash pulls, always kill or use crowd control for _Temple Adepts_.
|confirm
step
goto 32.1,69.1
.' There will be an electric field in the area. Magic won't work on mobs in these fields.
.' Use crowd control on _Temple Adepts_.
|confirm
step
goto 27.1,60.3
.from Skyfall Star##52019
.' There will be no aggro for these mobs and they will spread further apart the longer the fight goes.
.' AoE them down.
|confirm
step
goto 29.8,39
.' _Asaad_
.' He will channel _Supremacy of the Storm_. When he does so he will start to create a triangle on the ground randomly in the room.
.' You will need to run inside of the triangle.
.' His primary attack is _Chain Lightning_ so try to spread out of possible to reduce group damage.
.' He will cast _Static Cling_ which will root you to the ground and can be deadly. It is a _Magic_ effect and can be cleansed. |only if heroic_dung()
.from Asaad##43875 |only if not havequest(28760)
.kill Asaad##43875 |q 28760/1 |only if havequest(28760)
|confirm |only if not havequest(28760)
step
goto 29.2,46.4  |n
.clicknpc Slipstream##47066
|goto 54.1,16.8,0.5 |noway|c
step
goto 54.5,15.9
.talk 49943
..turnin 28779
..turnin 28760
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Lost City of the Tol'vir (LCoT) 83-85",[[
dungeon	747
description This guide will walk you through the Lost City of the Tol'vir dungeon.
achieveid 4848
author support@zygorguides.com
step
label	"start"
goto Lost City of the Tol'vir 31.7,16.3
.talk 50038
..accept 28783
..accept 28781
step
goto 30.1,34.6
.from 1 Oathsworn Captain##45122 |only if not havequest(28781)
.kill 1 Oathsworn Captain##45122 |q 28781/2 |only if havequest(28781)
.' The Captain will cast an AoE spell on the ground. Make sure to move out of this fast, it will do lots of damage.
|confirm |only if not havequest(28781)
step
goto 36.5,43.2
.' _General Husam_
.' He will use _Shockwave_. When he is beginning to cast, get out of the smoke on the ground. It will deal damage in each direction.
.' He will toss _Mystic Traps_ all around him. Do not get near these, or they will detonate, causing 35k damage.
.' When he uses Mystic Trap, he will cast _Detonate Traps_ soon after, causing them to glow followed by an explosion. Move away from them before the blow up. |only if heroic_dung()
.from General Husam##44577 |only if not havequest(28781)
.kill General Husam##44577 |q 28781/1 |only if havequest(28781)
|confirm |only if not havequest(28781)
step
goto 30.1,34.6
.kill 1 Oathsworn Captain##45122 |q 28781/2
|only if havequest(28781)
step
map  Lost City of the Tol'vir
path loop off
path	42.7,41.4	50.9,43.6	51.0,57.3
path	58.6,57.3	66.4,70.5
.' Follow this path and kill the boss
.' Lockmaw will use _Dust Flail_, which will affect anyone standing behind him.
.' Spread out so you don't hurt your group with _Viscous Poison_. It will damage anyone within 5 yards of you, as well as slow them by 30%
.' He will also cast _Scent of Blood_ on a random party member, causing 4 adds to attack them. Kill them fast and have your tank taunt them whenever possible.
.' At _30% Health_ he will gain the ability _Venomous Rage_, which can be dispelled.
.from Lockmaw##43614
|confirm
|only if not heroic_dung()
step
map  Lost City of the Tol'vir
path loop off
path	42.7,41.4	50.9,43.6	51.0,57.3
path	58.6,57.3	66.4,70.5
.' Follow this path and kill the boss.
.' _Lockmaw & Augh_
.' _Lockmaw_ will use _Dust Flail_, which will affect anyone standing behind him.
.' Spread out so you don't hurt your group with _Viscous Poison_. It will damage anyone within 5 yards of you, as well as slow them by 30%
.' He will also cast _Scent of Blood_ on a random party member, causing 4 adds to attack them. Kill them fast and have your tank taunt them whenever possible.
.' At _30% Health_ he will gain the ability _Venomous Rage_, which can be dispelled.
.from Lockmaw##43614
.' _Augh_ will appear after you defeat Lockmaw. Avoid his _Whirlwind_ by kiting him around.
.' He will toss a poison on random party members called _Paralytic Blow Dart_, dispel it if you are able.
.' He will cast _Dragon's Breath_ on your tank, disorienting him. At this time he will go after party members with high threat.
.' He will go into a frenzy which cannot be removed.
.from Augh##49045
|confirm
|only if heroic_dung()
step
map  Lost City of the Tol'vir
path loop off
path	66.0,69.6	59.1,58.3	49.7,57.1
path	47.1,68.1	49.0,76.4	42.0,81.6
path	39.7,71.2	36.7,61.1	27.1,65.5
.' Follow this path and kill the boss
.' _High Prophet Barim_
.' _Phase 1:_
.' He will use _Fifty Lashings_ to increase his damage output. You may want to use cooldowns.
.' He will cast _Plague of Ages_, this will deal nature damage 4k damage every second for 9 second and can be dispelled.
.' _Heaven's Fury_. This will open up a portal of light that will deal large damage to anyone standing in the light.
.' At 50% he will use _Repentance_, pulling the entire group to him. It will also deal out damage and know the players back at the end of it's duration.
.' He will summosn a _Blaze of the Heavens_ which flies around and attacks. At 1% it transforms into an egg and begins to heal. After 20 second it will hatch again and attack. |only if heroic_dung()
.' _Phase 2:_
.' He will place _Hallowed Ground_ on the ground that deals massive damage. Move out of it as fast as possible.
.' He will summon a _Harbinger of Darkness_ and start using _Soul Sever_. Soul Sever spawns a _Soul Fragment_ which needs to be killed quickly and can be slowed, stunned and snared.
.' Players will randomly be hit for around _7,000_ damage during this phase. Healers need to try and keep people topped off.
.' When the _Harbinger of Darkness_ is dead, it will go back to phase one.
.from High Prophet Barim##43612
|confirm
step
goto 36.0,62.8
.clicknpc Wind Tunnel##48092
.' Ride the Wind Tunnel and face the boss |invehicle
step
goto 35.7,53.9 |n
.' Ride the Wind Tunnel |goto 35.7,53.9,0.5 |noway |c
.' Land on the edge of the building |outvehicle
step
goto 35.7,53.9 |n
.' Ride the Wind Tunnel |goto 35.7,53.9,0.5 |noway |c
.' Land on the edge of the building |outvehicle
step
goto 40.8,55.3
.' There are 2 phases to this fight with Siamat.
.' _Phase 1:_
.' He will spawn _Minions of Siamat_. Interrupt their spells and kill them as fast as you can. Kill them by the ledges, as they spawn whirlwinds when they die.
.' He will summon _Servants of Siamat_, which need to be Tanked and Killed. Only 3 will spawn during the fight. After the 3rd one, Siamat will become vulnerable. Move away from them as they are about to die to avoid AoE damage.
.' During the fight, Siamat will cast _Storm Bolt_.
.' _Phase 2:_
.' He will use _Wailing Winds_ when the shield goes down. Move away from the edges and top every one off health wise if you are the healer.
.' Minions of Siamat will continue to spawn, but you can ignore them and focus on the boss.
.' He will use _Absorb Storms_ removing a minion and granting him a passive AoE.
.' he will use _Gathered Storms_, which absorbs a minion that he has summoned, blasting 2 players for nature damage. |only if not heroic_dung()
.' he will use _Gathered Storms_, which absorbs a minion that he has summoned, blasting all players for nature damage. |only if heroic_dung()
.from Siamat##44819 |only if not havequest(28783)
.kill Siamat##44819 |q 28783/1 |only if havequest(28783)
|confirm |only if not havequest(28783)
|modelnpc Minion of Siamat##44704+
|modelnpc Servant of Siamat##45259+
step
goto 40.8,55.3
.' There are 2 phases to this fight with Siamat.
.from Siamat##44819 |only if not havequest(28783)
.kill Siamat##44819 |q 28783/1 |only if havequest(28783)
|confirm |only if not havequest(28783)
|modelnpc Minion of Siamat##44704+
|modelnpc Servant of Siamat##45259+
step
goto Lost City of the Tol'vir 31.7,16.3
.talk 50038
..turnin 28783
..turnin 28781
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Halls of Origination (HoO) 83-85",[[
dungeon 759
description This guide will walk you through the Halls of Origination dungeon.
achieveid 4841
author support@zygorguides.com
step
label	"start"
goto Halls of Origination 58.3,62.7
.from Temple Guardian Anhuur##39425
.' You will have 2 phases when fighting this boss
.' _Phase 1:_
.' Fight this boss until he becomes _immune to damage_. This happens at _66% and 33%_.
.' He will cast _Burning Light_, which will cause a beam of light to shoot from the wall. Don't stand in the fire it creates on the ground.
.' He will cast _Divine Reckoning_ at a random party member. After 8 seconds, the spell will erupt and damage you along with anyone within 7 yards of you. It is a _Magic Debuff_ and can be dispelled.
.' _Phase 2:_
.' When he casts _Shield of Light_ he becomes immune to damage.
.. You will have to jump off the sides of the platform to the right or left, then _click the switch to deactivate_  shield of light and run back. |only if not heroic_dung()
.. You will have to jump off the sides of the platform to the right and left, then _click the switch to deactivate_  shield of light and run back. |only if heroic_dung()
.' Once he has shield of light, he will start channeling _Reverberating Hymn_ which deals 400 holy damage per stack. It stacks every 4 seconds and lasts until you click the left and right switches.
.' Immediately moving into phase 2, _Pit Vipers_ will spawn on the ground near the levers. The tank should take care to pick them up so that a DPS can click the lever.
|confirm
step
goto 71.3,49.8 |n
.' Follow the Hallway. |goto 71.3,49.8 <5 |noway |c
step
goto 90.1,49.7 |n
.' Follow the Hallway and go up these stairs. |goto Halls of Origination/2 |noway |c
step
goto Halls of Origination/2 32.7,48.7
.' Kill all the mobs in this area, then click a Camel to continue. |invehicle |c
step
goto 49.8,49.3
.from Earthrager Ptah##39428
.' You will have 2 phases when fighting this boss
.' _Phase 1:_
.' Look out for _Earth Spike_. The ground will rumble, then erupt a spike, move when you see dust.
.' He will use _Flame Bolt_, which is channeled and damages 2 random players. |only if not heroic_dung()
.' He will use _Flame Bolt_, which is channeled and damages 3 random players. |only if heroic_dung()
.' He will use _Raging Smash_ which will damage the tank and 2 additional targets within melee range.
.' _Phase 2:_
.' At _50%_ Ptah will burrow and you will have to fight some mobs.
.' He will summon a _Sand Vortex_ that moves around the battlefield.
.' He will summon _Dustbone Horrors_ and _Jeweled Scarabs_, which need to be killed before the boss returns.
.' Be aware of _quicksand_, this will slow your movement and cause damage while standing in it.
|confirm
step
goto 66.7,48.5
.click Halls of Origination Transit Device##8632
.' Teleport to The Maker's Rise. |goto Halls of Origination 67.5,52.8 <5 |noway |c
step
goto 63.6,49.8 |n
.' Follow these stairs up and to the right. |goto 63.6,49.8 <5 |noway |c
step
'Kill each Elemental Warden on the platforms around this room
.' [49.0,32.6]
.' [63.3,32.6]
.' [63.3,22.1]
.' [49.2,22.0]
|confirm
step
goto 56.2,19.3
.' _Anraphet_
.' This boss is more of a Healing intensive fight for the whole group.
.' Anraphet will cast _Alpha Beams_, this cause damage to a player and all players around them.
.' It will leave circle on the ground you need to avoid for 3 seconds. |only if not heroic_dung()
.' It will leave circle on the ground you need to avoid and lingers throughout the fight. |only if heroic_dung()
.' Anraphet will go into _Omega Stance_ causing damage to the whole group for 8 seconds.
.' After Omega Stance or Alpha Beams, he will use _Crumbling Ruin_, decreasing the groups health by 8%. It stacks 10 times.
.from Anraphet##39788
|confirm
step
goto 56.2,27.2 |n
.click Halls of Origination Transit Device##8632
.' Teleport to The Maker's Rise. |goto 67.5,52.8 <5 |noway |c
step
goto 66.4,48.3
.click The Maker's Lift Controller##8504 |n
.' <Bring Elevator to the 2nd floor> |goto Halls of Origination/3 45.9,47.6 |noway |c
step
goto 47.1,78.8
.' _Ammunae_
.' He will cast _Wither_ on a random party member. It reduces attack speed, casting speed and movement speed by 60%. It can be interrupted and dispelled (magic).
.' He will cast _Consume Life Energy_, which will deal nature damage and dimish a players 'resource'.
.' He will summon _Seedling Pods_. They will use _Energize_ on Ammunae, increasing his damage done by 5% and attack speed by 30% while alive. Kill them when they spawn.
.' It will also heal Ammunae for 1% of his max health every 3 seconds while alive. |only if heroic_dung()
.' He will use _Rampant Growth_, transforming all Seedling Pods into Bloodpetal Sprouts which will inflict massive damage to the party.
.' He will spawn a _Spore_. It moves slowly and when it dies, it leaves a spore cloud with a 6 yard radius that deals nature damage every 1.5 seconds.
.' The _Noxious Spore_ cloud that spawn can also dish out major damage to any plant life, inflicting 12 to 18% of it's health.
.from Ammunae##39731
|confirm
step
goto 67.1,49.5
.' _Setesh_
.' He will cast _Chaos Bolt_ at a random party member.
.' He will also place _Seed of Chaos_ on a random party member. When it detonates, the group will take shadow damage.
.' Through the fight, he will _Summon Chaos Portals_. It will spawn monsters that the DPS need to kill, then despawn after around 25 seconds. |only if not heroic_dung()
.' Through the fight, he will _Summon Chaos Portals_. It will spawn monsters while it is up. The DPS will need to attack the portal to stop it from spawning adds. |only if heroic_dung()
.from Setesh##39732
|confirm
step
goto 26.7,49.8
.' _Isiset_
.' She will cast _Supernova_ through out the fight. You can turn away from her to avoid the disorienting effect right before she finishes her cast.
.' When she reaches 66 and 33% health, she will use her _Mirror Images_ ability.
.' At 66% she will split into 3 forms. Kill the _Veil of Sky_ Mirror Image.
.' At 33% she will split into 2 forms. Kill the _Celestial Call_ Mirror Image.
|modelnpc Veil of Sky##39722, Celestial Call##39721
.from Isiset##39587
|confirm
step
goto 47.2,19.7
.' _Rajh_
.' Rajh will start with 100 energy and cast spells at 20 energy each.
.' Rajh's spells to avoid are: _Summon Sun Orb_. This is channeled above a players head, after 3 seconds it will drop and deal damage, knocking players nearby back. It can be interrupted.
.' He will cast _Inferno Leap_ and it needs to be _interrupted_. If not, he will jump towards a targeted player, and deal large damage knocking away any nearby players. |tip This can kill the player he lands on if they do not move.
.' He will also cast _Solar Winds_ which creates a fiery vortex in front of him that grows an dmoves around the room.
.' The Solar Wind will leave _Solar Fire_ on the ground, which deal out massive damage. |only if heroic_dung
.' When he reaches 10 or less energy, he walks to the middle of the room and casts _Blessing of the Sun_.
.' He will become 100% more vulnerable to player attack, while regenerating his energy and causing AoE fire damage to your group.
.from Rajh##39378
|confirm
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Grim Batol (GB) 84-85",[[
dungeon 757
description This guide will walk you through the Grim Batol dungeon.
achieveid 4840
step
goto Grim Batol,18.4,55.2
.talk 50385
..accept 28853
step
goto 19,55
.talk 50390
..accept 28852
step
goto 19.4,53
.talk 50387
..accept 28854
step
map Grim Batol
path follow strict;loop off;ants straight
path	18.0,43.4	20.2,39.6	26.2,39.8
path	27.9,27.1	31.8,26.2
.' Follow the path, clearing the trash as you go.
.' You will need to kill the nets that are holding the dragons down.
|confirm
step
.clicknpc Battered Red Drake##42571
.' You will end up flying through the instance, gaining the ability to attack groups of enemies.
.' The damage that you do to them stays even after you land.
..' Kill 15 Twilight's Hammer Minions |q 28852/1 |only if havequest(28852)
..' Kill 25 Trogg Dwellwers |q 28852/2 |only if havequest(28852)
.' Note that if you didn't get all of the Trogg Dwellers, you can fly back and get them at the end of the instance. |only if havequest(28852)
.' Click here once you've landed. |confirm
step
goto 40.4,68.4
.' _General Umbriss_
.' He will randomly use _Blitz_ towards a party member.  You need to move out of his path to avoid damage.
.' He will also use _Ground Siege_ in a random direction. It will also need to be avoided.
.' The tank will be hit with _Bleeding Wound_ which does massive damage every 2 seconds until they are healed to 90% of their health.
.' At 30% he will get a _Frenzy_ affect gaining 50% attack speed and damage.
.' Note that he also summons adds. If they reach him he will also gain the _Frenzy_ effect. |only if not heroic_dung()
.' He will summon adds. Make sure to keep them more than 10 yards away from the boss.  If close to the boss when they die, they will give the boss a 100% damage buff. |only if heroic_dung()
.' If the adds get near the boss, they will be buffed and deal significant more damage. |only if heroic_dung()
.' He will cast _Modgud's Malady_ which needs to be dispelled. It is a _Magic_ debuff and stacks up to 10 times. |only if heroic_dung()
.from General Umbriss##39625
|confirm
step
goto 46.3,48.5
.' Wait for a patrol to come here.
.' Watch for _Forgemaster Throngus_ who patrols through the room. Go back into the hallway if you haven't cleared the trash yet.
.' Once the trash is cleared, click here. |confirm
'|goto 48.7,47.1
'|goto 45.7,43.6
step
.' _Forgemaster Throngus_
.' This boss cycles through weapons, which have different strategies depending on which he's holding:
.' While using a _Mace_ his movement speed will be decreased by 70%, but his damage will be increased by 200%. The tank will need to kite him.
.' He will jump to random party members during Mace stance inflicting damage as a dot for 5 seconds.
.' He will leave a trail of _Burning Flames_ behind him as he walks, avoid it at all cost. |only if heroic_dung()
.' While using _Dual Blades_ he loses 20% damage, but uses the _Thrash_ ability, causing him to attack an additional two times when he swings his weapons.
.' He will also use _Disorienting Roar_ while in Dual Blade stance, reducing casting and attack speed by 50%. This effect can be removed by using an ability or spell.
.' He will also leave _Burning Flames_ on whoever he hits, which can be dispelled. This effect stack with each hit. |only if heroic_dung()
.' While using his _Phalanx_ He takes 99% reduced damage. Move behind him to bypass this mechanic.
.' While in Phalanx stance, there will also be archers constantly firing  at your group.
.' He will use the ability _Flaming Shield_ which deals fire damage to anyone in front of his shield, so be quick to move behind him. |only if heroic_dung()
.from Forgemaster Throngus##40177
|confirm
step
map Grim Batol
path follow loose;loop off
path	56.8,47.9	60.6,53.5	60.4,63.4
path	63.8, 64.6	67.2,68.1	73.0,59.0
path	68.5,54.7	70.7,41.7	69.8,30.2
.' Kill trash and follow the path to Drahga Shadowburner. |goto 69.8,30.2 <5 |noway|c
step
goto 70.3,20.7
.' _Drahga Shadow Burner_
.' Phase 1:
.' He will use _Invocation of Flame_ which will mark the ground at a random location. It will summon a fire elemental that targets a random player, shooting a red beam. _ALL DPS EXCEPT FOR THE TARGET_ need to kill it as soon as possible.
.' His basic attack is _Burning Shadowbolt_, which can be interrupted.
.' Phaste 2:
.' At 25% he will retreat and the dragon _Valiona_ will catch him. Tank her at the center of the room.
.' _Invocation of Flame_ will still be used, so be aware of the spawn point and target.
.' She will use _Devouring Flames_ which is a wide cone attack, so don't stand in front of her. |only if heroic_dung()
.' She will use _Valiona's Flame_ which targets a random player and breathes a 40 yard long cone which stuns and damages all players within the cone.
.' She swipes, so don't stand in front of her.
.' She places _Seeping Twilight_ on the ground, a large circular pool on the ground that deals out damage over time.
.' When her health is low she will drop Drahga Shadowburner.
.from Drahga Shadowburner##40319
.get Missive to Cho'gall |q 28853/1 |only if havequest(28853)
|confirm |only if not havequest(28853)
step
map Grim Batol
path follow strict;loop off
path	77.0,40.0	85.8,40.0	86.1,57.4
.' Follow the path to _Erudax, The Duke of Below_. |goto 86.1,57.4 <5 |noway|c
step
.' _Erudax, The Duke of Below_
.' The main mechanic for this fight is _Shadow Gale_, in which he will create a purple void zone circle on the ground. The entire group needs to move into it as fast as they can so that they aren't killed by the AoE that effects players outside of the circle. He will take double damage during the duration of the spell.
.' Each time he uses Shadow Gale, he will summon a _Faceless Corruptor_. Use every ability you have to slow them down and kill them. _ONLY SLOWING EFFECTS_ will work on them. If you don't kill them they will eventually heal the boss.
.' He will use _Enfeebling Blow_ which will knock the tank back, giving him a debuff called _Feeble Body_, increasing the damage the tank will take by 100% for 3 seconds.
.' He will use _Binding Shadows_ which is an AoE root that you can step out of before it hits the ground.
.' If you don't kill the adds fast enough, they will be buffed with _Shield of Nightmares_, which will damage your group each time the add is hit. |only if heroic_dung()
.from Erudax##40484 |only if not havequest(28854)
.kill Erudax##40484 |q 28854/1 |only if havequest(28854)
'|modelnpc 39392
'|goto 85.7,60.9
|only if not heroic_dung()
step
.' _Erudax, The Duke of Below_
.' The main mechanic for this fight is _Shadow Gale_, in which he will create a purple void zone circle on the ground. The entire group needs to move into it as fast as they can so that they aren't killed by the AoE that effects players outside of the circle. He will take double damage during the duration of the spell.
.' Each time he uses Shadow Gale, he will summon 2 _Faceless Corruptor_. Use every ability you have to slow them down and kill them. _ONLY SLOWING EFFECTS_ will work on them. If you don't kill them they will eventually heal the boss.
.' He will use _Enfeebling Blow_ which will knock the tank back, giving him a debuff called _Feeble Body_, increasing the damage the tank will take by 100% for 3 seconds.
.' He will use _Binding Shadows_ which is an AoE root that you can step out of before it hits the ground.
.' If you don't kill the adds fast enough, they will be buffed with _Shield of Nightmares_, which will damage your group each time the add is hit.
.from Erudax##40484 |only if not havequest(28854)
.kill Erudax |q 28854/1 |only if havequest(28854)
'|modelnpc 39392
'|goto 85.7,60.9
|only if heroic_dung()
step
.' You can either run to the beginning of the dungeon to turn in your quests.
.' Or
.' You can Right Click the dungeon finder icon on your mini map, teleport out of the dungeon, then teleport back in to the entrance.
|confirm
step
goto Grim Batol,18.4,55.2
.talk 50385
..turnin 28853
step
goto 19,55
.talk 50390
..turnin 28852
step
goto 19.4,53
.talk 50387
..turnin 28854
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Zul Gurub (ZG) 85",[[
dungeon 793
description This guide will walk you through the Zul Gurub dungeon.
achieveid 691
author support@zygorguides.com
step
label "start"
goto Zul'Gurub 30.7,47.5
.talk 53151
..accept 29252
step
goto 30.6,47.3
.talk 53024
..accept 29241
step
goto 30.5,51.0
.talk 53043
..accept 29242
step
goto Zul'Gurub/0 30.8,47.1
.talk 53023
..accept 29172 |or
..accept 29175 |or
..accept 29173 |or
.' T'ara will only offer 1 or 2 of these quests at a time.
step
goto 33.4,46.7
.from Venomancer Mauri##52380
.clicknpc Zanzil's Cauldron of Toxic Torment##52529
.' Get the Toxic Torment Buff. |havebuff Interface\Icons\INV_Misc_Cauldron_Nature
step
goto 37.2,46.4
.' Fight your way through this path.
.from Venomtip Needler##52379+
.' If you need another nature buff, you can get one here.
|confirm
step
goto 43.9,51.0
.from Tiki Lord Mu'Loa##52340+
|confirm
step
goto 48.5,52.1
.' Fight your way to the next cauldron.
.clicknpc Zanzil's Cauldron of Toxic Torment##52529
.' Get the Toxic Torment Buff. |havebuff Interface\Icons\INV_Misc_Cauldron_Nature
step
.' Pull the Venomguard Destroyers near the cauldron and kill them.
.from Venomguard Destroyer##52311+
|confirm
step
goto 50.8,55.1
.' _High Priest Venoxis_
.' _Phase 1:_
.' He will cast, _Toxic Link_ which attaches 2 party members. Run 25 yards away from each other to break this.
.' _Whispers of Hethiss_ is a beam of poison that will damage a player while channeled. This needs to be _interrupted_.
.' During the fight he will use _Venomous Effusion_ which creates a maze of poison on the ground. Avoid the wall at all cost.
.' _Phase 2:_
Venoxis will _Transform_ into a snake god. A _poisonous cloud maze_ will appear on the ground, avoid walking in this.
.' He will continue to use _Venomous Effusion_.
.' His damage will be increased by 50% while in snake form.
.' He will cast a _Pool of Acrid Tears_ on the player furthest away from him which deals nature damage. Avoid if you can.
.' He will use a cone attack, _Breath of Hethiss_, so melee should attack from the back if possible.
.' _Phase 3:_
.' Players will be _chased by Poison Tendrils_, avoid these for 12 seconds. After this phase, Venoxis will take 100% more damage for 10 seconds. If  you do not kill him, phase 1 will repeat.
.from High Priest Venoxis##52155
|confirm
step
.' Follow this path |goto 49.4,54.3 <5 |c
step
.' Follow this path |goto 49.0,58.1 <5 |c
step
.' Follow this path |goto 52.9,57.8 <5 |c
step
.' Once you get to this opening, avoid the rocks to make it up the hill. |goto 56.2,63.0 <5 |c
step
goto 56.9,72.3
.from Berserking Boulder Roller##52348
|confirm
step
goto 60.1,80.2
.' _Bloodlord Mandokir_
.' He will randomly use _Decapitate_ which will kill whoever he uses it on.
.' When the player is killed, a _Chained Spirit_ will revive you, giving you _Spirit's Vengeance_, granting 25% Health, Damage, Healing and decreasing damage taken by 10%. This effect stacks.
.' He will gain a _Level Up_ buff when he kills a player, increasing damage dealt by 20%, a stacking effect.
.' Mandokir will summon his raptor _Ohgan_. _Kill the Raptor_ everytime it spawns or it will kill a Chained Spirit.
.' There are only _8 spirits_, so kill Ohgan as fast as possible.
.' He will use _Devastating Slam_ often during the fight, a 45 degree cone attack that will likely kill you. It marks the ground as to where it strikes, so avoid it.
.' He will toss the _Bloodletting_ debuff on a random target, dealing 50% of their current health as shadow damage.
.from Bloodlord Mandokir##52151
|modelnpc Ohgan##52157
|confirm
step
.' Go back down the hill |goto 56.6,63.3 <5 |c
step
goto 54.6,63.0
.from Kaulema the Mover##52422
.' Slay Kaulema the Mover |q 29173/2
|only if havequest(29173)
step
goto 54.9,55.5
.from Mor'Lek the Dismantler##52405
.' Slay Mor'Lek the Dismantler |q 29173/1
|only if havequest(29173)
step
goto 54.5,52.7
.from Witch Doctor Qu'in##52322
.from Chosen of Hethiss##52323+
|confirm
step
goto 59.5,46.1
.from Ritual Tiki Mask##52364+
|confirm
step
goto 59.3,46.4
.' If a party member has an Archaeology of 225 at least and wishes to fight the boss, click here. |confirm |next
.' If the party wishes to skip the Archaeology boss, click here. |next "end" |confirm
step
.' The Edge of Madness event will summon 1 of 4 random bosses.
.' If Hazza'rah spawns click here. |next "hazza" |confirm
.' If Renataki spawns click here. |next "rena" |confirm
.' If Wushoolay spawns click here. |next "wush" |confirm
.' If Gri'lek spawns click here. |next "gri" |confirm
step
label hazza
'_Hazza'rah_
.' Hazza'rah will use _Nightmare_ and summon 4 adds. Each add will target a random party member and cast _Waking Nightmare_ stunning them. The player who is not stunned must kill the adds before they reach the targetted player.
.from Hazza'rah##52271
.get Mysterious Gurubashi Bijou##69647 |q 29242/1 |only if havequest(29242)
|confirm |only if not havequest(29242) |next "end"
step
label rena
'_Renataki_
.' _Renataki_ will use _Deadly Poison_ throughout the fight. This effect stacks and can be cleansed.
.' He will periodically _Vanish_. When he does this make sure everybody in the party is at 100% health. He will _Ambush_ a random target that deals 90% of the players health.
.' He will also cast _Thousand Blades_. This whirlwind is dangerous and needs to be avoided.
.from Renataki##52269
.get Mysterious Gurubashi Bijou##69647 |q 29242/1 |only if havequest(29242)
|confirm |only if not havequest(29242) |next "end"
step
label wush
'_Wushoolay_
.' Wushoolay will summon _Lightning Clouds_ periodically that should be avoided.
.' He will use _Forked Lightning_, so spread out.
.' Wushoolay will transform into a ball of lightning and charge at a random player. After he reaches them he begins to cast _Lightning Rod_. Everybody needs to run away from Wushoolay at this point, Lightning Rod does deadly amounts of damage and needs to be avoided
.from Wushoolay##52286
.get Mysterious Gurubashi Bijou##69647 |q 29242/1 |only if havequest(29242)
|confirm |only if not havequest(29242) |next "end"
step
label gri
'_Gri'lek_
.' Gri'lek is a kiting encounter.
.' He will fixate on a random party member and gain the _Avatar_ buff, increasing his damage output by 500% and reducing his movement speed. If he fixates on you just run away until it is done.
.' He will cast _Entangling Roots_ on the player he is chasing, this can and needs to be dispelled.
.' He constantly summons _Rupture Lines_. Avoid the rumbling dirt.
.from Gri'lek##52258
.get Mysterious Gurubashi Bijou##69647 |q 29242/1 |only if havequest(29242)
|confirm |only if not havequest(29242) |next "end"
step
label "end"
.' Follow this path. |goto 58.0,50.9 <5 |c
step
.' Follow this path. |goto 56.5,50.5 <5 |c
step
.' Follow this path. |goto 56.6,43.3 <5 |c
step
.' Follow this path. |goto 58.5,34.9 <5 |c
step
goto 62.4,33.8
.from Mortaxx##52438
.' Kill Mortaxx |q 29173/3
|only if havequest(29173)
step
.' Enter the building here |goto 57.9,29.8 <5 |c
|only if havequest(29175)
step
goto 57.7,27.3
.from Gurubashi Master Chef##52392
.' Destroy the Gurubashi Master Chef |q 29175/2
|only if havequest(29172)
step
.' Follow this path. |goto 55.6,27.7 <5 |c
step
.' Follow this path down towards the water. |goto 53.6,29.2 <5 |c
|only if havequest(29172)
step
goto 55.1,33.5
.from Lost Offspring of Gahz'ranka##52418
.' Kill a Lost Offspring of Gahz'ranka |q 29172/1
|only if havequest(29172)
step
.' Follow this path. |goto 47.9,27.6 <5 |c
step
goto 47.8,20.9
.' _High Priestess Kilnara_
.' _Phase 1_
.' The tank should pull the _Pride of Bethekk_ and keep aggro on Kilnara.
.' The DPS should focus on killing the 16 _Pride of Bethekk_, pulling 4 at a time. (You can kill more if you've got great dps)
.' She will cast _Tears of Blood_ on random party members. It deals massive damage and should be dispelled as soon as possible.
.' She will also cast _Lash of Anguish_ on a random target, which should be dispelled.
.' She will cast _Wave of Agony_ at a random player, attacking and knocking back anyone in front of her. Avoid it.
.' _Phase 2_
.' Once she reaches 50% health she will enter phase 2, vanishing. When she reappears, she will be in cat form.
.' She will deal out more damage. DPS her down as fast as possible.
.from High Priestess Kilnara##52059
|modelnpc Pride of Bethekk##52061+
|confirm
step
goto 47.6,23.6 |n
.' Go upstairs and leave the building. |goto 47.6,23.6 <5 |noway |c
step
.' Follow this path |goto 44.9,28.1 <5 |c
step
.' Follow this path |goto 42.2,29.1 <5 |c
step
.' Follow this path |goto 38.1,29.8 <5 |c
|only if havequest(29172)
step
.' Follow this path |goto 37.0,32.1 <5 |c
|only if havequest(29172)
step
.' Follow this path |goto 33.8,36.1 <5 |c
|only if havequest(29172)
step
.' Follow this path |goto 33.0,38.4 <5 |c
|only if havequest(29172)
step
.from Florawing Hive Queen##52442
.' Destroy the Florawing Hive Queen |q 29172/2
|only if havequest(29172)
step
.' Follow this path |goto 38.8,27.1 <5 |c
step
.' Follow this path |goto 34.8,27.1 <5 |c
step
.' Follow this path up to the next boss |goto 34.8,27.1 <5 |c
step
goto 31.4,24.8
.from Zanzil##52053
.' Zanzil will _Drain 3 Cauldrons_ separately during this fight. You will use the buff of each Cauldron for the proper _response to the boss_.
.' Use the Green Cauldron at [30.6,23.9] to get the buff. He will cast _Graveyard Gas_ and put poison all over the ground.
.' Use the Blue Cauldron at [31.6,27.3] to get the _Forstburn Formula_. He will resurrect a Berserker which you will have to kill.
.' Use the Red Cauldron at [33.0,24.3] to get the _Burning Blood_. The Tank will use this to aggro and kill all the Zombies that resurrect.
|confirm
step
.' Follow this path |goto 37.7,27.5 <5 |c
step
.' Follow this path |goto 42.7,30.2 <5 |c
step
.' Follow this path |goto 41.1,37.0 <5 |c
step
.' Follow this path |goto 46.1,39.9 <5 |c
step
.' Follow this path up and around |goto 46.6,36.6 <5 |c
step
goto 48.7,39.9
.' This fight has 2 phases
.' _Phase 1:_ Jin'do will cast an aura _Shadows of Hakkar_, which will do heavy damage to the group. Make sure to _stand in_ the _green bubbles_ while this aura is active.
.' Once his aura, _Shadows of Hakkar_ is over, move _out of_ the bubble.
.' _Phase 2:_ Jin'do will go away and there will be _3 chains_ protected by a barrier on the ground. The _tank_ will need to _grab a Gurubashi Spirit_ and get near the chains.
.' The Gurubashi Spirit will use _Body Slam_ on a player, if the player is near the chains, the _barrier will break_. Kill 3 chains by breaking the barrier and attacking them.
.from Jin'do the Godbreaker##52148 |only if not havequest(29252)
.kill Jin'do the Godbreaker##52148 |q 29241/1 |only if havequest(29241)
.get Zul'Gurub Stone |q 29252/1 |only if havequest(29252)
|confirm |only if not havequest(29252)
step
.' Go to this bridge |goto 42.5,42.4 <5 |c
|only if havequest(29172)
step
goto 43.0,44.6
.from Gub##52440
.' Destroy Gub |q 29175/1
|only if havequest(29175)
step
.' Follow this path |goto 43.6,47.9 <5 |c
|only if havequest(29172)
step
.' Cross the bridge |goto 44.8,51.0 <5 |c
|only if havequest(29172)
step
.' Follow this path |goto 47.8,53.0 <5 |c
|only if havequest(29172)
step
.' Follow the path to the edge of the water |goto 46.7,54.3 <5 |c
|only if havequest(29172)
step
goto 46.1,55.7
.from Tor-Tun##52414
.' Destroy Tor-Tun |q 29172/3
|only if havequest(29172)
step
goto 30.7,47.5
.talk 53151
..turnin 29252
step
goto 30.6,47.3
.talk 53024
..turnin 29241
step
goto 30.8,47.1
.talk 53023
..turnin 29172 |or
..turnin 29175 |or
..turnin 29173 |or
.' T'ara will only offer 1 or 2 of these quests at a time.
step
goto 30.5,50.9
.talk 53043
..turnin 29242
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Cataclysm Dungeons\\Zul'Aman (ZA) 85",[[
dungeon 781
description This guide will walk you through the Zul'Aman dungeon.
achieveid 688
step
label "start"
goto Zul'Aman,13.8,52.2
.talk 52925
..accept 29186
step
goto 14.2,53.2
.talk 52924
..accept 11196
step
goto 14.2,53.2
.talk 52924
.' Tell him you are ready to open the doors.
|confirm
step
goto 34.4,48.7
.' There will be a patrol walking at this point, wait for it to pass before moving to your left.
|confirm
step
map Zul'Aman
path follow strict;loop off;ants straight
path	31.8,43.8	29.7,41.3	33.0,40.0
path	35.4,40.1	37.8,39.4	37.4,34.3
path	36.0,28.5
.' At this point, follow the path up the steps.
.' There will be mobs that come after you from behind as you go up the steps.
.' Kill _Amani'shi Wind Walkers_ first, interrupting _Chain Heal_ whenever they cast it.
.' Follow the path up to Akil'zon's Altar. |goto 36.0,28.5 <5 |noway|c
'|modelnpc 24179
step
goto 35.1,22.9
.' _Akil'zon_
.' For this encounter, you will want to spread out to avoid zapping party members with AoE attacks.
.' _Amani Kidnappers_ will swoop down, picking up a random party member.  You will have to kill the bird quickly.
.' He will cast _Electrical Storm_ on a random party member.  When this happens, all players will need to move underneath the party member who is affected.
.' _Electrical Storm_ deals out massive damage that increases over time, so move quickly.
.' He will also put the _Static Disruption_ debuff on a random target, dealing nature damage and increasing nature damage taken. It can be dispelled.
.from Akil'zon##23574
'|modelnpc 52638
|confirm
step
map Zul'Aman
path follow strict;loop off;ants straight
path	34.3,61.7	31.0,63.4	30.8,66.6
path	33.9,70.4	34.1,75.8	37.5,76.5
.' The patrol will still be there you can avoid it or kill them.
.' Follow the path up, fighting trash as you go along.
.' Kill _Amani'shi Medicine Men_ first, followed by _Amani'shi Casters_.
.' If needed, you can also use crowd control on them when needed.
.' Note that at the last pull, you will not be able to crowd control at first.
|confirm
step
goto 40.6,76.5
.' _Nalorakk_
.' Phase 1(Troll Phase):
.' He will deal massive damage to the tank with _Brutal Strike_.
.' He also uses an ability called _Surge_.  It's important to know that he charges the player furthest from him, leaving a debuff that increases damage taken by _500%!_
.' This means that you need to rotate who gets hit by it. _Ranged DPS and Healers_ should be the ones to do this. However if you don't have enough ranged, have everyone stack close to the boss, and have a melee step out when it's their turn.
.' _He will use Surge 3 times, so 3 people will need to take the hit!_
.' Phase 2(Bear Phase):
.' He will put a massive bleed dot on the tank called _Lacerating Slash_.
.' He will also use _Deafening Roar_, which silences the party for 2 seconds. It will be important to keep the tank topped off on health during phase 2.
.' He will go back and forth between phases.
.from Nalorakk##23576
|confirm
step
map Zul'Aman
path follow strict;loop off;ants straight
path	34.3,76.8	33.9,90.6	37.5,91.4
path	43.3,89.5	48.0,90.0	49.6,86.2
path	52.2,84.2	55.9,84.2	59.8,83.6
path	58.5,77.6	55.4,76.8	55.4,72.6
.' Follow the path.
.' Use crowd control on _Amani'shi Flame Casters_ when applicable.
.' Keep an eye out for _Amani'shi Scouts_, if they do not die instantly they will summon additional adds..
|confirm
'|modelnpc 23586
'|modelnpc 23596
step
goto 55.4,71.0
.' _Jan'alai_
.' For this fight, he will cast _Flame Breath_ towards a random player. Step out of the way when he does so, it deals massive fire damage.
.' He will also _Summon Amani'shi Hatchers_, which hatch _Amani Dragonhawk Hatchlings_. _ONLY KILL ONE_ of the Hatchers, and let the other destroy the eggs.
.' Use every AoE ability you have to kill the hatchlings as they come in waves.
.' He will also periodically use _Fire Bomb_, where he will teleport to the center of the room and throw fire orbs around randomly. _DO NOT STAND ON THE FIRE ORBS_.
.' DPS him down slowly until the Amani Dragonhawk Hatchlings are dead.
.from Jan'alai##23578
|confirm
step
map Zul'Aman
path follow strict;loop off;ants straight
path	55.6,62.9	59.1,56.1	59.1,50.8
path	59.0,43.1	62.3,37.6	64.8,30.0
path	71.3,23.4	76.4,21.7
.' Follow the path. |goto 76.4,21.7,0.5 |noway|c
.' If you have any sort of water walk, you can reach the end waypoint without fighting much.
step
map Zul'Aman
path follow strict;loop off;ants straight
path	75.1,14.9	70.4,14.1	68.3,19.2
path	65.2,23.1
.' There will be packs of _Amani Lynx_ that are stealthed. Let your tank move ahead of the party.
.' At the last waypoint, use any crowd control you have available, and pull the group to the bottom of the stairs.
.' Kill the trolls first.
|confirm
'|modelnpc 24043
step
goto 58.0,23.3
.' _Halazzi_
.' He will drop a _Water Totem_, which heals and restores mana to anyone standing in it's radius. Move Halazzi out of it, and have ranged party members stand in it.
.' At 66% and 33% he will summon a _Spirit of the Lynx_ that has random threat no matter what. Halazzi's health will jump to 100% also. Burn him down.
.' Kill _Corrupted Lightning Totems_ as they are summoned. He only casts this while in troll form.
.' Dispel _Flame Shock_ if possible. He only casts this while in troll form.
.from Halazzi##23577
|confirm
step
map Zul'Aman
path follow strict;loop off;ants straight
path	59.4,33.9	62.4,34.3	59.9,40.1
path	58.8,45.8	58.1,53.9	64.9,53.2
.' Follow the path up to Hex Lord Malacrass.
.' Use crowd control on _Amani'shi Medicine Men_.
.' When you fight _Amani'shi Berserkers_ let your tank get threat before you attack, they cannot be taunted.
|confirm
'|modelnpc 23581
'|modelnpc 24374
step
label "hex"
goto 71.3,53.2
.' _Hex Lord Malacrass_
.' He will have two adds on both his sides. They can be crowd controlled and should be if possible.
.' If your group has no crowd control, then you will need to kill them before attacking the boss, as they do a lot of damage.
.' _Hex Lord Malacrass_ will adopt the abilities from a random party member. Click below to see what he will do.
.from Hex Lord Malacrass##24239+
.get The Hex Lord's Fetish |q 29186/1 |only if havequest(29186)
.' Death Knight: |next "dk" |confirm
.' Druid: |next "druid" |confirm
.' Hunter: |next "hunt" |confirm
.' Mage: |next "mage" |confirm
.' Paladin: |next "pally" |confirm
.' Priest: |next "priest" |confirm
.' Rogue: |next "rogue" |confirm
.' Shaman: |next "shaman" |confirm
.' Warlock: |next "lock" |confirm
.' Warrior: |next "war" |confirm
.' Click here to continue. |next "daa" |confirm
step
label "dk"
.' He will put _Death and Decay_ on the ground. Stay out of it.
.' Kill _Blood Worms_ if you see them.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "druid"
.' He will use _Lifebloom_ which should be dispelled.
.' He will also use _Thorns_ which should be removed from him.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "hunt"
.' He will use _Explosive Trap_.
.' He will use _Freezing Trap_.
.' He will use _Snake Trap_. AoE down the snakes.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "mage"
.' He will root nearby players with _Frost Nova_.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "pally"
.' He will use _Consecration_, which is an AoE Ground Type attack. Move out of it.
.' He will cast _Holy Light_ which needs to be interrupted.
.' He will use _Avenging Wrath_ which can be Purged, Dispelled or you can use Spell Steal.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "priest"
.' He will use _Mind Control_.
.' He Will use _Heal_ which needs to be interrupted.
.' He will use _Psychic Scream_ which will fear you if you are close so stay away if you can.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "rogue"
.' He will use _Blind_.
.' He will use _Would Poison_ on the tank which needs to be dispelled (Poison).
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "shaman"
.' He will use _Fire Nova_ so stay away from him if you can.
.' He will use _Healing Wave_ which needs to be interrupted.
.' He will use _Chain Lightning_ so spread out.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "lock"
.' He will cast _Rain of Fire_, simply move out of its radius.
.' He will cast _Unstable Affliction_, _DO NOT_ dispel it.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "war"
.' He will use _Spell Reflect_, be sure not to cast big spells when he's got it up.
.' He will use _Whirlwind_, so don't be within melee range.
.' He will use _Mortal Strike_ on the tank.
.' Click here to go back to the Hex Lord. |next "hex" |confirm
step
label "daa"
goto 73.8,53.2
.' Click the doors and fight the trolls that come out.
.' After you defeat the trolls, make your way to the steps.
|confirm
step
label "final"
.goto 90.8,52.6
.' _Daakara_
.' This encounter has 3 phases:
.' Phase 1:
.' He will use _Whirlwind_ often. Melee should move away from him when he is casting it to avoid damage.
.' He will also also use _Grievous Throw_ which leaves a bleed on a target. If you heal the afflicted target to full it will go away.
.' Phase 2:
.' He will take on two of the forms of former bosses. If Lynx is first, he will choose Dragonhawk second. If Bear is first then Eagle will be second. Click to get a detailed explaination.
.from Daakara##23863 |only if not havequest(11196)
|confirm |only if not havequest(11196) |next "turnin"
.kill Daakara##23863 |q 11196/1 |only if havequest(11196) |next "turnin"
.' Click here for details on _Shape of the Eagle_.|next "eagle" |confirm
.' Click here for details on _Shape of the Bear_.|next "bear" |confirm
.' Click here for details on _Shape of the Dragonhawk_.|next "dragonhawk" |confirm
.' Click here for details on _Shape of the Lynx_.|next "lynx" |confirm
step
label "eagle"
.' He will use _Energy Storm_ which will hit players whenever they cast a spell for 12,000 nature damage.
.' Kill _Lightning Totems_ as he places them on the battle field.
.' He will cast _Summoned Cyclone_ which needs to be avoided at all cost.
.' Click here to go back to the boss. |next "final" |confirm
step
label "bear"
.'His main ability will be _Creeping Paralysis_, this will stun the entire party after a few seconds but can be dispelled. Mass Dispell is extremely useful but if you do not have one then dispel priority targets (Healer, Tank, Highest DPS).
.' He will also use _Surge_, so you will all need to stack up on the boss, having 3 different ranged move out to take the surge.
.' Click here to go back to the boss. |next "final" |confirm
step
label "dragonhawk"
.' He will cast _Flame Whirl_ which increases damage from fire, this is unavoidable.
.' He will cast _Flame Breath_ at targets in front of him. Do not be in front of him.
.' He casts _Column of Fire_ which deals out a lot of damage, move out of it.
.' Click here to go back to the boss. |next "final" |confirm
step
label "lynx"
.' He will use _Claw Rage_ on a random party member. The tank will need to _Taunt it IMMEDIATELY_.
.' He will also spawn a Lynx during the fight.
.' He will also randomly use _Lynx Rush_ dealling mass damage to the target.
.' Click here to go back to the boss. |next "final" |confirm
step
label "turnin"
goto 91.6,54.8
.talk 52925
..turnin 29186
step
goto 93.0,53.6
.talk 52924
..turnin 11196
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Temple of the Jade Serpent 85-87",[[
dungeon 867
description This guide will walk you through the Siege of Niuzao Temple dungeon.
achieveid 6757
author support@zygorguides.com
step
goto Temple of the Jade Serpent/1 36.0,44.4
.talk 60578
..accept 31355
step
goto 36.0,44.4
.talk 64399
..accept 31356
step
map Temple of the Jade Serpent/1
path follow strict;loop off;ants straight
path	36.3,34.9	31.4,26.6	37.4,15.9
'Follow this path to the first boss. |goto 37.4,15.9 <5 |noway |c
step
'_Wise Mari_
'_Phase 1_
.' Throughout this entire fight avoid standing in any water, it will hurt you every second.
.' For the first phase Wise Mari will be immune to damage. You can break his protective bubble by defeat the four _Corrupt Living Water_ that he summons with _Call Water_.
.' When these adds die they will spawn three _Corrupt Droplet_ and leave behind _Sha Residue_. Kite the adds to a new location out of the residue and kill them.
.' After four adds die, Wise Mari will burst his bubble and begin Phase 2.
'_Phase 2_
.' The only mechanic in this phase is _Wash Away_. Mari will spin in a circle and shoot out a water spout, move around him to avoid being hit.
.from Wise Mari##56448
.' Wise Mari assisted |q 31355/1 |only if havequest(31355)
|confirm |only if not havequest(31355)
step
map Temple of the Jade Serpent/1
path follow strict;loop off;ants straight
path	37.4,15.9	31.4,26.6	36.3,34.9
path	31.4,57.6	27.7,62.8
'Follow the path back to The Scrollkeeper's Sanctum. |goto Temple of the Jade Serpent/2 |noway |c
step
map Temple of the Jade Serpent/2
path follow strict;loop off;ants straight
path	34.9,21.5	26.4,68.0	58.3,82.5
path	65.3,50.2	46.1,41.9
.' Kill all of the mini bosses along this platform as you follow the path. |goto Temple of the Jade Serpent/1 |noway |c
step
'When you are ready to face the boss kill the Corrupted Scroll.
.from Corrupted Scroll##57080
|confirm
step
'There are two possible encounters associated with this boss.
.' Click here for the _Trial of the Yaungol_ strategy. |next "yaungol" |confirm
.' Click here for the _Champion of the Five Suns_ strategy. |next "sun" |confirm
step
label "yaungol"
'_Trial of the Yaungol_
.' _Strife and Peril_ will spawn.
.' Your group will want to focus its attention on either Strife or Peril, splitting up damage is extremely bad in this fight.
.' When you attack one of the bosses they begin stacking a buff called _Intensity_. Keep your eye on this buff, the higher it gets the more damage it does to the group. Let this buff get to either eight or niene stacks and then switch all damage, including pets, to the other boss.
.' The boss that you are not focusing will begin to starve. _Dissipation_ reduces the _Insanity_ stacks.
.' While you are juggling the bosses they will cast _Agony_ on random party members. This does a decent amount of damage that must be healed.
.from Strife##59051
.from Peril##59726
.' Lorewalker Stonestep assisted |q 31355/3 |only if havequest(31355) |next "done"
|confirm |next "done" |only if not havequest(31355)
step
label "sun"
'_Champion of the Five Suns_
.' Five _Suns_ will spawn and being firing _Sunfire Rays_ at random players dealing damage.
.' You will be able to kill four of these Suns, and everytime you do a _Haunting Sha_ will spawn. You can either kill these one at a time or all together.
.' When all four _Haunting Sha_ are dead, _Zao Sunseeker_ will become possessed.
.' He will shoot _Hellfire Arrows_ at random party members, kill him before your healer becomes overwhelmed.
.from Zao Sunseeker##58826
.' Lorewalker Stonestep assisted |q 31355/3 |only if havequest(31355) |next "done"
|confirm |next "done" |only if not havequest(31355)
step
label done
map Temple of the Jade Serpent/1
path follow strict;loop off;ants straight
path	38.6,80.7	53.5,85.4	46.5,64.9
'Follow this path to the next boss. |goto 46.5,64.9 <5 |c
step
'Kill all the enemies on the Terrace of the Twin Dragons to face Liu Flameheart.
|confirm
step
'_Liu Flameheart_ |modelnpc Liu Flameheart##56732
'_Phase 1_
.' _Serpent Strike_ will hit the tank for significant damage and leave a heavy DoT effect on them, dispel this DoT.
.' _Serpent Kick_ will be used immediately after _Serpent Strike_, this does damage to anybody within 8 yards of Liu.
.' _Serpent Wave_ wil follow _Serpent Kick_, this shoots waves of fire out from around Liu. Do not be hit by these.
'_Phase 2_
.' At _70%_ life, all of Liu's abilities gain an additional effect.
.' _Jade Serpent Strike_ now also absorbs 70,000 healing received by the target.
.' _Jade Serpent Kick_ now also knocks players within 8 yards back.
.' _Jade Serpent Wave_ now also leaves a trial of fire along the path the flame travels.
'_Phase 3_
.' At _30%_ life, _Yu'lon_ spawns.
.' Yu'lon will breathe balls of fire that create a pool of Jade Fire where they land. Step out of this pool to avoid taking damage.
.from Yu'lon##56762
.' Liu Flameheart assisted |q 31355/2 |only if havequest(31355)
|confirm |only if not havequest(31355)
step
goto 56.1,57.0
'Perpare to face the Sha of Doubt.
|confirm
step
'_Sha of Doubt_
.' The Sha of Doubt will cast _Wither Will_ at two random party members, dealing damage to both of them.
.' _Touch of Nothingness_ is a dispellable effect that will be put on a random player. This does damage to all surrounding players so spread out.
.' Periodically the Sha of Doubt will become invulnerable and summon as many adds are there are players. Stack these adds up and kill all of them, if you do not kill them before their energy bar is full, they will heal the boss 10% life and explode dealing significant damage for each add left alive.
.from Sha of Doubt##56439 |only if not havequest(31356)
.kill Sha of Doubt##56439 |q 31356/1 |only if havequest(31356)
|confirm |only if not havequest(31356)
step
goto 36.0,44.4
.talk 60578
..turnin 31355
step
goto 36.0,44.4
.talk 64399
..turnin 31356
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Stormstout Brewery 85-87",[[
dungeon 876
description This guide will walk you through the Siege of Niuzao Temple dungeon.
achieveid 6457
author support@zygorguides.com
step
goto Stormstout Brewery/1 76.9,35.4
.talk 59822
..accept 31324
step
goto 75.1,36.2
.talk 59704
..accept 31327
step
'As you follow the next path make sure you loot the Hozen you kill for Stormstout Secrets and click the tomes along the floor.
.from Sodden Hozen Brawler##59605+, Inflamed Hozen Brawler##56924+, Sleepy Hozen Brawler##56863+, Drunken Hozen Brawler##56862+
.click Stormstout Secrets
.get 5 Stormstout Secrets |q 31324/1
|confirm
|only if havequest(31324)
step
map Stormstout Brewery/1
path follow strict;loop off;ants straight
path	54.0,50.3	44.0,65.1	30.7,84.3
path	21.5,76.5	23.9,63.1
.' Clear the trash along this path. |goto 23.9,63.1 <5 |noway |c
step
goto Stormstout Brewery/2 53.4,50.1
'You will need to kill 40 Hozen to summon the first boss.
.' Click here once Ook-Ook has spawned |confirm
step
'_Ook-Ook_
.' Ook-Ook will _Ground Pound_ which is a frontal shockwave, do not stand in it.
.' At 90%, 60%, and 30% Ook-Ook will _Go Bananas_. This increases his attack speed and damage dealt each time. This buff stacks. He will also summon Rolling Barrels when he does this.
.' DPS should click on Rolling Barrels and ride them into Ook-Ook. This will increase his damage taken as well as do a lot of damage. This effect stacks
.from Ook-Ook##56637
|confirm
step
map Stormstout Brewery/3
path follow strict;loop off;ants straight
path	32.2,88.9	37.2,93.5	43.2,85.8
path	35.9,77.0	20.5,68.7	29.5,49.9
path	38.0,46.3
'This is a gauntlet event. Kill the virmen as you run through the path.
.' Boppers are the most important type of virmen. Upon death they drop a _Big Ol' Hammer_. When you pick this up you will gain a new button on your screen, _Smash!_. Click it to clear many virmen at a time.
.' Finish the gauntlet event. |goto 38.0,46.3 <5 |noway |c
|modeldisplay Bopper##59426+
step
'_Hoptallus_
.' Hoptallus will constantly use _Furlwind_. this whirlwind will do large amounts of damage, so kite him around.
.' He will also summon virmen. These are the same types of adds that you cleared in the last path. Aim down the Boppers first and use their _Big Ol' Hammer_ to _Smash!_ all of the other adds.
.' _Carrot Breath_ will deal large damage to anybody standing within the cone, avoid this.
.from Hoptallus##56717
|confirm
step
map Stormstout Brewery/3
path follow strict;loop off;ants straight
path	39.7,38.9	43.3,26.7	58.9,29.7
path	75.3,56.5	89.9,44.2	73.6,34.2
'Kill the Alementals as you follow this path. |goto 73.6,34.2 <5 |noway |c
step
goto Stormstout Brewery/4 49.6,51.1
'Uncle Gao will summon 3 waves of Alementals before Yan-Zhu.
.from Yeasty Brew Alemental##66413+
.from Bloated Brew Alemental##59518+
.from Fizzy Brew Alemental##59520+
|confirm
step
'_Yan-Zhu the Uncasked_
.' Yan-Zhu will constantly through _Blackout Brew_, counter this by moving around until the debuff is off of you.
.' When Yan-Zhu casts _Carbonation_ click on the _Fizzy Bubbles_ around the room, this will grant you a buff that allows you to fly above the cloud.
.' _Bubble Shields_ will need to be killed very quickly, they reduce the amount of damage that the boss takes.
.kill Yan-Zhu the Uncasked##59479 |q 31327/1 |only if havequest(31327)
.from Yan-Zhu the Uncasked##59479 |only if not havequest(31327)
|confirm |only if not havequest(31327)
|tip Keep moving to remove the Blackout Brew, click Fizzy Bubbles to gain the ability to fly above Carbonation
step
goto Stormstout Brewery/4 39.0,56.0
.talk 59074
..turnin 31324
step
goto 41.1,55.2
.talk 64361
..turnin 31327
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Mogu'shan Palace (MSP) 87-90",[[
dungeon 885
description This guide will walk you through the Mogu'shan Palace dungeon.
achieveid 6755
author support@zygorguides.com
step
goto Mogu'shan Palace/1 30.3,19.2
.talk 64432
..accept 31357
..accept 31360
step
map Mogu'shan Palace/1
path follow strict;loop off;ants straight
path	40.2,26.1	40.3,57.6
'Clear the trash and follow this path to the boss. |goto 40.3,57.6 <5 |noway |c
step
'_Trial of the King_
.' The Trial of the King consists of three minibosses.
.' Click here for Kuai the Brute's strategy. |next "kuai" |confirm
.' Click here for Ming the Cunning's strategy. |next "ming" |confirm
.' Click here for Haiyan the Unstoppable's strategy. |next "haiyan" |confirm
step
label kuai
'_Kuai the Brute_
.' Kuai's pet _Mu'Shiba_ is the main threat of this fight, kill it first. _Mu'Shiba_ will use _Ravage_, stunning and damage over 10 seconds.
.' Kuai will cast _Shockwave_ knocking players into the air. They will take some falling damage so make sure everybody is healed up.
.from Kuai the Brute##61442
.' Click here for Haiyan the Unstoppable's strategy. |next "haiyan" |confirm
.' Click here for Ming the Cunning's strategy. |next "ming" |confirm
.' Click here to move on with the dungeon. |next "end" |confirm
step
label haiyan
'_Haiyan the Unstoppable_
.' _Conflagrate_ will dissorient and deal fire damage to whomever it is cast on. Make sure the group steps away from that person to avoid the fire from spreading.
.' Haiyan will cast _Meteor_ on a random player and put a red arrow over their head. Stack on that person to disperse the damage that comes with Meteor.
.' He will also use _Traumatic Blow_ on the tank, lowering his healing received.
.from Haiyan the Unstoppable##61445
.' Click here for Kuai the Brute's strategy. |next "kuai" |confirm
.' Click here for Ming the Cunning's strategy. |next "ming" |confirm
.' Click here to move on with the dungeon. |next "end" |confirm
step
label ming
'_Ming the Cunning_
.' Ming summons a _Whirling Dervish_ that will knock any player it hits back.
.' He will also use _Magnetic Field_. When he casts this run away from him until it is done, it will pull you back into it so keep running.
.from Ming the Cunning##61444
.' Click here for Haiyan the Unstoppable's strategy. |next "haiyan" |confirm
.' Click here for Kuai the Brute's strategy. |next "kuai" |confirm
.' Click here to move on with the dungeon. |next "end" |confirm
step
label end
'Loot the chest and follow these stairs down. |goto 48.8,74.9 <5 |c
step
'Make your way here. |goto Mogu'shan Palace/2 28.6,32.6 <5 |c
step
'Clear trash to this point. |goto 30.9,75.7 <5 |c
step
goto 37.4,64.5
.click Ancient Mogu Chest
.get Lantern of the Sorcerer King |q 31357/1
|only if havequest(31357)
step
goto 43.0,60.1
.click Ancient Mogu Chest
.get Mad King Meng's Balance |q 31357/2
|only if havequest(31357)
step
goto 60.2,82.6
.click Ancient Mogu Chest
.get Subetai's Bow of the Swift |q 31357/4
|only if havequest(31357)
step
goto 38.7,88.7
.click Ancient Mogu Chest
.get Qiang's "The Science of War" |q 31357/3
|only if havequest(31357)
step
goto 44.9,75.8
'_Gekkan_
.' Gekkan has 4 adds with him. The Protector will shield all of the other adds so he must die first. The Oracle is the healer so he needs to be interrupted and killed second.
.' Crowd Control is suggested in this fight but don't count on it. Gekkan will use _Reckless Inspiration_ on a random add. This makes them immune to crowd control and increases their attack speed for a period of time.
.from Gekkan##61243
|confirm
step
goto 71.5,76.1
'Ride the elevator up |goto Mogu'shan Palace/3 |noway |c
step
map Mogu'shan Palace/3
path follow strict;loop off;ants straight
path	45.0,24.3	40.4,36.1	40.3,75.2
'Clear the trash and make your way to the final boss. |goto 40.3,75.2 <5 |c
step
'_Xin the Weaponmaster_
.' Xin wil use _Ground Slam_ on the tank's position. Avoid this.
.' He will periodically animate a staff. This staff uses _Circle of Flame_ so make sure you are not in the circle when the staff finishes.
.' _Whirlwinding Axes_ will be summoned almost immediately. Avoid standing in their spin.
.' _Blade Trap_ will activate at 66% health. This is a linear stream of swords. Avoid them.
.' At 33% Xin will activate _Death from Above!_. This will cause crossbows to fire from his walls, this damage is unavoidable.
.from Xin the Weaponmaster##61398 |only if not havequest(31360)
.kill Xin the Weaponmaster##61398 |q 31360/1 |only if havequest(31360)
|confirm |only if not havequest(31360)
step
goto 38.7,81.5
.talk 64432
..turnin 31357
..turnin 31360
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Shado-Pan Monastery 87-90",[[
dungeon 877
description This guide will walk you through the Siege of Niuzao Temple dungeon.
achieveid 6469
author support@zygorguides.com
step
goto Shado-Pan Monastery/1 83.4,56.1
.talk 62236
..accept 30757
step
'Fight your way through this room |goto 60.2,43.8 <5 |c
step
'_Gu Cloudstrike_
'_Phase 1_
.' Gu will _Invoke Lightning_ and deal Nature damage to a random party member.
.' Gu's serpent will create a _Static Field_ under a random player. Move out of this and the effect that persists.
'_Phase 2_
.' Gu will become immune to damage so focus your attention on his Azure Serpent.
.' When the serpent casts _Magnetic Shroud_, stack up. This will absorb healing and then explode and heal for the amount it absorbed.
.' _Lightning Breath_ is a frontal cone, do not stand in front of the boss.
'_Phase 3_
.' Gu Cloudstrike will use _Overcharged Soul_ which deals damage to the entire party until he is defeated.
.from Gu Cloudstrike##56747
|confirm
step
'Follow the path to this point and defeat the Shado-Pan Disciples who attack you. |goto 23.6,83.6 <5 |c
step
'Follow this ramp up and be careful not to be hit by the freezing arrows. |goto 54.0,75.2 <5 |c
step
'Cross the bridge and enter Snowdrift Dojo |goto Shado-Pan Monastery/2 |c
step
goto Shado-Pan Monastery/2 60.5,64.8
.' Fight off the waves of Shado-Pan Novices and finally defeat Fragrant Lotus and Flying Snow.
.from Fragrant Lotus##56472, Flying Snow##56473
|confirm
step
'Prepare to fight Master Snowdrift. |goto 36.6,39.2 <5 |c
step
'_Master Snowdrift_
.' _Fists of Fury_ is a frontal attack that does massive damage, the tank needs to back out of this.
.' _Tornado Kick_ is a strong whirlwind attack that should be avoided.
.' Master Snowdrift will teleport around the room and make two clones of himself. You can tell the boss from his clones by looking at the debuffs on the boss. Step behind him and avoid the projectiles being cast around the room.
.' While Master Snowdrift is in _Parry Stance_ refrain from hitting him, or find a way to hit him from behind. He will retaliate to any attack that is made.
.from Master Snowdrift##56541
|confirm
step
goto 30.3,35.4
.talk 56541
..accept 31342
step
'Go outside and cross this bridge. |goto Shado-Pan Monastery/0 24.9,41.3 <5 |c
step
'Enter the Sealed Chambers |goto Shado-Pan Monastery/3 |c
step
'_Sha of Violence_
.' The Sha of Violence will use _Disorientating Smash_ on its target, this will deal a large amount of damage and cause the target to be blinded for four seconds.
.' _Smoke Blades_ is a whirlwind attack that will leave a debuff on anybody it hits that reduces critical strike chance to 0. If you attack with this debuff it will clear it faster. After the debuff is gone you will gain _Parting Smoke_ for three seconds, this causes your next hit to critically strike for 100% more damage than it normally would.
.' _Sha Spike_ will be cast at a random player knocking them up.
.' When the Sha of Violence is weak, it will _Enrage_ gaining 50% attack speed.
.from Sha of Violence##56719 |only if not havequest(31342)
.kill Sha of Violence##56719 |q 31342/1 |only if havequest(31342)
|confirm |only if not havequest(31342)
step
'The following trash can be very tough to beat.
.' You will have to release the spirits of the dead Pandaren before killing any of the hostile adds. If you kill the adds they will simply respawn.
|confirm
step
'Follow the path to the final boss. |goto Shado-Pan Monastery/0 69.9,49.5 <5 |c
step
'_Taran Zhu_
.' The biggest mechanic in this fight is Hatred meter. If this reaches 100% you will receive the debuff _Haze of Hate_ this reduces your accuracy by 90%.
.' To counteract the Hatred Meter you will have a new _Meditate_ button above your action bars. Click this button to prevent being consumed by hatred.
.' Tanks should tank Tarn Zhu up against one of the pillars in his arena. Taran Zhu will knock the tank back and cause this fight to be more hectic than it needs to be.
.' _Ring of Malice_ is a ring of shadows that Taran Zhu will use around himself. Either be very far away or nearby to avoid this.
.' He will periodically _Summon Gripping Hatred_. This shadow balls will grab players and pull them into _Pools of Shadows_. Kill these if you can.
.from Taran Zhu##56884
.' Rid Taran Zhu of his possession. |q 30757 |only if havequest(30757)
|confirm |only if not havequest(30757)
step
goto 69.3,49.7
.talk 56884
..turnin 30757
|only if havequest(30757)
step
goto 68.4,50.1
.talk 64387
..turnin 31342
|only if havequest(31342)
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Scholomance 90",[[
dungeon 898
dungeondifficulty Heroic
description This guide will walk you through the Scholomance dungeon.
achieveid 6762
author support@zygorguides.com
step
goto Scholomance/1 27.0,58.8
.talk 64563
..accept 31442
..accept 31448
step
'Go through the gate and down these steps |goto 48.6,43.2 <5 |c
step
'Clear this room of trash and be careful not to aggro Instructor Chillheart.
|confirm
step
'_Instructor Chillheart_
'_Phase 1_
.' _Wrack Soul_ will deal damage to a party member and jump to a nearby party member when it expires. Spread out.
.' _Touch of the Grave_ causes Chillheart's next 10 basic attacks to deal additional damage.
.' _Frigid Grasp_ will summon ice at a player's feet, step out of this before it explodes.
.' _Ice Wall_ will start at the back of the room and slowly creep toward the boss. If this touches any player then they will die instantly. This will act as the enrage timer of the fight.
.' Kill Chillheart's physical body and Phase 2 will start.
.from Instructor Chillheart##58633
'_Phase 2_
.' _The Anarchist Arcanist_ will float around creating _Arcane Bombs_. These explode on impact so avoid them.
.' _Antonidas' Self Help Guide to Standing in Fire_ will cast _Burn_. This does a small AoE of fire damage around the book.
.' _Wander's Colossal Book of Shadow Puppets_ will cast _Shadow Bolt_ at the party.
.from Instructor Chillheart's Phylactery##58664
|confirm
step
goto Scholomance/1 74.7,57.5
.click In the Shadow of the Light
.' Destroy In the Shadow of the Light |q 31442/1
|only if havequest(31442)
step
map Scholomance/1
path follow strict;loop off;ants straight
path	79.3,52.0	80.6,23.8
'Follow the path downstairs. |goto Scholomance/2 77.0,26.5 <5 |c
step
goto Scholomance/2 73.9,8.6
.click Kel'Thuzad's Deep Knowledge
.' Destroy Kel'Thuzad's Deep Knowledge |q 31442/2
|only if havequest(31442)
step
'Clear all of the trash in this room before fighting Jandice Barov.
|confirm
step
'_Jandice Barov_
.' _Wondrous Rapidity_ is a large cone attack in front of Jandice, make sure nobody is in front of her.
.' _Gravity Flux_ will create a gravity distortion field on the ground. If you step into this ability you will be knocked back as well as damaged heavily.
.' At _66%_ and _33%_ Jandice will cast _Phantasmal Images_. She will clone herself throughout the room, destroy the clones until she returns to battle.
.' While the clones are alive they will cast _Whirl of Illusion_, this does AoE damage to everybody in the group every second.
.from Jandice Barov##59184
|confirm |next
step
'Enter the Chamber of Summoning |goto 56.4,26.8 <5 |c
step
Go around the perimeter of this room and kill the Boneweavers. When all of them are dead, the boss wil initiate you.
.from Boneweaver##59193
|confirm
step
'_Rattlegore_
.' With each successful attack Rattlegore will gain a stack of _Rusting_. This increases his attack damage but lowers his attack and movement speeds. If the tank is getting hit too hard, kite Rattlegore around the room.
.' _Bone Spike_ will target a random party member and do massive damage to them. This can be prevented if every party member keeps _Bone Armor_ active.
.' _Bone Armor_ is granted by clicking on the _Bone Piles_ around the room.
.' Periodically a section of the room will be engulfed in _Soulflame_. Avoid the fire and be wary because it grows over time.
.from Rattlegore##59153
|confirm
step
'Enter the Chamber of Summoning |goto 40.3,26.6 <5 |c
step
goto 42.0,46.8
'When fighting the Flesh Horrors, aim for the Meat Grafts attached to their bodies. If all of the Grafts die then the Horrors will die with them.
.from Flesh Horror##59359+, Meat Graft##59982+
|confirm
step
goto 42.5,51.6
.click Forbidden Rites and other Rituals Necromantic
.' Destroy Forbidden Rites and other Rituals Necromantic |q 31442/3
|only if havequest(31442)
step
'Go down the steps and prepare to fight Lilian Voss. |goto 47.0,47.2 <5 |c
step
'_Lilian Voss_
'_Phase 1_
.' Lilian will _Shadow Shiv_ a random player after jumping onto them. When she latches onto you, run away from your allies to avoid AoE damage.
.' _Death's Grasp_ will pull every party member to Lilian and will be followed by _Dark Blaze_. _Dark Blaze_ causes every player to start leaving a trail of fire behind them, run to the outside of the room to trail your fire, this way the center is still open.
.' At _60%_ Phase 2 begins.
'_Phase 2_
.' Lilian's Soul will begin to attack you. She will periodically cast _Blazing Soul_ which does fire damage to all players.
.' For every melee attack Lilian lan ds she will unleash another _Blazing Soul_. You must kite her aroud.
.' _Fixate Anger_ will determine who is in charge of kiting. If she Fixates on your do not stop running.
.from Lilian's Soul##58791
'_Phase 3_
.' Phase 3 is a combination of Phases 1 and 2.
.' Be watchful of who Lilian's Soul is _Fixating_ on, this is the highest source of damage during the fight.
.' You will continued to get pulled in by _Death's Grasp_, be extra careful not to put a path of _Dark Blaze_ in somebodies way if they need to kite the Soul.
.from Lilian Voss##58722
|confirm
step
goto 54.4,56.6 |n
.' Go through this hallway.
step
'Clear the _Bored Students_ out of this room before you engage _Professor Slate_
|confirm
step
'_Professor Slate_
.' He casts _Toxic Potion_, which deals nature damage to everyone standing in the green pool that spawns. Run away to avoid being killed by this.
.' Occasionally he will use a _Fire Breath Potion_, which deals heavy fire damage to everyone in front of him.
.' At 30% health, he will use a _Potion of Brutish Force_, which increases his size and physical damage done by 50% for 2 minutes.
.from Professor Slate##59613
|confirm
step
goto Scholomance/2 47.1,73.1
.click The Dark Grimoire
.' Destroy The Dark Grimoire |q 31442/4
|only if havequest(31442)
step
goto 47.8,84.8 |n
'Go through this doorway and follow the stairs down. |goto Scholomance/3 |c
step
'_Darkmaster Gandling_
.' _Immolate_ will be used on a random party member, heal through this DoT.
.' Darkmaster Gandling will cast _Rise!_. This summons many _Failed Students_ that need to be killed. During this cast Darkmaster Gandling takes reduced damage.
.' Periodically a random party member will be teleported to one of Gandling's study rooms for a _Harsh Lesson_.
.' This party member must kill the _Expired Test Subjects_ and make their way back to the bosses room. If the healer gets trapped in one of these rooms, the _Fresh Test Subjects_ around the room will serve as useful bombs. Dispel the _Explosive Pain_ off of them to deal AoE damage to the Expired Test Subjects.
.kill Darkmaster Gandling##59080 |q 31448/1 |only if havequest(31448)
.from Darkmaster Gandling##59080 |only if not havequest(31448)
|confirm |only if not havequest(31448)
step
'Go back up the stairs. |goto Scholomance/3 49.1,21.5 <5 |c
step
goto Scholomance/3 49.2,21.2
.talk 64563
..turnin 31442
..turnin 31448
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Scarlet Monastery (SM) 90",[[
dungeon 874
dungeondifficulty Heroic
description This guide will walk you through the Scarlet Monastery dungeon.
achieveid 6761
author support@zygorguides.com
step
goto Scarlet Monastery/1 72.7,47.0
.talk 64838
..accept 31515
step
goto 30.8,46.2
.' Clear the trash up to this point. The Piles of Corpses should be killed first or you will be fighting zombies much longer than is necessary.
|confirm
step
'_Thalnos the Soulrender_
.' Thalnos will _Raise Fallen Crusaders_. This ability summons zombies that the tank needs to pick up. When the Fallen Crusaders hit you, they leave behind _Mind Rot_ which is a DoT that can stack.
.' He also summons _Spirit Gales_ often. These are puddles on the ground that should not be stepped in. This ability is interruptable.
.' _Evict Soul_ is a debuff that Thalnos throws on a random party member. This DoT needs to be dispelled as quickly as possible.
.' He will also use _Summon Empowering Spirit_. This ability will summon an add that DPS should kill as quickly as they can. If this add reaches a Fallen Crusader's corpse, it will spawn an _Empowered Zombie_.
.' _Empowered Zombies_ are large zombies which need to be tanked. They cannot be killed without great focus.
.from Thalnos the Soulrender##59789
|confirm
step
goto 20.6,45.9
.click Blade of the Anointed
.get Blade of the Anointed |q 31515/1
step
map Scarlet Monastery/1
path follow strict;loop off;ants straight
path	34.5,52.5	34.8,78.2	39.4,81.7
path	48.5,90.3
.' Follow the path. |goto Scarlet Monastery/2 |noway |c
step
goto Scarlet Monastery/2 49.1,24.5
.talk 64855
..turnin 31515
..accept 31516
step
map Scarlet Monastery/2
path follow strict;loop off;ants straight
path	45.7,31.9	43.9,36.3	49.2,43.3
'_Brother Korloff_
.' Brother Korloff loves to play with fire.
.' _Flying Kick_ will hit a random party member knocking them back, make sure they aren't knocked into any uncleared trash.
.' _Blazing Fists_ is a series of punches Korloff makes in front of him, have the tank take a step back and make sure all party members are behind Korloff.
.' _Firestorm Kick_ is whirlwind of flame, do not stand in it.
.' _Scorched Earth_ is a fire trail that Korloff leaves behind him, do not stand in it.
.' Every 10%, Korloff wil gain a stack of _Rising Flame_. These increase his fire damage done by 10% per stack.
.from Brother Korloff##59223
|tip The boss will patrol between two groups. Pull each group carefully, keeping in mind the boss' patrol.
|confirm
step
'Enter the cathedral here |goto 49.1,57.1 <5 |c
step
'Before encountering the boss, make sure you kill the trash within the cathedral. This includes the main room and both wings.
|confirm
step
'_Phase 1_
.' Commander Durand will fight you alone.
.' He uses _Flash of Steel_ and _Dashing Strike_. Both of these abilities cause him to jump to party members quickly, but neither do much damage as long as the group is spread out.
.' When Durand dies, phase 2 begins.
'_Phase 2_
.' High Inquisitor Whitemane will come and and begin to fight.
.' She will cast _Power Word: Shield, Smite and Heal_. Make sure to interrupt Smite and Heal.
.' Whitemane will also try casting _Mass Resurrection_. _DO NOT LET THIS CAST FINISH!!_ If this cast finishes, all corpses within 100 yards will resurrect and aggro.
.' After you weaken her she will cast _Deep Sleep_. This puts the entire party to sleep for 10 seconds, during this time she will resurrect Durand.
'_Phase 3_
.' Now you will have to fight the two together.
.' Kill Whitemane first, make sure to interrupt _Smite and Heal_
.' Stay spread out for Durand.
.from Commander Durand##60040
.from High Inquisitor Whitemane##3977
.' Use the Blades of the Anointed on High Inquisitor Whitemane's corpse. |use Blades of the Anointed##87390 |only if havequest(31516)
.' Blades of the Anointed thrust into Whitemane's corpse. |q 31516/1 |only if havequest(31516)
|confirm |only if not havequest(31516)
step
.talk 64842
..turnin 31516
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Scarlet Halls 90",[[
dungeon 871
dungeondifficulty Heroic
description This guide will walk you through the Scarlet Monastery dungeon.
achieveid 6760
author support@zygorguides.com
step
goto Scarlet Halls/1 31.9,84.4
.talk 64764
..accept 31495
..accept 31497
step
.from Vigilant Watchman##58898+, Master Archer##59175+, Scarlet Evangelist##58685+, Scarlet Myrmidon##58683+, Scarlet Cannoneer##59293+, Scarlet Scourge Hewer##58684+, Scarlet Defender##58676+, Scarlet Evoker##58756+, Scarlet Defender##58998+, Scarlet Treasurer##59241+, Scarlet Hall Guardian##59240+, Scarlet Pupil##59373+, Scarlet Scholar##59372+
.' Kill 50 Scarlet Crusaders. |q 31495/1
|only if havequest(31495)
step
'Prepare to cross the training ground. |goto 48.2,78.8 <5 |c
step
'In order to cross the training ground you must click on an archery target and use it as a shield. Avoid the fire arrows that leave patches of flame on the ground.
.from Commander Lindon##59191
|confirm
step
' Go through the tunnel |goto 49.8,36.9 <5 |c
step
'_Houndmaster Braun_
.' Houndmaster Braun will use _Piercing Throw_. This will target a player and deal damage to every player in between Braun and his target.
.' _Death Blossom_ is an AoE whirlwind, spread out to reduce its effectiveness.
.' Both of these abilities will cause _Bloody Mess_. This is DoT can stack and become deadly.
.' At _90%, 80%, 70%, and 60%_ Braun will summon an Obedient Hound to his side. You can either kill them or just have the tank hold onto them.
.' If the hounds remain alive at _50%_ Braun will begin to lose control of them and the will aid you in battle. When this happens, Braun will enter a _Bloody Rage_, gaining _25% increased damage_ and _50% increased attack speed_.
.from Houndmaster Braun##59303
|confirm
step
goto 55.3,21.1 |n
'Allow the remaining dogs to kill the door guards and procede to the Athenaeum. |goto Scarlet Halls/2 |noway |c
step
map Scarlet Halls/2
path follow strict;loop off;ants straight
path	47.6,88.3	56.8,79.4	61.7,69.5
path	61.7,55.5
'While following this path keep an eye out for Scarlet Cannoneers. They will spawn randomly and attack using the cannons along this path.
|goto 61.7,55.5 <5 |noway |c
|modelnpc Scarlet Cannoneer##59293
step
'_Armsmaster Harlan_
.' _Dragon's Reach_ is a cleave ability, avoid being in front of this boss.
.' Harlan will periodically summon two Scarlet Defenders. You can either kill these adds or you can wait for them to be caught in Harlan's _Blades of Light_ ability.
.' He will use _Heroic Leap_ to the center of the pit and begin casting _Blades of Light_. This whirlwind quickly moves around the entire area and does massive damage to anybody caught within it. Jump between the different levels of this area to avoid this ability easily.
.from Armsmaster Harlan##58632
|confirm
step
map Scarlet Halls/2
path follow strict;loop off;ants straight
path	48.6,58.1	48.7,50.5	39.1,46.8
path	39.1,23.2
.' Follow the path to the final boss.
step
'_Flameweave Koegler_
.' Koegler will try to light the book cases around the room on fire with _Book Burner_. You can avoid this mechanic by staying on the red carpet circle inside this room.
.' He will move to the center of the room and begin using _Greater Dragon's Breath_. Watch where he is facing and move around the room clockwise to avoid being hit.
.' _Quickened Mind_ will cause Koegler's next three spells to be cast nearly instantly. This can be dispelled off of him.
.from Flameweaver Koegler##59150
.get Codex of the Crusade |q 31497/1 |only if havequest(31497)
|confirm |only if not havequest(31497)
step
goto 39.3,17.0
.talk 64764
..turnin 31495
..turnin 31497
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Gate of the Setting Sun 90",[[
dungeon 875
description This guide will walk you through the Siege of Niuzao Temple dungeon.
achieveid 6759
author support@zygorguides.com
step
goto Gate of the Setting Sun/1 60.9,88.6
.talk 64467
..accept 31363
..accept 31364
step
.' While clearing trash avoid the Volatile Munitions that drop, they deal out massive fire damage around them. |goto 49.4,87.9 <5 |c
step
'_Saboteur Kip'tilak_
.' This fight is all about the positioning of the group and the boss.
.' Saboteur Kip'tilak will throw munitions around the room at random. These will not explode unless touched by another player.
.' At _60%_ and _30%_, he will use _World in Flames_. He will throw 5 munitions are the room at random and then explode them all at the same time.
.' He will pick a player at random and _Sabotage_ them. After 4 seconds the bombs attached to that player will explode in all 4 directions, similarly to the other bombs.
.from Saboteur Kip'tilak##56906
|confirm
step
map Gate of the Setting Sun/1
path follow strict;loop off;ants straight
path	46.2,75.4	41.4,71.4	41.4,64.4
path	41.5,53.5	46.3,33.3
.' Click the lever once the group is on the elevator. |goto Gate of the Setting Sun/2 |noway |c
step
'_Striker Ga'dok_
.' _Prey Time_ will target a random party member and deal damage to them over 5 seconds.
.' _Impaling Strike_ will be an attack used primarily on the tank. It will deal half of their current life in damage.
.' _Mantid Disruptors_ will throw Acid Bombs at the platform, avoid them.
.' At both _70%_ and _30%_ Ga'dok will use _Strafing Run_. This ability will cause him to light the platform on fire. You can avoid the fire by standing on the sides to the left or right of wherever the boss starts his run.
.' The _Strafing Runs_ will also summon _Mantid Strikers_. These adds need to be picked up by the tank.
.from Striker Ga'dok##56589 |tip Watch where he flies during Strafing Run. Do not be on that side.
|confirm
step
goto Gate of the Setting Sun/2 33.4,56.9
.' Click the Flak Cannon and kill all of the mantid before going back down the elevator.
.click Flak Cannon
|confirm
step
goto 51.4,51.3 |n
'Click the lever to go back down. |goto Gate of the Setting Sun/1 |noway |c
step
goto 47.9,12.7
.' Click the Signal Flame |tip This will summon the next boss.
.' Light the Signal Flame. |q 31363/1 |only if havequest(31363)
|confirm |only if not havequest(31363)
step
goto 46.2,22.5
.' Kill the trash before engage Commander Ri'mok.
|confirm
step
'_Commander Ri'mok_
.' Commander Ri'mok's fight can get a little hectic because there is a lot going on.
.' He will summon _Mantid Swarmers_ continuously. AoE them down.
.' A _Mantid Saboteur_ will be summoned every 45 seconds. Depending on your group you may want to kill these as they spawn. Normally the tank should hold onto these adds until the boss is dead.
.' _Frenzied Assault_ will deal massive damage in a cone in front of the boss. Do not stand in front of him while he uses this.
.' _Viscous Fluid_ is a green slime pool that Ri'mok will throw on the ground. Players lose 10% damage dealt per second in this green slime while the boss gets 10% damage per second.
.from Commander Ri'mok##56636
|confirm
step
goto 46.1,43.5 |n
'Jump down into the broken building and follow the path down. |goto 49.3,45.1 <5 |noway |c
step
'Exit the building and prepare to fight Raidgonn. |goto 52.7,50.6 <5 |c
|confirm
step
'_Raigonn_
'_Phase 1_
.' Raigonn will be mostly invulnerable until his _Weak Spot_ is killed.
.' Melee can gain access to his Weak Spot by using the Artillery that are around the room.
.' While the DPS are attacking the Weak Spot, the Tank should be on the ground floor picking up adds.
.' _Krik'thik Protectorate_ will increase the attack speed of all other Protectorate in the area by 50% when one of them drops below 20% health.
.' _Krik'thik Engulfers_ should be kill by ranged DPS. They summon _Englufing Winds_ which are tornados that can be a hassle to deal with.
.' _Krik'thik Swarm Bringers will be summoned every so often. They will cast _Screeching Swarm_. This ability will absorb healing while dealing damage.
.' After the Weak Spot is destroyed, the fight enters Phase 2.
'_Phase 2_
.' He will take 300% increased damage during this phase.
.' _Stomp_ will deal damage to everybody near the boss. It will also increase his damage dealt by 25% every time he uses it.
.' _Fixate_ will cause Raigonn to drop aggro and lock his attention onto a random party member. Their movement speed will be increased by 75% while he is fixating them.
.from Raigonn##56877 |only if not havequest(31364)
.kill Raigonn##56877 |q 31364/1 |only if havequest(31364)
|confirm |only if not havequest(31364)
'|modelnpc Artillery##59819
step
goto Gate of the Setting Sun/1 52.0,60.6
.talk 64467
..turnin 31363
..turnin 31364
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Pandaria Dungeons\\Siege of Niuzao Temple (NT) 90",[[
dungeon 887
description This guide will walk you through the Siege of Niuzao Temple dungeon.
achieveid 6763
author support@zygorguides.com
step
goto Siege of Niuzao Temple/2 63.1,77.7
.talk 64517
..accept 31365
..accept 31366
step
goto 60.5,54.4
.' Jump over the ledge here.
|confirm
step
'_Vizier Jin'bak_
.' In the middle of this room is a puddle of resin. The tank should periodically walk through this puddle to shrink it.
.' Vizier Jin'bak will summon three adds, kill them before they reach the resin puddle.
.' Shortly after summoning adds, he will cast _Detonate_, this does damage to the entire group. The smaller the puddle is, the less damage the group will take.
.from Vizier Jin'bak##61567
|confirm
step
map Siege of Niuzao Temple/2
path follow strict;loop off;ants straight
path	37.4,65.4	53.4,81.8	31.6,38.4
path	21.7,53.9
'Clear the trash and follow this path. |goto Siege of Niuzao Temple/3 |noway |c
step
map Siege of Niuzao Temple/2
path follow strict;loop off;ants straight
path	67.1,55.6	52.3,23.3
'Clear the trash and follow this path. |goto Siege of Niuzao Temple/1 |noway |c
step
goto Siege of Niuzao Temple/1 44.5,74.3
.talk 61620
.' We're ready to defend!
|confirm
step
'Before you fight Commander Vo'jak you must fight the wave of adds that he sends your way.
.' Stay on the top ledge and throw Mantid Tar Kegs down onto the path as the adds run up. This will deal heavy damage and increase their damage taken.
.' Have the ranged DPS in your party prioritizing the Sik'thik Amberwings. They will fly at the wall and _Bombard_ the party. If you do not kill them then they will fly away and continue to be a hassle later.
.' Defeat Commander Vo'jak's army and click here. |confirm
step
'_Commander Vo'jak_
.' While you fight Commander Vo'jak have a ranged dps continue to throw and Mantid Tar Kegs on the boss. Be careful not to hit the tank.
.' The boss will counter this by using _Thousand Blades_, clearing his debuffs. Have the tank kite him into the puddles to reapply the debuff.
.' While the boss is using _Thousand Blades_ run away! This whirlwind will deal massive, potentially fatal, damage.
.from Commander Vo'jak##61634
|confirm
step
'While following this next path make sure you are freeing the captive Shado-Pan.
.clicknpc Shado-Pan Prisoner##64520+
.' Free 12 Shado-Pan Prisoners |q 31365/1
|confirm
|only if havequest(31365)
step
map Siege of Niuzao Temple/1
path follow strict;loop off;ants straight
path	37.8,76.5	35.4,59.8	39.1,42.7
path	46.3,43.2
.' Follow the ramp and then follow this path. |goto 46.3,43.2 <5 |noway |c
step
'The next boss patrols this area, be careful to pull the trash groups without also gaining his attention.
|confirm
step
'_General Pa'valak_
.' General Pa'valak's two main abilities are _Blade Rush_ and _Tempest_. Tempest negates healing but is nothing too significant. When he begins casting _Blade Rush_ he will mark a spot on the ground with a circle, move out of it. He throws his weapon and then charges to it.
.' He will also cast _Bulwark_. This shield will absorb one million damage while adds run in to reinforce the boss. Kill the shield and AoE down the adds.
.' _Siege Explosives_ will spawn around the area. To make this fight go quicker use these explosives on the boss to increase his damage taken. It is also suggested you save a bomb for each wave of adds.
.from General Pa'valak##61485
|confirm
step
'Make sure you have freed 12 Shado-Pan Prisoners before you move on to the last boss.
.clicknpc Shado-Pan Prisoner##64520+
.' Free 12 Shado-Pan Prisoners |q 31365/1
|confirm
|only if havequest(31365)
step
goto 50.1,33.0
.' This event can be a little hectic. The aggro range on most of the enemies can cause the group to pull an uncomfortable amount, be careful to aggro only what your group is capable of clearing in a single pull.
.' Make your way to the other side of the bridge
|confirm
step
'_Wing Leader Ner'onok_
.' Wing Leader Ner'onok will use a variety of Resins designed to hinder movement.
.' When he casts _Quick Dry Resin_ start jumping. This removes the effect.
.' He will constantly throw down pools of _Caustic Pitch_. These damage and slow you if you run through them.
.' Periodically throughout the fight Wing Leader Ner'onok will fly to the opposite end of the bridge and begin casting _Gusting Winds_. Navigate to the other side of the bridge by having your back against the pod-like structures that are all along the bridge, they allow you to move forward without being pushed back.
.' When you make it to the other end of the bridge interrupt _Gusting Winds_. A ranged interrupt comes in great handy here.
.kill Wing Leader Ner'onok##62205 |q 31366/1 |only if havequest(31366)
.from Wing Leader Ner'onok##62205 |only if not havequest(31366)
|confirm |only if not havequest(31366)
step
goto Siege of Niuzao Temple/1 58.1,23.1
.talk 64517
..turnin 31365
..turnin 31366
]])
ZygorGuidesViewer:RegisterGuideSorting({
"Classic Dungeons",
"Outland Dungeons",
"Northrend Dungeons",
"Cataclysm Dungeons"})
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Classic\\Ruins of Ahn'Qiraj",[[
dungeon 772
description This guide will walk you through the Ruins of Ahn'Qiraj Raid.
author support@zygorguides.com
step
goto Ahn'Qiraj: The Fallen Kingdom 59.5,14.0
.talk 15500
..accept 8696
|only Hunter
step
goto 59.5,14.0
.talk 15499
..accept 8712
|only Hunter
step
goto 59.4,14.1
.talk 15498
..accept 8704
|only Hunter
step
goto 59.5,14.0
.talk 15500
..accept 8692
|only Druid
step
goto 59.5,14.0
.talk 15499
..accept 8708
|only Druid
step
goto 59.4,14.1
.talk 15498
..accept 8700
|only Druid
step
goto 59.5,14.0
.talk 15500
..accept 8691
|only Mage
step
goto 59.5,14.0
.talk 15499
..accept 8707
|only Mage
step
goto 59.4,14.1
.talk 15498
..accept 8699
|only Mage
step
goto 59.5,14.0
.talk 15500
..accept 8693
|only Rogue
step
goto 59.5,14.0
.talk 15499
..accept 8709
|only Rogue
step
goto 59.4,14.1
.talk 15498
..accept 8701
|only Rogue
step
goto 59.5,14.0
.talk 15500
..accept 8695
|only Paladin
step
goto 59.5,14.0
.talk 15499
..accept 8711
|only Paladin
step
goto 59.4,14.1
.talk 15498
..accept 8703
|only Paladin
step
goto 59.5,14.0
.talk 15500
..accept 8690
|only Shaman
step
goto 59.5,14.0
.talk 15499
..accept 8706
|only Shaman
step
goto 59.4,14.1
.talk 15498
..accept 8698
|only Shaman
step
goto 59.5,14.0
.talk 15500
..accept 8557
|only Warrior
step
goto 59.5,14.0
.talk 15499
..accept 8558
|only Warrior
step
goto 59.4,14.1
.talk 15498
..accept 8556
|only Warrior
step
goto 59.5,14.0
.talk 15500
..accept 8689
|only Priest
step
goto 59.5,14.0
.talk 15499
..accept 8705
|only Priest
step
goto 59.4,14.1
.talk 15498
..accept 8697
|only Priest
step
goto 59.5,14.0
.talk 15500
..accept 8694
|only Warlock
step
goto 59.5,14.0
.talk 15499
..accept 8710
|only Warlock
step
goto 59.4,14.1
.talk 15498
..accept 8702
|only Warlock
step
map  Ruins of Ahn'Qiraj
path loop off
path	60.8,17.4	62.5,19.7	62.9,23.5
path	58.7,27.9	56.9,31.6
.' Follow the path, clearing trash as you go until you get to the first boss, Kurannaxx. |goto 56.9,31.6 <5 |c |noway
step
goto 56.5,34.4
'_Kurinnaxx_
'_Phase 1_
.' _Mortal Wounds_ is a stackable debuff on the main tank that reduces healing by 10%. It hits as a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _Sandtrap_ affects the entire group. Random sandtraps spawn under a player's feet and erupts, dealing 2000 damage, silencing them, and reducing their chance to hit by 75% for 20 seconds. Keep an eye on your feet and move out of the way as quickly as possible.
.' _Summon_ teleports a random player to the boss' location.
.' _Thrash_ makes Kurannaxx hit two additional times.
'_Phase 2_
.' _Frenzy_ occurs when the boss reaches 30% health, this increases his attack speed by 75% and the Physical damage it deals by 250. Be sure to kill him quickly when he reaches this stage to avoid death.
|modelnpc Kurinnaxx##15348
|confirm
step
goto Ruins of Ahn'Qiraj 55.2,38.0
.talk 15471
.' Tell him "Let's find out."
.' This will start the next boss encounter, make sure you area ready.
|confirm
step
goto 59.1,42.8 |n
.' Follow the Lieutenant and his men here |goto 59.1,42.8 <5 |c
step
goto 59.1,42.8
'_Wave 1_
'_Captain Qeez_
.' He has a _cleave_ attack, so make sure that the main tank is the only one that is in front of the boss when this occurs.
|confirm
step
goto 59.1,42.8
'_Wave 2_
'_Captain Tuubid_
.' _Attack Order_ debuff causes the mobs to aggro to that player.
|confirm
step
goto 59.1,42.8
'_Wave 3_
'_Captain Drenn_
.' _Hurricane_ AoE lightning spell. Move out of the tornadoes that spawn.
|confirm
step
goto 59.1,42.8
'_Wave 4_
'_Captain Xurrem_
.' _Shockwave_ is an AoE knockdown, it has a short range so only melee should have to deal with it.
|confirm
step
goto 59.1,42.8
'_Wave 5_
'_Major Yeggeth_
.' _Blessing of Protection_ is a cleave and deals 900 damage, make sure the boss is turned away from the group.
.' _Shield of Rajaxx_ makes Yeggeth immune to all schools of magic for 6 seconds.
|confirm
step
goto 59.1,42.8
'_Wave 6_
'_Major Pakkon_
.' _Sweeping Slam_  AoE attack. It has a short range and only melee should have to deal with it.
|confirm
step
goto 59.1,42.8
'_Wave 7_
'_Colonel Zerran_
.' _Enlarge_ is a magic-dispellable buff that increases his melee damage. It also increases the damage of his adds. Dispell as fast as possible.
|confirm
step
goto 59.1,42.8
'_General Rajaxx_
.' _Thunder Clap_ cuts everyone's hitpoints in half and wipes all player's aggro. Healers should be ready for this and tanks will need to re-taunt.
.' He will yell _You are not worth my time, <player name>_ this dumps all of that player's aggro, and he will engage a new target.
|modelnpc General Rajaxx##15341
|confirm
step
map  Ruins of Ahn'Qiraj
path loop off
path	54.9,50.0	52.5,47.3	52.3,41.5
path	45.2,34.2	36.1,33.3
.' Follow the path, clearing trash as you go until you get to the next boss, Moam. |goto 36.1,33.3 <5 |c |noway
step
goto 32.3,35.8
'_Moam_
.' _Trample_ AoE melee attack, melee classes should be wary of this.
.' _Drain Mana_ takes 500 mana from 6 people and replenishes Moam's mana. This can be countered by keeping his mana drained.
.' _Arcane Explosion_ occurs if Moam reaches 100% mana. Huge AoE that deals 3000 damage, knocks players back, and then deals fall damage.
.' _Summon Mana Fiend_ occurs 90 seconds into the fight. Moam summoms 3 Mana Fiends. Must be killed off quickly. These are banishable and rootable and use Arcane Explosion.
.' _Turn to Stone_ also occurs 90 seconds into the fight. Moam turns to stone and cannot attack or use Drain Mana (he still regenerates some mana over time). He comes out of stone after 90 seconds or if his mana reaches 100%.
|modelnpc Moam##15340
|confirm
step
map  Ruins of Ahn'Qiraj
path loop off
path	31.1,41.8	33.9,45.5	34.0,50.4
path	39.9,58.9	42.9,69.9
.' Follow the path, clearing trash as you go until you get to the next boss, Ossirian The Unscarred. |goto 42.9,69.9 <5 |c |noway
step
goto 40.5,74.4
'_Ossirian The Unscarred_
.' _Strength of Ossirian_ Ossirian starts the fight with this buff, dealing massive melee damage. Ccounter this by kiting him to the crystals that spawn around the area. They greatly reduce his damage dealth and makes him vulnerable to a school of magic. Typically a specific raid member will be assigned to run around and scout out the crystal locations for the rest of the raid.
.' _Enveloping Winds_ is a 10 second stun to whoever has aggro. He will then ignore this person but their threat will be preserved. The effect can break upon taking damage.
.' _Tornadoes_ spawn when Ossirian is engaged. They will move around the area throughout the fight and deal high damage to anyone caught in them. Keep an eye out and avoid them.
.' _Warstomp_ is a short range AoE spell that hits around Ossirian and deals 1500 damage and knocks players back.
.' _Curse of Tongues_ is an AoE debuff that increases cast time of those caught in it. Avoid this by keeping the casters at range.
|modelnpc Ossirian The Unscarred##15339
|confirm
step
map  Ruins of Ahn'Qiraj
path loop off
path	50.1,74.1	55.1,81.3
.' Follow the path, clearing trash as you go until you get to the next boss, Ayamiss The Hunter. |goto 55.1,81.3 <5 |c |noway
step
goto 58.3,85.7
'_Ayamiss The Hunter_
'_Phase 1_
.' _Fly_ Ayamiss is airborne and cannot be hit with melee attacks during this phase. This ends once he reaches 70% health.
'_Phase 2_
.' _Poison Stinger_ applies a debuff that does 25 Nature damage per second but can stack up to 100 times. He only uses this while flying and is cast upon the player with most aggro.
.' _Stinger Spray_ does 1000 Nature damage to the raid. This occurs approximately every 30 seconds.
.' _Hive'Zara Swarmer_ summons about 20 wasps that fly up in the air. They attack together as they descend. Are relatively easy to deal with due to their low hit points.
.' _Paralyze_ shackles a player to the altar. An add will spawn and begin moving towards them, it must be killed before it reaches them or it will kill them and spawn another elite mob.
.' _Note_ this boss is immune to taunts.
|modelnpc Ayamiss The Hunter##15369
|confirm
step
map  Ruins of Ahn'Qiraj
path loop off
path	59.1,80.3	63.9,80.9	67.8,74.6
path	69.6,68.9
.' Follow the path, killing trash until you get to the last boss, Buru the Gorger. |goto 69.6,68.9 <5 |c |noway
step
goto 70.6,63.7
'_Buru the Gorger_
.' _Eggs_ deal 100-500 damage to players based on their proximity to the egg. They deal 45000 damage to Buru and is the only way to damage him.
.' _Dismember_ is a stacking bleed debuff that does 1248 damage every 2 seconds. Buru will use this if he catches up to the player that is kiting him.
.' _Speeding Up_ gives Buru increased movement speed while chasing a player. Remove this effect by hurting him with an egg explosion.
.' _Creeping Plague_ is activated when he has less than 20% health. It is a raid-wide stacking debuff that deals increased damage every 3 ticks. At this point the fight becomes a dps race.
.' _Thorns_ deals 200 damage to melee attackers.
.' _Note_ this boss is immune to taunts.
|modelnpc Buru the Gorger##15370
|confirm
step
goto Ahn'Qiraj: The Fallen Kingdom 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8696
|only if havequest(8696)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8712
|only if havequest(8712)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8704
|only if havequest(8704)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8692
|only if havequest(8692)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8708
|only if havequest(8708)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8700
|only if havequest(8700)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8691
|only if havequest(8691)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8707
|only if havequest(8707)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8699
|only if havequest(8699)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8693
|only if havequest(8693)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8709
|only if havequest(8709)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8701
|only if havequest(8701)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8695
|only if havequest(8695)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8711
|only if havequest(8711)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8703
|only if havequest(8703)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8690
|only if havequest(8690)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8706
|only if havequest(8706)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8698
|only if havequest(8698)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8557
|only if havequest(8557)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8558
|only if havequest(8558)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8556
|only if havequest(8556)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8689
|only if havequest(8689)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8705
|only if havequest(8705)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8697
|only if havequest(8697)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15500
..turnin 8694
|only if havequest(8694)
step
goto 59.5,14.0
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15499
..turnin 8710
|only if havequest(8710)
step
goto 59.4,14.1
.' It usually takes more than one run to get all the items needed for this quest.
.talk 15498
..turnin 8702
|only if havequest(8702)
step
.' Click the Head of Ossirian the Unscarred in your bags |use Head of Ossirian the Unscarred##21220
.accept 8791
|only if not completedq(8791)
step
goto Silithus 53.2,32.5
.talk 15181
..turnin 8791
|only if havequest(8791)
step
.' Congratulations, you have finished the Ruins of Ahn'Qiraj raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Classic\\Temple of Ahn'Qiraj",[[
dungeon 766
description This guide will walk you through the Ruins of Ahn'Qiraj Raid.
author support@zygorguides.com
step
map Ahn'Qiraj/2
path loop off
path	41.8,19.8	39.1,26.2
.' Follow the path, clearing trash as you go until you get to the first boss, The Prophet Skeram. |goto 39.1,26.2 <5 |noway |c
step
goto 43.5,41.3
'_The Prophet Skeram_
.' _True Fulfillment_ mind controls a target, giving them increased damage, higher resistances, increased movement speed, and instant-cast spells. The target grows in size, and can be sheeped, feared, stunned, or put to sleep.
.' _Quartile Split_ occurs at 75%, 50%, and 25%, creating two images that share Skeram's abilities. These have much less hit points than the real Skeram and need to be killed when they spawn.
.' _Arcane Explosion_ AoE hits for 1500 damage in a large radius. Skeram casts this only if there are more than 4 players in melee range. This spell is interruptable.
.' _Earth Shock_ hits Skeram's target for 2500 damage every second until that target is in melee range.
.' _Blinks_ to either platform beside him. This also wipes aggro.
|modelnpc The Prophet Skeram##15263
|confirm
step
goto 59.2,68.8
.talk 15503
..turnin 8595 |instant |or
..turnin 8579 |instant
step
goto 59.1,68.7
.talk 15503
..accept 8658
..accept 8626
|only Hunter
step
goto 59.4,68.5
.talk 15504
..accept 8656
|only Hunter
step
goto 59.6,68.2
.talk 15502
..accept 8657
..accept 8659
|only Hunter
step
goto 59.1,68.7
.talk 15503
..accept 8624
..accept 8621
|only Shaman
step
goto 59.4,68.5
.talk 15504
..accept 8622
|only Shaman
step
goto 59.6,68.2
.talk 15502
..accept 8623
..accept 8602
|only Shaman
step
goto 59.1,68.7
.talk 15503
..accept 8668
..accept 8665
|only Druid
step
goto 59.4,68.5
.talk 15504
..accept 8666
|only Druid
step
goto 59.6,68.2
.talk 15502
..accept 8667
..accept 8669
|only Druid
step
goto 59.1,68.7
.talk 15503
..accept 8631
..accept 8634
|only Mage
step
goto 59.4,68.5
.talk 15504
..accept 8633
|only Mage
step
goto 59.6,68.2
.talk 15502
..accept 8632
..accept 8625
|only Mage
step
goto 59.1,68.7
.talk 15503
..accept 8663
..accept 8660
|only Warlock
step
goto 59.4,68.5
.talk 15504
..accept 8661
|only Warlock
step
goto 59.6,68.2
.talk 15502
..accept 8662
..accept 8664
|only Warlock
step
goto 59.1,68.7
.talk 15503
..accept 8640
..accept 8637
|only Rogue
step
goto 59.4,68.5
.talk 15504
..accept 8638
|only Rogue
step
goto 59.6,68.2
.talk 15502
..accept 8639
..accept 8641
|only Rogue
step
goto 59.1,68.7
.talk 15503
..accept 8560
..accept 8559
|only Warrior
step
goto 59.4,68.5
.talk 15504
..accept 8562
|only Warrior
step
goto 59.6,68.2
.talk 15502
..accept 8561
..accept 8544
|only Warrior
step
goto 59.1,68.7
.talk 15503
..accept 8629
..accept 8655
|only Paladin
step
goto 59.4,68.5
.talk 15504
..accept 8627
|only Paladin
step
goto 59.6,68.2
.talk 15502
..accept 8628
..accept 8630
|only Paladin
step
goto 59.1,68.7
.talk 15503
..accept 8593
..accept 8596
|only Priest
step
goto 59.4,68.5
.talk 15504
..accept 8603
|only Priest
step
goto 59.6,68.2
.talk 15502
..accept 8592
..accept 8594
|only Priest
step
map Ahn'Qiraj
path loop off
path	Ahn'Qiraj/2 49.5,65.5	Ahn'Qiraj/3 46.1,24.3	Ahn'Qiraj/3 42.9,30.2
path	Ahn'Qiraj/3 47.8,67.9	Ahn'Qiraj/1 33.3,51.3	Ahn'Qiraj/1 35.0,49.5
path	Ahn'Qiraj/1 31.8,49.7	Ahn'Qiraj/1 29.5,49.4
.' Follow the path, clearing trash as you go until you get to the next boss, the Bug Trio. |goto 29.5,49.4 <5 |noway |c
step
goto 27.9,49.6
'_Bug Trio_
'_
'_Lord Kri_
.' This boss has a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _Toxic Volley_ hits for 500 Nature damage and inflicts a DoT that ticks for 125 damage. It can stack any number of times but can be dispelled.
.' _Death Effect:_ his corpse leaves a poison cloud that DoTs for 2000 damage per second, usually resulting in death if stood in.
'_Princess Yauj_
.' _Fear_ AoE fear. If the raid party is feared too far away, the fight can bug and reset. Tank her as far from the main raid as possible.
.' _Great Heal_ is a large single=target heal that she uses on herself or one of the other bug bosses. Interrupting this is critical.
.' _Death Effect:_ spawns several bug adds when she dies. These can be cc'ed with normal means and aren't usually an issue.
'_Vem_
.' _Berserker Charge_ Vem charges at a raid member, knocking them back and dealing extra damage.
.' _Knockdown_, Vem does slightly increased damage and knocks down his target and any nearby allies, stunning them for 2 seconds.
.' _Death Effect:_ enrages Yauj and Kri if they are still alive. Which will greatly increase their attack speed and damage. This is designed to wipe the raid. Kill them as quickly as possible to avoid death.
|modelnpc Lord Kri##15511
|modelnpc Princess Yauj##15543
|modelnpc Vem##15544
|confirm
step
map Ahn'Qiraj/1
path follow loose;loop off;ants straight
path	30.5,50.3	35.0,49.7	33.1,46.8
path	31.8,41.2	34.3,40.6	37.9,38.9
path	40.9,36.7
.' Follow the path, clearing trash as you go until you get to the next boss, Battleguard Sartura. |goto 40.9,36.7 <5 |noway |c
step
goto 43.1,35.2
'_Battleguard Sartura_
.' _Whirlwind_ makes Sartura immune to stun. While using this spell, she has increased movement speed and deals AoE damage. Avoid her and her guards when they are using this.
.' _Aggro drop_ Sartura and her guard occasionally reset aggro and will chase random raid members for 5 seconds. After the 5 seconds they will return to the one with highest aggro.
.' _Frenzy_ occurs when Sartura reaches 20% health, dealing 124 more physical damage and increased attack speed by 60%. Kill her as quickly as possible to avoid death.
.' _Enrage_ occurs after 10 minutes of combat. It will greatly increase her attack speed and damage. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
.' Her _guards_ use a knockback occasionally.
|modelnpc Battleguard Sartura##15516
|confirm
step
map Ahn'Qiraj/1
path follow loose;loop off;ants straight
path	44.6,29.2	47.4,18.5	56.2,14.6
path	60.1,19.3
.' Follow the path, clearing trash as you go until you get to the next boss, Fankriss the Unyielding. |goto 60.1,19.3 <5 |noway |c
step
goto 61.6,22.8
'_Fankriss the Unyielding_
.' _Mortal Wounds_ is a stackable debuff on the main tank that reduces healing by 10%. This is a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _Summon Worm_ is used randomly during the fight, and spawns 1-3 elite worm adds. They _enrage_ after 20 seconds, dealing 10,000-20,000 damage per hit. Kill them as soon as they spawn to prevent death.
.' _Adds_ will be spawned shortly after the fight starts and then again every minute. 3 groups of 4 adds will spawn, 1 group coming from each of the large tunnels that shoot off from the main room. When this happens a raid member will be teleported and rooted near one of the spawns. The adds can be snared or feared, and need to be killed quickly.
|modelnpc Fankriss the Unyielding##15510
|confirm
step
.' The next boss. Viscidus, is optional. He can only be killed if you and your raid members can deal Frost damage. It takes 200 Frost attacks to freeze him and then about 200 melee attacks. You and your group can skip this boss or fight him.
.' Click here to _fight_ Viscidus! |next "fightbonus" |confirm
.'_
.' Click here to _skip_ Viscidus! |next "skipbonus" |confirm
step
label "fightbonus"
map Ahn'Qiraj/1
path follow loose;loop off;ants straight
path	65.1,22.3	66.1,19.4	66.3,17.6
path	68.8,18.2
.' Follow the path up the brown pipe until you get to the next boss, Viscidus. |goto 68.8,18.2 <5 |noway |c
step
goto 70.7,18.4
'_Viscidus_
.' _Poison Shock_ is a 15 yard AoE that deals 1200 Nature damage every 10 seconds.
.' _Poison Volley_ is an AoE that does 1500 Nature damage and applies a DoT that does 500 damage every 2 seconds for 10 seconds. The DoT can be dispelled.
.' _Toxin Clouds_ are shot out by Viscidus every 30-40 seconds. It is an AoE that deals 1500 damage every 2 seconds and slows movement speed by 40%. The damage radius is bigger than the graphic so be sure to move way out of it.
'_Frost Phases_
.' The only way to kill Viscidus is to freeze him with ice and then shatter him.
.' _"Viscidus begins to slow"_ his movement and attack speed is reduced by 15%. This phase starts after he is hit with 100 Frost attacks,
.' _"Viscidus is freezing up"_ his movement and attack speed is reduced by 30%. This phase starts after he is hit with 150 Frost attacks.
.' _"Viscidus is frozen solid"_ his movement and attacks stop completely. The raid now needs to switch to melee attacks and "shatter" him. This phase starts after Viscidus is hit with 200 Frost attacks
'_Melee Phases_
.' _"Viscidus begins to crack"_ continue attacking with melee.
.' _"Viscidus looks ready to shatter"_ continue attacking with melee.
.' _"Viscidus explodes"_ Viscidus shatters into Globs of Viscidus. Each glob represents 5% of his total health. Destroy these to do damage. If he is below 5% health when shattered, he will die. If the Globs reach the center of the room, Viscidus will reform.
|modelnpc Viscidus##15299
|confirm
step
label "skipbonus"
map Ahn'Qiraj/1
path follow loose;loop off;ants straight
path	60.8,27.5	56.3,34.1	55.7,43.5
path	54.2,49.9	50.4,51.8	47.3,50.9
.' Follow the path, clearing trash as you go until you get to the next boss, Princess Huhuran. |goto 47.3,50.9 <5 |noway |c
step
goto 44.3,47.3
'_Princess Huhuran_
.' _Frenzy_ Huhuran's damage is greatly increased while she is in this form. A Hunter can remove it with Tranquilizing Shot.
.' _Poison Bolt_ deals 2000 damage to the closest 15 people. This only occurs when she Frenzies or Berserks.
.' _Noxious Poison_ deals 2900 Nature damage over 8 seconds and a Silence. This will hit a random person and any raid members around Huhuran.
.' _Wyvern Sting_ is an AoE sleep effect on a random area within melee range and can affect up to 10 raid members. If cleansed it deals 3000 Nature damage.
.' _Acid Spit_ DoT deals about 250 damage every 2 seconds. This DoT is stackable and tanks will need to be rotated for healing.
.' _Berserk_ occurs when Huhuran reaches 30% health. Her attack speed doubles and she releases poison bolts that deal 2000 damage every 3 seconds to the closest 15 raid members. Kill her as quickly as possible to avoid death.
|modelnpc Princess Huhuran##15509
|confirm
step
map Ahn'Qiraj/1
path follow loose;loop off;ants straight
path	44.6,54.6	47.4,61.5	56.1,68.3
.' Follow the path, clearing trash as you go until you get to the next boss, the Twin Emperors. |goto 56.1,68.3 <5 |noway |c
step
goto 57.7,70.7
'_The Twin Emperors_
.' Both share the following abilities:
.' _Heal Brother_ occurs whenever the Twins get within 60 yards of one another. They will spam this heal on each other, healing for 30,000 health per tick. They must be tanked far apart.
.' _Twin Teleport_ occurs every 30-40 seconds. The Twins switch places. This effect is preceded by a blue glow at their feet. After the teleport both Twins wipe all aggro and will attack the closest raid member to them.
.' _Combined Health_ the Twins share health percentage. Damaging one of them also does damage to the other.
.' _Berserk_ occurs after 15 minutes of combat. It will greatly increase their attack speed and damage. This is designed to wipe the raid. Kill them before they reach this stage to avoid death.
'_Emperor Vek'nilash_
.' _Melee_
.' _Immune_ to all magic damage (except Holy).
.' _Uppercut_ knocks back a single random target in melee range.
.' _Unbalancing Strike_ deals 350% weapon damage and leaves the target unbalanced, lowering their defense by 100 for 6 seconds.
.' _Mutate Bug_ mutates a bug every 10-15 seconds making it grow and attack the raid. Needs to be killed off.
'_Emperor Vek'lor_
.' _Caster_
.' _Immune_ to all physical damage.
.' _Shadow Bolt_ is spammed on Vek'lor's aggro target, hitting for 3000-4000.
.' _Blizzard_ AoE that does 1500 damage per tick and slows those within it.
.' _Arcane Burst_ AoE that deals 4000-5000 Arcane damage whenever a player is within melee range. Knocks back anyone it hits and slows their movement speed by 70%.
.' _Explode Bug_ every 7-10 seconds Vek'lor forces a nearby bug to explode, dealing a large amount of damage over a wide area.
|modelnpc Emperor Vek'nilash##15275
|modelnpc Emperor Vek'lor##15276
|confirm
step
.' The next boss, Ouro the Sand Worm, is optional.
.' Kill him! |next "killworm" |confirm
.'_
.' Skip him! |next "skipworm" |confirm
step
label "killworm"
map Ahn'Qiraj/1
path follow loose;loop off;ants straight
path	57.1,73.3	53.9,77.2	50.0,80.8
path	45.6,82.3	42.2,80.2	37.9,75.6
path	33.8,76.1
.' Follow the path, clearing trash as you go until you get to Ouro. |goto 33.8,76.1 <5 |noway |c
step
goto 30.6,79.3
'_Ouro_
.' _Sweep_ deals 1000-2500 Physical damage to his target, alonog with an AoE knockback. This usually drops the current tank's aggro and Ouro will need to be picked up by another tank.
.' _Sand Blast_ occurs every 20-25 seconds, dealing about 4000 Nature damage to the person with the highest threat. Also hits like a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _Submerge_ Ouro burrows underground, becoming untargetable and undamagable. He has a chance to do this every 1.5 minutes, it is random. He will also Submerge if no player is in melee range.
.' _Ground Rupture_ deals 2000 Physical damage to anyone on top of Ouro when he reappears from a Submerge.
.' _Dirt Mound's Quake_ deals 1500 Nature damage when Ouro is Submerged.
.' _Scarab Adds_ will spawn after each resurface. They despawn after 45 seconds, so any form of crowd control is enough to handle them.
.' _Enrage_ occurs when Ouro has 20% health left. It increases his attack speed by 150% and doubles his damage. He will no longer Submerge at this point. Kill him as fast as possible to avoid death.
|modelnpc Ouro##15517
|next "finalstart"|confirm
step
label "skipworm"
map Ahn'Qiraj/1
path follow loose;loop off;ants straight
path	57.1,73.3	53.9,77.2	50.0,80.8
path	45.6,82.3	42.2,80.2	37.9,75.6
path	37.6,70.3
.' Follow the path, clearing trash as you go |goto 37.6,70.3 <5 |noway |next "finalstart" |c
step
label "finalstart"
map Ahn'Qiraj
path follow loose;loop off;ants straight
path	Ahn'Qiraj/1 37.9,67.8	Ahn'Qiraj/1 32.1,56.1	Ahn'Qiraj/1 35.5,50.0
path	Ahn'Qiraj/1 34.0,45.2	Ahn'Qiraj/3 67.9,53.3	Ahn'Qiraj/3 71.4,75.8
path	Ahn'Qiraj/3 62.2,85.1
.' Follow the path through the final area as you make your way to the last boss, C'Thun. |goto Ahn'Qiraj/3 62.4,86.0 <5 |noway |c
step
goto 58.4,67.2
'_C'Thun_
'_Phase 1_
.' _Eye Beam_ , a green energy beam, is cast every 3 seconds by Eye of C'Thun and deals about 3000 Nature damage to a random target. If another player is within 10 yards of this target, the beam will jump to them, dealing 1.5x the damage dealt to the previous target. Raid members will want to stay spread out to avoid this happening.
.' _Dark Glare_ a red energy beam, deals about 50,000 Shadow damage to any player who passes through the beam. The initial target is random, but he will rotate 180 degrees clockwise or counterclockwise. When this happens, move out of its way.
.' _Eye Tentacle_ will spawn 8 small eye tentacles every 45 seconds, knocking back anyone within a few yards of their spawn point.  They use _Mind Flay_ which deals 750 damage every second for 3 seconds. If the group is ready for them they are easily killed due to their low hit points.
.' _Claw Tentacle_ spawn at random locations very often. When they spawn they deal about 1500 damage and a knockback.
'_Phase 2_ starts as soon as the Eye of C'Thun's health reaches 0.
.' _Giant Claw Tentacle_ spawns about every minute. These deal 3500 damage with Ground Rupture, and will need to be tanked due to their high damage. If no one is in range for an attack they will burrow and reappear with full health, repeating the Ground Rupture attack.
.' _Giant Eye Tentacle_ spawns about every minute. These also deal significant damage with Ground Rupture and they cast _Eye Beam_, which bounces off nearby raid members. Killing these are a priority.
'_Stomach of C'Thun_
.' Every 10 seconds, a random raid member will be swallowed by C'Thun and dropped into his stomach. Swim out of the acid pools and onto one of the patches of land. _Flesh Tentacles_ have 25,000 health and killing them weakens C'Thun, causing him to turn purple and become vulnerable for 45 seconds. This is the time you use to attack and kill him.
|modelnpc C'Thun##15727
|confirm
step
.' Click the Eye of C'Thun in your bags |use Eye of C'Thun##21221
..accept 8801
|only if not completedq(8801)
step
goto 51.2,85
.talk 15379
..turnin 8801
|only if not completedq(8801)
step
.' Throughout the raid, you may have looted a quest item. If not, just skip this step.
.click the Ancient Qiraji Artifact in your bags |use Ancient Qiraji Artifact##21230
..accept 8784
step
goto Ahn'Qiraj/2 59.1,68.7
.talk 15503
..turnin 8658
..turnin 8626
|only Hunter
step
goto 59.4,68.5
.talk 15504
..turnin 8656
|only Hunter
step
goto 59.6,68.2
.talk 15502
..turnin 8657
..turnin 8659
|only Hunter
step
goto 59.1,68.7
.talk 15503
..turnin 8624
..turnin 8621
|only Shaman
step
goto 59.4,68.5
.talk 15504
..turnin 8622
|only Shaman
step
goto 59.6,68.2
.talk 15502
..turnin 8623
..turnin 8602
|only Shaman
step
goto 59.1,68.7
.talk 15503
..turnin 8668
..turnin 8665
|only Druid
step
goto 59.4,68.5
.talk 15504
..turnin 8666
|only Druid
step
goto 59.6,68.2
.talk 15502
..turnin 8667
..turnin 8669
|only Druid
step
goto 59.1,68.7
.talk 15503
..turnin 8631
..turnin 8634
|only Mage
step
goto 59.4,68.5
.talk 15504
..turnin 8633
|only Mage
step
goto 59.6,68.2
.talk 15502
..turnin 8632
..turnin 8625
|only Mage
step
goto 59.1,68.7
.talk 15503
..turnin 8663
..turnin 8660
|only Warlock
step
goto 59.4,68.5
.talk 15504
..turnin 8661
|only Warlock
step
goto 59.6,68.2
.talk 15502
..turnin 8662
..turnin 8664
|only Warlock
step
goto 59.1,68.7
.talk 15503
..turnin 8640
..turnin 8637
|only Rogue
step
goto 59.4,68.5
.talk 15504
..turnin 8638
|only Rogue
step
goto 59.6,68.2
.talk 15502
..turnin 8639
..turnin 8641
|only Rogue
step
goto 59.1,68.7
.talk 15503
..turnin 8560
..turnin 8559
|only Warrior
step
goto 59.4,68.5
.talk 15504
..turnin 8562
|only Warrior
step
goto 59.6,68.2
.talk 15502
..turnin 8561
..turnin 8544
|only Warrior
step
goto 59.1,68.7
.talk 15503
..turnin 8629
..turnin 8655
|only Paladin
step
goto 59.4,68.5
.talk 15504
..turnin 8627
|only Paladin
step
goto 59.6,68.2
.talk 15502
..turnin 8628
..turnin 8630
|only Paladin
step
goto 59.1,68.7
.talk 15503
..turnin 8593
..turnin 8596
|only Priest
step
goto 59.4,68.5
.talk 15504
..turnin 8603
|only Priest
step
goto 59.6,68.2
.talk 15502
..turnin 8592
..turnin 8594
|only Priest
step
goto 59.6,68.2
.talk 15502
..turnin 8784
|only if havequest(8784)
step
goto 59.2,68.8
.talk 15503
.' Turn in any extra Qiraji Lord's Insginias you have.
..turnin 8595 |instant |or
..turnin 8579 |instant
|confirm
step
.' Congratulations, you have finished the Temple of Ahn'Qiraj raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Classic\\Molten Core",[[
dungeon 696
description This guide will walk you through the Molten Core Raid.
author support@zygorguides.com
step
goto Burning Steppes/16 54.3,83.4
.talk 14387
.' Tell him "Transport me to the Molten Core." |goto Molten Core/1 27.9,26.5 <5 |noway |c
step
map Molten Core/1
path loop off
path	31.2,22.8	36.2,17.3	43.8,15.8
path	47.3,20.4	49.1,29.2	53.1,30.1
path	56.5,32.4	62.5,40.2
.' Follow the path, clearing trash as you make your way to the first boss, Lucifron. |goto 62.5,40.2 <5 |noway |c
step
goto 66.3,35.6
'_Lucifron_
.' _Impending Doom_ inflicts 2000 Shadow damage to nearby raid members after 10 seconds.
.' _Lucifron's Curse_ is a debuff applied to nearby raid members, increasing the cost of their spells and abilities by 100% for 5 minutes.
.' _Shadow Shock_ hits all raid members within 20 yards of Lucifron for Shadow damage.
'_Flamewalker Protectors_
.' _Cleave_ inflicts melee damage to the targeted raid member and up to 5 nearby allies. Make sure the tank is the only one in front of the Protector when this occurs.
.' _Dominate Mind_ takes control of a player for 15 seconds.
|modelnpc Lucifron##12118
|confirm
step
.' Clear trash here before engaging the next boss, Magmadar. |goto 68.5,26.3 <5 |c
step
goto 70.1,19.6
'_Magmadar_
.' _Magma Spit_ deals 100 Fire damage and another 75 Fire damage every 3 seconds for 30 seconds. Stacks up to 3 times.
.' _Lava Breath_ inflicts 1200 Fire damage to raid members in front of Magmadar. Make sure the tank is the only one in front of Magmadar when this occurs.
.' _Panic_ is an AoE Fear that causes raid members near Magmadar to flee in fear for 8 seconds.
.' _Lava Bomb_ is an AoE spell that deals 3200 Fire damage over 8 seconds to any player standing in the affected area.
.' _Frenzy_ occurs at 30% health, increasing Magmadar's attack speed by 150%. Kill him as quickly as possible to avoid death.
|modelnpc Magmadar##11982
|confirm
step
map Molten Core/1
path loop off
path	62.5,40.1	59.7,35.9	55.0,31.9
path	50.4,32.9	44.6,30.7	40.9,38.6
path	36.8,51.1
.' Follow the path, clearing trash as you make your way to the next boss, Gehennas. |goto 36.8,51.1 <5 |noway |c
step
goto 32,48
'_Gehennas_
.' _Shadow Bolt_ deals 2000-2500 Shadow damage to a non-tank raid member.
.' _Rain of Fire_ is an AoE that deals 925-1075 Fire damage every 2 seconds for 6 seconds.
.' _Gehennas' Curse_ reduces healing effects of nearby raid members by 75% for 5 minutes.
'_Flamewalkers_
.' _Fist of Ragnaros_ stuns nearby raid members for 4 seconds.
.' _Sunder Armor_ is a stacking debuff that reduces the target's armor by 1000 and stacks up to 20 times.
|modelnpc Gehennas##12259
|confirm
step
map Molten Core/1
path loop off
path	34.1,51.8	34.0,61.3	32.6,66.2
.' Follow the path, clearing trash as you make your way to the next boss, Garr. |goto 32.6,66.2 <5 |noway |c
step
goto 30,70
'_Garr_
.' _Antimagic Pulse_ is an AoE dispell, removing 1 beneficial spell effect from nearby raid members.
.' _Magma Shackles_ is an AoE, reducing the movement speed of nearby raid members by 40% for 15 seconds.
'_Firesworn_
.' _Immolate_ inflicts 800 Fire damage and an additional 400 Fire damage every 3 seconds for 21 seconds.
.' _Eruption_ causes the Firesworn to explode, dealing 1850-2150 Fire damage to nearby players.
|modelnpc Garr##12057
|confirm
step
map Molten Core/1
path loop off
path	37.0,70.6	46.0,70.8
.' Follow the path, clearing trash as you make your way to the next boss, Baron Geddon. |goto 46.0,70.8 <5 |noway |c
step
goto 50.6,75.5
'_Baron Geddon_
|tip He is on a small patrol route, you may have to wait a few seconds for him to appear here.
.' _Inferno_ is a proximity-based AoE, hitting all raid members near Geddon. It lasts 10 seconds, each pulse coming 1 second apart and increasing in damage every pulse.
.' _Ignite Mana_ is a DoT that removes 400 mana and does 400 damage to the target. It lasts 5 minutes but can be dispelled.
.' _Living Bomb_ is a debuff that cannot be dispelled. After 10 seconds the bomb explodes, dealing 3200 Fire damage to the targeted raid member and all nearby allies. The target will also be launched into the air and take fall damage upon landing.
.' At 2% health, Geddon turns himself into a _Living Bomb_. You have 5 seconds to kill him before he explodes and deals a large amount of damage to all raid members in melee range, usually killing them.
|modelnpc Baron Geddon##12056
|confirm
step
goto 52.7,83.5
'_Shazzrah_
.' _Arcane Explosion_ inflicts 925-1075 Arcane damage to nearby raid members.
.' _Shazzrah's Curse_ is a debuff that increases the target's magical damage taken by 100% for 5 minutes.
.' _Magic Grounding_ is a buff that reduces the magical damage taken by Shazzrah by 50% for 30 seconds.
.' _Counterspell_ is an AoE interrupt, countering the spellcasting of nearby raid members and also disabling that school of magic for 10 seconds.
.' _Blink_ teleports Sazzrah 20 yards forward, freeing him from any bonds and wiping threat.
|modelnpc Shazzrah##12264
|confirm
step
map Molten Core/1
path loop off
path	57.6,74.1	66.1,65.8	72.6,73.5
path	76.0,79.5
.' Follow the path, clearing trash as you make your way to the next boss, Sulfuron Harbinger. |goto 76.0,79.5 <5 |noway |c
step
goto 83,83.2
'_Sulfuron Harbinger_
.' _Demoralizing Shout_ reduces all nearby raid members' attack power by 300 for 30 seconds.
.' _Inspire_ is a buff that doubles Sulfuron's attack speed and increases damage by 25%.
.' _Flame Spear_ deals 1000 Fire damage to the target and any nearby raid members.
.' _Hand of Ragnaros_ deals 300-400 Fire damage to nearby players, also knocking them back and stunning them for 2 seconds.
'_Flamewalker Priest_
.' _Shadow Word: Pain_ is a DoT that deals 418 Shadow damage every 3 seconds.
.' _Immolate_ deals 400 Fire damage every 3 seconds.
.' _Dark Mending_ heals the target for 27,750-32,250.
|modelnpc Sulfuron Harbinger##12098
|confirm
step
map Molten Core/1
path loop off
path	74.9,71.5	78.6,56.8	72.9,50.1
.' Follow the path, clearing trash as you make your way to the next boss, Golemagg the Incinerator. |goto 72.9,50.1 <5 |noway |c
step
goto 68.7,59.4
'_Golemagg the Incinerator_
'_Phase 1_
.' _Golemagg's Trust_ buffs Golemagg's Core Ragers, increasing their damage by 500 and attack speed by 50%.
.' _Magma Splash_ is a stacking debuff applied when raid members attack Golemagg. It starts off dealing 50 damage per tick and reducing armor by 250, stacking up to 50 times with 2500 damage per tick and 12,500 armor reduction,
.' _Pyroblast_ deals 2000 damage to a random raid member and inflicting a DoT that deals 200 damage every 3 seconds for 12 seconds.
'_Phase 2_ starts at 10% health.
.' Golemagg will start using _Earthquake_, dealing damage to all raid members near him and increasing his attack speed.
.' _Attract Rager_ is applied to every raid member when Golemagg reaches 10% health. It increases the aggro of all players except the tank.
'_Core Ragers_
.' _Mangle_ reduces the target's movement speed by 50% and deals 300 damage every 2 seconds.
.' These adds cannot die while Golemagg is still alive. They need to be tanked away from Golemagg, but taking them too far away will cause the fight to bug and reset.
|modelnpc Golemagg the Incinerator##11988
|confirm
step
map Molten Core/1
path loop off
path	77.1,52.5	79.8,60.2	78.0,68.7
path	83.2,74.2
.' Follow the path, clearing trash as you make your way to the next boss, Majordomo Executus. |goto 83.2,74.2 <5 |noway |c
step
goto 82.7,65.4
'_Majordomo Executus_
.' The goal of this fight is to _kill all of Majordomo's guards_.
.' _Aegis of Ragnaros_ Majordomo casts this on himself, absorbing 30,000 damage, reflecting 250 damage on melee attacks and healing himself to full, making him impossible to kill.
.' _Shield_ this shielding spell lasts for 10 seconds and is given to all of Majordomo's allies: _Purple Shield:_ up to 100 melee damage is reflected. _White Shield:_ each magic attack has a 50% chance to reflect back at the caster.
.' _Teleport_ instantly transports a random raid member into the nearby fiery pit of Molten Core. The player will take 1000-2000 Fire damage while standing in this. Move out of it as quickly as possible.
.' _Blast Wave_ is an AoE that hits nearby raid members will moderate Fire damage.
'_Flamewalker Elite_
.' _Fire Blast_ deals 900-1200 Fire damage to the target.
.' _Shield_ and _Blast Wave_ that function the same as Majordomo's.
'_Flamewalker Healer_
.' _Shadowbolt_ deals 1000 Shadow damage to a random raid member.
.' _Shield_ that functions the same as Majordomo's.
|modelnpc Majordomo Executus##12018
|confirm
step
goto 83.0,65.0
.' Make sure to click the Cache of the Firelord to receive your loot.
|confirm
step
map Molten Core/1
path loop off
path	78.4,58.7	74.7,71.8	70.5,66.4
path	63.9,67.7	46.9,71.7	36.4,69.9
path	39.1,57.1	53.2,67.1	62.1,61.1
path	58.8,45.8	49.7,52.0	54.6,53.9
.' Follow the path, clearing trash as you make your way to the final boss, Ragnaros. |goto 54.6,53.9 <5 |noway |c
step
goto 54.6,53.9
.talk 54404
.' Tell him "Tell me more.", and click through the dialogue.
.' This will start the final fight, make sure you are ready.
|confirm
step
goto 55.1,54.5
'_Ragnaros_
.' _Wrath of Ragnaros_ occurs every 25 seconds and sends all raid members in melee range flying back. All melee players should back up prior to this being used to avoid being knocked into lava.
.' _Hammer of Ragnaros_ hits a random mana-using raid member, knocking back all players within 20 yards of the target.
.' _Lava Splash_ affects players close to a lava flow.
.' _Elemental Fire_ is a debuff that deals 4800 damage over 8 seconds to Ragnaros' target.
.' _Magma Blast_ occurs if no one is engaging Ragnaros in melee combat. It deals 4000-6000 damage to the whole raid.
.' _Melt Weapon_ deals damage to raid members' weapons. It takes 1 point of weapon durability per attempt. A spare weapon or repair bot is recommended.
.' _Summon Sons of Flame_ occurs after 3 minutes of combat. Ragnaros will summon 8 Sons of Flame and submerge under the lava. After 90 seconds, or once all 8 adds are killed, Ragnaros will reappear.
|modelnpc Ragnaros##11502
|confirm
step
.' Congratulations, you have finished the Molten Core raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Classic\\Blackwing Lair",[[
dungeon 755
description This guide will walk you through the Molten Core Raid.
author support@zygorguides.com
step
.' The first part of this guide will walk you through the attunement quest for this Raid, which takes place in the Blackrock Spire dungeon. Then you will be able to teleport into the Raid with the Orb of Command.
|confirm
step
goto Burning Steppes/14 73.4,52.9
.from Scarshield Quartermaster##9046
.collect Blackhand's Command##18987
|only if not completedq(7761)
step
.' Click the Blackhand's Command in your bags |use Blackhand's Command##18987
.accept 7761
|only if not completedq(7761)
step
map Blackrock Spire/5
path loop off
path	Blackrock Spire/4 34.1,35.6	37.2,33.0	37.1,23.0
.' Follow the path as you make your way through the dungeon |goto 37.1,23.0 <5 |noway |c
|only if not completedq(7761)
step
goto 37.1,23.0
.' Clear this room of all enemies
|confirm
|only if not completedq(7761)
step
map Blackrock Spire/5
path loop off
path	33.5,15.3	30.5,13.1	Blackrock Spire/6 30.4,21.8
.' Follow the path as you make your way through the dungeon. |goto Blackrock Spire/6 30.4,21.8 <5 |noway |c
|only if not completedq(7761)
step
goto 32.7,26.8
.' Click the Blackrock Altar and kill the Blackhand Incarcerators that become attackable.
|confirm
|only if not completedq(7761)
step
goto 30.3,27.1
.' Wait until the Elemental at this spot reaches full power and then it will become attackable. Kill it.
|confirm
|only if not completedq(7761)
step
map Blackrock Spire/7
path loop off
path	Blackrock Spire/6 30.4,31.9	Blackrock Spire/6 30.3,39.4	28.7,41.1
path	28.4,35.9	30.3,32.8	31.0,27.8
path	40.2,28.1
.' Follow the path as you make your way through the dungeon. |goto 40.2,28.1 <5 |noway |c
|only if not completedq(7761)
step
.' Jump down here at enter the pit |goto 46.0,28.0 <5 |c
|only if not completedq(7761)
step
goto 48.4,26.2
.' Fight off the waves of enemies and the boss, Warchief Rend.
|confirm
|only if not completedq(7761)
step
map Blackrock Spire/7
path loop off
path	48.6,32.9	50.3,35.7	54.4,35.9
path	54.7,43.2	50.8,45.7	42.6,45.8
path	39.4,48.2
.' Follow this path to the last part of this dungeon quest. |goto 39.4,48.2 <5 |noway |c
|only if not completedq(7761)
step
goto 33.5,44.6
.' Kill General Drakkisath
|modelnpc General Drakkisath##10363
|confirm
|only if not completedq(7761)
step
goto 33.5,44.6
.' Click Drakkisath's Brand
.turnin 7761
|only if not completedq(7761)
step
goto Burning Steppes/14 64.3,70.9 |n
.' Click the Orb of Command to teleport to Blackwing Lair. |goto Blackwing Lair/1 51.7,81.8 <5 |noway |c
step
goto Blackwing Lair/1 35.7,67.2
'_Razorgore the Untamed_
.' The objective of this fight is to destroy all the eggs in the room by controlling Razorgore the Untamed before killing him.
.' When _controlling_ Razorgore, he has the following abilities:
.' _Destroy Egg_ destroys a nearby egg. 3 second cast.
.' _Calm Dragonkin_ puts the target Dragonkin to sleep for 30 seconds.
.' _Fireball Volley_ inflicts 1000 Fire damage to all enearby enemies. 2 second cast.
.' When _fighting_ Razorgore, he also has these abilities:
.' _Conglagration_ inflicts 3000 Fire damage over 10 seconds to the target, also hitting nearby enemies for 150 damage.
.' _Summon Player_ teleports the targeted player to Razorgore's location.
|modelnpc Razorgore the Untamed##12435
|confirm
step
goto 33,27
'_Vaelastrasz the Corrupt_
.' _Essence of the Red_ this is a raid-wide buff that lasts for 3 minutes after engaging Vaelastrasz in combat. It restores 500 mana, 50 energy, 20 rage per second, or 20 runir power, depending on your class.
.' _Fire Nova_ is an AoE that inflicts 560-650 Fire damage to nearby enemies.
.' _Flame Breath_ inflicts 3060-4000 Fire damage to enemies in a _cone_ in front of Vaelastrasz. Make sure the tank is the only one in front of Vaelastrasz when this occurs.
.' _Burning Adrenaline_ is a buff applied to random raid members throughout the fight and also on the tank every 45 seconds. It increases damage by 100%, attack speed by 100% and spells cast instantly, but it reduces max health by 5% every second, eventually killing the player.
.' _Cleave_ hits for 2000 damage and can _chain_ off of targets that are struck. Make sure the tank is the only one in front of Vaelastrasz when this occurs.
|modelnpc Vaelastrasz the Corrupt##13020
|confirm
step
path loop off
path	Blackwing Lair/1 41.1,28.4	Blackwing Lair/1 44.9,28.3	Blackwing Lair/2 41.6,44.0
path	Blackwing Lair/2 25.0,61.1	Blackwing Lair/2 33.5,62.7	Blackwing Lair/2 49.3,80.3
path	Blackwing Lair/3 53.1,67.9
.' Follow the path, clearing trash and clicking Supression Devices as you make your way to the next boss, Broodlord Lashlayer |goto Blackwing Lair/3 53.1,67.9 <5 |noway |c
step
goto 48,57
'_Broodlord Lashlayer_
.' _Blase Wave_ deals Fire damage to nearby enemies and reducing their movement speed for 8 seconds.
.' _Cleave_ deals 110% weapon damage and hits additional raid members near the target. Make sure the tank is the only one in front of the boss when this occurs.
.' _Knock Away_ deals a small amount of damage and knocks the target back.
.' _Mortal Strike_ deals 500% weapon damage to the target and applies a wounded debuff, reducing healing the target receives by 50% for 5 seconds.
|modelnpc Broodlord Lashlayer##12017
|confirm
step
goto Blackwing Lair/3 46.8,44.2
'_Firemaw_
.|tip He patrols through the next room so you may have to wait a minute for him to come back to this spot.
.' _Flame Buffet_ inflicts 140-150 Fire damage to the target and increases the Fire damage they take by 150 for 20 seconds.
.' _Shadow Flame_ is an AoE that inflicts 4000-5000 Shadow damage to raid members in a _cone_ in front of Firemaw. Make sure the tank is the only one in front of him when this occurs.
.' _Summon Player_ teleports a random raid member to Firemaw's location.
.' _Wing Buffet_ is an AoE that inflicts 550-950 damage to raid members in a _cone_ in front of Firemaw. Make sure the tank is the only one in front of him when this occurs.
|modelnpc Firemaw##11983
|confirm
step
path loop off
path	Blackwing Lair/3 41.7,35.4	Blackwing Lair/3 32.0,39.1	Blackwing Lair/4 22.1,56.9
path	Blackwing Lair/4 28.3,48.4
.' Follow the path, clearing trash as you make your way to the next boss, Ebonroc. |goto Blackwing Lair/4 28.3,48.4 <5 |noway |c
step
goto 35,37
'_Ebonroc_
.' _Flame Buffet_ inflicts 140-150 Fire damage to the target and increases the Fire damage they take by 150 for 20 seconds.
.' _Shadow Flame_ is an AoE that inflicts 4000-5000 Shadow damage to raid members in a _cone_ in front of Firemaw. Make sure the tank is the only one in front of him when this occurs.
.' _Shadow of Ebonroc_ is a 8 second debuff that is cast on the main tank. It restores Ebonroc's health by 25,000 every time he hits the target.
|modelnpc Ebonroc##14601
|confirm
step
goto 37.5,42.7
'_Flamegor_
.' _Flame Buffet_ inflicts 140-150 Fire damage to the target and increases the Fire damage they take by 150 for 20 seconds.
.' _Shadow Flame_ is an AoE that inflicts 4000-5000 Shadow damage to raid members in a _cone_ in front of Firemaw. Make sure the tank is the only one in front of him when this occurs.
.' _Fire Nova_ deals 550 Fire damage to all raid members. This occurs every 5 seconds but only when enraged.
|modelnpc Flamegor##11981
|confirm
step
goto 49.9,70.5
.' Click this Lever to start the next boss fight with Chromaggus.
|confirm
step
goto 43.0,69.0
'_Chromaggus_
.' Chromaggus uses _breath_ attacks that hits the entire raid, within line of sight. There are 5 possible types.
.' _Incinerate: Red_ deals 3600-4300 Fire damage.
.' _Corrosive Acid: Green_ deals 875-1125 Nature damage every 3 seconds for 15 seconds. Also reduces armor by 4000-5000.
.' _Frost Burn: Blue_ reduces attack speed by 80% and deals 1000-1500 Frost damage.
.' _Ignight Flesh: Black_ deals 650-850 Fire damage every 3 seconds for 60 seconds. This can stack if used again before the original 60 seconds are up.
.' _Time Lapse: Bronze_ is a 6 second stun and reduces maximum health to half. Will also heal when the stun wears off.
.' Chromaggus also used _Brood Afflictions_ which debuff everyone in the zone.
.' _Red:_ deals 50 Fire damage every 3 seconds. Will heal Chromaggus if an affected player dies.
.' _Green:_ reduces healing by 50% and deals 50 Nature damage every 5 seconds.
.' _Blue:_ slows movement speed by 70% and cast speed by 50%. Also drains 50 mana and 1 health every second.
.' _Black:_ increases Fire damage taken by 100%.
.' _Bronze:_ is a 4 second stun that occurs randomly for 10 minutes. Can be removed with Hourglass Sand. |use Hourglass Sand##19183
|modelnpc Chromaggus##14020
|confirm
step
map Blackwing Lair/4
path loop off
path	54.5,57.6	65.4,69.3	75.4,88.0
.' Follow this path to the final boss, Nefarian. |goto 75.4,88.0 <5 |noway |c
step
goto 75.4,88.0
.talk 10162
.' Click through the dialogue, ending with the final statement, "Please do."
.' This will start this boss fight, make sure you are ready.
|confirm
step
goto 72.6,81.7
'_Nefarian_
'_Phase 1_
.' As soon as the fight starts he will be invulnerable and will randomly Shadow Bolt and Fear random raid members. The room also begins to fill with Drakonids.
.' The objective of Phase 1 is to kill the Drakonids.
'_Red Drakonid_ have a short-range conal DoT attack. This DoT is stackable. Make sure the tank is the only one in front of them when this occurs.
'_Blue Drakonid_ attacks drain mana and lowers attack speed.
'_Green Drakonid_ stun raid members.
'_Black Drakonid_ have a direct-damage Fire attack.
'_Bronze Drakonid_ reduce attack and cast speed.
.' _Phase 2_ starts after 42 Drakonids have been slain. Negarian then turns into his dragon form.
.' Click here to proceed to _Phase 2_ |confirm
step
goto 70.6,73.3
'_Nefarian_
'_Phase 2_
.' _Shadow Flame_ is a raid-wide AoE that Nefarian casts before landing at the start of the phase. It deals 1000 Shadow damage to every raid member.
.' _Veil of Shadow_ is a debuff that reduces healing effects bt 75% for 6 seconds.
.' _Tail Lash_ hits all raid members behind Nefarian, dealing 1000-1400 damage and stunning them for 2 seconds.
.' _Bellowing Roar is a large AoE Fear, scattering all raid members within 35 yards of Nefarian for 4 seconds.
.' _Cleave_ hits the target and any nearby players. Make sure the tank is the only person in front of Nefarian when this occurs.
.' Every 30 seconds, Nefarian will "call out" a specific class. This class is affected by the following:
.' _Death Knights_ cast Death Grip on the entire raid, pulling every to Nefarian.
.' _Druids_ are stuck in Cat Form.
.' _Hunters_equipped weapon is instantly broken. Bring backup weapons to use in case this happens.
.' _Mages_ cast Wild Polymorph on random raid members, polymorphing the target.
.' _Monks_ roll around uncontrollable for 5 seconds.
.' _Priests_ direct heals with apply a stackable DoT with Corrupted Healing instead of restoring health.
.' _Paladins_ cast Blessing of Protection on Nefarian, protecting him from all physical attacks for the duration.
.' _Rogues_ are teleported and immobilized in an area near Nefarian.
.' _Shamans_ give Nefarian buffed totems.
.' _Warriors_ are stuck in Berserker Stance and take an additional 30% damage.
.' _Warlocks_ 2 Infernals per Warlock are summoned to fight for Nefarian. They stun and do minor damage to the raid.
.' _Phase 3_ starts at 20% health.
|modelnpc Nefarian##11583
.' Click here to proceed to _Phase 3_ |confirm
step
goto 70.6,73.3
'_Nefarian_
'_Phase 3_
.' Nefarian will revive all dead Drakonids as Bone Constructs. AoE them down.
.' After all are dead Nefarian continues to cast class debuff callouts until he dies.
|modelnpc Nefarian##11583
|confirm
step
.' You may have looted a quest item from Nefarian.
.' Click the Head of Nefarian in your bags. |use Head of Nefarian##19003
.accept 7781
|only if not completedq(7781)
step
goto Stormwind City 85.6,31.8
.talk 29611
..turnin 7781
|only if not completedq(7781)
step
goto Stormwind City 85.6,31.8
.talk 29611
..accept 7782
|only if completedq(7781)
step
goto Stormwind City 71.4,80.5
.talk 14721
..turnin 7782
|only if completedq(7781)
step
.' Congratulations, you have finished the Blackwing Lair raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\Karazhan",[[
dungeon 799
description This guide will walk you through the Karazhan Raid.
author support@zygorguides.com
step
map Karazhan/1
path loop off
path	50.0,72.3	49.1,84.9	42.3,86.2
.' Follow the path, clearing trash as you make your way to the first boss, Attumen the Huntsman. |goto 42.3,86.2 <5 |noway |c
step
goto 45.5,82.6
'_Attumen the Huntsman_
.' He spawns here after Midnight reaches 95% health. They both are immune to taunts.
.' _Shadow Cleave_ hits for 4,000 Shadow damage. It is a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _Intangible Presence_ is cast every 30 seconds, it is a reflectable AoE that reduces change to hit with melee, ranged attacks, and spells by 50%. The area it affects is centered on Attumen's target.
.' _Mount Up_ occurs when Attument or Midnight are at 25% health. He will mount up, turning into a single boss with a higher health percentage.
.' _Berserker Charge_ hits a random target, dealing 200 damage and applying a knockdown with a brief stun.
|modelnpc Attumen the Huntsman##16152
|confirm
step
map Karazhan/1
path loop off
path	44.8,88.8	49.2,75.6	53.0,64.1
path	Karazhan/3 54.5,65.9	Karazhan/3 45.2,54.5	Karazhan/3 36.8,65.4
.' Follow the path, clearing trash as you make your way to the next boss, Moroes. |goto Karazhan/3 36.8,65.4 <5 |noway |c
step
goto 28.2,64.2
'_Moroes_
.' This boss is immune to taunts.
.' _Vanish_ disappears briefly. This does not affect aggro.
.' _Garrote_ is a Physical DoT applied to a random raid member after a _Vanish_. Deals 1000 damage every 3 seconds for 5 minutes.
.' _Blind_ is a disorient effect hits Moroes' closest non-tank target.
.' _Gouge_ hits Moroes' current target, stunning them. This effect breaks on damage. While they are stunned Moroes will switch to the raid member with the second highest threat.
.' _Enrage_ occurs at 30% health. This encreases Moroes' damage. Kill him as quickly as possible to avoid death.
|modelnpc Moroes##15687
|confirm
step
path loop off
path	Karazhan/3 46.4,55	66.6,49.8	71.2,42.8
path	Karazhan/4 63.4,42.1	Karazhan/4 66.2,33.1	Karazhan/4 76.6,41.5
.' Follow the path, clearing trash as you make your way to the next boss, the Maiden of Virtue. |goto Karazhan/4 76.6,41.5 <5 |noway |c
step
goto 82.5,48.5
'_Maiden of Virtue_
.' This boss is immune to taunts.
.' _Holy Ground_ is a permanent AoE that deals 300 Holy damage and 1 second of silence every 3 seconds to all raid members within 12 yards. This effect lasts the entire fight.
.' _Holy Fire_ deals 3500 Fire damage and an additional 1750 Fire damage every 2 seconds for 12 seconds. This can be dispelled.
.' _Holy Wrath_ is a chain AoE, dealing Holy damage and jumping to nearby raid members. Each jump increases the damage dealt by 40%. Counter this by staying spread out.
.' _Repentance_ deals 2000 Holy damage and incapacitates the entire raid for 12 seconds, but broken by any damage taken. This does not hit the Maiden's target.
.' _Berserk_ occurs after 10 minutes of combat, increasing her damage by 500%. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
|modelnpc Maiden of Virtue##16457
|confirm
step
map Karazhan/4
path loop off
path	77.1,42.1	65.9,32.5	56.2,37.1
path	48.6,39.7	41.1,38.5	28.0,23.1
path	24.2,24.5	19.9,40.3	11.0,41.3
path	12.3,25.3
.' Follow the path, clearing trash as you make your way to the next boss, the Opera Event. |goto 12.3,25.3 <5 |noway |c
step
goto 20.8,27.0
.talk 16812
.' Tell him "I'm not an actor." and "Ok, I'll give it a try."
.' This will start the Opera boss event, make sure you are ready.
|confirm
step
goto 21.0,35.3
.' You'll get one of three events:
.' The Big Bad Wolf |next "wolf" |confirm
'_
.' Wizard of Oz |next "oz" |confirm
'_
.' Romulo & Julianne |next "raj" |confirm
step
label "wolf"
goto 17.0,34.7
.talk 17603
.' Tell her "Oh, grandmother, what big ears you have."
.' This will start the boss fight, make sure you are ready.
|confirm
step
goto 18.4,34.9
'_The Big Bad Wolf_
.' _Terrifying Howl_ is cast every 25-35 seconds, fearing nearby enemies for 3 seconds.
.' _Little Red Riding Hood_ changes the target into Little Red Riding Hood, reducing their armor and resistances to 0, and increasing speed by 50%. Also pacifies and silences the target. This raid member will need to run away until this effect ends.
.' _Wide Swipe_ is cast every 25-35 seconds and stuns the target for 4 seconds.
|modelnpc The Big Bad Wolf##17521
|next "operaend" |confirm
step
label "oz"
goto 18.4,34.9
'_Wizard of Oz_
.' _Kill order:_ Dorothee, Roar, Strawman, Tinhead. Killing these spawn The Crone. Killing him ends the event.
'_Abilities:_
.'_Dorothee:_ _Water bolt_ hits for 2000 Frost damage. _Frightened Scream_ AoE, fearing 3 raid members for 2 seconds.
.' _Roar_: _Frightened Scream_ AoE, fearing 3 raid members for 2 seconds.
.' _Strawman:_ _Burning Straw_ chance to be disoriented for 6 seconds when hit by a Fire spell. _Brain Bash_ stuns target for 4 seconds.
.' _Tinhead:_ _Cleave_ make sure that the main tank is the only one that is in front of the boss when this occurs. _Rust_ slows Tinhead's speed by 10%, stacking up to 8 times.
.' _The Crone:_ _Cyclone_ knocks the targeted player up into the air, dealing fall damage. _Chain Lightning_ deals 2800 damage, jumping to nearby targets and dealing increased damage after every jump. Hits up to 5 players.
|modelnpc The Crone##18168
|next "operaend" |confirm
step
label "raj"
goto 18.4,34.9
'_Romulo & Julianne_
'_Romulo_
.' _Backward Lunge_ strikes an enemmy behind the Romulo, dealing weapon damage plus 300 and knocking the target back.
.' _Deadly Swathe_ strikes nearby enemies in front of Romulo, dealing weapon damage plus 300 and hitting up to 3 targets.
.' _Poisoned Thrust_ is a stacking debuff that reduces all stats by 10%, stacking up to 8 times.
.' _Daring_ inceases Romulo's attack speed and Physical damage dealt by 50% for 8 seconds.
'_Julianne_
.' _Eternal Affection_ heals one of Julianne's allies for about 50,000.
.' _Powerful Attraction_ stuns Julianne's target for 6 seconds.
.' _Blinding Passion_ deals 4500 Holy damage over 4 seconds.
.' _Devotion_ increases Julianne's spell casting speed and damage by 50% for 10 seconds.
|modelnpc Romulo##17533
|modelnpc Julianne##17534
|next "operaend" |confirm
step
label "operaend"
map Karazhan
path loop off
path	Karazhan/4 20.0,41.2	Karazhan/5 47.0,83.9	Karazhan/5 78.1,58.4
path	Karazhan/5 65.0,25.7	Karazhan/6 40.9,16.2	Karazhan/6 41.8,34.5
path	Karazhan/6 48.6,46.5	Karazhan/6 52.8,53.9	Karazhan/6 58.1,62.1
path	Karazhan/6 55.9,73.9	Karazhan/6 62.5,75.4	Karazhan/6 65.3,68.8
path	Karazhan/7 57.3,27.0	Karazhan/7 47.8,62.5	Karazhan/7 53.9,54.9
path	Karazhan/7 51.0,67.9	Karazhan/8 60.5,51.9	Karazhan/8 44.2,39.9
path	Karazhan/8 29.8,57.7	Karazhan/8 45.7,74.5	Karazhan/9 61.1,21.3
path	Karazhan/9 59.9,17.6	Karazhan/9 56.0,14.5	Karazhan/9 45.6,14.1
path	Karazhan/9 40.5,19.9
.' Follow the path, clearing trash as you make your way to the next boss, The Curator. |goto Karazhan/9 40.5,19.9 <5 |noway |c
step
goto 46.8,31.9
'_The Curator_
.' _Summon Astral Flare_ spawns an Astral Flare every 10 seconds (10 Flares total). The Flare spawns with threat from a non-tank raid member and immediately moves in their direction, hitting nearby raid members with Arcing Sear. Killing these as quickly as possible is essential, as you don't want more than one of them dealing damage at the same time.
.' _Hateful Bolt_ hits a non-tank raid member, dealing about 5000 Arcane damage.
.' _Evocation_ lasts for 20 seconds and occurs after The Curator has summoned all 10 Astral Flares. He becomes inactive for 20 seconds while recovering mana and increasing damage taken by 200%. Deal as much damage as possible.
.' _Soft Enrage_ occurs when The Curator reaches 15% health, increasing melee attack speed and Hateful Bolt cast rate. At this point he no longer uses Evocation or summons Astral Flares. Kill him as fast as possible to avoid death.
.' _Enrage_ occurs after 10 minutes of combat. It will greatly increase his attack speed and damage. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc The Curator##15691
|confirm
step
map Karazhan/9
path loop off
path	52.2,42.8	49.7,51.4	49.9,59.3
path	40.9,59.2	33.8,62.2	41.9,67.4
path	44.4,79.7	40.0,83.0	32.1,68.2
path	Karazhan/10 28.7,55.9	Karazhan/10 27.7,43.5	Karazhan/10 36.5,33.3
path	Karazhan/10 36.8,22.3
.' Follow the path, clearing trash as you make your way to the next boss, Terestian Illhoof. |goto Karazhan/10 36.8,22.3 <5 |noway |c
step
.' Click the Perplexing Bookshelf to gain access to the secret room.
|confirm
step
.' Follow the path down |goto Karazhan/11 44.0,51.0 <5 |c
step
goto Karazhan/11 53,69
'_Terestian Illhoof_
.' _Shadow Bolt_ deals 4000 Shadow damage to the raid member with the highest threat.
.' _Sacrifice_ deals 1500 damage per second to a random raid member, also summoning them to the center of the room and stunning them until the Chains are killed. While the Chains are alive, Illhoof is also healed for 3000 health per second. When this occurs, killing these are top priority.
.' _Berserk_ occurs after 10 minutes of combat. Illhoof spams high-damage Shadow Bolt Volleys on the entire raid group every few seconds.
|modelnpc Terestian Illhoof##15688
|confirm
step
map Karazhan/10
path loop off
path	38.3,25.6	48.3,45.2	58.1,42.4
path	63.6,35.8
.' Follow the path, clearing trash as you make your way to the next boss, the Shade of Aran |goto 63.6,35.8 <5 |noway |c
step
.' Click the Private Library Door
|confirm
step
goto 71.1,26.6
'_Shade of Aran_
.' _Summon Water Elementals_ Aran spawns 4 Water Elementals to protect him.
.' _Frostbolt_ deals 4000 Frost damage and reduces the movement speed of the target for 4 second.
.' _Fireball_ deals 4000-5000 Fire damage and is interruptable.
.' _Arcane Missles_ is a 5 second channelled spell that shoots 5 missles, each dealing about 1350 Arcane damage. This spell is interruptable.
.' _Chains of Ice_ is a 10 second root.
.' _AoE Counterspell_ is a 10 yard AoE interrupt and locks out schools of magic for 10 seconds.
.' _Flame Wreath_ is used randomly every 30 seconds. It creates a fiery aura around 3 random raid members that last for 20 seconds. When stepped on, the Auras are triggered, dealing 3000-4000 Fire damage to everyone in the room. They can still be triggered after Aran's death.
.' _Circular Blizzard_ is used randomly every 30 seconds. It is a large AoE that slowly moves clockwise around the room, dealing 1500 Frost damage every 2 seconds and slowing movement speed by 65%.
.' _Magnetic Pull/Slow/Super Arcane Explosion_ pulls everyone to the center of the room and slows them. It is then followed by a 10 second cast Arcane Explosion that deals 10,000 damage in a 20 yard radius.
|modelnpc Shade of Aran##16524
|confirm
step
map Karazhan/12
path loop off
path	Karazhan/10 55.2,46.8	 Karazhan/10 60.2,58.5	Karazhan/12 48.4,57.2
path	54.6,49.6	52.8,70.9	44.5,80.8
path	36.3,78.0	25.3,60.7	Karazhan/13 53.8,78.1
path	Karazhan/13 44.1,57.1
.' Follow the path, clearing trash as you make your way to the next boss, Netherspite |goto Karazhan/13 44.1,57.1 <5 |noway |c
step
goto 36.6,43.6
'_Netherspite_
.' _Nether Burn_ deals 1200 Shadow damage every 5 seconds. Only active during Portal Phase.
.' _Void Zone_ opens a large portal that lasts 25 seconds, inflicting 1000 Shadow damage every 2 seconds to all raid members in that area.
.' _Netherbreath_ is a conal ability that hits all targets in front of Netherspite, dealing 4500 Arcane damage and knocking them back about 20 yards.
.' _Empowerment_ occurs during Portal Phases a few seconds after beams come up. It is a self-buff and increases Netherspite's damage by 200%.
.' _Enrage_ occurs after 9 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Portal Phase_
.' The first phase of the fight, lasting 1 minute and alternating with the Banish Phase.
.' A _red_, _green_, and _blue_ portal open up and shoot out beams of light at Netherspite, giving him a stacking buff. Physically blocking these beams with a raid member is essential, cutting off his buffs and giving them to that player. Along with the buffs come a negative effect, so rotating raid members is normal.
.' _Red_ is _tank_ beam.
.' _Green_ is _healer_ beam.
.' _Blue_ is _DPS_ beam.
'_Banish Phase_
.' The second phase of the fight, lasting 30 seconds and alternating with the Portal Phase.
.' Netherspite becomes banished, remaining stationary but becoming vulnerable to all damage.
.' After a few seconds Netherspite targets random raid members, casting Netherbreath.
.' All aggro is wiped at the end of this phase.
|modelnpc Netherspite##15689
|confirm
step
map Karazhan/12
path loop off
path	Karazhan/13 49.5,67.2	Karazhan/13 56.4,83.0	27.1,61.5
path	27.0,57.6	21.3,47.6	23.8,41.6
path	30.1,33.9	34.4,26.7	39.9,18.6
.' Follow the path, clearing trash as you make your way to the next boss, the Chess event. |goto 39.9,18.6 <5 |noway |c
step
.' Click the Gamesman's Hall Door to enter the Gamesman's Hall
|confirm
step
goto Karazhan/14 40.9,68.9
.talk 21684
.' "Control King Llane"
.' This will start the boss fight, make sure you are ready.
|confirm
step
goto Karazhan/14 39.4,67.6
.' The goal of this fight is to kill _Warchief Blackhand_ by using your "chess" pieces, without letting them kill _King Llane_. Each piece has their own abilities.
'_King Llane_
.' _Sweep_ deals 4000 damage to 3 frontal adjacent enemies.
.' _Heroism_ grants 50% increased damage to all allies on 8 adjacent squares.
'_Human Conjurer_
.' _Elemental Blast_ deals 4000 damage to any single enemy piece.
.' _Rain of Fire_ deals 6000 damage to any enemy chess piece and all adjacent enemy pieces.
'_Human Cleric_
.' _Healing_ heals a friendly piece for 12,000.
.' _Holy Lance_ deals 2000 damage to enemy pieces standing in front in a straight line of 3.
'_Human Charger_
.' _Smash_ deals 3000 damage to a target 1 square in front.
.' _Stomp_ reduces damage dealt by all adjacent enemy pieces by 50%. Lasts 10 seconds.
'_Conjured Water Elemental_
.' _Geyser_ deals 3000 damage to all adjacent enemy pieces.
.' _Water Shield_ reduces damage taken by 50% for 5 seconds.
'_Human Footman_
.' _Heroic Blow_ deals 1000 damage to a target 1 square in front.
.' _Shield Block_ absorbs 500 damage and lasts 5 seconds.
'_Medivh cheats:_
.' _Healing Cheat_ heals Warchief Blackhand and all other enemy pieces to full health.
.' _Damage Cheat_ places a fire AoE on a random player controlled piece.
.' _Berserking Cheat_ grants one of Medivh's pieces increased size, speed, and damage.
|modelnpc Warchief Blackhand##21752
|confirm
step
goto 25.9,74.5
.' Loot the Dust Covered Chest
|confirm
step
map Karazhan/14
path loop off
path	52.0,61.6	56.0,56.8	50.1,46.2
path	50.1,46.2	68.3,46.2	77.2,58.0
.' Follow the path, clearing trash as you go. Climb these stairs all the way to the top. |goto Karazhan/17 48.2,85.7 <5 |noway |c
step
map Karazhan/17
path loop off
path	40.9,81.6	39.7,72.3	44.7,64.1
.' Follow this path to the final boss, Prince Malchezaar. |goto 44.7,64.1 <5 |noway |c
step
goto 53.9,42.0
'_Prince Malchezaar_
'_Phase 1_ starts immediately after Malchezaar is engaged in combat.
'_Phase 2_ starts at 60% health. Malchezaar will now use _Flying Axes_ in this phase.
'_Phase 3_ starts at 30% health. _Flying Axes_ and _Amplify Damage_ will be used in this final phase.
.' _Summon Infernal_ spawns a Netherspite Infernal that casts a large AoE _Hellfire_ on the raid. These cannot be attacked and must be avoided.
.' _Enfeeble_ is cast on 5 random raid members, reducing their maximum health to 1 hit point for 7 seconds.
.' _Shadow Nova_ is usually cast after Enfeeble. It has a range of 24 yards, deals about 3000 Shadow damage and has a knockback. This can easily be avoided by staying at range of Malchezaar.
.' _Shadow Word:Pain_ is a DoT cast on Malchezaar's main target or a random target, dealing 1500 damage per tick. This can be dispelled.
.' _Thrash_ makes Malchezaar's melee attack hit his target 2 additional times.
.' _Flying Axes_ hit random targets
.' _Amplify Damage_ is a debuff that doubles the target's damage taken from all sources. It is cast on a random target and is not dispellable.
|modelnpc Prince Malchezaar##15690
|confirm
step
.' If you or another raid member has completed the Medivh's Journal questline and has access to the Blackened Urn, you can summon and fight the bonus boss, Nightbane.
.' Click here to fight Nightbane! |next "nightbane" |confirm
'_
.' Click here to skip Nightbane! |next "raidend" |confirm
step
label "nightbane"
step
path loop off
path	Karazhan/17 45.2,63.2	Karazhan/17 40.5,77.1	Karazhan/17 49.2,87.1
path	Karazhan/16 66.6,62.0	Karazhan/15 79.5,72.5	Karazhan/14 78.0,58.5
path	Karazhan/14 66.2,42.9	Karazhan/14 52.9,42.0	Karazhan/14 32.0,83.8
path	Karazhan/14 23.9,80.1	Karazhan/12 36.8,22.2	Karazhan/12 28.4,35.9
path	Karazhan/12 21.6,46.6	Karazhan/12 29.0,62.8	Karazhan/12 39.7,82.2
path	Karazhan/12 44.3,80.8	Karazhan/12 52.0,71.6	Karazhan/12 55.9,52.3
path	Karazhan/12 47.9,57.1	Karazhan/10 49.7,47.0	Karazhan/10 40.2,32.7
path	Karazhan/10 28.4,43.9	Karazhan/10 29.1,56.4	Karazhan/9 40.1,83.0
path	Karazhan/9 43.5,66.7	Karazhan/9 49.6,58.9	Karazhan/9 48.5,47.7
path	Karazhan/9 51.3,40.3	Karazhan/9 42.6,19.1	Karazhan/9 49.1,14.7
path	Karazhan/9 54.3,14.8
.' Follow the long path back to Nightbane. |goto Karazhan/9 54.3,14.8 <5 |noway |c
step
.' Jump off here |goto Karazhan/9 60.6,20.0 <5 |c
step
.' Jump off here |goto Karazhan/7 49.5,35.3 <5 |c
step
map Karazhan/6
path loop off
path	59.8,64.6	61.8,70.6	55.8,73.3
path	52.4,71.6	46.8,77.0	43.0,83.5
.' Follow the path to Nightbane. |goto Karazhan/6 43.0,83.5 <5 |noway |c
step
goto Karazhan/6 43.0,83.6
.' Click the Urn to summon Nightbane. This will start the boss fight, make sure you are ready.
|confirm
step
goto 44.6,83.5
'_Nightbane_
.' Nightbane is immune to taunts.
'_Phase 1_ is the ground phase. Nightbane will alternate between this phase and Phase 2 during the fight.
'_Phase 2_ is flight phase. These phases occur at 75%, 50%, and 25% health.
'_Ground Phases_
.' _Bellowing Roar_ is an AoE fear, cast every 45-60 seconds. Healers and DPS at range should be able to avoid it.
.' _Charrred Earth_ is an AoE cast on a random raid member. Does a DoT that deals up to 3000 FIre damage per second. It disappears once the player leaves the affected area.
.' _Distracting Ash_ debuff that reduces Nightbane's target's chance to hit with melee and spells by 30% for 40 seconds. This can be dispelled.
.' _Smoldering Breath_ inflicts 5000-6000 Fire damage to raid members in a _frontal cone_.
.' _Tail Sweep_ deals 450 Fire damage in a _cone behind_ Nightbane. Also knocks them back and deals 450 damage every 3 seconds for 25 seconds.
.' He also has a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
'_Flight Phases_
.' _Rain of Bones_ is cast on a random raid member, dealing 400 AoE damage and summoning Restless Skeleton adds.
.' _Smoking Blast_ deals 2000 damage every second for 15 seconds to the target with highest threat and an additional 3000 Fire damage over 18 seconds. This DoT can be dispelled.
.' _Fireball Barrage_ is cast if any member of the raid is too far away from Nightbane while in the Flight phase. Each fireball deals 3500 damage, casting a fireball each second until all raid members are within range again.
|modelnpc Nightbane##17225
|confirm
step
label "raidend"
.' Congratulations, you have finished the Karazhan raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\Sunwell Plateau",[[
dungeon 789
description This guide will walk you through the Sunwell Plateau Raid.
author support@zygorguides.com
step
map Sunwell Plateau/1
path loop off
path	33.9,43.3	36.6,51.5	33.7,58.3
path	25.1,59.8	17.6,55.5	16.9,45.7
path	23.6,39.7	31.5,33.8	40.0,35.3
path	40.5,65.3	21.3,66.5	15.6,66.0
path	12.2,54.0	21.3,50.8
.' Follow the path, clearing trash as you make your way to the first boss, Kalecgos. |goto 21.3,50.8 <5 |noway |c
step
label "dragon"
goto 29,50
'_Kalecgos_
.' This fight takes place in two realms at the same time. Both fights need to be done and coordinated at the same time.
'_Normal Realm_
.' Players must face and subdue Kalecgos the dragon.
.' _Arcane Buffet_ deals 500 Arcane damage plus a stacking debuff that increases Arcane damage taken by 500 for 40 seconds. Hits all players in the normal realm.
.' _Frost Breath_ is a 30 yard frontal AoE spell that slows attack speed by 75% to all raid members affected.
.' _Tail Lash_ deals 1500 damage and a 2 second stun to raid members behind Kalecgos.
.' _Spectral Blast_ is cast about every 20 seconds and Kalecgos targets a random raid member. After 2 seconds they take 5000 Arcane AoE damage. They are also teleported to the spectral realm and for about 10 seconds a portal is opened that other raid members can use to go there too.
.' _Wild Magic_ gives a random target a random buff/debuff. _Green:_ increases healing by 100%. _Purple:_ increases cast time by 100%. _Red:_ reduces chance to hit with meele and ranged attacks by 50%. _White:_ increases damage done by critical hits by 100%. _Black:_ increases threat generation by 100%. _Blue:_ reduces spell and ability costs by 50%.
.' _Enrage_ occurs when either Kalecgos or Sathrovarr reach 10% health, both enrage, greatly increasing their damage. Kill them both as fast as possible to avoid death.
.' Click here to see mechanics for the spectral realm and Sathrovarr the Corruptor |next "kalec" |confirm
'_
.' Click here when the fight is over |next "kalecgosend" |confirm
|modelnpc Kalecgos##24850
step
label "kalec"
goto 29,50
'_Sathrovarr the Corruptor & Kalec_
.' This fight takes place in two realms at the same time. Both fights need to be done and coordinated at the same time.
'_Spectral Realm_
.' _Spectral Realm_ is a debuff dealt to players upon entering the realm. It lasts 60 seconds and they are teleported back to the normal realm once it expires.
'_Sathrovarr_
.' _Corrupting Strike_ deals 9000 Shadow damage, a stun, and an additional 1000 damage over 3 seconds.
.' _Curse of Boundless Agony_ is a 30 second Shadow DoT that deals increasing damage every tick, starting at 100 damage and doubling every 5 seconds. If this kills the target or if it is removed it jumps to the next target.
.' _Shadow Bolt Volley_ deals 5000 Shadow damage to a random raid member and up to two nearby members.
.' _Enrage_ occurs when either Kalecgos or Sathrovarr reach 10% health, both enrage, greatly increasing their damage. Kill them both as fast as possible to avoid death.
'_Kalec_
.' _Revitalize_ is a random buff dealt to nearby raid members, restoring 450 health and mana every 3 seconds for 10 seconds and stacking up to two times.
.' Click here to see mechanics for the normal realm and Kalecgos |next "dragon" |confirm
'_
.' Click here when the fight is over |next "kalecgosend" |confirm
|modelnpc Sathrovarr the Corruptor##24892
step
label "kalecgosend"
map Sunwell Plateau/1
path loop off
path	50.4,51.5	51.9,60.3	59.5,71.1
path	62.4,73.7
' Follow the path, clearing trash as you make your way to the next boss, Brutallus. |goto 62.4,73.7 <5 |noway |c
step
goto 65,84
'_Brutallus_
.' _Meteor Slash_ deals 20,000 Fire damage split between enemy targets in a cone in front of Brutallus. Also increases Fire damage taken by 75%. Stacks up to 100 and lasts 40 seconds.
.' _Burn_ is a 60 second debuff and spreads to nearby raid members. The debuff deals Fire damage every second and slowly increases.
.' _Stomp_ is an AoE that deals 20,000 damage and reduces armor of those hit by 50% for 10 seconds. Removes Burn if target has it.
.' _Berserk_ occurs after 6 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Brutallus##24882
|confirm
step
goto 67.0,90.0
'_Felmyst_
.' _Cleave_ make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _Corrosion_ deals 10,000 Nature damage and increases physical damage taken by 100% on the tank. Lasts 10 seconds.
.' _Gas Nova_ is an AoE spell that deals 2000 Nature damage and drains 3000 health and 1000 mana every 2 seconds for 30 seconds.
.' _Noxious Fumes_ deals 1000 Nature damage every 3 seconds to all raid members within 100 yards of Felmyst. Not resistable.
.' _Encapsulate_ lifts a random raid member into the air and does 5 ticks, dealing 3500 Arcane damage to them and everyone within 20 yards.
.' _Demonic Vapor_ is a breath ability used when Felmyst is in the air. It deals 4000 Nature damage and summons skeleton adds. Stepping in the trail left by this also deals 2000 Nature damage per second for 10 seconds and summons additional skeleton adds.
.' _Fog Corruption_ is an AoE spell that mind controls raid members inside of it fr the duration of the battle. Players affected by this should be killed off.
.' _Berserk_ occurs after 10 minutes of combat. It increases her damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
|modelnpc Felmyst##25038
|confirm
step
map Sunwell Plateau/1
path loop off
path	76.8,69.8	69.4,74.6	76.6,57.9
path	67.5,68.2	73.5,54.8	74.9,50.2
path	78.1,38.0	68.6,35.0
.' Follow the path, clearing trash as you make your way to the next boss, the Eredar Twins. |goto 68.6,35.0 <5 |noway |c
step
goto 64,32
'_Eredar Twins_
'_Lady Sacrolash_
.' _Confounding Blow_ confounds the target for 6 seconds, dealing 7500-10,000 Shadow damage. Temporarily drops target's aggro.
.' _Dark Touched_ is a stacking debuff that reduces healing effects by 5%, stacking up to 20 times. Lasts 3 minutes and can only be removed by being hit with Fire damage from either boss.
.' _Shadow Blades_ deals 2500-3500 physical damage and also applies _Dark Strike_, which deals 450 Shadow damage every 2 seconds and slows the target by 10% for 10 seconds, stacking up to 5 times.
.' _Shadow Nova_ deals 3500 Shadow damage to every raid member in a 10 yard radius of Sacrolash's target.
.' _Shadow Image_ Lady Sacrolash spawns multiple Shadow Image adds which die after a short amount of time. These adds cannot be targeted and can deal melee or AoE damage.
.' _Berserk_ occurs after 6 minutes of combat. It increases her damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
'_Grand Warlock Alythess_
.' _Pyrogenics_ is a buff that increases Alythess' Fire damage by 35%.
.' _Flame Touched_ is a stackable debuff that deals 300 Fire damage every 2 seconds. Stacks up to 20 times and can only be removed by being hit with Shadow damage.
.' _Conflagration_ is a debuff that deals 1600 Fire damage to everyone within 8 yards of Alythess' target. It also confounds the target and deals 1600 Fire damage every second in the targeted area.
.' _Blaze_ deals 5500-7500 Fire damage and leaves an AoE on the ground which burns for 2500 Fire damage.
.' _Flame Sear_ is a debuff that affects 3-5 random raid members and deals 650 Fire damage every half-second for 6 seconds.
.' _Berserk_ occurs after 6 minutes of combat. It increases her damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
|modelnpc Lady Sacrolash##25165
|modelnpc Grand Warlock Alythess##25166
|confirm
step
map Sunwell Plateau/1
path loop off
path	58.3,29.5	66.9,22.6	71.6,27.2
path	79.9,20.8	72.8,16.8	67.3,26.7
.' Follow the path, clearing trash as you make your way to the next boss, M'uru. |goto 67.3,26.7 <5 |noway |c
step
goto Sunwell Plateau/2 46,24
'_M'uru_
.' _Negative Energy_ hits 4-5 random raid members, dealing 1000 Shadow damage. This lasts the entire fight.
.' _Darkness_ creates a large AoE Void Zone that deals 3000 Shadow damage every second to all raid members inside it, and they cannot be healed. This also spawns 8 Dark Fiend adds.
.' Summons _Shadowsword Berserker_. These deal high melee damage and have a buff that increases their attack speed and movement by 100%.
.' Summons _Shadowsword Fury Mage_. These cast Fel Fireball, which deal 5000 Fire damage. They can also melee for 3000-4000 damage. They have a buff, Spell Fury, which causes spells to cast instantly, increases magic damage by 50% and makes them unable to move.
.' Summons _Void Sentinel_. Spawns a Voidwalker that hits for 9000-10,000 with melee attacks. Also has Shadow Pulse, an AoE that deals 4000 Shadow damage every 3 seconds. And Void Blast, which deals 10,000-11,000 damage as well as a debuff slowing attack speed by 35%.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Entropius_
.' _Negative Energy_ deals 2000 Shadow damage to Entropius' target, jumping to a nearby target and hitting for half the previous damage. The number of maximum targets this can hit increases every 12-15 seconds.
.' _Darkness_ spawns a Void Zone under a raid member, dealing 3000 Shadow damage every second. This also spawns a Dark Fiend that must be Dispelled or Purged.
.' _Singularity_ spawns over a raid member, after a few seconds it will move around and toss any nearby raid members into the air, interrupting them and dealing 500 damage.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc M'uru##25741
|modelnpc Entropius##25840
|confirm
step
map Sunwell Plateau/2
path loop off
path	32.4,41.5	27.9,60.9	36.7,83.6
path	48.2,87.0	60.3,66.5
.' Follow the path, clearing trash as you make your way to the last boss, Kil'jaeden. |goto 60.3,66.5 <5 |noway |c
step
goto 46,61
'_Kil'jaeden_
.' This fight consists of 5 phases.
'_Phase 1_
.' Kill the 3 Hands of the Deceiver adds that are around the Sunwell.
.' _Shadow Bolt Volley_ deals 1000 Shadow damage to all raid members within 30 yards and applies a stackable debuff that increases Shadow damage taken by 750.
.' _Soul Portal_ opens a portal that spawns Volatile Felfire Fiend. They run toward nearby raid members and explode, dealing massive damage. They have very low hit points and should be killed immediately.
.' _Shadow Infusion_ occurs when the Hand has 20% health. The Hand will get a buff that makes him immune to stun and silence effects.
.' _Phase 2_ starts after all Hands of the Deceiver are killed.
.' Click here to move to Phase 2 |confirm
step
goto 46,61
'_Kil'jaeden_
.' This fight consists of 5 phases.
'_Phase 2_
.' Kil'jaeden spawns, knocking back all raid members within the glowing circle in the middle of the room.
.' _Soul Flay_ deals 3000 Shadow damage per second and reduces the target's movement speed.
.' _Legion Lightning_ is an AoE spell that deals 3000-3500 Shadow damage to the target, then jumping to a nearby raid member and dealing less damage after each jump. It also drains 1500 mana from each player it affects.
.' _Fire Bloom_ places a debuff on up to 5 random raid members, dealing 1700 Fire damage every 2 seconds to the target and all other raid members within 10 yards. Lasts 20 seconds.
.' _Summon Shield Orb_ spawns a Shield Orb that attacks the raid with Shadow Bolts. These have low hit points and should be killed quickly after spawning.
.' _Phase 3_ starts at 85% health.
|modelnpc Kil'jaeden##25315
.' Click here to move to Phase 3 |confirm
step
goto 46,61
'_Kil'jaeden_
.' This fight consists of 5 phases.
'_Phase 3_
.' 30-40 seconds into this phase, Kalecgos will empower 1 of the 4 Orbs of the Blue Dragonflight around the room. Clicking these will turn you into a dragon and give you special abilities for 2 minutes. These abilities help counter some of Kil'jaeden's abilities.
.' _Sinister Reflection_ spawns 4 Sinister Reflections, mimicking the target player's class and image. Melee attacks from these Reflections deal large amounts of damage.
.' _Shadow Spike_ bombards randomly targeted raid members with spikes that explode when they reach their target, dealing 5000-6200 Shadow damage to all nearby raid members. Also applies a debuff that reduces healing effects by 50% for 10 seconds.
.' _Flame Dart Explosion_ deals 1800 Fire damage every 3 seconds, unless the player is affected by Breath:Haste from the Orbs of the Blue Dragonflight.
.' _Darkness of a Thousand Souls_ Kil'jaeden covers himself with wings. After 8 seconds he hits all raid members for 47,000-52,000 Shadow damage. This is countered by the Shield of the Blue ability from Orbs of the Blue Dragonflight.
.' _Phase 4_ starts at 55% health.
|modelnpc Kil'jaeden##25315
.' Click here to move to Phase 4 |confirm
step
goto 46,61
'_Kil'jaeden_
.' This fight consists of 5 phases.
'_Phase 4_
.' _Armageddon_ is an AoE meteor spell that deals 10,000 damage to any player inside it. There will be 3 of these will be in effect for this entire phase.
.' _Phase 5_ starts at 25% health.
|modelnpc Kil'jaeden##25315
.' Click here to move to Phase 5 |confirm
step
goto 46,61
'_Kil'jaeden_
.' This is the final phase of this fight.
'_Phase 5_
.' _Sacrifice of Aveena_ is a debuff that is applied to Kil'jaeden for this phase. It increases Holy damage he takes by 25%.
.' This phase is a dps race - kill him as quickly as possible to avoid death.
|modelnpc Kil'jaeden##25315
|confirm
step
.' Congratulations, you have finished the Sunwell Plateau raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\Hyjal Summit (Battle for Mount Hyjal)",[[
dungeon 161
description This guide will walk you through the Battle for Mount Hyjal Raid.
author support@zygorguides.com
step
goto Tanaris/18 41.7,38.7
.talk 20130
..accept 10277
|only if not completedq(10277)
step
goto 53.6,55.6
.' Follow the Custodian of Time as she travels around the Cavern. |q 10277/1
|modelnpc Custodian of Time##20129
|only if havequest(10277)
step
goto Hyjal Summit 10.5,63.8
.talk 17772
.' Tell her "My companions and I are with you."
.' This will start the first boss fight, so make sure you are ready.
|confirm
step
goto 7.6,69.5
.' Defeat the 8 waves of enemies.
|confirm
step
goto 7.6,69.5
'_Rage Winterchill_
.' _Frost Armor_ increases Rage Winterchill's armor by 3000, slows melee attackers' movement speed by 50%, and increases the time between their attacks by 25%.
.' _Icebolt_ stuns the player for 4 seconds and deals about 5000 damage.
.' _Death and Decay_ is an AoE spell that ticks for 15% of max health as damage per second to each player in the targeted area. Move out of it as quickly as possible.
.' _Frost Nova_ is an AoE spell that roots all players around Rage Winterchill for up to 10 seconds.
.' _Enrage_ occurs after 10 minutes of combat. It will greatly increase his attack speed and damage. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Rage Winterchill##17767
|confirm
step
goto Hyjal Summit 10.6,63.8
.talk 17772
.' Tell her "I am ready."
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 7.6,69.5
.' Defeat the 8 waves of enemies.
|confirm
step
goto 7.6,69.5
'_Anetheron_
.' _Vampiric Aura_ his melee attacks heals him for 300% of the damage.
.' _Carrion Swarm_ is a random one-direction cone attack that does 3000-6000 Shadow damage plus gives a 15 second debuff that reduces healing done by 75%. He casts this about every 15 seconds.
.' _Sleep_ is a 10 second stun that affects 3 targets.
.' _Inferno_ is cast every 60 seconds and targets a random raid member. An elite Towering Inferno is summoned on them and they are stunned for 2 seconds.
.' _Berserk_ occurs after 10 minutes of combat. It will greatly increase his attack speed and damage. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Anetheron##17808
|confirm
step
goto 10.5,63.8
.talk 17772
.' Tell her "Until we meet again."
.' This will start the transition to the next phase of the raid, make sure you are ready to move out.
|confirm
step
map  Hyjal Summit
path loop off
path	16.6,60.4	19.0,56.3	28.5,63.7
path	35.6,55.6	32.6,41.4	41.1,35.6
.' Follow this road east to the Horde Camp.
|confirm
step
goto 48.0,41.5
.talk 17852
.' Tell him "I am with you."
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 43.1,36.0
.' Defeat the first wave of enemies.
|confirm
step
goto 51.9,33.1
.' Defeat the second wave of enemies.
|confirm
step
goto 43.8,35.8
.' Defeat the third wave of enemies.
|confirm
step
goto 43.8,35.8
.' Defeat these few enemies.
|confirm
step
goto 51.9,33.1
.' Defeat any remaining enemies here.
|confirm
step
goto 43.8,35.8
.' Defeat these enemies.
|confirm
step
goto 47.4,37.5
.' Defeat any remaining enemies around here.
.' This wave usually has a flying Frost Wyrm, make sure to look up in the sky.
|confirm
step
goto 43.8,35.8
.' Defeat enemies here.
.' This wave usually has a flying Frost Wyrm, make sure to look up in the sky.
|confirm
step
goto 43.8,35.8
.' Defeat the last wave of enemies here.
|confirm
step
goto 43.5,36.0
'_Kaz'rogal_
.' This boss has a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _War Stomp_ is a 15 yard AoE melee attack, dealing 1700-2500 damage and also stuns for 5 seconds.
.' _Cripple_ debuff is given to a raid member that increases time between attacks by 75%, movement speed by 75% and reduces their strength by 75%. Lasts 12 seconds.
.' _Mark of Kaz'rogal_, a debuff that affects all mana users in the raid. Drains 600 mana per second for 5 seconds. If mana is reduced to 0 by the debuff, the debuff is removed and the target and nearby allies take 10,000-11,000 Shadow damage.
|modelnpc Kaz'rogal##17888
|confirm
step
goto 48.0,41.5
.talk 17852
.' Tell him "We have nothing to fear."
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 44.0,36.2
.' Defeat the first wave of enemies here.
|confirm
step
goto 44.8,38.1
.' Defeat the enemies in this area.
.' This wave usually has a flying Frost Wyrm, make sure to look up in the sky.
|confirm
step
goto 45.2,38.2
.' Defeat the Giant Infernals in the Horde Camp.
|confirm
step
goto 43.7,36.1
.' Defeat the rest of the enemies here.
|confirm
step
goto 47.9,37.
.' Defeat the Giant Infernals and Fel Stalkers attack the Camp.
|confirm
step
goto 43.7,36.1
.' Defeat the enemies in this area.
|confirm
step
goto 47.4,38.4
.' Defeat the Giant Infernals, Ghouls, Crypt Fiends, and Fel Stalkers attacking in this area.
|confirm
step
goto 43.7,36.1
.' Defeat the last wave of enemies here.
|confirm
step
goto 43.7,36.1
'_Azgalor_
.' This boss has a _cleave_, so make sure that the main tank is the only one that is in front of the boss when this occurs.
.' _Rain of Fire_ is a 15 yard AoE cast on a random target. Lasts 30 seconds and deals 1700 fire damage every 2 seconds. Party members inside the AoE for more than 2 seconds will also be hit for 1200 damage every second for 5 seconds.
.' _Howl of Azgalor_ is a 5 second zone-wide AoE silence.
.' _Doom_ is cast on a random party member every 45 seconds. The target dies after 20 seconds and spawns an elite add. Cannot be resisted or removed.
.' _Enrage_ occurs after 10 minutes of combat. It will greatly increase his attack speed and damage. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Azgalor##17842
|confirm
step
goto Hyjal Summit 48.0,41.5
.talk 17852
.' Tell him "Until we meet again."
.' This will start the transition to the final phase of the raid, make sure you are ready to move out.
|confirm
step
map  Hyjal Summit
path loop off
path	56.2,45.6	60.3,53.5	67.2,48.3
path	73.4,44.4	75.2,46.3
.' Follow the road east to Nordrassil.
|confirm
step
goto 74.3,57.6
.talk 17948
.' Tell her "I would be grateful to any aid you can provide."
.' This will start the next boss fight, so make sure you are ready.
|confirm
step
goto  75.3,45.6
.' Jump off the cliff here
.click the Tears of the Goddess in your bag to slow your fall |use Tears of the Goddess##24494
.' The slow effect only lasts 2 seconds, so wait until the last second to use it.
|confirm
step
goto 76.9,36.7
'_Archimonde_
.' _Air Burst_ is a 15 yard AoE spell that deals 1500 Nature damage centered on a random target. All affected players are tossed into the air and take over 100% fall damage. Avoid at all costs.
.' _Fear_ is an 8 second AoE Fear. Affects entire raid.
.' _Grip of the Legion_ deals 2500 damage every 2 seconds for 5 minutes. This is dispellable.
.' _Doomfire_ spell that leaves a trail of fire on the ground. Anyone touched by it gets a debuff that deals 2400 Fire damage every 3 seconds, which is reduced by 150 every tick.
.' _Finger of Death_ deals 30,000 Shadow damage to a single target. It is only used if no targets are in melee range.
.' _Soul Charge_ this is cast whenever a raid member dies.Achimonde gains a Soul Charge which he can use at a random time. The type of affect it has depends on the character class that made it. The following effects apply to the whole raid:
....' Priest, Mage, Warlock: 4500 Fire damage and a 4 second silence.
....' Warrior, Rogue, Paladin: 4500 Physical damage and 4 seconds damage taken increased by 50%.
....' Druid, Shaman, Hunter: 4500 Nature damage over 8 seconds plus 2250 mana burn.
.' _Enrage_ mechanic: _Hand of Death_ deals 100,000 Shadow damage to everyone in the raid. This is cast 9 minutes after Archimonde is engaged or if he reaches Nordrassil. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Archimonde##17968
|confirm
step
goto Tanaris/18 41.7,38.7
.talk 20130
..turnin 10277
|only if havequest(10277)
step
.' Congratulations, you have completed the Battle for Mount Hyjal raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\Gruul's Lair",[[
dungeon 776
description This guide will walk you through the Gruul's Lair Raid.
author support@zygorguides.com
step
goto Gruul's Lair 55,57
'_High King Maulgar_
.' _Arcing Smash_ deals damage to the target and any raid members near the target. It hits as a _cleave_, so make sure the tank is the only one in front of Maulgar when this happens.
.' _Berserker Charge_ causes Maulgar to charge his current target, an additional 300 damage and knocking them back.
.' _Intimidating Roar_ is a debuff that causes the target to freeze in fear for 8 seconds and causing all nearby raid members to flee in fear.
.' _Mighty Blow_ deals an additional 150 damage and knocks the target back.
.' _Whirlwind_ hits nearby enemies, inflicting an additional 110 damage for 15 seconds.
'_Krosh Firehand_ uses _Spell Shield_ which reduces magic damage taken by 75%.
'_Olm the Summoner_ has a stacking DoT and summons Fel Stalker adds. Also occasionally uses _Death Coil_, healing himself for the same amount of damage it deals.
'_Kiggler the Crazed_ has _Lightning Bolt_ which does about 3000 damage to a single target. _Arcane Shock_ deals 800 damage and applies a DoT.
'_Blindeye the Seer_ uses _Power Word: Shield_ which absorbs damage and _Prayer of Healing_ which heals to full. Also has a smaller heal that heals for about 5% health.
|modelnpc High King Maulgar##18831
|confirm
step
map Gruul's Lair
path loop off
path	43.9,69.0	33.2,65.4	20.9,52.7
.' Follow this short path, clearing trash as you make your way to the second and final boss, Gruul the Dragonkiller. |goto 20.9,52.7 <5 |noway |c
step
goto 21,27
'_Gruul the Dragonkiller_
.' _Growth_ is a buff that increases Gruul's damage by 15% and size by 10% every 15 seconds, stacking up to 30 times. Lasts for 5 minutes.
.' _Hurtful Strike_ hits the target with the second highest aggro within Gruul's melee range. Melee DPS should try to keep their aggro lower than the main tank and also the off tank.
.' _Reverberation_ is a zone-wide silence AoE. It lasts 4 seconds.
.' _Cave In_ is a random AoE that deals 3000 damage every 3 seconds in the targeted area.
.' _Ground Slam_ is a zone-wide knockback, then triggers _Gronn Lord's Grasp_.
.' _Gronn Lord's Grasp_ is a stacking debuff that occurs after a Ground Slam. It reduces movement speed by 20%, stacking 5 times and then causing the _Stoned_ effect.
.' _Stoned_ stuns the target. Occurs after getting 5 stacks of Gronn Lord's Grasp. Will then cause _Shatter_.
.' _Shatter_ is a raid wide AoE. The damage dealt is increased if raid members are near each other when it occurs. Hits for 1000-8500, depending on player proximity.
|modelnpc Gruul the Dragonkiller##19044
|confirm
step
.' Congratulations, you have finished the Gruul's Lair raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\Magtheridon's Lair",[[
dungeon 779
description This guide will walk you through the Magtheridon's Lair Raid.
author support@zygorguides.com
step
map Magtheridon's Lair
path loop off
path	68.4,25.6	68.6,48.0	63.6,59.3
path	57.5,68.7	65.0,93.4
.' Follow the path, clearing trash as you make your way to Magtheridon. |goto 65.0,93.4 <5 |noway |c
step
goto 68,78
'_Magtheridon_
'_Phase 1_
.' This phase starts as soon you a Channeler is engaged in combat. The raid has 2 minutes before Magtheridon becomes active. Interrupt and kill as many Channelers as possible.
'_Phase 2_
.' Starts when Magtheridon becomes active and starts attacking the raid. Any remaining Channelers need to be killed.
.' _Blast Nova_ deals a large amount of Fire damage to all raid members in range. Must be interrupted by clicking the 5 Manticron Cubes.
'_Phase 3_
.' Starts when Megtheridon reaches 30% health. He shatters the walls, causing the roof to collapse which deals 5200-6800 damage and a 2 second stun to the entire raid.
.' _Collapse_ is the destroyed ceiling falling on the raid. It occurs randomly for the rest of this phase.
|modelnpc Magtheridon##17257
|confirm
step
.' You may have looted a quest item from Magtheridon.
.' Click Magtheridon's Head in your bags |use Magtheridon's Head##32385
.accept 11002
|only if not completedq(11002)
step
goto Hellfire Peninsula 56.6,66.6
.talk 16819
.|tip He is upstairs in the main war room.
..turnin 11002
|only if not completedq(11002)
step
.' Congratulations, you have finished the Magtheridon's Lair raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\Serpentshrine Cavern",[[
dungeon 780
description This guide will walk you through the Serpentshrine Cavern Raid.
author support@zygorguides.com
step
.' Take the elevator down |goto Serpentshrine Cavern 13.5,58.9 <5 |c
step
map Serpentshrine Cavern
path loop off
path	15.4,58.0	17.3,66.4	19.0,69.2
path	19.7,68.0
.' Follow the path to the second elevator, and then take it up. |goto 19.7,68.0 <5 |noway |c
step
map Serpentshrine Cavern
path loop off
path	21.4,72.9	25.8,73.7	28.1,80.1
path	32.2,82.6
.' Follow this path, clearing trash as you make your way to the first boss, Hydross the Unstable. |goto 32.2,82.6 <5 |noway |c
step
goto 36.8,84.8
'_Hydross the Unstable_
.' _Summon Elementals_ Hydross summons 4 elementals whenever he switches between forms.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Water Form_
.' Immune to Frost damage.
.' _Mark of Hydross_ is a raid-wide debuff that stacks up to 6 times, increasing Frost damage taken by 10%, 25%, 50%, 100%, 250%, and finally 500%.
.' _Water Tomb_ is a single target AoE. It stuns all affected players for 4 seconds and deals 4500 Frost damage over 5 seconds.
'_Poison Form_
.' Immune to Nature damage.
.' _Mark of Corruption_ is a raid-wide debuff that stacks up to 6 times, increasing Nature damage taken by 10%, 25%, 50%, 100%, 250%, and finally 500%.
.' _Vile Sludge_ is a debuff that deals 4000 Nature damage over 24 seconds and reduces healing and damage by 50%.
|modelnpc Hydross the Unstable##21216
|confirm
step
map Serpentshrine Cavern
path loop off
path	35.9,75.9	36.4,69.6	35.5,64.4
.' Clear the trash as you make your way to this spot. |goto 35.5,64.4 <5 |noway |c
step
.' Jump off the pipe here and swim to the large circular platform below. |goto 36.3,61.9 <5 |c
step
goto 40,58
'_The Lurker Below_
.' You or a raid member has to fish here in the Strange Pool to summon the boss.
'_Phase 1_
.' _Water Bolt_ deals 9000-11,000 damage to a random target. He will only use this if no player is within melee range.
.' _Whirl_ deals 3000-4000 physical damage and a 5 yard knockback to all raid members in melee range.
.' _Geyser_ affects a random raid member, dealing 3000-4000 Frost damage and a 10 yard knockback to everyone within 10 yards of the target.
.' _Spout_ shoots a water spray from his mouth and begins to rotate 360 degrees, dealing 3500 Frost damage and knocking the target back 60 yards.
'_Phase 2_
.' The Lurker Below submerges underwater, spawning 9 adds. He will resurface after 1 minute.
.' _Coilfang Guardian_ adds have a Arcing Smash _cone_, make sure the tank is the only one in front of them when this occurs.
.' _Coilfang Ambusher_ adds have a Multi-Shot attack, which hits up to 3 players for 2000-3000 damage.
|modelnpc The Lurker Below##21217
|confirm
step
map Serpentshrine Cavern
path loop off
path	37.1,51.7	40.6,49.5	41.0,42.7
path	40.0,40.1	33.5,41.9	30.0,39.1
path	30.6,29.6	36.1,25.0
.' Follow the path, clearing trash as you make your way to the next boss, Leotheras the Blind. |goto 36.1,25.0 <5 |noway |c
step
goto 41.3,26.2
'_Leotheras the Blind_
.' He switches between human and demon forms, wiping aggro each time this happens.
'_Phase 1_ lasts until Leotheras has 15% health.
'_Human Form_
.' _Whirldwind_ lasts 12 seconds and deals 3000 damage plus a DoT that lasts 15 seconds and deals 2500 damage every 3 seconds.
'_Demon Form_
.' _Chaos Blast_ is an AoE that deals 150 Fire damage and applies a debuff that increases Fire damage taken by 1675 and stacks up to 20 times.
.' _Summon Inner Demons_ occurs about 45 seconds into the Demon Form phase. Inner Demons are spawns for 5 raid members, they can only be attacked by the person they belong to. If Leotheras returns to human form while one of them are alive that player will be Mind Controlled for 10 minutes.
'_Phase 2_ starts at 15% health.
.' Leotheras splits and _both forms_ must be dealt with at the same time. The demon form has full health while the human form retains the same health.
.' _Kill_ the human form as quickly as possible to avoid death.
|modelnpc Leotheras the Blind##21215
|confirm
step
map Serpentshrine Cavern
path loop off
path	43.1,30.7	45.3,23.3	39.5,20.6
path	42.3,14.2	44.8,14.1
.' Follow the path, clearing trash as you make your way to the next boss, Fathom-Lord Karathress. |goto 44.8,14.1 <5 |noway |c
step
goto 48,17
'_Fathom-Lord Karathress_
.' _Cataclysmic Bolt_ affects a random raid member, dealing 50% of the their maximum health as Shadow damage, knocking them down and stunning them.
.' _Sear Nova_ deals 3000 Fire damage to all raid members in melee range.
.' _Bkessing of the Tides_ Karathress gains this buff if he reaches 75% health while any of the other adds are alive. It increases his attack and cast speed by 66% for each add.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Fathom-Guard Sharkkis_
.' _Multi-shot_ deals 2000-3000 damage to 3 raid members.
.' _Leeching Arrow_ targets a random raid member and burns 525 health and mana every second for 12 seconds.
.' _Hurl Trident_ hits a random player for 1500 damage.
.' _Summon Pet_ spawns an elemental or a beast pet to attack the raid. The elemental pet explodes upon death.
.' _The Beast Within_ increases Sharkkis' damage by 30# and pet damage by 50%.
'_Fathom-Guard Tidalvess_
.' _Spitfire Totem_ lasts 1 minute and attacks up to 5 raid members within 45 yards for 2500-3500 Fire damage.
.' _Poison CLeansing Totem_ removes 1 poison effect from Tidalvess every 5 seconds.
.' _Earthbind Totem_ periodically reduces the movement speed of nearby players by 50%.
'_Fathom-Guard Caribdis_
.' _Water Bolt Volley_ is a 45 yard AoE that deals 2700-3200 Frost damage.
.' _Tidal Surge_ is a 10 yard AoE stun that lasts 3 seconds.
.' _Healing Wave_ is a single target heal cast on any mob or herself, healing 20-30% health.
.' _The Whirlwind_ spawns a whirlwind near her, which moves around the room, knocking players into the air for 3-9 seconds and applying a debuff that increases their cast time.
|modelnpc Fathom-Lord Karathress##21214
|confirm
step
map Serpentshrine Cavern
path loop off
path	51.4,14.3	58.1,11.4	59.4,19.1
.' Follow the path, clearing trash as you make your way to the next boss, Morogrim Tidewalker. |goto 59.4,19.1 <5 |noway |c
step
goto 59.4,25.1
'_Morogrim Tidewalker_
.' _Tidal Wave_ deals 4000-5000 Frost damage to the target and reduces their attack speed by 400% for 15 seconds.
.' _Watery Grave_ teleports 4 raid members under the waterfalls, inflicting 6000 damage after 6 seconds and fall damage.
.' _Earthquake_ deals 3000-4000 damage in a 50 yard radius around Morogrim.
.' _Summon Murlocs_ Morogrim spawns 2 packs of 6 murlocs each after every _Earthquake_.
.' _Summon Watery Globules_ occurs at 25% health. He stops casting Watery Grave and summons 4 watery globules, 1 from each grave spot. They move towards a raid member and explode for 4000-6000 damage.
|modelnpc Morogrim Tidewalker##21213
|confirm
step
map Serpentshrine Cavern
path loop off
path	59.5,32.9	52.9,41.7	47.0,41.7
path	41.3,41.6	40.9,48.0	46.9,51.0
path	50.5,58.3
.' Follow the path, clearing trash as you make your way to the last boss, Lady Vashj. |goto 50.5,58.3 <5 |noway |c
step
goto 50.5,58.3
.' Click Lady Vashj's Bridge Console to bring up the bridge.
|confirm
step
goto 71.8,59.1
'_Lady Vashj_
'_Phase 1 and 3_
.' _Multi-Shot_ hits the target and up to 4 nearby raid members for 6500-7500 damage.
.' _Shock Blast_ deals 9000 Nature damage and a 5 second stun to the targeted raid member.
.' _Entangle_ is a 10 second AoE immobilize that affects all raid members within 15 yards of Vashj.
.' _Static Charge_ is a debuff that does 2000 damage per tick to the target and anyone within 5 yards, lasting 15 seconds.
'_Phase 2_
.' Vashj spawns adds to attack the raid and her shield must be turned off.
.' _Forked Lightning_ deals 2300-2600 Nature damage in a _frontal cone_. Make sure the tank is the only one in front of her when this occurs.
'_Enchanted Elemental:_ slowly moves toward Vashj and increases her damage by 5% if it reacher her.
'_Tainted Elemental:_ does _Poison Spit_, which hits a random target for 2000 Nature damage and an additional DoT that deals 2000 damage every second for 8 seconds.
'_Coilfang Elite:_ has a _cleave_. Make sure the tank is the only one in front of them when this occurs.
'_Coilfang Strider:_ uses _Mind Blast_ that deals 2500 Shadow damage. _Panic Aura_ is an 8 yard AoE that causes a 4 second Fear every tick.
'_Phase 3_
'_Toxic Spore Bats:_ continue to spawn in this phase. They use _Toxic Spores_, which is a poison AoE that deals 1500 Nature damage per second.
|modelnpc Lady Vashj##21212
|confirm
step
.' Congratulations, you have finished the Magtheridon's Lair raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\The Eye",[[
dungeon 782
description This guide will walk you through The Eye Raid.
author support@zygorguides.com
step
map Tempest Keep
path loop off
path	50.0,87.6	50.1,75.6	50.1,65.5
path	50.0,62.7	47.7,64.4
.' Follow the path, clearing trash as you make your way to the first boss, Al'ar. |goto 47.7,64.4 <5 |noway |c
step
goto 45.4,59.2
'_Al'ar <Phoenix God>_
.|tip When pulled, he flies to the spot first. Have a tank here to pick him up.
'_Phase 1_
.' _Flame Buffet_ only occurs if there isn't a player within melee range of Al'ar. It is a stackable debuff that occurs every 1.5 seconds, causing 2000 Fire damage and increasing Fire damage taken by 10% per stack.
.' _Flame Quills_ occurs when Al'ar flies to the center of the room and launches quills at the platforms or the outer part of the ramps, dealing 8000 Fire damage per second.
.' _Ember Blast_ occurs when Al'ar "dies". He explodes, knocking back nearby raid members and dealing 10,000 Fire damage.
'_Phase 2_
.' _Flame Buffet_ only occurs if there isn't a player within melee range of Al'ar. It is a stackable debuff that occurs every 1.5 seconds, causing 2000 Fire damage and increasing Fire damage taken by 10% per stack.
.' _Dive Bomb_ occurs after Al'ar "dies" the first time. He comes back to life and targets a random raid member. He crashes into the ground, dealing 5000 Fire damage and spawning two Ember of Al'ar adds. He then uses _Rebirth_.
.' _Rebirth_ occurs after _Dive Bomb_, dealing 5000 Fire damage to anyone in melee range and knocking them back.
.' _Flame Patch_ spawns under a random player, dealing 3000 Fire damage per tick, and applying a stacking debuff that deals Fire damage and increases Fire damage taken.
.' _Melt Armor_ reduces the target's armor by 80% for 60 seconds. When this happens tanks must taunt immediately.
.' _Berserk_ is an _enrage_ mechanic and occurs after 10 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Al'ar##19514
|confirm
step
map Tempest Keep
path loop off
path	45.1,58.8	47.1,51.0	40.5,41.5
path	31.8,44.6
.' Follow the path, clearing trash as you make your way to the next boss, the Void Reaver. |goto 31.8,44.6 <5 |noway |c
step
goto 25.4,49.6
'_Void Reaver_
.' Void Reaver is _immune_ to taunts.
.' _Pounding_ is a channeled AoE that lasts 3 seconds, dealing 1350-2250 Arcane damage every second.
.' _Arcane Orb_ is an AoE used on a random raid member that is at least 18 yards away from Void Reaver. It deals 7000 Arcane damage along with a 6 second silence.
.' _Knock Away_ is a single target knockback used on the raid member with the highest aggro, slightly reducing their aggro.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Void Reaver##19516
|confirm
step
map Tempest Keep
path loop off
path	32.1,44.4	40.5,41.5	46.9,50.8
path	53.0,51.1	59.6,41.6	68.2,44.5
.' Follow the path, clearing trash as you make your way to the next boss, High Astromancer Solarian. |goto 68.2,44.5 <5 |noway |c
step
goto 73.8,49.2
'_High Astromancer Solarian_
.' _Wrath of the Astromancer_ is a debuff placed on a random raid member. After 6 seconds they explode, dealing 5400-6600 Arcane damage to all nearby allies.
.' _Arcane Missles_ targets a random raid member and deals 3000 Arcane damage every second for 3 seconds.
.' _Blinding Light_ is an AoE that deals 2500 Arcane damage to the entire raid.
.' Every 50 seconds Solarian disappear and summons adds to attack the raid. She returns after 15 seconds, with 2 more elite adds.
|modelnpc High Astromancer Solarian##18805
|confirm
step
map Tempest Keep
path loop off
path	68.2,44.5	59.6,41.6	57.7,33.4
path	53.9,27.2
.' Follow the path, clearing trash as you make your way to the final boss, Kael'thas Sunstrider. |goto 53.9,27.2 <5 |noway |c
step
goto 50.0,15.0
'_Kael'thas Sunstrider_
.' Kael'thas and his advisors are _immune_ to taunts.
'_Phase 1-3_
.' _Fireball_ deals 16,000-20,000 Fire damage and is interruptible.
.' _Flamestrike_ is an AoE that deals 120,000 damage in a 6 yard radius plus an additional 3000 Fire damage per second for 8 seconds.
.' _Summon Pheonix_ occasionally spawns a pheonix add to attack the raid.
.' _Arcane Disruption_ deals 1750-2250 Arcane damage and a 10 second disorient to the entire raid.
.' _Shock Barrier_ is a shield that absorbs 80,000 damage, lasting 10 seconds and making him immune to interrupts.
'_Phase 4 Abilities_
.' _Mind Control_ Kael'thas mind controls 3-4 raid members.
.' _Pyroblast_ deals 45,000-55,000 Fire damage. It is interruptable and he will begin casting it after using _Shock Barrier_
'_Phase 5 Abilities_
.' _Gravity Lapse_ all raid members are teleported to Kael'thas and then "swim" in the air for 30 seconds.
.' _Nether Beam_ deals 2000 Arcane damage to a random raid member, jumping to nearby players and doubling the damage dealt every jump.
.' _Nether Vapor_ spawns black clouds above Kael'thas. If touched they apply a stacking debuff that reduces max health ny 10% and stack up to 9 times.
'_Thaladred the Darkener_
.' _Gaze_ fixes all aggro to the targeted player.
.' _Phychic Blow_ deals 4500 damage plus a knockback.
.' _Silence_ is an AoE silence, affecting all raid members within 8 yards of Thaldred.
'_Lord Sanguinar_
.' _Bellowing Roar_ is an AoE fear, centered on himself and fearing all raid members within 35 yards.
'_Grand Astromancer Capernian_
.' _Fireball_ deals 6000-7000 Fire damage to a single target.
.' _Conflagration_ hits a random raid member within 30 yards, causing disorient and 600-900 damage per second plus 300 damage per second to all players within 8 yards of the targeted player.
.' _Arcane Explosion_ deals 4500 Arcane damage plus a knockdown and debuff that slows movement speed.
'_Master Engineer Telonicus_
.' _Bomb_ deals 7000-9000 Fire damage in a small AoE centered on his target.
.' _Remote Toy_ is a 60 second debuff used on a random raid member, is occasionally stuns the player for 4 seconds throughout the duration.
|modelnpc Kael'thas Sunstrider##19622
|confirm
step
.' You may have looted a quest item from Kael'thas.
.' Click the Verdant Sphere in your bags |use Verdant Sphere##32405
.accept 11007
|only if not completedq(11007)
step
goto Shattrath City 54.2,44.4
.talk 18481
..turnin 11007
|only if havequest(11007)
step
.' Congratulations, you have finished The Eye raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Outland\\Black Temple",[[
dungeon 796
description This guide will walk you through the Black Temple Raid.
author support@zygorguides.com
step
map Black Temple/2
path loop off
path	23.0,46.7	24.8,41.1	32.7,41.4
path	37.1,38.4	37.4,21.7
.' Follow the path, clearing trash as you make your way to the first boss, High Warlord Naj'entus. |goto 37.4,21.7 <5 |noway |c
step
goto 43.6,19.4
'_High Warlord Naj'entus_
.' _Needle Spine_ deals 2900-3900 damage and an additional 2500 Frost damage to 3 random targets.
.' _Impaling Spine_ hits a random raid member for 4500-4900 damage, stunning the target and applying a DoT that deals 2750 damage every 3 seconds, lasting 30 seconds.
.' _Tidal Shield_ Naj'entus becomes immune to all damage and regenerates health. Can only be broken by a player throwing Naj'entus Spine at him. The raid takes 8500 Frost damage once the shield is broken. |use Najentus Spine##32408
.' _Enrage_ occurs after 8 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc High Warlord Naj'entus##22887
|confirm
step
map Black Temple/2
path loop off
path	33.4,19.2	29.2,19.7	27.4,14.7
path	27.5,6.6
.' Follow the path, clearing trash as you make your way to the next boss, Supremus. |goto 27.5,6.6 <5 |noway |c
step
map Black Temple/1
path loop off
path	28.1,67.8	31.9,51.9	42.1,50.0
.' Follow the path, clearing trash as you make your way to the next boss, Supremus. |goto 42.1,50.0 <5 |noway |c
step
goto 61.1,47.6
'_Supremus_
'_Phase 1_
.' _Molten Flame_ is a blue fire trail coming from Supremus, spreading along the ground towards a random target. The flames deal 3325-3675 Fire damage per second.
.' _Hateful Strike_ is a melee attack that deals 27,500-32,200 damage to the target with most health and within melee range.
'_Phase 2_
.' _Gaze_ fixes aggro on a target and and Supremus moves towards them at 90% normal speed.
.' _Molten Punch_ affects the target if they are affected by _Gaze_. It knocks the target back and deals 5200 damage if they are within 40 yards of Supremus.
.' _Volcanic Geyser_ randomly summons small volcanoes which deal 4100-4800 Fire damage to targets within 15 yards.
|modelnpc Supremus##22898
|confirm
step
path loop off
path	Black Temple/1 64.0,47.6	Black Temple/3 24.9,51.2	Black Temple/3 29.5,56.6
path	Black Temple/1 85.4,51.9	Black Temple/3 40.8,67.8	Black Temple/3 40.8,79.8
.' Follow the path, clearing trash as you make your way to the next boss, the Shade of Akama. |goto Black Temple/3 40.8,79.8 <5 |noway |c
step
goto Black Temple/3 40.9,74.5
.talk 23191
.' Tell him "I'm with you, Akama."
.' This will start the boss fight, make sure you are ready.
|confirm
step
goto Black Temple/3 40.8,88.9
'_Shade of Akama_
'_Phase 1_
.' You must kill the Channelers around the Shade of Akama, as well as the adds he spawns during this time.
.' _Ashtongue Defender:_ uses _Debilitating Shot_, which applies a debuff to the target that reduces all melee damage done by 75% for 5 seconds.
.' _Ashtongue Elementalist:_ uses _Rain of Fire_, an AoE which does 3500 Fire damage every 2 seconds, lasting 8 seconds.
.' _Ashtongue Rogue:_ uses _Debilitating Poison_, which slows attack and casting speed of the target by 50% along with a DoT that does 1800 Nature damage every 2 seconds for 8 seconds.
.' _Ashtongue Spiritbinder:_ uses _Spirit Mend_, which heals another Ashtongue for 2500 every 2 seconds for 10 seconds. _Chain Heal_ heals up to 5 Ashtongue for 7000 each.
'_Phase 2_ starts when the Channelers are killed and the Shade becomes attackable.
.' During this phase all raid members (even most healers) need to attack and kill the Shade of Akama as quickly as possible.
|modelnpc Shade of Akama##22841
|confirm
step
path loop off
path	Black Temple/3 40.9,67.6	Black Temple/3 53.0,66.1	Black Temple/3 60.3,66.3
path	Black Temple/3 67.9,66.6	Black Temple/3 70.1,75.9	Black Temple/3 57.9,91.4
path	Black Temple/5 75.5,67.5	Black Temple/5 55.2,68.5	Black Temple/5 34.6,56.5
.' Follow the path, clearing trash as  you make your way to the next boss, Teron Gorefiend. |goto Black Temple/5 34.6,56.5 <5 |noway |c
step
goto Black Temple/5 39,19
'_Teron Gorefiend_
.' _Incinerate_ is cast on a random raid member, dealing 3000 Fire damage and applying a debuff which does an additional 8000 Fire damage over 3 seconds.
.' _Doom Blossom_ Teron summons a dark cloud which lasts for 2 minutes and casts Shadow Bolts at random raid members, dealing 1500 Shadow damage.
.' _Crushing Shadows_ is cast on 5 random raid members, applying a 15 second debuff which increases Shadow damage taken by 60%.
.' _Shadow of Death_ is cast every 30 seconds on a random raid member, giving a debuff that kills the target after 55 seconds. Upon their death, 4 Shadowy Constructs spawn and the player turns into a Ghost for 60 seconds.
'_Shadowy Constructs_
.' _Shadow Strike_ melee attack that hits for about 2500 Shadow damage.
.' _Atrophy_ is a 60 second debuff applied on a melee attack. It decreases attack speed by 5% and stacks up to 10 times.
.' _Immune_ cannot take damage from normal players, only by players in _Ghost form_.
|modelnpc Teron Gorefiend##22871
|confirm
step
path loop off
path	Black Temple/5 34.5,59.1	Black Temple/5 76.4,68.8	Black Temple/3 70.4,76.5
path	Black Temple/3 59.5,66.2	Black Temple/3 52.7,62.8	Black Temple/3 54.2,50.0
path	Black Temple/3 52.8,37.8	Black Temple/3 59.6,35.1	Black Temple/3 62.6,34.7
path	Black Temple/4 73.4,35.4	Black Temple/4 72.7,21.6	Black Temple/4 65.7,21.3
path	Black Temple/4 53.2,30.1	Black Temple/4 53.4,39.1
.' Follow the path, clearing trash as you make your way to the next boss, Gurtogg Bloodboil. |goto Black Temple/4 53.4,39.1 <5 |noway |c
step
goto 53,46
'_Gurtogg Bloodboil_
.' _Fel Acid Breath_ is a 10 yard AoE, hitting in a _frontal cleave_ and applying a DoT that deals 2750 Nature damage every 5 seconds for 20 seconds. Make sure the tank is the only one in front of him when this occrs.
.' _Arcing Smash_ deals 5000 damage and a 5 yard _frontal cleave_. Make sure the tank is the only one in front of him when this occurs.
.'_Phase 1 Abilities_
.' _Bloodboil_ is a debuff cast every 10 seconds on the 5 raid members farthest away from Gurtogg. It deals 600 damage per stack every second for 24 seconds.
.' _Acidic Wound_ is a stacking debuff applied on melee every 2 seconds and lasts 60 seconds. It reduces armor by 500 and ticks for 250 damage.
.' _Eject_ knockbacks the target, reducing their threat.
.' _Bewildering Strike_ disorients the target for 5 seconds.
'_Phase 2 Abilities_
.' _Arcing Smash_ damage is increased to 12,000 and gives a debuff that reduces healing received by 50%.
.' _Fel Rage (Player)_ 30 second debuff that increases armor, health, healing, damage, and size. Gurtogg will only attack the player that has this.
.' _Fel Rage (Self)_ is a stacking buff which increases Gurtogg's damage and forces him to only attack the player with _Fel Rage_.
.' _Acidic Geyser_ deals 5000 Nature damage in a 5 yard AoE on and around the _Fel Rage_ target.
.' _Fel Acid Breath_ this is only cast on the raid member with _Fel Rage_.
.' _Insignificance_ all raid members get this buff during this phase. Threat cannot be gained.
.' _Berserk_ occurs after 10 minutes of combat. It increases his damage by 500%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Gurtogg Bloodboil##22948
|confirm
step
map Black Temple/4
path loop off
path	58.4,47.5	65.1,47.4	74.2,53.3
path	73.7,69.8	73.3,84.8	70.6,85.8
.' Follow the path, clearing trash as you make your way to the next boss, the Reliquary of Souls. |goto 70.6,85.8 <5 |noway |c
step
goto 65,85
'_Reliquary of Souls_
'_Essence of Suffering_
.' _Aura of Suffering_ reduces the raid's healing, regeneration, and armor by 100% and defense by 500.
.' _Fixate_ Essence of Suffering locks aggro on the closest target for 5 seconds.
.' _Enrage_ occurs every 45 seconds, increasing its damage and attack speed.
.' _Soul Drain_ is a debuff applied to 3 random targets, dealing 2600-3300 damage plus draining 2600-3300 mana every 3 seconds.
'_Essence of Desire_
.' _Aura of Desire_ is applied to the whole raid. 50% of damage dealt also hits the player. Healing is increased by 100% and max mana is reduced by 5% every 8 seconds.
.' _Deaden_ is a debuff that increases damage taken by 100% and lasts 10 seconds.
.' _Spirit Shock_ deals 10,000 Arcane damage and a 5 second confuse.
.' _Rune Shield_ lasts 15 seconds and absorbs 50,000 damage, also granting immunity to interrupts and increasing attack and casting speeds.
'_Essence of Anger_
.' _Aura of Anger_ deals raid-wide Shadow damage which increases by 100 every 3 seconds and increases damage taken by 5% each tick.
.' _Seethe_ gives Essence of Anger a 10 second buff that increases its attack speed and cast speed by 100%.
.' _Soul Scream_ hits 5 raid members in a 10 yard _frontal cone_ are hit with 3000 Shadow damage plus draining 5000 mana/rage.
.' _Spite_ affects 3 random raid members, applying a debuff that gives 2 seconds immunity, then deals 7500 Nature damage, followed by another 2 seconds immunity.
|modelnpc Reliquary of the Lost##22856
|confirm
step
path loop off
path	Black Temple/4 73.3,86.2	Black Temple/4 73.4,62.3	Black Temple/4 73.5,52.0
path	Black Temple/4 66.7,39.5	Black Temple/4 63.1,39.3	Black Temple/3 41.6,34.1
path	Black Temple/3 41.6,24.7	Black Temple/3 34.3,17.1	Black Temple/3 26.2,21.3
path	Black Temple/6 12.7,66.6	Black Temple/6 26.9,53.8	Black Temple/6 32.3,36.7
path	Black Temple/6 56.5,37.1
.' Follow the path, clearing trash as you make your way to the next boss, Mother Shahraz. |goto Black Temple/6 56.5,37.1 <5 |noway |c
step
goto 66.6,37
'_Mother Shahraz_
.' _Saber Lash_ is a _frontal cleave_ dealing 30,000 damage divided up among 3 targets. Make sure the tanks are the only ones in front of her when this occurs.
.' _Fatal Attraction_ teleports 3 random raid members to one location and applies the _Demonic Energy_ debuff.
.' _Demonic Energy_ lasts 30 seconds, the affected players release a 15 yard AoE that deals 3000 Shadow damage per second. It is removed if all 3 are 25 yards away from each other.
.' _Silencing Shriek_ is an 18 yard AoE silence.
.' _Beams_ a beam is cast every 9 seconds and hits 10 random raid members. There are multiple types of beams.
...' _Sinful Beam:_ deals 7000-8000 Shadow damage.
...' _Sinister Beam:_ deals 2000 Shadow damage and knocks those affected into the air, dealing 50% fall damage.
...' _Vile Beam:_ deals 2500 Shadow damage every second for 8 seconds.
...' _Wicked Beam:_ deals 4000 Shadow damage plus drains 1000 mana.
.' _Prismatic Aura_ occurs every 15 seconds, choosing a random aura which reduces damage of one spell type and increasing another.
...' _Arcane:_ Arcane damage reduced by 25%. Nature damage increased by 25%.
...' _Nature:_ Nature damage reduced by 25%. Arcane damage increased by 25%.
...' _Holy:_ Holy damage reduced by 25%. Shadow damage increased by 25%.
...' _Shadow:_ Shadow damage reduced by 25%. Holy damage increased by 25%.
...' _Fire:_ Fire damage reduced by 25%. Frost damage increased by 25%.
...' _Frost:_ Frost damage reduced by 25%. Fire damage increased by 25%.
.' _Enrage_ occurs at 10%, increasing her damage and attack speed slightly. Kill her as quickly as possible to avoid death.
.' _Hard Enrage_ occurs after 10 minutes of combat. It increases her damage by 500%. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
|modelnpc Mother Shahraz##22947
|confirm
step
map Black Temple/7
path loop off
path	Black Temple/6 67.2,45.9	Black Temple/6 67.3,55.2	Black Temple/7 69.2,14.1
path	65.9,24.9	69.1,36.6	65.8,48.3
path	65.8,60.1	63.1,79.0	47.0,73.9
.' Follow the path, clearing trash as you make your way to the next boss, The Illidari Council. |goto 47.0,73.9 <5 |noway |c
step
goto 47.3,47.9
'_The Illidari Council_
'_Lady Malande_
.' _Empowered Smite_ deals 5500-6000 Holy damage.
.' _Divine Wrath_ affects a random raid member, dealing 5000 Holy damage and a DoT that deals 2500 Fire damage every second for 8 seconds.
.' _Reflective Shield_ lasts 20 seconds, absorbing up to 25,000 damage and reflecting half the damage back to the attacker.
.' _Circle of Healing_ heals the Council for 100,000. It is a 2.5 second cast and is interruptible.
'_High Nethermancer Zerevor_
.' _Arcane Bolt_ deals 15,000 Arcane damage.
.' _Flamestrike_ is a 10 yard AoE at a random location that deals 5000 Fire damage and an additional 3000 every second for 12 seconds.
.' _Blizzard_ is a 10 yard AoE at a random location that deals 6000 Frost damage every second for 12 seconds.
.' _Dempen Magic_ is a self buff that reduces Zerevor's magic damage taken by 75% and healing by up to 500.
.' _Arcane Explosion_ is a 10 yard AoE centered around Zerevor that deals 9000 Arcane damage. This is only used if there is a raid member within 10 yards of him.
'_Gathios the Shatterer_
.' _Hammer of Justice_ stuns a random raid member within 10-40 yards, lasting 6 seconds.
.' _Consecration_ is a 12 yard AoE, centered around Gathios, that lasts 21 seconds and deals 2250 damage every 3 seconds.
.' _Seal of Command_ is a self buff that inceases Gathios' melee damage by 70%. Shares a cool down with _Seal of Blood_.
.' _Seal of Blood_ is a self buff that increases Gathios' by 800 Holy damage.
,' _Judgement_ removes one of Gathios' Seals, adding a DoT to his target. When removed with this, Seal of Command hits for 7000 over 9 seconds and Seal of Blood hits for 11,000 damage over 9 seconds.
.' _Blessing of Spell Warding_ is a 15 second buff that grants Gathios' target immunity to spell damage. Shared cool down with _Blessing of Protection_.
.' _Blessing of Protection_ is a 15 second buff that grants Gathios' target immunity to physical damage. Shared cool down with _Blessing of Spell Warding_.
.' _Devotion Aura_ is a 30 second buff applied to the rest of the Council, increasing their armor by 20%. Shared cool down with _Chromatic Resistance Aura_.
.' _Chromatic Resistane Aura_ is a 30 second buff applied to the rest of the Council, increasing their resistances by 250. Shared cool down with _Devotion Aura_.
'_Veras Darkshadow_
.' _Deadly Poison_ deals 1000 Nature damage every second for 4 seconds.
.' _Envenom_ is used on a raid member that has _Deadly Poison_, removing the debuff and dealing 6000 damage.
.' _Vanish_ Veras stealths from combat, lasting 30 seconds.
.' _Enrage_ occurs after 15 minutes. It increases their damage by 500%. Kill them before they reach this stage to avoid death.
|modelnpc Lady Malande##22951
|modelnpc High Nethermancer Zerevor##22950
|modelnpc Gathios the Shatterer##22949
|modelnpc Veras Darkshadow##22952
|confirm
step
goto Black Temple/7 47.1,54.6
.talk 23089
.' Tell him "I'm ready."
|confirm
step
path loop off
path	36.9,48.0	47.6,34.9	47.5,30.2
path	Black Temple/8 67.9,20.4	Black Temple/8 71.7,38.4
.' Follow Akama up to Illidan. |goto Black Temple/8 71.7,38.4 <5 |noway |c
step
goto Black Temple/8 72.0,38.5
.talk 23089
.' Tell him "We're ready to face Illidan."
.' This will start the final boss fight, make sure you are ready.
|confirm
step
goto 53,54
'_Illidan Stormrage_
'_Phase 1, 3, and 5 Abilities_
.' _Flame Crash_ is a 10 yard AoE that deals 5000 Fire damage every tick, lasting 2 minutes and centered on the ground at Illidan's target.
.' _Draw Soul_ is a _frontal cone_ AoE that deals 5000 Shadow damage and heals Illidan for 100,000 for each target hit.
.' _Parasitic Shadowfiend_ affects a random raid member, applying a debuff that deals 3000 Shadow damage every tick and spawning 2 Parasitic Shadowfiends upon expiration.
.' _Agonizing Flames_ is a 5 yard AoE on a random raid member that deals 5000 Fire damage plus a DoT that deals 36,000 damage over 60 seconds, slightly increasing damage each tick.
'_Phase 2_
.' _Fireball_ deals 3000-4000 Fire damage in a 10 yard radius.
.' _Eye Beam_ puts a trail of blue Demon Fire on the ground. Raid members standing where these land take 20,000 Shadow damage. The fire trails lasts for 1 minute and deals 2000 Fire damage every second to any standing in it.
.' _Dark Barrage_ affects a random raid member, dealing 3000 Shadow damage every second for 10 seconds.
'_Phase 4_
.' _Shadow Blast_ is a 20 yard AoE that hits for 11,000 Shadow damage, centered on the player with highest aggro.
.' _Flame Burst_ deals 3500 Fire damage to all raid members plus additional damage to players closer together than 5 yards.
.' _Shadow Demons_ spawns 4 Shadow Demon adds. They target random raid members, stun them, and then move towards that player.
.' _Aura of Dread_ is a 15 yard AoE, centered on Illidan, dealing 1000 Shadow damage every second and increasing Shadow damage taken by 30%.
|modelnpc Illidan Stormrage##22917
|confirm
step
.' Congratulations, you have finished the Black Temple raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\Naxxramas",[[
dungeon 535
description This guide will walk you through the Naxxramas Raid.
author support@zygorguides.com
step
.' This raid has 5 different sections. You can start at any of them.
.' Click here to start in the Arachnid Quarter |next "arachnidstart" |confirm
.' Click here to start in the Plague Quarter |next "plaguestart" |confirm
.' Click here to start in the Military Quarter |next "militarystart" |confirm
.' Click here to start in the Construct Quarter |next "constructstart" |confirm
.' Click here to start in Frostwyrm Lair |next "frostwyrmstart" |confirm
step
label "arachnidstart"
map Naxxramas/2
path loop off
path	33.2,74.6	33.1,65.1	30.7,56.7
.' Follow the path, clearing trash as you make your way to the first arachnid boss, Anub'Rekhan. |goto 30.7,56.7 <5 |noway |c
step
goto 30.7,43.9
'_Anub'Rekhan_
.' Click here for 10 man abilities |next "anub10" |confirm
.' Click here for 25 man abilities |next "anub25" |confirm
step
label "anub10"
goto 30.7,43.9
'_Anub'Rekhan 10 man_
.' _Impale_ is an AoE that hits a random raid member and everyone in a straight line in between them dealing 4800-6100 damage, then launching all affected players into the air and dealing 50% fall damage.
.' _Locus Swarm_ Anub'Rekhan slows to 60% normal movement speed and deals AoE damage to all those within 30 yards around him as he moves.  Remaining within range of the AoE applies a stacking DoT that deals 1100 Nature damage every 2 seconds, and silences the player. This also spawns a Crypt Guard add.
.' _Summon Corpse Scarabs_ occurs whenever there is a corpse in the room (either a player or a Crypt Guard), Anub'Rekhan spawns Corpse Scarabs from it. 10 spawn from a Crypt Guard corpse and 5 from a player corpse, moving with threat toward a random player.
|modelnpc Anub'Rekhan##15956
|next "anubcomplete" |confirm
step
label "anub25"
goto 30.7,43.9
'_Anub'Rekhan 25 man_
.' _Impale_ is an AoE that hits a random raid member and everyone in a straight line in between them dealing 5700-7300 damage, then launching all affected players into the air and dealing 50% fall damage.
.' _Locus Swarm_ Anub'Rekhan slows to 75% normal movement speed and deals AoE damage to all those within 30 yards around him as he moves.  Remaining within range of the AoE applies a stacking DoT that deals 1500 Nature damage every 2 seconds, and silences the player. This also spawns a Crypt Guard add.
.' _Summon Corpse Scarabs_ occurs whenever there is a corpse in the room (either a player or a Crypt Guard), Anub'Rekhan spawns Corpse Scarabs from it. 10 spawn from a Crypt Guard corpse and 5 from a player corpse, moving with threat toward a random player.
.' _Enrage_  occurs after 10 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Anub'Rekhan##15956
|next "anubcomplete" |confirm
step
label "anubcomplete"
map Naxxramas/2
path loop off
path	32.6,64.0	39.2,65.1	44.9,56.8
path	50.0,50.4	52.2,40.9	49.0,36.4
.' Follow the path, clearing trash as you make your way to the next boss, Grand Widow Faerlina. |goto 49.0,36.4 <5 |noway |c
step
goto 44.5,36.0
'_Grand Widow Faerlina_
.' Click here for 10 man abilities. |next "widow10" |confirm
.' Click here for 25 man abilities. |next "widow25" |confirm
step
label "widow10"
goto 44.5,36.0
'_Grand Widow Faerlina 10 man_
.' _Poison Bolt Volley_ hits the 10 closest players to Faerlina, dealing 2600-3300 Nature damage and applies a DoT that deals 1500 Nature damage every 2 seconds for 8 seconds.
.' _Rain of Fire_ is cast on random raid members during the fight. It deals 1750-275 Fire damage every 2 seconds and lasts 6 seconds.
.' _Frenzy_ occurs every 60-80 seconds, increasing  Faerlina's damage by 150%, attack speed by 50%, and size by 50%.
|modelnpc Grand Widow Faerlina##15953
|next "widowcomplete" |confirm
step
label "widow25"
goto 44.5,36.0
'_Grand Widow Faerlina 25 man_
.' _Poison Bolt Volley_ hits the 10 closest players to Faerlina, dealing 3300-4200 Nature damage and applies a DoT that deals 2000 Nature damage every 2 seconds for 8 seconds.
.' _Rain of Fire_ is cast on random raid members during the fight. It deals 3700-4300 Fire damage every 2 seconds and lasts 6 seconds.
.' _Frenzy_ occurs every 60-80 seconds, increasing  Faerlina's damage by 150%, attack speed by 50%, and size by 50%.
|modelnpc Grand Widow Faerlina##15953
|next "widowcomplete" |confirm
step
label "widowcomplete"
map Naxxramas/2
path loop off
path	52.1,41.7	53.7,50.0	59.3,54.8
path	59.5,67.2	64.3,70.6	67.7,60.6
path	67.9,51.9	64.0,35.9	62.7,29.5
path	64.7,25.1
.' Follow the path, clearing trash as you make your way to the next boss, Maexxna. |goto 64.7,25.1 <5 |noway |c
step
goto 69.8,16.2
'_Maexxna_
.' Click here for 10 man abilities. |next "maexxna10" |confirm
.' Click here for 25 man abilities. |next "maexxna25" |confirm
step
label "maexxna10"
goto 69.8,16.2
'_Maexxna 10 man_
.' _Web Wrap_ occurs after 20 seconds, and then every 40 seconds after that. Sends a random raid member flying towards the webs on the western side of the wall, encasing and stunning them in a cocoon. The cocoons must be killed by other raid members to release the trapped member. When trapped inside, the player takes 2500-3000 Nature damage every 2 seconds.
.' _Web Spray_ occurs every 40 seconds, stunning everyone in the room for 6 seconds, dealing 1700-2200 Nature damage.
.' _Poison Shock_ deals 3500-4500 Nature damage in a _frontal cone_. Make sure the tank is the only one in front of Maexxna when this occurs,
.' _Necrotic Poison_ is a debuff applied to raid members in front of Maexxna that reduces healing by 75% for 30 seconds.
.' _Frenzy_ occurs at 30%, increasing damage by 50%, attack speed by 50%, and size by 15%.
|modelnpc Maexxna##15952
|next "maexxnacomplete" |confirm
step
label "maexxna25"
goto 69.8,16.2
'_Maexxna 25 man_
.' _Web Wrap_ occurs after 20 seconds, and then every 40 seconds after that. Sends a random raid member flying towards the webs on the western side of the wall, encasing and stunning them in a cocoon. The cocoons must be killed by other raid members to release the trapped member. When trapped inside, the player takes 2500-3000 Nature damage every 2 seconds.
.' _Web Spray_ occurs every 40 seconds, stunning everyone in the room for 6 seconds, dealing 2100-2800 Nature damage.
.' _Poison Shock_ deals 4500-4580 Nature damage in a _frontal cone_. Make sure the tank is the only one in front of Maexxna when this occurs,
.' _Necrotic Poison_ is a debuff applied to raid members in front of Maexxna that reduces healing by 90% for 30 seconds.
.' _Frenzy_ occurs at 30%, increasing damage by 75%, attack speed by 50%, and size by 15%.
|modelnpc Maexxna##15952
|next "maexxnacomplete" |confirm
step
label "maexxnacomplete"
.' You have cleared the Arachnid Quarter!
.' Click here to start the Plague Quarter |next "plaguestart" |confirm
.' Click here to start the Military Quarter |next "militarystart" |confirm
.' Click here to start the Construct Quarter |next "constructstart" |confirm
.' Click here to start Frostwyrm Lair |next "frostwyrmstart" |confirm
.' Click here if you've cleared the whole raid |next "raidend" |confirm
step
label "plaguestart"
map Naxxramas/4
path loop off
path	33.0,22.8	35.6,28.6	33.5,37.6
path	33.5,50.0
.' Follow the path, clearing trash as you make your way to the first boss, Noth the Plaguebringer. |goto 33.5,50.0 <5 |noway |c
step
goto 34,54
'_Noth the Plaguebringer_
.' Click here for 10 man abilities. |next "noth10" |confirm
.' Click here for 25 man abilities. |next "noth25" |confirm
step
label "noth10"
goto 34,54
'_Noth the Plaguebringer 10 man_
'_Phase 1_
.' _Curse of the Plaguebringer_ affects 3 random raid members. If the curse is not removed within 10 seconds it will inflict _Wrath of the Plaguebringer_.
.' _Wrath of the Plaguebringer_ for every missed curse, all players within 30 yards take 3700-4300 Shadow damage and 1500 Shadow damage every 2 seconds for 10 seconds.
.' _Summon Plagued Warriors_ occurs every 30 seconds, Noth spawns 2 Plagued Warrior adds. They have a _cleave_.
.' _Berserk_ occurs after 3 skeleton phases. It increases his damage by 1000%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Phase 2_
.' Every 110 seconds Noth teleports away and becomes invulnerable while waves of skeletons attack the raid. This lasts 70 seconds.
.' _Plagued Champions:_ use _mortal strike_, which is a _cleave_.
.' _Plagued Guardians:_ use _Arcane Explosion_, which deals 4000 damage to all raid members within 30 yards.
|modelnpc Noth the Plaguebringer##15954
|next "nothcomplete" |confirm
step
label "noth25"
goto 34,54
'_Noth the Plaguebringer 25 man_
'_Phase 1_
.' _Curse of the Plaguebringer_ affects 10 random raid members. If the curse is not removed within 10 seconds it will inflict _Wrath of the Plaguebringer_ on them and all allies within 30 yards.
.' _Wrath of the Plaguebringer_ for every missed curse, all players within 30 yards take 5500-6500 Shadow damage and 3500 Shadow damage every 2 seconds for 10 seconds.
.' _Blink_ Noth teleports, completely wiping aggro.
.' _Cripple_ occurs after a _Blink_, affect players in the area he teleports from, slowing their movement speed by 50%, attack speed by 100%, and strength by 50%.
.' _Summon Plagued Warriors_ occurs every 30 seconds, Noth spawns 2 Plagued Warrior adds. They have a _cleave_.
.' _Berserk_ occurs after 3 skeleton phases. It increases his damage by 1000%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Phase 2_
.' Every 110 seconds Noth teleports away and becomes invulnerable while waves of skeletons attack the raid. This lasts 70 seconds.
.' _Plagued Champions:_ use _mortal strike_, which is a _cleave_.
.' _Plagued Guardians:_ use _Arcane Explosion_, which deals 6000 damage to all raid members within 30 yards.
|modelnpc Noth the Plaguebringer##15954
|next "nothcomplete" |confirm
step
label "nothcomplete"
map Naxxramas/4
path loop off
path	39.3,56.8	43.6,35.7	50.0,35.6
.' Follow the path, clearing trash as you make your way to the next boss, Heigan the Unclean. |goto 50.0,35.6 <5 |noway |c
step
goto 51.1,43.2
'_Heigan the Unclean_
.' Click here for 10 man abilities. |next "heigan10" |confirm
.' Click here for 25 man abilities. |next "heigan25" |confirm
step
label "heigan10"
goto 51.1,43.2
'_Heigan the Unclean 10 man_
'_Phase 1_
.' _Spell Disruption_ is a 30 yard AoE aura around Heigan. It increases cast time by 300% to anyone inside it.
.' _Decrepit Fever_ is applied to a random raid member, dealing 4500 Nature damage every 3 seconds and reduces the max health of all players within 20 yards by 50%.
'_Phase 2_
.' Occurs after 90 seconds Heigan teleports to the middle of the raised platform.
.' _Plague Cloud_ deals 7500 Nature damage every second to anyone on the raised platform.
.' _Eruption_ affects 3/4 of the remaining area of the room. It deals 6500-7300 Nature damage each hit.
|modelnpc Heigan the Unclean##15936
|next "heigancomplete" |confirm
step
label "heigan25"
goto 51.1,43.2
'_Heigan the Unclean 25 man_
'_Phase 1_
.' _Spell Disruption_ is a 20 yard AoE aura around Heigan. It increases cast time by 300% to anyone inside it.
.' _Decrepit Fever_ is applied to a random raid member, dealing 3000 Nature damage every 3 seconds and reduces the max health of all players within 20 yards by 50%.
'_Phase 2_
.' Occurs after 90 seconds Heigan teleports to the middle of the raised platform.
.' _Plague Cloud_ deals 7500 Nature damage every second to anyone on the raised platform.
.' _Eruption_ affects 3/4 of the remaining area of the room. It deals 6500-7300 Nature damage each hit.
|modelnpc Heigan the Unclean##15936
|next "heigancomplete" |confirm
step
label "heigancomplete"
map Naxxramas/4
path loop off
path	53.9,45.9	58.7,28.5	71.2,28.4
.' Follow the path, clearing trash as you make your way to the last plague boss, Loatheb. |goto 71.2,28.4 <5 |noway |c
step
goto 75.0,28.5
'_Loatheb_
.' Click here for 10 man abilities. |next "loa10" |confirm
.' Click here for 25 man abilities. |next "loa25" |confirm
step
label "loa10"
goto 75.0,28.5
'_Loatheb 10 man_
.' _Necrotic Aura_ occurs every 20 seconds, reducing the healing done by the raid by 100% for 17 seconds.
.' _Deathblossom_ occurs every 30 seconds, affecting the entire raid. It deals 200 Nature damage every second for 6 seconds, and then deals 1200 damage.
.' _Inevitable Doom_ occurs after 2 minutes of combat. Every 30 seconds it inflicts 4000 Shadow damage.
.' _Fungal Creep_ is a buff that increases critical chance by 50% and spells and abilities cause no threat. This buff lasts 90 seconds. Every 30 seconds more Spores are spawned. Killing these allow 5 raid members to get the buff.
|modelnpc Loatheb##16011
|next "loacomplete" |confirm
step
label "loa25"
goto 75.0,28.5
'_Loatheb 25 man_
.' _Necrotic Aura_ occurs every 20 seconds, reducing the healing done by the raid by 100% for 17 seconds.
.' _Deathblossom_ occurs every 30 seconds, affecting the entire raid. It deals 400 Nature damage every second for 6 seconds, and then deals 1500 damage.
.' _Inevitable Doom_ occurs after 2 minutes of combat. Every 30 seconds it inflicts 5000 Shadow damage.
.' _Fungal Creep_ is a buff that increases critical chance by 50% and spells and abilities cause no threat. This buff lasts 90 seconds. Every 20 seconds more Spores are spawned. Killing these allow 5 raid members to get the buff.
|modelnpc Loatheb##16011
|next "loacomplete" |confirm
step
label "loacomplete"
.' You have cleared the Plague Quarter!
.' Click here to start the Arachnid Quarter |next "arachnidstart" |confirm
.' Click here to start the Military Quarter |next "militarystart" |confirm
.' Click here to start the Construct Quarter |next "constructstart" |confirm
.' Click here to start Frostwyrm Lair |next "frostwyrmstart" |confirm
.' Click here if you've cleared the whole raid |next "raidend" |confirm
step
label "militarystart"
map Naxxramas/3
path loop off
path	64.6,25.1	55.1,32.4	49.7,44.8
path	52.8,44.1	42.7,39.3
.' Follow the path, clearing trash as you make your way to the first boss, Instructor Razuvious. |goto 42.7,39.3 <5 |noway |c
step
goto 42.7,44
'_Instructor Razuvious_
.' Click here for 10 man abilities. |next "raz10" |confirm
.' Click here for 25 man abilities. |next "raz25" |confirm
step
label "raz10"
goto 42.7,44
'_Instructor Razuvious 10 man_
.' _Disrupting Shout_ occurs about every 15 seconds. It is an AoE that deals 4200-4700 Physical damage to the whole raid.
.' _Jagged Knife_ occurs every 10 seconds and affects a random raid member. It deals 5000 Physical damage and applies a DoT which deals 10,000 damage over 5 seconds.
.' _Unbalancing Strike_ inflicts 350% weapon damage and reduces the target's defense skill by 100 for 6 seconds. Only the _Death Knight Understudies_ should be hit with this, as it deals about 100,000 damage.
|modelnpc Instructor Razuvious##16061
|next "razcomplete" |confirm
step
label "raz25"
goto 42.7,44
'_Instructor Razuvious 25 man_
.' _Disrupting Shout_ occurs about every 15 seconds. It is an AoE that deals 7100-7800 Physical damage to the whole raid.
.' _Jagged Knife_ occurs every 10 seconds and affects a random raid member. It deals 5000 Physical damage and applies a DoT which deals 10,000 damage over 5 seconds.
.' _Unbalancing Strike_ inflicts 350% weapon damage and reduces the target's defense skill by 100 for 6 seconds. Only the _Death Knight Understudies_ should be hit with this, as it deals about 100,000 damage.
|modelnpc Instructor Razuvious##16061
|next "razcomplete" |confirm
step
label "razcomplete"
map Naxxramas/3
path loop off
path	42.5,51.6	54.2,50.1	57.1,43.6
path	66.4,44.7
.' Follow the path, clearing trash as you make your way to the next boss, Gothik the Harvester. |goto 66.4,44.7 <5 |noway |c
step
goto 66.9,58.2
'_Gothik the Harvester_
.' Click here for 10 man abilities. |next "got10" |confirm
.' Click here for 25 man abilities. |next "got25" |confirm
step
label "got10"
goto 66.9,58.2
'_Gothik the Harvestor 10 man_
'_Phase 1_
.' Adds spawn on both sides of the room and Grothik is invulnerable. After 4 minutes and 34 seconds he comes down and is attackable.
'_Phase 2_
.' _Shadowbolt_ hits Grothik's target for 2800-3500 Shadow damage.
.' _Harvest Soul_ occurs every 15 seconds. It reduces the stats the entire raid by 10%. It can stack up to 10 times.
|modelnpc Gothik the Harvester##16060
|next "gotcomplete" |confirm
step
label "got25"
goto 66.9,58.2
'_Gothik the Harvestor 25 man_
'_Phase 1_
.' Adds spawn on both sides of the room and Grothik is invulnerable. After 4 minutes and 34 seconds he comes down and is attackable.
'_Phase 2_
.' _Shadowbolt_ hits Grothik's target for 4500-5500 Shadow damage.
.' _Harvest Soul_ occurs every 15 seconds. It reduces the stats the entire raid by 10%. It can stack up to 10 times.
|modelnpc Gothik the Harvester##16060
|next "gotcomplete" |confirm
step
label "gotcomplete"
map Naxxramas/3
path loop off
path	62.4,62.3	58.5,72.3	50.2,72.4
path	41.9,67.2	36.0,68.1
.' Follow the path, clearing trash as you make your way to the last boss,the Four Horsemen. |goto 36.0,68.1 <5 |noway |c
step
goto 30,75
'_The Four Horseman_
.' Click here for 10 man abilities. |next "four10" |confirm
.' Click here for 25 man abilities. |next "four25" |confirm
step
label "four10"
goto 30,75
'_The Four Horsemen 10 man_
'_Thane Korth'azz_
.' _Meteor_ deals 13,750-15,250 Fire damage split between all players in an 8 yard radius.
.' _Mark of Korth'azz_ is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
'_Baron Rivendare_
.' _Unholy Shadow_ deals 2100-2600 Shadow and a DoT that deals 4800 Shadow damage over 8 seconds.
.' _Mark of Rivendare_is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
'_Lady Blaumeux_
.' _Shadow Bolt_ deals 2500 Shadow damage to the closest player.
.' _Void Zone_ summons a void zone on a random raid member within 45 yards. It deals 2750 Shadow damage every second to anyone in it.
.' _Mark of Blaumeux_is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
.' _Unyielding Pain_ occurs if no player with aggro is within 55 yards of her. She will spam this ability, dealing 6000 Shadow damage to the entire raid.
'_Sir Zeliek_
.' _Holy Blast_ deals 2500 Holy damage to the closest player.
.' _Mark of Zeliek_is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
.' _Condemnation_ occurs if no player with aggro is within 55 yards. It deals 6000 damage to the entire raid.
|next "fourcomplete" |confirm
step
label "four25"
goto 30,75
'_The Four Horsemen 25 man_
'_Thane Korth'azz_
.' _Meteor_ deals 47,500-52,500 Fire damage split between all players in an 8 yard radius.
.' _Mark of Korth'azz_ is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
'_Baron Rivendare_
.' _Unholy Shadow_ deals 4500-5500 Shadow and a DoT that deals 12,000 Shadow damage over 8 seconds.
.' _Mark of Rivendare_is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
'_Lady Blaumeux_
.' _Shadow Bolt_ deals 8500-9500 Shadow damage to the closest player.
.' _Void Zone_ summons a void zone on a random raid member within 45 yards. It deals 4200 Shadow damage every second to anyone in it.
.' _Mark of Blaumeux_is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
.' _Unyielding Pain_ occurs if no player with aggro is within 55 yards of her. She will spam this ability, dealing 6000 Shadow damage to the entire raid.
'_Sir Zeliek_
.' _Holy Blast_ deals 9000-10,000 Holy damage to the closest player.
.' _Mark of Zeliek_is a stacking debuff which increases in damage every stack. Players should be rotated between bosses to avoid their debuffs stacking too high.
.' _Condemnation_ occurs if no player with aggro is within 55 yards. It deals 6000 damage to the entire raid.
|next "fourcomplete" |confirm
step
label "fourcomplete"
.' You have cleared the Military Quarter!
.' You can use the Naxxramas Portal here [Naxxramas/3 27.9,80.4] to return to the start of the raid.
.' Click here to start the Arachnid Quarter |next "arachnidstart" |confirm
.' Click here to start the Plague Quarter |next "plaguestart" |confirm
.' Click here to start the Construct Quarter |next "constructstart" |confirm
.' Click here to start Frostwyrm Lair |next "frostwyrmstart" |confirm
.' Click here if you've cleared the whole raid |next "raidend" |confirm
step
label "constructstart"
map Naxxramas/1
path loop off
path	68.2,77.0	63.8,70.3	56.8,66.3
path	52.6,70.5	49.5,73.3	47.3,61.3
.' Follow the path, clearing trash as you make your way to the first boss, Patchwerk. |goto 47.3,61.3 <5 |noway |c
step
goto 50.9,54.6
'_Patchwerk_
.' Click here for 10 man abilities. |next "werk10" |confirm
.' Click here for 25 man abilities. |next "werk25" |confirm
step
label "werk10"
goto 53.8,50.3
'_Patchwerk 10 man_
.' _Hateful Strike_ melee attack that deals 20,000-27,000 Physical damage. Also adds threat to the three players with the highest aggro.
.' _Frenzy_ occurs at 5%, increasing his damage by 25% and attack speed by 40%.
.' _Berserk_ occurs after 6 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Patchwerk##16028
|next "werkcomplete" |confirm
step
label "werk25"
goto 53.8,50.3
'_Patchwerk 25 man_
.' _Hateful Strike_ melee attack that deals 80,000 Physical damage to 2 raid members. Also adds threat to the three players with the highest aggro.
.' _Frenzy_ occurs at 5%, increasing his damage by 25% and attack speed by 40%.
.' _Berserk_ occurs after 6 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Patchwerk##16028
|next "werkcomplete" |confirm
step
label "werkcomplete"
map Naxxramas/1
path loop off
path	55.0,40.7	59.7,46.5
.' Follow the path, clearing trash as you make your way to the next boss,. |goto 59.7,46.5 <5 |noway |c
step
goto 63.5,52.7
'_Grobbulus_
.|tip He walks up and down this ramp, you may have to wait for him.
.' Click here for 10 man abilities. |next "grob10" |confirm
.' Click here for 25 man abilities. |next "grob25" |confirm
step
label "grob10"
goto 63.5,52.7
'_Grobbulus 10 man_
.' _Poison Cloud_ are dropped below Grobbulus every 15 seconds. They expand over time and last 75 seconds. They deal 2000 Nature damage every second.
.' _Slime Spray_ deals 6300-7700 Nature damage to enemie in front of Grobbulus in a 45 _cone_. Make sure the tank is the only one in front of Grobbulus when this occurs. Players hit by this summon a Fallout Slime add.
.' _Mutating Injection_ is cast on a random raid member. After 10 seconds, or if cleansed, it deals 8000-9000 Nature damage to everyone within 20 yards.
.' _Berserk_ occurs after 12 minutes of combat. It increases his damage by 1000%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Grobbulus##15931
|next "grobcomplete" |confirm
step
label "grob25"
goto 63.5,52.7
'_Grobbulus 25 man_
.' _Poison Cloud_ are dropped below Grobbulus every 15 seconds. They expand over time and last 75 seconds. They deal 4250 Nature damage every second.
.' _Slime Spray_ deals 12,000-13,000 Nature damage to enemie in front of Grobbulus in a 45 _cone_. Make sure the tank is the only one in front of Grobbulus when this occurs. Players hit by this summon a Fallout Slime add.
.' _Mutating Injection_ is cast on a random raid member. After 10 seconds, or if cleansed, it deals 8000-9000 Nature damage to everyone within 20 yards.
.' _Berserk_ occurs after 9 minutes of combat. It increases his damage by 1000%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Grobbulus##15931
|next "grobcomplete" |confirm
step
label "grobcomplete"
map Naxxramas/1
path loop off
path	66.9,53.0	60.9,57.5	60.0,60.8
path	58.0,61.4	55.2,57.1	50.3,49.8
.' Follow the path, clearing trash as you make your way to the next boss, Gluth. He will aggro as soon as you enter the room. |goto 50.3,49.8 <5 |noway |c
step
goto 48.1,46.8
'_Gluth_
.' Click here for 10 man abilities. |next "gluth10" |confirm
.' Click here for 25 man abilities. |next "gluth25" |confirm
step
label "gluth10"
goto 48.1,46.8
'_Gluth 10 man_
.' _Mortal Wound_ applies a stacking debuff that reduces the effectiveness of healing by 10% for 15 seconds.
.' _Enrage_ increases Gluth's attack speed and damage by 25% for 8 seconds.
.' _Decimate_ reduces the health of all nearby targets (enemies and friendlies) to 5%.
.' _Devour Zombie_ Gluth moves towards any zombies that get near him. They heal him for 5%.
.' _Berserk_ occurs after 8 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Gluth##15932
|next "gluthcomplete" |confirm
step
label "gluth25"
goto 48.1,46.8
'_Gluth 25 man_
.' _Mortal Wound_ applies a stacking debuff that reduces the effectiveness of healing by 10% for 15 seconds.
.' _Enrage_ increases Gluth's attack speed by 100%.
.' _Decimate_ reduces the health of all nearby targets (enemies and friendlies) to 5%.
.' _Devour Zombie_ Gluth moves towards any zombies that get near him. They heal him for 5%.
.' _Berserk_ occurs after 8 minutes of combat. It increases his damage by 1000%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Gluth##15932
|next "gluthcomplete" |confirm
step
label "gluthcomplete"
map Naxxramas/1
path loop off
path	42.5,38.6	34.9,27.2
.' Follow the path, clearing trash as you make your way to the next boss, Thaddius. |goto 34.9,27.2 <5 |noway |c
step
goto 29,19
'_Thaddius_
.' Click here for 10 man abilities. |next "thaddius10" |confirm
.' Click here for 25 man abilities. |next "thaddius25" |confirm
step
label "thaddius10"
goto 26.9,14.9
.' _10 man_
'_Feugen_
.' _Static Field_ deals 2500 Nature damage every 3 seconds to anyone within 60 yards.
.' _Magnetic Pull_ pulls Stalagg's tank to Feugen every 30 seconds and switches their threat.
'_Stalagg_
.' _Power Surge_ increases Stalagg's attack speed by 200% for 10 seconds.
.' _Magnetic Pull_ pulls Feugen's tank to Stalagg every 30 seconds and switches their threat.
'_Thaddius_
.' _Chain Lightning_ hits 3 random raid members for 3600-4400 Nature damage.
.' _Polarity Shift_ charges the entire raid either negatively or positively, split roughly half and half. When standing on another player with the same charge a 10% damage buff is gained. When standing on people with opposite charges, both deal 3500 damage to each other.
.' _Ball Lightning_ occurs if no one is in melee range of Thaddius, it deals 17,500-22,500 Nature damage.
.' _Enrage_ occurs after 6 minutes of combat. It increases his damage by 1000%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Thaddius##15928
|next "thaddiuscomplete" |confirm
step
label "thaddius25"
goto 26.9,14.9
.' _25 man_
'_Feugen_
.' _Static Field_ deals 3500 Nature damage every 3 seconds to anyone within 60 yards.
.' _Magnetic Pull_ pulls Stalagg's tank to Feugen every 30 seconds and switches their threat.
'_Stalagg_
.' _Power Surge_ increases Stalagg's attack speed by 200% for 10 seconds.
.' _Magnetic Pull_ pulls Feugen's tank to Stalagg every 30 seconds and switches their threat.
'_Thaddius_
.' _Chain Lightning_ hits 5 random raid members for 7000-8000 Nature damage.
.' _Polarity Shift_ charges the entire raid either negatively or positively, split roughly half and half. When standing on another player with the same charge a 10% damage buff is gained. When standing on people with opposite charges, both deal 4500 damage to each other.
.' _Ball Lightning_ occurs if no one is in melee range of Thaddius, it deals 17,500-22,500 Nature damage.
.' _Enrage_ occurs after 6 minutes of combat. It increases his damage by 1000%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Thaddius##15928
|next "thaddiuscomplete" |confirm
step
label "thaddiuscomplete"
.' You have cleared the Construct Quarter!
.' You can use the Naxxramas Portal here [Naxxramas/1 27.2,10.7] to return to the start of the raid.
.' Click here to start the Arachnid Quarter |next "arachnidstart" |confirm
.' Click here to start the Plague Quarter |next "plaguestart" |confirm
.' Click here to start the Military Quarter |next "militarystart" |confirm
.' Click here to start Frostwyrm Lair |next "frostwyrmstart" |confirm
.' Click here if you've cleared the whole raid |next "raidend" |confirm
step
label "frostwyrmstart"
goto Naxxramas/5 53.4,50.1 |n
.' Click the Orb of Naxxramas to teleport to the Upper Necropolis. |goto Naxxramas/6 73.6,72.8 <5 |noway |c
step
goto 59,68
'_Sapphiron_
.' Click here for 10 man abilities. |next "sapp10" |confirm
.' Click here for 25 man abilities. |next "sapp25" |confirm
step
label "sapp10"
goto 59,68
'_Sapphiron 10 man_
'_Phase 1_
.' He has a _cleave_ and _tailslap_. Raid members need to stay from the front and back of him to avoid damage from these.
.' _Frost Aura_ hits the entire raid for 1200 Frost damage shortly after she is engaged. It continues to do damage every 2 seconds until she is slain.
.' _Life Drain_ is a debuff that affect two random raid members. It drains 2100-2800 health as Shadow damage and heals Sapphiron for 4300-5600 every 3 seconds.
.' _Chill_ is an ice storm that moves around the room while Sapphiron is engaged. It deals 3500-4500 Frost damage every 2 seconds and slows their movement speed by 50% for 10 seconds, affecting everyone within it.
'_Phase 2_
.' Phase 2 starts after 45 seconds, Sapphiron takes to the air, knocking back all players within melee range.
.' _Ice Bolt_ affects two players per air phase. Each player is encased in ice, stunning them until the ice shatters, dealing 2500-3400 Frost damage to them and 15,000 damage to nearby players within 5 yards.
.' _Frost Breath_ occurs after Sapphiron uses "Ice Bolt" twice. Sapphiron starts casting, taking 7 seconds to complete the cast. Once casted, it deals 150,000 Frost damage to everyone in the room. This can be avoided by taking cover behind the ice blocks.
.' _Enrage_ occurs after 15 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Sapphiron##15989
|next "sappquest" |confirm
step
label "sapp25"
goto 59,68
'_Sapphiron 25 man_
'_Phase 1_
.' He has a _cleave_ and _tailslap_. Raid members need to stay from the front and back of him to avoid damage from these.
.' _Frost Aura_ hits the entire raid for 1600 Frost damage shortly after she is engaged. It continues to do damage every 2 seconds until he is slain.
.' _Life Drain_ is a debuff that affect two random raid members. It drains 4500-5500 health as Shadow damage and heals Sapphiron for 9000-11,000 every 3 seconds for 12 seconds.
.' _Chill_ is an ice storm that moves around the room while Sapphiron is engaged. It deals 5500-6500 Frost damage every 2 seconds and slows their movement speed by 50% for 10 seconds, affecting everyone within it.
'_Phase 2_
.' Phase 2 starts after 45 seconds, Sapphiron takes to the air, knocking back all players within melee range.
.' _Ice Bolt_ affects two players per air phase. Each player is encased in ice, stunning them until the ice shatters, dealing 6300-7700 Frost damage to them and all nearby players.
.' _Frost Breath_ occurs after Sapphiron uses "Ice Bolt" twice. Sapphiron starts casting, taking 7 seconds to complete the cast. Once casted, it deals 150,000 Frost damage to everyone in the room. This can be avoided by taking cover behind the ice blocks.
.' _Enrage_ occurs after 15 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Sapphiron##15989
|next "sappquest" |confirm
step
label "sappquest"
.' Sapphiron may have dropped you a quest item.
.' Click the Key to the Focusing Iris in your bags |use Key to the Focusing Iris##44569
..accept 13372 |or
.' Click the Heroic Key to the Focusing Iris in your bags |use Heroic Key to the Focusing Iris##44577
..accept 13375 |or
|next "sappcomplete"
step
label "sappcomplete"
map Naxxramas/6
path loop off
path	45.0,64.0	35.7,55.0	34.2,41.0
.' Follow the path to the last boss, Kel'Thuzad. |goto 34.2,41.0 <5 |noway |c
step
goto 36,25
'_Kel'Thuzad_
.' Click here for 10 man abilities. |next "kel10" |confirm
.' Click here for 25 man abilities. |next "kel25" |confirm
step
label "kel10"
goto 36,25
'_Kel'Thuzad 10 man_
'_Phase 1_
.' Kel'Thuzad is invulnerable. You must kill the adds he spawns.
'_Phase 2_
.' Begins after 3 minutes and 48 seconds.
.' _Frost Bolt (single)_ inflicts 10,000-13,000 Frost damage and reduces movement speed by 50% for 4 seconds.
.' _Frost Bolt (multiple)_ hits the entire raid, inflicting 4500-5500 Frost damage and reducing their movement speed for 4 seconds.
.' _Mana Detonation_ is a debuff applied to a random mana-user. The player will explode after 5 seconds, dealing Arcane damage to all players within 10 yards based on their maximum mana.
.' _Shadow Fissure_ summons a red fissure around the targeted player which deals 62,000-137,500 Shadow damage after 5 seconds.
.' _Frost Blast_ places an Ice Block on the targeted player and all players within 10 yards of them, stunning them. This effect will continue to chain to additional players within 10 yards. It deals 104% of their maximum health as damage over 4 seconds. If they are not healed in that time they will die.
'_Phase 3_
.' Occurs at 45% health.
.' 2 Guardians of Ice Crown adds are summoned to attack the raid, but despawn when Kel'Thuzad dies.
|modelnpc Kel'Thuzad##15990
|next "kelcomplete" |confirm
step
label "kel25"
goto 36,25
'_Kel'Thuzad 25 man_
'_Phase 1_
.' Kel'Thuzad is invulnerable. You must kill the adds he spawns.
'_Phase 2_
.' Begins after 3 minutes and 48 seconds.
.' _Frost Bolt (single)_ inflicts 30,000 Frost damage and reduces movement speed by 50% for 4 seconds. This must be interrupted.
.' _Frost Bolt (multiple)_ hits the entire raid, inflicting 7000-8800 Frost damage and reducing their movement speed for 4 seconds.
.' _Mana Detonation_ is a debuff applied to a random mana-user. The player will explode after 5 seconds, dealing Arcane damage to all players within 10 yards based on their maximum mana.
.' _Shadow Fissure_ summons a red fissure around the targeted player which deals 62,000-137,500 Shadow damage after 5 seconds.
.' _Frost Blast_ places an Ice Block on the targeted player and all players within 10 yards of them, stunning them. This effect will continue to chain to additional players within 10 yards. It deals 104% of their maximum health as damage over 4 seconds. If they are not healed in that time they will die.
.' _Chains of Kel'Thuzad_ mind controls up to 3 raid members, increasing their damage by 200% and healing by 500%. This lasts 20 seconds and they will buff and heal Kel'Thuzad.
'_Phase 3_
.' Occurs at 45% health.
.' 2 Guardians of Ice Crown adds are summoned to attack the raid, but despawn when Kel'Thuzad dies.
|modelnpc Kel'Thuzad##15990
|next "kelcomplete" |confirm
step
label "kelcomplete"
.' You have cleared Frostwyrm Lair!
.' Click here to start the Arachnid Quarter |next "arachnidstart" |confirm
.' Click here to start the Plague Quarter |next "plaguestart" |confirm
.' Click here to start the Military Quarter |next "militarystart" |confirm
.' Click here to start the Construct Quarter |next "constructstart" |confirm
.' Click here if you've cleared the whole raid |next "raidend" |confirm
step
label "raidend"
goto Dragonblight/0 59.8,54.6
.talk 26917
.|tip She is at the very top of Wyrmrest Temple.
..turnin 13372
|only if havequest(13372)
step
goto Dragonblight/0 59.8,54.6
.talk 26917
.|tip She is at the very top of Wyrmrest Temple.
..turnin 13375
|only if havequest(13375)
step
.' Congratulations, you have finished the Naxxramas raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\Vault of Archavon",[[
dungeon 532
description This guide will walk you through the Vault of Archavon Raid.
author support@zygorguides.com
step
.' In order to enter and complete this raid your faction must control Wintergrasp.
|confirm
step
goto Wintergrasp/0 50.4,16.4 |n
.' Click the Defender's Portal to be teleported to the raid entrance. |goto Wintergrasp/0 50.4,15.9 <5 |noway |c
step
map Vault of Archavon/1
path loop off
path	46.5,55.5	39.9,55.4
.' Follow the path, clearing trash as you make your way to the first boss, Koralon the Flame Watcher. |goto 39.9,55.4 <5 |noway |c
step
goto 35.3,55.4
'_Koralon the Flame Watcher_
.' Click here for 10 man abilities |next "kor10" |confirm
.' Click here for 25 man abilities |next "kor25" |confirm
step
label "kor10"
goto 35.3,55.4
'_Koralon the Flame Watcher 10 man_
.' _Burning Fury_ increases Korlon's damage by 5%. This occurs when Koralon is engaged and stacks every 20 seconds.
.' _Meteor Fists_ inflicts 100-125% weapon damage, splitting Koralon's damage evenly between an additional player within 10 yards of his primary target.
.' _Burning Breath_ inflicts 4000 Fire damage to all targets every second for 3 seconds.
.' _Flaming Cinder_ inflicts 4000 Fire damage and an additional 3000 Fire damage every second to all players within 4 yards of the _Burning Cinder_ placed on the ground.
|modelnpc Koralon the Flame Watcher##35013
|next "korcomplete" |confirm
step
label "kor25"
goto 35.3,55.4
'_Koralon the Flame Watcher 25 man_
.' _Burning Fury_ increases Korlon's damage by 5%. This occurs when Koralon is engaged and stacks every 20 seconds.
.' _Meteor Fists_ inflicts 100-125% weapon damage, splitting Koralon's damage evenly between an additional player within 10 yards of his primary target.
.' _Burning Breath_ inflicts 4000 Fire damage to all targets every second for 3 seconds.
.' _Flaming Cinder_ inflicts 5300-5700 Fire damage and an additional 4300-4600 Fire damage every second to all players within 4 yards of the _Burning Cinder_ placed on the ground.
|modelnpc Koralon the Flame Watcher##35013
|next "korcomplete" |confirm
step
label "korcomplete"
map Vault of Archavon/1
path loop off
path	52.0,55.4	58.7,55.5
.' Follow the path, clearing trash as you make your way to the next boss, Emalon the Storm Watcher. |goto 58.7,55.5 <5 |noway |c
step
goto 62,55
'_Emalon the Storm Watcher_
.' Click here for 10 man abilities |next "emalon10" |confirm
.' Click here for 25 man abilities |next "emalon25" |confirm
step
label "emalon10"
goto 62,55
'_Emalon the Storm Watcher 10 man_
.' _Chain Lightning_ deals 4500-5400 Nature damage, jumping to nearby raid members and increasing the damage dealt by 50% each jump.
.' _Lightning Nova_ is an AoE that deals 21,000-24,000 Nature damage to all raid members within 20 yards of Emalon.
.' _Overcharge_ Emalon overcharged a Tempest Minion, increasing its damage by 20% and stacking up to 10 times. The minion will _explode_ after 10 stacks are reached, dealing 30,000 Nature damage to all players inside the raid. Kill them before this happens to avoid death.
.' _Enrage_ occurs after 6 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Emalon the Storm Watcher##33993
|next "emaloncomplete" |confirm
step
label "emalon25"
goto 62,55
'_Emalon the Storm Watcher 25 man_
.' _Chain Lightning_ deals 7400-8600 Nature damage, jumping to nearby raid members and increasing the damage dealt by 20% each jump.
.' _Lightning Nova_ is an AoE that deals 30,000 Nature damage to all raid members within melee range of Emalon. Damage is reduced with distance, but cannot be outranged in 25 man.
.' _Overcharge_ Emalon overcharged a Tempest Minion, increasing its damage by 20% and stacking up to 10 times. The minion will _explode_ after 10 stacks are reached, dealing 30,000 Nature damage to all players inside the raid. Kill them before this happens to avoid death.
|modelnpc Emalon the Storm Watcher##33993
|next "emaloncomplete" |confirm
step
label "emaloncomplete"
map Vault of Archavon/1
path loop off
path	49.3,40.5	49.3,32.9	49.2,22.8
.' Follow the path, clearing trash as you make your way to the next boss, Archavon the Stone Watcher. |goto 49.2,22.8 <5 |noway |c
step
goto 49,16
'_Archavon the Stone Watcher_
.' Click here for 10 man abilities |next "arch10" |confirm
.' Click here for 25 man abilities |next "arch25" |confirm
step
label "arch10"
goto 49,16
'_Archavon the Stone Watcher 10 man_
.' _Rock Shards_ hits the target with rocks every 0.1 second for 3 seconds, dealing about 7000 damage to them and all nearby raid members.
.' _Stomp_ deals 3700-4300 damage and stuns the target for 2 seconds.
.' _Crushing Leap_ Archavon leaps at a raid member, leaving a trail of white clouds on the ground that deal 2000 damage per tick to those standing in them.
.' _Enrage_ occurs after 5 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Archavon the Stone Watcher##31125
|next "archcomplete" |confirm
step
label "arch25"
goto 49,16
'_Archavon the Stone Watcher 25 man_
.' _Rock Shards_ hits the target with rocks every 0.1 second for 3 seconds, dealing about 7000 damage to them and all nearby raid members.
.' _Stomp_ deals 3700-4300 damage and stuns the target for 2 seconds.
.' _Crushing Leap_ Archavon leaps at a raid member, leaving a trail of white clouds on the ground that deal 2000 damage per tick to those standing in them.
.' _Enrage_ occurs after 5 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Archavon the Stone Watcher##31125
|next "archcomplete" |confirm
step
label "archcomplete"
map Vault of Archavon/1
path loop off
path	52.2,36.6	58.7,36.6
.' Follow the path, clearing trash as you make your way to the last boss, Toravon the Ice Watcher. |goto 58.7,36.6 <5 |noway |c
step
goto 62,36
'_Toravon the Ice Watcher_
.' Click here for 10 man abilities |next "tor10" |confirm
.' Click here for 25 man abilities |next "tor25" |confirm
step
label "tor10"
goto 62,36
'_Toravon the Ice Watcher 10 man_
.' _Frozen Mallet_ Toravon's attacks have a chance to deal Frostbite, inflicting 975-1025 damage every 2 seconds for 10 seconds, stacks up to 100 times.
.' _Frozen Orb_ spawns a Frozen Orb at the targeted location. These deal 3000 damage every second to enemies within 8 yards.
.' _Whiteout_ inflicts 11,500 Frost damage to all nearby raid members and increases Frost damage taken by 25%, stacks up to 20 times.
.' _Freezing Ground_ has a 6 yard range and inflicts 4500-5600 Frost damage, immobilizing those affected for 5 seconds.
|modelnpc Toravon the Ice Watcher##38433
|next "torcomplete" |confirm
step
label "tor25"
goto 62,36
'_Toravon the Ice Watcher 25 man_
.' _Frozen Mallet_ Toravon's attacks have a chance to deal Frostbite, inflicting 2000 damage every 2 seconds for 10 seconds, stacks up to 100 times.
.' _Frozen Orb_ spawns 3 Frozen Orbs at the targeted location. These deal 3000 damage every second to enemies within 8 yards.
.' _Whiteout_ inflicts 13,500 Frost damage to all nearby raid members and increases Frost damage taken by 25%, stacks up to 20 times.
.' _Freezing Ground_ has a 6 yard range and inflicts 4500-5600 Frost damage, immobilizing those affected for 5 seconds.
|modelnpc Toravon the Ice Watcher##38433
|next "torcomplete" |confirm
step
label "torcomplete"
.' Congratulations, you have finished the Vault of Archavon raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\The Obsidian Sanctum",[[
dungeon 531
description This guide will walk you through the Obsidian Sanctum Raid.
author support@zygorguides.com
step
.' Go down into the large fissure, following it underneath Wyrmrest Temple. |goto Dragonblight/0 59.7,49.3 <10 |c
step
goto The Obsidian Sanctum/0 63.5,50.1 |n
.' Enter the raid |goto The Obsidian Sanctum/0 63.5,50.1 <5 |noway |c
step
.' This raid has 10 man and 25 man versions.
.' The abilities and strategies are the same, the only differences being the enemies' increased health and damage in 25 man.
|confirm
step
goto 53.3,47.8
.' The boss of this raid, Sartharion, is found at this location.
.' He has 3 drake lieutenants, Shadron, Tenebron, and Vesperon. You can kill all 3 drakes or leave some combination of them alive. Killing Sartharion with one or more lieutenants alive increase the difficulty and quality of loot that drops.
.' For each lieutenant left alive, the raid will receive a debuff when engaging Sartharion:
.' _Power of Shadron_ increases the raid's Fire damage taken by 100%.
.' _Power of Tenebron_ increases the raid's Shadow damage taken by 100%.
.' _Power of Vesperon_  decreases the raid's maximumm health by 25%.
.' Click here to kill Shadron |next "shadron" |confirm
.' Click here to kill Tenebron |next "tenebron" |confirm
.' Click here to kill Vesperon |next "vesperon" |confirm
.' Click here to kill Sartharion! |next "sarth" |confirm
step
label "shadron"
goto 51.5,34.5
'_Shadron_
.' _Shadow Breath_ is a _fronal cleave_, dealing 7000-8000 Shadow damage (9000-10,000 in heroic). Make sure the tank is the only one in front of him when this occurs.
.' _Shadow Fissure_ creates a shadowy area on the ground, which hits everyone within it for 6000-9000 damage after 5 seconds (9500-13,500 in heroic).
|modelnpc Shadron##30451
.' Click here to kill Tenebron |next "tenebron" |confirm
.' Click here to kill Vesperon |next "vesperon" |confirm
.' Click here to kill Sartharion! |next "sarth" |confirm
step
label "tenebron"
goto 41,48.5
'_Tenebron_
.' _Shadow Breath_ is a _fronal cleave_, dealing 7000-8000 Shadow damage (9000-10,000 in heroic). Make sure the tank is the only one in front of him when this occurs.
.' _Shadow Fissure_ creates a shadowy area on the ground, which hits everyone within it for 6000-9000 damage after 5 seconds (9500-13,500 in heroic).
|modelnpc Tenebron##30452
.' Click here to kill Shadron |next "shadron" |confirm
.' Click here to kill Vesperon |next "vesperon" |confirm
.' Click here to kill Sartharion! |next "sarth" |confirm
step
label "vesperon"
goto 52.5,62.5
'_Vesperon_
.' _Shadow Breath_ is a _fronal cleave_, dealing 7000-8000 Shadow damage (9000-10,000 in heroic). Make sure the tank is the only one in front of him when this occurs.
.' _Shadow Fissure_ creates a shadowy area on the ground, which hits everyone within it for 6000-9000 damage after 5 seconds (9500-13,500 in heroic).
|modelnpc Vesperon##30449
.' Click here to kill Shadron |next "shadron" |confirm
.' Click here to kill Tenebron |next "tenebron" |confirm
.' Click here to kill Sartharion! |next "sarth" |confirm
step
label "sarth"
'_Sartharion_
.' If any of the 3 drake lieutenants are still alive when engaged, the corresponding debuff will be applied to the raid.
.' Sartharion will also call for the lieutenants' assistance, aggroing them and their nearby adds onto the raid.
|confirm
step
goto 53.3,47.8
'_Sartharion_
.' _Cleave_ hits the target and up to 10 nearby raid members. Make sure the tank is the only one in front of Satharion when this occurs.
.' _Tail Lash_ hits all raid members behind Satharion, inflicting 3000-4000 damage (4300-5600 on heroic).
.' _Fire Breath_ inflicts 8700-11,200 Fire damage (11,000-14,000 on heroic) to enemies in a _cone_ in front of Sartharion. Make sure the tank is the only one in front of him when this occurs.
.' _Enrage_ occurs after 15 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Sartharion##28860
|confirm
step
.' Congratulations, you have finished the Obsidian Sanctum raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\The Ruby Sanctum",[[
dungeon 609
description This guide will walk you through the Ruby Sanctum Raid.
author support@zygorguides.com
step
.' Go down into the large fissure, following it underneath Wyrmrest Temple. |goto Dragonblight/0 59.7,49.3 <10 |c
step
goto The Ruby Sanctum/0 49.0,30.9 |n
.' Enter the raid |goto The Ruby Sanctum/0 49.0,30.9 <5 |noway |c
step
.' This raid has 3 mini bosses that must be killed before the final boss, Halion, emerges. The first 2 can be killed in any order, then unlocking access to the third boss.
.' Click here to kill Balharus |next "balth" |confirm
.' Click here to kill Saviana |next "sav" |confirm
step
label "balth"
goto 65.8,53.9
'_Baltharus the Warborn_
.' _Blade Tempest_ is a _cleave_ that deals 70% of weapon damage. Make sure the tank is the only one in front of Baltharus when this occrs.
.' _Repelling Wave_ knocks nearby raid members back, inflicting 4100-4800 Fire damage and stunning them for 3 seconds.
.' _Summon Clone_ occurs when Baltharus is at 50% health. He spawns a clone of himself with the same health.
.' _Enervating Brand_ affects the target and any raid members within 12 yards of them. This occurs every 2 seconds and reduces the affected players' damage by 2% per stack, while increasing Baltharus' damage simultaneously.
|modelnpc Baltharus the Warborn##39751
.' Click here to kill the other beginning boss, Saviana |next "sav" |confirm
.' Click here to kill the third boss, General Zarithrian |next "gen" |confirm
step
label "sav"
goto 36.5,53.7
'_Saviana Ragefire_
.' _Flame Breath_ hits as a _cleave_, inflicting 24,500-31,500 Fire damage. Make sure the tank is the only one in front of Saviana when this occurs.
.' _Enrage_ increases his attack speed by 150% and causes Fire Nova, which inflicts 7600 Fire damage to players within 100 yards.
.' _Conflagration_ occurs during air phases, Saaviana enguls the targeted raid member in flames, dealing 8000 Fire damage to them and all other raid members in 10 yards.
.' _Flame Beacon_ occurs during air phases, Saviana targets 3 people and deals 24,500-31,500 damage (33,700-43,00 in heroic).
|modelnpc Saviana Ragefire##39747
.' Click here to kill the other beginning boss, Balharus |next "balth" |confirm
.' Click here to kill the third boss, General Zarithrian |next "gen" |confirm
step
label "gen"
goto 49.6,80.1
'_General Zarithrian_
.' _Cleave Armor_ is a debuff that reduces the target's armor by 20% for 30 seconds and stacks up to 5 times.
.' _Intimidating Roar_ is an AoE sun, paralyzing raid members near Zarithrian for 4 seconds.
.' _Summon Flamecaller_ spawns 3 Onyx Firecallers.
|modelnpc General Zarithrian##39746
.' Click here to kill the final boss, Halion! |next "hel" |confirm
step
label "hel"
goto 49.3,53.9
'_Halion <The Twilight Destroyer>_
'_Phase 1_
.' Halion has the typical dragon abilities, including two _frontal cleaves_ and a _tail lash_. The tank should be the only one in front of him and all raid members should stay away from his tail.
.' _Fiery Combustion_ engulfs the target in flame, dealing 4000 Fire damage every 2 seconds, every tick applies _Mark of Combustion_ which deals 3000 Fire damage per charge.
.' _Meteor Strike_ deals 18,000-22,000 Fire damage to enemies within 12 yards.
'_Phase 2_
.' Halion spawns a portal to the Twilight realm. The raid should split and kill Halion in both realms
.' _Dark Breath_ is a frontal cone_, inflicting 17,000-23,000 Shadow damage. Make sure the tank is the only one in front of Halion when this occurs.
.' _Dusk Shroud_ is an aura the eminates from Halion. Raid members near him take 3000 Shadwo damage every 2 seconds.
.' _Soul Consumption_ deals 4000 Shadow damage every 2 seconds. Every time this deals damage it applies Mark of Consumption, which deals 3000 Shadow damage per charge.
|modelnpc Halion <The Twilight Destroyer>##39863
|confirm
step
.' Congratulations, you have finished the Ruby Sanctum raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\The Eye of Eternity",[[
dungeon
description This guide will walk you through the Eye of Eternity Raid.
author support@zygorguides.com
step
goto The Eye of Eternity/1 37.7,50.7
.' Click the Focusing Iris to start the fight.
|confirm
step
goto 38.2,50.4
'_Malygos_
'_Phase 1_
.' _Arcane Explosion_ deals 6000-8000 Arcane damage to everyone in the raid.
.' _Arcane Breath_ deals 18,800-28,300 Arcane damage in a _cone_ in front of Malygos. It also applies a debuff, causing an explosion after 5 seconds which deals 10,000 damage to all nearby players.
.' _Arcane Storm_ Arcane missles fire into the air, which hit random raid members 9500-10,500 Arcane damage.
.' _Vortex_ occurs when Malygos flies into the air, tossing the raid around and dealing 2000 Arcane damage. The raid will then be dropped on the center platform, taking 10% fall damage.
.' _Power Sparks_ Malygos summons a Power Spark away from the platform and move towards him, increasing his damage done by 50% if it reaches him. If killed it leaves a mark on the ground that increases players' damage by 50% when standing in it.
.' _Detonate_ affects the player with the most mana in the raid, burning 10,000 mana and dealing 2 Arcane damage per mana point.
'_Phase 2_ occurs when Malygos is at 50% health.
|modelnpc Malygos##28859
.' Click here to proceed to _Phase 2_. |confirm
step
goto 38.2,50.4
'_Malygos_
'_Phase 2_
.' Nexus Lord and Scions of Eternity adds will spawn at the beginning of the phase.
.' _Arcane Storm_ Arcane missles fire into the air, which hit random raid members 9500-10,500 Arcane damage.
.' _Deep Breath_ deals 5000 Arcane damage per second to the whole raid for 5 seconds.
.' _Power Overload_ spawn purple anti-magic zones on the platform, reducing all magic taken by 50% while standing inside.
.' _Phase 3_ starts when all the adds are dead.
|modelnpc Malygos##28859
.' Click here to proceed to _Phase 3_ |confirm
step
goto 38.2,50.4
'_Malygos_
'_Phase 3_
.' All raid members land on red dragons after Malygos shatters the platform.
.' _Arcane Pulse_ deals 28,200-31,700 Arcane damage to anyone within 30 yards of Malygos.
.' _Static Field_ creates a blue orb at a random location. The orb deals 10,000 Arcane damage per second to anyone within 30 yards of it.
.' _Surge of Power_ affects a random raid member. Malygos shoots a beam at them, dealing 12,000 Arcane damage every half-second for 3 seconds, as well as 5000 Arcane damage per second to anyone near the primary target.
|modelnpc Malygos##28859
|confirm
step
.' Congratulations, you have finished the Eye of Eternity raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\Ulduar",[[
dungeon 529
description This guide will walk you through the Ulduar Raid.
author support@zygorguides.com
step
goto Ulduar/1 49.9,86.0
.talk 33579
.' Tell him "We're ready. Begin the assault!"
|confirm
step
.' You and your party can now climb into the siege vehicles and start the battle.
.' Salvaged Siege Engines are found at [Ulduar/1 50.0,90.0]
.|tip Malee and ranged damage dealer.
.' Salvaged Choppers are found at [Ulduar/1 51.6,86.1]
.|tip AoE kill Iron Dwarves on foot, drop ignitable oil patches, collect Gunners who have completed their mission, bring pyrite to Demolishers.
.' Salvaged Demolishers are found at [Ulduar/1 53.9,86.9]
.|tip Ranged damage dealer.
.' Click here once you've chosen your vehicles |confirm
step
.' Make your way north to the first boss, the Flame Leviathan |goto 49.1,45.1 <10 |c
.' Kill adds as you go. The Storm Beacon towers spawn an endless stream of adds, but can be destroyed to stop the spawning.
.' You can find a repair station at [Ulduar/1 51.9,47.4]. Driving over this will completely heal your vehicle and restore energy, steam pressure, and pyrite.
.' Another of these repair stations can be found at [Ulduar/1 46.5,47.1]
step
goto 49,40
.' Clearing these adds will cause the boss, the Flame Leviathan to spawn.
.' It is a good idea to shoot down as many Mechanolift 304-A as possible. These drop pyrite, which replenish your ammo and is used for some vehicle abilities.
|modelnpc Mechanolift 304-A##33214
|confirm
step
goto 49,40
'_The Flame Leviathan_
.' _Flame Vents_ has a 50 yard range and inflicts 3000 Fire damage every second to enemies around Flame Leviathan, lasting 10 seconds.
.' _Battering Ram_ deals damage and knocks the target backward, increasing damage taken by 50% for 20 seconds.
.' _Gathering Speed_ increases Flame Leviathan's speed by 5%, stacking up to 20 times.
.' _Missle Barrage_ fires missles into the air that hit random raid members and deal 3700-4200 damage.
|modelnpc Flame Leviathan##33113
|confirm
step
map Ulduar/1
path loop off
path	48.5,35.1	48.6,26.5	40.6,26.5
.' Follow the path, clearing trash as you make your way to the next boss, Ignis the Furnace Master. |goto 40.6,26.5 <5 |noway |c
step
goto 37.5,26.5
'_Ignis the Furnace Master_
.' Click here for 10 man abilities. |next "ignis10" |confirm
.' Click here for 25 man abilities. |next "ignis25" |confirm
step
label "ignis10"
goto 37.5,26.5
'_Ignis the Furnace Master 10 man_
.' _Scorch_ is a _cleave_, dealing 2500 Fire damage every half second to all raid members in front of Ignis and lasting 3 seconds. The ground is also scorched by this effect, it catches fire and deals 3300-3700 Fire damage every second to anyone standing in it. _Iron Construct_ adds dragged through the fire begin to heat up, causing them to become _molten_.
.' _Flame Jets_ Ignis stomps on the ground, causing geysers of flame to erupt under all raid members. These deal 5500-6500 Fire damage, knocking players into the air for 8 seconds and dealing an additional 1000 Fire damage every second.
.' _Slag Pot_ a random raid member is thrown into Ignis' slag pot, dealing 3500 Fire damage every second for 10 seconds. If they survive their haste is increased by 100% for 10 seconds.
.' _Activate Construct_ Ignis actives an Iron Construct add. These adds must be dragged through his _Scorch_ ability, turning them Molten after 10 stacks. Once Molten, it must be run into water to turn it _Brittle_. This stuns the Construct, increasing its chance of being critically hit and causing it to shatter if hit for more than 5000 damage in one attack.
.' _Strength of the Creator_ is a stacking buff applied to Ignis when an Iron Construct is alive. It increases his damage by 20% for every Iron Construct summoned.
|modelnpc Ignis the Furnace Master##33118
|next "igniscomplete" |confirm
step
label "ignis25"
goto 37.5,26.5
'_Ignis the Furnace Master 25 man_
.' _Scorch_ is a _cleave_, dealing 4000 Fire damage every half second to all raid members in front of Ignis and lasting 3 seconds. The ground is also scorched by this effect, it catches fire and deals 3300-3700 Fire damage every second to anyone standing in it. _Iron Construct_ adds dragged through the fire begin to heat up, causing them to become _molten_.
.' _Flame Jets_ Ignis stomps on the ground, causing geysers of flame to erupt under all raid members. These deal 8500-11,200 Fire damage, knocking players into the air for 8 seconds and dealing an additional 2600 Fire damage every second.
.' _Slag Pot_ a random raid member is thrown into Ignis' slag pot, dealing 5000 Fire damage every second for 10 seconds. If they survive their haste is increased by 100% for 10 seconds.
.' _Activate Construct_ Ignis actives an Iron Construct add. These adds must be dragged through his _Scorch_ ability, turning them Molten after 10 stacks. Once Molten, it must be run into water to turn it _Brittle_. This stuns the Construct, increasing its chance of being critically hit and causing it to shatter if hit for more than 5000 damage in one attack.
.' _Strength of the Creator_ is a stacking buff applied to Ignis when an Iron Construct is alive. It increases his damage by 20% for every Iron Construct summoned.
|modelnpc Ignis the Furnace Master##33118
|next "igniscomplete" |confirm
step
label "igniscomplete"
map Ulduar/1
path loop off
path	49.1,26.6	51.3,26.6
.' Follow the path to the next boss, Razorscale. |goto 51.3,26.6 <5 |noway |c
step
goto Ulduar/1 51.3,26.6
.talk 33210
.' Tell him "We are ready to help!"
.' This will start the boss fight, make sure you are ready.
|confirm
step
goto 54.1,26.3
'_Razorscale_
.' Click here for 10 man abilities. |next "rzr10" |confirm
.' Click here for 25 man abilities. |next "rzr25" |confirm
step
label "rzr10"
goto 54.1,26.3
'_Razorscale 10 man_
.' Phase 1 starts with Razorscale in the air, spawning adds and attacking the raid.
.' Avoid her attacks and kill the adds until the Harpoon Turrets are ready at [Ulduar/1,52.2,26.8]. These will pull her to the ground so you can attack her.
.' _Fireball_ deals 11,000-12,000 Fire damage.
.' _Wing Buffet_ occurs when Razorscale breaks her chains. It knocks back players within 35 yards of her.
.' _Flame Buffet_ is a debuff that increases the target's Fire damage taken by 1000 for 1 minute.
.' _Flame Breath_ deals 13,000-16,700 Fire damage to players in a _cone_ in front of Razorscale. Make sure the tank is the only one in front of her when this occurs.
.' _Devouring Flame_ Razorscale spits a Lava Bomb at a player, inflicting 6000-7000 Fire damage to them and an additional 6000-7000 Fire damage every second to anyone standing within 6 yards of the explosion, lasting 25 seconds.
.' _Fuse Armor_ is a debuff that reduces the armor, attack, and movement speed by 20%, stacking up to 5 times.
.' _Berserk_ occurs after 8 minutes, it increases Razorscale's attack speed by 150% and damge dealt by 900%. Kill her as fast as possible to avoid death.
|modelnpc Razorscale##33186
|next "rzrcomplete" |confirm
step
label "rzr25"
goto 54.1,26.3
'_Razorscale 25 man_
.' Phase 1 starts with Razorscale in the air, spawning adds and attacking the raid.
.' Avoid her attacks and kill the adds until the Harpoon Turrets are ready at [Ulduar/1,52.2,26.8]. These will pull her to the ground so you can attack her.
.' _Fireball_ deals 11,000-12,000 Fire damage.
.' _Wing Buffet_ occurs when Razorscale breaks her chains. It knocks back players within 35 yards of her.
.' _Flame Buffet_ is a debuff that increases the target's Fire damage taken by 1500 for 1 minute.
.' _Flame Breath_ deals 17,500-22,500 Fire damage to players in a _cone_ in front of Razorscale. Make sure the tank is the only one in front of her when this occurs.
.' _Devouring Flame_ Razorscale spits a Lava Bomb at a player, inflicting 8800-10,200 Fire damage to them and an additional 8800-10,200 Fire damage every second to anyone standing within 6 yards of the explosion, lasting 25 seconds.
.' _Fuse Armor_ is a debuff that reduces the armor, attack, and movement speed by 20%, stacking up to 5 times.
.' _Berserk_ occurs after 8 minutes, it increases Razorscale's attack speed by 150% and damge dealt by 900%. Kill her as fast as possible to avoid death.
|modelnpc Razorscale##33186
|next "rzrcomplete" |confirm
step
label "rzrcomplete"
map Ulduar/1
path loop off
path	48.6,25.3	48.6,19.0	48.5,17.3
.' Follow the path, clearing trash as you make your way to the next boss, XT-002 Deconstructor. |goto 48.5,17.3 <5 |noway |c
step
goto 48.5,13.9
'_XT-002 Deconstructor_
.' Click here for 10 man abilities. |next "xt10" |confirm
.' Click here for 25 man abilities. |next "xt25" |confirm
step
label "xt10"
goto 48.5,13.9
'_XT-002 Deconstructor 10 man_
.' _Gravity Bomb_ after 9 seconds a Gravity Bomb spawns at the targeted player's location, pulling raid members within 12 yards into the gravity well, dealing 12,000 damage.
.' _Light Bomb_ is a DoT that deals 2700 AoE damage every tick.
.' _Heartbreak_ XT reveals his heart, lowering it to the ground and becoming invulnerable. Destroy the heart. This will increase his health by 50% and damage by 15%.
.' _Tympanic Tantrum_ deals damage equal to 10% of the player's maximum health every second for 8 seconds. Nearby raid members are stunned for this duration.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc XT-002 Deconstructor##33293
|next "xtcomplete" |confirm
step
label "xt25"
goto 48.5,13.9
'_XT-002 Deconstructor 25 man_
.' _Gravity Bomb_ after 9 seconds a Gravity Bomb spawns at the targeted player's location, pulling raid members within 12 yards into the gravity well, dealing 12,000 damage.
.' _Light Bomb_ is a DoT that deals 2700 AoE damage every tick.
.' _Heartbreak_ XT reveals his heart, lowering it to the ground and becoming invulnerable. Destroy the heart. This will increase his health by 60% and damage by 15%.
.' _Tympanic Tantrum_ deals damage equal to 10% of the player's maximum health every second for 8 seconds. Nearby raid members are stunned for this duration.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc XT-002 Deconstructor##33293
|next "xtcomplete" |confirm
step
label "xtcomplete"
goto 48.5,11.1 |n
.' Click the Ulduar Teleporter and teleport to the Antechamber of Ulduar. |goto Ulduar/2 37.1,75.3 <10 |noway |c
step
map Ulduar/2
path loop off
path	37.2,70.6	37.1,47.6	28.9,30.2
path	15.3,36.7
.' Follow the path, clearing trash as you make your way to the next boss, the Assembly of Iron. |goto 15.3,36.7 <5 |noway |c
step
goto 15,56
'_Assembly of Iron_
.' Click here for 10 man abilities. |next "iron10" |confirm
.' Click here for 25 man abilities. |next "iron25" |confirm
step
label "iron10"
goto 15,56
'_Assembly of Iron 10 man_
.' _Enrage_ occurs after 15 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Steelbreaker_
.' _Fusion Punch_ deals 18,800-21,100 Nature damage and an additional 15,000 Nature damage every second for 4 seconds. Only hits the tanks.
.' _High Voltage_ inflicts 1500 Nature damage to the whole raid every 3 seconds.
'_Runemaster Molgeim_
.' _Rune of Power_ is a ground-targeted AoE that buffs anyone, enemy of friendly, standing on it with increased damage by 50%.
.' _Shield of Runes_ a shield that absorbs 20,000 damage. If the shield absorbs this much damage, Molgeim is buffed with 50% increased damage. This should be purged, dispelled, or spellstolen.
'_Stormcaller Brundir_
.' _Chain Lightning_ deals 4100-4800 Nature damage to the target and can jump to 5 additional targets within 10 yards.
.' _Overload_ deals 20,000 Nature damage to all raid members within 20 yards of Brundir. While casting this he deals 250 Nature damage and is immune to stuns.
|modelnpc Steelbreaker##32867
|modelnpc Runemaster Molgeim##32927
|modelnpc Stormcaller Brundir##32857
|next "ironcomplete" |confirm
step
label "iron25"
goto 15,56
'_Assembly of Iron 25 man_
.' _Enrage_ occurs after 15 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Steelbreaker_
.' _Fusion Punch_ deals 35,000 Nature damage and an additional 20,000 Nature damage every second for 4 seconds. Only hits the tanks.
.' _High Voltage_ inflicts 3000 Nature damage to the whole raid every 3 seconds.
'_Runemaster Molgeim_
.' _Rune of Power_ is a ground-targeted AoE that buffs anyone, enemy of friendly, standing on it with increased damage by 50%.
.' _Shield of Runes_ a shield that absorbs 20,000 damage. If the shield absorbs this much damage, Molgeim is buffed with 50% increased damage. This should be purged, dispelled, or spellstolen.
'_Stormcaller Brundir_
.' _Chain Lightning_ deals 4100-4800 Nature damage to the target and can jump to 5 additional targets within 10 yards.
.' _Overload_ deals 20,000 Nature damage to all raid members within 20 yards of Brundir. While casting this he deals 250 Nature damage and is immune to stuns.
|modelnpc Steelbreaker##32867
|modelnpc Runemaster Molgeim##32927
|modelnpc Stormcaller Brundir##32857
|next "ironcomplete" |confirm
step
label "ironcomplete"
map Ulduar/2
path loop off
path	15.4,37.1	26.8,30.5	35.7,36.5
path	36.1,25.0	37.2,21.8
.' Follow the path, clearing trash as you make your way to the next boss, Kologarn. |goto 37.2,21.8 <5 |noway |c
step
goto 37.3,14.0
'_Kologarn_
.' Click here for 10 man abilities. |next "kolo10" |confirm
.' Click here for 25 man abilities. |next "kolo25" |confirm
step
label "kolo10"
goto 37.3,14.0
'_Kologarn 10 man_
.' _Overhead Smash_ applies a debuff that reduces armor by 20% for 6 seconds and interrupts casting for 8 seconds for any raid members in melee range.
.' _Petrifying Breath_ occurs when no target is in melee range, usually results in a wipe. It deals 14,000-16,000 damage every second for 4 seconds, also increasing damage taken by 20% for 8 seconds.
.' _Focused Eyebeam_ shoots beams out of his eyes, dealing 2700-3200 Nature damage to players within 3 yards.
'_Left Arm_
.' _Shockwave_ sweeps the entire raid, dealing 8800-10,200 Nature damage.
'_Right Arm_
.' _Stone Grip_ grabs a random raid member and stuns them, dealing 3200-3700 damage per second until the player is dead or the arm takes 150,000 damage.
|modelnpc Kologarn##32930
|next "kolocomplete" |confirm
step
label "kolo25"
goto 37.3,14.0
'_Kologarn 25 man_
.' _Overhead Smash_ applies a debuff that reduces armor by 25% for 45 seconds, stacking up to 4 times and interrupts casting for 8 seconds for any raid members in melee range.
.' _Petrifying Breath_ occurs when no target is in melee range, usually results in a wipe. It deals 18,700-21,200 damage every second for 4 seconds, also increasing damage taken by 20% for 8 seconds.
.' _Focused Eyebeam_ shoots beams out of his eyes, dealing 3700-4300 Nature damage to players within 3 yards.
'_Left Arm_
.' _Shockwave_ sweeps the entire raid, dealing 11,500-13,400 Nature damage.
'_Right Arm_
.' _Stone Grip_ grabs a random raid member and stuns them, dealing 5300-5600 damage per second until the player is dead or the arm takes 450,000 damage.
|modelnpc Kologarn##32930
|next "kolocomplete" |confirm
step
label "kolocomplete"
goto Ulduar/2 38.6,0.1 |n
.' Loot the chest and go down the stairs here. |goto Ulduar/3 53.7,77.0 <10 |noway |c
step
goto 56.7,65.5
'_Auriaya_
.|tip She walks back and forth along the northern part of this balcony, you may have to wait a minute for her to come back.
.' Click here for 10 man abilities. |next "aur10" |confirm
.' Click here for 25 man abilities. |next "aur25" |confirm
step
label "aur10"
goto 56.7,65.5
'_Auriaya 10 man_
.' _Terrifying Screech_ is an AoE Fear, causing players near Auriaya to flee in horror for 5 seconds.
.' _Sentinel Blast_ inflicts 5500 Shadow damage and increases their Shadow damage taken by 100% for 5 seconds. Stacks 5 times.
.' _Sonic Screech_ is an AoE that deals 74,000-86,000 Shadow damage to all enemies in its path. The damage is split between the target.
.' _Summon Swarming Guardian_ summons a swarm of smaller panther adds.
|modelnpc Auriaya##33515
|next "aurcomplete" |confirm
step
label "aur25"
goto 56.7,65.5
'_Auriaya 25 man_
.' _Terrifying Screech_ is an AoE Fear, causing players near Auriaya to flee in horror for 5 seconds.
.' _Sentinel Blast_ inflicts 5500 Shadow damage and increases their Shadow damage taken by 100% for 5 seconds. Stacks 5 times.
.' _Sonic Screech_ is an AoE that deals 185,000-215,000 Shadow damage to all enemies in its path. The damage is split between the target.
.' _Summon Swarming Guardian_ summons a swarm of smaller panther adds.
|modelnpc Auriaya##33515
|next "aurcomplete" |confirm
step
label "aurcomplete"
map Ulduar/3
path loop off
path	51.0,55.9	51.3,43.2	52.1,33.4
.' Follow the path, clearing trash on your way to the next boss, Freya. |goto 52.1,33.4 <5 |noway |c
step
.' The next boss fight has 10 and 25 man versions. The abilities used and strategies are very similar, the differences being the mobs' increased health and damage in 25 man.
|confirm
step
goto 53,23
.' Freya is found at this spot. She has 3 Ancients that have unique abilities that increase the fight's difficulty if left alive:
.' _Brightleaf's Essence_ increases magic damage dealt by Freya and her Nature allies by 50%.
.' _Stonebark's Essence_ increases physical damage dealt by Freya by 50%.
.' _Ironbranch's Essence_ also increases physical damage dealt by Freya by 50%.
|modelnpc Freya##32906
.' Click here to kill the Ancients and weaken Freya! |next "killancients" |confirm
.' Click here to kill Freya without killing Ancients! |next "freya" |confirm
step
label "killancients"
goto 40.1,20.5
'_Elder Brightleaf_
.' Casts a _Sunbeam_ that, if not moved out of, will heal him quickly.
.' _Solar Flare_ does 8000-9000 AoE damage to the raid.
|modelnpc Elder Brightleaf##32915
|confirm
step
goto 55.5,40.7
'_Elder Stonebark_
.' _Ground Tremor_ hits the entire raid for 8500-9500 damage.
.' _Petrified Bark_ reflects melee attacks and abilities for 30 seconds. Lasts for 60 charges (120 charges in 25 man).
.' _Fists of Stone_ is a buff that generally will kill the tank. This usually causes the need for a tank swap after it is cast on the main tank. Increases Stonebark's movement speed by 20%, damage by 250% and has a chance to reduce the target's chance to block, dodge, or parry by 100%.
|modelnpc Elder Stonebark##32914
|confirm
step
goto 61.8,20.7
'_Elder Ironbranch_
.' _Impale_ deals 16,500-20,000 (33,000-37,000 in 25 man) damage every second for 5 seconds. Must be healed through.
.' _Iron Roots_ roots the targeted player in place. These roots must be targeted and destroyed to free the player.
|modelnpc Elder Ironbranch##32913
|confirm
step
label "freya"
goto 53,23
'_Freya_
'_Phase 1_
.' As you engage Freya, she will cast a heal on herself and a buff that increases her healing by 4% per stack, starting with 150 stacks, making her basically invincible at the start of the fight. These buffs are removed by killing the adds she spawns.
.' _Touch of Eonar_ this is a permanent buff on Freya that heals her for 6000 health every second.
.' _Attuned to Nature_ is the stacking buff that increases Freya's healing by 4% per stack. It is removed by killing adds.
.' _Sunbeam_ deals 5000-6000 Nature damage to enemies within 8 yards of the targeted area.
.' _Nature Bomb_ Freya launches 10-15 nature bombs onto random raid members. These bombs explode after 10 seconds, dealing 6000 Nature damage to raid members within 10 yards of the bomb, also knocking them back.
|modelnpc Freya##32906
|confirm
step
map Ulduar/3
path loop off
path	51.1,51.3	52.2,57.7	56.3,56.9
path	60.6,49.3	65.8,48.6
.' Follow the path, clearing trash as you make your way to the next boss, Thorim. |goto 65.8,48.6 <5 |noway |c
step
goto 67.6,48.7
'_Thorim_
.' Click here for 10 man abilities. |next "thor10" |confirm
.' Click here for 25 man abilities. |next "thor25" |confirm
step
label "thor10"
goto 67.6,48.7
'_Thorim 10 man_
'_Phase 1_
.' The raid group must be split into two. One group down the hallway to reach Thorim and the other to to stay and fight the waves of adds.
.' _Charge Orb_ charges a nearby orb with electricity, making it fire a Lightning Shock every second, inflicting 3000 Nature damage to raid members within 30 yards.
.' _Stormhammer_ deals 2500 damage to a raid member, knocking them down and stunning them for 2 seconds and causing a _Deafening Thunder_.
.' _Defeaning Thunder_ deals 5000 Nature damage to players in close proximity to a Stormhammer strike.
.' _Summon Lightning Orb_ spawns an orb that lays waste to all in its path. This occurs if no one is in the room below Thorim or if no one reaches him within 5 minute.
'_Phase 2_
.' _Chain Lightning_ hits a player for 3700-4300 Nature damage, jumping to nearby players and dealing an additional 50% damage every jump.
.' _Lightning Charge_ deals 14,000-16,100 damage in a _cone_. Make sure the tank is the only one in front of Thorim when this occurs.
.' _Unbalancing Strike_ inflicts 200% weapon damage and applies a debuff that lowers their defense by 200 for 15 seconds.
|modelnpc Thorim##32865
|next "thorcomplete" |confirm
step
label "thor25"
goto 67.6,48.7
'_Thorim 25 man_
'_Phase 1_
.' The raid group must be split into two. One group down the hallway to reach Thorim and the other to to stay and fight the waves of adds.
.' _Charge Orb_ charges a nearby orb with electricity, making it fire a Lightning Shock every second, inflicting 3000 Nature damage to raid members within 30 yards.
.' _Stormhammer_ deals 2500 damage to a raid member, knocking them down and stunning them for 2 seconds and causing a _Deafening Thunder_.
.' _Defeaning Thunder_ deals 5000 Nature damage to players in close proximity to a Stormhammer strike.
.' _Summon Lightning Orb_ spawns an orb that lays waste to all in its path. This occurs if no one is in the room below Thorim or if no one reaches him within 5 minute.
'_Phase 2_
.' _Chain Lightning_ hits a player for 4600-5300 Nature damage, jumping to nearby players and dealing an additional 50% damage every jump.
.' _Lightning Charge_ deals 14,000-16,100 damage in a _cone_. Make sure the tank is the only one in front of Thorim when this occurs.
.' _Unbalancing Strike_ inflicts 200% weapon damage and applies a debuff that lowers their defense by 200 for 15 seconds.
|modelnpc Thorim##32865
|next "thorcomplete" |confirm
step
label "thorcomplete"
map Ulduar/3
path loop off
path	56.4,56.8	56.5,64.4	57.8,72.4
path	64.3,77.6	72.8,79.1	75.9,74.7
path	73.6,64.4	71.8,63.9
.' Follow the path, clearing trash as you make your way to the next boss, Hodir. |goto 71.8,63.9 <5 |noway |c
step
goto 66.0,63.7
'_Hodir_
.' Click here for 10 man abilities. |next "hodir10" |confirm
.' Click here for 25 man abilities. |next "hodir25" |confirm
step
label "hodir10"
goto 66.0,63.7
'_Hodir 10 man_
.' _Biting Cold_ aura that stacks a DoT that deals Frost damage. Dispellable by moving.
.' _Freeze_ inflicts 5500-6500 Frost damage to nearby players, immobilizing them for 10 seconds.
.' _Flash Freeze_ freezes the raid in ice blocks. Other raid members must attack and kill the ice block to free the encased player. Being hit by a second Flash Freeze before being freed is an instant kill. Avoid this by hiding in _Snowdrifts_ that spawn.
.' _Icicles_ ice shards fall from the ceiling, dealing a knockback and 12,000 Frost damage to any players underneath. This spawns the _Snowdrifts_ that players hide in to avoid _Flash Freeze_.
.' _Frozen Blows_ is a self buff that adds 31,000 Frost damage to all melee attacks but reduces physical damage by 70%. Also hits the raid for 4000 Frost damage every 2 seconds. Lasts 20 seconds.
|modelnpc Hodir##32845
|next "hodircomplete" |confirm
step
label "hodir25"
goto 66.0,63.7
'_Hodir 25 man_
.' _Biting Cold_ aura that stacks a DoT that deals Frost damage. Dispellable by moving.
.' _Freeze_ inflicts 5500-6500 Frost damage to nearby players, immobilizing them for 10 seconds.
.' _Flash Freeze_ freezes the raid in ice blocks. Other raid members must attack and kill the ice block to free the encased player. Being hit by a second Flash Freeze before being freed is an instant kill. Avoid this by hiding in _Snowdrifts_ that spawn.
.' _Icicles_ ice shards fall from the ceiling, dealing a knockback and 13,500 Frost damage to any players underneath. This spawns the _Snowdrifts_ that players hide in to avoid _Flash Freeze_.
.' _Frozen Blows_ is a self buff that adds 40,000 Frost damage to all melee attacks but reduces physical damage by 70%. Also hits the raid for 4000 Frost damage every 2 seconds. Lasts 20 seconds.
|modelnpc Hodir##32845
|next "hodircomplete" |confirm
step
label "hodircomplete"
map Ulduar/3
path loop off
path	59.5,64.1	56.2,62.4	51.0,54.5
.' Follow the path to the Ulduar Teleporter |goto 51.0,54.5 <5 |noway |c
step
goto 51.0,54.5 |n
.' Click the Ulduar Teleporter and teleport to the Spark of Imagination. |goto Ulduar/5 43.6,61.9 <10 |noway |c
step
goto 43.6,41.1
'_Mimiron_
.' This fight consists of 4 phases, each of which Mimiron is in a different mechanical vehicle or device.
.' Click here for 10 man abilities. |next "mimi10" |confirm
.' Click here for 25 man abilities. |next "mimi25" |confirm
step
label "mimi10"
goto 43.6,41.1
'_Mimiron 10 man_
'_Phase 1_
'_Leviathan MKII_
.' _Napalm Shell_ deals 8000 Fire damage to players within 5 yards of the targeted area, also applying a DoT that deals 4000 Fire damage every second for 8 seconds.
.' _Plasma Blast_ is a 3 second cast, dealing 17,000 damage every second.
.' _Proximity Mine_ these mines are dropped 15 yards from Leviathan. They explode and dead 9000 Fire damage when triggered by a player. They self-detonate after 35 seconds and deal 12,000 damage.
.' _Shock Blast_ deals 100,000 Nature damage to targets within 15 yards.
|modelnpc Leviathan MKII##33432
.' Click here for _Phase 2_ |confirm
step
goto 43.6,41.1
'_Mimiron 10 man_
'_Phase 2_
'_VX-001_
.' _Heat Wave_ deals 10,000 Fire damage to the entire raid and an additional 2000 damage every second for 5 seconds.
.' _Rapid Burst_ is a _cone_ that deals 2500-2800 damage to all targets in front of VK-001. Make sure the tank is the only one in front of him when this occurs.
.' _Rocket Strike_ deals 5,000,000 damage at the targeted area.
.' _Spinning Up_ VX-001 starts spinning for 4 seconds, then casting _P3Wx2 Laser Barrage_, which deals 20,000 Arcane damage to targets within 80 yards in front of VX-001. Make sure the tank is the only one in front of him when this occurs.
|modelnpc VX-001##33651
.' Click here to move to _Phase 3_ |confirm
step
goto 43.6,41.1
'_Mimiron 10 man_
'_Phase 3_
'_Aerial Command Unit_
.' _Plasma Ball_ is used on the player with highest threat, dealing 9500-10,500 damage.
.' Summons bot adds:
.' _Assault Bot:_ uses Magnetic Field, which roots the target and increases their damage taken by 30% for 6 seconds.
.' _Bomb Bot:_ explodes, dealing 23,500-26,500 Fire damage in a 5 yard radius.
|modelnpc Aerial Command Unit##33670
.' Click here to advance to _Phase 4_ |confirm
step
goto 43.6,41.1
'_Mimiron 10 man_
'_Phase 4_
'_V-07-TR-0N_
.' Uses combined abilities from previous phases.
.' _Proximity Mine_ these mines are dropped 15 yards from Leviathan. They explode and dead 9000 Fire damage when triggered by a player. They self-detonate after 35 seconds and deal 12,000 damage.
.' _Shock Blast_ deals 100,000 Nature damage to targets within 15 yards.
.' _Rocket Strike_ deals 5,000,000 damage at the targeted area.
.' _P3Wx2 Laser Barrage_ deals 20,000 Arcane damage to targets within 80 yards in front of VX-001.
.' _Hand Pulse_ deals 4700-5300 damage to the raid.
.' _Plasma Ball_ is used on the player with highest threat, dealing 9500-10,500 damage.
.' You must destroy all 4 parts within 20 seconds.
|modelnpc Mimiron##33350
|next "mimicomplete" |confirm
step
label "mimi25"
goto 43.6,41.1
'_Mimiron 25 man_
'_Phase 1_
'_Leviathan MKII_
.' _Napalm Shell_ deals 9000 Fire damage to players within 5 yards of the targeted area, also applying a DoT that deals 6000 Fire damage every second for 8 seconds.
.' _Plasma Blast_ is a 3 second cast, dealing 25,000 damage every second.
.' _Proximity Mine_ these mines are dropped 15 yards from Leviathan. They explode and dead 20,000 Fire damage when triggered by a player. They self-detonate after 35 seconds and deal 12,000 damage.
.' _Shock Blast_ deals 100,000 Nature damage to targets within 15 yards.
|modelnpc Leviathan MKII##33432
.' Click here for _Phase 2_ |confirm
step
goto 43.6,41.1
'_Mimiron 25 man_
'_Phase 2_
'_VX-001_
.' _Heat Wave_ deals 10,500 Fire damage to the entire raid and an additional 3000 damage every second for 5 seconds.
.' _Rapid Burst_ is a _cone_ that deals 3300-3700 damage to all targets in front of VK-001. Make sure the tank is the only one in front of him when this occurs.
.' _Rocket Strike_ deals 5,000,000 damage at two targeted areas.
.' _Spinning Up_ VX-001 starts spinning for 4 seconds, then casting _P3Wx2 Laser Barrage_, which deals 20,000 Arcane damage to targets within 80 yards in front of VX-001. Make sure the tank is the only one in front of him when this occurs.
|modelnpc VX-001##33651
.' Click here to move to _Phase 3_ |confirm
step
goto 43.6,41.1
'_Mimiron 25 man_
'_Phase 3_
'_Aerial Command Unit_
.' _Plasma Ball_ is used on the player with highest threat, dealing 14,100-15,800 damage.
.' Summons bot adds:
.' _Assault Bot:_ uses Magnetic Field, which roots the target and increases their damage taken by 30% for 6 seconds.
.' _Bomb Bot:_ explodes, dealing 23,500-26,500 Fire damage in a 5 yard radius.
|modelnpc Aerial Command Unit##33670
.' Click here to advance to _Phase 4_ |confirm
step
goto 43.6,41.1
'_Mimiron 25 man_
'_Phase 4_
'_V-07-TR-0N_
.' Uses combined abilities from previous phases.
.' _Proximity Mine_ these mines are dropped 15 yards from Leviathan. They explode and dead 9000 Fire damage when triggered by a player. They self-detonate after 35 seconds and deal 12,000 damage.
.' _Shock Blast_ deals 100,000 Nature damage to targets within 15 yards.
.' _Rocket Strike_ deals 5,000,000 damage at two targeted areas.
.' _P3Wx2 Laser Barrage_ deals 20,000 Arcane damage to targets within 80 yards in front of VX-001.
.' _Hand Pulse_ deals 7000-8000 damage to the raid.
.' _Plasma Ball_ is used on the player with highest threat, dealing 14,100-15,800 damage.
.' You must destroy all 4 parts within 20 seconds.
|modelnpc Mimiron##33350
|next "mimicomplete" |confirm
step
label "mimicomplete"
goto Ulduar/5 43.7,62.0 |n
.' Click on the Ulduar Teleporter and teleport to the Conservatory of Life |goto Ulduar/3 50.9,54.3 <10 |noway |c
step
.' The last normal boss in this raid, Yogg-Saron, has varying degrees of difficulty, depending on the amount of help (if any) taken from the Keepers you've already defeated. Each Keeper grants you a buff and aids you in the fight.
.' After proceeding to the next boss, you won't be able to come back and get these buffs unless you exit and enter the raid again. It is suggested you pick up any wanted buffs now.
.' _Mimiron_ grants Speed of Invention, increasing movement speed by 20%. In battle, he aids you with Destabilization Matrix, a debuff that reduces the enemiy tentacles' attack speed by 100% and casting speed by 300%. He is found at [Ulduar/3 47.8,60.3].
.' _Thorim_ grants Fury of the Storm, increasing total health by 20%. In battle he uses Titanic Storm. This spell kills immortal creates. It is the only way to kill immortal creatures. He is found at [Ulduar/3 54.3,60.3]
.' _Freya_ grants Resilience of Nature, increasing healing received by 20%. In battle she uses Sanity Well. These green pillars of light are placed at the sides of the room and regenerate Sanity to players standing in them. This is the only way to regenerate Sanity. She is found at [Ulduar/3 46.5,69.9].
.' _Hodir_ grants Fortitude of Frost, reducing your damage taken by 20% from all sources. In battle he uses Horid's Protective Gaze. Instead of dying, a player gets a chance to get encased in ice. This is a 10 second buff that can be removed by the player, saving them from death. He is found at [Ulduar/3 55.6,70.1].
|confirm
step
map Ulduar/3
path loop off
path	43.9,72.4	38.0,74.6	30.0,74.6
.' Follow the path, clearing trash as you make your way to the next boss, General Vezax. |goto 30.0,74.6 <5 |noway |c
step
map Ulduar/4
path loop off
path	31.4,51.5	27.9,56.7	30.1,63.5
path	34.9,68.4	42.8,65.5
.' Follow the path as you make your way to the next boss, General Vezax. |goto 42.8,65.5 <5 |noway |c
step
goto 52.6,57.1
'_General Vezax_
.' Click here for 10 man abilities. |next "vez10" |confirm
.' Click here for 25 man abilities. |next "vez25" |confirm
step
label "vez10"
goto 52.6,57.1
'_General Vezax 10 man
.' _Shadow Crash_ deals 5600-6300 Shadow damage and knocking back all raid members near the impact area. After impact, it leaves a field that lingers for 20 seconds, increasing magic damage dealt and cast speed by 100%, and reducing healing done and mana costs by 75%.
.' _Searing Flames_ deals 13,800-16,100 Fire damage to all raid members within 100 yards, also reducing their armor by 75% for 10 seconds.
.' _Surge of Darkness_ buffs Vezax, increasing his Physical damage by 100% and reducing movement speed by 55%. Lasts 10 seconds.
.' _Mark of the Faceless_ siphons 5000 health every second from players near the target. Lasts 10 seconds.
.' _Aura of Despair_ is a debuff applied to mana-users. It prevents mana regeneration and reduces melee attack speed by 20%.
.' _Corrupted Rage_ affects Shamans with Shamanistic Rage, increasing mana restored by 100% per proc, but reducing healing by 90%.
.' _Corrupted Wisdom_ affects Paladins with Judgments of the Wise, corrupting it to reduce healing.
.' _Saronite Vapors_ spawn green crystals with 12,600 hit points. Destroying these leave a green puddle on the ground. Standing in them cause players to take stacking Shadow damage but they regenerate mana.
|modelnpc General Vezax##33271
|next "vezcomplete" |confirm
step
label "vez25"
goto 52.6,57.1
'_General Vezax 25 man_
.' _Shadow Crash_ deals 11,300-12,700 Shadow damage and knocking back all raid members near the impact area. After impact, it leaves a field that lingers for 20 seconds, increasing magic damage dealt and cast speed by 100%, and reducing healing done and mana costs by 75%.
.' _Searing Flames_ deals 13,800-16,100 Fire damage to all raid members within 100 yards, also reducing their armor by 75% for 10 seconds.
.' _Surge of Darkness_ buffs Vezax, increasing his Physical damage by 100% and reducing movement speed by 55%. Lasts 10 seconds.
.' _Mark of the Faceless_ siphons 5000 health every second from players near the target. Lasts 10 seconds.
.' _Aura of Despair_ is a debuff applied to mana-users. It prevents mana regeneration and reduces melee attack speed by 20%.
.' _Corrupted Rage_ affects Shamans with Shamanistic Rage, increasing mana restored by 100% per proc, but reducing healing by 90%.
.' _Corrupted Wisdom_ affects Paladins with Judgments of the Wise, corrupting it to reduce healing.
.' _Saronite Vapors_ spawn green crystals with 25,200 hit points. Destroying these leave a green puddle on the ground. Standing in them cause players to take stacking Shadow damage but they regenerate mana.
|modelnpc General Vezax##33271
|next "vezcomplete" |confirm
step
label "vezcomplete"
map Ulduar/4
path loop off
path	62.0,60.2	68.0,56.5
.' Follow the path to the final boss, Yogg-Saron. |goto 68.0,56.5 <5 |noway |c
step
goto
'_Yogg-Saron_
.' Click here for 10 man abilities. |next "yogg10" |confirm
.' Click here for 25 man abilities. |next "yogg25" |confirm
step
label "yogg10"
goto Ulduar/4 68,40.8
'_Yogg-Saron 10 man_
'_Phase 1_
'_Sara_
.' Sara stands in the center of the room. The only way to damage her is the AoE damage from dying Guardian adds.
.' _Sara's Fervor_ increases one of her allies' damage by 20% and their damage taken by 100%, lasting 15 seconds.
.' _Sara's Blessing_ heals one of her allies' for 27,000-33,000 and causes 60,000 Shadow damage over 20 seconds.
.' _Sara's Anger_ deals 12,500 Shadow damage over 3 seconds and increases Physical damage dealt by 12,000, lasting 12 seconds.
.' Sanity buff indicates overall level of Sanity. If Sanity reaches 0, the player becomes Insane.
.' Insane is a 60 second debuff. Yogg-Baron mind controls the taget, increasing their damage by 100% and health by 300%. The raid must kill the player. This debuff kills the target upon expiration.
|modelnpc Yogg-Saron##33288
|modelnpc Sara##33134
.' Click here to move to _Phase 2_ |confirm
step
goto Ulduar/4 68,40.8
'_Yogg-Saron 10 man_
'_Phase 2_
.' There are now two areas; outside and inside the Mind's Eye. During this phase, portals are opened to the Mind's Eye. This phase ends when the Mind's Eye is at 30% health.
'_Crusher Tentacle_
.' _Diminish Power_ weakens all members of the raid, reducing their damage by 20%. Stacks up to 4 times. Hitting the tentacle with by a melee attack disrupts the cast.
.' _Focused Anger_ enrages the tentacle, increasing its attack speed and damage by 3% per stack, stacking up to 99 times.
'_Corruptor Tentacle_
.' _Curse of Doom_ a debuff that inflicts 20,00 Shadow damage after 12 seconds.
.' _Apathy_ is a 20 second debuff which reduces attack, casting, and movement speeds by 60%.
.' _Black Plague_ is a 24 second debuff which periodically stuns the target for 2 seconds.
.' _Draining Poison_ is an 18 second debuff that deals 3500 Nature damage and drains 1400 mana every 3 seconds.
'_Constrictor Tentacle_
.' _Squeeze_ grabs a nearby raid member, inflicting 7500 Physical damage every second.
'_Sara_
.' _Psychosis_ deals 5000 Shadow damage and reduces Sanity by 12.
.' _Brain Link_ links two random raid members with a beam, which deals 3000 Shadow damage and reduces Sanity by 2 to both players while they are more than 20 yards apart.
.' _Malady of the Mind_ 4 second debuff. The target takes 5000 Shadow damage, loses 3 Sanity and gets Feared. The debuff jumps to another target within 10 yards upon expiration.
.' _Death Ray_ spawns slow moving green beams which kill any player they touch.
'_The Mind's Eye (Inside)_
'_Laughing Skull:_ uses _Lunatic Gaze_ looking into the laughin skull inflicts 1750 Shadow damage and reduces Sanity by 2% every second.
'_Influence Tentacle:_ uses _Grim Reprisal_ reflects 60% of damage taken back to the attacker.
'_Brain of Yogg-Saron:_ drives all nearby players insane, reducing Sanity to 0. 60 second cast time.
|modelnpc Yogg-Saron##33288
|modelnpc Sara##33134
.' Click here to move to _Phase 3_ |confirm
step
goto Ulduar/4 68,40.8
'_Yogg-Saron 10 man_
'_Phase 3_
'_The Head of the Beast_
.' _Lunatic Gaze_ looking into the maw unhinges your mind, inflicting 5700-6300 Shadow damage and reducing Sanity by 4% every second.
.' _Empowering Shadows_ empowers Yogg-Saron's friendly units within 20 yards, causing them to regenerate 750,000 health over 20 seconds.
.' _Shadowy Barrier_ protects Yogg-Saron from all attacks and spells.
.' _Extinguish All Life_ wipes the raid. Occurs after 15 minutes of combat.
.' If you recruited the help of _3 Keepers or less_, he also uses:
.' _Deafening Roar_ deals 7600-8400 Physical damage and silences them for 4 seconds.
|modelnpc Yogg-Saron##33288
|next "yoggcomplete" |confirm
step
label "yogg25"
goto Ulduar/4 68,40.8
'_Yogg-Saron 25 man_
'_Phase 1_
'_Sara_
.' Sara stands in the center of the room. The only way to damage her is the AoE damage from dying Guardian adds.
.' _Sara's Fervor_ increases one of her allies' damage by 20% and their damage taken by 100%, lasting 15 seconds.
.' _Sara's Blessing_ heals one of her allies' for 27,000-33,000 and causes 60,000 Shadow damage over 20 seconds.
.' _Sara's Anger_ deals 12,500 Shadow damage over 3 seconds and increases Physical damage dealt by 12,000, lasting 12 seconds.
.' Sanity buff indicates overall level of Sanity. If Sanity reaches 0, the player becomes Insane.
.' Insane is a 60 second debuff. Yogg-Baron mind controls the taget, increasing their damage by 100% and health by 300%. The raid must kill the player. This debuff kills the target upon expiration.
|modelnpc Yogg-Saron##33288
|modelnpc Sara##33134
.' Click here to move to _Phase 2_ |confirm
step
goto Ulduar/4 68,40.8
'_Yogg-Saron 25 man_
'_Phase 2_
.' There are now two areas; outside and inside the Mind's Eye. During this phase, portals are opened to the Mind's Eye. This phase ends when the Mind's Eye is at 30% health.
'_Crusher Tentacle_
.' _Diminish Power_ weakens all members of the raid, reducing their damage by 20%. Stacks up to 4 times. Hitting the tentacle with by a melee attack disrupts the cast.
.' _Focused Anger_ enrages the tentacle, increasing its attack speed and damage by 3% per stack, stacking up to 99 times.
'_Corruptor Tentacle_
.' _Curse of Doom_ a debuff that inflicts 20,00 Shadow damage after 12 seconds.
.' _Apathy_ is a 20 second debuff which reduces attack, casting, and movement speeds by 60%.
.' _Black Plague_ is a 24 second debuff which periodically stuns the target for 2 seconds.
.' _Draining Poison_ is an 18 second debuff that deals 3500 Nature damage and drains 1400 mana every 3 seconds.
'_Constrictor Tentacle_
.' _Squeeze_ grabs a nearby raid member, inflicting 7500 Physical damage every second.
'_Sara_
.' _Psychosis_ deals 5000 Shadow damage and reduces Sanity by 12.
.' _Brain Link_ links two random raid members with a beam, which deals 3000 Shadow damage and reduces Sanity by 2 to both players while they are more than 20 yards apart.
.' _Malady of the Mind_ 4 second debuff. The target takes 5000 Shadow damage, loses 3 Sanity and gets Feared. The debuff jumps to another target within 10 yards upon expiration.
.' _Death Ray_ spawns slow moving green beams which kill any player they touch.
'_The Mind's Eye (Inside)_
'_Laughing Skull:_ uses _Lunatic Gaze_ looking into the laughin skull inflicts 1750 Shadow damage and reduces Sanity by 2% every second.
'_Influence Tentacle:_ uses _Grim Reprisal_ reflects 60% of damage taken back to the attacker.
'_Brain of Yogg-Saron:_ drives all nearby players insane, reducing Sanity to 0. 60 second cast time.
|modelnpc Yogg-Saron##33288
|modelnpc Sara##33134
.' Click here to move to _Phase 3_ |confirm
step
goto Ulduar/4 68,40.8
'_Yogg-Saron 25 man_
'_Phase 3_
'_The Head of the Beast_
.' _Lunatic Gaze_ looking into the maw unhinges your mind, inflicting 5700-6300 Shadow damage and reducing Sanity by 4% every second.
.' _Empowering Shadows_ empowers Yogg-Saron's friendly units within 20 yards, causing them to regenerate 750,000 health over 20 seconds.
.' _Shadowy Barrier_ protects Yogg-Saron from all attacks and spells.
.' _Extinguish All Life_ wipes the raid. Occurs after 15 minutes of combat.
.' If you recruited the help of _3 Keepers or less_, he also uses:
.' _Deafening Roar_ deals 7600-8400 Physical damage and silences them for 4 seconds.
|modelnpc Yogg-Saron##33288
|next "yoggcomplete" |confirm
step
label "yoggcomplete"
.' There is a bonus boss, Algalon the Observer, that can be killed, but only in hard mode.
.' Click here to kill Algalon! |next "alstart" |confirm
.' Click here to skip Algalon! |next "raidcomplete" |confirm
step
label "alstart"
goto Ulduar/4 52.6,57.1 |n
.' Click the Ulduar Teleporter and teleport to the Antechamber of Ulduar. |goto Ulduar/2 37.1,75.7 <10 |noway |c
step
map Ulduar/2
path loop off
path	37.0,70.4	37.2,47.2	46.5,30.6
path	58.0,36.9	59.2,43.1
.' Follow the path, clearing any trash as you make your way to the bonus boss, Algalon the Observer. |goto 59.2,43.1 <5 |noway |c
step
goto 59.2,43.1
.' Click the Celestial Planetarium Access panel.
.' This will start the fight, make sure you are ready.
|confirm
step
goto 73.5,46.2
'_Algalon the Observer_
.' Click here for 10 man abilities. |next "al10" |confirm
.' Click here for 25 man abilities. |next "al25" |confirm
step
label "al10"
goto 77.8,46.8
'_Algalon the Observer 10 man_
.' _Cosmic Smash_ calls a boulder from the sky, dealing 41,400-43,500 Fire damage.
.' _Black Hole Explosion_ deals 16,000-17,000 instant Shadow damage to the raid.
.' _Quantum Strike_ hits Algalon's target for 15,600-17,300 Physical damage
.' _Phase Punch_ deals 8800-10,200 damage to the targets within 5 yards, also fading them to another plane of existence after 5 stacks.
.' _Big Bang_ deals 76,300-88,600 Physical damage to nearby enemies.
.' _Ascend to the Heavens_ occurs after 6 minutes of combat, dealing 655,000-725,000 damage to the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Algalon the Observer##32871
|next "raidcomplete" |confirm
step
label "al25"
goto 77.8,46.8
'_Algalon the Observer 25 man_
.' _Cosmic Smash_ calls a boulder from the sky, dealing 41,400-43,500 Fire damage.
.' _Black Hole Explosion_ deals 16,000-17,000 instant Shadow damage to the raid.
.' _Quantum Strike_ hits Algalon's target for 34,100-35,800 Physical damage
.' _Phase Punch_ deals 8800-10,200 damage to the targets within 5 yards, also fading them to another plane of existence after 5 stacks.
.' _Big Bang_ deals 107,000-113,000 Physical damage to nearby enemies.
.' _Ascend to the Heavens_ occurs after 6 minutes of combat, dealing 655,000-725,000 damage to the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Algalon the Observer##32871
|next "raidcomplete" |confirm
step
label "raidcomplete"
.' Congratulations, you have finished the Ulduar raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\Trial of the Crusader",[[
dungeon 543
description This guide will walk you through the Trial of the Crusader Raid.
author support@zygorguides.com
step
goto Trial of the Crusader/1 64.8,54.1
.talk 34816
.' Tell him "Yes. We are prepared for the challenges ahead of us."
.' This will start the first boss fight, make sure you are ready.
|confirm
step
goto 51.3,52.5
'_Beasts of Northrend_
.' Click here for 10 man abilities. |next "beasts10" |confirm
.' Click here for 25 man abilities. |next "beasts25" |confirm
step
label "beasts10"
goto 51.3,52.5
'_Beasts of Northrend 10 man_
'_Phase 1_
'_Gormok the Impaler_
.' _Staggering Stomp_ deals 8000 Physical damage to all raid members within 15 yards and interrupts spellcasting for 8 seconds.
.' _Impale_ inflicts 100% weapon damage to the target and applies a bleed DoT that deals 1400-1800 damage every 2 seconds for 30 seconds. Stackable.
'_Snobold Vassals_
.' 4 of these adds jump from Gormok's back.
.' _Fire Bomb_ deals 4800-6200 Fire damage to all enemies within 8 yards of the targeted player.
.' _Head Crack_ stuns the target for 2 seconds.
.' _Batter_ strikes a player for 75% of normal damage and interrupts spellcasting for 5 seconds.
|modelnpc Gormok the Impaler##34796
.' Click here to move to _Phase 2_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 10 man_
'_Phase 2_
'_Acidmaw_
.' _Paralytic Bite_ inflicts 7800-9200 Nature damage and applies _Paralytic Toxin_.
.' _Paralytic Toxin_ is a stacking debuff that inflicts increasing Nature damage and reduces movement speed. Exposure to Burning Bile removes this effect.
.' _Slime Pool_ deals 5000-6000 Nature damage to players within the targeted area.
.' _Sweep_ deals 6800-8000 Physical damage to players within 15 yards and knocks them back. Only used while rooted.
'_Dreadscale_
.' _Burning Bite_ deals 7800-9000 Fire damage and coats them with _Burning Bile_. This effect removes any stacks of _Paralytic Toxin_.
.' _Molten Spew_ deals 2750-3225 Fire damage every 0.25 seconds for 2.5 seconds to players in front of Dreadscale.
.' _Slime Pool_ inflicts 5000-6000 Nature damage to players within the targeted area.
.' _Burning Spray_ does 7000-8000 Fire damage to the target and nearby players, also coating them with _Burning Bile_.
.' _Sweep_ inflicts 7000-8000 Physical damage to enemies within 15 yards and knocks them back. Only used while rooted.
|modelnpc Acidmaw##35144
|modelnpc Dreadscale##34799
.' Click here to advance to _Phase 3_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 10 man_
'_Phase 3_
'_Icehowl_
.' _Arctic Breath_ freezes targets in a _cone_ in front of Icehowl, dealing 15,000 Frost damage over 5 seconds.
.' _Ferocious Butt_ Icehowl headbutts the target, dealing 41,600-48,300 Physical damage and a 3 second stun.
.' _Massive Crash_ Icehowl leaps into the air and crashes down, dealing 7400-8600 Physical damage to the raid, stunning them and knocing them back.
.' _Whirl_ deals 7000-8000 Physical damage to all nearby players and knocks them back.
.' _Frothing Rage_ increases Icehowl's Physical damage and attack speed by 50%.
|modelnpc Icehowl##34797
|next "beastscomplete" |confirm
step
label "beasts25"
goto 51.3,52.5
'_Beasts of Northrend 25 man_
'_Phase 1_
'_Gormok the Impaler_
.' _Staggering Stomp_ deals 8200 Physical damage to all raid members within 15 yards and interrupts spellcasting for 8 seconds.
.' _Impale_ inflicts 100% weapon damage to the target and applies a bleed DoT that deals 2100-2800 damage every 2 seconds for 40 seconds. Stackable.
'_Snobold Vassals_
.' 4 of these adds jump from Gormok's back.
.' _Fire Bomb_ deals 4800-6200 Fire damage to all enemies within 8 yards of the targeted player.
.' _Head Crack_ stuns the target for 2 seconds.
.' _Batter_ strikes a player for 75% of normal damage and interrupts spellcasting for 5 seconds.
|modelnpc Gormok the Impaler##34796
.' Click here to move to _Phase 2_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 25 man_
'_Phase 2_
'_Acidmaw_
.' _Paralytic Bite_ inflicts 11,100-13,000 Nature damage and applies _Paralytic Toxin_.
.' _Paralytic Toxin_ is a stacking debuff that inflicts increasing Nature damage and reduces movement speed. Exposure to Burning Bile removes this effect.
.' _Slime Pool_ deals 5000-6000 Nature damage to players within the targeted area.
.' _Sweep_ deals 7000-8000 Physical damage to players within 15 yards and knocks them back. Only used while rooted.
'_Dreadscale_
.' _Burning Bite_ deals 13,000-15,000 Fire damage and coats them with _Burning Bile_. This effect removes any stacks of _Paralytic Toxin_.
.' _Molten Spew_ deals 3700-4300 Fire damage every 0.25 seconds for 2.5 seconds to players in front of Dreadscale.
.' _Slime Pool_ inflicts 5000-6000 Nature damage to players within the targeted area.
.' _Burning Spray_ does 7000-8000 Fire damage to the target and nearby players, also coating them with _Burning Bile_.
.' _Sweep_ inflicts 7000-8000 Physical damage to enemies within 15 yards and knocks them back. Only used while rooted.
|modelnpc Acidmaw##35144
|modelnpc Dreadscale##34799
.' Click here to advance to _Phase 3_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 25 man_
'_Phase 3_
'_Icehowl_
.' _Arctic Breath_ freezes targets in a _cone_ in front of Icehowl, dealing 20,000 Frost damage over 5 seconds.
.' _Ferocious Butt_ Icehowl headbutts the target, dealing 70,000-80,000 Physical damage and a 3 second stun.
.' _Massive Crash_ Icehowl leaps into the air and crashes down, dealing 10,100-11,800 Physical damage to the raid, stunning them and knocing them back.
.' _Whirl_ deals 9200-10,750 Physical damage to all nearby players and knocks them back.
.' _Frothing Rage_ increases Icehowl's Physical damage and attack speed by 50%.
|modelnpc Icehowl##34797
|next "beastscomplete" |confirm
step
label "beastscomplete"
goto 64.7,54.2
.talk 35035
.' Tell him "What new challenge await us?"
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4
'_Lord Jaraxxus_
.' Click here for 10 man abilities. |next "jara10" |confirm
.' Click here for 25 man abilities. |next "jara25" |confirm
step
label "jara10"
goto 51.2,52.4
'_Lord Jaraxxus 10 man_
.' _Nether Power_ Jaraxxus buffs himself, increasing magic damage by 20% per stack. Starts at 10 stacks but can be spellstolen or dispelled.
.' _Fel Fireball_ inflicts 15,100-15,800 damage and an additional 6000 Fire damage every second for 5 seconds.
.' _Fel Lightning_ deals 11,700-12,300 Fire damage to a random raid member and up to 4 additional nearby players.
.' _Legion Flame_  deals 3000 Fire damage every second for 6 seconds to a random player. Also leaves a mark on the ground that deals the same damage.
.' _Incinerate Flesh_ absorbs the next 30,000 healing the target receives and decreases their damage by 50% for 12 seconds. If it is not removed before it expires this will cause a _Burning Inferno_.
.' _Burning Inferno_ deals 4000 Fire damage to the raid every second for 5 seconds.
.' _Nether Portal_ Jaraxxus opens a portal to the nether world, inflicting 9000 Shadow damage to players in a 10 yard radius of the portal and summoning a Mistress of Pain add.
.' _Infernal Eruption_ Jaraxxus summons an Infernal Volcano which deals 9500 Fire damage to nearby enemies and summons 3 Felflame Infernal adds.
'_Mistress of Pain_
.' _Spinning Pain Spike_ she spins away from a random target, dealing 50% of their maximum health in Physical damage.
'_Felflame Infernal_
.' _Fel Inferno_ affects a random raid member. The Infernal channels, dealing 5000 Fire damage every second to nearby raid members.
.' _Fel Streak_ inflicts 6500 Fire damage to their primary target.
|modelnpc Lord Jaraxxus##34780
|next "jaracomplete" |confirm
step
label "jara25"
goto 51.2,52.4
'_Lord Jaraxxus 25 man_
.' _Nether Power_ Jaraxxus buffs himself, increasing magic damage by 20% per stack. Starts at 10 stacks but can be spellstolen or dispelled.
.' _Fel Fireball_ inflicts 24,300-25,600 damage and an additional 9500 Fire damage every second for 5 seconds.
.' _Fel Lightning_ deals 11,700-12,300 Fire damage to a random raid member and up to 4 additional nearby players.
.' _Legion Flame_  deals 3000 Fire damage every second for 6 seconds to a random player. Also leaves a mark on the ground that deals the same damage.
.' _Incinerate Flesh_ absorbs the next 60,000 healing the target receives and decreases their damage by 50% for 12 seconds. If it is not removed before it expires this will cause a _Burning Inferno_.
.' _Burning Inferno_ deals 4000 Fire damage to the raid every second for 5 seconds.
.' _Nether Portal_ Jaraxxus opens a portal to the nether world, inflicting 9000 Shadow damage to players in a 10 yard radius of the portal and summoning a Mistress of Pain add.
.' _Infernal Eruption_ Jaraxxus summons an Infernal Volcano which deals 9500 Fire damage to nearby enemies and summons 3 Felflame Infernal adds.
'_Mistress of Pain_
.' _Spinning Pain Spike_ she spins away from a random target, dealing 50% of their maximum health in Physical damage.
'_Felflame Infernal_
.' _Fel Inferno_ affects a random raid member. The Infernal channels, dealing 5000 Fire damage every second to nearby raid members.
.' _Fel Streak_ inflicts 6500 Fire damage to their primary target.
|modelnpc Lord Jaraxxus##34780
|next "jaracomplete" |confirm
step
label "jaracomplete"
.' The next boss fight has many similarities to arena-style PvP. Having a few pieces of PvP gear helps but is not required.
.' You will fight a team consisting of members of the opposite faction. The individual members are randomly selected from a pool of possible NPCs.
|confirm
step
goto 64.7,54.2
.talk 35035
.' Tell him "Of course!"
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4
'_The Faction Champions_
.' The AI of the enemy NPCs behave a lot like players in PvP.
.' Healer NPCs switch to spam heals on DPS members.
.' Damage-dealing NPCs gang up on a single raid member, especially those with low health.
.' All members, even the healers, use crowd control abilities such as Polymorph, Fear, Hex, and Banish.
.' Their AI uses proximity, health, and damage to determine who they attack. Try to avoid being in the center of their group; they are likely to all turn and one shot you. If you're AoE'ing, be ready to get aggro.
.' A general strategy is to use all cooldowns as soon as the fight starts (Bloodlust/Heroism, trinkets, etc) and try and kill the healers first. The fight gets immensely easier after each kill.
|confirm
step
goto 64.7,54.2
.talk 35035
.' Tell him "That tough, huh?"
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4
'_The Twin Val'kyr_
.' Click here for 10 man abilities. |next "twin10" |confirm
.' Click here for 25 man abilities. |next "twin25" |confirm
step
label "twin10"
goto 51.2,52.4
'_The Twin Val'kyr 10 man_
.' The Light and Dark Essences look like portals and spawn next to the bosses before the fight starts. Attuning to one of these and then collecting their Concentrated Essence increases your damage by 100% when attacking the Twin with the same Essence.
'_Fjola Lightbane_
.' _Shield of Lights_ absorbs 175,000 damage and prevents spell interruption for 15 seconds.
.' Light Fortex_ is channeled for 5 seconds and deals 6000 damage every second.
.' _Surge of Light_ deals 1500 damage every 2 seconds to non-Light players.
.' _Touch of Light_ deals 6000 Light damage every 2 seconds to players under the effects of Dark Essence. Does not harm those with Light Essence.
'_Eydis Darkbane_
.' _Shield of Darkness_ absorbs 175,000 damage and prevents spell interruption for 15 seconds.
.' _Dark Vortex_ this spell is channeled for 5 seconds and deals 6000 damage every second.
.' _Surge of Darkness_ deals 1500 damage every 2 seconds to non-Dark players.
.' _Touch of Darkness_ inflicts 6000 Dark damage every 2 seconds to players under the effects of Light Essence. Does not harm those with Dark Essence.
.' _Shared abilities:_
.' _Twin's Pact_ heals them for 20% of their maximum health.
.' _Power of the Twins_ occurs when one Twin begins casting _Twin's Pact_, the other gains this buff, increasing that Twin's damage by 20% and enables her to dual-wield. While this buff is active, each successful melee attack further increases her attack speed by 10% until the buff fades.
.' _Twin Spike_ deals 100% weapon damage and inflicts a debuff that increases the target's damage taken by 20% for 15 seconds or 10 charges.
'_Concentrated Essences_
.' Absorb these white or black spheres when attuned to the same Essence to deal 100% extra damage to the corresponding Twin.
|modelnpc Fjola Lightbane##34497
|modelnpc Eydis Darkbane##34496
|next "twincomplete" |confirm
step
label "twin25"
goto 51.2,52.4
'_The Twin Val'kyr 25 man_
.' The Light and Dark Essences look like portals and spawn next to the bosses before the fight starts. Attuning to one of these and then collecting their Concentrated Essence increases your damage by 100% when attacking the Twin with the same Essence.
'_Fjola Lightbane_
.' _Shield of Lights_ absorbs 700,000 damage and prevents spell interruption for 15 seconds.
.' Light Fortex_ is channeled for 5 seconds and deals 9000 damage every second.
.' _Surge of Light_ deals 2500 damage every 2 seconds to non-Light players.
.' _Touch of Light_ deals 9000 Light damage every 2 seconds to players under the effects of Dark Essence. Does not harm those with Light Essence.
'_Eydis Darkbane_
.' _Shield of Darkness_ absorbs 700,000 damage and prevents spell interruption for 15 seconds.
.' _Dark Vortex_ this spell is channeled for 5 seconds and deals 6000 damage every second.
.' _Surge of Darkness_ deals 2500 damage every 2 seconds to non-Dark players.
.' _Touch of Darkness_ inflicts 9000 Dark damage every 2 seconds to players under the effects of Light Essence. Does not harm those with Dark Essence.
.' _Shared abilities:_
.' _Twin's Pact_ heals them for 20% of their maximum health.
.' _Power of the Twins_ occurs when one Twin begins casting _Twin's Pact_, the other gains this buff, increasing that Twin's damage by 20% and enables her to dual-wield. While this buff is active, each successful melee attack further increases her attack speed by 10% until the buff fades.
.' _Twin Spike_ deals 100% weapon damage and inflicts a debuff that increases the target's damage taken by 20% for 15 seconds or 10 charges.
'_Concentrated Essences_
.' Absorb these white or black spheres when attuned to the same Essence to deal 100% extra damage to the corresponding Twin.
|modelnpc Fjola Lightbane##34497
|modelnpc Eydis Darkbane##34496
|next "twincomplete" |confirm
step
label "twincomplete"
goto 64.7,54.2
.talk 35035
.' Tell him "Your words of praise are appreciated."
.' This will start the final boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4 |n
.' The Lich King will appear and then destroy the floor, sending the raid falling down below. |goto Trial of the Crusader/2 |noway |c
step
goto Trial of the Crusader/2 53.5,34.0
'_Anub'arak_
.' Click here for 10 man abilities. |next "anu10" |confirm
.' Click here for 25 man abilities. |next "anu25" |confirm
step
label "anu10"
goto 53.5,34.0
'_Anub'arak 10 man_
'_Phase 1:_ Anub'arak on the ground.
.' _Freezing Slash_ deals 25% Froststrike weapon damage (Frost or Physical damage, whichever deals more to the target), also freezes the target in ice for 3 seconds.
.' _Penetrating Cold_ is cast on 2 random raid members, dealing 3500 Frost damage every 3 seconds for 18 seconds. When this expires on its targets it is automatically applied to 2 new targets.
.' _Submerge_ Anub'arak submerges underground. This starts Phase 2.
'_Phase 2:_ Anub'arak underground.
.' _Pursued by Anub'arak_ Anub'arak is following you!
.' _Impale_ occurs if Anub'arak reaches his target. It deals 17,500-20,000 damage
.' _Pursuing Spikes_ fires a spike through the floor, impaling all players within 4 yards and dealing 2800-3200 damage and knocking them into the air.
.' _Summon Scarab_ spawns a Swarm Scarab from the ground
'_Phase 3:_ the leeching swarm.
.' _Leeching Swarm_ occurs when Anub'arak is 30% of his maximum health. He releases a swarm of insects that attack the raid, leeching 10% of the targets' current health every second. The health leeched heals Anub'arak for the same amount.
|modelnpc Anub'arak##34564
|next "anucomplete" |confirm
step
label "anu25"
goto 53.5,34.0
'_Anub'arak 25 man_
'_Phase 1:_ Anub'arak on the ground.
.' _Freezing Slash_ deals 25% Froststrike weapon damage (Frost or Physical damage, whichever deals more to the target), also freezes the target in ice for 3 seconds.
.' _Penetrating Cold_ is cast on 5 random raid members, dealing 3500 Frost damage every 3 seconds for 18 seconds. When this expires on its targets it is automatically applied to 5 new targets.
.' _Submerge_ Anub'arak submerges underground. This starts Phase 2.
'_Phase 2:_ Anub'arak underground.
.' _Pursued by Anub'arak_ Anub'arak is following you!
.' _Impale_ occurs if Anub'arak reaches his target. It deals 17,500-20,000 damage
.' _Pursuing Spikes_ fires a spike through the floor, impaling all players within 4 yards and dealing 2800-3200 damage and knocking them into the air.
.' _Summon Scarab_ spawns a Swarm Scarab from the ground
'_Phase 3:_ the leeching swarm.
.' _Leeching Swarm_ occurs when Anub'arak is 30% of his maximum health. He releases a swarm of insects that attack the raid, leeching 10% of the targets' current health every second. The health leeched heals Anub'arak for the same amount.
|modelnpc Anub'arak##34564
|next "anucomplete" |confirm
step
label "anucomplete"
.' Congratulations, you have finished the Trial of the Crusader raid guide!
.' There is a portal to Dalaran at this spot. [Trial of the Crusader/2 51.3,56.9]
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\Trial of the Grand Crusader",[[
dungeon 543
description This guide will walk you through the Trial of the Grand Crusader Raid.
author support@zygorguides.com
step
.' The major difference between this raid and the Trial of the Crusader is the Argent Crusade Tribute Chest.
.' All boss encounters in this raid are in Heroic Mode and the raid is given 50 attempts to complete it.
.' If the raid wipes 50 times all undefeated bosses will despawn and be unavailable for the remainder of the lockout period.
.' When the raid is completed, higher amounts of remaining attempts will reward more and higher quality loot and extra achievements.
|confirm
step
goto Trial of the Crusader/1 64.8,54.1
.talk 34816
.' Tell him "Yes. We are prepared for the challenges ahead of us."
.' This will start the first boss fight, make sure you are ready.
|confirm
step
goto 51.3,52.5
'_Beasts of Northrend_
.' Click here for 10 man abilities. |next "beasts10" |confirm
.' Click here for 25 man abilities. |next "beasts25" |confirm
step
label "beasts10"
goto 51.3,52.5
'_Beasts of Northrend 10 man Heroic_
'_Phase 1_
'_Gormok the Impaler_
.' _Staggering Stomp_ deals 9200-9800 Physical damage to all raid members within 15 yards and interrupts spellcasting for 8 seconds.
.' _Impale_ inflicts 150% weapon damage to the target and applies a bleed DoT that deals 2600-3300 damage every 2 seconds for 30 seconds. Stackable.
'_Snobold Vassals_
.' 4 of these adds jump from Gormok's back.
.' _Fire Bomb_ deals 4800-6200 Fire damage to all enemies within 8 yards of the targeted player.
.' _Head Crack_ stuns the target for 2 seconds.
.' _Batter_ strikes a player for 75% of normal damage and interrupts spellcasting for 5 seconds.
|modelnpc Gormok the Impaler##34796
.' Click here to move to _Phase 2_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 10 man Heroic_
'_Phase 2_
'_Acidmaw_
.' _Paralytic Bite_ inflicts 13,000-15,000 Nature damage and applies _Paralytic Toxin_.
.' _Paralytic Toxin_ is a stacking debuff that inflicts increasing Nature damage and reduces movement speed. Exposure to Burning Bile removes this effect.
.' _Slime Pool_ deals 5000-6000 Nature damage to players within the targeted area.
.' _Sweep_ deals 8300-9700 Physical damage to players within 15 yards and knocks them back. Only used while rooted.
'_Dreadscale_
.' _Burning Bite_ deals 13,000-15,0000 Fire damage and coats them with _Burning Bile_. This effect removes any stacks of _Paralytic Toxin_.
.' _Molten Spew_ deals 3700-4300 Fire damage every 0.25 seconds for 2.5 seconds to players in front of Dreadscale.
.' _Slime Pool_ inflicts 5000-6000 Nature damage to players within the targeted area.
.' _Burning Spray_ does 8300-9700 Fire damage to the target and nearby players, also coating them with _Burning Bile_.
.' _Sweep_ inflicts 8300-9700 Physical damage to enemies within 15 yards and knocks them back. Only used while rooted.
|modelnpc Acidmaw##35144
|modelnpc Dreadscale##34799
.' Click here to advance to _Phase 3_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 10 man Heroic_
'_Phase 3_
'_Icehowl_
.' _Arctic Breath_ freezes targets in a _cone_ in front of Icehowl, dealing 20,000 Frost damage over 5 seconds.
.' _Ferocious Butt_ Icehowl headbutts the target, dealing 55,500-64,500 Physical damage and a 3 second stun.
.' _Massive Crash_ Icehowl leaps into the air and crashes down, dealing 10,100-11,800 Physical damage to the raid, stunning them and knocing them back.
.' _Whirl_ deals 9000-10,750 Physical damage to all nearby players and knocks them back.
.' _Frothing Rage_ increases Icehowl's Physical damage and attack speed by 50%.
|modelnpc Icehowl##34797
|next "beastscomplete" |confirm
step
label "beasts25"
goto 51.3,52.5
'_Beasts of Northrend 25 man Heroic_
'_Phase 1_
'_Gormok the Impaler_
.' _Staggering Stomp_ deals 11,700-12,300 Physical damage to all raid members within 15 yards and interrupts spellcasting for 8 seconds.
.' _Impale_ inflicts 150% weapon damage to the target and applies a bleed DoT that deals 4000-5000 damage every 2 seconds for 45 seconds. Stackable.
'_Snobold Vassals_
.' 4 of these adds jump from Gormok's back.
.' _Fire Bomb_ deals 4800-6200 Fire damage to all enemies within 8 yards of the targeted player.
.' _Head Crack_ stuns the target for 2 seconds.
.' _Batter_ strikes a player for 75% of normal damage and interrupts spellcasting for 5 seconds.
|modelnpc Gormok the Impaler##34796
.' Click here to move to _Phase 2_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 25 man Heroic_
'_Phase 2_
'_Acidmaw_
.' _Paralytic Bite_ inflicts 18,500-21,500 Nature damage and applies _Paralytic Toxin_.
.' _Paralytic Toxin_ is a stacking debuff that inflicts increasing Nature damage and reduces movement speed. Exposure to Burning Bile removes this effect.
.' _Slime Pool_ deals 5000-6000 Nature damage to players within the targeted area.
.' _Sweep_ deals 10,100-11,800 Physical damage to players within 15 yards and knocks them back. Only used while rooted.
'_Dreadscale_
.' _Burning Bite_ deals 18,500-21,500 Fire damage and coats them with _Burning Bile_. This effect removes any stacks of _Paralytic Toxin_.
.' _Molten Spew_ deals 4600-5300 Fire damage every 0.25 seconds for 2.5 seconds to players in front of Dreadscale.
.' _Slime Pool_ inflicts 5000-6000 Nature damage to players within the targeted area.
.' _Burning Spray_ does 13,000-15000 Fire damage to the target and nearby players, also coating them with _Burning Bile_.
.' _Sweep_ inflicts 10,000-11,800 Physical damage to enemies within 15 yards and knocks them back. Only used while rooted.
|modelnpc Acidmaw##35144
|modelnpc Dreadscale##34799
.' Click here to advance to _Phase 3_. |confirm
step
goto 51.3,52.5
'_Beasts of Northrend 25 man Heroic_
'_Phase 3_
'_Icehowl_
.' _Arctic Breath_ freezes targets in a _cone_ in front of Icehowl, dealing 30,000 Frost damage over 5 seconds.
.' _Ferocious Butt_ Icehowl headbutts the target, dealing 83,000-96,750 Physical damage and a 3 second stun.
.' _Massive Crash_ Icehowl leaps into the air and crashes down, dealing 10,100-11,800 Physical damage to the raid, stunning them and knocing them back.
.' _Whirl_ deals 13,000-15,000 Physical damage to all nearby players and knocks them back.
.' _Frothing Rage_ increases Icehowl's Physical damage and attack speed by 50%.
|modelnpc Icehowl##34797
|next "beastscomplete" |confirm
step
label "beastscomplete"
goto 64.7,54.2
.talk 35035
.' Tell him "What new challenge await us?"
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4
'_Lord Jaraxxus_
.' Click here for 10 man abilities. |next "jara10" |confirm
.' Click here for 25 man abilities. |next "jara25" |confirm
step
label "jara10"
goto 51.2,52.4
'_Lord Jaraxxus 10 man Heroic_
.' _Nether Power_ Jaraxxus buffs himself, increasing magic damage by 20% per stack. Starts at 10 stacks but can be spellstolen or dispelled.
.' _Fel Fireball_ inflicts 15,100-15,800 damage and an additional 6000 Fire damage every second for 5 seconds.
.' _Fel Lightning_ deals 11,700-12,300 Fire damage to a random raid member and up to 4 additional nearby players.
.' _Legion Flame_  deals 3000 Fire damage every second for 6 seconds to a random player. Also leaves a mark on the ground that deals the same damage.
.' _Incinerate Flesh_ absorbs the next 30,000 healing the target receives and decreases their damage by 50% for 12 seconds. If it is not removed before it expires this will cause a _Burning Inferno_.
.' _Burning Inferno_ deals 4000 Fire damage to the raid every second for 5 seconds.
.' _Nether Portal_ Jaraxxus opens a portal to the nether world, inflicting 9000 Shadow damage to players in a 10 yard radius of the portal and summoning a Mistress of Pain add.
.' _Infernal Eruption_ Jaraxxus summons an Infernal Volcano which deals 9500 Fire damage to nearby enemies and summons 3 Felflame Infernal adds.
'_Mistress of Pain_
.' _Spinning Pain Spike_ she spins away from a random target, dealing 50% of their maximum health in Physical damage.
'_Felflame Infernal_
.' _Fel Inferno_ affects a random raid member. The Infernal channels, dealing 5000 Fire damage every second to nearby raid members.
.' _Fel Streak_ inflicts 6500 Fire damage to their primary target.
|modelnpc Lord Jaraxxus##34780
|next "jaracomplete" |confirm
step
label "jara25"
goto 51.2,52.4
'_Lord Jaraxxus 25 man Heroic_
.' _Nether Power_ Jaraxxus buffs himself, increasing magic damage by 20% per stack. Starts at 10 stacks but can be spellstolen or dispelled.
.' _Fel Fireball_ inflicts 24,300-25,600 damage and an additional 9500 Fire damage every second for 5 seconds.
.' _Fel Lightning_ deals 11,700-12,300 Fire damage to a random raid member and up to 4 additional nearby players.
.' _Legion Flame_  deals 3000 Fire damage every second for 6 seconds to a random player. Also leaves a mark on the ground that deals the same damage.
.' _Incinerate Flesh_ absorbs the next 60,000 healing the target receives and decreases their damage by 50% for 12 seconds. If it is not removed before it expires this will cause a _Burning Inferno_.
.' _Burning Inferno_ deals 4000 Fire damage to the raid every second for 5 seconds.
.' _Nether Portal_ Jaraxxus opens a portal to the nether world, inflicting 9000 Shadow damage to players in a 10 yard radius of the portal and summoning a Mistress of Pain add.
.' _Infernal Eruption_ Jaraxxus summons an Infernal Volcano which deals 9500 Fire damage to nearby enemies and summons 3 Felflame Infernal adds.
.' _Touch of Jaraxxus_ inflicts 4000 Shadow damage every second to a random raid member. Applies _Curse of the Nether_ to nearby players.
.' _Curse of the Nether_ inflicts 4500 Shadow damage every second for 15 seconds.
'_Mistress of Pain_
.' _Spinning Pain Spike_ she spins away from a random target, dealing 50% of their maximum health in Physical damage.
'_Felflame Infernal_
.' _Fel Inferno_ affects a random raid member. The Infernal channels, dealing 5000 Fire damage every second to nearby raid members.
.' _Fel Streak_ inflicts 6500 Fire damage to their primary target.
|modelnpc Lord Jaraxxus##34780
|next "jaracomplete" |confirm
step
label "jaracomplete"
.' The next boss fight has many similarities to arena-style PvP. Having a few pieces of PvP gear helps but is not required.
.' You will fight a team consisting of members of the opposite faction. The individual members are randomly selected from a pool of possible NPCs.
|confirm
step
goto 64.7,54.2
.talk 35035
.' Tell him "Of course!"
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4
'_The Faction Champions Heroic_
.' The AI of the enemy NPCs behave a lot like players in PvP.
.' Healer NPCs switch to spam heals on DPS members.
.' Damage-dealing NPCs gang up on a single raid member, especially those with low health.
.' All members, even the healers, use crowd control abilities such as Polymorph, Fear, Hex, and Banish.
.' Their AI uses proximity, health, and damage to determine who they attack. Try to avoid being in the center of their group; they are likely to all turn and one shot you. If you're AoE'ing, be ready to get aggro.
.' A general strategy is to use all cooldowns as soon as the fight starts (Bloodlust/Heroism, trinkets, etc) and try and kill the healers first. The fight gets immensely easier after each kill.
|confirm
step
goto 64.7,54.2
.talk 35035
.' Tell him "That tough, huh?"
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4
'_The Twin Val'kyr_
.' Click here for 10 man abilities. |next "twin10" |confirm
.' Click here for 25 man abilities. |next "twin25" |confirm
step
label "twin10"
goto 51.2,52.4
'_The Twin Val'kyr 10 man Heroic_
.' The Light and Dark Essences look like portals and spawn next to the bosses before the fight starts. Attuning to one of these and then collecting their Concentrated Essence increases your damage by 100% when attacking the Twin with the same Essence.
'_Fjola Lightbane_
.' _Shield of Lights_ absorbs 300,000 damage and prevents spell interruption for 15 seconds.
.' Light Fortex_ is channeled for 5 seconds and deals 6000 damage every second.
.' _Surge of Light_ deals 1500 damage every 2 seconds to non-Light players.
.' _Touch of Light_ deals 6000 Light damage every 2 seconds to players under the effects of Dark Essence. Does not harm those with Light Essence.
'_Eydis Darkbane_
.' _Shield of Darkness_ absorbs 300,000 damage and prevents spell interruption for 15 seconds.
.' _Dark Vortex_ this spell is channeled for 5 seconds and deals 6000 damage every second.
.' _Surge of Darkness_ deals 1500 damage every 2 seconds to non-Dark players.
.' _Touch of Darkness_ inflicts 6000 Dark damage every 2 seconds to players under the effects of Light Essence. Does not harm those with Dark Essence.
.' _Shared abilities:_
.' _Twin's Pact_ heals them for 50% of their maximum health.
.' _Power of the Twins_ occurs when one Twin begins casting _Twin's Pact_, the other gains this buff, increasing that Twin's damage by 20% and enables her to dual-wield. While this buff is active, each successful melee attack further increases her attack speed by 10% until the buff fades.
.' _Twin Spike_ deals 100% weapon damage and inflicts a debuff that increases the target's damage taken by 20% for 15 seconds or 10 charges.
'_Concentrated Essences_
.' Absorb these white or black spheres when attuned to the same Essence to deal 100% extra damage to the corresponding Twin.
|modelnpc Fjola Lightbane##34497
|modelnpc Eydis Darkbane##34496
|next "twincomplete" |confirm
step
label "twin25"
goto 51.2,52.4
'_The Twin Val'kyr 25 man Heroic_
.' The Light and Dark Essences look like portals and spawn next to the bosses before the fight starts. Attuning to one of these and then collecting their Concentrated Essence increases your damage by 100% when attacking the Twin with the same Essence.
'_Fjola Lightbane_
.' _Shield of Lights_ absorbs 1,200,000 damage and prevents spell interruption for 15 seconds.
.' Light Fortex_ is channeled for 5 seconds and deals 9000 damage every second.
.' _Surge of Light_ deals 2500 damage every 2 seconds to non-Light players.
.' _Touch of Light_ deals 9000 Light damage every 2 seconds to players under the effects of Dark Essence. Does not harm those with Light Essence.
'_Eydis Darkbane_
.' _Shield of Darkness_ absorbs 1,200,000 damage and prevents spell interruption for 15 seconds.
.' _Dark Vortex_ this spell is channeled for 5 seconds and deals 6000 damage every second.
.' _Surge of Darkness_ deals 2500 damage every 2 seconds to non-Dark players.
.' _Touch of Darkness_ inflicts 9000 Dark damage every 2 seconds to players under the effects of Light Essence. Does not harm those with Dark Essence.
.' _Shared abilities:_
.' _Twin's Pact_ heals them for 50% of their maximum health.
.' _Power of the Twins_ occurs when one Twin begins casting _Twin's Pact_, the other gains this buff, increasing that Twin's damage by 20% and enables her to dual-wield. While this buff is active, each successful melee attack further increases her attack speed by 10% until the buff fades.
.' _Twin Spike_ deals 100% weapon damage and inflicts a debuff that increases the target's damage taken by 20% for 15 seconds or 10 charges.
'_Concentrated Essences_
.' Absorb these white or black spheres when attuned to the same Essence to deal 100% extra damage to the corresponding Twin.
|modelnpc Fjola Lightbane##34497
|modelnpc Eydis Darkbane##34496
|next "twincomplete" |confirm
step
label "twincomplete"
goto 64.7,54.2
.talk 35035
.' Tell him "Your words of praise are appreciated."
.' This will start the final boss fight, make sure you are ready.
|confirm
step
goto 51.2,52.4 |n
.' The Lich King will appear and then destroy the floor, sending the raid falling down below. |goto Trial of the Crusader/2 |noway |c
step
goto Trial of the Crusader/2 53.5,34.0
'_Anub'arak_
.' Click here for 10 man abilities. |next "anu10" |confirm
.' Click here for 25 man abilities. |next "anu25" |confirm
step
label "anu10"
goto 53.5,34.0
'_Anub'arak 10 man Heroic_
'_Phase 1:_ Anub'arak on the ground.
.' _Freezing Slash_ deals 25% Froststrike weapon damage (Frost or Physical damage, whichever deals more to the target), also freezes the target in ice for 3 seconds.
.' _Penetrating Cold_ is cast on 2 random raid members, dealing 6000 Frost damage every 3 seconds for 18 seconds. When this expires on its targets it is automatically applied to 2 new targets.
.' _Submerge_ Anub'arak submerges underground. This starts Phase 2.
'_Phase 2:_ Anub'arak underground.
.' _Pursued by Anub'arak_ Anub'arak is following you!
.' _Impale_ occurs if Anub'arak reaches his target. It deals 17,500-20,000 damage
.' _Pursuing Spikes_ fires a spike through the floor, impaling all players within 4 yards and dealing 2800-3200 damage and knocking them into the air.
.' _Summon Scarab_ spawns a Swarm Scarab from the ground
'_Phase 3:_ the leeching swarm.
.' _Leeching Swarm_ occurs when Anub'arak is 30% of his maximum health. He releases a swarm of insects that attack the raid, leeching 10% of the targets' current health every second. The health leeched heals Anub'arak for the same amount.
|modelnpc Anub'arak##34564
|next "anucomplete" |confirm
step
label "anu25"
goto 53.5,34.0
'_Anub'arak 25 man Heroic_
'_Phase 1:_ Anub'arak on the ground.
.' _Freezing Slash_ deals 25% Froststrike weapon damage (Frost or Physical damage, whichever deals more to the target), also freezes the target in ice for 3 seconds.
.' _Penetrating Cold_ is cast on 5 random raid members, dealing 6000 Frost damage every 3 seconds for 18 seconds. When this expires on its targets it is automatically applied to 5 new targets.
.' _Submerge_ Anub'arak submerges underground. This starts Phase 2.
'_Phase 2:_ Anub'arak underground.
.' _Pursued by Anub'arak_ Anub'arak is following you!
.' _Impale_ occurs if Anub'arak reaches his target. It deals 17,500-20,000 damage
.' _Pursuing Spikes_ fires a spike through the floor, impaling all players within 4 yards and dealing 2800-3200 damage and knocking them into the air.
.' _Summon Scarab_ spawns a Swarm Scarab from the ground
'_Phase 3:_ the leeching swarm.
.' _Leeching Swarm_ occurs when Anub'arak is 30% of his maximum health. He releases a swarm of insects that attack the raid, leeching 10% of the targets' current health every second. The health leeched heals Anub'arak for the same amount.
|modelnpc Anub'arak##34564
|next "anucomplete" |confirm
step
label "anucomplete"
.' Congratulations, you have finished the Trial of the Grand Crusader raid guide!
.' There is a portal to Dalaran at this spot. [Trial of the Crusader/2 51.3,56.9]
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Northrend\\Icecrown Citadel",[[
dungeon 604
description This guide will walk you through the Icecrown Citadel Raid.
author support@zygorguides.com
step
map Icecrown Citadel/1
path loop off
path	38.9,29.2	38.9,36.3	39.0,52.4
.' Follow the path, clearing trash as you make your way to the first boss, Lord Marrowgar. |goto 39.0,52.4 <5 |noway |c
step
goto Icecrown Citadel 38.9,59.8
'_Lord Marrowgar man_
.' Click here for 10 man abilities. |next "marr10" |confirm
.' Click here for 25 man abilities. |next "marr25" |confirm
step
label "marr10"
goto Icecrown Citadel 38.9,59.8
'_Lord Marrowgar 10 man_
.' _Bone Storm_ is an AoE hits players around Marrowgar, dealing less damage the further away you are.
.' _Bone Spike Graveyard_ deals 10% of the target's maximum health as damage every second until killed by other raid members. Will automatically end after 5 mintues.
.' _Coldflame_ summons a line of frost that hits players in the way, dealing 9000 Frost damage every second for 8 seconds.
.' _Bone Slice_ deals 200% normal damage, split between Marrowgar's target and a nearby ally.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Lord Marrowgar##36612
|next "marrcomplete" |confirm
step
label "marr25"
goto Icecrown Citadel 38.9,59.8
'_Lord Marrowgar 25 man_
.' _Bone Storm_ is an AoE hits players around Marrowgar, dealing less damage the further away you are.
.' _Bone Spike Graveyard_ deals 10% of each the target's maximum health as damage every second until killed by other raid members. Will automatically end after 5 mintues.
.' _Coldflame_ summons a line of frost that hits players in the way, dealing 9000 Frost damage every second for 8 seconds.
.' _Bone Slice_ deals 200% normal damage, split between Marrowgar's target and a nearby ally.
.' _Enrage_ occurs after 10 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Lord Marrowgar##36612
|next "marrcomplete" |confirm
step
label "marrcomplete"
map Icecrown Citadel/1
path loop off
path	34.2,58.8	34.3,66.7	38.9,72.4
path	39.0,80.5
.' Follow the path, clearing trash as you make your way to the next boss, Lady Deathwhisper. |goto 39.0,80.5 <5 |noway |c
step
goto 39.1,86.7
.' _Lady Deathwhisper_
.' Click here for 10 man abilities. |next "death10" |confirm
.' Click here for 25 man abilities. |next "death25" |confirm
step
label "death10"
goto 39.1,86.7
.' _Lady Deathwhisper 10 man_
'_Phase 1_
.' _Animate Dead_ reanimates a Cult Adherent or Cult Fanatic.
.' _Dark Empowerment_ is a buff applied to Deathwhisper's allies. It increases their spell damage and make them immune to interrupts.
.' _Dark Transformation_ transforms a random Cult Fanatic into an Undead Behemoth, increasing its damage by 100%.
.' _Mana Barrier_ is a buff that heals the target in exchange for their mana.
.' _Shadow Bolt_ deals 9000-11,800 Shadow damage to the target.
.' _Death and Decay_ is an AoE that inflicts 4500 Shadow damage every second to all players in the targeted area for 10 seconds.
'_Phase 2_
.' _Frostbolt_ deals 37,000 Frost damage to a raid member and reduces their movement speed by 50% for 4 seconds. Interruptable.
.' _Frostbolt Volley_ inflicts 10,800-13,200 Frost damage to nearby raid members, also reducing their movement speed by 50% for 4 seconds.
.' _Summon Vengeful Shade_ spawns a Vengeful Shade that cannot be attacked. It chases a player for a short time, casting Vengeful Blast on them.
.' _Touch of Insignificance_ reduces the target's threat generation by 20%. Stacks up to 5 times.
.' _Death and Decay_ is an AoE that inflicts 4500 Shadow damage every second to all players in the targeted area for 10 seconds.
|modelnpc Lady Deathwhisper##36855
|next "deathcomplete" |confirm
step
label "death25"
goto 39.1,86.7
.' _Lady Deathwhisper 25 man_
'_Phase 1_
.' _Animate Dead_ reanimates a Cult Adherent or Cult Fanatic.
.' _Dark Empowerment_ is a buff applied to Deathwhisper's allies. It increases their spell damage and make them immune to interrupts.
.' _Dark Transformation_ transforms a random Cult Fanatic into an Undead Behemoth, increasing its damage by 100%.
.' _Mana Barrier_ is a buff that heals the target in exchange for their mana.
.' _Shadow Bolt_ deals 11,800-13,100 Shadow damage to the target.
.' _Death and Decay_ is an AoE that inflicts 4500 Shadow damage every second to all players in the targeted area for 10 seconds.
.' _Dominate Mind_ charms the targeted player for 20 seconds, increasing their damage by 200% and healing by 500%.
'_Phase 2_
.' _Frostbolt_ deals 52,000 Frost damage to a raid member and reduces their movement speed by 50% for 4 seconds. Interruptable.
.' _Frostbolt Volley_ inflicts 14,400-17,600 Frost damage to nearby raid members, also reducing their movement speed by 50% for 4 seconds.
.' _Summon Vengeful Shade_ spawns a Vengeful Shade that cannot be attacked. It chases a player for a short time, casting Vengeful Blast on them.
.' _Touch of Insignificance_ reduces the target's threat generation by 20%. Stacks up to 5 times.
.' _Death and Decay_ is an AoE that inflicts 4500 Shadow damage every second to all players in the targeted area for 10 seconds.
.' _Dominate Mind_ charms the targeted player for 20 seconds, increasing their damage by 200% and healing by 500%.
|modelnpc Lady Deathwhisper##36855
|next "deathcomplete" |confirm
step
label "deathcomplete"
.' Take the elevator up to the next level |goto 39.0,85.5 <5 |c
step
map Icecrown Citadel/2
path loop off
path	45.6,77.4	39.4,71.3	31.8,55.4
path	24.1,55.3
.' Follow the path towards the next boss encounter, the Gunship Battle. |goto 24.1,55.3 <5 |noway |c
step
goto 22.6,60.9
.talk 37184
.' Tell him to give you a jetpack!
.' Equip the jetpack |use Goblin Rocket Pack##49278
|confirm
step
goto Icecrown Citadel/2 21.6,60.1
.talk 36948
.' Tell him "My companions are all accounted for, Muradin. Let's go!"
.' This will start the boss fight, make sure you are ready.
|confirm
step
'_Gunship Battle_
.' This fight is an aerial battle between the Skybreaker and Ogrim's Hammer.
.' The raid will need to be split into an offensive and defensive team.
.' Defensive Team: consists of the majority of the raid. Their job is to keep the players' gunship free of enemy boarding parties and kill ranged attackers.
.' Offensive Team: consists of melee attackers, tank and healer(s). Their job is to use the guns to attack the enemy ship and board it using the jetpacks, to kill adds.
.' Use the jetpack to get to the enemy ship. |use Goblin Rocket Pack##49278
'_High Overlord Saurfang_
.' _Battle Fury_ increases damage dealt by 5%.
.' _Cleave_ inflicts 120% weapon damage to the target and up to 3 nearby alliles. Make sure the tank is the only one in front of him when this occurs.
.' _Rending Throw_ Saurfang throws his weapon at a player, dealing 3000-4000 weapon damage and an additional 2000 Physical damage ever 3 seconds for 18 seconds.
'_Kor'kron Battle-Mage_
.' _Below Zero_ freezes the target in ice, stunning them.
'_Kor'kron Rocketeer_
.' _Rocket Artillery_ launches a rocket that deals 6300-7700 Fire damage to players in the targeted area.
'_Kor'kron Sergeant_
.' _Bladestorm_ lasts 6 seconds and hits up to 4 nearby players every second.
.' _Wounding Strike_ deals 200% weapon damage and applies a debuff that reduces healing by 25% for 10 seconds.
.' _Desperate Resolve_ increases their attack speed and armor by 60%.
'_Kor'kron Reaver_
.' _Desperate Resolve_ increases their attack speed and armor by 60%.
'_Kor'kron Axethrower_
.' _Shoot_ shoots at the target, dealing 4000 Physical damage.
.' _Hurl Axe_ deals 4000 Physical damage.
|modelnpc High Overlord Saurfang##36939
|modelnpc Kor'kron Battle-Mage##37117
|modelnpc Kor'kron Rocketeer##36982
|modelnpc Kor'kron Sergeant##36960
|modelnpc Kor'kron Reaver##36957
|modelnpc Kor'kron Axethrower##36968
|confirm
step
goto Icecrown Citadel/2 45.6,72.3
.talk 37200
.' Tell him "We're ready, Muradin."
.' This will start the next boss fight, make sure you are ready.
|confirm
step
goto 45.6,65.5
'_Deathbringer Saurfang_
.' Click here for 10 man abilities. |next "saur10" |confirm
.' Click here for 25 man abilities. |next "saur25" |confirm
step
label "saur10"
goto 45.6,65.5
'_Deathbringer Saurfang 10 man_
.' _Blood Link_ is a stacking buff that gives Saurfang Blood Power, increasing his size and damage dealt by 1% per stack.
.' _Blood Nova_ is an AoE that deals 10,000 Physical damage to players nearby the target.
.' _Boiling Blood_ does 9500 Physical damage every 3 seconds for 24 seconds.
.' _Call Blood Beast_ summons a blood beast add every 40 seconds.
.' _ Frenzy_ increases Saurfang's attack speed by 30%.
.' _Mark of the Fallen Champion_ causes Saurfang's attacks to splash to his target, dealing 5700-6300 additional damage. If the target dies while affected by this, Saurfang is healed for 5% of his total health.
.' _Rune of Blood_ is a debuff used on a raid member. Saurfang's attacks leech 5100-6800 additional health from players affected by this, healing him for 10 times the amount of health leeched.
|modelnpc Deathbringer Saurfang##37813
|next "saurcomplete" |confirm
step
label "saur25"
goto 45.6,65.5
'_Deathbringer Saurfang 25 man_
.' _Blood Link_ is a stacking buff that gives Saurfang Blood Power, increasing his size and damage dealt by 1% per stack.
.' _Blood Nova_ is an AoE that deals 10,000 Physical damage to players nearby the target.
.' _Boiling Blood_ does 9500 Physical damage every 3 seconds for 24 seconds.
.' _Call Blood Beast_ summons a blood beast add every 40 seconds.
.' _ Frenzy_ increases Saurfang's attack speed by 30%.
.' _Mark of the Fallen Champion_ causes Saurfang's attacks to splash to his target, dealing 5700-6300 additional damage. If the target dies while affected by this, Saurfang is healed for 5% of his total health.
.' _Rune of Blood_ is a debuff used on a raid member. Saurfang's attacks leech 5100-6800 additional health from players affected by this, healing him for 10 times the amount of health leeched.
|modelnpc Deathbringer Saurfang##37813
|next "saurcomplete" |confirm
step
label "saurcomplete"
goto Icecrown Citadel/3 51.5,16.5 |n
.' Go through the doorway here to go up to the next level of Icecrown. |goto Icecrown Citadel/5 51.8,83.4 <10 |noway |c
step
map Icecrown Citadel/5
path loop off
path	51.8,80.5	47.7,76.8	51.9,71.4
path	45.8,62.9	41.7,53.7	38.1,58.6
path	32.7,53.7	28.8,65.4	24.5,65.4
.' Follow the path, clearing trash as you make your way to the next boss, Festergut. |goto 24.5,65.4 <5 |noway |c
step
goto 19.8,65.4
'_Festergut_
.' Click here for 10 man abilities. |next "gut10" |confirm
.' Click here for 25 man abilities. |next "gut25" |confirm
step
label "gut10"
goto 19.8,65.4
'_Festergut 10 man_
.' _Gas Spore_ inflicts 2 random raid members with a gaseous spore. The spore explodes after 12 seconds, dealing 2000 damage to all nearby raid members and giving them 25% resistance to the blight. Stacks up to 3 times.
.' _Inhale Blight_ inhales the Gaseous Blight in the room, increasing damage dealt and attack speed by 25%.
.' _Pungent Blight_ occurs after _Inhale Blight_. Deals 48,750-51,250 damage to the raid and releases the blight back into the room.
.' _Vile Gas_ inflicts a plague in the targeted area, inflicting 5000 damage every 2 seconds for 6 seconds. Being hit by this causes a player to vomit, dealing an additional 4000 damage to nearby allies.
.' _Gastric Bloat_ inflicts 10,000 Nature damage to the target and applies a stacking debuff. At 10 stacks, Gastric Explosion occurs, killing the player and dealing 30,000 Shadow damage to nearby raid members within 10 yards.
|modelnpc Festergut##36626
|next "gutcomplete" |confirm
step
label "gut25"
goto 19.8,65.4
'_Festergut 25 man_
' _Gas Spore_ inflicts 2 random raid members with a gaseous spore. The spore explodes after 12 seconds, dealing 2000 damage to all nearby raid members and giving them 25% resistance to the blight. Stacks up to 3 times.
.' _Inhale Blight_ inhales the Gaseous Blight in the room, increasing damage dealt and attack speed by 25%.
.' _Pungent Blight_ occurs after _Inhale Blight_. Deals 48,750-51,250 damage to the raid and releases the blight back into the room.
.' _Vile Gas_ inflicts a plague in the targeted area, inflicting 5000 damage every 2 seconds for 6 seconds. Being hit by this causes a player to vomit, dealing an additional 4000 damage to nearby allies.
.' _Gastric Bloat_ inflicts 10,000 Nature damage to the target and applies a stacking debuff. At 10 stacks, Gastric Explosion occurs, killing the player and dealing 30,000 Shadow damage to nearby raid members within 10 yards.
.' _Putrid Hate_ is an AoE that deals large damage in a 200 yard radius. This also applies a debuff to nearby raid members, reducing their movement speed by 25%.
|modelnpc Festergut##36626
|next "gutcomplete" |confirm
step
label "gutcomplete"
goto 23.5,63.6
.' Make sure to click the Gas Release Valve before leaving the room. This, along with another valve, must be used to access a later boss.
|confirm
step
map Icecrown Citadel/5
path loop off
path	29.6,54.1	29.2,42.3	24.7,42.1
.' Follow the path, clearing trash as you make your way to the next boss, Rotface. |goto 24.7,42.1 <5 |noway |c
step
goto 19.8,41.9
'_Rotface_
.' Click here for 10 man abilities. |next "rot10" |confirm
.' Click here for 25 man abilities. |next "rot25" |confirm
step
label "rot10"
goto 19.8,41.9
'_Rotface 10 man_
.' _Ooze Flood_ inflicts 5400-5700 damage every second and also reduces the target's movement speed by 25% for 5 seconds.
.' _Slime Spray_ summons a green ooze rain, dealing 5500 Nature damage every second for 5 seconds to players in a _cone_ in front of Rotface.
.' _Mutated Infection_ inflicts 4000 Shadow damage every second and reduces healing received by 50% for 12 seconds. Upon expiration, a small ooze is created at the target's location.
'_Little Ooze_
.' _Sticky Ooze_ deals 3000 damage every second and reduces movement speed by 50%.
.' _Weak Radiating Ooze_ deals 3500 damage every 2 seconds to players within the targeted area. Also causes Little Ooze to merge together, creating a Big Ooze.
'_Big Ooze_
.' _Sticky Ooze_ deals 3000 damage every second and reduces movement speed by 50%.
.' _Radiating Ooze_ deals 4500 damage every 2 seconds to players within the targeted area. Also causes Big Oozes to merge together, applying a buff that increases their damage dealt by 20% and stacking up to 10 times.
.' _Unstable Ooze Explosion_ occurs when a Big Ooze merges with 5 other oozes.
|modelnpc Rotface##36627
|next "rotcomplete" |confirm
step
label "rot25"
goto 19.8,41.9
'_Rotface 25 man_
.' _Ooze Flood_ inflicts 5400-5700 damage every second and also reduces the target's movement speed by 25% for 5 seconds.
.' _Slime Spray_ summons a green ooze rain, dealing 5500 Nature damage every second for 5 seconds to players in a _cone_ in front of Rotface.
.' _Mutated Infection_ inflicts 4000 Shadow damage every second and reduces healing received by 50% for 12 seconds. Upon expiration, a small ooze is created at the target's location.
'_Little Ooze_
.' _Sticky Ooze_ deals 3000 damage every second and reduces movement speed by 50%.
.' _Weak Radiating Ooze_ deals 3500 damage every 2 seconds to players within the targeted area. Also causes Little Ooze to merge together, creating a Big Ooze.
'_Big Ooze_
.' _Sticky Ooze_ deals 3000 damage every second and reduces movement speed by 50%.
.' _Radiating Ooze_ deals 4500 damage every 2 seconds to players within the targeted area. Also causes Big Oozes to merge together, applying a buff that increases their damage dealt by 20% and stacking up to 10 times.
.' _Unstable Ooze Explosion_ occurs when a Big Ooze merges with 5 other oozes.
|modelnpc Rotface##36627
|next "rotcomplete" |confirm
step
label "rotcomplete"
goto 23.5,43.9
.' Make sure to click the Ooze Release Valve before leaving the room. This opens the door to the next boss.
|confirm
step
map Icecrown Citadel/5
path loop off
path	24.4,53.4	18.4,53.7	17.3,53.7
.' Kill the Flesh-eating Insects in this hallway until the door opens to the next boss, Professor Putricide. |goto 17.3,53.7 <5 |noway |c
step
goto 13.3,53.7
'_Professor Putricide_
.' Click here for 10 man abilities. |next "prof10" |confirm
.' Click here for 25 man abilities. |next "prof25" |confirm
step
label "prof10"
goto 13.3,53.7
'_Professor Putricide 10 man_
.' _Mutated Slime_ vial of Mutated Slime is thrown at the target, creating a puddle that deals 4500 damage every second until it is consumed by another Mutated creature.
.' _Unstable Experiment_ Putricide performs an experiment, spawning a Volitile Ooze or a Gas Cloud. The Ooze should be killed and the Cloud should be kited.
.' _Choking Gas Bomb_ is an AoE around Putricide, dealing 5500 damage every second and then explodes for 16,000 damage after 20 seconds.
.' _Malleable Goo_ Putricide bounces a Malleable ball of Green Goo at his target, inflicting 14,500-15,300 damage and reducing their attack and cast speed by 200% for 15 seconds.
.' _Mutated Plague_ occurs during Phase 3. This is applied to Putricide's target every 10 seconds, dealing increased Shadow damage to the raid. Kill him as fast as possible to avoid death.
|modelnpc Professor Putricide##36678
|next "profcomplete" |confirm
step
label "prof25"
goto 13.3,53.7
'_Professor Putricide 25 man_
.' _Mutated Slime_ vial of Mutated Slime is thrown at the target, creating a puddle that deals 4500 damage every second until it is consumed by another Mutated creature.
.' _Unstable Experiment_ Putricide performs an experiment, spawning a Volitile Ooze or a Gas Cloud. The Ooze should be killed and the Cloud should be kited.
.' _Choking Gas Bomb_ is an AoE around Putricide, dealing 5500 damage every second and then explodes for 16,000 damage after 20 seconds.
.' _Malleable Goo_ Putricide bounces a Malleable ball of Green Goo at his target, inflicting 14,500-15,300 damage and reducing their attack and cast speed by 200% for 15 seconds.
.' _Mutated Plague_ occurs during Phase 3. This is applied to Putricide's target every 10 seconds, dealing increased Shadow damage to the raid. Kill him as fast as possible to avoid death.
|modelnpc Professor Putricide##36678
|next "profcomplete" |confirm
step
label "profcomplete"
map Icecrown Citadel/5
path loop off
path	32.5,53.7	37.5,58.6	41.7,53.8
path	51.9,38.5	51.8,20.0
.' Follow the path, clearing trash as you make your way to the next boss, the Blood Prince Council. |goto 51.8,20.0 <5 |noway |c
step
goto 51.8,14.5
'_Blood Prince Council_
.' Click here for 10 man abilities. |next "council10" |confirm
.' Click here for 25 man abilities. |next "council25" |confirm
step
label "council10"
goto 51.8,14.5
'_Blood Prince Council_
'_Prince Valanar_
.' _Shock Vortex_ creates an AoE vortex that deals 8000 damage to players within 12 yards and knocks them back.
.' _Kinetic Bomb_ summons a bomb that drifts toward the ground, exploding on impact and dealing 10,800-13,200 damage to players and knocking them back.
.' _Empowered Shock Vortex_ creates vortexes under players near Valanar, dealing 5000 damage and knocking them back.
'_Prince Taldaram_
.' _Conjure Flame_ creates a ball of flames that flies through the air, growing in size and dealing at least 7500 Fire damage to players near the impact. Deals increased damage based on size.
.' _Conjure Empowered Flame_ created an empowered ball of flame that flies towards the target and explodes on impact, dealing at least 7500 Fire damage. Also inflicts 1000 Fire damage to nearby players.
.' _Glittering Sparks_ sparks shoot from Taldaram in a _cone_, burning players for 13,000 Fire damage over 8 seconds and reducing their movement speed by 20%.
'_Prince Keleseth_
.' _Shadow Lance_ deals 16,000 Shadow damage to the target.
.' _Shadow Resonance_ affects the nearest target, dealing 1000 damage and reducing Shadow damage taken from all sources by 35%.
.' _Empowered Shadow Lance_ deals 78,000-82,000 damage to the target.
|modelnpc Prince Valanar##37970
|modelnpc Prince Taldaram##37973
|modelnpc Prince Keleseth##37972
|next "councilcomplete" |confirm
step
label "council25"
goto 51.8,14.5
'_Blood Prince Council_
'_Prince Valanar_
.' _Shock Vortex_ creates an AoE vortex that deals 9000 damage to players within 12 yards and knocks them back.
.' _Kinetic Bomb_ summons a bomb that drifts toward the ground, exploding on impact and dealing 12,000 damage to the entire raid and knocks them back.
.' _Empowered Shock Vortex_ creates vortexes under players near Valanar, dealing 7000 damage and knocking them back.
'_Prince Taldaram_
.' _Conjure Flame_ creates a ball of flames that flies through the air, growing in size and dealing at least 10,000 Fire damage to players near the impact. Deals increased damage based on size.
.' _Conjure Empowered Flame_ created an empowered ball of flame that flies towards the target and explodes on impact, dealing at least 10,000 Fire damage. Also inflicts 1000 Fire damage to nearby players.
.' _Glittering Sparks_ sparks shoot from Taldaram in a _cone_, burning players for 16,100 Fire damage over 8 seconds and reducing their movement speed by 20%.
'_Prince Keleseth_
.' _Shadow Lance_ deals 17,000-18,000 Shadow damage to the target.
.' _Shadow Resonance_ affects the nearest target, dealing 1000 damage and reducing Shadow damage taken from all sources by 35%.
.' _Empowered Shadow Lance_ deals 85,000-89,600 damage to the target.
|modelnpc Prince Valanar##37970
|modelnpc Prince Taldaram##37973
|modelnpc Prince Keleseth##37972
|next "councilcomplete" |confirm
step
label "councilcomplete"
map Icecrown Citadel/6
path loop off
path	Icecrown Citadel/5 46.7,14.1	Icecrown Citadel/5 42.7,17.0	Icecrown Citadel/6 30.7,59.8
path	51.1,73.6	51.1,59.8
.' Follow the path, clearing trash as you make your way to the next boss, Blood-Queen Lana'thel. |goto 51.1,59.8 <5 |noway |c
step
goto 51.1,43.8
'_Blood-Queen Lana'thel_
.' Click here for 10 man abilities. |next "lana10" |confirm
.' Click here for 25 man abilities. |next "lana25" |confirm
step
label "lana10"
goto 51.1,43.8
'_Blood-Queen Lana'thel 10 man_
'_Ground Phase_
.' _Blood Mirror_ a debuff that inflicts 100% of the damage the target takes to the linked player.
.' _Delirious Slash_ inflicts 50% of weapon damage and applies a bleed DoT that inflicts 4500-5500 every 3 seconds for 15 seconds.
.' _Vampiric Bite_ deals 12,000-14,000 damage to the target, also applying _Essence of the Blood Queen_.
.' _Essence of the Blood Queen_ infuses the target with the blood of the Vampyr Queen, increasing damage by 100% and healing them for 10% of damage inflicted.
.' _Pact of the Darkfallen_  deals 5000 Shadow damage every 2 seconds to the target and nearby players. Expires when all linked players are within 5 yards.
.' _Shroud of Sorrow_ deals 4500 Shadow damage every 3 seconds to players within 40 yards of Lana'thel.
.' _Swarming Shadow_ is a mass of shadows that appear under the target, dealing 2500 Shadow damage every 2 seconds.
.' _Twilight Blootbolt_ deals 9200-10,750 damage to the target and surrounding players.
'_Air Phase_
.' _Incite Terror_ is a 4 second Fear on the entire raid.
.' _Bloodbolt Whirl_ summons a maelstrom of Bloodbolts every 2 seconds for 6 seconds, these deal 10,000 damage to the targeted player and their surrounding allies.
|modelnpc Blood-Queen Lana'thel##37955
|next "lanacomplete" |confirm
step
label "lana25"
goto 51.1,43.8
'_Blood-Queen Lana'thel 25 man_
'_Ground Phase_
.' _Blood Mirror_ a debuff that inflicts 100% of the damage the target takes to the linked player.
.' _Delirious Slash_ inflicts 50% of weapon damage and applies a bleed DoT that inflicts 5500-6500 every 3 seconds for 15 seconds.
.' _Vampiric Bite_ deals 12,000-14,000 damage to the target, also applying _Essence of the Blood Queen_.
.' _Essence of the Blood Queen_ infuses the target with the blood of the Vampyr Queen, increasing damage by 100% and healing them for 10% of damage inflicted.
.' _Pact of the Darkfallen_  deals 5000 Shadow damage every 2 seconds to the target and nearby players. Expires when all linked players are within 5 yards.
.' _Shroud of Sorrow_ deals 4500 Shadow damage every 3 seconds to players within 40 yards of Lana'thel.
.' _Swarming Shadow_ is a mass of shadows that appear under the target, dealing 2500 Shadow damage every 2 seconds.
.' _Twilight Blootbolt_ deals 9200-10,750 damage to the target and surrounding players.
'_Air Phase_
.' _Incite Terror_ is a 4 second Fear on the entire raid.
.' _Bloodbolt Whirl_ summons a maelstrom of Bloodbolts every 2 seconds for 6 seconds, these deal 10,000 damage to the targeted player and their surrounding allies.
|modelnpc Blood-Queen Lana'thel##37955
|next "lanacomplete" |confirm
step
label "lanacomplete"
goto 51.1,69.7 |n
.' Jump down the hole here |goto Icecrown Citadel/5 51.9,33.6 <10 |noway |c
step
map Icecrown Citadel/5
path loop off
path	51.8,38.5	61.7,53.8	69.3,53.8
path	74.4,53.7	76.6,58.4	76.6,62.7
.' Follow the path, clearing trash as you make your way to the next boss, Valithria Dreamwalker. |goto 76.6,62.7 <5 |noway |c
step
goto 76.6,73.7
'_Valithria Dreamwalker_
.' This fight ends after Valithria is healed back to 100% health.
.' Most healers should focus on Valithria, while the rest of the raid group take care of the adds that continuously spawn.
.' Click here for 10 man abilities. |next "dream10" |confirm
.' Click here for 25 man abilities. |next "dream25" |confirm
step
label "dream10"
goto 76.6,73.7
'_Valithria Dreamwalker 10 man_
.' _Nightmare Portal_ summons Nightmare Portals that allow access to the Dreamstate. Valithria's healers should use these and absorb the Dream Clouds that spawn there, granting them _Emerald Vigor_.
.' _Emerald Vigor_ is a buff that regenerates 200 mana every 3 seconds and increases damage and healing done by 10%.
.' _Dreamwalker's Rage_ occurs when Valithria reaches 100% health. It deals 10,000,000 Nature damage to all surrounding enemies. Does not hit players.
'_Blazing Skeleton_
.' _Fireball_ deals 4700-5300 Fire damage to the target.
.' _Lay Waste_ surrounds the skeleton in flames for 12 seconds, dealing 4000 Fire damage to the raid every 2 seconds.
'_Blistering Zombie_
.' _Acid Burst_ explodes the zombie, dealing 9400-10,500 Nature damage and 750 Nature damage every second to all players within 15 yards.
.' _Corrosion_ is a DoT applied to a player after being hit by a melee attack, dealing 2000 Nature damage every 3 seconds and reducing their armor by 10%. Stacks up to 5 times.
'_Risen Archmage_
.' _Column of Frost_ marks a location under a players feet. After 2 seconds frost erupts, dealing 12,000 Frost damage to players within 3 yards and knocking them into the air.
.' _Frostbolt Volley_ deals 8500-10,500 Frost damage to nearby players, also reducing their movement speed for 4 seconds.
.' _Mana Void_ summons a void at a random player's location, burning 800 mana per second to all players within 6 yards.
'_Gluttonous Abomination_
.' _Gut Spray_ deals 20000 Nature damage per second and increases the target's Physical damage taken by 25% for 12 seconds.
|modelnpc Valithria Dreamwalker##36789
|next "dreamcomplete" |confirm
step
label "dream25"
goto 76.6,73.7
'_Valithria Dreamwalker 25 man_
.' _Nightmare Portal_ summons Nightmare Portals that allow access to the Dreamstate. Valithria's healers should use these and absorb the Dream Clouds that spawn there, granting them _Emerald Vigor_.
.' _Emerald Vigor_ is a buff that regenerates 200 mana every 3 seconds and increases damage and healing done by 10%.
.' _Dreamwalker's Rage_ occurs when Valithria reaches 100% health. It deals 10,000,000 Nature damage to all surrounding enemies. Does not hit players.
'_Blazing Skeleton_
.' _Fireball_ deals 4700-5300 Fire damage to the target.
.' _Lay Waste_ surrounds the skeleton in flames for 12 seconds, dealing 4000 Fire damage to the raid every 2 seconds.
'_Blistering Zombie_
.' _Acid Burst_ explodes the zombie, dealing 9400-10,500 Nature damage and 750 Nature damage every second to all players within 15 yards.
.' _Corrosion_ is a DoT applied to a player after being hit by a melee attack, dealing 2000 Nature damage every 3 seconds and reducing their armor by 10%. Stacks up to 5 times.
'_Risen Archmage_
.' _Column of Frost_ marks a location under a players feet. After 2 seconds frost erupts, dealing 12,000 Frost damage to players within 3 yards and knocking them into the air.
.' _Frostbolt Volley_ deals 8500-10,500 Frost damage to nearby players, also reducing their movement speed for 4 seconds.
.' _Mana Void_ summons a void at a random player's location, burning 800 mana per second to all players within 6 yards.
'_Gluttonous Abomination_
.' _Gut Spray_ deals 20000 Nature damage per second and increases the target's Physical damage taken by 25% for 12 seconds.
|modelnpc Valithria Dreamwalker##36789
|next "dreamcomplete" |confirm
step
label "dreamcomplete"
goto Icecrown Citadel/5 76.7,92.9 |n
.' Ride this elevator down |goto Icecrown Citadel/4 36.6,88.7 <10 |noway |c
step
goto 36.6,67.5
.' Clear the waves of adds that spawn in this room until the northern gate at [36.5,50.1] opens.
|confirm
step
goto 36.5,35.5
.' Clear the two groups of trash here, leaving Spinestalker and Rimefang alive. Once you kill those two the boss, Sindragosa, will spawn.
|modelnpc Spinestalker##37534
|modelnpc Rimefang##37533
|confirm
step
goto 36.6,23.4
'_Sindragosa_
.' Click here for 10 man abilities. |next "sin10" |confirm
.' Click here for 25 man abilities. |next "sin25" |confirm
step
label "sin10"
goto 36.6,23.4
'_Sindragosa 10 man_
'_Ground Phase_
.' _Cleave_ hits Sindragosa's target and up to 10 additional nearby players. Make sure the tank is the only one in front of her when this occurs.
.' _Frost Aura_ deals 3000 Frost damage every 3 seconds to all players near Sindragosa.
.' _Frost Breath_ inflicts 27,750-32,250 Frost damage to raid members in a 60 yard _cone_ in front of Sindragosa. Also reduces their attack speed by 50% and movement speed by 15% for 90 seconds.
.' _Ice Grip_ pulls nearby players to the caster and then uses _Blistering Cold_.
.' _Blistering Cold_ deals 30,000 Frost damage to players within 25 yards.
.' _Chilled to the Bone_ deals 1000 Frost damage every 2 seconds per stack.
.' _Permeating Chill_ affects players attacking Sindragosa with physical attacks. They receive this debuff, dealing 1000 Frost damage every 2 seconds per stack.
.' _Tail Smash_ deals 11,250-18,750 damage on players behind Sindragosa.
'_Air Phase_
.' _Frost Beacon_ Marks the targeted player for imprisonment in an Ice Tomb, dealing 8% of their maximum health in damage every second if not freed by other players.
.' _Frost Bomb_ deals 23,500-26,400 Frost damage to all nearby raid members.
'_Final Phase_
.' _Mystic Buffet_ buffets all players near Sindragosa with Arcane energy, increasing all magic damage taken by 20% for stack.
|modelnpc Sindragosa##36853
|next "sincomplete" |confirm
step
label "sin25"
goto 36.6,23.4
'_Sindragosa 25 man_
'_Ground Phase_
.' _Cleave_ hits Sindragosa's target and up to 10 additional nearby players. Make sure the tank is the only one in front of her when this occurs.
.' _Frost Aura_ deals 4500 Frost damage every 3 seconds to all players near Sindragosa.
.' _Frost Breath_ inflicts 37,000-43,000 Frost damage to raid members in a 60 yard _cone_ in front of Sindragosa. Also reduces their attack speed by 50% and movement speed by 15% for 90 seconds.
.' _Ice Grip_ pulls nearby players to the caster and then uses _Blistering Cold_.
.' _Blistering Cold_ deals 35,000 Frost damage to players within 25 yards.
.' _Chilled to the Bone_ deals 1000 Frost damage every 2 seconds per stack.
.' _Permeating Chill_ affects players attacking Sindragosa with physical attacks. They receive this debuff, dealing 1000 Frost damage every 2 seconds per stack.
.' _Tail Smash_ deals 11,250-18,750 damage on players behind Sindragosa.
'_Air Phase_
.' _Frost Beacon_ Marks the targeted player for imprisonment in an Ice Tomb, dealing 8% of their maximum health in damage every second if not freed by other players.
.' _Frost Bomb_ deals 23,500-26,400 Frost damage to all nearby raid members.
'_Final Phase_
.' _Mystic Buffet_ buffets all players near Sindragosa with Arcane energy, increasing all magic damage taken by 20% for stack.
|modelnpc Sindragosa##36853
|next "sincomplete" |confirm
step
label "sincomplete"
map Icecrown Citadel/4
path loop off
path	39.4,44.1	46.9,33.4	51.7,33.3
.' Follow this path to the elevator and take it up to the next level. |goto Icecrown Citadel/5 86.0,53.8 <10 |noway |c
step
map Icecrown Citadel/5
path loop off
path	80.7,53.8	67.7,53.7	59.2,53.7
path	52.0,53.7
.' Follow the path to the teleporter and step through it, taking you to the final boss, the Lich King. |goto Icecrown Citadel/7 49.4,39.4 <5 |noway |c
step
goto 49.8,50.4
.talk 38995
.' Tell him "We are prepared, Highlord..."
.' This will start the final battle with the Lich King, make sure you are ready.
|confirm
step
goto 49.8,50.4
'_The Lich King_
'_Phase 1_
.' Adds in this Phase should be tanked away from the group and killed using Necrotic Plague. This is a DoT that is used on players, but when dispelled (or the player dies) it jumps to a nearby unit, hopefully the summoned adds.
.' _Summon Drudge Ghouls_ spawns 3 Drudge Ghoulds to attack the raid.
.' _Summon Shambling Horror_ spawns a Shambling Horror to attack the raid. These have a _frontal cone_ attack and _enrage_.
.' _Nectrotic Plague_ is a DoT the Lich King uses on a player, dealing 50,000 damage over 5 seconds for 15 seconds. If the player dies, or if it is dispelled, it jumps up to 10 yards to a nearby unit. You want it to jump to the summoned adds.
.' _Infest_ deals 6500-7500 Shadow damage to the raid, also increasing their Shadow damage taken.
.' _Summon Shadow Trap_ the Lich King spawns a Shadow Trap under the targeted player's feet. Stepping within 5 yards of the trap causes it to explode, dealing 23,500-26,500 Shadow damage to players within 10 yards and knocking them back.
|modelnpc The Lich King##36597
.' _Phase 1.5_ starts when the Lich King reaches 70% health.
.' Click here to move to _Phase 1.5_. |confirm
step
goto 49.8,50.4
'_The Lich King_
'_Phase 1.5_
.' _Remorseless Winter_ creates a large winter storm that deals 7000-8000 Frost damage every second to all raid members within 45 yards.
.' _Pain and Suffering_ deals 2800-3200 Shadow damage and an additional 500 Shadow damage every second for 3 seconds in a _frontal cone_.
.' _Summon Ice Sphere_ spawns an Ice Sphere that moves towards its target. If it reaches them it deals 9400-10,600 Frost damage to all players within 10 yards, also knocking them back.
.' _Raging Spirit_ rips out a piece of the target's spirit, spawning a Raging Spirit that deals 20,000 Shadow damage to all players in a 15 yard _frontal cone_ and silencing them for 5 seconds.
.' _Quake_ occurs at the _end of the phase_, the Lich King sends out shockwaves, causing the edge of the platform to crumble.
|modelnpc The Lich King##36597
.' Click here to move to _Phase 2_. |confirm
step
goto 49.8,50.4
'_The Lich King_
'_Phase 2_
.' _Infest_ deals 6500-7500 Shadow damage to the raid, also increasing their Shadow damage taken.
.' _Soul Reaper_ hits the target for 50% weapon damage and applies Soul Reaper, which deals 50,000 Shadow damage after 5 seconds. Also increases the Lich King's haste by 100% for 5 seconds.
.' _Defile_ creates a defiled area under a player's feet, dealing damage to anyone inside it and growing in size whenever it deals damage.
.' _Summon Val'kyr_ spawns a Val'kyr Shadowguard, which picks up a random player and tries to throw them off the edge of the platform if not killed in time.
.' _Phase 2.5_ starts when the Lich King falls below 40% health.
|modelnpc The Lich King##36597
.' Click here to advance to _Phase 2.5_. |confirm
step
goto 49.8,50.4
'_The Lich King_
'_Phase 2.5_
.' _Remorseless Winter_ creates a large winter storm that deals 7000-8000 Frost damage every second to all raid members within 45 yards.
.' _Pain and Suffering_ deals 2800-3200 Shadow damage and an additional 500 Shadow damage every second for 3 seconds in a _frontal cone_.
.' _Summon Ice Sphere_ spawns an Ice Sphere that moves towards its target. If it reaches them it deals 9400-10,600 Frost damage to all players within 10 yards, also knocking them back.
.' _Raging Spirit_ rips out a piece of the target's spirit, spawning a Raging Spirit that deals 20,000 Shadow damage to all players in a 15 yard _frontal cone_ and silencing them for 5 seconds.
.' _Quake_ occurs at the _end of the phase_, the Lich King sends out shockwaves, causing the edge of the platform to crumble.
|modelnpc The Lich King##36597
.' Click here to proceed to _Phase 3_. |confirm
step
goto 49.8,50.4
'_The Lich King_
'_Phase 3_
.' _Soul Reaper_ hits the target for 50% weapon damage and applies Soul Reaper, which deals 50,000 Shadow damage after 5 seconds. Also increases the Lich King's haste by 100% for 5 seconds.
.' _Defile_ creates a defiled area under a player's feet, dealing damage to anyone inside it and growing in size whenever it deals damage.
.' _Vile Spirits_ spawns 10 Vile Spirit adds which attack the raid after 30 seconds. They can explode when near players, dealing 20,000 Shadow damage to players within 5 yards.
.' _Harvest Souls_ deals 7500 Shadow damage every second for 6 seconds. If the targets are still alive when this expires, they are sent _into Frostmourne._
'_Inside Frostmourne_
.' There is an NPC and a mob inside. A tank can pick up the mob, who will also need to be interrupted. Healers can heal the NPC and all DPS have 60 seconds to kill the mob. Failure to kill the mob in time results in all players' deaths.
.' _Phase 4_ starts when the Lich King reaches 10% health.
|modelnpc The Lich King##36597
.' Click here to advance to _Phase 4_. |confirm
step
goto 49.8,50.4
'_The Lich King_
'_Phase 4_
.' _Fury of Frostmourne_ occurs at the beginning of Phase 4, dealing Shadow damage to the entire raid.
.' _Raise Dead_ raises the dead back to life to fight again for the Lich King.
.' _Enrage_ occurs after 15 minutes of combat. It increases his damage by 900% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc The Lich King##36597
|confirm
step
.' Congratulations, you have finished the Icecrown Citadel raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Cataclysm\\Baradin Hold",[[
dungeon 752
description This guide will walk you through the Baradin Hold Raid.
author support@zygorguides.com
step
.' This raid can only be completed if your faction controls Baradin Hold, the main fortress on Tol Barad.
|confirm
step
map Baradin Hold/1
path loop off
path	48.0,72.5	54.6,62.9	70.8,62.9
.' Follow the path, clearing trash as you make your way to the first boss, Argaloth. |goto 70.8,62.9 <5 |noway |c
step
goto 80.4,63.4
'_Argaloth_
.' _Meteor Slash_ deals 200,000 Fire damage, split between players within 65 yards in front of Argaloth, also increasing their fire damage taken by 100%.
.' _Consuming Darkness_ the Shambling Doom inflicts 3000 Shadow damage and applies a DoT that deals additional Shadow damage every 0.5 seconds for 15 seconds. Should be dispelled immediately.
.' _Fel Firestorm_ occurs when Argaloth is at 66% and 33% health. He rains fireballs down on the raid, leaving scorch marks on the ground upon impact. These marks deal 8500 Fire damage when standing in them.
.' _Berserk_ is an _enrage_, occuring after 5 minutes of combat. It increases his damage by 900% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Argaloth##47120
|confirm
step
goto 15.5,63.1
'_Occu'thar_
.' _Eyes of Occu'thar_ spawns a swarm of eyes that attach to players, inflicting 8700-9200 Shadow damage every second. After 10 seconds they explode, dealing 25,000 Shadow damage to the raid.
.' _Focused Fire_ Occu'thar targets a player, then inflicts 25,000 Fire damage every second to players within 12 yards of the targeted location.
.' _Searing Shadows_ deals 105,000 Shadow damage to players in a cone in front of Occu'thar, also increasing Shadow damage they take by 100% for 30 seconds.
.' _Berserk_ is an _enrage_, occuring after 8 minutes of combat. It increases her damage by 900% and attack speed by 150%. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
|modelnpc Occu'thar##52363
|confirm
step
goto 47.9,17.9
'_Alizabal_
.' _Skewer_ skewers and stuns Alizabal's target for 8 seconds, dealing 10,000 Physical damage every 2 seconds and increasing the target's damage taken by 150%.
.' _Seething Hate_ affects a random player, dealing 200,000 total Fire damage, split between all players within 6 yards of the target.
.' _Blade Dance_ Alizabal enters a blade dance, dealing 12,500 Physical damage every second to all players within 13 yards of her and deflecting all incoming attacks.
.' _Berserk_ is an _enrage_, occuring after 5 minutes of combat. It increases her damage by 900% and attack speed by 150%. This is designed to wipe the raid. Kill her before she reaches this stage to avoid death.
|modelnpc Alizabal##55869
|confirm
step
.' Congratulations, you have finished the Baradin Hold raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Cataclysm\\Blackwing Descent",[[
dungeon 754
description This guide will walk you through the Blackwing Descent Raid.
author support@zygorguides.com
step
map Blackwing Descent/1
path loop off
path	42.6,59.9	36.1,59.7	31.5,56.9
.' Follow the path, clearing trash as you make your way to the first boss, Magmaw. |goto 31.5,56.9 <5 |noway |c
step
goto Blackwing Descent/1 27.5,53.1
'_Magmaw_
.' _Mangle_ deals 110,000-128,400 damage every 5 seconds. Usually used on tanks.
.' _Sweltering Armor_ Magmaw melts the taget's armor, reducing it by 50%.
.' _Magma Spit_ inflicts 39,300-50,600 Fire damage to the targeted player when no player is in melee range. Triggers _Molten Tantrum_.
.' _Molten Tantrum_ with no one nearby to attack, Magmaw goes into a frenzy, increasing Fire damage dealt by 100% and stacking up to 10 times.
.' _Lava Spew_ deals 31,400-36,500 Fire damage to all players within 60 yards every second, lasting 3 seconds.
.' _Parasitic Infection_ deals 12,000-14,000 damage every 2 seconds, causing _Infectious Vomit_ after 10 seconds.
.' _Infectious Vomit_ deals 40,000 damage to all players within 8 yards of the affected player.
.' _Pillar of Flame_ Magmaw launches magma at a player, dealing 25,000 damage and sundering the ground, which deals 120,000 Fire damage and a knockback to players near the affected spot.
.' _Massive Crash_ Magmaw slams down onto the ground, stunning any players caught underneath him for 3 seconds.
.' When Magmaw slumps forward players can jump on his head and use constricting chains to impale him on the spike in the room, increasing his vulnerability.
|modelnpc Magmaw##41570
|confirm
step
map Blackwing Descent/1
path loop off
path	51.2,59.9	58.3,59.7	61.0,57.3
.' Follow the path, clearing trash as you make your way to the next boss, the Omnotron Defense System. |goto 61.0,57.3 <5 |noway |c
step
goto Blackwing Descent/1 65.4,57.4
'_Omnotron Defense System_
'_Arcanotron_
.' _Power Generator_ is a large vortex on the ground, buffing any who stand in it with increased damage and mana regeneration. Try to keep the boss out of it.
.' _Arcane Annihilator_ is interruptable and deals massive damage to a single target.
.' _Power Conversion_ causes Arcanotron to gain a stacking damage buff every time he takes damage. Mages can steal this buff.
'_Electron_
.' _Lightning Conductor_ places a debuff on one player that hits all nearby players for a large amount of damage. Those affected by this should move away from the rest of the group.
.' _Electrical Discharge_ deals 24,000 Nature damage and jumps to additional players within 8 yards.
.' _Unstable Shield_ deals 30,000 Nature damage to the attacker and all players within 6 yards of the attacker, whenever Electron is hit with an attack.
'_Magmatron_
.' _Incineration Security Measure_ shoots out jets of flame all over the room. Unlimited range and unavoidable.
.' _Acquiring Target_ targets a raid member and shoots a jet of flame at them after a few seconds. Deals 21,000 Fire damage every second for 4 seconds. Avoid it and heal the targeted player.
.' _Flamethrower_ targets the tank, launching a jet of flame at them.
.' _Barrier_ absorbs 300,00 damage and explodes to deal 75,000 damage to the raid if broken through.
'_Toxitron_
.' _Poison Cloud_ is a pool on the ground that increases all damage taken by 50%, including Toxitron.
.' _Poison Protocol_ spawns 3 Poison Bombs that move towards a chosen target. If they reach their target, they explode for about 100,000 damage and leave a pool of poison.
.' _Poison Soaked Shell_ occurs when Toxitron is attacked, it applies a stacking DoT to the attacker but also applies a buff that makes your attacks deal an additional 10,000 Nature damage.
|modelnpc Arcanotron##42166
|modelnpc Electron##42179
|modelnpc Magmatron##42178
|modelnpc Toxitron##42180
|confirm
step
map Blackwing Descent/1
path loop off
path	58.8,54.7	47.1,53.3
map Blackwing Descent/2
path	47.4,88.1	57.6,69.5	65.5,69.7
.' Follow the path, clearing trash as you make your way to the next boss, Maloriak. |goto Blackwing Descent/2 65.5,69.7 <10 |noway |c
step
goto Blackwing Descent/2 71.4,70.2
'_Maloriak_
'_All Phases_
.' _Arcane Storm_ deals 14,100-15,800 Arcane damage every second for 6 seconds to the entire raid.
.' _Release Abberations_ releases 3 Abberations from their growth chamber, causing them to attack all players.
'_Red Vial Phase_ - "Mix and Stir! Apply heat!"
.' _Consuming Flames_ ignites a random player, dealing 9000 Fire damage every second for 10 seconds. Any other magic damage taken while this is in effect increases its damage dealt by 25%.
.' _Scorching Blast_ deals 750,000 total Fire damage, split between all players in a 60 yard _cone_ in front of Maloriak.
'_Blue Vial Phase_ - "How well does the mortal shell handle extreme temperature change? Must find out! For Science!"
.' _Biting Chill_ surrounds a random player with a ring of frost that deals 7500 Frost damage to the target and all players within 8 yards every second for 10 seconds.
.' _Flash Freeze_ affects a random player, dealing 50,000 Frost damage to them and all players within 10 yards and encasing them in ice for 30 seconds. When the ice breaks it deals an additional 50,000 Frost damage.
'_Green Vial Phase_ - "This one's a little unstable! But what's progress without failure?"
.' _Remedy_ heals Maloriak for 150,000 health and 2000 mana every second for 10 seconds.
.' _Debilitating Slime_ coats every unit in the room with slime, increasing damage taken by 100% and suppresses any Growth Catalysts on the target, temporarily removing it.
'_25% Health_ - "When pushed to the edge, results may become unpredictable!"
.' _Release All_ releases all remaining Aberrations from their growth chambers. Also releases two Prime Subjects from their chambers.
.' _Magma Jets_ releases a stream of fire towards a random player. These deal 25,000 Fire damage initially and leaves a burning effect on the ground that deals 5000 Fire damage.
.' _Absolute Zero_ summons a sphere of energy near a random player. If this sphere comes within 6 yards of a player, it explodes and deals 20,000 Frost damage to all players within 6 yards, also knocking them back.
.' _Acid Nova_ deals 5000 Nature damage every second for 10 seconds.
|modelnpc Maloriak##41378
|confirm
step
map Blackwing Descent/2
path loop off
path	65.5,69.7	57.6,69.5	47.4,53.0
path	47.5,40.9
.' Follow the path, clearing trash as you make your way to the next boss, Atramedes. |goto Blackwing Descent/2 47.5,40.9 <5 |noway |c
step
goto Blackwing Descent/2 47.5,32.7
'_Atramedes_
.' This fight has a Sound Bar. If it gets to 100 Atramedes will aggro and kill you. Clicking one of the Ancient Dwarven Shields around the room to reset the raid's sound to 0.
.' _Vertigo_ increases Atramedes' damage taken by 50%.
'_Ground Phase_
.' _Sonar Pulse_ discs of energy fly around the room, dealing 6000 Arcane damage if touched and adds 7 Sound.
.' _Sonic Breath_ is cast on the player with the highest Sound. It deals 15,000 damage and 20 Sound every second.
.' _Modulation_ increases the raid's Sound by 7 and dealing 40,000 Shadow damage.
.' _Searing Flame_ deals 15,000 Fire damage every second for 8 seconds and increases Fire damage taken by 25% per stack. Adds 13-15 Sound.
'_Air Phase_
.' _Roaring Flame Breath_ is used on the player with the highest Sound, dealing 15,000 damage and 20 Sound every second.
.' _Sonar Pulse_ discs of energy fly around the room, dealing 6000 Arcane damage if touched and adds 7 Sound.
.' _Sonar Bomb_ deals 30,000 Arcane damage to all players within 8 yards and adds 30 Sound.
.' _Sonic Fireball_ deals 30,000 Fire damage to all players within 6 yards of the impact.
.' _Roaring Flame_ throws flame patches around the room, dealing 15,000 Fire damage and an additional 8000 Fire damage every second for 4 seconds. Increases Sound by 5.
|modelnpc Atramedes##41442
|confirm
step
map Blackwing Descent/2
path loop off
path	47.5,40.9	47.4,53.0	36.7,69.5
path	28.8,69.7
.' Follow the path, clearing trash as you make your way to the next boss, Chimaeron. |goto Blackwing Descent/2 28.8,69.7 <5 |noway |c
step
goto Blackwing Descent/2 24.5,70.2
'_Chimaeron_
'_Phase 1_
.' _Caustic Slime_ deals 235,000 total Nature damage to all players within 6 yards of the impact and reduces their chance to hit.
.' _Fued_ Chimaeron is unable to perform melee attacks while his heads are fighting each other.
.' _Double Attack_ Chimaeron strikes twice with his next attack.
.' _Break_ Chimaeron attacks viciously, increasing the target's Physical damage taken by 25% and reducing healing by 15% for 1 minute.
'_Phase 2_
.' Phase 2 starts when Chimaeron reaches 20% health.
.' _Mortality_ Chimaeron becomes immune to taunt and increases his damage by 20%. Also reduces healing for all players by 99%.
.' _Double Attack_ Chimaeron strikes twice with his next attack.
|modelnpc Chimaeron##43296
|confirm
step
map Blackwing Descent/2
path loop off
path	28.8,69.7	36.7,69.5
.' Follow the path, clearing trash as you make your way to the final boss, Nefarian. |goto Blackwing Descent/2 36.7,69.5 <5 |noway |c
step
goto Blackwing Descent/2 48,70.2
'_Nefarian_
.'
'_Phase 1_
'_Onyxia_
.' _Electric Discharge_ inflicts 20,000 Nature damage every secondto players on Onyxia's sides.
.' _Shadowflame Breath_ inflicts 35,000 damage every 0.5 second for 1.5 seconds to players in a _cone_ in front of Onyxia.
.' _Cleave_ deals 110% melee damage to the targeted player and all nearby players. Make sure the tank is the only one in front of Onyxia when this occurs.
.' _Tail Lash_ deals 20.000 damage and a 2 second stun to all players behind Onyxia.
.' _Children of Deathwing_ Nefarian and Onyxia attack 100% faster when they are within 60 yards of each other.
'_Nefarian_
.' _Hail of Bones_ deals 25,000 Shadow damage to nearby players and spawns an Animated Bone Warrior.
.' _Cleave_ deals 110% melee damage to the targeted player and all nearby players. Make sure the tank is the only one in front of Nefarian when this occurs.
.' _Shadowflame Breath_ inflicts 55,000 damage every 0.5 second for 1.5 seconds to players in a _cone_ in front of Onyxia.
.' _Tail Lash_ deals 20.000 damage and a 2 second stun to all players behind Onyxia.
.' _Children of Deathwing_ Nefarian and Onyxia attack 100% faster when they are within 60 yards of each other.
|modelnpc Onyxia##10184
|modelnpc Nefarian##41376
.' Click here to proceed to Phase 2. |confirm
step
goto Blackwing Descent/2 48,70.2
'_Phase 2_
'_Nefarian_
.' _Shadowflame Barrage_ is cast repeatedly for all of Phase 2. It deals 25,000 damage to the target.
.' _Shadow of Cowardice_ inflicts 30,000 damage to the targeted player and increases their Shadow damage taken by 100%.
|modelnpc Nefarian##41376
.' Click here to move to Phase 3. |confirm
step
goto Blackwing Descent/2 48,70.2
'_Phase 3_
'_Nefarian_
.' _Cleave_ deals 110% melee damage to the targeted player and all nearby players. Make sure the tank is the only one in front of Nefarian when this occurs.
.' _Shadowflame Breath_ inflicts 55,000 damage every 0.5 second for 1.5 seconds to players in a _cone_ in front of Onyxia.
.' _Tail Lash_ deals 20.000 damage and a 2 second stun to all players behind Onyxia.
.' _Shadowblaze Spark_ creates a blaze at the targeted location. It spreads from the initial location, following players and speeding up if it is touched.
.' _Shadowblaze_ deals 50,000 damage every second to players standing within the Shadowblaze.
|modelnpc Nefarian##41376
|confirm
step
.' Congratulations, you have finished the Blackwing Descent raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Cataclysm\\The Bastion of Twilight",[[
dungeon 758
description This guide will walk you through the Bastion of Twilight Raid.
author support@zygorguides.com
step
map The Bastion of Twilight/1
path loop off
path	39.6,42.7	39.6,19.8	46.6,19.4
.' Follow the path, clearing trash as you make your way to the first boss, Halfus Wyrmbreaker. |goto The Bastion of Twilight/1 46.6,19.4 <5 |noway |c
step
goto The Bastion of Twilight/1 53.4,19.3
'_Halfus Wyrmbreaker_
.' _Furious Roar_ occurs after Halfus reaches 50% health. It deals 10,000 Physical damage and knocks the raid down.
.' _Dragon's Vengeance_ Halfus takes an additional 100% damage from all sources, stacking up to 3 times.
.' _Frenzied Assault_ increases the caster's attack speed by 100%.
.' _Malevolent Strikes_ reduces the effectiveness of any healing on the target by 6%, stacking up to 15 times.
.' _Shadow Nova_ deals 30,000 Shadow damage and knocking back all players.
.' _Berserk_ occurs after 6 minutes of combat. It increases his damage by 500% and attack speed by 150%. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
'_Proto-Behemoth_
.' _Fireball_ deals Fire damage to players in a small area.
.' _Fireball Barrage_ deals 34,000-46,000 Fire damage to players in a small area.
.' _Scorching Breath_ deals 9000 Fire damage per second to players in front of the Proto-Behemoth.
|modelnpc Halfus Wyrmbreaker##44600
|confirm
step
map The Bastion of Twilight/1
path loop off
path	53.7,29.1	53.7,48.9	53.7,68.3
.' Follow the path, clearing trash as you make your way to the next boss, Valiona and Theralion. |goto The Bastion of Twilight/1 53.7,68.3 <5 |noway |c
step
goto The Bastion of Twilight/1 53.9,78.4
'_Valiona & Theralion_
.'
'_Valiona_
'_Grounded_
.' _Blackout_ deals 35,000 total Shadow damage to players within 8 yards, splitting damage to all players affected by it.
.' _Devouring Flames_ deals 40,000 Shadow damage to players in front of Valiona. Deals less damage to players farther away from Valiona.
'_Flying_
.' _Deep Breath_ Valiona begins to cast Twilight Flames that deal 15,000 Shadow damage every second and send the target to the Twilight Realm.
.' _Twilight Meteorite_ is a debuff placed on a random player. After 6 seconds it deals 120,000 total Shadow damage to players within 8 yards, spliting damage between all nearby players.
'_Theralion_
'_Grounded_
.' _Engulfing Magic_ increases damage and healing done by 100% and causes you to inflict Shadow damage equal to the damage or healing you do to allies within 10 yards.
.' _Fabulous Flames_ deal 15,000 Shadow damage every second.
'_Flying_
.' _Dazzling Destruction_ causes swirls on the ground to appear. After 5 seconds 50,000 Shadow damage to every player within the blast zone.
.' _Twilight Blast_ deals 30,000 Shadow damage to players within 8 yards of the target.
|modelnpc Valiona##45992
|modelnpc Theralion##45993
|confirm
step
map The Bastion of Twilight/1
path loop off
path	53.6,86.7	47.1,92.8
map The Bastion of Twilight/2
path	42.4,18.2
.' Follow the path, clearing trash as you make your way to the next boss, the Twilight Ascendant Council. |goto  The Bastion of Twilight/2 42.4,18.2 <5 |noway |c
step
goto The Bastion of Twilight/2 42.5,45.5
'_Twilight Ascendant Council_
'_Phase 1_
'_Feludius_
.' _Heart of Ice_ deals increasing Frost damage every 2 seconds, starting at 2000 Frost damage and increasing to 4000 and then 8000 Frost damage.
.' _Hydro Lance_ deals 45,000-55,000 Frost damage to a random player.
.' _Water Bomb_ deals 10,000 Frost damage to players within 6 yards, applying the _Waterlogged_ debuff.
.' _Waterlogged_ reduces the target's movement speed by 25%. Fire can remove this effect.
.' _Glaciate_ deals massive Frost damage to any nearby players, and reducing damage the farther away from Feludius each player is.
.' _Frozen Blood_ freezes a Waterlogged enemy solid, stunning them and dealing 20,000 Frost damage every 2 seconds for 10 seconds.
'_Ignacious_
.' _Burning Blood_ deals increasing Fire damage every 2 seconds, starting at 2000 Fire damage and increasing to 4000 and then 8000 Fire damage.
.' _Aegis of Flame_ surrounds the caster with a shield of flame, absorbing 1,000,000 damage and preventing spell interruption.
.' _Flame Torrent_ inflicts 40,000 Fire damage in a 18 yard _cone_ every second for 3 seconds.
.' _Inferno Leap_ deals 23,000-27,000 Fire damage and knocking away all nearby players.
.' _Inferno Rush_ Ignacious runs back to his primary aggro target, leaving a trail of flames behind him that deal 5000 Fire damage every 0.5 seconds and removes _Waterlogged_.
.' _Rising Flames_ deals increasing Fire damage to the entire raid until interrupted.
|modelnpc Feludius##43687
|modelnpc Ignacious##43686
.' Click here to move to Phase 2. |confirm
step
goto The Bastion of Twilight/2 42.5,45.5
'_Twilight Ascendant Council_
'_Phase 2_
'_Arion_
.' _Call Winds_ creates a cyclone that deals 7000 Nature damage and levitates the player into the air.
.' _Lightning Rod_ marks a player that attracts electrical attacks. Arion will cast _Chain Lightning_ on this player.
.' _Chain Lightning_ affects players that have been marked to attract electricity. It deals 10,000 Nature damage and bounces to additional nearby players.
.' _Disperse_ the caster dissolves into the wind and materializes somewhere else in the room.
.' _Lightning Blast_ deals 72,000-88,000 Nature damage to the tank after Arion casts _Disperse_.
.' _Thundershock_ deals 146,000-153,000 Nature damage to all nearby players.
'_Terrastra_
.' _Gravity Well_ creates a well that connects nearby players to the ground, reducing the effectiveness of some powerful electric attacks.
.' _Gravity Core_ occurs after entering a Gravity Well, dealing 10,000 Physical damage every 2 seconds for 10 seconds.
.' _Harden Skin_ increases Terrastra's Physical damage dealt by 100% and absorbing 50% of all damage taken, up to a maximum of 650,000.
.' _Eruption_ shoots a spike through the floor, impaling players within 4 yards and dealing 47,000-52,000 damage and knocking them up into the air.
.' _Quake_ sends a shockwave through the ground, dealing 150,000 damage to all players who are in contact with the ground. Deals more to targets grounded by a Gravity Well.
|modelnpc Arion##43688
|modelnpc Terrastra##43689
.' Click here to move to Phase 3. |confirm
step
goto The Bastion of Twilight/2 42.5,45.5
'_Twilight Ascendant Council_
'_Phase 3_
'_Elementium Monstrosity_
.' _Lava Seed_ creates kernels of Flame energy in the surrounding area which erupt and deal 35,000-45,000 Fire damage to nearby players.
.' _Liquid Ice_ the Monstrosity freezes the ground beneath it. The pools of ice will grow as the Monstrosity stands in it, also increasing the damage dealt.
.' _Electric Instability_ deals constant Nature damage to random players, increasing intensity the longer the Monstrosity is alive and jumping to nearby players.
.' _Gravity Crush_ traps a player in a gravity bubble, lifting them from the ground and dealing 10% of their maximum health every 0.5 seconds for 6 seconds. After 6 seconds the player is dropped to the ground.
|modelnpc Elementium Monstrosity##43735
|confirm
step
map The Bastion of Twilight/2
path loop off
path	43.9,75.3	56.6,75.1
.' Follow the path, clearing trash as you make your way to the next boss, Cho'gall. |goto The Bastion of Twilight/2 56.6,75.1 <5 |noway |c
step
goto The Bastion of Twilight/2 73.4,75.4
'_Cho'gall_
'_Phase 1_
.' _Conversion_ causes the target to channel Worshipping on Cho'gall, causing him to gain _Twisted Devotion_ every 3 seconds.
.' _Twisted Devotion_ increases Cho'gall's damage by 10% for 20 seconds.
.' _Flame's Orders_ adds 20,000 Fire damage to each melee attack and spawns Fire patches.
.' _Shadow's Orders_ adds a Shadow AoE effect to Cho'gall's attacks. These hit everyone in the room.
.' _Fury of Cho'gall_ deals 30,000 Shadow damage and 30,000 Physical damage and increasing their Shadow and Physical damage taken by 20%.
.' _Summon Corrupting Adherent_ summons Corrupted Adherents to attack the raid.
.' _Phase 2_ starts when Cho'gall reaches 25% health.
|modelnpc Cho'gall##43324
.' Click here to move to Phase 2. |confirm
step
goto The Bastion of Twilight/2 73.4,75.4
'_Cho'gall_
'_Phase 2_
.' _Fury of Cho'gall_ deals 30,000 Shadow damage and 30,000 Physical damage and increasing their Shadow and Physical damage taken by 20%.
.' _Corruption of the Old God_ deals 5000 Shadow damage every 2 seconds to the entire raid.
.' _Darkened Creations_ Cho'gall summons Darkened Creations to attack the raid. They use _Debilitating Beam_.
.' _Debilitating Beam_ reducing healing and damage by 75% and deals 5000 Shadow damage every second for 10 seconds.
|modelnpc Cho'gall##43324
|confirm
step
.' The final boss in this encounter, Sinestra, can only be done in Heroic mode.
.' Click here to fight Sinestra! |next "sinestra" |confirm
.' Click here to skip Sinestra. |next "raidend" |confirm
step
label "sinestra"
goto The Bastion of Twilight/3 49,34.2
'_Sinestra_
.' _Twilight Blast_ deals 166,000-184,000 Shadow damage to the targeted player.
.' _Twilight Essence_ deals 10,000 Shadow damage to players within the bubbling essence.
.' _Unleash Essence_ releases twilight energy that hits all nearby players, dealing damage equal to 10% of their maximum health every second.
.' _Twilight Slicer_ fires a beam of twilight energy, dealing 50,000 Shadow damage every 0.3 seconds to any players that touch it.
.' _Wrack_ deals increasing Shadow damage over 1 minute.
.' _Twilight Pulse_ deals 30,000 Shadow damage every 0.5 seconds to players within 10 yards.
.' _Twilight Spit_ deals 4000 Shadow damage and increasing Shadow damage taken by 10%.
.' _Indomitable_ regains control through sheer will, becoming immune to all bonds and restraints until stopped. Deals 40,000 Shadow damage to players within 10 yards and knocks them back.
|modelnpc Sinestra##45213
|confirm
step
label "raidend"
.' Congratulations, you have finished the Bastion of Twilight raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Cataclysm\\Firelands",[[
dungeon 800
description This guide will walk you through the Firelands Raid.
author support@zygorguides.com
step
map Firelands/1
path loop off
path	33.2,77.0	41.7,70.4	32.9,52.8
path	33.1,29.3
.' Follow the path, clearing trash as you make your way to the first boss, _Beth'tilac_. |goto Firelands/1 33.1,29.3 <5 |noway |c
step
goto Firelands/1 20.8,32
'_Beth'tilac_
'_Phase 1_
.' _Ember Flare_ deals 20,000 Fire damage to players near Beth'tilac.
.' _Meteor Burn_ deals 40,000 Fire damage to players within 7 yards of the impact. Also burns a hole in the web that players can fall through.
.' _Consume_ Beth'tilac consumes Cinderweb Spiderlings, healing herself for 10% of her maximum health.
.' _Smoldering Devastation_ occurs when Bath'tilac's energy fully depletes. She sets herself ablaze, dealing 400,000 Fire damage on all players located on the same level.
'_Phase 2_
.' _Frenzy_ increases her damage by 5%, stacking up to 50 times.
.' _The Widow's Kiss_ reduces the target's healing by 10%, stacking every 2 seconds for 20 seconds, also causes the affected player to deal damage to all nearby players within 10 yards.
.' _Ember Flare_ deals 20,000 Fire damage to players on the same level as Beth'tilac.
.' _Consume_ Beth'tilac consumes Cinderweb Spiderlings, healing herself for 10% of her maximum health.
|modelnpc Beth'tilac##52498
|confirm
step
map Firelands/1
path loop off
path	33.1,29.3	32.9,52.8	41.7,70.4
path	56.7,62.5
.' Follow the path, clearing trash as you make your way to the next boss, _Shannox_. |goto Firelands/1 56.7,62.5 <5 |noway |c
step
goto Firelands/1 49.2,46
'_Shannox_
.' _Immolation Trap_ Fire trap similar to a Hunter's trap. Deals Fire damage to the player that walks over it and all nearby players.
.' _Crystal Prison Trap_ trap that stuns the player that walks over it.
.' _Hurl Spear_ deals large AoE damage to players within 3 yards of the impact point and moderate damage to players within 50 yards. Also creates fire eruptions on the floor around the impact point.
.' _Arcing Slash_ deals Physical damage in a _frontal cleave_. Make sure the tank is the only player in front of Shannox when this occurs.
.' _Jagged Tear_ DoT that deals 2000 damage every 3 seconds for 24 seconds.
|modelnpc Shannox##53691
|confirm
step
map Firelands/1
path loop off
path	64.7,56.6	71.6,71.0
.' Follow the path, clearing trash as you make your way to the next boss, _Lord Rhyolith_. |goto Firelands/1 71.6,71.0 <5 |noway |c
step
goto Firelands/1 65.6,75.6
'_Lord Rhyolith_
.' _Concussive Stomp_ deals 32,000-38,000 Fire damage to players near Ryolith.
.' _Burning Feet_ increases movement speed by 100%.
.' _Immolation_ deals Fire damage every second to players near Ryolith.
.' _Molten Spew_ Ryolith spits lava at nearby players.
.' _Obsidian Armor_ reduces damage taken by 1% per stack.
.' _Superheated_ increases damage dealt by 10% every 10 seconds.
|modelnpc Lord Rhyolith##52558
|confirm
step
map Firelands/1
path loop off
path	71.6,71.0	64.7,56.6	57.9,51.2
.' Follow the path, clearing trash as you make your way to the next boss, _Alysrazor_. |goto Firelands/1 57.9,51.2 <5 |noway |c
step
goto Firelands/1 64,39
'_Alysrazor_
'_Phase 1_
.' _Firestorm_ deals 31,500 Fire damage to all players and knocking them back, also deals an additional 10,500 Fire damage every second for 10 seconds.
.' _Blazing Claw_ deals 105,000 Physical damage to players in a 25-yard _cone_ every 1.5 seconds. Each swipe also increases the target's Fire and Physical damage taken by 10% for 15 seconds.
.' _Molting_ Alysrazor creates _Molten Feathers_ nearby.
.' _Molten Feather_ these feathers can be picked up by a player, up to a maximum of 3. While holding a feather all spells can be cast while moving and increases movement speed by 30% per feather. If 3 feathers are obtained, the player will gain _Wings of Flame_.
.' _Wings of Flame_ allows the player to fly for 20 seconds.
|modelnpc Alysrazor##52530
.' Click here to move to Phase 2. |confirm
step
goto Firelands/1 64,39
'_Alysrazor_
'_Phase 2_
.' _Fiery Vortex_ a vortex appears in the middle of the room, dealing 105,000 Fire damage every 0.5 seconds.
.' _Fiery Tornado_ tornado erupts from the Fiery Vortex, dealing 26,000 Fire damage every second.
.' _Blazing Power_ rings of fire appear on the ground and last 3 seconds. Players who pass through these gain increased haste by 4% and stacks up to 25 times.
|modelnpc Alysrazor##52530
.' Click here to move to Phase 3. |confirm
step
goto Firelands/1 64,39
'_Alysrazor_
'_Phase 3_
.' _Burnout_ Alystrazor's fire burns out, causing her to become immobile and increasing damage taken by 100% and creating a n _Essence of the Green_ when struck with a spell.
.' _Essence of the Green_ emits 10% of maximum mana to players.
'_Phase 4_
.' _Blazing Buffet_ deals 10,500 Fire damage to all players every second as long as Alysrazor remains Ignited.
.' _Blazing Claw_ deals 105,000 Physical damage to players in a 25-yard _cone_ every 1.5 seconds. Each swipe also increases the target's Fire and Physical damage taken by 10% for 15 seconds.
.' _Full Power_ occurs when Alysrazor reaches 100 Molten Power. This deals 52,500 Fire damage to all players and knocks them back.
|modelnpc Alysrazor##52530
|confirm
step
goto Firelands/1 49.2,32.8
'_Baleroc_
.' _Blaze of Glory_ increases the target's maximum health by 20% but also their Physical damage taken by 20%.
.' _Countdown_ forms a link between 2 players for 8 seconds. After 8 seconds both will explode and deal 125,000 Fire damage to all nearby players unless the linked players move near each other.
.' _Incendiary Soul_ increases the target's Fire damage taken by 20%.
.' _Decimation Strike_ deals Shadow damage equal to 90% of the target's maximum health, but always at least 250,000 damage, and reduces the target's healing done by 90% for 4 seconds.
.' _Decimation Blade_ deals 90% of the target's maximum health.
|modelnpc Baleroc##53494
|confirm
step
map Firelands/1
path loop off
path	49.3,17.2
map Firelands/2
path	51.0,90.0
.' Follow the path, clearing trash as you make your way to the next boss, _Majordomo Staghelm_. |goto Firelands/2 51.0,90.0 <5 |noway |c
step
goto Firelands/2 50.9,72.4
'_Majordomo Staghelm_
.' _Fury_ increases Staghelm's damage by 8% per stack. He gains a stack of this every time he switches between scorpion and cat forms.
.' _Searing Seeds_ plants fiery seeds into all raid members. Each seed grows at a different rate and explodes when fully grown, dealing 45,000 Fire damage to players within 12 yards.
.' _Fiery Cyclone_ tosses all raid members into the air, preventing any action but making them invulnerable.
'_Scorpion Form_
.' _Flame Scythe_ inflicts Fire damage in a _cone_ in front of Staghelm, split between all players hit.
'_Cat Form_
.' _Leaping Flames_ leaps at a player, leaving behind a Spirit of the Flame and landing in a blaze, igniting the ground and causing it to deal 19,000 Fire damage every 0.5 seconds for 1 minute.
|modelnpc Majordomo Staghelm##52571
|confirm
step
map Firelands/2
path loop off
path	51.0,90.0	50.6,30.2
.' Follow the path, clearing trash as you make your way to the next boss, _Ragnaros_. |goto Firelands/2 50.6,30.2 <5 |noway |c
step
goto Firelands/2 50.7,15.3
'_Ragnaros_
.' _Sulfuras Smash_ Ragnaros strikes the ground, sending out waves of lava from the impact point.
.' _Magma Trap_ creates a trap that deals 60,000 Fire damage to all players and knocking back the player that triggered it.
.' _Hand of Ragnaros_ deals 23,000 Fire damage to all players within 55 yards and knocking them back.
.' _Wrath of Ragnaros_ deals 45,000 Fire damage to players within 6 yards of the target, also knocking them back.
.' _Molten Seed_ after 10 seconds they spawn adds that can't be tanked.
.' _Engulfing Flames_ Ragnaros summons flames around the platform that can obscure the platform.
.' _Living Meteors_ rains meteors down on targeted players. Damaging the meteors will cause it to roll away and change its path. Anyone struck by one of these will be killed.
|modelnpc Ragnaros##52409
|confirm
step
.' Congratulations, you have finished the Firelands raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Cataclysm\\Throne of the Four Winds",[[
dungeon 773
description This guide will walk you through the Throne of the Four Winds Raid.
author support@zygorguides.com
step
goto Throne of the Four Winds/1 47.5,50
'_Al'Akir_
'_Phase 1_ - 100%-80% health.
.' _Ice Storm_ creates a blizzard that moves around the platform and deals 15,000 Frost damage every 0.5 seconds.
.' _Static Shock_ shocks all nearby players and deals 1000 Nature damage every second for 5 seconds.
.' _Electrocute_ channels a bolt at the target that deals increasing Nature damage. Ends if the target moves into melee range of Al'Akir.
.' _Wind Burst_ deals 28,000-31,000 Nature damage to all players and knocking them back.
.' _Lightning Strike_ deals 20,000 Nature damage in a _cone_, facing a random player every second for 30 seconds. All players affected deal an additional 20,000 Nature damage to nearby players.
.' _Squall Line_ creates a wall of tornadoes that circle around Al'Akir platform with an opening that players must move through.
|modelnpc Al'Akir##46753
.' _Phase 2_ starts when Al'Akir reaches 80% health.
.' Click here to move to Phase 2. |confirm
step
goto 47.5,50
'_Al'Akir_
'_Phase 2_ - 80%-25% health.
.' _Static Shock_ shocks all nearby players, dealing 1000 Nature damage every second, interrupting spells cast and lasting 5 seconds.
.' _Electrocute_ channels a bolt at the target that deals increasing Nature damage. Ends if the target moves into melee range of Al'Akir.
.' _Squall Line_ creates a wall of tornadoes that circle around Al'Akir platform with an opening that players must move through.
.' _Acid Rain_ deals 500 Nature per second to all players and increasing by 500 damage every 15 seconds until Al'Akir reaches Phase 3.
.' _Stormling_ summons a Stormling add to attack the raid. When killed it applies _Feedback_ to Al'Akir.
.' _Feedback_ increases Al'Akir's damage dealt by 10% per stacks for 20 seconds.
|modelnpc Al'Akir##46753
.' _Phase 3_ starts when Al'Akir reaches 25% health.
.' Click here to move to Phase 3. |confirm
step
goto 47.5,50
'_Al'Akir_
'_Phase 3_ - 25% health.
.' _Eye of the Storm_ increases the caster's movement speed by 300%.
.' _Chain Lightning_ deals 15,000 Nature damage to a random player and then jumps to an additional nearby player, increasing damage dealt by 30% each jump.
.' _Lightning Rod_ causes a random player to emit lightning to nearby players after 5 seconds, dealing 5000 Nature damage every second for 5 seconds.
.' _Lightning Cloud_ summons clouds at the same altitude of the targeted player. After 5 seconds, these clouds erupt in lightning and deal 50,000 Nature damage per second to all players at the same altitude. Lasts 30 seconds.
.' _Wind Burst_ deals 38,000-42,000 Nature damage to all players and knocking them back.
|modelnpc Al'Akir##46753
|confirm
step
goto 47.5,26
'_Conclave of Wind_
'_Anshal_
.' _Soothing Winds_ summons a green circle on a random player. This heals all players inside it for 20,000 health every second, but also silencing and pacifying them.
.' _Nurture_ summons a Ravenous Creeper every second for 5 seconds. These use _Toxic Spores_.
.' _Toxic Spores_ spores erupt from the caster every second for 5 seconds, afflicting all players within 8 yards with a toxin that deals 500 Nature damage every second for 5 seconds and stacking up to 25 times.
.' _Zephyr_ occurs at full energy, healing all of his allies for 25,000 health every second and also increasing damage by 15% for 15 seconds.
'_Nezir_
.' _Wind Chill_ deals 5000 Frost damage to all players within 100 yards and increasing all Frost damage taken by 10% for 30 seconds.
.' _Permafrost_ deals 10,000 Frost damage every second in a _cone_ for 3 seconds.
.' _Ice Patch_ deals 7800 Frost damage every second to all players within 20 yards and slowing their movement speed by 10%.
.' _Sleet Storm_ deals 30,000 Frost damage every second to all players within 100 yards, split between all targets.
'_Rohash_
.' _Slicing Gale_ deals 11,000-13,000 Nature damage, increasing Nature damage they take by 5% for 30 seconds.
.' _Wind Blast_ spins around the platform, blasting the area in front of the caster every second, dealing 10,000 Nature damage and a 200 yard knockback.
.' _Tornado_ summons 3 nearby tornadoes that move around the area, dealing 42,000 Nature damage to any player that gets within 10 yards.
.' _Hurricane_ occurs at full energy. Creates a large vortex that catches all nearby players and launches them high into the air and dealing 2500 Nature damage every second for 15 seconds.
|modelnpc Anshal##45870
|modelnpc Nezir##45871
|modelnpc Rohash##45872
|confirm
step
.' Congratulations, you have finished the Throne of the Four Winds raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Cataclysm\\Dragon Soul ",[[
dungeon 824
description This guide will walk you through the Dragon Soul Raid.
author support@zygorguides.com
step
goto Dragon Soul/1 50.6,69.6
'_Morchok_
'_Phase 1_
.' _Stomp_ deals 750,000 total Physical damage split between all affected players. Morchok's target and their closest ally take a double portion of the damage.
.' _Crush Armor_ deals 120% melee damage and reducing their armor by 10% for 20 seconds and stacking up to 10 times.
.' _Resonating Crystal_ creates a crystal that explodes after 12 seconds for massive Shadow damage split between 3 random players, the damage is increased depending on the distance of each player from the crystal.
.' _Furious_ occurs at 20% health. Morchok's attack speed by 30% and damage by 20%.
'_Phase 2_
.' _Earthen Vortex_ Morchok draws all players towards him, dealing 5% of their maximum health as Physical damage every second for 5 seconds.
.' _Falling Fragments_ shards of Earth's Vengeance erupt from the ground, inflicting 15,000 Physical damage to players within 2 yards.
.' _Black Blood of the Earth_ Morchok causes the black blood to erupt from the ground, inflicting 5000 Nature damage and increasing Nature damage taken by 100% every second while a player is standing in the blood.
|modelnpc Morchok##55265
|confirm
step
goto Dragon Soul/2 50.9,53
'_Warlord Zon'ozz_
.' _Black Blood of Go'rath_ deals 9000 damage every second.
.' _Phychic Drain_ channels phychic force in a _cone_ in front of Zon'ozz, dealing 83,000-96,750 Shadow damage.
.' _Disrupting Shadows_ deals 25,000-30,000 Shadow damage every 2 seconds.
.' _Focused Anger_ increases the target's Physical damage taken and attack speed by 10%.
|modelnpc Warlord Zon'ozz##55308
|confirm
step
goto Dragon Soul/3 46.8,49.9
'_Yor'sahj the Unsleeping_
.' _Searing Blood_ hits random players, dealing increased damage the farther away from Yor'sahj the targets are.
.' _Deep Corruption_ receiving healing or absorption effects will trigger an explosion at 5 stacks.
.' _Void Bolt_ hurls a shadow bolt at a player, dealing Shadow damage.
|modelnpc Yor'sahj the Unsleeping##55312
|confirm
step
goto Dragon Soul/4 52.7,52
'_Hagara the Stormbinder_
.' _Focused Assault_ slices rapidly at the target, dealing 50% weapon damage.
.' _Feedback_ increases damage taken by 100% for 15 seconds.
.' _Ice Tomb_ conjures spheres of ice to entomb players in blocks of ice.
.' _Frostflake_ applies a stacking snare that reduces movement speed by 10%.
.' _Lightning Storm_ deals 11,000 Nature damage to all nearby players.
.' _Shattered Ice_ deals 54,000-63,000 Frost damage to a player and reduces their movement speed for 4 seconds.
|modelnpc Hagara the Stormbinder##55689
|confirm
step
goto Dragon Soul/1 49.9,45.7
'_Ultraxion_
.' _Unstable Monstrosity_ Twilight energry erupts from Ultraxion's hide, dealing 300,000 total Shadow damage every 6 secondsm split between all players in the Twilight Realm and within 30 yards of the target. Every minute Ultraxion is engaged in combat it reduces the time between arcs by 1 second.
.' _Heroic Will_ pulls the player from the Twilight realm for up to 5 seconds, but they are unable to attack, cast, or move while concentrating.
.' _Fading Light_ sucks the light from the targeted player and a random player, drawing them into the Twilight over 5-10 seconds. Upon expiration, players in the Twilight realm die.
.' _Hour of Twilight_ players caught in the Twilight realm when this occurs take 300,000 Shadow damage. If this doesn't hit a player it generally results in a wipe.
.' _Twilight Burst_ occurs if all raid members stand outside the range of Ultraxion's claws. He deals 75,000 Shadow damage on all plaers and increases their magical damage taken by 50% for 6 seconds.
|modelnpc Ultraxion##55294
|confirm
step
goto Dragon Soul/5 48.3,16.8
'_Warmaster Blackhorn_
.' _Shockwave_ deals 100,000 Physical damage to players in a 80-yard _cone_ in front of the caster, stunning them for 4 seconds.
.' _Siphon Vitality_ deals damage equal to 20% of the target's remaining health, also healing the caster.
.' _Devastate_ deals 120% weapon damage and applying _Sunder Armor_.
.' _Sunder Armor_ reduces the target's armor by 20% per stack, applying up to 5 times and lasting 30 seconds.
.' _Disrupting Roar_ deals 50,000 Physical damage to all players and interrupting their spellcasting within 10 yards for 8 seconds.
|modelnpc Warmaster Blackhorn##56427
|confirm
step
goto Dragon Soul/6 33.2,85.2
'_Spine of Deathwing_
.' If all players are on one side of Deathwing, he will do a barrel roll, attempting to throw off any players not attached to his back.
'_Corruption_ adds
.' _Searing Plasma_ debuff that absorbs 200,000 healing from the target and dealing 7500 Physical damage periodically.
.' When killed, these spawn a Hideous Amalgamation add and _Corrupted Blood_.
.' _Corrupted Blood_ forms from the hole left by the Corruption. It deals a small amount of damage and prevents players from being thrown off Deathwing's back. When these are killed it creates an untargetable residue, which crawls back to the hole.
'_Hideous Amalgamation_
.' _These should be tanked and dragged through the residue created after killing Corrupted Blood, giving it a stack of _Absorbed Blood_. At 10 stacks, the Amalgamation explodes, removing a nearby armor plate from Deathwing, and likely killing any nearby players.
.' _Seal Armor Breach_ occurs after a Hideous Amalgamation explodes and removes one of Deathwing's armor plates. If this cast is completed, the armor plate is re-sealed.
.' This fight ends after 3 armor plates are successfully removed, sending Deathwing down into the Maelstrom.
|modelnpc Deathwing##53879
|confirm
step
goto Dragon Soul/7 47.8,53.8
'_Madness of Deathwing - Aspects_
'_Aspects_
'_Alexstrasza_
.' _Alexstrasza's Presence_ increases the player's maximum health by 20%.
.' _Cauterize_ inflicts lethal damage to Blistering Tentacles over 10 seconds and reduces the damage inflicted by Corrupted Blood.
'_Nozdormu_
.' _Nozdormu's Presence_ increases the player's haste by 20%.
.' _Time Zone_ creates a Time Zone at the targeted location, causing Deathwing's Elementium Meteor to greatly decrease its travel speed and also reduces any enemies' attack speed by 50%.
'_Ysera_
.' _Ysera's Presence_ increases player healing by 20%.
.' _Enter the Dream_ allows players to enter the Dream, decreasing all damage taken by 50% for 2.5 seconds.
'_Kalecgos_
.' _Kalecgos' Presence_ increases player damage dealt by 20%.
.' _Spellweaving_ allows attacks and abilities to cause Spellweaving, which inflicts 2000 Arcane damage to enemies within 6 yards, but not including the current target.
'_Thrall_
.' _Carrying Winds_ takes players between adjacent platforms and increases their movement speed by 60% for 10 seconds. This effect is stackable.
|confirm
step
goto Dragon Soul/7 47.8,53.8
'_Madness of Deathwing_
'_Phase 1_
.' _Assault Aspect_ Deathwing attacks the platform with the largest number of players and an attached limb.
.' _Cataclysm_ Deathwing attempts to bring forth a 2nd cataclysm, dealing 1,500,000 Fire damage to all players.
.' _Elementium Bolt_ is cast on a targeted platform. If reached, it deals 390,000 Fire damage every 5.2 seconds
.' _Corrupting Parasite_ inflicts increasing Shadow damage periodically. Upon expiration, it detaches itself from the player and deals 250,000 Fire damage to players within 10 yards.
.' _Unstable Corruption_ deals 10% of the Corrupted Parasite's health as Fire damage to all players.
.' _Hemorrhage_ creates Regenerative Blood at a nearby location that regenerate health and attack the raid.
'_Limb Tentacle_
.' _Burning Blood_ gushes from the Tentacle, inflicting Fire damage every 2 seconds, the damage increasing as the Tentacle's health lowers.
.' _Agonizing Pain_ the pain from severing Deathwing's limb stuns him and inflicts 20% of his health as damage.
'_Mutated Corruption_
.' _Crush_ deals 100,000 Physical damage to players in a _cone_ in front of the Corruption.
.' _Impale_ deals 400,000 Physical damage to the Corruption's target.
.' _Phase 2_ starts when Deathwing reaches 20% health.
|modelnpc Deathwing##56173
.' Click here to advance to Phase 2. |confirm
step
goto Dragon Soul/7 47.8,53.8
'_Madness of Deathwing_
'_Phase 2_
.' _Congealing Blood_ Deathwing begins to hemorrhage, creating Congealing Blood at a nearby location. These move toward him, healing him for 1% of his health if they reach him.
.' _Elementium Fragment_ Deathwing's armor chips off, forming Elementium Fragments that deal 200,000 Physical damage to random players.
.' _Elementium Terror_ Deathwing's armor chips off, forming Elementium Terrors that deal 60,000 Physical damage and an additional 20,000 Physical damage every second.
.' _Corrupted Blood_ deals Fire damage every 2 seconds, the damage increasing as Deathwing's health lowers.
|modelnpc Deathwing##56173
|confirm
step
.' Congratulations, you have finished the Dragon Soul raid guide!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Guardians of Mogu'shan (LFR)",[[
description This guide will walk you through the Guardians of Mogu'shan Raid.
author support@zygorguides.com
step
map Mogu'shan Vaults/1
path loop off
path	68.4,64.2	54.8,64.2	50.4,64.3
.' Follow the path, clearing trash as you make your way to the Stone Guard. |goto 50.4,64.3 <5 |noway |c
step
goto 45.6,64.5
'_Stone Guard_
'_Amethyst Guardian_
.' _Amethyst Petrification_ attempts to turn its enemies to amethyst, reducing Shadow damage they take by 90% and progressively slowing their movement speed.
.' _Amethyst Pool_ deals 60,000 every second to players who enter the area.
'_Cobalt Guardian_
.' _Cobalt Petrification_ attempts to turn its enemies to cobalt, reducing Arcane damage they take by 90% and slowing their movement speed.
.' _Rend Flesh_ deals 50,000 Physical damage every second for 15 seconds.
'_Jade Guardian_
.' _Jade Petrification_ attempts to turn its enemies to jade, reducing Nature damage they take by 90% and slowing their movement speed.
.' _Jade Shards_ inflicts 50,000 Nature damage to all players.
.' _Rend Flesh_ deals 50,000 Physical damage every second for 15 seconds.
'_Jasper Guardian_
.' _Jesper Petrification_ attempts to turn its enemies to jesper, reducing Fire damage they take by 90% and slowing their movement speed.
.' _Jesper Chains_ links 2 players together. While they are over 10 yards apart they both take 35,000 Fire damage every second, increasing by 15% per second.
.' _Rend Flesh_ deals 50,000 Physical damage every second for 15 seconds.
|modelnpc Amethyst Guardian##60047
|modelnpc Cobalt Guardian##60051
|modelnpc Jade Guardian##60043
|modelnpc Jasper Guardian##59915
|confirm
step
map Mogu'shan Vaults/1
path loop off
path	38.0,64.4	32.8,64.3	32.1,52.1
path	32.1,42.4
.' Follow the path, clearing trash as you make your way to the first boss, Feng the Accursed. |goto 32.1,42.4 <5 |noway |c
step
goto 32.1,33.2
'_Feng the Accursed_
.' _Arcane Resonance_ charges random players with Arcane Resonance, which deals 30,000 Arcane damage to all other players within 6 yards, amplfying the damage by 30,000.
.' _Arcane Shock_ hits the targeted player for 29,000 Arcane damage every 2 seconds for 20 seconds.
.' _Chains of Shadow_ deals Shadow damage to the target, jumping to up to 3 additional targets.
.' _Flaming Spear_ sears the flesh of a player, inflicting normal weapon damage plus 30,000 Fire damage every 2 seconds for 20 seconds.
.' _Lightning Fists_ slams a lightning fist into the ground, causing a shockwave that deals damage to players in front of Feng.
.' _Wildfire Spark_ ignites a random player, causing them to explode after 5 seconds and igniting a wildfire at their location.
|modelnpc Feng the Accursed##60009
|confirm
step
map Mogu'shan Vaults/1
path loop off
path	32.2,23.2	32.2,13.2	Mogu'shan Vaults/2 77.6,67.1
path	Mogu'shan Vaults/2 77.7,47.3
.' Follow the path, clearing trash as you make your way to the second boss, Gara'jal the Spiritbinder. |goto Mogu'shan Vaults/2 77.7,47.3 <5 |noway |c
step
goto 77.0,35.9
'_Gara'jal the Spiritbinder_
.' _Banishment_ banishes the target to the spirit world.
.' _Final Destination_ Gara'jal harnesses his great power and rips the souls from all raid members.
.' _Summon Spirit Totem_ summons a totem that provides a conduit between this world and the spirit world.
.' _Voodoo Doll_ damage received is duplicated to all other Voodoo Dolls.
|modelnpc Gara'jal the Spiritbinder##60143
|confirm
step
.' Congratulations, you have finished this section of the Mogu'shan Vaults raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\The Vault of Mysteries (LFR)",[[
description This guide will walk you through the Vault of Mysteries Raid.
author support@zygorguides.com
step
map Mogu'shan Vaults/2
path loop off
path	45.3,34.5	33.8,29.9	33.9,43.0
.' Follow the path as you make your way towards the first boss. |goto 33.9,43.0 <5 |noway |c
step
map Mogu'shan Vaults/2
path loop off
path 31.7,69.3	24.2,66.5
.' Follow this path, clearing trash to the first boss, the Spirit Kings. |goto 24.2,66.5 <5 |noway |c
step
goto 18.3,52.0
'_The Spirit Kings_
'_Qiang the Merciless_
.' _Annihilate_ inflicts a total of 1,500,000 damage to players in front of Qiang within melee range.
.' _Flanking Orders_ calls in a wave of Mogu adds.
.' _Massive Attacks_ deals a total of 800,000 damage split among all players hit.
.' _Impervious Shield_ makes Qiang with a shield, making him immune to damage. If damaged while this shield is up, he retaliates, dealing damage to the raid.
'_Subetai the Swift_
.' _Pillage_ affects a random player and all other players within 8 yards of them, reducing damage, healing done and armor by 50%.
.' _Sleight of Hand_  damaging Subetai while this is up causes _Robbed Blind_. Subetai can be stunned during this.
.' _Robbed Blind_ debuff that reduces the target's damage and healing done by 50% and increasing their Physical damage taken by 50%.
.' _Volley_ inflicts 92,000-103,000 Physical damage to players in a _cone_ in front of Subetai.
'_Meng the Demented_
.' _Cowardice_ increases Meng's insanity level every 0.5 seconds. Once fully insane he switches personalities. Also reflects a portion of damage back at attacking players.
.' _Crazed_ increases Meng's insanity level every 0.5 seconds. Once fully insane he switches personalities. Also reflects a portion of damage back at attacking players.
.' _Delirious_ doubles Meng's sanity gains.
.' _Maddening Shout_ AoE that deals 63,000-66,000 Shadow damage every 3 seconds and causing players to hate each other. Players regain their sanity after taking 30,000 damage from another raid member.
'_Zian the Endless Shadow_
.' _Charged Shadows_ inflicts 97,500-102,500 Shadow damage to enemies standing within 8 yards of each other.
.' _Shadow Blast_ deals 100,000 Shadow damage to enemies within 8 yards of the targeted player.
.' _Undying Shadows_ creates an Undying Shadow at the targeted area, dealing 30,000 Shadow damage every second to players within 10 yards.
.' _Shield of Darkness_ is only used in Heroic mode. Any damage dealt to Zian will deal 300,000 Shadow damage to the raid.
|modelnpc Qiang the Merciless##60709
|modelnpc Subetai the Swift##60710
|modelnpc Meng the Demented##60708
|modelnpc Zian of the Endless Shadow##6070
|confirm
step
map Mogu'shan Vaults/3
path loop off
path	Mogu'shan Vaults/2 24.7,69.6	Mogu'shan Vaults/2 33.3,69.7	Mogu'shan Vaults/2 27.1,51.6
path	61.0,12.4	62.0,21.7	68.7,26.8
path	66.0,41.4	57.7,41.4	49.2,43.6
path	44.5,51.3	30.0,51.3
.' Follow the path, clearing trash as you make your way to the next boss, Elegon. |goto 30.0,51.3 <5 |noway |c
step
goto Mogu'shan Vaults/3 22.7,51.7
'_Elegon_
.' _Catastrophic Anomaly_ causes the Engine of Nalak'sha to explode and deal 90,200-100,000 Arcane damage to the raid.
.' _Celestial Breath_ applies a DoT to players in a _cone_ in front of Elegon that deals Arcane damage.
.' _Draw Power_ Elegon creates 6 Energy Cores and increasing his damage taken by 10%.
.' _Radiating Energies_  applies an Arcane DoT to the entire raid.
.' _Unstable Energy_ occurs if the Empyreal Fosuses are active. Elegon becomes immune to damage and deals 175,000 damage to the raid.
|modelnpc Elegon##60410+
|confirm
step
map Mogu'shan Vaults/3
path loop off
path	45.4,51.5	65.3,41.4	68.7,56.2
.' Follow the path towards the final boss, the Will of the Emperor. |goto 68.7,56.2 <5 |noway |c
step
.' Take these stairs down to the lower platform. |goto Mogu'shan Vaults/3 72.0,66.8 <5 |c
step
goto 68.6,82.2
'_Will of the Emperor_
.' Adds continuously spawn during this fight. They should be killed off while damaging the bosses.
'_Qin-xi_
.' _Stomp_ inflicts 150,000 Physical damage and stuns nearby enemies for 2 seconds.
'_Jan-xi_
.' _Stomp_ inflicts 150,000 Physical damage and stuns nearby enemies for 2 seconds.
|modelnpc Qin-xi##60399
|modelnpc Jan-xi##60400
|confirm
step
.' Congratulations, you have finished this section of the Mogu'shan Vault raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\The Dread Approach (LFR)",[[
description This guide will walk you through the Dread Approach Raid.
author support@zygorguides.com
step
map Heart of Fear/1
path loop off
path	34.3,76.4	41.3,61.5	50.6,61.4
path	54.6,54.0	61.4,62.5
Follow the path, clearing trash as you make your way to the first boss, Imperial Vizier Zor'lok. |goto 61.4,62.5 <5 |noway |c
step
goto 62.8,66.3
'_Imperial Vizier Zor'lok_
'_Phase 1_
.' After engaged, Zor'lok will fly to a randomly chosen platform. He will move to another platform at 80% and 60% health. Each platform gives him a different buff.
.' _Left Platform:_ _Attenuation_ creates moving circles, sonic rings. These deal a moderate amount of damage and can be avoided.
.' _Middle Platform:_ _Convert_ Zor'lok mind controls random players. These players must be brought down to 50% health.
.' _Right Platform:_ _Force and Verve_ creates 3 shields which can protect raid members by reducing their damage taken by 40%. Zor'lok then releases a violent burst of sound, damaging the entire raid every second for 10 seconds.
.' _Inhale_ increases the damage of _Exhale_ by 50%.
.' _Exhale_ deals a large amount of damage to a random player, also stunning them for 6 seconds.
'_Phase 2_
.' Phase 2 starts when Zor'lok reaches 40% health. He flies to the center of the room and uses all the abilities from Phase 1.
.' He also gains _Pheromoes of Zeal_, increasing his damage by 10% and haste by 20. Kill him as quickly as possible to avoid death.
|modelnpc Imperial Vizier Zor'lok##62980
|confirm
step
map Heart of Fear/1
path loop off
path	57.8,58.2	55.4,52.4	59.9,43.8
path	68.0,34.6	51.9,35.9	59.8,28.8
Follow the path, clearing trash as you make your way to the next boss, Blade Lord Ta'yak. |goto 59.8,28.8 <5 |noway |c
step
goto 59.8,22.3
'_Blade Lord Ta'yak_
'_Phase 1_
.' Phase 1 ends when Ta'yak reaches 20% health.
.' _Tempest Slash_ creates a tornado that moves around, dealing 75,000 Nature damage and a knockback if touched.
.' _Overwhelming Assault_ stacking debuff used on tanks, dealing 150% weapon damage and increasing further damage from Overwhelming Assault by 100%.
.' _Wind Step_ Ta'yak teleports to a random player, applying a DoT to all players within 10 yards that deals 55,000 damage every 2 seconds for 30 seconds.
.' _Unseen Strike_ occurs after Ta'yak vanishes. After 5 seconds it deals 3,000,000 total Physical damage and a knockback to the target and players in a 15 yard _cone_ in front of him. This damage is split between all players affected.
'_Phase 2_
.' Ta'yak pushes the raid to one end of the room and then flies to the other end and starts spamming the raid with a strong wind and tornados that knock them back to the opposite end of the room. The goal is to get back to Ta'yak by avoiding the tornadoes and then finish him off.
.' _Intensify_ is a stacking buff that deals 15,000 damage to the raid and increases his damage and haste by 5% per stack.
.' At 10% health, Ta'yak _changes sides_, going to the opposite end of the room and coninues creating wind and tornadoes.
|modelnpc Blade Lord Ta'yak##62543
|confirm
step
map Heart of Fear/1
path loop off
path	44.9,32.7	35.6,32.7	32.2,15.2
path	Heart of Fear/2 66.2,20.2
Follow the path, clearing trash as you make your way to the final boss, Garalon. |goto Heart of Fear/2 66.2,20.2 <5 |noway |c
step
goto Heart of Fear/2 66.3,16.7
'_Garalon_
.' Killing Garalon's legs takes priority, as killing them slows Garalon by 15% and reduces his health by 3%. _Mend Leg_ causes a random leg to respawn.
.' _Furious Swipe_ deals damage to all players in a _cone_ in front of Garalon. If he doesn't hit at least 2 players, he will gain Fury, a stacking buff that increases his damage and movement speed by 10% for 30 seconds.
.' _Pheromones_ is a debuff that deals 15,000 Nature damage every 2 seconds to the raid and creates a Pheromone Trail that increases Pheromones damage by 10% per stack.
.' _Crush_ deals 150,000 damage to the raid and knocks them down for 2 seconds. Any player underneath Garalon when this occurs takes an additional 800,000 damage.
.' _Enrage_ occurs after 7 minutes of combat. It will greatly increase his attack speed and damage. This is designed to wipe the raid. Kill him before he reaches this stage to avoid death.
|modelnpc Garalon##63191
|confirm
step
Congratulations, you have finished this section of the Heart of Fear raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Nightmare of Shek'zeer (LFR)",[[
description This guide will walk you through the Nightmare of Shek'zeer Raid.
author support@zygorguides.com
step
map Heart of Fear/2
path loop off
path	68.3,26.3	69.2,37.8	66.8,38.0
.' Follow the path to the first boss, Wind Lord Mel'jarak. |goto 66.8,38.0 <5 |noway |c
step
goto 66.4,31.8
.' 3 different types of adds spawn throughout this fight.
.' _Priests:_ heal and buff their damage.
.' _Warriors:_ charge their target.
.' _Hunters:_ apply debuffs to the raid.
'_Wind Lord Mel'jarak_
.' _Rain of Blades_ deals raid-wide Physical damage every 0.5 second.
.' _Whirling Blade_ deals 120,000 Physical damage to all players in its path. Upon reaching its target, it returns to Mel'jarak, inflicting another 120,000 damage to anyone in its return path.
.' _Wind Bomb_ throws a bomb at a player, dealing 100,000 Nature damage to players within 5 yards of where it lands. After 3 seconds, it arms and will detonate if any player comes within 6 yards, usually wiping the raid.
|modelnpc Wind Lord Mel'jarak##62397
|confirm
step
map Heart of Fear/2
path loop off
path	66.2,40.3	66.2,47.1	66.0,52.5
path	66.2,64.6	60.2,73.8	55.1,73.7
path	50.7,74.0	47.6,78.9	47.4,68.5
.' Follow the path, clearing trash as you make your way to the next boss,  Amber-Shaper Un'sok. |goto 47.4,68.5 <5 |noway |c
step
goto 45.5,73.8
'_ Amber-Shaper Un'sok_
.' _Amber Scalpel_ targets a random player with a beam that deals large damage and leaves a trail of amber that forms Living Amber adds.
.' _Parasitic Growth_ is a DoT that deals increasing damage as the affected player is healed.
.' _Reshape Life_ turns the targeted player into a Mutated Construct.
'_Mutated Construct_
.' _Amber Strike_ does a large amount of damage to the target and increases their damage taken by 10% for 15 seconds as well as interrupting them.
.' _Struggle for Control_ a small self-stun used to prevent _Amber Explosion_ from being cast. Costs willpower.
.' _Consume Amber_ heals yourself for 2,000,000, increases your maximum health and restores 20 willpower.
.' _Break Free_ frees you from the Construct. Can only be used while under 20% health.
|modelnpc Amber-Shaper Un'sok##62511
|confirm
step
map Heart of Fear/2
path loop off
path	42.1,73.8	34.8,77.8	30.9,75.2
.' Follow the path, clearing trash as you make your way to the final boss, Grand Empress Shek'zeer. |goto 30.9,75.2 <5 |noway |c
step
goto 28.9,73.9
'_Grand Empress Shek'zeer_
.' _Dissonance Field_ creates 2 vortexes which explode after 60 seconds, dealing 200,000 damage to the entire raid. Players standing in a vortex cannot receive healing and casting spells inside it reduces the explosion timer.
.' _Cry of Terror_ is a debuff applied to a random player, dealing 45,000 Shadow damage to the rest of the raid every 2 seconds. Standing in the vortex negates this damage.
.' _Visions of Demise_ is a debuff that affects 2 random players. After 4 seconds they are Feared and will deal damage to everyone around them.
.' _Consuming Terror_ inflicts 200,000 Shadow damage and Fears players in a _cone_ in front of Shek'zeer.
.' After 150 seconds the boss will disappear and summon a group of adds. These are usually tanked apart so they don't increase each others' damage.
|modelnpc Grand Empress Shek'zeer##62837
|confirm
step
Congratulations, you have finished this section of the Heart of Fear raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Terrace of Endless Spring (LFR)",[[
dungeon 886
description This guide will walk you through the Terrace of Endless Spring Raid.
author support@zygorguides.com
step
map Terrace of Endless Spring
path loop off
path	80.5,38.4	76.1,59.9
.' Follow the path, clearing trash around the first boss, the Protectors of the Endless. |goto 76.1,59.9 <5 |noway |c
step
goto 78.5,48.9
'_Protectors of the Endless_
'_Elder Regail_
.' _Lightning Bolt_ inflicts 125,000-145,000 Nature damage to the targeted player.
.' _Lightning Prison_ stuns 2 players and deals 28,000 Nature damage to all players within 7 yards.
.' _Lightning Storm_ is an AoE, starting from Regail's location and pulsing outward in 3 pulses, dealing high Nature damage.
'_Elder Asani_
.' _Water Bolt_ deals 100,000 Nature damage to the target and all additional players within 3 yards of that target.
.' _Cleansing Waters_ is a buff that heals Asani's allies and can be dispelled.
.' _Corrupted Waters_ spawns an orb in the center of the room, increasing the Protectors' attack and casting speeds by 50%. When destroyed it increases players' attack and casting speed by 25% for 20 seconds when standing in the inner circle.
'_Protector Kaolan_
.' _Touch of Sha_ occurs if you kill Kaolan last. It deals 38,000 Shadow damage every 3 seconds to a targeted player for the entire fight.
.' _Defiled Ground_ are summoned under the tank, dealing 110,000 Shadow damage every second and does not despawn.
.' _Expel Corruption_ is an AoE that deals up to 195,000 Shadow damage to all players within 30 yards. Damage is decreased the further away a player is from Kaolan when this occurs.
|modelnpc Elder Regail##60585
|modelnpc Elder Asani##60586
|modelnpc Protector Kaolan##60583
|confirm
step
goto 78.5,48.9
'_Tsulong_
'_Night Phases_
.' Damage Tsulong as much as possible in these phases.
.' _Shadow Breath_ _frontal cone_ ability, dealing 240,000 Shadow damage and increasing Shadow damage taken by 100% for 30 seconds.
.' _Dread Shadows_ is a stacking DoT that deals 30,000 Shadow damage and increases damage from Dread Shadows by 10% per stack. This is removed by running through a _Sunbeam_ Tsulong creates.
.' _Nightmares_ inflicts 110,000 Shadow damage and Fears all players within 8 yards in the targeted area.
'_Day Phases_
.' Heal Tsulong as much as possible in these phases. He is friendly, and helps kill the Sha that spawn by using _Sun Breath_. This is a cone that also restores 25% maximum mana to every player in front of him.
.' During these phases, Tsulong is not attackable and summons waves of Sha to attack the raid.
.' _Embodied Terror:_ Sha with large amounts of health. _Terrorize_ deals high damage and should be dispelled.
.' _Fright Spawn:_ spawn when an Embodied Terror is killed. _Fright_ deals high damage and a Fear to players within 30 yards.
.' _Unstable Sha:_ spawn in the outer areas of the room and move toward Tsulong. If they reach Tsulong they explode.
|modelnpc Tsulong##62442
|confirm
step
goto 58.6,48.6
'_Lei Shi_
.' _Hide_ Lei Shi vanishes, hiding from players. AoE attacks can hit her to make her reveal herself.
.' _Get Away_ is an AoE that pushes the raid back, away from Lei Shi and deals 90,000 Shadow damage. Running towards Lei Shi halves the damage it deals every second.
.' _Protect_ Lei Shi shields herself, becoming immune to damage and summoning elementals to attack the raid. Ends after 1 elemental is killed.
.' _Spray_ water bursts from the ground at the targeted player's location, dealing 110,000 Frost damage to all players within 2.5 yards and increasing their Frost damage taken by 16% for 10 seconds.
|modelnpc Lei Shi##62983
|confirm
step
goto 39.2,48.6
'_Sha of Fear_
.' _Breath of Fear_ occurs about every 30 seconds. when Sha of Fear gains a full energy bar. It deals 500,000 Shadow damage and a Fear to all players. Those standing in the glowing light spot in the middle of the room will be unaffected by this.
.' _Terror Spawn_ group of 2 adds that spawn. They can only be damaged from behind.
.' _Eerie Skull_ deals 74,000-90,000 Shadow damage to a random raid member.
.' _Ominous Cackle_ occurs every 90 seconds. It takes 5 random players to an outer shrine where they must kill the corrupted guardian, granting _Fearless_.
.' _Fearless_ is a buff applied to players killing the corrupted guardian in one of the outer shrines. It increases damage and healing done by 60%, makes you immune to Breath of Fear, and greatly increases movement speed.
|modelnpc Sha of Fear##60999
|confirm
step
Congratulations, you have finished the Terrace of Endless Spring raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Last Stand of the Zandalari (LFR)",[[
description This guide will walk you through the Last Stand of the Zandalari Raid.
author support@zygorguides.com
step
map Throne of Thunder/1
path loop off
path	34.9,23.7	39.8,23.8	46.2,25.8
path	53.4,25.8
.' Follow the path, clearing trash as you make your way to the first boss, Jin'rokh the Breaker. |goto 53.4,25.8 <5 |noway |c
step
goto 59.5,25.8
'_Jin'rokh the Breaker_
.' _Thundering Throw_ is usually used on the tank, throwing them at a Mogu statue, inflicting 250,000 Physical damage and dealing 200,000 Nature damage and a 5 second stun to players within an 8 yard area.
.' _Conductive Water_ is a pool formed at the base of broken statues. Standing in the water increases healing by 60% and damage by 40%, but also increases damage taken from Jin'rokh's abilities by 50%.
.' _Lightning Storm_ deals 70,000 damage to the raid every second for 15 seconds. Also turns _Conductive Water_ to _Electrified Water_, quickly killing anyone still standing in it.
.' _Static Burst_ deals 95,000-105,000 Nature damage to the raid. After 3 seconds it applies 10 stacks of _Static Wounds_ to his current target.
.' _Static Wound_ is a debuff that deals 20,000 Nature damage per stack to the affected player each time he takes melee damage. Also deals a third of this damage to nearby players.
.' _Focused Lightning_ occurs on a healer or ranged DPS. Lightning moves towards the targeted player and deals 175,000 Nature damage upon reaching them, also damaging all other players within 8 yards of the initial explosion.
|modelnpc Jin'rokh the Breaker##69465
|confirm
step
map Throne of Thunder/1
path loop off
path	59.5,25.8	59.5,43.6	59.5,53.5
path	59.6,74.9	62.1,79.8	76.0,79.6
path	83.1,79.6
.' Follow the path, clearing trash as you make your way to the next boss, Horridon. |goto 83.1,79.6 <5 |noway |c
step
goto 26.8,78.7
.' Waves of adds are spawned throughout this fight. It is recommended the raid kills them and then goes back to attacking Horridon.
.' When a Dinomancer dies it drops an Orb of Control. Clicking this will destroy the gate and stop the adds spawning for a short time.
'_Horridon_
.' _Triple Puncture_ deals 370,000-430,000 damage to the target and increases their damage taken from Triple Puncture by 10% per stack. Lasts 90 seconds.
.' _Double Swipe_ is a _frontal and rear cone_ attack. Avoid this by standing at Horridon's sides.
.' _Charge_ is used on random players. Horridon charges at the target, immediately follwed by _Double Swipe_.
.' When Horridon is at 30% health he spawns War-God Jalak. Kill them both as fast as possible to avoid death.
|modelnpc Horridon##68476
|confirm
step
map Throne of Thunder/2
path loop off
path	27.2,68.4	27.1,47.4	27.2,35.4
path	27.2,26.0	33.5,19.4	38.0,19.4
path	45.1,14.1
.' Follow the path, clearing trash as you make your way to the final boss, the Council of Elders. |goto 45.1,14.1 <5 |noway |c
step
goto 50.1,18.4
'_Council of Elders_
'_Kazra'jin_
.' _Discharge_ Kazra'jin stuns himself for 20 seconds, reflecting 5% damage back to attackers.
.' _Overload_ Kazra'jin stuns himself for 20 seconds, reflecting 40% damage back to atteckers.
.' _Reckless Charge_ deals 90,000 Nature damage to the target and all players inbetween and knocking back all players within 5 yards of where he lands.
'_Sul the Sandcrawler_
.' _Quicksand_ spawns a pool of quicksand under the targeted player's feet, rooting them in place and dealing Nature damage every second.
.' _Sand Bolt_ inflicts 150,000-175,000 Nature damage to the target and all players within 5 yards of that target.
.' _Sandstorm_ summons a sandstorm that deals 38,500 damage every second for 8 seconds.
'_Frost King Malakk_
.' _Biting Cold_ blasts the target with ice, inflicting 95,000 Frost damage.
.' _Frostbite_ deals 110,000 Frost damage and applies 5 stacks of Frostbite.
.' _Frigid Assault_ imbues Malakk's axes with ice, dealing an additional 75,000 damage to his target and stunning them if it reaches 15 stacks.
'_High Priestess Mar'li_
.' _Blessed Loa Spirit_ summons a Blessed Loa Spirit add that moves towards the council member with the lowest health. If reached, they are healed for 5% of their maximum health.
.' _Shadowed Loa Spirit_ summons a Shadowed Loa Spirit that follows a random player. If it gets within 6 yards of them it instantly kills them.
.' _Wrath of the Loa_ hits the targeted player for 150,000-160,000 damage.
.' _Twisted Fate_ links 2 players' souls together, dealing 250,000 Shadow damage every 3 seconds. The damage dealt is reduces the further apart the players are.
|modelnpc Kazra'jin##69134
|modelnpc Sul the Sandcrawler##69078
|modelnpc Frost King Malakk##69131
|modelnpc High Priestess Mar'li##69132
|confirm
step
Congratulations, you have finished this section of the Throne of Thunder raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Forgotten Depths (LFR)",[[
description This guide will walk you through the Forgotten Depths Raid.
author support@zygorguides.com
step
map Throne of Thunder/3
path loop off
path	20.3,82.0	24.6,82.5
.' Follow the path, clearing trash as you make your way to the first boss, Tortos. |goto 24.6,82.5 <5 |noway |c
step
goto 31.5,82.0
'_Tortos_
.' _Call of Tortos_ summons smaller turtle adds to attack the raid. They spin around, dealing damage and a knockback to those near them.
.' _Furious Stone Breath_ deals 150,000 Nature damage to the raid every 0.5 seconds for 4.5 seconds. Can be interrupted by kicking shells from dead turtle adds.
.' _Quake Stomp_  Tortos stuns the raid for 1 second, inflicting damage equal to 65% of their maximum health. This also increases the requency of Rockfalls for 8 seconds.
.' _Snapping Bite_ Tortos snaps his jaws closed, dealing 510,000-540,000 damage.
|modelnpc Tortos##67977
|confirm
step
map Throne of Thunder/3
path loop off
path	32.8,80.9	33.1,72.3	33.7,62.5
path	41.7,58.7	38.3,48.6	41.0,38.9
path	43.3,16.9	52.5,40.0	58.9,50.5
path	63.4,57.5	69.6,48.8	70.2,37.7
.' Follow the path, clearing trash as you make your way to the next boss, Magaera. |goto 70.2,37.7 <5 |noway |c
step
goto 72.1,25.3
'_Magaera_
.' The only way to damage Magaera is to kill the Flaming, Frozen, and Venemous Heads that spawn.
'_Flaming Head_
.' _Cinders_targets a player, dealing 105,000 Fire damage and an additional 75,00 Fire damage every second for 30 seconds. If removed, a pool of flames is created, dealing 105,000 Fire damage every second to all players within 5 yards.
.' _Ignite Flesh_ _frontal cone_ attack that deals 125,000 Fire damage every second for 3 seconds. Also applies a DoT that burns for 30,00 Fire damage every second for 45 seconds.
.' _Magaera's Rage_ occurs if no player is within melee range. It deals 250,000 Fire damage to all players within 5 yards of the targeted area.
'_Frozen Head_
.' _Arctic Freeze_ _frontal cone_ that deals 125,000 Frost damage every seconds for 3 seconds. Players hit by this also get a debuff that stuns them for 20 seconds if it reaches 5 stacks.
.' _Megaera's Rage_ occurs if no player is within melee range. It deals 250,000 Frost damage to all players within 5 yards of the targeted area.
.' _Torrent of Ice_ channels a beam of ice at a player, inflicting 60,000 Frost damage every 0.5 seconds for 8 seconds and slowing movement speed of anyone who touches it.
'_Venemous Heads_
.' _Rot Armor_ _frontal cone_ that deals 127,500 Nature damage every second for 3 seconds and increases the damage taken of those affected by 10% for 45 seconds.
.' _Megaera's Rage_ occurs if no player is within melee range. It deals 250,000 Nature damage to all players within 5 yards of the targeted area.
|modelnpc Flaming Head##70212
|modelnpc Frozen Head##70235
|modelnpc Venemous Head##70247
|confirm
step
map Throne of Thunder/4
path loop off
path	Throne of Thunder/3 66.8,39.2	Throne of Thunder/3 66.4,43.4	Throne of Thunder/3 72.7,53.1
path	22.0,35.3	15.6,55.5	23.5,64.4
path	31.6,65.8	40.4,74.6	42.8,76.6
path	43.4,84.5	54.3,90.0	65.9,86.1
path	72.0,81.5	74.9,67.3	62.7,57.5
path	53.8,52.0
.' Follow the path, clearing trash as you make your way to the final boss, Ji-Kun. |goto 53.8,52.0 <5 |noway |c
step
goto 49.7,41.6
'_Ji-Kun_
.' _Caw_ Ji-Kun releases concentrated sound waves at multiple players. When they reach their targets they explode and deal 270,000-330,000 damage to all players within 8 yards.
.' _Feed Young_ summons pools of food for the young, dealing damage to players standing in it.
.' _Down Draft_ summons a powerful draft of air that pushes players off the platform.
.' _Infected Talons_ DoT that inflicts 30,000 Nature damage every 3 seconds.
.' _Quils_ deals 65,000 damage every second to all players.
|modelnpc Ji-Kun##69712
|confirm
step
Congratulations, you have finished this section of the Throne of Thunder raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Halls of Flesh-Shaping (LFR)",[[
description This guide will walk you through the Halls of Flesh-Shaping Raid.
author support@zygorguides.com
step
map Throne of Thunder/5
path loop off
path	77.0,10.6	71.1,12.9	71.2,15.8
path	73.9,20.4	71.4,30.8
.' Follow the path, clearing trash as you make your way to the first boss, Durumu the Forgotten. |goto 71.4,30.8 <5 |noway |c
step
goto 71.1,34.4
'_Durumu the Forgotten_
.' _Disentigration Beam_ fires of beam of energy towards the edge of the platform, killing any player it touches.
.' _Arterial Cut_ applies a bleed DoT to the target.
.' _Gaze_ deals 50% weapon damage to the entire raid.
.' _Hard Stare_ deals 135%-165% weapon damage and applies _Serious Wound_ to the target.
.' _Serious Wound_ debuff that reduces the target's healing received by 10%.
|modelnpc Durumu the Forgotten##68036
|confirm
step
map Throne of Thunder/5
path loop off
path	71.1,43.2	70.9,51.2	72.1,61.4
path	71.0,71.5	74.0,77.9	63.6,78.0
path	57.2,78.1
.' Follow the path, clearing trash as you make your way to the next boss, Primordius. |goto 57.2,78.1 <5 |noway |c
step
goto 57.2,78.2
'_Primordius_
.' _Primordial Strike_ is a _frontal cone_. The tank should be the only one in front of Primordius when this occurs.
.' _Malformed Blood_ is a stacking debuff usually applied to the tank. It deals 20,000 damage per stack every tick.
.' Each time Primordius' energy reaches 100% he gains a stacking buff that increases damage done by 10% per stack and giving additional abilities.
.' _Erupting Pustules_ deals 120,000 Nature damage to players within 2.5 yards of the targeted area.
.' _Metabolic Boost_ increases Primordius' attack speed by 50% and decreases his cooldowns by 50%.
.' _Volatile Pathogen_ deals 60,000 damage every second for 10 seconds.
.' _Acidic Spines_ deals 100,000 Nature damage to all players within 5 yards of the Primordius' target.
.' _Caustic Gas_ is a _cone_ that deals 800,000 total Nature damage split between all players affected.
.' _Ventral Sacs_ deals 22,000 damage every second to the entire raid.
|modelnpc Primordius##69017
|confirm
step
map Throne of Thunder/5
path loop off
path	50.9,78.0	39.9,78.1	42.8,72.2
path	42.5,61.1
.' Follow the path, clearing trash as you make your way to the final boss, Dark Animus. |goto 42.5,61.1 <5 |noway |c
step
goto 43.0,57.1
.' Adds spawn throughout the fight.
.' _Anima Golem:_ weak mobs, don't require anything special.
.' _Large Anima Golem:_ needs to be tanked.
.' _Massive Anima Golem_ should be tanked, has a large _frontal cleave_.
'_Dark Animus_
.' _Touch of the Animus_ affects a random player, inflicting 30,000 Fire damage every 4 seconds for the rest of the fight.
.' _Anima Font_ affects a player with Touch of the Animus, causing them to shoot Anima bolts from nearby locations that deal 250,000 Fire damage.
.' _Anima Ring_ summons spheres in a circle around the taget, moving inward. Players that touch the spheres consume it, increasing melee damage taken by 200% per stack.
.' _Interrupting Jolt_ deals 400,000 Nature damage and inerrupts the raid's spellcasting.
|modelnpc Dark Animus##69427
|confirm
step
Congratulations, you have finished this section of the Throne of Thunder raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Pinnacle of Storms (LFR)",[[
description This guide will walk you through the Pinnacle of Storms Raid.
author support@zygorguides.com
step
map Throne of Thunder/5
path loop off
path	35.1,43.1	33.3,38.9	33.5,32.5
path	43.7,31.9	46.8,27.5	46.9,18.7
path	Throne of Thunder/6 25.5,64.5
.' Follow the path, clearing trash as you make your way to the first boss, Iron Qon. |goto Throne of Thunder/6 25.5,64.5 <5 |noway |c
step
goto Throne of Thunder/6 34.6,58.9
'_Iron Qon_
.' _Impale_ is a stacking debuff that deals normal weapon damage plus 40,000 bleeding damage every 2 seconds.
.' _Rising Anger_ increases Qon's damage and attack speed by 10%.
.' Qon throws out lines of ice, fire, and lightning during the fight, avoid standing in these as well as tornadoes and goo.
|modelnpc Iron Qon##68078
|confirm
step
map Throne of Thunder/6
path loop off
path	40.1,59.0	62.2,58.9	69.1,51.3
path	73.8,44.3
.' Follow the path, clearing trash as you make your way to the next boss, the Twin Consorts. |goto 73.8,44.3 <5 |noway |c
step
goto Throne of Thunder/6 78.7,38.0
'_Twin Consorts_
'_Lu'lin_
.' _Beast of Nightmares_ summons a Beast of Nightmares add that can only be damaged by Lu'lin's target.
.' _Bloom Moon Lotus_ summons a Moon Lotus flower, which puts nearby players to sleep for 5 seconds.
.' _Cosmic Barrage_ deals 175,000 damage every 0.5 seconds for 2 seconds.
'_Suen_
.' _Fan of Flames_ deals a large amount of damage to the target, also increasing Fire damage they take by 25%.
.' _Blazing Radiance_ deals 35,000 Fire damage to the raid and increasing their Fire damage taken by 5%.
.' _Flames of Passion_ Suen leaps at a player, dealing 46,000-54,000 Fire damage and knocking all nearby players back.
.' _Nuclear Inferno_ deals 100,000 Fire damage every second for 4 seconds. While channeling, her spell deflection and dodge chance are both 100%.
|modelnpc Lu'lin <Mistress of Solitude>##68905
|modelnpc Suen <Mistress of Anger>##68904
|confirm
step
map Throne of Thunder/6
path loop off
path	84.9,48.2	87.0,57.0	85.0,65.7
path	87.4,74.1
.' Follow the path, clearing trash as you make your way to the final boss, Lei Shen. |goto 87.4,74.1 <5 |noway |c
step
goto Throne of Thunder/7 49.8,15.4 |n
.' Click and use the Displacement Pad here. |goto Throne of Thunder/7 50.7,50.9 <5 |noway |c
step
goto 50.7,51.0
'_Lei Shen_
.' _Overcharge_ charges the targeted player with lightning, placing a ring of lightning at their feet, with a safe spot in the very center. Players within 5-40 yards take 100,000 Nature damage and are stunned for 3 seconds.
.' _Static Shock_ places a blue arrow above the targeted player and deals 400,000 total damage after 8 seconds, split between all players within 8 yards of the targeted player.
.' _Bouncing Bolt_ Lei Shen throws a bolt of lightning across the platform. It creates a blue circle where it lands, spawning an add if a player doesn't stand in the circle in time.
.' _Diffusion Chain_ deals 75,000 damage to the targeted player, then jumps to nearby players. Each time this hits a player it will also spawn a Diffused Lightning add.
.' _Decapitate_ usually affects the tank, dealing up to 3,000,000 damage. The damage dealt is decreased the further away the target is from Lei Shen.
.' _Thunderstruck_ is an AoE that deals 1,000,000 damage to all players in the targeted area. Damage dealt is decreased the further away the area is from Lei Shen.
|modelnpc Lei Shen##68397
|confirm
step
Congratulations, you have finished this section of the Throne of Thunder raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Vale of Eternal Sorrows (LFR)",[[
description This guide will walk you through the Vale of Eternal Sorrows Raid.
author support@zygorguides.com
step
map Siege of Orgrimmar/2
path loop off
path	89.3,45.2	79.3,57.3	59.1,49.6
path	48.1,51.1
.' Follow the path, clearing trash as you make your way to the first boss, Immerseus. |goto 48.1,51.1 <5 |noway |c
step
goto 41.8,45.6
'_Immerseus_
.' _Corrosive Blast_ is a _cone_ attack that deals 585,000-612,000 Shadow damage to the target and additional nearby players. Make sure the tank is the only one in front of Immerseus when this occurs.
.' _Sha Bolt_ deals 15,000 Shadow damage to all players within 5 yards of the target, also forming a _Sha Pool_.
.' _Sha Pool_ summons a swirling pool at multiple players' feet, dealing 10,000 Shadow damage every second for 10 seconds.
.' _Swirl_ creates moving void zones that deal move towards players, dealing damage every second. Immerseus then faces a random direction, rotating half-way around the room and casting a large torrent that deals 60,000 Shadow damage per second.
.' Adds spawn throughout the fight, they should be killed when they appear.
|modelnpc Immerseus##71543
|confirm
step
map Siege of Orgrimmar/2
path loop off
path	31.7,45.4	16.8,44.9	11.7,52.7
path	11.8,69.6
.' Follow the path, clearing trash as you make your way to the next boss, the Fallen Protectors. |goto 11.8,69.6 <5 |noway |c
step
goto Siege of Orgrimmar/1 27.0,18.0
'_Fallen Protectors_
'_Rook Stonetoe_
.' _Vengeful Strikes_ is a _cone_ that stuns all affected targets and deals 180,000 damage twice every second for 3 seconds. Make sure the tank is the only one in front of him when this occurs.
.' _Corrupted Brew_ inflicts 115,000-132,000 Shadow damage to players within 5 yards of the target and reducing their movement speeds by 50%.
.' _Corruption Kick_ spins rapidly in a circle, rooting himself in place dealing 117,000-125,000 Physical damage to targets within 10 yards every second for 4 seconds and applying a DoT that deals 70,000 Shadow damage every 2 seconds for 6 seconds.
'_He Softfoot_
.' _Shadowstep_ applies a DoT to the target that deals 80,000 Physical damage every 2 seconds.
.' _Instant Poison_ deals an additional 73,000-77,000 additional Nature damage on a successful melee attack. This effect has a 3 second cooldown.
.' _Noxious Poison_ causes successful melee attacks to create pools of poison that deal 85,000 Nature damage every second. This effect has a 3 second cooldown.
.' _Gouge_ stuns the target for 4 seconds if they are facing Softfoot.
'_Sun Tenderheart_
.' _Sha Sear_ deals Shadow damage every second to the target and all players within 5 yards around them.
.' _Shadow Word: Bane_ debuff that deals 100.000 Shadow damage every 3 seconds for 18 seconds. Each time it deals damage it will jump to an additional target, jumping up to 3 times.
.' _Calamity_ deals damage to the entire raid, hitting for 30% of each player's maximum health.
.' Whenever one of the three bosses reach 66% or 33% health they will summon adds to attack the raid. These should be killed off when they appear.
|modelnpc Rook Stonetoe##71475
|modelnpc He Softfoot##71479
|modelnpc Sun Tenderheart##71480
|confirm
step
map Siege of Orgrimmar/1
path loop off
path	33.8,19.0	36.3,30.2	40.3,40.0
path	41.9,44.8	45.5,60.0	43.0,70.5
path	Siege of Orgrimmar/3 56.7,30.2	Siege of Orgrimmar/3 70.3,35.9	Siege of Orgrimmar/3 70.2,54.1
.' Follow the path, clearing trash as you make your way to the next boss, Norushen. |goto Siege of Orgrimmar/3 70.2,54.1 <5 |noway |c
step
goto 54.2,59.2
'_Norushen_
.' Corruption is applied to the raid and has a unique bar for each player. Increased Corruption reduces the player's damage dealt to the boss. This can be cleansed by using a Purifying Light orb at the edge of the room, which takes the player to a unique, role-specific solo phase.
.' Adds will spawn throughout the fight, they should be killed as soon as they appear. Killing these, however, form void zones that increase players' corruption.
.' _Self Doubt_ increases the damage the target takes from by next _Unleashed Anger_ by 40%.
.' _Unleashed Anger_ deals 400,000 Physical damage to the target.
|modelnpc Norushen##72276
|confirm
step
map Siege of Orgrimmar/3
path loop off
path	46.2,61.8	32.0,80.5	21.6,71.7
path	27.6,54.3
.' Follow the path, clearing trash as you make your way to the final boss, the Sha of Pride. |goto 27.6,54.3 <5 |noway |c
step
goto 29.5,67.2
'_Sha of Pride_
.' Pride is the main mechanic of this fight, each player has a unique bar. Every 60 seconds Sha of Pride uses _Swelling Pride_, which causes a raide-wide effect based on each player's level of Pride.
.' _Bursting Pride: 25-49 Pride_ forms Sha energy at the target's location and explodes after 3 seconds, dealing 300,000 Shadow damage to all players within 4 yards.
.' _Projection: 50-74 Pride_ causes a projection to appear at a nearby location from the target, dealing 225,000 Shadow damage to all players after 8 seconds unless the targeted player is standing within it.
.' _Aura of Pride: 75-99 Pride_ creates an aura around the target, inflicting 250,000 Shadow damage to other nearby players within 4.5 yards every second.
.' _Overcome: 100 Pride_ players become mind controlled and attack other raid members, increasing their damage and healing by 50%. If another Swelling Pride is cast while mind controlled, the players are mind controlled for the rest of the fight.
.' _Wounded Pride_ applies a debuff to the target that causes them to gain 5 Pride whenever they take melee damage from the boss.
.' _Self-Reflection_ causes 5 players to spawn a reflection of themselves, creating 5 Reflection adds.
.' _Unleashed_ occurs when Sha of Pride reaches 30% health. It deals 245,000 Shadow damage to the raid every 10 seconds for the rest of the fight. Kill the Sha of Pride as fast as possible to avoid death.
|modelnpc Sha of Pride##71734
|confirm
step
Congratulations, you have finished this section of the Siege of Orgrimmar raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Gates of Retribution (LFR)",[[
description This guide will walk you through the Gates of Retribution Raid.
author support@zygorguides.com
step
map Siege of Orgrimmar/4
path loop off
path	84.0,28.6	76.7,30.8	79.8,38.5
path	81.4,40.7	81.6,48.8
.' Follow the path, destroying cannons along the beach. |goto 81.6,48.8 <5 |noway |c
step
goto 77.0,29.3
.talk 72302
.' Tell her" By your command."
.' This will start the first boss fight, make sure you are ready.
|confirm
step
goto 77.2,29.6
.' You will need to split your group into two, one group will need to clear the towers, and then use the two turrets on top of them to shoot down Galakras.
'_Galakras_
.' _Pulsing Flames_ deals 30,000 Fire damage. Each pulse increases Fire damage taken by 3%.
|modelnpc Galakras##72249
|confirm
step
map Siege of Orgrimmar/4
path loop off
path	69.3,33.0	59.4,37.4	55.1,54.9
path	44.5,68.9	35.1,70.9	33.6,53.6
.' Follow the path to the next boss, the Iron Juggernaut. |goto 33.6,53.6 <5 |noway |c
step
goto 32.9,48.5
'_Iron Juggernaut_
.' _Shock Pulse_ releases a seismic eruption that knocks all nearby players back and deals 73,000-76,000 Nature damage.
.' _Seismic Activity_ causes an earthquake that deals 40,000 Nature damage every second.
.' _Ignite Armor_ deals 30,000 Fire damage every second and increases Fire damage they take by 10% for 30 seconds.
.' _Flame Vents_  deals 300,000 Fire damage in a _cone_ in front of the boss. Make sure the tank is the only one in front of the Iron Juggernaut when this occurs.
.' _Explosive Tar_ summons tar at the targeted location that deals 20,000 Nature damage every second and reduces movement speed by 30%.
|modelnpc Iron Juggernaut##71466
|confirm
step
map Siege of Orgrimmar/4
path loop off
path	Siege of Orgrimmar/4 30.5,36.0	Siege of Orgrimmar/4 31.1,29.6	Siege of Orgrimmar/4 34.8,22.7
path	Siege of Orgrimmar/5 51.4,75.2	Siege of Orgrimmar/5 52.1,64.0	Siege of Orgrimmar/5 49.0,66.6
path	Siege of Orgrimmar/5 47.9,71.7
.' Follow the path, clearing trash at each point around the next boss, the Kor'kron Dark Shaman. |goto Siege of Orgrimmar/5 47.9,71.7 <5 |noway |c
step
goto 48.4,64.3
'_Kor'kron Dark Shaman_
.' The bosses are inside the building here, and will need to be pulled outside. A Hunter/Rogue can aggro them and then Feign Death/ Vanish to do this.
'_Earthbreaker Haromm_
.' _Froststorm Strike_ deals 300,000 Frost damage and increases damage taken from this attack by 25% for 30 seconds.
.' _Toxic Mist_ creates a toxic pool that deals 80,000 Nature damage every 3 seconds.
.' _Foul Stream_ deals 300,000 Nature damage to all players in a line in front of Haromm.
.' _Ashen Wall_ summons Ash Elementals to attack the raid. They automatically die after a set amount of time.
'_Wavebinder Kardris_
.' _Toxic Storm_ creates a storm that deals 300,000 Nature damage to players within 9 yards every 2 seconds. Also sometimes creates toxic tornadoes.
.' _Foul Geyser_ creates water that deals 300,000 Nature damage every 0.5 seconds for 8 seconds to players within 4 yards of the area.
.' _Falling Ash_ summons ash to fall on the targeted area, dealing 999,999 Fire damage to all players within 17 yards and 300,000 Fire damage at the moment of impact.
|modelnpc Earthbreaker Haromm##71859
|modelnpc Wavebinder Kardris##71858
|confirm
step
map Siege of Orgrimmar/5
path loop off
path	51.4,60.7	51.8,48.4	54.2,47.6
path	61.2,43.2	60.8,33.0	55.8,28.9
path	Siege of Orgrimmar/6 67.2,36.2	Siege of Orgrimmar/6 67.7,50.8	Siege of Orgrimmar/7 41.5,28.3
path	Siege of Orgrimmar/7 53.0,56.4
.' Follow the path, clearing trash as you make your way to the final boss, General Nazgrim. |goto Siege of Orgrimmar/7 53.0,56.4 <5 |noway |c
step
goto Siege of Orgrimmar/7 65.2,73.5
'_General Nazgrim_
.' _Battle Stance_ generates 1 Rage per seconds.
.' _Berserker Stance_ Nazgrim deals extra damage, but also takes extra damage.
.' _Defensive Stance_ reduces his damage taken, and increases Rage generated. DPS should stop damaging Nazgrim when he is in this stance.
.' _Sundering Blow_ deals 600,000 Physical damage and reduces the target's armor by 10% for 30 seconds.
.' _Bonecracker_ reduces the target's maximum health by 10% and causes them to bleed for 40,000 damage every second.
.' _Heroic Shockwave_ deals 300,000 Physical damage to players within 10 yards of the targeted area.
.' _Kor'kron Banner_ summons a banner that causes Nazgrim's allies to generate additional Rage for him.
.' _War Song_ deals damage to the entire raid, equal to 50% of each player's maximum health.
.' _Ravager_ Nazgrim throws his sword, dealing 370,000-430,000 damage to players within 6 yards every second and granting Nazgrim 5 Rage for each player hit.
|modelnpc General Nazgrim##71515
|confirm
step
Congratulations, you have finished this section of the Siege of Orgrimmar raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\The Underhold (LFR)",[[
description This guide will walk you through the Underhold Raid.
author support@zygorguides.com
step
map Siege of Orgrimmar/8
path loop off
path	20.4,66.3	23.2,60.9	28.3,60.6
path	34.0,59.3	38.4,57.5	41.4,62.9
path	51.3,61.1	57.2,55.5	53.4,66.1
path	47.5,55.3	44.2,48.0
.' Follow the path, clearing trash as you make your way to the first boss, Malkorok. |goto 44.2,48.0 <5 |noway |c
step
goto 41.6,42.3
'_Malkorok_
.' _Ancient Miasma_ is cast while Malkorok's energy builds up to 100. It converts all healing to absorption shields and deals 30,000 Shadow damage to the raid every 2 seconds.
.' _Seismic Slam_ deals 500,000 damage in a 5 yard area around a random player, knocking all affected players into the air.
.' _Arcing Smash_ deals 600,000 damage to all players in a _cone_. Make sure the tank is the only one in front of Malkorok when this occurs.
.' _Imploding Energy_ creates pools of energy that deal 450,000 Shadow damage after 4 seconds to players within 5 yards. If no players are damaged by the initial explosion, it instead deals 510,000 Shadow damage to the entire raid.
.' _Breath of Y'sharrj_ causes the areas recently hit by Arcing Smash/Seismic Slam to explode, dealing 1,200,000 Shadow damage.
.' _Blood Rage_ causes Malkorok's attack to now deal 1,800,000 total Physical damage, split between all players hit by the _cone_.
.' _Displaced Energy_ deals 250,000 Shadow damage to the target every 3 seconds for 9 seconds. Upon expiring, it explodes and deals an additional 450,000 Shadow damage to all players within 8 yards.
|modelnpc Malkorok##71454
|confirm
step
map Siege of Orgrimmar/8
path loop off
path	36.9,31.7	31.6,16.9	37.8,12.1
path	42.7,23.4	53.9,30.0	61.6,23.5
path	67.8,32.4	78.0,55.9	78.3,73.6
path	Siege of Orgrimmar/9 55.4,26.0
.' Follow the path, clearing trash as you make your way to the next boss, the Spoils of Pandaria. |goto Siege of Orgrimmar/9 55.4,26.0 <5 |noway |c
step
goto 52.7,29.2
'_Spoils of Pandaria_
.' The objective of this fight is to open all of the boxes in the rooms before the timer runs out. Opening a box spawns a group of adds that must be killed.
'_Mantid Adds_
.' _Commanders_ are kill priority. Avoid the tornadoes they spawn and kill off priests.
'_Mogu Adds_
.' _Statue_ are kill priority, but can be left if LFR. One person should stand in the beams they cast. Dispell Ritualists.
|modelnpc Spoils of Pandaria##71889
|confirm
step
map Siege of Orgrimmar/9
path loop off
path	41.5,40.2	37.4,51.4	40.5,57.9
path	46.3,70.7	46.9,76.2
.' Follow the path, clearing trash around the final boss, Thok the Bloodthirsty. |goto 46.9,76.2 <5 |noway |c
step
goto 51.3,82.1
'_Thok the Bloodthirsty_
.' _Acid Breath_ inflicts 485,000-512,00 Nature damage in a _frontal cone_. Make sure the tank is the only one in front of Thok when this occurs.
.' _Blood Frenzy_ is a buff that slowly increases Thok's damage and movement speed.
.' _Deafening Screech_ inflicts 243,000-255,000 Physical damage to the raid and interrupting spellcasting for 2 seconds.
.' _Fearsome Roar_ deals 200,000 Physical damage in a _frontal cone_ and increasing damage taken by those affected by 25%. Make sure the tank is the only one in front of Thok when this occurs.
.' _Freezing Breath_ deals 200,000 Frost damage in a _frontal cone_. After 5 stacks the target will be frozen solid in a tomb of ice and stunned.
.' _Mending_ heals Thok for 8% of his maximum health.
.' _Panic_ deals 200,000 damage in a _frontal cone_.
.' _Scorching Breath_ deals 200,000 Fire damage in a _frontal cone_.
.' _Tail Lash_ deals 200,000 damage to players behind Thok, also stunning them for 2 seconds.
|modelnpc Thok the Bloodthirsty##71529
|confirm
step
Congratulations, you have finished this section of the Siege of Orgrimmar raid!
]])
ZygorGuidesViewer:RegisterGuide("Zygor's Horde Dungeon Guides\\Raids\\Pandaria (LFR)\\Downfall (LFR)",[[
description This guide will walk you through the Downfall Raid.
author support@zygorguides.com
step
map Siege of Orgrimmar/8
path loop off
path	73.0,43.4	82.2,58.6	89.9,62.0
path	Siege of Orgrimmar/10 36.4,69.8
.' Follow the path, clearing trash as you make your way to the first boss, Siegecrafter Blackfuse. |goto Siege of Orgrimmar/10 36.4,69.8 <5 |noway |c
step
goto Siege of Orgrimmar/10 40.3,65.7
.' Kill the waves of enemies that spawn here.
|confirm
step
.' Jump onto this large pipe once all the waves of enemies have been defeated. |goto Siege of Orgrimmar/10 46.3,59.5 <5 |c
step
goto 58.7,46.5
'_Siegecrafter Blackfuse_
.' It helps if 2 teams are assigned to kill the Disassembled Weapon adds that spawn every 30 seconds. The groups will have to alternate between spawns.
.' If a Weapon add is not killed before they reach the northeastern wall, Blackfuse will gain _Energized Defensive Matrix_.
.' _Energized Defense Matrix_ reduces Blackfuse's damage taken by 90%.
.' _Matter Purification Beam_ inflicts 475,000 Fire damage.
.' _Launch Sawblade_ fires a blade at the target, knocking away all players in its path upon firing and dealing 41,000-48,000 damage every second.
.' _Electrostatic Charge_ deals 250,000 Nature damage and increases the target's damage taken from Electrostatic Charge by 100% and Reactive Armor by 200%.
.' _Reactive Armor_ reduces damage taken by 80%. Adds that have this buff should be kited through AoEs on the ground, as they still take normal damage from environmental effects.
.' _Protective Frenzy_ is a buff that increases damage by 100%, health by 250% and granting immunity to immobolize, stun, and slow effects.
.' _Automatic Repair Beam_ Blackfuse heals one of his allies for 5% of their maximum health every 3 seconds. Because of this, adds should be tanked at least 35 yards away from him.
|modelnpc Siegecrafter Blackfuse##71504
|confirm
step
.' Jump onto this large pipe |goto Siege of Orgrimmar/10 50.2,55.6 <5 |c
step
map Siege of Orgrimmar/11
path loop off
path	Siege of Orgrimmar/10 31.1,74.6	Siege of Orgrimmar/10 24.7,81.2	Siege of Orgrimmar/8 87.6,76.0
path	35.6,21.3	48.1,13.1	57.1,13.0
path	63.1,24.5	62.2,32.6	35.6,21.3
path	66.5,33.2	69.5,28.0	69.1,38.2
.' Follow the path, clearing trash as you make your way to the next boss, the Paragons of the Klaxxi. |goto 69.1,38.2 <5 |noway |c
step
goto 68.3,35.3
.' The Paragons join the fight in a certain order, as listed. Upon killing a Paragon, the others return to 100% health.
'_Rik'kal the Dissector_, _Skeer the Bloodseeker_, and _Hisek the Swarmkeeper_.
.' _Injection_ deals 40,000 Nature damage every second for 12 seconds.
.' _Mutate_ turns the target into a mantic scorpion. The transformation deals 150,000 Fire damage to all nearby players within 3 yards and an additional 50,000 Fire damage every second to the original target.
.' _Bloodletting_ causes Skeer's target to bleed, flinging globules of their blood onto the floor around the room that move to a nearby Paragon and heal them.
.' _Multi-Shot_ Hisek shoots at 3 players at once.
.' _Rapid Fire_ Hisek fires out multiple _Sonic Pulses_ that deal 300,000 Physical damage every second to all players caught in their path.
'_Ka'roz the Locust_
.' _Caustic Amber_ AoE that deals 138,000-160,000 Nature damage every second.
'_Korven the Prime_
.' _Encase in Amber_ encased one of Koven's allies in an amber shield, absorbing all damage for 10 seconds. If the shield is not destroyed in this time that ally's health is restored to 100%.
.' _Shield Bash_ is a 6 second stun, followed by a _frontal cleave_
'_Iyyokuk the Lucid_
.' _Insane Calculation: Fiery Edge_ fires a beam of fire between multiple players, dealing increased damage the closer the targets are to each other.
'_Xaril the Poisoned Mind_
.' _Caustic Blood_ deals 500,000 Nature damage every second and stacks up to 10 times.
.' _Tenderizing Strikes_ is a debuff that increases the target's damage taken from Kil'ruk the Wind-Reaver.
'_Kaz'tik the Manipulator_
.' _Mesmerize_ forces the targeted player to walk toward a nearby Hungry Kunchong.
'_Kil'ruk the Wind-Reaver_
.' _Death From Above_ deals damageto all players within 8 yards.
|modelnpc Rik'kal the Dissector##71158
|modelnpc Skeer the Bloodseeker##71152
|modelnpc Hisek the Swarmkeeper##71153
|modelnpc Ka'roz the Locust##71154
|modelnpc Korven the Prime##71155
|modelnpc Iyyokuk the Lucid##71160
|modelnpc Xaril the Poisoned Mind##71157
|modelnpc Kaz'tik the Manipulator##71156
|modelnpc Kil'ruk the Wind-Reaver##71161
|confirm
step
map Siege of Orgrimmar/11
path loop off
path	72.6,45.3	77.4,56.2	76.7,71.4
path	68.2,81.4	64.9,92.6	Siege of Orgrimmar/12 51.2,45.9
.' Follow the path, clearing trash as you make your way to the final boss, Garrosh Hellscream. |goto Siege of Orgrimmar/12 51.2,45.9 <5 |noway |c
step
goto Siege of Orgrimmar/12 50.1,60.5
'_Garrosh Hellscream_
.' An Engineer add spawns on both sides of the room after Garrosh throws a whirling axe at the raid with _Desecrate_. They cast Iron Star, which will release a large rolling wheel into the room that kills any raid members it crushes. A DPS should be assigned to kill an Engineer while the raid stays protected on the safe side of the room, free of the Iron Star wheel.
.' In between main phases the raid will get teleported to 1 of 3 possible zones. Kill the adds in the zone and rush Garrosh, continuing to damage him as much as possible.
.' _Desecrate_ Garrosh throws a whirling axe at the raid, dealing damage on impact and spawning a void zone that deals 75,000 damage every second to all players inside it. Destroying the axe will remove the void zone.
.' _Whirling Corruption_ deals 105,000 Shadow damage to nearby players every 0.5 seconds. The damage decreases the farther away you are from Garrosh.
.' _Touch of Y'Shaarj_ mind controls the targeted player, making them attack other raid members. Bringing them down to 20% health will break the mind control.
.' In Phase 3 Garrosh gains Heart of the Y'Shaarj and takes on a Sha-like appearance. The axes from _Desecrate_ can no longer be destroyed and should simply be avoided while continuing to damage Garrosh. Kill him as fast as possible to avoid death.
|modelnpc Garrosh Hellscream##71865
|confirm
step
goto Siege of Orgrimmar/12 47.8,65.7
.' Click on Fading Breah
..accept 33147
|modelnpc Fading Breath##73335
|only if not completedq(33147)
step
goto Siege of Orgrimmar/12 50.2,56.9
.talk 73318
..turnin 33147
..accept 33138
|only if not completedq(33138)
step
goto Vale of Eternal Blossoms/0 67.4,45.7
.talk 73136
..turnin 33138
|only if not completedq(33138)
step
Congratulations, you have finished the Siege of Orgrimmar raid and witnessed the downfall of Garrosh Hellscream!
]])
