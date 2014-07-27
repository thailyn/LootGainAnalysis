using LootGainLib;
using LootGainLib.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine("Must provide a file name as an argument.");
                System.Console.ReadLine();
                return;
            }

            if (args.Length < 2)
            {
                System.Console.WriteLine("Must provide an item id as an argument.");
                System.Console.ReadLine();
                return;
            }

            var parser = new FileParser();
            var sources = parser.Parse(args[0]);

            System.Console.WriteLine("Done parsing.  Parsed {0} data sources.", sources.Count);

            var attributeValues = new AttributeValues();
            attributeValues.FindValues(sources);
            System.Console.WriteLine("Done finding attribute values.");

            int itemId = int.Parse(args[1]);
            var entropy = sources.EntropyOnItemId(itemId);
            System.Console.WriteLine("Base entropy: {0}", entropy);

            /*
            var informationGain = sources.InformationGainOnItemId(itemId, LootGainLib.Attribute.SourceName, null,
                attributeValues.ValuesMap[LootGainLib.Attribute.SourceName]);
            System.Console.WriteLine("Information gain on source name: {0}", informationGain);

            informationGain = sources.InformationGainOnItemId(itemId, LootGainLib.Attribute.ZoneName, null,
                attributeValues.ValuesMap[LootGainLib.Attribute.ZoneName]);
            System.Console.WriteLine("Information gain on zone name: {0}", informationGain);
             * */

            foreach (var attribute in Enum.GetValues(typeof(LootGainLib.Attribute)).Cast<LootGainLib.Attribute>())
            {
                switch (attribute)
                {
                    case LootGainLib.Attribute.Quest:
                        foreach (int quest in attributeValues.ValuesMap[LootGainLib.Attribute.Quest].Keys)
                        {
                            var questInformationGain = sources.InformationGainOnItemId(itemId, attribute, quest,
                                attributeValues.ValuesMap[attribute]);
                            System.Console.WriteLine("Information gain on quest {0}: {1}", quest,
                                questInformationGain);
                        }
                        break;
                    case LootGainLib.Attribute.Loot:
                        break;
                    default:
                        var loopInformationGain = sources.InformationGainOnItemId(itemId, attribute, null,
                            attributeValues.ValuesMap[attribute]);
                        System.Console.WriteLine("Information gain on {0}: {1}", attribute.ToString(),
                            loopInformationGain);
                        break;
                }
            }

            System.Console.ReadLine();
        }
    }
}
