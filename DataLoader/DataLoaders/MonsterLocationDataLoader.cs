using DWM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TGH.Common;
using TGH.Common.Utilities.DataLoader.Implementations;

namespace DWM.DataLoader.DataLoaders
{
	public class MonsterLocationDataLoader : DataLoader<MonsterLocation>
	{
		#region File-Specific Constant(s)
		//Token(s)
		private const int FILE_OFFSET_TRAVELERS_GATE = 3;
		private const int FILE_OFFSET_FOREIGN_MASTERS = 213;
		private const int LINE_COUNT_TRAVELERS_GATE = 205;
		private const int LINE_COUNT_FOREIGN_MASTERS = 47;

		//File Path(s)
		private static readonly string MONSTER_LOCATIONS_FILEPATH =
			$@"{FileConstants.EXECUTABLE_DIRECTORY}\SourceFiles\MonsterLocations.txt";
		#endregion



		#region Non-Public Member(s)
		private IEnumerable<Monster> _monsters;
		#endregion



		#region Constructor(s)
		public MonsterLocationDataLoader(IEnumerable<Monster> monsters)
		{
			_monsters = monsters;
		}
		#endregion



		#region Abstract Implementation
		public override Func<MonsterLocation, int> KeySelector => monsterLocation => monsterLocation.MonsterLocationId;


		public override IEnumerable<MonsterLocation> ReadDataIntoMemory()
		{
			List<MonsterLocation> monsterLocations = new List<MonsterLocation>();

			string[] monsterLocationData = File.ReadAllLines(MONSTER_LOCATIONS_FILEPATH);

			monsterLocations
				.AddRange
				(
					ParseTravelersGateMonsters
					(
						monsterLocationData
							.Skip(FILE_OFFSET_TRAVELERS_GATE)
							.Take(LINE_COUNT_TRAVELERS_GATE)
							.Select(line => line.Trim())
					)
				);

			monsterLocations
				.AddRange
				(
					ParseForeignMasterMonsters
					(
						monsterLocationData
							.Skip(FILE_OFFSET_FOREIGN_MASTERS)
							.Take(LINE_COUNT_FOREIGN_MASTERS)
							.Select(line => line.Trim())
					)
				);

			return monsterLocations;
		}
		#endregion



		#region Non-Public Method(s)
		private List<MonsterLocation> ParseTravelersGateMonsters(IEnumerable<string> travelersGateData)
		{
			List<MonsterLocation> travelersGateMonsters = new List<MonsterLocation>();
			string currentTravelersGate = string.Empty;

			foreach(string dataLine in travelersGateData)
			{
				if(Regex.IsMatch(dataLine, "^([A-z])"))
				{
					//Lines starting with text represent the start of a new traveler's gate (Specifically, its name).
					currentTravelersGate = dataLine.Substring(0, dataLine.IndexOf(' '));
				}
				else if (Regex.IsMatch(dataLine, "^([0-9])"))
				{
					//Lines starting with a number represent levels within a traveler's gate (containing monsters).
					string[] locationDataPoints = 
						dataLine.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

					int[] levels = 
						locationDataPoints[0]
							.Split('-', StringSplitOptions.RemoveEmptyEntries)
							.Select(levelText => int.Parse(levelText))
							.ToArray();

					foreach(string monsterName in locationDataPoints.Skip(1))
					{
						travelersGateMonsters
							.Add
							(
								new MonsterLocation
								{
									TravelersGateName = currentTravelersGate,
									MinLevelEncountered = levels[0],
									MaxLevelEncountered = levels.Length > 1 ? levels[1] : levels[0],
									Monster = _monsters.FindByName(monsterName)
								}
							);
					}
				}
			}

			return travelersGateMonsters;
		}


		private List<MonsterLocation> ParseForeignMasterMonsters(IEnumerable<string> foreignMasterData)
		{
			List<MonsterLocation> foreignMasterMonsters = new List<MonsterLocation>();
			int[] currentLevels = new int[] {-1, -1};

			foreach (string dataLine in foreignMasterData)
			{
				if(dataLine.StartsWith("Team Levels"))
				{
					currentLevels =
						dataLine
							.Substring(12)
							.Split(new[] { '-', '+' }, StringSplitOptions.RemoveEmptyEntries)
							.Select(levelText => int.Parse(levelText))
							.ToArray();
				}
				else if(Regex.IsMatch(dataLine, "^([A-z])"))
				{
					foreignMasterMonsters.AddRange
					(
						dataLine
							.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
							.Select
							(
								monsterName =>
									new MonsterLocation
									{
										TravelersGateName = "Foreign Master",
										MinLevelEncountered = currentLevels[0],
										MaxLevelEncountered = 
											currentLevels.Length > 1 ? 
												currentLevels[1] :
												currentLevels[0],
										Monster = _monsters.FindByName(monsterName)
									}
							)
					);
				}
			}

			return foreignMasterMonsters;
		}
		#endregion
	}
}
