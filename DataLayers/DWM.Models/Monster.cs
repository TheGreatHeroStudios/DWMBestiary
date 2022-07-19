using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class Monster
	{
		#region Public Propert(ies)
		public int MonsterId { get; set; }
		public string MonsterName { get; set; }
		public Family FamilyId { get; set; }
		public int MaxLevel { get; set; }
		public int WildnessLevel { get; set; }
		public GenderProbability GenderProbabilityId { get; set; }
		public bool IsFlying { get; set; }
		public bool IsMetal { get; set; }
		#endregion



		#region Navigation Propert(ies)
		public List<NaturalSkill> NaturalSkills { get; set; }
		public List<MonsterGrowth> StatGrowth { get; set; }
		public List<SkillResistance> SkillResistances { get; set; }
		#endregion
	}
}
