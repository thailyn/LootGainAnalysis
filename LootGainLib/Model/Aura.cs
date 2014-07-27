using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Aura
    {
        public string Name { get; set; }
        public string Rank { get; set; } // string because "rank" can be something like "Special Ability"
        public int? Count { get; set; }
        public int? SpellId { get; set; }
        public string Caster { get; set; }
    }
}
