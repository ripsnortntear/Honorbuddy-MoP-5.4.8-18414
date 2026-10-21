using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.POI;
using Styx.Helpers;
using Styx.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWCache;
using Styx.WoWInternals.WoWObjects;
using Styx.MemoryManagement;

namespace HighVoltz
{
    /// <summary>
    /// Utility functions
    /// </summary>
    public static class Util
    {
        private const int CacheSize = 0x500;

        /// <summary>
        ///  Random Number Genorator
        /// </summary>
        public static Random Rng = new Random(Environment.TickCount);

        private static WoWPoint _lastPoint = WoWPoint.Zero;
        private static DateTime _lastMove = DateTime.Now;
        private static uint _ping = Lua.GetReturnVal<uint>("return GetNetStats()", 3);
        private static readonly Stopwatch PingSW = new Stopwatch();
        private static readonly Dictionary<uint, int> BagStorageTypes = new Dictionary<uint, int>();

        static Util()
        {
            IsBankFrameOpen = false;
        }

        /// <summary>
        /// Creates a random upper/lowercase string
        /// </summary>
        /// <returns>Random String</returns>
        public static string RandomString
        {
            get
            {
                int size = Rng.Next(6, 15);
                var sb = new StringBuilder(size);
                for (int i = 0; i < size; i++)
                {
                    // random upper/lowercase character using ascii code
                    sb.Append((char) (Rng.Next(2) == 1 ? Rng.Next(65, 91) + 32 : Rng.Next(65, 91)));
                }
                return sb.ToString();
            }
        }

        public static bool IsBankFrameOpen { get; internal set; }

        public static bool IsGbankFrameVisible
        {
            get
            {
                return
                    Lua.GetReturnVal<int>(
                        "if GuildBankFrame and GuildBankFrame:IsVisible() then return 1 else return 0 end ", 0) == 1;
            }
        }

        /// <summary>
        /// Returns WoW's ping, refreshed every 30 seconds.
        /// </summary>
        public static uint WoWPing
        {
            get
            {
                if (!PingSW.IsRunning)
                    PingSW.Start();
                if (PingSW.ElapsedMilliseconds > 30000)
                {
                    _ping = Lua.GetReturnVal<uint>("return GetNetStats()", 3);
                    PingSW.Reset();
                    PingSW.Start();
                }
                return _ping;
            }
        }

        /// <summary>
        /// Returns the localized name of an item that is cached
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetItemCacheName(uint id)
        {
            WoWCache.InfoBlock cache = StyxWoW.Cache[CacheDb.Item].GetInfoBlockById(id);
            if (cache != null)
                return StyxWoW.Memory.ReadString(cache.ItemSparse.Name, Encoding.UTF8);
            return null;
        }

        public static void MoveTo(WoWPoint point)
        {
            if (BotPoi.Current.Type != PoiType.None)
                BotPoi.Clear();
            if (CharacterSettings.Instance.UseMount && !StyxWoW.Me.Mounted && Mount.CanMount() && Mount.ShouldMount(point))
                Mount.MountUp(() => point);
            _lastPoint = point;
            _lastMove = DateTime.Now;
            Navigator.MoveTo(point);
        }

        public static WoWPoint GetMoveToDestination()
        {
            if (DateTime.Now.Subtract(_lastMove).TotalSeconds < 4 && _lastPoint != WoWPoint.Zero)
                return _lastPoint;
            return StyxWoW.Me.Location;
        }

        /// <summary>
        /// Converts a string of 3 numbers to a WoWPoint.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static WoWPoint StringToWoWPoint(string location)
        {
            WoWPoint loc = WoWPoint.Zero;
            var pattern = new Regex(@"-?\d+\.?(\d+)?", RegexOptions.CultureInvariant);
            MatchCollection matches = pattern.Matches(location);
            if (matches.Count >= 3)
            {
                loc.X = matches[0].ToString().ToSingle();
                loc.Y = matches[1].ToString().ToSingle();
                loc.Z = matches[2].ToString().ToSingle();
            }
            return loc;
        }

