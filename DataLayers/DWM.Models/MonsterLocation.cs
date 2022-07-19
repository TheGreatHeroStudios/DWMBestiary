using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class MonsterLocation
	{
		#region Public Propert(ies)
		public int MonsterLocationId { get; set; }
		public int MonsterId { get; set; }
		public string TravelersGateName { get; set; }
		public int MinLevelEncountered { get; set; }
		public int MaxLevelEncountered { get; set; }
		#endregion



		#region Navigation Propert(ies)
		public Monster Monster { get; set; }
		#endregion
	}
}
