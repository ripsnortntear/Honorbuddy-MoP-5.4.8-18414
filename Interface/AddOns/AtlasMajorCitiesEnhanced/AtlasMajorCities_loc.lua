AtlasMajorCities_Strings = {

Atlas_Category = "Major Cities";

-- key bindings
BIND_ASSIGN  = "/amca - Assign shop position";
BIND_COMMENT = "/amcc - Change NPC comment";
BIND_CREATE  = "/amcp - Create a shop";
BIND_LABEL   = "/amcl - Change the shop label";
BIND_SIGN    = "/amcs - Set shop sign position";
BIND_TITLE   = "/amct - Change shop comment";
BIND_ZONE    = "/amcz - Setup the zone";

-- help messages
Help01 = "/amc {mode} - Set one of the following modes:";
Help02 = "on - default mode to change or set all kind of text";
Help03 = "all - mode to setup everything";
Help04 = "sign - additional mode to scan shop signs (first person view)";
Help05 = "npc - mode to scan NPCs only";
Help06 = "off - switch off the DB edit mode";
Help07 = "==========================================================";
Help08 = "/amc {function} - Activate one of the following functions:";
Help09 = "help - display this help text";
Help10 = "reload - (re)load the UserDB in place of the AddonDB";
Help11 = "revert - (re)load the AddonDB";
Help12 = "copydbtouser - overwrite the UserDB with the AddonDB (make a copy)";
Help13 = "deleteuserdb - create an empty UserDB to save changes only";
Help14 = "movelabel - allow the positioning of the labels at the map";
Help15 = "defaultlabel - show labels at its default positions";
Help16 = "shoplabel - show labels at the shop positions";
Help17 = "signlabel - show labels at the shop sign positions";
Help18 = "==============================";
Help19 = "Commands to change the UserDB:";
Help20 = "/amcz [zone title] - Setup the Zone at this position (default mode)";
Help21 = "/amcp - Create shop at this position (all mode)";
Help22 = "Add 'move' to move the assigned shop to the actual position";
Help23 = "Add 'nonedelete' to delete the shop at the undefined position";
Help24 = "/amca - Assign to the shop at this position (default mode)";
Help25 = "Add 'none' to setup or assign a shop at the undefined position";
Help26 = "/amcl [label] - Set shop label (displayed at map) (all mode)";
Help27 = "/amct [comment/title] - Change comment of assigned shop (default mode)";
Help28 = "/amcs - Mark the position of the shop sign (all mode)";
Help29 = "Change the shop title by mouse over the shop sign and pressing CTRL+SHIFT";
Help30 = "Add NPCs to a shop by targeting it, and moving the mouse over the target window";
Help31 = "/amcc [comment] - Change comment of the target NPC";
Help32 = "Add 'delete' to remove a zone name, shop label, shop title, shop sign, or shop position";

-- error messages
errNoAssignment = "ERROR: No shop assignment. Assign a shop first.";
errNoCityDef    = "ERROR: There is no city map in the database for: ";
errNoNPCDef     = "ERROR: This NPC is not in the database.";
errNoShopList   = "ERROR: There is no shop setup near this position.";
errNoShopSign   = "ERROR: No shop sign at this mouse position. (Maybe a slight different mouse position helps.)";
errNoZoneDef    = "ERROR: No zone definition for this map or at this position: ";
errNoZoneList   = "ERROR: There is no zone setup at this position.";
errNoSameCity   = "ERROR: The assigned shop is not in the actual city.";
errOldShop      = "ERROR: There is already a shop at this position.";
errOldShopSign  = "ERROR: There is already a shop sign at this position: ";
errNoNPC        = "ERROR: No target or target not in NPC database.";
errNoMoveNopos  = "ERROR: The shop with no position can not be moved.";

-- popup dialog titles
dialogNPCComment = "Setup a comment added to the next NPC";
dialogShopLabel  = "Give the label for this location";
dialogShopTitle  = "Give the shop title or location description";
dialogZoneTitle  = "Give the subzone title";

-- label pull down menu
labelPullDown = "Label Display Mode:";
-- cmd line commands of the display modes
movelabel = "movelabel";
defaultlabel = "defaultlabel";
shoplabel = "shoplabel";
signlabel = "signlabel";
-- cmd line commands for data reload
reload = "reload";
revert = "revert";

-- status messages
msgCreatedShop = "** Shop created with ID: ";
msgCreatedSign = "* Shop sign created with ID: ";
msgCreatedNPC  = "* NPC with # added to shop with label: ";

msgChangedShop = "* Shop position changed: ";
msgChangedNPC  = "* Changed NPC #";

msgDelComment = "* Comment from target NPC removed";
msgDelNPC     = "* Removed NPC with #";
msgDelSign    = "* Sign of this shop removed";
msgDelLabel   = "* Label for this shop removed";
msgDelShop    = "** This shop is deleted";
msgDelTitle   = "* Comment from this shop removed";
msgDelZone    = "*** Title for this zone removed";

msgIsLabel       = "* Shop label: ";
msgIsNPCComment  = "* Comment added to target NPC: ";
msgIsShop        = "** Shop is now: ";
msgIsShopComment = "* Shop comment: ";
msgIsShopSign    = "* Changed shop sign #";

msgOff       = "AtlasMajorCities scan mode disabled";
msgOn        = "AtlasMajorCities default scan mode enabled (all text)";
msgOnAll     = "AtlasMajorCities all scan mode enabled (positions and text)";
msgOnNPC     = "AtlasMajorCities NPC scan mode enabled";
msgOnSign    = "AtlasMajorCities sign scan mode enabled";
msgOffSign   = "AtlasMajorCities sign scan mode disabled";
msgDBcopied  = "AtlasMajorCities DB copied to user data";
msgDBdeleted = "AtlasMajorCities user DB deleted and new initialized";
msgnoDB      = "AtlasMajorCities base DB not found. Nothing changed";
msgUserOnly  = "This function is only avalaible with UserDB. First use: /amc reload";
msgOnMove    = "AtlasMajorCities labels positioning mode enabled at the map";

-- sub-commands
subMoveString     = "move";
subDeleteString   = "delete";
subNoPosString    = "none";
subDelNoPosString = "nonedelete";
}

-- ********************************************************************

if ( GetLocale() == "deDE" ) then

AtlasMajorCities_Locale = {

Atlas_Category = "Gro\195\159st\195\164dte";

}

else

AtlasMajorCities_Locale = {}

end

-- ********************************************************************

AtlasMajorCities_loc = AtlasMajorCities_Strings;
for key, val in pairs(AtlasMajorCities_loc) do
	if ( AtlasMajorCities_Locale[key] ) then
		AtlasMajorCities_loc[key] = AtlasMajorCities_Locale[key];
	end
end
