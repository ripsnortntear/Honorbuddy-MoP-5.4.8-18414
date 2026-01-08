using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Styx;

namespace AntiCap
{
    public class Item : INotifyPropertyChanged
    {
        public uint ItemID;
        private string _itemName;
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                NotifyPropertyChanged("ItemName");
            }
        }
        public uint Quality;
        public BindingList<Price> Prices;
        [XmlIgnore]
        public string PricesString
        {
            get
            {
                var price = Prices[0].ToString();
                if (Prices.Count > 1)
                {
                    var priceString = new StringBuilder();
                    for (int i = 0; i < Prices.Count - 1; i++)
                    {
                        if (i == Prices.Count - 1)
                        {
                            priceString.Append(string.Format(" and {0}", Prices[i].ToString()));
                        }
                        else
                        {
                            priceString.Append(Prices[i].ToString());
                            if (i < Prices.Count - 2)
                            {
                                priceString.Append(", ");
                            }
                        }
                    }
                    price = priceString.ToString();
                }
                return price;
            }
            set
            {
                NotifyPropertyChanged("PricesString");
            }
        }
        public float VendorX;
        public float VendorY;
        public float VendorZ;
        public WoWPoint VendorLocation
        {
            get { return new WoWPoint(VendorX, VendorY, VendorZ); }
        }
        public uint VendorID;
        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                NotifyPropertyChanged("Amount");
            }
        }
        

        public static uint[] AllCurrencies
        {
            get
            {
                return new uint[]
                           {
                               241, // ChampionsSeal
                               390, // ConquestPoints
                               61,  // DalaranJewelcraftersToken
                               515, // DarkmoonPrizeTicket
                               398, // DraeneiArchaeologyFragment
                               384, // DwarfArchaeologyFragment
                               697, // ElderCharmOfGoodFortune
                               81,  // EpicureansAward
                               615, // EssenceOfCorruptedDeathwind
                               393, // FossilArchaeologyFragment
                               392, // HonorPoints
                               361, // IllustriousJewelcraftersToken
                               402, // IronpawToken
                               395, // JusticePoints
                               738, // LesserCharmOfGoodFortune
                               416, // MarkOfTheWorldTree
                               677, // MoguArchaeologyFragment
                               614, // MoteOfDarkness
                               400, // NerubianArchaeologyFragment
                               394, // NightElfArchaeologyFragment
                               397, // OrcArchaeologyFragment
                               676, // PandarenArchaeologyFragment
                               391, // TolBaradCommendation
                               401, // TolvirArchaeologyFragment
                               385, // TrollArchaeologyFragment
                               396, // ValorPoints
                               399, // VrykulArchaeologyFragment
                               698  // ZenJewelcraftersToken
                           };
            }
        }
        public void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
