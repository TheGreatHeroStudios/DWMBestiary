using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWM.DataTransferObjects
{
	public class MonsterFamilyTreeNode
	{
		public string? ParentNodePath { get; set; }
		public string? NodePath { get; set; }
		public string NodeType { get; set; }
		public int NodeLevel { get; set; }
		public int NodeOrdinal { get; set; }
		public string? NodeName { get; set; }
		public string? PedigreeName { get; set; }
		public string? PedigreeType { get; set; }
		public MonsterFamilyTreeNode? PedigreeNode { get; set; }
		public string? PartnerName { get; set; }
		public string? PartnerType { get; set; }
		public MonsterFamilyTreeNode? PartnerNode { get; set; }
	}


	public class MonsterFamilyTree
	{
		public int MaxHierarchyLevel { get; set; }
		public MonsterFamilyTreeNode RootNode { get; set; }
	}
}
