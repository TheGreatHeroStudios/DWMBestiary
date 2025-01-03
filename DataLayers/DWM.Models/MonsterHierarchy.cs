using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.Models
{
	public class MonsterHierarchy
	{
		public int HierarchyLevel { get; set; }
		public int NodeOrdinal { get; set; }
		public string RecordType { get; set; }
		public string? ParentHierarchyKey { get; set; }
		public string HierarchyKey { get; set; }
		public int OffspringMonsterId { get; set; }
		public string OffspringMonsterName { get; set; }
		public string PedigreeType { get; set; }
		public int PedigreeId { get; set; }
		public string PedigreeName { get; set; }
		public string PartnerType { get; set; }
		public int PartnerId { get; set; }
		public string PartnerName { get; set; }
	}
}
