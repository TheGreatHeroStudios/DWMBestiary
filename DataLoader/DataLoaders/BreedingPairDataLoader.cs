using DWM.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGH.Common;
using TGH.Common.Utilities.DataLoader.Implementations;
using TGH.Common.Extensions;
using DWM.Models.Enums;

namespace DWM.DataLoader.DataLoaders
{
	public class BreedingPairDataLoader : DataLoader<BreedingPair>
	{
		#region File-Specific Constant(s)
		private static readonly string BREEDING_DATA_FILEPATH =
			$@"{FileConstants.EXECUTABLE_DIRECTORY}\SourceFiles\DWMBreeding.html";
		#endregion



		#region Non-Public Member(s)
		private IEnumerable<Monster> _monsters;
		#endregion



		#region Constructor(s)
		public BreedingPairDataLoader(IEnumerable<Monster> monsters)
		{
			_monsters = monsters;
		}
		#endregion



		#region Abstract Implementation
		public override Func<BreedingPair, int> KeySelector => breedingPair => breedingPair.BreedingPairId;


		public override IEnumerable<BreedingPair> ReadDataIntoMemory()
		{
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(File.ReadAllText(BREEDING_DATA_FILEPATH));

			Monster currentOffspringMonster = null;
			int pedigreeDataIndex;
			int partnerDataIndex;

			return
				document
					.DocumentNode
					.Descendants("tbody")
					.SelectMany(table => table.Descendants("tr").Skip(1))
					.SelectMany
					(
						breedingPairRow =>
						{
							IEnumerable<HtmlNode> dataNodes = breedingPairRow.Descendants("td");
							List<BreedingPair> availablePairs = new List<BreedingPair>();

							if (dataNodes.Count() >= 4)
							{
								//Rows with more than 3 data nodes contain both breeding pair data and offspring data.
								currentOffspringMonster =
									_monsters.FindByName(dataNodes.ElementAt(1).InnerText);

								pedigreeDataIndex = 2;
								partnerDataIndex = 3;
							}
							else
							{
								//Rows with fewer than 4 data nodes contain alternate breeding pairs for the last offspring. 
								pedigreeDataIndex = 0;
								partnerDataIndex = 1;
							}

							IEnumerable<(string Pedigree, string Partner)> breedingMatches =
									dataNodes
										.ElementAt(pedigreeDataIndex)
										.Descendants("a")
										.Select(pedigreeNode => pedigreeNode.InnerText)
										.CrossJoin
										(
											dataNodes
												.ElementAt(partnerDataIndex)
												.Descendants("a")
												.Select(partnerNode => partnerNode.InnerText)
										);

							return
								breedingMatches
									.Select
									(
										breedingMatch =>
										{
											Family? pedigreeFamily =
												breedingMatch.Pedigree.StartsWith("[") ?
													Enum.Parse<Family>
														(
															breedingMatch
																.Pedigree
																.Substring
																(
																	1,
																	breedingMatch.Pedigree.Length - 2
																),
															true
														) :
														null;

											Monster? pedigreeMonster =
												breedingMatch.Pedigree.StartsWith("[") ?
													null :
													_monsters.FindByName(breedingMatch.Pedigree);

											Family? partnerFamily =
												breedingMatch.Partner.StartsWith("[") ?
													Enum.Parse<Family>
														(
															breedingMatch
																.Partner
																.Substring
																(
																	1,
																	breedingMatch.Partner.Length - 2
																),
															true
														) :
														null;

											Monster? partnerMonster =
												breedingMatch.Partner.StartsWith("[") ?
													null :
													_monsters.FindByName(breedingMatch.Partner);

											return
												new BreedingPair
												{
													OffspringMonster = currentOffspringMonster,
													PedigreeFamilyId = pedigreeFamily,
													PedigreeMonster = pedigreeMonster,
													PartnerFamilyId = partnerFamily,
													PartnerMonster = partnerMonster
												};
										}
									);
						}
					)
					.ToList();
		}
		#endregion
	}
}
