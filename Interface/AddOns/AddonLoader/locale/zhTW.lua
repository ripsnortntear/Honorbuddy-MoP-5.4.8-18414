if GetLocale() ~= "zhTW" then return end
local L = AddonLoader.L
L.explain = "你可以重寫每個插件的載入條件. 重寫後需要重載界面才能應用新的條件. 現在插件列表中未載入的插件還保持原載入條件."
L.hideloading = "隱藏載入訊息"
L.reset = "重置"
L.clicktoload = "|cffeda55f點擊|r 載入。"
