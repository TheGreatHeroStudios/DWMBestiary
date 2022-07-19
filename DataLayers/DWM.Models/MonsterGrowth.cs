using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class MonsterGrowth
	{
		#region Public Propert(ies)
		public int MonsterGrowthId { get; set; }
		public Stat StatId { get; set; }
		public int MonsterId { get; set; }
		public int GrowthCategoryId { get; set; }
		#endregion



		#region Navigation Propert(ies)
		public GrowthCategory GrowthCategory { get; set; }
		#endregion
	}
}
