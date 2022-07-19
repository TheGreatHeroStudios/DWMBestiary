using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class SkillUpgrade
	{
		#region Public Propert(ies)
		public int SkillUpgradeId { get; set; }
		public int BaseSkillId { get; set; }
		public int UpgradedSkillId { get; set; }
		#endregion


		#region Navigation Propert(ies)
		public Skill UpgradedSkill { get; set; }
		public Skill BaseSkill { get; set; }
		#endregion
	}
}
