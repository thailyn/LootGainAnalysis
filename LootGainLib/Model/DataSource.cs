using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class DataSource
    {
        public int DataVersion { get; set; }
        public string Build { get; set; }
        public double Time { get; set; }

        public string PlayerName { get; set; }
        public string RealmName { get; set; }
        public string PlayerRace { get; set; }
        public int PlayerSex { get; set; }
        public string PlayerClass { get; set; }
        public int PlayerLevel { get; set; }
        public int? PlayerSpecialization { get; set; }

        public bool InParty { get; set; }
        public bool inRaid { get; set; }

        public string ZoneName { get; set; }
        public string SubZoneName { get; set; }

        public string GuildName { get; set; }
        public int? GuildLevel { get; set; }

        public List<Quest> Quests { get; set; }

        public List<Item> Items { get; set; }

        public List<Profession> Professions { get; set; }

        public List<Aura> Auras { get; set; }

        public string Guid { get; set; }
        public string SourceName { get; set; }
        public int? SourceLevel { get; set; }
        public string SourceClass { get; set; }
        public int? SourceRace { get; set; }
        public int? SourceSex { get; set; }
        public string SourceClassification { get; set; }
        public string SourceCreatureFamily { get; set; }
        public string SourceCreatureType { get; set; }
        public bool SourceIsPlayer { get; set; }

        public string LootType { get; set; }
        public List<Loot> Loot { get; set; }

        public DataSource()
        {
            Quests = new List<Quest>();
            Items = new List<Item>();
            Professions = new List<Profession>();
            Auras = new List<Aura>();
            Loot = new List<Loot>();
        }

        public bool HasLoot(int itemId)
        {
            foreach (var loot in Loot)
            {
                if (loot.ItemLink != null)
                {
                    var itemInfo = ItemInfo.ParseItemString(loot.ItemLink);
                    if (itemInfo.LinkType == LinkType.Item)
                    {
                        if (itemInfo.Id == itemId)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
