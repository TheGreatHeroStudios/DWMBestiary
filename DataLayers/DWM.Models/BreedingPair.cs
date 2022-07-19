using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class BreedingPair
	{
		#region Public Propert(ies)
		public int BreedingPairId { get; set; }
		public int OffspringMonsterId { get; set; }
		public int? OffspringPlusRequirement { get; set; }
		public Family? PedigreeFamilyId { get; set; }
		public int? PedigreeMonsterId { get; set; }
		public int? PedigreePlusRequirement { get; set; }
		public Family? PartnerFamilyId { get; set; }
		public int? PartnerMonsterId { get; set; }
		#endregion



		#region Navigation Propert(ies)
		public Monster OffspringMonster { get; set; }
		public Monster PedigreeMonster { get; set; }
		public Monster PartnerMonster { get; set; }
		#endregion
	}
}
