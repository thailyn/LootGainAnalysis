using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Loot
    {
        bool? IsCoin { get; set; }
        int? Quantity { get; set; }
        string ItemLink { get; set; }
    }
}
