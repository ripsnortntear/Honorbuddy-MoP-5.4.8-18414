#region System Namespace
using System.IO;
#endregion

#region Styx Namespace
using Styx;
using Styx.Common;
using Styx.Helpers;
#endregion

namespace ObjectGatherer {
    public class ObjectGatherer_Settings : Settings {
        public static readonly ObjectGatherer_Settings Instance = new ObjectGatherer_Settings();
        public ObjectGatherer_Settings()
            : base(Path.Combine(Path.Combine(Utilities.AssemblyDirectory, "Settings"), string.Format("ObjectGatherer-Settings-{0}.xml", StyxWoW.Me.Name)))
        {
        }

        #region Ancient Guo-Lai Cashe Settings
        [Setting, DefaultValue(true)]
        public bool AglcCb { get; set; }
        #endregion

        #region Dark Soil Settings
        [Setting, DefaultValue(true)]
        public bool DsCb { get; set; }
        #endregion

        #region Gold Coins Settings
        [Setting, DefaultValue(true)]
        public bool GcCb { get; set; }
        #endregion

        #region Is Another Man's Treasure Settings
        [Setting, DefaultValue(true)]
        public bool IamtCb { get; set; }
        #endregion

        #region Is Another Man's Treasure NPC's Settings
        [Setting, DefaultValue(true)]
        public bool IamtnCb { get; set; }
        #endregion

        #region Netherwing Egg Settings
        [Setting, DefaultValue(true)]
        public bool NeCb { get; set; }
        #endregion

        #region Onyx Egg Settings
        [Setting, DefaultValue(true)]
        public bool OeCb { get; set; }
        #endregion

        #region Treasure Chests Settings
        [Setting, DefaultValue(true)]
        public bool TcCb { get; set; }
        #endregion

        #region Quests Settings
        [Setting, DefaultValue(true)]
        public bool QCb { get; set; }
        #endregion
        
        #region Skin/Herb/Mine Corpses Settings
        [Setting, DefaultValue(true)]
        public bool ShmcCb { get; set; }
        #endregion
    }
}
