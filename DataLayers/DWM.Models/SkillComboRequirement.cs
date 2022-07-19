using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class SkillComboRequirement
	{
		#region Public Propert(ies)
		public int SkillComboRequirementId { get; set; }
		public int TargetSkillId { get; set; }
		public int RequiredSkillId { get; set; }
		#endregion



		#region Navigation Propert(ies)
		public Skill RequiredSkill { get; set; }
		public Skill TargetSkill { get; set; }
		#endregion
	}
}
