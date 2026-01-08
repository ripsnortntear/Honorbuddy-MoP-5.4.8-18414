using Eclipse.EclipsePlugins.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Eclipse.EclipsePlugins.Controllers
{
    public static class ZygorReverseEngineer
    {
        public static EclipseProfile ReadLua(EclipseProfile dt, String filename)
        {
            var _dt = dt;
            var file = File.ReadAllLines(filename);
            var count = 0;
            for (int i =0; i < file.Count(); i++){
                var str = file[i];
                count++;
                if (str.Contains("goto"))
                {
                    QuestOrder qo = new QuestOrder { type = QuestOrder.QOType.RunTo };
                    qo = getPosFromString(str, qo);
                    EclipseProfile.QuestOrders.Add(qo);
                }
                if (str.Contains("..accept")) EclipseProfile.QuestOrders.Add(new QuestOrder { type = QuestOrder.QOType.PickUp, GiverId = file[i - 1].Replace(".talk", "").Trim(), QuestId = uint.Parse(str.Replace("..accept", "").Trim()) });
                if (str.Contains("..turnin")) EclipseProfile.QuestOrders.Add(new QuestOrder { type = QuestOrder.QOType.TurnIn, GiverId = file[i - 1].Replace(".talk", "").Trim(), QuestId =  uint.Parse(str.Replace("..turnin", "").Trim()) });
                if (str.Contains(".kill") && !file[i-1].Contains(".from"))
                {
                    QuestOrder qo = new QuestOrder { type = QuestOrder.QOType.Objective, objectiveType = QuestObjective.QuestType.KillMob };
                    str = str.Replace(".kill", "").Trim();
                    var s = str.Split(new string[] { "#", "|q", "+", "/" }, StringSplitOptions.RemoveEmptyEntries);
                    uint killcount =0;
                    uint.TryParse(s[0].Trim().Split(new string[] {" "},StringSplitOptions.RemoveEmptyEntries)[0], out killcount);
                    if (s.Count() == 5)
                    {
                        qo.KillCount = killcount.ToString();
                        qo.MobId = s[1].Trim();
                        qo.QuestId =  uint.Parse(s[3].Trim());
                    }
                    else
                    {
                        qo.MobId = s[1].Trim();
                        qo.MobName = s[0].Trim();
                        qo.QuestId =  uint.Parse(s[1].Trim());
                    }

                    EclipseProfile.QuestOrders.Add(qo);
                }

            }
            Console.Write(count);
            return dt;
        }
        private static QuestOrder getPosFromString(string line, QuestOrder qo)
        {
            var _qo = qo;
            string[] ss = line.Split(' ');
            foreach (var s in ss)
            {
                if (s.Contains(","))
                {
                    var pos = s.Split(',');
                    _qo.X = pos[0];
                    _qo.Y = pos[1];
                }
            }
            return _qo;
        }

    }
}
