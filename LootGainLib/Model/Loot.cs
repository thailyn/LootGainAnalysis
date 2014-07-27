using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Loot
    {
        public bool? IsCoin { get; set; }
        public int? Quantity { get; set; }
        public string ItemLink { get; set; }
    }
}
