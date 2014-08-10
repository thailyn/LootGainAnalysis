using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LootGainLib
{
    public enum LinkType
    {
        Unknown,
        Item,
        Currency,
    }

    public class ItemInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public LinkType LinkType { get; set; }

        public static ItemInfo ParseItemString(string itemString)
        {
            if (string.IsNullOrWhiteSpace(itemString))
            {
                throw new ArgumentNullException();
            }

            if (itemString == "Coin")
            {
                return new ItemInfo
                {
                    Id = 0,
                    Name = "Coin",
                    LinkType = LinkType.Item,
                };
            }

            Regex itemStringRegex = new Regex(@"\|?c?f?f?([0-9a-fA-F]*)\|?H?([^:]*):?(\d+):?(\d*):?(\d*):?(\d*):?(\d*):?(\d*):?(\-?\d*):?(\-?\d*):?(\d*):?(\d*):?(\-?\d*)\|?h?\[?([^\[\]]*)\]?\|?h?\|?r?");
            var match = itemStringRegex.Match(itemString);

            var itemInfo = new ItemInfo
            {
                Id = int.Parse(match.Groups[3].Value),
                Name = match.Groups[14].Value,
                LinkType = ParseLinkType(match.Groups[2].Value),
            };

            return itemInfo;
        }

        public static LinkType ParseLinkType(string linkTypeString)
        {
            if (string.IsNullOrWhiteSpace(linkTypeString))
            {
                return LinkType.Unknown;
            }

            if (string.Equals(linkTypeString, "item", StringComparison.CurrentCultureIgnoreCase))
            {
                return LinkType.Item;
            }

            if (string.Equals(linkTypeString, "currency", StringComparison.CurrentCultureIgnoreCase))
            {
                return LinkType.Currency;
            }

            throw new Exception();
        }
    }
}
