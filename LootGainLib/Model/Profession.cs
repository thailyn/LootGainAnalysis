using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootGainLib.Model
{
    public class Profession
    {
        string Name { get; set; }
        int? SkillLevel { get; set; }
        int? MaxSkillLevel { get; set; }
        int? SkillModifier { get; set; }
    }
}
