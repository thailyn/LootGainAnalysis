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

            var parser = new FileParser();
            var sources = parser.Parse(args[0]);
            sources.UseAttributesWithValues = false;

            System.Console.WriteLine("Done parsing.  Parsed {0} data sources.", sources.Count);

            var attributeValues = new AttributeValues();
            attributeValues.FindValues(sources);
            System.Console.WriteLine("Done finding attribute values.");

            int itemId;
            if (args.Length < 2)
            {
                var rand = new Random();
                int index = rand.Next(attributeValues.ValuesMap[LootGainLib.Attribute.Loot].Keys.Count);
                itemId = (int)attributeValues.ValuesMap[LootGainLib.Attribute.Loot].Keys.ToList()[index];
            }
            else
            {
                itemId = int.Parse(args[1]);
            }

            var loot = from s in sources
                       from l in s.Loot
                       where !string.IsNullOrWhiteSpace(l.ItemLink)
                       where l.ItemLink.Contains("item:" + itemId.ToString() + ":")
                       select l;
            var singleLoot = loot.FirstOrDefault();

            if (singleLoot == null)
            {
                System.Console.WriteLine("No known item with item id '{0}'.", itemId);
                return;
            }

            var item = ItemInfo.ParseItemString(singleLoot.ItemLink);
            System.Console.WriteLine("Item id: {0}, Item Name: {1}", item.Id, item.Name);
            //itemId = 6303;
            //itemId = 45191;
            //itemId = 82261;

            //int itemId = int.Parse(args[1]);
            var entropy = sources.EntropyOnItemId(item.Id);
            System.Console.WriteLine("Base entropy: {0}", entropy);

            /*
            var informationGain = sources.InformationGainOnItemId(itemId, LootGainLib.Attribute.SourceName, null,
                attributeValues.ValuesMap[LootGainLib.Attribute.SourceName]);
            System.Console.WriteLine("Information gain on source name: {0}", informationGain);

            informationGain = sources.InformationGainOnItemId(itemId, LootGainLib.Attribute.ZoneName, null,
                attributeValues.ValuesMap[LootGainLib.Attribute.ZoneName]);
            System.Console.WriteLine("Information gain on zone name: {0}", informationGain);
             * */

            /*
            LootGainLib.Attribute bestAttribute;
            object bestAttributeValue;
            double bestInformationGain = sources.FindGreatestInformationGain(itemId, attributeValues,
                out bestAttribute, out bestAttributeValue);
            System.Console.WriteLine("Completed finding greatest information gain.  Attribute {0} with value {1} at {2}.",
                bestAttribute.ToString(), bestAttributeValue, bestInformationGain);
             * */

            DecisionTreeNode rootNode = new DecisionTreeNode()
            {
                Sources = sources,
                ItemId = itemId,
            };
            rootNode.CreateChildrenOnItemId(itemId, attributeValues);
            rootNode.ConsolePrint(string.Empty);

            System.Console.ReadLine();
        }
    }
}
