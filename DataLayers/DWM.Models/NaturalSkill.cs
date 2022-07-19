using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class NaturalSkill
	{ 
		#region Public Propert(ies)
		public int NaturalSkillId { get; set; }
		public int MonsterId { get; set; }
		public int SkillId { get; set; }
		#endregion



		#region Navigation Propert(ies)
		public Skill Skill { get; set; }
		#endregion
	}
}
