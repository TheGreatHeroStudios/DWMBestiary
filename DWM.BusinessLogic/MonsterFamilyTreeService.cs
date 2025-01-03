using DWM.DataTransferObjects;
using DWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGH.Common.Persistence.Interfaces;
using TGH.Common.Repository.Interfaces;

namespace DWM.BusinessLogic
{
	public class MonsterFamilyTreeService
	{
		private const string PROC_NAME = "sprMonsterHierarchy";
		private IGenericRepository _repo;


		public MonsterFamilyTreeService(IGenericRepository repo)
		{
			_repo = repo;
		}


		public MonsterFamilyTree? GetMonsterFamilyTree(string targetMonsterName, int? maxHierarchyLevels = null)
		{
			int validMonsterCount = _repo.GetRecordCount<Monster>(m => m.MonsterName.Equals(targetMonsterName, StringComparison.InvariantCultureIgnoreCase));

			if (validMonsterCount <= 0)
			{
				//If the monster name provided is not valid, do nothing.
				return null;
			}

			var hierarchyRecords =
				_repo
					.RetrieveEntities<MonsterHierarchy>
					(
						PROC_NAME,
						targetMonsterName
					)
					.AsEnumerable();

			if (!hierarchyRecords.Any())
			{
				//If no records were read for the target moster,
				//return a blank tree node for the target monster.
				return
					new MonsterFamilyTree
					{
						MaxHierarchyLevel = 1,
						RootNode =
							new MonsterFamilyTreeNode
							{
								NodeType = "Target Monster",
								NodeLevel = 1,
								NodeOrdinal = 1,
								NodeName = targetMonsterName
							}
					};
			}
			else
			{
				//Determine how deep the tree should be based on the max level of recursion
				int maxRecursion =
					maxHierarchyLevels == null ?
						hierarchyRecords.Max(hr => hr.HierarchyLevel) :
						Math.Min(maxHierarchyLevels.Value, hierarchyRecords.Max(hr => hr.HierarchyLevel)) + 1;

				//Project each hierarchy into a node that can be assembled
				List<MonsterFamilyTreeNode> disassembledNodes =
					hierarchyRecords
						.Where(hr => hr.HierarchyLevel < maxRecursion)
						.Select
						(
							hr =>
								new MonsterFamilyTreeNode
								{
									ParentNodePath = hr.ParentHierarchyKey,
									NodePath = hr.HierarchyKey,
									NodeType = hr.RecordType,
									NodeLevel = hr.HierarchyLevel,
									NodeOrdinal = hr.NodeOrdinal,
									NodeName = hr.OffspringMonsterName,
									PedigreeName = hr.PedigreeName,
									PedigreeType = hr.PedigreeType,
									PartnerName = hr.PartnerName,
									PartnerType = hr.PartnerType
								}
						)
						.ToList();

				

				//Iterate over the materialized nodes and assemble them into a binary tree.
				//Once the leaf nodes have been found, 'FirstOrDefault()' allows us to map
				//one final pair of nodes for the leaf node monster's pedigree and partner.
				foreach (MonsterFamilyTreeNode node in disassembledNodes)
				{
					node.PedigreeNode =
						disassembledNodes
							.FirstOrDefault
							(
								dn =>
									dn.ParentNodePath == node.NodePath &&
									dn.NodeType == "Pedigree",
								(
									node.NodeLevel + 1 > maxRecursion ?
										null :
										new MonsterFamilyTreeNode
										{
											ParentNodePath = node.NodePath,
											NodePath = null,
											NodeType = "Pedigree",
											NodeLevel = node.NodeLevel + 1,
											NodeOrdinal = ((node.NodeOrdinal - 1) * 2) + 1,
											NodeName = node.PedigreeName
										}
								)
							);

					node.PartnerNode =
						disassembledNodes
							.FirstOrDefault
							(
								dn =>
									dn.ParentNodePath == node.NodePath &&
									dn.NodeType == "Partner",
								(
									node.NodeLevel + 1 > maxRecursion ?
										null :
										new MonsterFamilyTreeNode
										{
											ParentNodePath = node.NodePath,
											NodePath = null,
											NodeType = "Partner",
											NodeLevel = node.NodeLevel + 1,
											NodeOrdinal = ((node.NodeOrdinal - 1) * 2) + 2,
											NodeName = node.PartnerName
										}
								)
							);
				}

				return
					new MonsterFamilyTree
					{
						MaxHierarchyLevel = maxRecursion + 1,
						RootNode = disassembledNodes.First()
					};
			}
		}
	}
}
