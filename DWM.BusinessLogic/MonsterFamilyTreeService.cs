using DWM.DataTransferObjects;
using DWM.Models;
using DWM.Models.Enums;
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
		//private const string PROC_NAME = "sprMonsterHierarchy";
		private IGenericRepository _repo;
		private IEnumerable<MonsterHierarchy> _monsterBreedingOptions;


		public MonsterFamilyTreeService(IGenericRepository repo)
		{
			_repo = repo;
			_monsterBreedingOptions = BuildBreedingPairOptions();
		}


		public MonsterFamilyTree? GetMonsterFamilyTree(string targetMonsterName, int? maxHierarchyLevels = null)
		{
			int validMonsterCount = 
				_repo
					.GetRecordCount<Monster>
					(
						m => m.MonsterName.Equals(targetMonsterName, StringComparison.InvariantCultureIgnoreCase)
					);

			if (validMonsterCount <= 0)
			{
				//If the monster name provided is not valid, do nothing.
				return null;
			}

			List<MonsterHierarchy> hierarchyRecords = BuildMonsterHierarchy(targetMonsterName, maxHierarchyLevels ?? 999);
				/*_repo
					.RetrieveEntities<MonsterHierarchy>
					(
						PROC_NAME,
						targetMonsterName
					)
					.AsEnumerable();*/

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

		
		private IEnumerable<MonsterHierarchy> BuildBreedingPairOptions()
		{
			//Get a single breeding pair for each monster as an 'offspring' (result)
			//Favor generic combos (families) over specific.
			var breedingPairs =
				_repo
					.RetrieveEntities<BreedingPair>(bp => true)
					.GroupBy(bp => bp.OffspringMonsterId)           //PARTITION BY
					.Select
					(
						prt =>
							prt
								.OrderByDescending(prt => prt.PedigreeFamilyId)
								.ThenByDescending(prt => prt.PartnerFamilyId)
								.ThenBy(prt => prt.PedigreeMonsterId)
								.ThenBy(prt => prt.PartnerMonsterId)
								.FirstOrDefault()					//Precedence = 1
					);

			var monsters = _repo.RetrieveEntities<Monster>(bp => true);

			//Get each combination of breeding pair:

			//Family Pedigree; Family Partner (Ideal Leaf Nodes)
			IEnumerable<MonsterHierarchy> familyFamilyPairs =
				breedingPairs
					.Where
					(
						bp => 
							bp?.PedigreeFamilyId != null &&
							bp?.PartnerFamilyId != null
					)
					.Join
					(
						monsters,
						bp => bp?.OffspringMonsterId,
						off => off.MonsterId,
						(bp, off) => (bp, off)
					)
					.Select
					(
						(tuple, off) =>
							new MonsterHierarchy
							{
								OffspringMonsterId = tuple.bp?.OffspringMonsterId ?? -1,
								OffspringMonsterName = tuple.off.MonsterName,
								PedigreeType = "Family",
								PedigreeId = (int)(tuple.bp?.PedigreeFamilyId ?? Family.Unknown),
								PedigreeName =
									Enum.GetName(tuple.bp?.PedigreeFamilyId ?? Family.Unknown)!,
								PartnerType = "Family",
								PartnerId = (int)(tuple.bp?.PartnerFamilyId ?? Family.Unknown),
								PartnerName =
									Enum.GetName(tuple.bp?.PartnerFamilyId ?? Family.Unknown)!
							}
					);

			//Monster Pedigree; Monster Partner
			IEnumerable<MonsterHierarchy> monsterMonsterPairs =
				breedingPairs
					.Where
					(
						bp =>
							bp?.PedigreeMonsterId != null &&
							bp?.PartnerMonsterId != null
					)
					.Join
					(
						monsters,
						bp => bp?.OffspringMonsterId,
						off => off.MonsterId,
						(bp, off) => (bp, off)
					)
					.Join
					(
						monsters,
						tuple => tuple.bp?.PedigreeMonsterId ?? -1,
						ped => ped.MonsterId,
						(tuple, ped) => (tuple.bp, tuple.off, ped)
					)
					.Join
					(
						monsters,
						tuple => tuple.bp?.PartnerMonsterId ?? -1,
						prt => prt.MonsterId,
						(tuple, prt) => (tuple.bp, tuple.off, tuple.ped, prt)
					)
					.Select
					(
						tuple =>
							new MonsterHierarchy
							{
								OffspringMonsterId = tuple.off.MonsterId,
								OffspringMonsterName = tuple.off.MonsterName,
								PedigreeType = "Monster",
								PedigreeId = tuple.ped.MonsterId,
								PedigreeName = tuple.ped.MonsterName,
								PartnerType = "Monster",
								PartnerId = tuple.prt.MonsterId,
								PartnerName = tuple.prt.MonsterName
							}
					);

			//Monster Pedigree; Family Partner
			IEnumerable<MonsterHierarchy> monsterFamilyPairs =
				breedingPairs
					.Where
					(
						bp =>
							bp?.PedigreeMonsterId != null &&
							bp?.PartnerFamilyId != null
					)
					.Join
					(
						monsters,
						bp => bp?.OffspringMonsterId,
						off => off.MonsterId,
						(bp, off) => (bp, off)
					)
					.Join
					(
						monsters,
						tuple => tuple.bp?.PedigreeMonsterId ?? -1,
						ped => ped.MonsterId,
						(tuple, ped) => (tuple.bp, tuple.off, ped)
					)
					.Select
					(
						tuple =>
							new MonsterHierarchy
							{
								OffspringMonsterId = tuple.off.MonsterId,
								OffspringMonsterName = tuple.off.MonsterName,
								PedigreeType = "Monster",
								PedigreeId = tuple.ped.MonsterId,
								PedigreeName = tuple.ped.MonsterName,
								PartnerType = "Family",
								PartnerId = (int)(tuple.bp?.PartnerFamilyId ?? Family.Unknown),
								PartnerName =
									Enum.GetName(tuple.bp?.PartnerFamilyId ?? Family.Unknown)!
							}
					);

			//Family Pedigree; Monster Partner
			IEnumerable<MonsterHierarchy> familyMonsterPairs =
				breedingPairs
					.Where
					(
						bp =>
							bp?.PedigreeFamilyId != null &&
							bp?.PartnerMonsterId != null
					)
					.Join
					(
						monsters,
						bp => bp?.OffspringMonsterId,
						off => off.MonsterId,
						(bp, off) => (bp, off)
					)
					.Join
					(
						monsters,
						tuple => tuple.bp?.PartnerMonsterId ?? -1,
						prt => prt.MonsterId,
						(tuple, prt) => (tuple.bp, tuple.off, prt)
					)
					.Select
					(
						tuple =>
							new MonsterHierarchy
							{
								OffspringMonsterId = tuple.off.MonsterId,
								OffspringMonsterName = tuple.off.MonsterName,
								PedigreeType = "Family",
								PedigreeId = (int)(tuple.bp?.PedigreeFamilyId ?? Family.Unknown),
								PedigreeName = Enum.GetName(tuple.bp?.PedigreeFamilyId ?? Family.Unknown)!,
								PartnerType = "Monster",
								PartnerId = tuple.prt.MonsterId,
								PartnerName = tuple.prt.MonsterName
							}
					);

			return 
				familyFamilyPairs
					.Union(monsterMonsterPairs)
					.Union(monsterFamilyPairs)
					.Union(familyMonsterPairs);
		}


		private List<MonsterHierarchy> BuildMonsterHierarchy(string targetMonsterName, int? maxHierarchyLevels = 999)
		{
			List<MonsterHierarchy> monsterHierarchy = new List<MonsterHierarchy>(); 

			MonsterHierarchy? rootNode =
				_monsterBreedingOptions
					.FirstOrDefault
					(
						option =>
							option.OffspringMonsterName.Equals(targetMonsterName, StringComparison.InvariantCultureIgnoreCase)
					);

			if (rootNode != null)
			{
				rootNode.HierarchyLevel = 1;
				rootNode.NodeOrdinal = 1;
				rootNode.RecordType = "Target Monster";
				rootNode.ParentHierarchyKey = null;
				rootNode.HierarchyKey = rootNode.OffspringMonsterId.ToString();

				monsterHierarchy.Add(rootNode);
				BuildParentNodes(monsterHierarchy, rootNode, maxHierarchyLevels);
			}

			return monsterHierarchy;
		}


		private void BuildParentNodes(List<MonsterHierarchy> currentTree, MonsterHierarchy offspring, int? maxHierarchyLevels = 999)
		{
			//Only continue building nodes until the pedigree and partner are both of type 'Family'
			//(or until the specified max lever of recursion has been reached)
			if (offspring.PedigreeType == "Monster" && offspring.HierarchyLevel < maxHierarchyLevels)
			{
				MonsterHierarchy? pedigree =
					_monsterBreedingOptions
						.FirstOrDefault
						(
							option =>
								option.OffspringMonsterId == offspring.PedigreeId
						);

				if (pedigree != null)
				{
					pedigree.HierarchyLevel = offspring.HierarchyLevel + 1;
					pedigree.NodeOrdinal = ((offspring.NodeOrdinal - 1) * 2) + 1;
					pedigree.RecordType = "Pedigree";
					pedigree.ParentHierarchyKey = offspring.HierarchyKey;
					pedigree.HierarchyKey = $"{offspring.HierarchyKey}/{offspring.PedigreeId}-{pedigree.NodeOrdinal}";

					currentTree.Add(pedigree);
					BuildParentNodes(currentTree, pedigree, maxHierarchyLevels);
				}
			}

			if (offspring.PartnerType == "Monster" && offspring.HierarchyLevel < maxHierarchyLevels)
			{
				MonsterHierarchy? partner =
					_monsterBreedingOptions
						.FirstOrDefault
						(
							option =>
								option.OffspringMonsterId == offspring.PartnerId
						);

				if (partner != null)
				{
					partner.HierarchyLevel = offspring.HierarchyLevel + 1;
					partner.NodeOrdinal = ((offspring.NodeOrdinal - 1) * 2) + 2;
					partner.RecordType = "Partner";
					partner.ParentHierarchyKey = offspring.HierarchyKey;
					partner.HierarchyKey = $"{offspring.HierarchyKey}/{offspring.PartnerId}-{partner.NodeOrdinal}";

					currentTree.Add(partner);
					BuildParentNodes(currentTree, partner, maxHierarchyLevels);
				}
			}
		}
	}
}
