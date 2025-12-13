#region

using Styx.WoWInternals;

#endregion

namespace Superbad
{
    public partial class Superbad
    {
        public static double LuaGetRage()
        {
            return Lua.GetReturnVal<int>("return UnitPower(\"player\");", 0);
        }

        public static double LuaGetSpellCharges()
        {
            return Lua.GetReturnVal<int>("return GetSpellCharges(102703)", 0);
        }
    }
}