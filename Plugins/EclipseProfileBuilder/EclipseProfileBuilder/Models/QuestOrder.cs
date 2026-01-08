using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Models
{
    public class QuestOrder
    {
        public QuestOrder Parent { get; set; }
        private string _giverName = string.Empty;
        private string _turnInName = string.Empty;
        public string QuestName { get; set; }
        public string GiverName
        {
            get
            {
                if (_giverName == string.Empty) return "Unknown";
                else return _giverName;
            }
            set { _giverName = value; }
        }
        public string TurnInName
        {
            get
            {
                if (_turnInName == string.Empty) return "Unknown";
                else return _turnInName;
            }
            set { _turnInName = value; }
        }
        public string GiverId { get; set; }
        public uint QuestId { get; set; }
        public QuestObjective.QuestType objectiveType {get;set;}
        public string QuestObjectiveType
        {
            get
            {
                if (objectiveType != null) return objectiveType.ToString();
                else return string.Empty;
            }
        }
        public string Type
        {
            get
            {
                if (type != null) return type.ToString();
                else return string.Empty;
            }
        }
        public string MobId { get; set; }
        public string ItemId { get; set; }
        public string KillCount { get; set; }
        public string CollectCount { get; set; }
        public string TurnInId { get; set; }
        public QOType type { get; set; }
        public string Description { get; set; }
        public string DisplayName {
            get { 
                if (QuestName != null) return string.Format("{0} => {1}", type.ToString(), QuestName);
                else if (type == QOType.CustomBehavior) return string.Format("{0} => {1}", type.ToString(), ((CustomBehavior)this).file);
                else if (type == QOType.RunTo) return string.Format("{0} => {1},{2},{3}", type.ToString(), X,Y,Z);
                else if (type == QOType.LogicBlock)
                {
                    var qol = (QuestOrderLogic)this;
                    var tag = "";
                    if (qol.StartTag) tag = "(Start)";
                    else tag = "(End)";
                    if (Description == string.Empty || Description == null) return string.Format("{0}=>{2}{1}", qol.LogicType, qol.Condition, tag);
                    else return string.Format("{0}=>{2}{1}", qol.LogicType, qol.Description, tag);
                }
                else if (type == QOType.PickUp || type == QOType.TurnIn) return string.Format("{0} => {2}({1})", type.ToString(), QuestId, QuestName);
                else if (type == QOType.Objective) return string.Format("{0} => {2}({1})", type.ToString(), QuestId, QuestName);
                else return string.Format("{0} => {1}", type.ToString(), objectiveType);
            }
        }
        public enum QOType
        {
            PickUp,
            Objective,
            TurnIn,
            UseItem,
            CustomBehavior,
            LogicBlock,
            RunTo,
            FlyTo,
            MoveTo
        }
        public string ItemName { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }

        public string MobName { get; set; }

        public string NumOfTimes { get; set; }
    }
}
