--[[
	Auctioneer
	Version: 5.19.5445 (QuiescentQuoll)
	Revision: $Id: CoreManifest.lua 5442 2013-11-28 09:50:05Z brykrys $
	URL: http://auctioneeraddon.com/

	This is an addon for World of Warcraft that adds statistical history to the auction data that is collected
	when the auction is scanned, so that you can easily determine what price
	you will be able to sell an item for at auction or at a vendor whenever you
	mouse-over an item in the game

	License:
		This program is free software; you can redistribute it and/or
		modify it under the terms of the GNU General Public License
		as published by the Free Software Foundation; either version 2
		of the License, or (at your option) any later version.

		This program is distributed in the hope that it will be useful,
		but WITHOUT ANY WARRANTY; without even the implied warranty of
		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
		GNU General Public License for more details.

		You should have received a copy of the GNU General Public License
		along with this program(see GPL.txt); if not, write to the Free Software
		Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

	Note:
		This AddOn's source code is specifically designed to work with
		World of Warcraft's interpreted AddOn system.
		You have an implicit license to use this AddOn with these facilities
		since that is its designated purpose as per:
		http://www.fsf.org/licensing/licenses/gpl-faq.html#InterpreterIncompat

	CoreManifest is the first Auctioneer file to be loaded

	CoreManifest will:
		Set up the AucAdvanced global table and its basic framework
		Load external libraries
		Set up basic debugging and logging functions
		Perform startup checks

	About the ABORTLOAD flag
		CoreManifest, and certain other Core modules, will set the ABORTLOAD flag
		if they detect a critical problem during loading.

		The ABORTLOAD flag will block the loading or activation of certain key functions
		of Auctioneer, but allows most other Core components to install regardless.
		This should, in most cases, avoid large (and confusing) error cascades.
]]

AucAdvanced = {Modules = {Filter={}, Match={}, Stat={}, Util={}}}
local lib = AucAdvanced

local DEV_VERSION = "5.19.DEV"
local MINIMUM_TOC = 50400
local MINIMUM_CLIENT = "5.4"

lib.Version="5.19.5445";
if (lib.Version == "<".."%version%>") then
	lib.Version = DEV_VERSION
end
local major, minor, release, revision = strsplit(".", lib.Version)
lib.MajorVersion = major
lib.MinorVersion = minor
lib.RelVersion = release
lib.Revision = revision

-- Single instance of a 'no operation' function
lib.NOPFUNCTION = function() end

-- Test for load failure conditions in priority order

-- Check TOC version meets minimum requirements
local _,_,_,tocVersion = GetBuildInfo()
if (tocVersion < MINIMUM_TOC) then
	message("Auctioneer requires game client version "..MINIMUM_CLIENT.." or higher.")
	lib.ABORTLOAD = "Incorrect WoW client version"
end

-- Check that Stubby exists
if not Stubby then
	-- Can only occur if the Stubby AddOn has loaded, but failed to create the Stubby global table
	-- Assume Stubby has already thrown an error in this case.
	if not lib.ABORTLOAD then lib.ABORTLOAD = "Missing library: Stubby" end
end

-- Test load libraries
local LibRevision, DebugLib, Configator, Babylonian, TipHelper, LibDataBroker
if LibStub then
	LibRevision = LibStub("LibRevision", true)
	DebugLib = LibStub("DebugLib", true)
	Configator = LibStub("Configator", true)
	Babylonian = LibStub("Babylonian", true)
	TipHelper = LibStub("nTipHelper:1", true)
	LibDataBroker = LibStub("LibDataBroker-1.1", true)
else -- missing LibStub - for now we'll assume Stubby has already thrown an error for this
	if not lib.ABORTLOAD then lib.ABORTLOAD = "Missing library: LibStub" end
end
-- Check essential libraries
if not (Configator and Babylonian and TipHelper) then
	if not lib.ABORTLOAD then -- only report error if not already aborting load
		local missing = ""
		if not Configator then
			missing = missing.." Configator"
		end
		if not Babylonian then
			missing = missing.." Babylonian"
		end
		if not TipHelper then
			missing = missing.." Tiphelper"
		end
		lib.ABORTLOAD = "Missing library(s):"..missing
		geterrorhandler()("Auctioneer was unable to load one or more libraries:"..missing)
	end
end

