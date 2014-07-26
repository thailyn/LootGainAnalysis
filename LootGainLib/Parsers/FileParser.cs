using LootGainLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Parsers
{
    public enum FileParserState
    {
        Begin,
        InData,
        InSources,
        InSource,
    }

    public class FileParser
    {
        public FileParser()
        {

        }

        public List<DataSource> Parse(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException();
            }

            if (!System.IO.File.Exists(fileName))
            {
                throw new InvalidOperationException(string.Format("File '{0}' does not exist.", fileName));
            }

            var dataSources = new List<DataSource>();
            DataSource currentSource = null;

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
                        if (line.Contains(@"LootGain_Data = {"))
                        {
                            System.Console.WriteLine("Now in data.");
                            state = FileParserState.InData;
                        }
                        break;
                    case FileParserState.InData:
                        if (line.Contains(@"	[""sources""] = {"))
                        {
                            System.Console.WriteLine("Now in sources.");
                            state = FileParserState.InSources;
                        }
                        break;
                    case FileParserState.InSources:
                        if (line.Contains(@"		{"))
                        {
                            System.Console.WriteLine("Now in source.");
                            state = FileParserState.InSource;
                        }
                        break;
                    case FileParserState.InSource:
                        if (line.Contains(@"		}"))
                        {
                            System.Console.WriteLine("Done with source.");
                            state = FileParserState.InSources;
                        }
                        break;
                }
            }

            return dataSources;
        }
    }
}
