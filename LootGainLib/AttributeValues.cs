using LootGainLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib
{
    public enum Attribute
    {
        Build,
        PlayerName,
        PlayerRace,
        PlayerClass,
        ZoneName,
        SubZoneName,
        Quest,
        SourceName,
        LootType,
        Loot
    }

    public class AttributeValues
    {
        public Dictionary<Attribute, Dictionary<object, int>> ValuesMap { get; set; }

        public AttributeValues()
        {
            ValuesMap = new Dictionary<Attribute, Dictionary<object, int>>();
            foreach (var attribute in Enum.GetValues(typeof(Attribute)).Cast<Attribute>())
            {
                ValuesMap[attribute] = new Dictionary<object, int>();
            }
        }
        public void FindValues(List<DataSource> sources)
        {
            int intValue;
            foreach (var source in sources)
            {
                if (!ValuesMap[Attribute.Build].ContainsKey(source.Build))
                {
                    ValuesMap[Attribute.Build].Add(source.Build, 1);
                }
                else
                {
                    ValuesMap[Attribute.Build][source.Build]++;
                }

                if (!ValuesMap[Attribute.PlayerName].ContainsKey(source.PlayerName))
                {
                    ValuesMap[Attribute.PlayerName].Add(source.PlayerName, 1);
                }
                else
                {
                    ValuesMap[Attribute.PlayerName][source.PlayerName]++;
                }

                if (!ValuesMap[Attribute.PlayerRace].ContainsKey(source.PlayerRace))
                {
                    ValuesMap[Attribute.PlayerRace].Add(source.PlayerRace, 1);
                }
                else
                {
                    ValuesMap[Attribute.PlayerRace][source.PlayerRace]++;
                }

                if (!ValuesMap[Attribute.PlayerClass].ContainsKey(source.PlayerClass))
                {
                    ValuesMap[Attribute.PlayerClass].Add(source.PlayerClass, 1);
                }
                else
                {
                    ValuesMap[Attribute.PlayerClass][source.PlayerClass]++;
                }

                if (!ValuesMap[Attribute.ZoneName].ContainsKey(source.ZoneName))
                {
                    ValuesMap[Attribute.ZoneName].Add(source.ZoneName, 1);
                }
                else
                {
                    ValuesMap[Attribute.ZoneName][source.ZoneName]++;
                }

                if (!ValuesMap[Attribute.SubZoneName].ContainsKey(source.SubZoneName))
                {
                    ValuesMap[Attribute.SubZoneName].Add(source.SubZoneName, 1);
                }
                else
                {
                    ValuesMap[Attribute.SubZoneName][source.SubZoneName]++;
                }

                foreach (var quest in source.Quests)
                {
                    if (!ValuesMap[Attribute.Quest].ContainsKey(quest.QuestId))
                    {
                        ValuesMap[Attribute.Quest].Add(quest.QuestId, 1);
                    }
                    else
                    {
                        ValuesMap[Attribute.Quest][quest.QuestId]++;
                    }
                }

                if (!ValuesMap[Attribute.SourceName].ContainsKey(source.SourceName))
                {
                    ValuesMap[Attribute.SourceName].Add(source.SourceName, 1);
                }
                else
                {
                    ValuesMap[Attribute.SourceName][source.SourceName]++;
                }

                if (!ValuesMap[Attribute.LootType].ContainsKey(source.LootType))
                {
                    ValuesMap[Attribute.LootType].Add(source.LootType, 1);
                }
                else
                {
                    ValuesMap[Attribute.LootType][source.LootType]++;
                }

                foreach (var loot in source.Loot)
                {
                    if (!string.IsNullOrWhiteSpace(loot.ItemLink))
                    {
                        var itemInfo = ItemInfo.ParseItemString(loot.ItemLink);
                        if (!ValuesMap[Attribute.Loot].TryGetValue(itemInfo.Id, out intValue))
                        {
                            ValuesMap[Attribute.Loot].Add(itemInfo.Id, 1);
                        }
                        else
                        {
                            ValuesMap[Attribute.Loot][itemInfo.Id]++;
                        }
                    }
                }
            }
        }
    }
}