lib.Libraries = {
	LibRevision = LibRevision,
	DebugLib = DebugLib,
	Configator = Configator,
	Babylonian = Babylonian,
	TipHelper = TipHelper,
	LibDataBroker = LibDataBroker,
}


-- Auctioneer's revision information functions

local versionPrefix = lib.MajorVersion.."."..lib.MinorVersion.."."..lib.RelVersion.."."
lib.moduledetail = {}
lib.revisions = {}
lib.distribution = {--[[<%revisions%>]]} --Currently unused, needs a change in the build script

if LibRevision then
	function lib.RegisterRevision(path, revision)
		if (not path and revision) then return end

		local detail, file, rev = LibRevision:Set(path, revision, versionPrefix, "auctioneer", "libs")
		if file then
			lib.revisions[file] = rev
		end
		return detail, file, rev
	end
else
	-- LibRevision failed to load. We consider this non-essential, as it is primarily used for extra info in error reports
	lib.RegisterRevision = lib.NOPFUNCTION
	 -- Notify in chat only. AucPrint is not available here, so use plain lua 'print'
	print("Auctioneer Manifest: LibRevision is missing")
end

function lib.GetCurrentRevision()
	local revNumber = 0
	local revFile
	for file, revision in pairs(lib.revisions) do
		if (revision > revNumber) then
			revNumber = revision
			revFile = file
		end
	end

	return revNumber, revFile
end

function lib.GetRevisionList()
	return lib.revisions
end

function lib.GetDistributionList()
	return lib.distribution
end


-- Auctioneer's debug functions

lib.Debug = {}
local addonName = "Auctioneer" -- the addon's name as it will be displayed in the debug messages

if DebugLib then
	-------------------------------------------------------------------------------
	-- Prints the specified message to nLog.
	--
	-- syntax:
	--    errorCode, message = debugPrint([message][, category][, title][, errorCode][, level])
	--
	-- parameters:
	--    message   - (string) the error message
	--                nil, no error message specified
	--    category  - (string) the category of the debug message
	--                nil, no category specified
	--    title     - (string) the title for the debug message
	--                nil, no title specified
	--    errorCode - (number) the error code
	--                nil, no error code specified
	--    level     - (string) nLog message level
	--                         Any nLog.levels string is valid.
	--                nil, no level specified
	--
	-- returns:
	--    errorCode - (number) errorCode, if one is specified
	--                nil, otherwise
	--    message   - (string) message, if one is specified
	--                nil, otherwise
	-------------------------------------------------------------------------------
	function lib.Debug.DebugPrint(message, category, title, errorCode, level)
		return DebugLib.DebugPrint(addonName, message, category, title, errorCode, level)
	end

	-------------------------------------------------------------------------------
	-- Brings the Level parameter into the auctioneer API fold.
	-- Level is a lookup table for validating the 'level' parameter used in DebugPrint
	-- example AucAdvanced.Debug.Level.Critical
	lib.Debug.Level = DebugLib.Level

	-------------------------------------------------------------------------------
	-- Used to make sure that conditions are met within functions.
	-- If test is false, the error message will be written to nLog and the user's
	-- default chat channel.
	--
	-- syntax:
	--    assertion = assert(test, message)
	--
	-- parameters:
	--    test    - (any)     false/nil, if the assertion failed
	--                        anything else, otherwise
	--    message - (string)  the message which will be output to the user
	--
	-- returns:
	--    assertion - (boolean) true, if the test passed
	--                          false, otherwise
	-------------------------------------------------------------------------------
	function lib.Debug.Assert(test, message)
		return DebugLib.Assert(addonName, test, message)
	end


else
	-- DebugLib failed to load. Again this is considered non-essential as it is primarily used for info logging and debugging
	-- DebugPrint is used for logging to nLog - it does nothing if nLog is not installed
	lib.Debug.DebugPrint = lib.NOPFUNCTION
	lib.Debug.Level = {}
	lib.Debug.Assert = assert
	print("Auctioneer Manifest: DebugLib is missing")
end


function lib.ValidateInstall()
	if lib.ABORTLOAD then
		return nil, lib.ABORTLOAD
	end
	return lib.Resources and lib.Resources.Active
end


lib.RegisterRevision("$URL: http://svn.norganna.org/auctioneer/branches/5.19/Auc-Advanced/CoreManifest.lua $", "$Rev: 5442 $")
