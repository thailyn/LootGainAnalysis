using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Aura
    {
        string Name { get; set; }
        int? Rank { get; set; }
        int? Count { get; set; }
        int? SpellId { get; set; }
        string Caster { get; set; }
    }
}
