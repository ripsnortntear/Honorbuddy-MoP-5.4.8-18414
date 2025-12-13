
RoutineToggles = {
	movementStatus = false;
	targetingStatus = false;
	facingStatus = false;
	aoeStatus = false;
	cooldownStatus = false;
	burstStatus = false;
}
	 
SLASH_CDTOGGLE1 = '/MillzCooldowns';
function SlashCmdList.CDTOGGLE(msg, editbox) -- 4. 
	if msg == "true" then
		RoutineToggles.cooldownStatus = true;
	end
	if msg == "false" then
		RoutineToggles.cooldownStatus = false;
	end
end

SLASH_BURSTTOGGLE1 = '/MillzBurst';
function SlashCmdList.BURSTTOGGLE(msg, editbox) -- 4.
	if msg == "true" then
		RoutineToggles.burstStatus = true;
	end
	if msg == "false" then
		RoutineToggles.burstStatus = false;
	end
end

SLASH_AOETOGGLE1 = '/MillzAoE';
function SlashCmdList.AOETOGGLE(msg, editbox) -- 4.
	if msg == "true" then
		RoutineToggles.aoeStatus = true;
	end
	if msg == "false" then
		RoutineToggles.aoeStatus = false;
	end
end

SLASH_MOVEMENTTOGGLE1 = '/MillzMovement';
function SlashCmdList.MOVEMENTTOGGLE(msg, editbox) -- 4.
	if msg == "true" then
		RoutineToggles.movementStatus = true;
	end
	if msg == "false" then
		RoutineToggles.movementStatus = false;
	end
end

SLASH_TARGETINGTOGGLE1 = '/MillzTargeting';
function SlashCmdList.TARGETINGTOGGLE(msg, editbox) -- 4.
	if msg == "true" then
		RoutineToggles.targetingStatus = true;
	end
	if msg == "false" then
		RoutineToggles.targetingStatus = false;
	end
end

SLASH_FACINGTOGGLE1 = '/MillzFacing';
function SlashCmdList.FACINGTOGGLE(msg, editbox) -- 4.
	if msg == "true" then
		RoutineToggles.facingStatus = true;
	end
	if msg == "false" then
		RoutineToggles.facingStatus = false;
	end
end

function RoutineToggles_OnUpdate(self, elapsed) 
	self.TimeSinceLastUpdate = self.TimeSinceLastUpdate + elapsed;
	local cStatus = "|cffFF0000Disabled";
	local bStatus = "|cffFF0000Disabled";
	local aStatus = "|cffFF0000Disabled";
	local tStatus = "|cffFF0000Disabled";
	local mStatus = "|cffFF0000Disabled";
	local fStatus = "|cffFF0000Disabled";
		
	if (RoutineToggles.cooldownStatus == true) then
		cStatus = "|cff00FF00Enabled";
	end
	if (RoutineToggles.burstStatus == true) then
		bStatus = "|cff00FF00Enabled";
	end
	if (RoutineToggles.aoeStatus == true) then
		aStatus = "|cff00FF00Enabled";
	end
	if (RoutineToggles.targetingStatus == true) then
		tStatus = "|cff00FF00Enabled";
	end
	if (RoutineToggles.movementStatus == true) then
		mStatus = "|cff00FF00Enabled";
	end
	if (RoutineToggles.facingStatus == true) then
		fStatus = "|cff00FF00Enabled";
	end	
	
	if (self.TimeSinceLastUpdate > 1.0) then
		Millz_FrameText:SetText("[Millz' Combat Routines]\n|cffFFFFFFTargeting: "..tStatus.."\n|cffFFFFFFFacing: "..fStatus.."\n|cffFFFFFFMovement: "..mStatus.."\n\n|cffFFFFFFCooldowns: "..cStatus.."\n|cffFFFFFFBurst: "..bStatus.."\n|cffFFFFFFAoE: "..aStatus.."\n");
	end
end 	



