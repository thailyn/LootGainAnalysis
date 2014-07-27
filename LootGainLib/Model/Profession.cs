using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Profession
    {
        public string Name { get; set; }
        public int? SkillLevel { get; set; }
        public int? MaxSkillLevel { get; set; }
        public int? SkillModifier { get; set; }
    }
}
