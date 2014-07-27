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

            System.Console.WriteLine("Done parsing.  Parsed {0} data sources.", sources.Count);

            var attributeValues = new AttributeValues();
            attributeValues.FindValues(sources);
            System.Console.WriteLine("Done finding attribute values.");

            System.Console.ReadLine();
        }
    }
}
