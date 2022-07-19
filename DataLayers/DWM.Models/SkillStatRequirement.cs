using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class SkillStatRequirement
	{
		#region Public Propert(ies)
		public int SkillStatRequirementId { get; set; }
		public int TargetSkillId { get; set; }
		public Stat StatId { get; set; }
		public int RequiredValue { get; set; }
		#endregion



		#region Navigation Propert(ies)
		public Skill TargetSkill { get; set; }
		#endregion
	}
}
