DESCRIPTION

MillButton is a very small addon that greatly simplifies the macro a scribe uses to mill semi-large quantities of herbs. Instead of being limited by the macro's resources - character limit, what functions can be called - the new macro (instructions are further down) calls MillButton, which executes from the Addons.

Important:

* The herbs must be: in stacks of 5; multiples of 5; or at least greater than or equal to 5. The addon will not and cannot compact disparate same-type stacks. It will output the message "No more herbs in stacks of 5 or more." to your chat window.

* Make certain that only the herbs you want to mill are in your bags. The addon is not capable of differentiating between mill and don't-mill.

* If you are a scribe that does a super load of milling on a constant basis, i.e.: to create and sell lots of glyphs on the Auction House, I suggest using a different addon. MillButton is geared for scribes that perform milling in spurts.

* MillButton now mills Desecrated Herb again! Thank you Puxx and Ro.

* Added locale support for deDE, esES/esMX, frFR, itIT, and ptBR! Big thank you to flow0248.

* Added locale support for ruRU! Thank you to Black_Mokona.

* MillButton now ignores Golden Lotus and all partial herbs (ex: Torn Green Tea Leaf)! flow0248 is awesome. <3 <3 <3

INSTRUCTIONS	

--Download the addon and place it in your AddOns folder. The AddOns folder is located within the Interface folder of your World of Warcraft folder. To give you an idea: C:\Program Files\World of Warcraft\Interface\AddOns. If you are using the Curse client, then don't worry about this step.

--Next, make certain you have MillButton enabled in the Addons list. Launch World of Warcraft. At the character selection screen, look at the bottom left for the AddOns button. Click the button. Scroll, if necessary, down the list until you see "MillButton". Click next to it to place a check mark (click again to remove the check mark, disabling MillButton, if you want to turn the addon off). Click "Okay".

--Now log in to the character you mill with. Once in the game, type /macro . This shortcut command opens up your Macro page. NOTE: If you have multiple characters that you will use to mill herbs, then stay in the General Macros tab. Otherwise, click <Character name> Specific Macros. Or not. Whichever you prefer or works best for you.

--Choose an icon for the macro to use. Name it something appropriate to what you will use it for. Keep it short. In my case, I named it "mill".

--In the box labeled "Enter Macro Commands" type the following text:

#showtooltip Milling
/run MillButtonSetup()
/click MillButton

--This macro works no matter your game locale.

--Remember to click the Save button! Just in case. :)

--Finally, click and drag the icon to any location on your action bars. Now if you want, you can - as Ro put it - "Put it on mousewheel and spin ahoy." I myself prefer to click.

I hope this is useful! If I wasn't clear on something, please let me know in the Comments section for this project on Curse.com. http://www.curse.com/addons/wow/millbutton

CREDIT

Last but never least - credit and thank you to Ro of Hyjal for creating and sharing this in the WoW Professions forum ( http://us.battle.net/wow/en/forum/topic/2301723526#4 ). (Plus, I'm really sorry that I missed giving you credit for so long, I honestly thought I'd written it in here!) I'm maintaining it on Curse so more people are aware of it and can benefit from it. Ro, you are awesome. <3