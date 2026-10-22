-- MoP_GM/UI/ConfirmDialog.lua
-- Single confirm-popup used by every danger=true command.

StaticPopupDialogs["MOPGM_CONFIRM_CMD"] = {
    text = "MoP_GM — Run the following command?\n\n|cffffd100%s|r",
    button1 = YES,
    button2 = NO,
    OnAccept = function(self, data)
        local line = data or self.text and self.text.text_arg1 or self.data
        if line and line ~= "" then
            MoP_GM._ExecuteRaw(line)
        end
    end,
    OnShow = function(self, data)
        self.data = data
    end,
    timeout = 0,
    whileDead = true,
    hideOnEscape = true,
    preferredIndex = 3,
    showAlert = true,
}
