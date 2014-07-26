using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Item
    {
        string ItemLink { get; set; }
        int? QuestId { get; set; }
        int count { get; set; }
    }
}
