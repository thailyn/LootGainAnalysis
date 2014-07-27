using LootGainLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LootGainLib.Parsers
{
    public enum FileParserState
    {
        Begin,
        InData,
        InSources,
        InSource,
        RunOutSource,
    }

    public class FileParser
    {
        public FileParser()
        {

        }

        public DataSourcesCollection Parse(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }

            if (!System.IO.File.Exists(fileName))
            {
                throw new InvalidOperationException(string.Format("File '{0}' does not exist.", fileName));
            }

            var dataSources = new DataSourcesCollection();
            DataSource currentSource = null;

            var simpleRegex = new Regex(@"\s+""?([^""]*)""?, --");
            var reverseRegex = new Regex(@"\s+\[""?([^\]""]+)""?\] = ""?([^""]*)""?,");

            string line;
            var state = FileParserState.Begin;
            var reader = new System.IO.StreamReader(fileName);
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                switch (state)
                {
                    case FileParserState.Begin:
                        if (line.StartsWith(@"LootGain_Data = {"))
                        {
                            //System.Console.WriteLine("Now in data.");
                            state = FileParserState.InData;
                        }
                        break;
                    case FileParserState.InData:
                        if (line.StartsWith(@"	[""sources""] = {"))
                        {
                            //System.Console.WriteLine("Now in sources.");
                            state = FileParserState.InSources;
                        }
                        break;
                    case FileParserState.InSources:
                        if (line.StartsWith(@"		{"))
                        {
                            //System.Console.WriteLine("Now in source.");
                            currentSource = new DataSource();
                            state = FileParserState.InSource;
                        }
                        break;
                    case FileParserState.InSource:
                        if (line.StartsWith(@"		}"))
                        {
                            //System.Console.WriteLine("Done with source.");
                            dataSources.Add(currentSource);

                            state = FileParserState.InSources;
                            break;
                        }

                        Match match;
                        string text;

                        // Data version
                        match = simpleRegex.Match(line);
                        text = match.Groups[1].Value;
                        currentSource.DataVersion = int.Parse(text);
                        if (currentSource.DataVersion != 6)
                        {
                            state = FileParserState.RunOutSource;
                            //System.Console.WriteLine("Found a source with an old data version.  Skipping.");
                            break;
                        }

                        // Build
                        var nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.Build = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.Time = double.Parse(text);

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.PlayerName = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.RealmName = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.PlayerRace = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.PlayerSex = int.Parse(text);

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.PlayerClass = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.PlayerLevel = int.Parse(text);

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.InParty = string.Equals(text, "1");

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.inRaid = bool.Parse(text);

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.ZoneName = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.SubZoneName = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.PlayerSpecialization = int.Parse(text);

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.GuildName = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.GuildLevel = int.Parse(text);

                        nextLine = reader.ReadLine();
                        while((nextLine = reader.ReadLine()) != null)
                        {
                            if (nextLine.StartsWith(@"			}"))
                            {
                                break;
                            }

                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            var quest = new Quest() { QuestId = int.Parse(text) };
                            currentSource.Quests.Add(quest);
                        }

                        // skip currencies
                        nextLine = reader.ReadLine();
                        nextLine = reader.ReadLine();

                        // items
                        nextLine = reader.ReadLine();
                        while ((nextLine = reader.ReadLine()) != null)
                        {
                            if (nextLine.StartsWith(@"			}"))
                            {
                                break;
                            }

                            var item = new Item();

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            item.ItemLink = text;

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            if (!string.Equals(text, "nil"))
                            {
                                item.QuestId = int.Parse(text);
                            }

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            item.Count = int.Parse(text);

                            currentSource.Items.Add(item);
                            nextLine = reader.ReadLine();
                        }

                        // professions
                        nextLine = reader.ReadLine();
                        while ((nextLine = reader.ReadLine()) != null)
                        {
                            if (nextLine.StartsWith(@"			}"))
                            {
                                break;
                            }

                            var profession = new Profession();

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            profession.Name = text;

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            profession.SkillLevel = int.Parse(text);

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            profession.MaxSkillLevel = int.Parse(text);

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            profession.SkillModifier = int.Parse(text);

                            currentSource.Professions.Add(profession);
                            nextLine = reader.ReadLine();
                        }

                        // auras
                        nextLine = reader.ReadLine();
                        while ((nextLine = reader.ReadLine()) != null)
                        {
                            if (nextLine.StartsWith(@"			}"))
                            {
                                break;
                            }

                            var aura = new Aura();

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            aura.Name = text;

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            aura.Rank = text;

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            aura.Count = int.Parse(text);

                            nextLine = reader.ReadLine();
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            aura.SpellId = int.Parse(text);

                            nextLine = reader.ReadLine();

                            if (nextLine.Contains(@"[5]") && !nextLine.Contains(@"}"))
                            {
                                match = simpleRegex.Match(nextLine);
                                text = match.Groups[1].Value;
                                aura.Caster = text;

                                nextLine = reader.ReadLine();
                            }

                            currentSource.Auras.Add(aura);
                        }

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.Guid = text;

                        nextLine = reader.ReadLine();
                        match = simpleRegex.Match(nextLine);
                        text = match.Groups[1].Value;
                        currentSource.SourceName = text;

                        nextLine = reader.ReadLine();
                        if (nextLine.Contains(@"[24]"))
                        {
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                currentSource.SourceLevel = int.Parse(text);
                            }

                            nextLine = reader.ReadLine();
                        }

                        if (nextLine.Contains(@"[25]"))
                        {
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            currentSource.SourceClass = text;

                            nextLine = reader.ReadLine();
                        }

                        if (nextLine.Contains(@"[26]"))
                        {
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            if (!string.Equals(text, "nil") && !string.IsNullOrWhiteSpace(text))
                            {
                                currentSource.SourceRace = int.Parse(text);
                            }

                            nextLine = reader.ReadLine();
                        }

                        if (nextLine.Contains(@"[27]"))
                        {
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            if (!string.Equals(text, "nil") && !string.IsNullOrWhiteSpace(text))
                            {
                                currentSource.SourceSex = int.Parse(text);
                            }

                            nextLine = reader.ReadLine();
                        }

                        if (nextLine.Contains(@"[28]"))
                        {
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            currentSource.SourceClassification = text;

                            nextLine = reader.ReadLine();
                        }

                        if (nextLine.Contains(@"[29]"))
                        {
                            match = simpleRegex.Match(nextLine);
                            text = match.Groups[1].Value;
                            currentSource.SourceCreatureFamily = text;

                            nextLine = reader.ReadLine();
                        }

                        while (true)
                        {
                            if (nextLine.StartsWith(@"		}"))
                            {
                                line = nextLine;
                                break;
                            }

                            if (nextLine.Contains(@"[30]"))
                            {
                                match = simpleRegex.Match(nextLine);
                                text = match.Groups[1].Value;
                                currentSource.SourceCreatureType = text;

                                nextLine = reader.ReadLine();
                            }
                            else if (nextLine.Contains(@"[32]"))
                            {
                                match = reverseRegex.Match(nextLine);
                                text = match.Groups[2].Value;
                                currentSource.LootType = text;

                                nextLine = reader.ReadLine();
                            }
                            // loot
                            else if (nextLine.Contains(@"[33]"))
                            {
                                while ((nextLine = reader.ReadLine()) != null)
                                {
                                    if (nextLine.StartsWith(@"			}"))
                                    {
                                        nextLine = reader.ReadLine();
                                        break;
                                    }

                                    var loot = new Loot();

                                    while ((nextLine = reader.ReadLine()) != null)
                                    {
                                        if (nextLine.StartsWith(@"				}"))
                                        {
                                            break;
                                        }

                                        match = reverseRegex.Match(nextLine);
                                        string category = match.Groups[1].Value;
                                        text = match.Groups[2].Value;

                                        if (string.Equals(category, "itemLink"))
                                        {
                                            loot.ItemLink = text;
                                        }
                                        else if (string.Equals(category, "quantity"))
                                        {
                                            loot.Quantity = int.Parse(text);
                                        }
                                        else if (string.Equals(category, "isCoin"))
                                        {
                                            loot.IsCoin = bool.Parse(text);
                                        }
                                        else if (string.Equals(category, "looted"))
                                        {

                                        }
                                    }

                                    // looted
                                    //nextLine = reader.ReadLine();

                                    currentSource.Loot.Add(loot);
                                    //nextLine = reader.ReadLine();
                                }
                            }
                        }

                        //System.Console.WriteLine("Done with source.");
                        dataSources.Add(currentSource);
                        state = FileParserState.InSources;

                        break;
                    case FileParserState.RunOutSource:
                        if (line.StartsWith(@"		}"))
                        {
                            //System.Console.WriteLine("Done with source.");
                            state = FileParserState.InSources;
                            break;
                        }
                        break;
                }
            }

            return dataSources;
        }
    }
}
