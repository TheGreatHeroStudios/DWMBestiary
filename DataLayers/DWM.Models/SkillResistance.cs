using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class SkillResistance
	{
		#region Public Propert(ies)
		public int SkillResistanceId { get; set; }
		public int MonsterId { get; set; }
		public SkillClassification SkillClassificationId { get; set; }
		public int ResistanceLevel { get; set; }
		#endregion
	}
}
