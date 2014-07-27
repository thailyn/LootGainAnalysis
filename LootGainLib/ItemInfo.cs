using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LootGainLib
{
    public class ItemInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static ItemInfo ParseItemString(string itemString)
        {
            if (string.IsNullOrWhiteSpace(itemString))
            {
                throw new ArgumentNullException();
            }

            Regex itemStringRegex = new Regex(@"\|?c?f?f?([0-9a-fA-F]*)\|?H?([^:]*):?(\d+):?(\d*):?(\d*):?(\d*):?(\d*):?(\d*):?(\-?\d*):?(\-?\d*):?(\d*):?(\d*):?(\-?\d*)\|?h?\[?([^\[\]]*)\]?\|?h?\|?r?");
            var match = itemStringRegex.Match(itemString);

            var itemInfo = new ItemInfo
            {
                Id = int.Parse(match.Groups[3].Value),
                Name = match.Groups[14].Value
            };

            return itemInfo;
        }
    }
}
