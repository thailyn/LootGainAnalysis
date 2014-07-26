using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class DataSource
    {
        int DataVersion { get; set; }
        string Build { get; set; }
        double Time { get; set; }

        string PlayerName { get; set; }
        string RealmName { get; set; }
        string PlayerRace { get; set; }
        int PlayerSex { get; set; }
        string PlayerClass { get; set; }
        int PlayerLevel { get; set; }
        int? PlayerSpecialization { get; set; }

        bool InParty { get; set; }
        bool inRaid { get; set; }

        string ZoneName { get; set; }
        string SubZoneName { get; set; }

        string GuildName { get; set; }
        int? GuildLevel { get; set; }

        List<Quest> Quests { get; set; }

        List<Item> Items { get; set; }

        List<Profession> Professions { get; set; }

        List<Aura> Auras { get; set; }

        string Guid { get; set; }
        string SourceName { get; set; }
        int? SourceLevel { get; set; }
        string SourceClass { get; set; }
        int? SourceRace { get; set; }
        int? SourceSex { get; set; }
        string SourceClassification { get; set; }
        string SourceCreatureFamily { get; set; }
        string SourceCreatureType { get; set; }
        bool SourceIsPlayer { get; set; }

        string LootType { get; set; }
        List<Loot> Loot { get; set; }

        public DataSource()
        {
            Quests = new List<Quest>();
            Items = new List<Item>();
            Professions = new List<Profession>();
            Auras = new List<Aura>();
            Loot = new List<Loot>();
        }
    }
}
