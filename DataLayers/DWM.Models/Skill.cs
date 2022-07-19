using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class Skill
	{
		#region Public Propert(ies)
		public int SkillId { get; set; }
		public string SkillName { get; set; }
		public string SkillDescription { get; set; }
		public SkillClassification SkillClassificationId { get; set; }
		#endregion



		#region Navigation Propert(ies)
		[InverseProperty(nameof(SkillStatRequirement.TargetSkill))]
		public List<SkillStatRequirement> StatRequirements { get; set; }

		[InverseProperty(nameof(SkillComboRequirement.TargetSkill))]
		public List<SkillComboRequirement> ComboRequirements { get; set; }

		[InverseProperty(nameof(SkillUpgrade.BaseSkill))]
		public List<SkillUpgrade> AvailableUpgrades { get; set; }
		#endregion
	}
}
