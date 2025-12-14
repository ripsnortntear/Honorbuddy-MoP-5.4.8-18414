using System;
using System.Collections.Generic;

namespace VitalicRotation.Helpers
{
    [Flags]
    public enum Totems
    {
        None = 0,
        SpiritLink    = 1 << 0, // 98008
        Earthgrab     = 1 << 1, // 51485
        HealingStream = 1 << 2, // 5394
        HealingTide   = 1 << 3, // 108280
        Capacitor     = 1 << 4, // 108269
        Grounding     = 1 << 5, // 8177
        Windwalk      = 1 << 6, // 108273
        Tremor        = 1 << 7, // 8143 (UI expects)
        StoneBulwark  = 1 << 8  // 108270 (UI expects)
    }

    public static class TotemMappings
    {
        // Mapping des sorts de création -> famille Totems (parité code existant/UI)
        public static readonly Dictionary<int, Totems> SpellToTotemMap = new Dictionary<int, Totems>
        {
            { 98008,  Totems.SpiritLink   },
            { 51485,  Totems.Earthgrab    },
            { 5394,   Totems.HealingStream},
            { 108280, Totems.HealingTide  },
            { 108269, Totems.Capacitor    },
            { 8177,   Totems.Grounding    },
            { 108273, Totems.Windwalk     },
            { 8143,   Totems.Tremor       },
            { 108270, Totems.StoneBulwark },
        };
    }
}
