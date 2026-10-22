if GetLocale() ~= "zhCN" then return end
local L = AddonLoader.L
L.explain = "你可以重写每个插件的载入条件. 重写后需要重载界面才能应用新的条件. 现在插件列表中未载入的插件还保持原载入条件."
L.hideloading = "隐藏聊天框反馈"
L.reset = "重置"
L.clicktoload = "|cffeda55f点击|r 载入."
