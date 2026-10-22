function arInit()
	frmautorepair:SetScript("Onevent", arevent);
	frmautorepair:RegisterEvent("MERCHANT_SHOW");
	frmautorepair:RegisterEvent("VARIABLES_LOADED");
	SLASH_AUTOREPAIR1, SLASH_AUTOREPAIR2 = '/ar', '/autorepair';
end

function SlashCmdList.AUTOREPAIR(msg, editbox)
	if armode==nil then
		armode=0;
	end
	if msg=="ownonly" then armode=0;
	elseif msg=="own" then armode=1;
	elseif msg=="guild" then armode=2;
	elseif msg=="guildonly" then armode=3;
	elseif msg=="off" then armode=4;
	else DEFAULT_CHAT_FRAME:AddMessage("/ar ownonly/own/guild/guildonly/off",255,255,0); end

	arStatus();
end

function arevent(self, event)
	if armode==nil then
		armode=0;
	end

	if (event=="MERCHANT_SHOW" and CanMerchantRepair()==1) then
		repairAllCost, canRepair = GetRepairAllCost();
		if (canRepair==1) then
			if(armode<=1) then
				if( repairAllCost<=GetMoney() ) then
					RepairAllItems(0);
					DEFAULT_CHAT_FRAME:AddMessage("Your items have been repaired for "..GetCoinText(repairAllCost,", ")..".",255,255,0);
				else
					DEFAULT_CHAT_FRAME:AddMessage("You don't have enough money for repair!",255,0,0);
				end
			end

			if(armode==2 or armode==3)then
				RepairAllItems(1);
			end

			if(armode==2) then
				if( repairAllCost<=GetMoney() ) then
					RepairAllItems(0);
					DEFAULT_CHAT_FRAME:AddMessage("Your items have been repaired for "..GetCoinText(repairAllCost,", ")..".",255,255,0);
				else
					DEFAULT_CHAT_FRAME:AddMessage("You don't have enough money for repair!",255,0,0);
				end
			end		
		end
	end

	if (event=="VARIABLES_LOADED") then
		arStatus();
	end

end


function arStatus()
	local modetext="";
	if(armode==0)then modetext="repair only from own money"; end
	if(armode==1)then modetext="try to repair from own money first, repair from guild bank if fails"; end
	if(armode==2)then modetext="try to repair from guild bank first, repair from own money if fails"; end
	if(armode==3)then modetext="repair only from guild bank"; end
	if(armode==4)then modetext="automatic repairs off"; end
	DEFAULT_CHAT_FRAME:AddMessage("AutoRepair mode: "..modetext,255,255,0);
end