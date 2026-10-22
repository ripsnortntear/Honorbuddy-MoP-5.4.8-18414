-- Pawn by Vger-Azjol-Nerub
-- www.vgermods.com
-- © 2006-2013 Green Eclipse.  This mod is released under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 license.
-- See Readme.htm for more information.

-- 
-- Chinese (Traditional) resources
------------------------------------------------------------

local function PawnUseThisLocalization()
PawnLocal =
{
	AsteriskTooltipLine = "|TInterface\\AddOns\\Pawn\\Textures\\Question:0|t 特殊效果不包含在數值中.",
	AverageItemLevelIgnoringRarityTooltipLine = "平均物品等級",
	BackupCommand = "備份",
	BaseValueWord = "基礎",
	CogwheelName = "榫輪",
	CopyScaleEnterName = "為你的新比重起個名字,  %s 的備份名稱為:",
	CorrectGemsValueCalculationMessage = "   -- 正確的寶石的價值將: %g", -- Needs review
	CrystalOfFearName = "晶化恐懼",
	DebugOffCommand = "偵錯 關",
	DebugOnCommand = "偵錯 開",
	DeleteScaleConfirmation = "你確定你要刪除 %s? 這將不能復原. 確定輸入 \"%s\" :",
	DidntUnderstandMessage = "   (?) 無法識別 \"%s\".",
	EnchantedStatsHeader = "(當前值)",
	EngineeringName = "工程學",
	ExportAllScalesMessage = "Press Ctrl+C to copy your scale tags, create a file on your computer to save them in for backup, and then press Ctrl+V to paste them.",
	ExportScaleMessage = "按 Ctrl+C 為 |cffffffff%s|r 複製下面的比重標籤, 然後按 Ctrl+V 黏貼.",
	FailedToGetItemLinkMessage = "   從提示欄獲取物品連接失敗.  這可能緣於一次模組衝突.",
	FailedToGetUnenchantedItemMessage = "   獲取基本物品數值失敗.  這可能緣於一次模組衝突.",
	FoundStatMessage = "   %d %s",
	GemColorList1 = "%d %s",
	GemColorList2 = "%d %s 或 %s",
	GemColorList3 = "%d 個任何顯色",
	GenericGemLink = "|Hitem:%d|h[寶石 %d]|h",
	GenericGemName = "(寶石 %d)",
	HiddenScalesHeader = "其它比重",
	ImportScaleMessage = "按 Ctrl+V to 黏貼一個你從別處複製的比重標籤于此處:",
	ImportScaleTagErrorMessage = "Pawn 不明白這個標籤.  你的複製是完整的嗎?  嘗試重新複製一遍:",
	ItemIDTooltipLine = "物品 ID",
	ItemLevelTooltipLine = "物品等級",
	LootUpgradeAdvisorHeader = "點擊來同你的物品比較。|n",
	LootUpgradeAdvisorHeaderMany = "|TInterface\\AddOns\\Pawn\\Textures\\UpgradeArrow:0|t 對於 %d 比重來說這件物品是個提升.點擊來同你的物品進行比較.",
	MissocketWorthwhileMessage = "   -- 但是這最好只用  %s 寶石:",
	NeedNewerVgerCoreMessage = "Pawn 需要一个新版本的 VgerCore. 請使用Pawn内置的VgerCore",
	NewScaleDuplicateName = "這個名字已存在.  重新為你的比重起個名字:",
	NewScaleEnterName = "為你的比重起個名字:",
	NewScaleNoQuotes = "比重不能有 \" 在他的名字中.  重新為你的比重起個名字:",
	NormalizationMessage = "   ---- 規範化通過除以 %g",
	NoScale = "(無)",
	NoScalesDescription = "準備開始, 導入一個比重或者啟用一個新的.",
	NoStatDescription = "從左側列表選擇一個屬性.",
	Or = "或 ",
	ReforgeCappedStatWarning = "當重鑄命中或者熟練時請小心使用！不要讓你的未擊中的幾率高過0%。",
	ReforgeDebugMessage = "   ---- 通過重鑄裝備獲得 +%g",
	ReforgeInstructions = "重鑄 %s 到 %s",
	ReforgeInstructionsNoReforge = "不要重鑄",
	RenameScaleEnterName = "%s 的新名為:",
	SocketBonusValueCalculationMessage = "   -- 插槽加成是值得的:",
	StatNameText = "1 |cffffffff%s|r 價值:",
	TooltipBestAnnotation = "%s  |cff8ec3e6(最佳)|r", -- Needs review
	TooltipBestAnnotationSimple = "%s  你最好", -- Needs review
	TooltipBigUpgradeAnnotation = "%s  |TInterface\\AddOns\\Pawn\\Textures\\UpgradeArrow:0|t|cff00ff00 提升%s|r",
	TooltipSecondBestAnnotation = "%s  |cff8ec3e6(第二最佳)|r", -- Needs review
	TooltipSecondBestAnnotationSimple = "%s  您的第二最佳", -- Needs review
	TooltipUpgradeAnnotation = "%s  |TInterface\\AddOns\\Pawn\\Textures\\UpgradeArrow:0|t|cff00ff00+%.0f%% 提升%s|r",
	TooltipUpgradeFor1H = " 對於單手來說",
	TooltipUpgradeFor2H = " 對於雙手來說",
	TooltipVersusLine = "%s|n  vs. |c%s%s|r",
	TotalValueMessage = "   ---- 總計: %g",
	UnenchantedStatsHeader = "(基本值)",
	Unusable = "(不能用)",
	UnusableStatMessage = "   -- %s 為無用的, 所以停止.",
	Usage = [=[Pawn 所作的 Vger-Azjol-Nerub
www.vgermods.com
 
/pawn -- 顯示或隱藏 UI Pawn
/pawn debug [ on | off ] -- 在主控台中顯示調試消息
/pawn backup -- 備份所有您進行縮放以擴展標記]=], -- Needs review
	ValueCalculationMessage = "   %g %s x %g each = %g",
	VisibleScalesHeader = "%s的比重",
	Stats = {
		AgilityInfo = "主要屬性, 敏捷.", -- Needs review
		Ap = "攻擊強度", -- Needs review
		ApInfo = "攻擊強度. 不直接存在於大多數物品上。 不包括從力量或敏捷上獲取的攻擊強度。", -- Needs review
		ArmorInfo = "護甲，無論物品類型。不區分基礎護甲和額外護甲，因為額外護甲的項目是過時的。護甲，無論物品類型。不區分基礎護甲和額外護甲，因為額外護甲的物品是過時的。", -- Needs review
		ArmorTypes = "防禦屬性", -- Needs review
		CasterStats = "法術屬性", -- Needs review
		Cloth = "布甲", -- Needs review
		ClothInfo = "此物若為布甲，點數將被分配。", -- Needs review
		Crit = "致命一擊等級", -- Needs review
		CritInfo = "致命一擊等級。影響近戰攻擊，遠程攻擊，法術。", -- Needs review
		DodgeInfo = "防禦屬性。", -- Needs review
		DpsInfo = "每秒武器傷害。 (若你想要為不同武器的不同DPS估值, 請看 \"特定武器屬性\" 部份.)", -- Needs review
		ExpertiseInfo = "熟練等級。 使你的敵人的躲閃和招架無效的統計。", -- Needs review
		HasteInfo = "加速等級。影響近戰攻擊，遠程攻擊，法術。\"", -- Needs review
		Hit = "命中等級", -- Needs review
		HitInfo = "命中等級.  影響近戰攻擊，遠程攻擊，法術。", -- Needs review
		HybridStats = "混合等級", -- Needs review
		IntellectInfo = "主要屬性, 智力.", -- Needs review
		Leather = "皮甲", -- Needs review
		LeatherInfo = "此物若為皮甲，點數將被分配。", -- Needs review
		Mail = "鎖甲", -- Needs review
		MailInfo = "此物若為鎖甲，點數將被分配。", -- Needs review
		MasteryInfo = "精通等級。提高你主天賦的特有加成。", -- Needs review
		MetaSocket = "變換", -- Needs review
		MetaSocketInfo = "一個變換插槽, 是否空的還是滿的.  只計算一個變換寶石的附加效果, 而非屬性加成.", -- Needs review
		ParryInfo = "防禦屬性。", -- Needs review
		PhysicalStats = "物理攻擊屬性", -- Needs review
		Plate = "鎧甲", -- Needs review
		PlateInfo = "此物若為鎧甲，點數將被分配。", -- Needs review
		PrimaryStats = "基本屬性",
		PvPPower = "PvP能量", -- Needs review
		PvPPowerInfo = "PvP能量. 使你的能力，給其他玩家（但不包括生物）造成更多的傷害，並在某些PVP的情況下，你的治療法術治療其他玩家。", -- Needs review
		PvPResilience = "PvP韌性", -- Needs review
		PvPResilienceInfo = "PvP韌性. 減少您受到的損害從其他玩家的攻擊。", -- Needs review
		PvPStats = "PvP屬性", -- Needs review
		Shield = "盾牌", -- Needs review
		ShieldInfo = "此物若為盾牌，點數將被分配。", -- Needs review
		Sockets = "插槽", -- Needs review
		SpecialWeaponStats = "特定武器屬性", -- Needs review
		SpeedBaseline = "速度基線", -- Needs review
		SpeedBaselineInfo = "它並非一個實際的屬性。  此數被乘以比重值之前要扣除速度屬性。", -- Needs review
		SpeedBaselineIs = "|cffffffff速度基線|r:", -- Needs review
		SpeedInfo = "武器速度, 每秒頻率.  (如果你偏好快速武器, 此數應為負.  同樣請看: \"速度基線\" 于 \"特定武器屬性\" 部份.)", -- Needs review
		SpeedIs = "|cffffffff武器速度|r:", -- Needs review
		SpellPower = "法術強度", -- Needs review
		SpellPowerInfo = "法術強度. 存在於施法者武器上而非大多數護甲。 不包括從智力上獲取的法術強度。", -- Needs review
		SpiritInfo = "主要屬性, 精神.", -- Needs review
		StaminaInfo = "主要屬性, 耐力.", -- Needs review
		StrengthInfo = "主要屬性, 力量.", -- Needs review
		TankStats = "防禦屬性", -- Needs review
		WeaponMainHandDps = "主手: DPS", -- Needs review
		WeaponMainHandDpsInfo = "武器每秒傷害，只針對於主手武器。", -- Needs review
		WeaponMainHandMaxDamage = "主手: 最大傷害", -- Needs review
		WeaponMainHandMaxDamageInfo = "武器最大傷害，只針對於主手武器。", -- Needs review
		WeaponMainHandMinDamage = "主手: 最小傷害", -- Needs review
		WeaponMainHandMinDamageInfo = "武器最小傷害，只針對於主手武器。", -- Needs review
		WeaponMainHandSpeed = "主手: 武器速度", -- Needs review
		WeaponMainHandSpeedInfo = "武器速度，只針對於主手武器。", -- Needs review
		WeaponMaxDamage = "最大傷害", -- Needs review
		WeaponMaxDamageInfo = "武器最大傷害。", -- Needs review
		WeaponMeleeDps = "近戰: DPS", -- Needs review
		WeaponMeleeDpsInfo = "武器每秒傷害，只針對於近戰武器。", -- Needs review
		WeaponMeleeMaxDamage = "近戰: 最大傷害", -- Needs review
		WeaponMeleeMaxDamageInfo = "武器最大傷害，只針對於近戰武器。", -- Needs review
		WeaponMeleeMinDamage = "近戰: 最小傷害", -- Needs review
		WeaponMeleeMinDamageInfo = "武器最小傷害，只針對於近戰武器。", -- Needs review
		WeaponMeleeSpeed = "近戰: 武器速度", -- Needs review
		WeaponMeleeSpeedInfo = "武器速度，只針對於近戰武器。", -- Needs review
		WeaponMinDamage = "最小傷害", -- Needs review
		WeaponMinDamageInfo = "武器最小傷害。", -- Needs review
		WeaponOffHandDps = "副手: DPS", -- Needs review
		WeaponOffHandDpsInfo = "武器每秒傷害，只針對於副手武器。", -- Needs review
		WeaponOffHandMaxDamage = "副手: 最大傷害", -- Needs review
		WeaponOffHandMaxDamageInfo = "武器最大傷害，只針對於副手武器。", -- Needs review
		WeaponOffHandMinDamage = "副手: 最小傷害", -- Needs review
		WeaponOffHandMinDamageInfo = "武器最小傷害，只針對於副手武器。", -- Needs review
		WeaponOffHandSpeed = "副手: 武器速度", -- Needs review
		WeaponOffHandSpeedInfo = "武器速度，只針對於副手武器。", -- Needs review
		WeaponOneHandDps = "單手: DPS", -- Needs review
		WeaponOneHandDpsInfo = "武器每秒傷害，只針對於標記為單手的武器，不包括主手或副手武器。", -- Needs review
		WeaponOneHandMaxDamage = "單手: 最大傷害", -- Needs review
		WeaponOneHandMaxDamageInfo = "武器最大傷害，只針對於標記為單手的武器，不包括主手或副手武器。", -- Needs review
		WeaponOneHandMinDamage = "單手: 最小傷害", -- Needs review
		WeaponOneHandMinDamageInfo = "武器最小傷害，只針對於標記為單手的武器，不包括主手或副手武器。", -- Needs review
		WeaponOneHandSpeed = "單手: 武器速度", -- Needs review
		WeaponOneHandSpeedInfo = "武器速度，只針對於標記為單手的武器，不包括主手或副手武器。", -- Needs review
		WeaponRangedDps = "遠程: DPS", -- Needs review
		WeaponRangedDpsInfo = "武器每秒傷害，只針對於遠程武器。", -- Needs review
		WeaponRangedMaxDamage = "遠程: 最大傷害", -- Needs review
		WeaponRangedMaxDamageInfo = "武器最大傷害，只針對於遠程武器。", -- Needs review
		WeaponRangedMinDamage = "遠程: 最小傷害", -- Needs review
		WeaponRangedMinDamageInfo = "武器最小傷害，只針對於遠程武器。", -- Needs review
		WeaponRangedSpeed = "遠程: 武器速度", -- Needs review
		WeaponRangedSpeedInfo = "武器速度，只針對於遠程武器。", -- Needs review
		WeaponStats = "武器屬性", -- Needs review
		WeaponTwoHandDps = "雙手: DPS", -- Needs review
		WeaponTwoHandDpsInfo = "武器每秒傷害，只針對於雙手武器。", -- Needs review
		WeaponTwoHandMaxDamage = "雙手: 最大傷害", -- Needs review
		WeaponTwoHandMaxDamageInfo = "武器最大傷害，只針對於雙手武器。", -- Needs review
		WeaponTwoHandMinDamage = "雙手: 最小傷害", -- Needs review
		WeaponTwoHandMinDamageInfo = "武器最小傷害，只針對於雙手武器。", -- Needs review
		WeaponTwoHandSpeed = "雙手: 武器速度", -- Needs review
		WeaponTwoHandSpeedInfo = "武器速度，只針對於雙手武器。", -- Needs review
		WeaponType1HAxe = "單手斧", -- Needs review
		WeaponType1HAxeInfo = "此物若為單手斧，點數將被分配。", -- Needs review
		WeaponType1HMace = "單手錘", -- Needs review
		WeaponType1HMaceInfo = "此物若為單手錘，點數將被分配。", -- Needs review
		WeaponType1HSword = "單手劍", -- Needs review
		WeaponType1HSwordInfo = "此物若為單手劍，點數將被分配。", -- Needs review
		WeaponType2HAxe = "雙手斧", -- Needs review
		WeaponType2HAxeInfo = "此物若為雙手斧，點數將被分配。", -- Needs review
		WeaponType2HMace = "雙手錘", -- Needs review
		WeaponType2HMaceInfo = "此物若為雙手錘，點數將被分配。", -- Needs review
		WeaponType2HSword = "雙手劍", -- Needs review
		WeaponType2HSwordInfo = "此物若為雙手劍，點數將被分配。", -- Needs review
		WeaponTypeBow = "弓", -- Needs review
		WeaponTypeBowInfo = "此物若為弓，點數將被分配。", -- Needs review
		WeaponTypeCrossbow = "弩", -- Needs review
		WeaponTypeCrossbowInfo = "此物若為弩，點數將被分配。", -- Needs review
		WeaponTypeDagger = "匕首", -- Needs review
		WeaponTypeDaggerInfo = "此物若為匕首，點數將被分配。", -- Needs review
		WeaponTypeFistWeapon = "拳套", -- Needs review
		WeaponTypeFistWeaponInfo = "此物若為拳套，點數將被分配。", -- Needs review
		WeaponTypeFrill = "副手飾物", -- Needs review
		WeaponTypeFrillInfo = "此物若為\"拿在副手\"的施法副手裝備，點數將被分配。不能為盾牌或者武器。", -- Needs review
		WeaponTypeGun = "槍械", -- Needs review
		WeaponTypeGunInfo = "此物若為槍械，點數將被分配。", -- Needs review
		WeaponTypeOffHand = "副手武器", -- Needs review
		WeaponTypeOffHandInfo = "此物若為只能裝備在副手的武器時，點數將被分配。不能為副手\"飾物\"(施法)物品，盾牌或者兩隻手都可那的武器。", -- Needs review
		WeaponTypePolearm = "長柄武器", -- Needs review
		WeaponTypePolearmInfo = "此物若為長柄武器，點數將被分配。", -- Needs review
		WeaponTypes = "武器類型", -- Needs review
		WeaponTypeStaff = "法杖", -- Needs review
		WeaponTypeStaffInfo = "此物若為法杖，點數將被分配。", -- Needs review
		WeaponTypeWand = "魔杖", -- Needs review
		WeaponTypeWandInfo = "此物若為魔杖，點數將被分配。", -- Needs review
	},
	TooltipParsing = {
		Agility = "^%+?([-%d%.,]+)敏捷$", -- Needs review
		AllStats = "^%+?([%d%.,]+)到所?有屬性$", -- Needs review
		Ap = "^%+?([%d%.,]+) 攻擊強度$", -- Needs review
		Armor = "^([%d%.,]+)點?護甲$", -- Needs review
		Armor2 = "^裝備: %+([%d%.,]+)點護甲值。$", -- Needs review
		Axe = "^斧$", -- Needs review
		BagSlots = "^%d+格容器$", -- Needs review
		BladesEdgeMountains = "^劍刃山脈$", -- Needs review
		Bow = "^弓$", -- Needs review
		ChanceOnHit = "擊中時可能: ", -- Needs review
		Charges = "^.+ Charges?$", -- Needs review
		Cloth = "^布甲$", -- Needs review
		CooldownRemaining = "^冷卻時間:", -- Needs review
		Crit = "^%+?([%d%.,]+)致命一擊等級$", -- Needs review
		Crit2 = "^裝備: 提高([%d%.,]+)點致命一擊等級。$", -- Needs review
		Crossbow = "^弩$", -- Needs review
		Dagger = "^匕首$", -- Needs review
		Design = "設計圖:", -- Needs review
		DisenchantingRequires = "^分解需要", -- Needs review
		Dodge = "^%+?([%d%.,]+)閃躲等級$", -- Needs review
		Dodge2 = "^裝備: 提高([%d%.,]+)點閃躲等級。$", -- Needs review
		Dps = "^%(([%d%.,]+)每秒傷害%)$", -- Needs review
		DpsAdd = "^增加 ([%d%.,]+)每秒傷害$", -- Needs review
		Duration = "^持續:", -- Needs review
		Elite = "^精英$", -- Needs review
		EnchantmentArmorKit = "^強化%(%+([%d%.,]+)護甲%)$", -- Needs review
		EnchantmentCounterweight = "^平衡比重%(%+([%d%.,]+)加速等級%)", -- Needs review
		EnchantmentFieryWeapon = "^灼熱武器$", -- Needs review
		EnchantmentHealth = "^%+([%d%.,]+)生命力?$", -- Needs review
		EnchantmentHealth2 = "^UNUSED$", -- Needs review
		EnchantmentLivingSteelWeaponChain = "^活化鋼武器鍊$",
		EnchantmentPyriumWeaponChain = "^黃鐵武器鍊$",
		EnchantmentTitaniumWeaponChain = "^泰坦鋼武器鍊$", -- Needs review
		Equip = "裝備: ", -- Needs review
		Expertise = "^%+?([%d%.,]+)熟練等級$", -- Needs review
		Expertise2 = "^裝備: 使你的熟練等級提高([%d%.,]+)點。$", -- Needs review
		FistWeapon = "^拳套$", -- Needs review
		Flexible = "^Flexible$",
		Formula = "公式: ", -- Needs review
		Gun = "^槍械$", -- Needs review
		Haste = "^%+?([%d%.,]+)加速等級$", -- Needs review
		Haste2 = "^裝備: 提高([%d%.,]+)點加速等級。$", -- Needs review
		HeirloomLevelRange = "^需要等級 %d+ 到 (%d+)", -- Needs review
		HeirloomXpBoost = "^裝備: Experience gained", -- Needs review
		HeirloomXpBoost2 = "^UNUSED$", -- Needs review
		Heroic = "^英雄模式$", -- Needs review
		HeroicElite = "^英雄難度精英$", -- Needs review
		HeroicThunderforged = "^Heroic Thunderforged$", -- Requires localization
		HeroicWarforged = "^Heroic Warforged$",
		Hit = "^%+?([%d%.,]+)命中等級$", -- Needs review
		Hit2 = "^裝備: 提高([%d%.,]+)點命中等級。$", -- Needs review
		Hit3 = "^UNUSED$", -- Needs review
		Hp5 = "^裝備: 每5秒恢復([%d%.,]+)生命力。$", -- Needs review
		Hp52 = "^每5秒恢復([%d%.,]+)生命力。?$", -- Needs review
		Hp53 = "^UNUSED$", -- Needs review
		Hp54 = "^UNUSED$", -- Needs review
		Intellect = "^%+?([-%d%.,]+)智力$", -- Needs review
		Leather = "^皮甲$", -- Needs review
		Mace = "^錘$", -- Needs review
		Mail = "^鎖甲$", -- Needs review
		Manual = "手冊: ", -- Needs review
		Mastery = "^%+?([%d%.,]+)精通等級$", -- Needs review
		Mastery2 = "^裝備: 提高([%d%.,]+)點精通等級。$", -- Needs review
		MetaGemRequirements = "|cff%x%x%x%x%x%x需求", -- Needs review
		MultiStatSeparator1 = "and", -- Needs review
		NormalizationEnchant = "^Enchanted: (.*)$", -- Needs review
		NormalizationReforge = "^(.*) %(Reforged from (.*)%)$", -- Needs review
		OnlyFitsInMetaGemSlot = "^\"只適用於變換寶石插槽。\"$", -- Needs review
		Parry = "^%+?([%d%.,]+)招架等級$", -- Needs review
		Parry2 = "^裝備: 提高([%d%.,]+)點招架等級。$", -- Needs review
		Pattern = "圖樣: ", -- Needs review
		Plans = "結構圖: ", -- Needs review
		Plate = "^鎧甲$", -- Needs review
		Polearm = "^長柄武器$", -- Needs review
		PvPPower = "^%+?([%d%.,]+)PvP能量$", -- Needs review
		RaidFinder = "團隊搜尋器",
		Recipe = "配方: ", -- Needs review
		Requires2 = "^UNUSED$", -- Needs review
		Resilience = "^%+?([%d%.,]+)韌性等級$", -- Needs review
		Resilience2 = "^裝備: 提高([%d%.,]+)點韌性等級。$", -- Needs review
		Schematic = "圖鑒: ", -- Needs review
		Scope = "^瞄準鏡%(%+([%d%.,]+)傷害%)$", -- Needs review
		ScopeCrit = "^瞄準鏡%(%+([%d%.,]+)致命一擊等級%)$", -- Needs review
		ScopeRangedCrit = "^%+?([%d%.,]+)遠程致命一擊等級$", -- Needs review
		Season = "^季節", -- Needs review
		ShadowmoonValley = "^影月谷$", -- Needs review
		Shield = "^盾牌$", -- Needs review
		SocketBonusPrefix = "插槽獎勵: ", -- Needs review
		Speed = "^速度([%d%.,]+)$", -- Needs review
		Speed2 = "^UNUSED$", -- Needs review
		SpellPower = "^%+?([%d%.,]+)法術能量$", -- Needs review
		Spirit = "^%+?([-%d%.,]+)精神$", -- Needs review
		Staff = "^法杖$", -- Needs review
		Stamina = "^%+?([-%d%.,]+)耐力$", -- Needs review
		Strength = "^%+?([-%d%.,]+)力量$", -- Needs review
		Sword = "^劍$", -- Needs review
		TempestKeep = "^風暴要塞$", -- Needs review
		TemporaryBuffMinutes = "^.+%(%d+ 分%)$", -- Needs review
		TemporaryBuffSeconds = "^.+%(%d+ 秒%)$", -- Needs review
		Thunderforged = "^Thunderforged$", -- Requires localization
		Timeless = "^Timeless$",
		UpgradeLevel = "^Upgrade Level:", -- Needs review
		Use = "使用: ", -- Needs review
		Wand = "^魔杖$", -- Needs review
		Warforged = "^Warforged$",
		WeaponDamage = "^([%d%.,]+) %- ([%d%.,]+) 傷害$", -- Needs review
		WeaponDamageArcane = "^%+?([%d%.,]+)%-([%d%.,]+)秘法傷害$", -- Needs review
		WeaponDamageEnchantment = "^%+?([%d%.,]+)武器傷害$", -- Needs review
		WeaponDamageEquip = "^裝備: %+?([%d%.,]+)點武器傷害。$", -- Needs review
		WeaponDamageExact = "^%+?([%d%.,]+)傷害$", -- Needs review
		WeaponDamageFire = "^%+?([%d%.,]+)%-([%d%.,]+)火焰傷害$", -- Needs review
		WeaponDamageFrost = "^%+?([%d%.,]+)%-([%d%.,]+)冰霜傷害$", -- Needs review
		WeaponDamageHoly = "^%+?([%d%.,]+)%-([%d%.,]+)神聖傷害$", -- Needs review
		WeaponDamageNature = "^%+?([%d%.,]+)%-([%d%.,]+)自然傷害$", -- Needs review
		WeaponDamageShadow = "^%+?([%d%.,]+)%-([%d%.,]+)暗影傷害$", -- Needs review
	},
	UI = {
		AboutHeader = "關於 Pawn", -- Needs review
		AboutReadme = "新到 Pawn 嗎？請參閱入門基本介紹的選項卡。", -- Needs review
		AboutTab = "關於", -- Needs review
		AboutTranslation = "正體中文：BNS(世界之樹-三皈依)", -- Needs review
		AboutVersion = "版本 %s", -- Needs review
		AboutWebsite = [=[由 Vger 其他載入項，請訪問 vgermods.com。

Wowhead stat 權數來與許可權 — — 請直接回饋 Wowhead 的預設縮放值。]=], -- Needs review
		CompareClearItems = "清除", -- Needs review
		CompareClearItemsTooltip = "移除兩件對照的物品.", -- Needs review
		CompareCogwheelSockets = "綬通訊端", -- Needs review
		CompareColoredSockets = "彩色通訊端", -- Needs review
		CompareEquipped = "已裝備", -- Needs review
		CompareGemTotalValue = "寶石的價值", -- Needs review
		CompareHeader = "用 %s 進行對比", -- Needs review
		CompareMetaSockets = "通訊端元", -- Needs review
		CompareOtherHeader = "其他", -- Needs review
		CompareReforgingPotential = "重鑄潛力", -- Needs review
		CompareShaTouchedSockets = "煞化",
		CompareSlotEmpty = "(空)", -- Needs review
		CompareSocketBonus = "插槽獎勵", -- Needs review
		CompareSocketsHeader = "通訊端", -- Needs review
		CompareSpecialEffects = "特殊效果", -- Needs review
		CompareSwap = "‹ 對調 ›", -- Needs review
		CompareSwapTooltip = "將兩邊物品對調.", -- Needs review
		CompareTab = "比較", -- Needs review
		CompareVersus = "—vs.—", -- Needs review
		CompareWelcomeLeft = "首先, 從列表左邊選擇一個比重.", -- Needs review
		CompareWelcomeRight = [=[然後, 將一個物品放入此欄.

Pawn 會將它與你裝備的物品進行對比.]=], -- Needs review
		CompareYourBest = "最好的槽", -- Needs review
		GemsColorHeader = "%s 寶石", -- Needs review
		GemsHeader = "為 %s 鑲嵌寶石", -- Needs review
		GemsNoneFound = "沒找到寶石.", -- Needs review
		GemsQualityLevel = "有色寶石", -- Needs review
		GemsQualityLevelTooltip = [=[要建議寶石項的水準。

例如，如果"463"，則 Pawn 將顯示適合 463 級別的專案中使用的寶石: Mists of Pandaria 英雄地牢戰利品。]=], -- Needs review
		GemsShowBest = "顯示可用的最佳寶石", -- Needs review
		GemsShowBestTooltip = "顯示可用於當前所選的規模絕對最佳寶石。這些寶石的一些將會太過強大，對通訊端到老年人和低品質的專案。", -- Needs review
		GemsShowForItemLevel = "顯示寶石級別的一個專案的建議：", -- Needs review
		GemsShowForItemLevelTooltip = "顯示當前選定的規模和專案的具體級別的建議，Pawn 寶石。", -- Needs review
		GemsTab = "珠寶", -- Needs review
		GemsWelcome = "選擇規模，Pawn 建議在左邊看到寶石。", -- Needs review
		HelpHeader = "歡迎使用 Pawn!", -- Needs review
		HelpTab = "準備開始", -- Needs review
		HelpText = [=[Pawn 通過計算分數來幫你更加簡單的選出哪個物品更適合你。 這些分數顯示在你所有物品提示欄的底部。

|cff8ec3e6嘗試這些功能，一旦你掌握基礎:|r
 • 用 Pawn 的比較頁面來比較兩件物品的屬性。
 • 用在一件物品鏈接窗口上點擊右鍵來查看它比你當前的物品如何。
 • 用Shift加右鍵點擊一件有插槽的物品，可讓 Pawn 為它提供寶石相關的建議。
 • 在比重頁面上為你的一個比重做備份，並在編輯頁面上自定義屬性值。
 • 查看讀我文檔體會更多 Pawn 的高級功能相關。]=], -- Needs review
		InterfaceOptionsBody = "點擊 Pawn 按鈕到達此處。  你也可從插件頁面打開 Pawn ，或者為它綁定一個按鍵。", -- Needs review
		InterfaceOptionsWelcome = "Pawn 設置位於 Pawn 介面處.", -- Needs review
		InventoryButtonTooltip = "點擊顯示Pawn介面.", -- Needs review
		InventoryButtonTotalsHeader = "所有已裝備物品之總計:", -- Needs review
		KeyBindingCompareItemLeft = "比較物品 (左)", -- Needs review
		KeyBindingCompareItemRight = "比較物品 (右)", -- Needs review
		KeyBindingShowUI = "顯示 Pawn 介面", -- Needs review
		OptionsAdvisorHeader = "建議選項", -- Needs review
		OptionsAlignRight = "於右側", -- Needs review
		OptionsAlignRightTooltip = "在角色屬性面板右下側顯示 Pawn 按鍵.", -- Needs review
		OptionsBlankLine = "數值前加一個空白行", -- Needs review
		OptionsBlankLineTooltip = "Keep your item tooltips extra tidy by enabling this option, which adds a blank line before the Pawn values.", -- Needs review
		OptionsButtonHidden = "隱藏", -- Needs review
		OptionsButtonHiddenTooltip = "不在角色屬性面板顯示.", -- Needs review
		OptionsButtonPosition = "顯示Pawn按鍵:", -- Needs review
		OptionsButtonPositionLeft = "於左側", -- Needs review
		OptionsButtonPositionLeftTooltip = "在角色屬性面板左下側顯示 Pawn 按鍵.", -- Needs review
		OptionsButtonPositionRight = "於右側", -- Needs review
		OptionsButtonPositionRightTooltip = "在角色屬性面板右下側顯示 Pawn 按鍵.", -- Needs review
		OptionsColorBorder = "可升級的著色提示外框", -- Needs review
		OptionsColorBorderTooltip = "啟用此選項以更改升級到綠色的項的工具提示邊框的顏色。如果它干擾其他載入項更改工具提示邊框，請禁用此選項。", -- Needs review
		OptionsCurrentValue = "同時顯示目前與基礎的數值", -- Needs review
		OptionsCurrentValueTooltip = [=[啟用此選項，使 Pawn 顯示專案的兩個值： 當前值，反映了實際的寶石，附魔，項的目前狀態，再造項具有目前，與空通訊端提供沒有任何好處，除了基數的值，通常是什麼 Pawn 顯示。將的基礎值之前顯示的當前值。此選項有沒有影響，除非您打開專案值顯示的工具提示。

您還是應該使用的基準值用於確定在尾盤，兩個專案之間，但在調配時，很有用的當前值和為了使它更易於決定它是否值得立即裝備一個新的專案，在它之前有寶石或附魔。]=], -- Needs review
		OptionsDebug = "顯示偵錯訊息", -- Needs review
		OptionsDebugTooltip = [=[如果您不確定如何 Pawn 計算某個特定的項的值，啟用此選項，使垃圾郵件各種 '有用' 資料到聊天主控台，每當您將滑鼠懸停在某個專案的 Pawn。此資訊包括哪些統計資訊，Pawn 認為專案存在，專案 Pawn 的哪些部分不懂，和如何為每個您秤花每個考慮。

此選項將迅速填滿你的聊天記錄，因此你想要將它關閉，一旦你完成調查。

快捷方式：
/pawn debug on
/pawn debug off]=], -- Needs review
		OptionsHeader = "調整 Pawn 設置", -- Needs review
		OptionsInventoryIcon = "顯示清單圖標", -- Needs review
		OptionsInventoryIconTooltip = "啟用此選項以顯示庫存圖示旁邊專案連結視窗。", -- Needs review
		OptionsItemIDs = "顯示物品ID", -- Needs review
		OptionsItemIDsTooltip = [=[啟用此選項，使 Pawn 顯示你碰到的每個專案的專案 ID 以及所有附魔和寶石的 ID。

World of Warcraft 中的每個項都有與它關聯 ID 號。通常，此資訊才有用到載入項作者。]=], -- Needs review
		OptionsLootAdvisor = "顯示裝備掉落升級建議", -- Needs review
		OptionsLootAdvisorTooltip = "當戰利品滴眼液在地牢中，並且它是你的性格的升級時，Pawn 將顯示附加到告訴您有關升級的戰利品輥框彈出式功能表。", -- Needs review
		OptionsOtherHeader = "其它選項", -- Needs review
		OptionsQuestUpgradeAdvisor = "顯示任務獎勵升級建議", -- Needs review
		OptionsQuestUpgradeAdvisorTooltip = "在你尋求日誌和時交談 NPC，如果一個追求回報的選擇是升級您當前的齒輪，Pawn 將顯示一個綠色的箭頭圖示，在該專案上。如果升級的任何專案，Pawn 將顯示一堆硬幣上是值得最大時賣給供應商的專案。", -- Needs review
		OptionsReforgingAdvisor = "顯示重鑄建議", -- Needs review
		OptionsReforgingAdvisorTooltip = "探訪神秘的再造時, 典當將顯示一個快顯視窗，這表明哪些統計資訊，若要更改專案的功率最大限度地。", -- Needs review
		OptionsResetUpgrades = "重新掃描裝備", -- Needs review
		OptionsResetUpgradesTooltip = [=[Pawn 會忘記其瞭解的最佳專案你過以往任何時候都配備和提供更多的最新升級資訊在將來重新掃描您的齒輪。

如果您發現該 Pawn 使窮人升級建議由於您已經出售，被摧毀，或否則請勿再使用的專案，請使用此功能。這會影響您使用 Pawn 的字元的所有。]=], -- Needs review
		OptionsSocketingAdvisor = "顯示插槽建議", -- Needs review
		OptionsSocketingAdvisorTooltip = "當添加到專案中的寶石，Pawn 將顯示一個快顯視窗，這表明您可以添加到項中，將最大化其力量的寶石。（每種顏色的寶石建議的完整清單，請參見寶石選項卡，還可以在自訂的寶石要使用品質。）", -- Needs review
		OptionsTab = "設置", -- Needs review
		OptionsTooltipHeader = "提示欄設置", -- Needs review
		OptionsTooltipUpgradesOnly = "只顯示升級", -- Needs review
		OptionsTooltipUpgradesOnlyTooltip = "這是最簡單的選項。只顯示項的升級到您當前的齒輪，升級的百分比，並說明哪些專案是你自己的每個比額的最佳專案。不顯示任何東西在所有為較小的專案。", -- Needs review
		OptionsTooltipValuesAndUpgrades = "顯示比重數值和升級%", -- Needs review
		OptionsTooltipValuesAndUpgradesTooltip = "顯示您的所有物品上的所有比重Pawn值，除了那些值為零的。此外，指示哪些物品值得升級相對於您目前的裝備。", -- Needs review
		OptionsTooltipValuesOnly = "只顯示比重數值,不顯示升級 %", -- Needs review
		OptionsTooltipValuesOnlyTooltip = "所有專案，除了那些具有零值的顯示所有您可見秤 Pawn 值。不要顯示哪些專案是升級到您當前的齒輪。此選項反映了較舊版本的 Pawn 的預設行為。", -- Needs review
		OptionsUpgradeHeader = "在提示欄顯示 |TInterface\\AddOns\\Pawn\\Textures\\UpgradeArrow:0|t 升級:", -- Needs review
		OptionsUpgradesForBothWeaponTypes = "同時顯示單手與雙手武器的升級建議", -- Needs review
		OptionsUpgradesForBothWeaponTypesTooltip = [=[Pawn 的升級顧問應跟蹤和顯示您的雙手武器的升級和你揮動雙 （或腳輪、 主手和手關閉擺闊） 武器分開設置。

如果選中此選項，您可以使用雙手武器和仍然看到作為升級的顯然處於弱勢的單手武器，如果他們比你有的以前最好 （或第二最佳） 單手武器更好，因為 Pawn 是分別為兩個武器集跟蹤升級。

如果不選中，裝備雙手武器將會阻止 Pawn 顯示您升級為單手項，反之亦然。]=], -- Needs review
		OptionsWelcome = "按照你的偏好配置 Pawn.  更改會馬上生效.", -- Needs review
		ReforgingAdvisorHeader = "Pawn 重鑄建議:", -- Needs review
		ScaleChangeColor = "調整顏色", -- Needs review
		ScaleChangeColorTooltip = "調整物品提示欄中比重名稱和數值的顏色.", -- Needs review
		ScaleCopy = "複製", -- Needs review
		ScaleCopyTooltip = "創建一個新比重從此比重複製而來.", -- Needs review
		ScaleDefaults = "預設", -- Needs review
		ScaleDefaultsTooltip = "創建一個新比重從預設比重複製過來.", -- Needs review
		ScaleDeleteTooltip = [=[將這個比重刪除.

這個命令不能被撤銷!]=], -- Needs review
		ScaleEmpty = "創建", -- Needs review
		ScaleEmptyTooltip = "創建一個新比重從零開始", -- Needs review
		ScaleExport = "導出", -- Needs review
		ScaleExportTooltip = "在網路上同別人分享你的比重.", -- Needs review
		ScaleHeader = "管理你的 Pawn 比重", -- Needs review
		ScaleImport = "導入", -- Needs review
		ScaleImportTooltip = "通過網路黏貼一個比重標籤，來添加一個新比重.", -- Needs review
		ScaleNewHeader = "創建一個新比重", -- Needs review
		ScaleRename = "重命名", -- Needs review
		ScaleRenameTooltip = "為這個比重重命名.", -- Needs review
		ScaleSelectorHeader = "選擇一個比重:", -- Needs review
		ScaleSelectorShowScale = "在提示欄中顯示比重", -- Needs review
		ScaleSelectorShowScaleTooltip = "當這個選項被選中時，此比重值將顯示在此角色的物品工具提示上。每個比重可以顯示在一個角色，多個角色，或沒有角色。", -- Needs review
		ScaleShareHeader = "共享你的比重", -- Needs review
		ScaleTab = "比重", -- Needs review
		ScaleTypeNormal = "你可以在編輯頁面中調整這個比重.", -- Needs review
		ScaleTypeReadOnly = "若要自定義這個比重，你應該先為它做個備份.", -- Needs review
		ScaleWelcome = "比重是設定屬性和價值，這是用來分配物品的EP值。你可以定制你自己的或別人已經創造的比重值。", -- Needs review
		SocketingAdvisorButtonTooltip = "點擊顯示Pawn珠寶介面, 此處你可看到 Pawn 為每種比重推薦的寶石,和更換至更高或更低品質的寶石.", -- Needs review
		SocketingAdvisorHeader = "Pawn 插槽建議:", -- Needs review
		ValuesDoNotShowUpgradesFor1H = "請不要顯示單手裝備的提升", -- Needs review
		ValuesDoNotShowUpgradesFor2H = "請不要顯示雙手裝備的提升", -- Needs review
		ValuesDoNotShowUpgradesTooltip = "啟用該選項來隱藏此類型裝備的提升。比如，雖然坦騎可以用雙手武器，但是對坦騎設定來說一件雙手武器從來都不是一個\\\"提升\\\"，因此Pawn將不會為它們顯示提升信息。同樣，懲戒騎可以使用單手武器，但它們從不是一個提升。", -- Needs review
		ValuesFollowSpecialization = "只有顯示50等以後我最佳護甲類型的提升", -- Needs review
		ValuesFollowSpecializationTooltip = "啟用該選項來隱藏50等以後非職業專精的護甲類型。比如，神聖聖騎士在50等學到了鎧甲專精, 當只裝備鎧甲時增加智力5%。當此選項選擇時Pawn將不會考慮布、皮及鎖甲對50+神聖聖騎士的提升", -- Needs review
		ValuesHeader = "調整 %s 比重", -- Needs review
		ValuesIgnoreStat = "帶此屬性的物品沒有用處。", -- Needs review
		ValuesIgnoreStatTooltip = "啟用這個選項造成任何物品與這個屬性沒有得到這個比重的值。 例如, 薩滿不能裝備鎧甲, 所以為薩滿設計的比重值可以標記鎧甲為不可用的, 所以鎧甲的護甲不能從這比重中得到一個值。", -- Needs review
		ValuesNormalize = "校正數值 (比如 Wowhead)", -- Needs review
		ValuesNormalizeTooltip = "啟用此選項將劃分專案由你的尺度，在所有統計值的總和的最後計算的值，像 Wowhead 和 Lootzor 一樣。這有助於甚至出像其中一個規模具有統計資訊值約 1 的情況，另有價值約 5。它還有助於保持精華較小的數位。", -- Needs review
		ValuesRemove = "移除", -- Needs review
		ValuesRemoveTooltip = "從比重中移除此屬性。", -- Needs review
		ValuesTab = "數值", -- Needs review
		ValuesWelcome = "你可以為該比重自定義分配給每項屬性的數值.  若要管理你的那些比重和增加新比重,請用比重頁面.", -- Needs review
		ValuesWelcomeNoScales = "你還未選擇比重. 若要啟動,請去比重頁面并啟動一個新比重或者從網上粘貼一個.", -- Needs review
		ValuesWelcomeReadOnly = "已被選擇的該比重不能被更改.若你想改變這些數值,請去比重頁面并生成一份該比重的備份或者啟動一個新比重.", -- Needs review
	},
	Wowhead = {
		DeathKnightBloodTank = "死騎: 血", -- Needs review
		DeathKnightFrostDps = "死騎: 冰", -- Needs review
		DeathKnightUnholyDps = "死騎: 邪惡", -- Needs review
		DruidBalance = "德魯伊: 平衡", -- Needs review
		DruidFeralDps = "德魯伊: 貓", -- Needs review
		DruidFeralTank = "德魯伊: 熊", -- Needs review
		DruidRestoration = "德魯伊: 恢復", -- Needs review
		HunterBeastMastery = "獵人: 獸王", -- Needs review
		HunterMarksman = "獵人: 射擊", -- Needs review
		HunterSurvival = "獵人: 生存", -- Needs review
		MageArcane = "法師: 奧", -- Needs review
		MageFire = "法師: 火", -- Needs review
		MageFrost = "法師: 冰", -- Needs review
		MonkBrewmaster = "武僧: 釀酒", -- Needs review
		MonkMistweaver = "武僧: 織霧", -- Needs review
		MonkWindwalker = "武僧: 御風", -- Needs review
		PaladinHoly = "聖騎: 神聖", -- Needs review
		PaladinRetribution = "聖騎: 懲戒", -- Needs review
		PaladinTank = "聖騎: 坦克", -- Needs review
		PriestDiscipline = "牧師: 戒律", -- Needs review
		PriestHoly = "牧師: 神聖", -- Needs review
		PriestShadow = "牧師: 暗影", -- Needs review
		Provider = "Wowhead 比重", -- Needs review
		RogueAssassination = "盜賊: 刺殺", -- Needs review
		RogueCombat = "盜賊: 戰鬥", -- Needs review
		RogueSubtlety = "盜賊: 敏銳", -- Needs review
		ShamanElemental = "薩滿: 元素", -- Needs review
		ShamanEnhancement = "薩滿: 增強", -- Needs review
		ShamanRestoration = "薩滿: 恢復", -- Needs review
		WarlockAffliction = "術士: 痛苦", -- Needs review
		WarlockDemonology = "術士: 惡魔", -- Needs review
		WarlockDestruction = "術士: 毀滅", -- Needs review
		WarriorArms = "戰士: 武器", -- Needs review
		WarriorFury = "戰士: 狂暴", -- Needs review
		WarriorTank = "戰士: 坦克", -- Needs review
	},
}
end

if GetLocale() == "zhTW" then
	PawnUseThisLocalization()
end

-- After using this localization or deciding that we don't need it, remove it from memory.
PawnUseThisLocalization = nil
