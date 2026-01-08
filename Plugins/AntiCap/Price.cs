using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiCap
{
    public class Price
    {
        public int Amount;
        public uint Currency;
        public string CurrencyName;

        public new string ToString()
        {
            return string.Format("{0} {1}", Amount, CurrencyName);
        }
    }
}
