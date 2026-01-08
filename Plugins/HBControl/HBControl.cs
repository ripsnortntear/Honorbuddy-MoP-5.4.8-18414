using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;

public class HBControl : HBPlugin
{
    public override string Name { get { return "HBControl"; } }
    public override string Author { get { return "Texy"; } }
    public override Version Version { get { return new Version(1, 0); } }
    //public override bool WantButton { get { return true; } }
    public override string ButtonText { get { return "Start or Stop"; } }
    public override bool WantButton { get { return false; } }


    public static void infoLog(string message, params object[] args)
    {
        Logging.Write(Colors.SkyBlue, "[HBControl] " + message, args);
    }
    

    //public override void OnButtonPress()
    //{
    //    infoLog("The button was pressed.");
    //}

    private System.Timers.Timer checkStateTimer;

    public override void OnEnable()
    {
        base.OnEnable();
        CreateInGameButton();
        infoLog("Plugin enabled and button created.");

        checkStateTimer = new System.Timers.Timer(1000); // Check every 1 seconds
        checkStateTimer.Elapsed += CheckStateTimer_Elapsed;
        checkStateTimer.Start();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        // Code pour arrêter et nettoyer le timer si nécessaire
        if (checkStateTimer != null)
        {
            checkStateTimer.Stop();
            checkStateTimer.Elapsed -= CheckStateTimer_Elapsed;
            checkStateTimer.Dispose();
        }

        // Code Lua pour cacher le cadre et le réinitialiser
        string hideAndResetFrameLuaCode = @"
        if MyAddonFrame then 
            MyAddonFrame:Hide() 
            MyAddonFrame = nil -- Réinitialisez MyAddonFrame pour forcer sa recréation
        end";
        Lua.DoString(hideAndResetFrameLuaCode);

        infoLog("Plugin disabled, frame hidden, and MyAddonFrame reset.");
    }



    private void CreateInGameButton()
    {
        string luaCode = @"
        -- At the beginning of your Lua script, make sure the table exists.
        _MY_PLUGIN_GLOBALS = _MY_PLUGIN_GLOBALS or {}
        local frame = CreateFrame(""Frame"", ""MyAddonFrame"", UIParent)
        frame:SetSize(35, 30)
        frame:SetMovable(true)
        frame:EnableMouse(true)
        frame:SetPoint('CENTER', UIParent, 'CENTER', 100, 0)
        local texture = frame:CreateTexture(nil, ""BACKGROUND"")
        frame:SetBackdrop({
            bgFile = ""Interface\\Buttons\\WHITE8x8"",
            edgeFile = ""Interface\\DialogFrame\\UI-DialogBox-Border"",
            tile = false,
            tileSize = 0,
            edgeSize = 8,
            insets = {
            left = 5,
            right = 5,
            top = 5,
            bottom = 5
            }
        })
        frame:SetBackdropColor(0, 0, 0, 0.1)
        frame:SetBackdropBorderColor(0, 0, 0, 0.8)
        local function dragframe(self)
            self:StartMoving()
        end

        local function stopDragframe(self)
            self:StopMovingOrSizing()
        end
        frame:SetScript(""OnMouseDown"", dragframe)
        frame:SetScript(""OnMouseUp"", stopDragframe)

        local toggleButton = CreateFrame('Button', 'MyToggleButton', frame)
        toggleButton:SetSize(25, 20)
        toggleButton:SetText('Start')
        
        toggleButton:SetPoint('CENTER', frame, 'CENTER', 0, 0)
        local setFont = toggleButton:GetFontString()
        setFont:SetFont(""Fonts/FRIZQT__.TTF"", 9)
        setFont:SetPoint('CENTER', 0, 0)
        setFont:SetTextColor(1, 1, 1, 1)
        toggleButton:SetBackdrop({
            bgFile = ""Interface\\Buttons\\WHITE8x8"",
            edgeFile = ""Interface\\Buttons\\WHITE8x8"",
            tile = false,
            tileSize = 0,
            edgeSize = 1,
            insets = {
                left = 0,
                right = 0,
                top = 0,
                bottom = 0
            }
        })
        toggleButton:SetBackdropColor(0, 0, 0, 0.3)
        toggleButton:SetBackdropBorderColor(0, 0, 0, 1)
        toggleButton:SetScript(""OnEnter"", function(self)
            self:SetBackdropColor(0, 0, 0, 0.5)
        end)
        toggleButton:SetScript(""OnLeave"", function(self)
            self:SetBackdropColor(0, 0, 0, 0.1)
        end)


        toggleButton:SetScript('OnClick', function()
            _MY_PLUGIN_GLOBALS.isBotRunning = not _MY_PLUGIN_GLOBALS.isBotRunning
            if _MY_PLUGIN_GLOBALS.isBotRunning then
                toggleButton:SetText('Stop')
                print('The bot is now running')
                --setFont red
                setFont:SetTextColor(1, 0, 0, 1)
            else
                toggleButton:SetText('Start')
                print('The bot is now stopped')
                --setFont green
                setFont:SetTextColor(0, 1, 0, 1)
            end
        end)
        ";

        Lua.DoString(luaCode);
    }

    public override void Pulse()
    {

    }
    private bool? lastBotState = null;

    private void CheckStateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        bool isBotRunning = Lua.GetReturnVal<bool>("return _MY_PLUGIN_GLOBALS.isBotRunning", 0);

        // Check if the state has changed since the last check.
        if (lastBotState == null || isBotRunning != lastBotState)
        {
            try
            {
                if (isBotRunning)
                {
                    TreeRoot.Start();
                    infoLog("The bot is running.");
                }
                else
                {
                    TreeRoot.Stop();
                    infoLog("The bot is stopped.");
                }
            }
            catch (Exception ex)
            {
                infoLog("Error while trying to start/stop the bot: " + ex.Message);
            }
            lastBotState = isBotRunning; // Update the previous state.
        }
    }
}