        /// <summary>
        ///  Returns number items with a matching id that player has in personal bank
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static int GetBankItemCount(uint itemID)
        {
            try
            {
                // number of items in objectmanger - (carriedItemCount + BuybackItemCount)
                return (int) ObjectManager.GetObjectsOfType<WoWItem>().
                                 Sum(i => i != null
                                          && i.IsValid
                                          && i.Entry == itemID
                                              ? i.StackCount
                                              : 0) -
                       (GetCarriedItemCount(itemID) + GetBuyBackItemCount(itemID));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns number items with a matching id that player is carrying
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>Number of items in player Inventory</returns>
        public static int GetCarriedItemCount(uint id)
        {
            return
                (int) StyxWoW.Me.CarriedItems.Sum(i => i != null && i.IsValid && i.Entry == id ? i.StackCount : 0);
        }

        /// <summary>
        /// Returns number items with a matching id that player is carrying
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <returns>Number of items in merchant buyback frame</returns>
        public static int GetBuyBackItemCount(uint id)
        {
            return
                (int)
                StyxWoW.Me.Inventory.Buyback.Items.Sum(
                    i => i != null && i.IsValid && i.Entry == id ? i.StackCount : 0);
        }

        // credits Dfagan

        public static int StorageType(uint id)
        {
            int storagetype;
            if (BagStorageTypes.ContainsKey(id))
            {
                BagStorageTypes.TryGetValue(id, out storagetype);
            }
            else
            {
                storagetype = Lua.GetReturnVal<int>("return GetItemFamily(" + id + ")", 0);
                BagStorageTypes.Add(id, storagetype);
            }
            return storagetype;
        }


        public static uint BagRoomLeft(uint id)
        {
            int storagetype = StorageType(id);
            uint freeSlots = StyxWoW.Me.Inventory.Backpack.FreeSlots;
            for (uint i = 0; i < 4; i++)
            {
                WoWContainer bagAtIndex = StyxWoW.Me.GetBagAtIndex(i);
                if (bagAtIndex != null)
                {
                    int bagtype = StorageType(bagAtIndex.Entry);
                    if (bagtype == 0 || (bagtype & storagetype) > 0)
                    {
                        freeSlots += bagAtIndex.FreeSlots;
                    }
                }
            }
            return freeSlots;
        }

        // this factors in the material list
        public static int CalculateRecipeRepeat(Recipe recipe)
        {
            int repeat = (from ingred in recipe.Ingredients
                          let ingredCnt = (int) ingred.InBagItemCount -
                                          (Professionbuddy.Instance.MaterialList.ContainsKey(ingred.ID)
                                               ? Professionbuddy.Instance.MaterialList[ingred.ID]
                                               : 0)
                          select (int) Math.Floor(ingredCnt/(double) ingred.Required)).Min();
            return repeat > 0 ? repeat : 0;
        }

        internal static void OnBankFrameOpened(object obj, LuaEventArgs args)
        {
            IsBankFrameOpen = true;
        }

        internal static void OnBankFrameClosed(object obj, LuaEventArgs args)
        {
            IsBankFrameOpen = false;
        }

        /// <summary>
        /// Looks for a pattern in WoW's memory and returns the offset of pattern if found otherwise an InvalidDataException is thrown
        /// </summary>
        /// <param name="pattern">the pattern to look for, in space delimited hex string format e.g. "DE AD BE EF" </param>
        /// <param name="mask">the mask specifies what bytes in pattern to ignore, The '?' character means ignore the byte, anthing else is not ignored</param>
        /// <returns>The offset the first match of the pattern was found at.</returns>
        public static IntPtr FindPattern(string pattern, string mask)
        {
            byte[] patternArray = HexStringToByteArray(pattern);
            bool[] maskArray = MaskStringToBoolArray(mask);
            ProcessModule wowModule = StyxWoW.Memory.Process.MainModule;

            IntPtr start = wowModule.BaseAddress;
            int size = wowModule.ModuleMemorySize;
            int patternLength = mask.Length;
            for (int cacheOffset = 0; cacheOffset < size; cacheOffset += CacheSize - patternLength)
            {
                int bytesToRead = CacheSize > size - cacheOffset ? size - cacheOffset : CacheSize;

                // byte[] cache = StyxWoW.Memory.ReadBytes(cacheOffset,
                //                                             CacheSize > size - cacheOffset
                //                                                 ? size - (int)cacheOffset
                //                                                 : CacheSize);
                byte[] cache = StyxWoW.Memory.ReadBytes(start + cacheOffset, bytesToRead);

                for (uint cacheIndex = 0; cacheIndex < cache.Length - patternLength; cacheIndex++)
                {
                    if (DataCompare(cache, cacheIndex, patternArray, maskArray))
                        return new IntPtr(cacheOffset + cacheIndex);
                }
            }
            throw new InvalidDataException("Pattern not found");
        }

        private static byte[] HexStringToByteArray(string hexString)
        {
            return hexString.Split(' ')
                .Aggregate(new List<byte>(), (a, b) =>
                                                 {
                                                     a.Add(byte.Parse(b, NumberStyles.HexNumber));
                                                     return a;
                                                 })
                .ToArray();
        }

        private static bool[] MaskStringToBoolArray(string mask)
        {
            return mask.Aggregate(new List<bool>(), (a, b) =>
                                                        {
                                                            a.Add(b != '?');
                                                            return a;
                                                        }).ToArray();
        }

        private static bool DataCompare(byte[] data, uint dataOffset, byte[] pattern, IEnumerable<bool> mask)
        {
            return !mask.Where((t, i) => t && pattern[i] != data[dataOffset + i]).Any();
        }

        public static void ScanForOffsets()
        {
            ProcessModule mod = StyxWoW.Memory.Process.MainModule;
            var baseAddress = (uint) mod.BaseAddress;
            if (GlobalPBSettings.Instance.WowVersion != mod.FileVersionInfo.FileVersion ||
                GlobalPBSettings.Instance.KnownSpellsPtr == 0 || GlobalPBSettings.Instance.MinEnchantSkillReqPtr == 0)
            {
                Professionbuddy.Log("Scanning for new offsets for WoW {0}", mod.FileVersionInfo.FileVersion);
                try
                {
                    IntPtr pointer =
                        FindPattern(
                            "00 00 00 00 C1 EA 05 23 04 91 F7 D8 1B C0 F7 D8 5D C3",
                            "????xxxxxxxxxxxxxx");

                    GlobalPBSettings.Instance.KnownSpellsPtr = StyxWoW.Memory.Read<uint>(true, pointer) - baseAddress;
                    Professionbuddy.Log("Found KnownSpellsPtr offset 0x{0:X}", GlobalPBSettings.Instance.KnownSpellsPtr);

                    GlobalPBSettings.Instance.MinEnchantSkillReqPtr =
                        (uint)FindPattern(
                            "56 8b c1 33 f6 e8 00 00 00 00 85 c0 7e 00 50 b9 00 00 00 00 e8 00 00 00 00 85 c0 74 00 8b 40 18 5e c3",
                            "xxxxxx????xxx?xx????x????xxx?xxxxx");

                    Professionbuddy.Log("Found MinEnchantSkillReqPtr offset 0x{0:X}",
                                        GlobalPBSettings.Instance.MinEnchantSkillReqPtr);

                    GlobalPBSettings.Instance.WowVersion = mod.FileVersionInfo.FileVersion;

                    GlobalPBSettings.Instance.Save();
                }
                catch (InvalidDataException)
                {
                    Professionbuddy.Err("There was a problem scanning for offsets");
                }
            }
        }
    }

    internal static class Exts
    {
        private static readonly Encoding EncodeUtf8 = Encoding.UTF8;

        public static uint ToUint(this string str)
        {
            uint val;
            uint.TryParse(str, out val);
            return val;
        }

        /// <summary>
        /// Converts a string to a float using En-US based culture
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToSingle(this string str)
        {
            float val;
            float.TryParse(str, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign
                           , CultureInfo.InvariantCulture, out val);
            return val;
        }

        /// <summary>
        /// Converts a string to a formated UTF-8 string using \ddd format where ddd is a 3 digit number. Useful when importing names into lua that are UTF-16 or higher.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToFormatedUTF8(this string text)
        {
            var buffer = new StringBuilder(EncodeUtf8.GetByteCount(text));
            byte[] utf8Encoded = EncodeUtf8.GetBytes(text);
            foreach (byte b in utf8Encoded)
            {
                buffer.Append(string.Format("\\{0:D3}", b));
            }
            return buffer.ToString();
        }

        /// <summary>
        /// This is a fix for WoWPoint.ToString using current cultures decimal separator.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToInvariantString(this WoWPoint text)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", text.X, text.Y, text.Z);
        }

        public static int MinEnchantLevelReq(this WoWItem item)
        {
            WoWCache.InfoBlock infoBlock = StyxWoW.Cache[CacheDb.Item].GetInfoBlockById(item.Entry);

            return
                StyxWoW.Memory.Call<int>(
                    StyxWoW.Memory.ImageBase + (int) GlobalPBSettings.Instance.MinEnchantSkillReqPtr,
                    CallingConvention.ThisCall, (uint) infoBlock.Address);
        }

        public static int MinInscriptionLevelReq(this WoWItem item)
        {
            WoWCache.ItemSparseEntry itemSparse = StyxWoW.Cache[CacheDb.Item].GetInfoBlockById(item.Entry).ItemSparse;
            if ((itemSparse.Flags & 0x20000000) > 0)
                return itemSparse.RequiredSkillLevel;
            return -1;
        }

        public static int MinJewelCraftLevelReq(this WoWItem item)
        {
            WoWCache.ItemSparseEntry itemSparse = StyxWoW.Cache[CacheDb.Item].GetInfoBlockById(item.Entry).ItemSparse;
            if ((itemSparse.Flags & 0x40000) > 0)
                return itemSparse.RequiredSkillLevel;
            return -1;
        }
    }
}