using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Item
    {
        public string ItemLink { get; set; }
        public int? QuestId { get; set; }
        public int Count { get; set; }
    }
}
