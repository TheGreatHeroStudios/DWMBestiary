using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.DataTransferObjects
{
	public class MonsterDetails
	{
		public int MonsterId { get; set; }
		public string MonsterName { get; set; }
		public string? Family { get; set; }
		public int MaxLevel { get; set; }
		public string? GenderProbability { get; set; }
		public bool IsFlying { get; set; }
		public bool IsMetal { get; set; }
	}
}
