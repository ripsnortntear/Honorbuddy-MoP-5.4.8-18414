A short introduction to the edit mode (see at beginning of AtlasMajorCities.lua to enable it):

Commands:
---------

/amc [help / reload / revert / copydbtouser / deleteuserdb / on / all / sign / npc / off]
  on - activate the default editing mode which allow all renamings
       (/amcz (renaming only), /amca, /amct, /amcc, sign, npc)
  all - activate all changes
  sign - switch to activate/deactivate the scanning mode of signs (switches to first person view)
  npc - activate the scanning of NPCs only
  off - deactivate the collection of changes to the users database and the scanning mode

  reload - (re)load the users database in place of the addon database
  revert - (re)load the addon database and ignore the users created database or changes
  copydbtouser - copy all the DBs to the users space, overwrites all former changes made - the internal DB can now be modified by the user
  deleteuserdb - delete the user space and change the user mode to collect only changes related to the addon database

  movelabel - allow the positioning of the labels at the AMCE map
  defaultlabel - show labels at its default positions
  shoplabel - show labels at the shop positions and allow the assign to a shop and remove NPCs from a shop by mouse click
  signlabel - show labels at the shop sign positions

/amcz [{zone name} / delete]
  Create or change the database entry for the actual zone and set the continent, city, and zone name.
  The name of the city is displayed at the pull-down menu of atlas, and is taken from the city name itself, the city faction, and the race of a char in the target, if any.
  Default is to uses the name displayed over the minimap as zone name, but you can also give {zone name} manually.
  The option delete remove the zone name.

/amca [none]
  Activate the shop with the label at the actual position of the char for modifications.
  You optional give "none" to define the "no position" shop (see below).
  If the "shoplabel" mode is activated, a right click to a shop label at the AMCE map allows to assign by click, independend of the position.

/amcp [none / delete / nonedelete]
  Setup a new shop at the actual position, and ask for a label for it. The label is shown at the map, right at your actual position. So, check your position at the map if the label would be right there, because the map is not always identical to the minimap.
  If there already exist a shop at your position, you only move the label to your actual position.
  You optional give "none" to create the "no position" shop (see below).
  With "delete" you remove the active shop. Check that you assigned the right shop with /amca.
  With "nonedelete" you remove the "no position" shop (see below).
  In the "movelabel" mode you can move the label position using your mouse at the AMCE window.

/amcl [{label}]
  Change the label of the active shop. Activate the shop with /amca or /amcp.
  If {label} is not given, a popup window will open.

/amcs
  Add an empty shop sign at the actual position to the active shop. Use /amca to activate the shop at the label position first.
  The name at the sign is set separately in the "sign" mode. The positions of shop signs are shown at the AMCE map in the "signlabel" mode.

/amct [{shop comment}]
  Set or change the comment to a shop. Shop signs are shown between "*" in the list, shop comments are enclosed in "." to separate and identify them.
  If {shop comment} is not given, a popup window will open. This popup window is filled with the old shop comment, the tooltip text, or the minimap title.

/amcc [{NPC comment}]
  Set a comment to the NPC in target, displayed in brackets behind the NPC in the list.
  If {NPC comment} is not given, a popup window will open. This popup window is filled with the old NPC comment.

mouse over sign and CTRL+Shift
  To get the text from a shop sign in the "sign" mode, move the mouse over the sign and press CTRL+Shift.
  Hint: The map is subdivided into a grid, to define the position of the signs. If the sign is located near the border of such a grid cell, a part of the sign is not at the right position, and not identified as the right sign. So, if the shop sign is not found, try to move the mouse to the other side of the sign or change your position. Signs are also only scanned up to a special distance, so you have to go near the sign. Some time it helps also to change your angle to the sign, because signs are not symmetric.

mouse over NPC target frame
  To add a NPC to a shop, activate a shop with /amca or /amcp, take the NPC in target and move the mouse over the target window. If the NPC is already assigned to a shop, only its name is refreshed.
  If you are in the "npc" mode, you refresh only the description of an already assigned NPC, i.e. for localization.

Explanations:
-------------

To all actions a status message is displayed at the chat window.

The "no position" shop is a shop with no defined position. Its used to show NPCs at the list, which have no fixed position to show a label, i.e. the NPC walks around. The label shown at list defaults to "-", if no label is set.

If a shop has only a label, but no description and NPCs, it is not shown at the list. This way the letter box is labeled (with the predefined label "@").

The slash-commands are also included in the key bindings, so you can assign a key to every function. But to use an option, the slash command is required.

The option strings can be loclalized in AtlasMajorCities_loc.lua.

Why the first person mode to scan signs: To identify the signs at its position, the first person mode is required, because it is not possible anymore to get the data of the players camera. One only gets the viewing angle of the player and the display position of the mouse. In the first person mode it is still possible to get the position, where the mouse points to, to identify the sign at the mouse position.
