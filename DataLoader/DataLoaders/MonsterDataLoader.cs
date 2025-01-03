using DWM.Models;
using DWM.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGH.Common;
using TGH.Common.Utilities.DataLoader.Implementations;
using TGH.Common.Utilities.Logging;

namespace DWM.DataLoader.DataLoaders
{
	public class MonsterDataLoader : DataLoader<Monster>
	{
		#region File-Specific Constant(s)
		//Token(s)
		private const int MONSTER_COUNT = 215;
		private const int FILE_OFFSET_RECRUITMENT_LEVELS = 62;
		private const int FILE_OFFSET_SKILL_RESISTANCES = 231;
		private const int OFFENSIVE_SKILL_ENUM_OFFSET = 65;

		private static readonly Stat[] STAT_GROWTH_CATEGORIES =
			new Stat[]
			{
				Stat.Health,
				Stat.Magic,
				Stat.Attack,
				Stat.Defense,
				Stat.Agility,
				Stat.Intelligence
			};

		//File Path(s)
		private static readonly string RECRUITMENT_LEVELS_FILEPATH =
			$@"{FileConstants.EXECUTABLE_DIRECTORY}\SourceFiles\RecruitmentLevels.txt";

		private static readonly string MONSTER_STATS_FILEPATH =
			$@"{FileConstants.EXECUTABLE_DIRECTORY}\SourceFiles\MonsterStatistics.txt";

		//Message(s)
		private const string INFO_MONSTER_LOADING_SKIPPED =
			"Monster data has already been loaded into the target database.  Skipping...";

		private const string INFO_BEGIN_RECRUITMENT_LEVEL_LOADING = "Loading recruitment levels...";
		private const string INFO_BEGIN_MONSTER_LOADING = "Loading monster data...";
		#endregion



		#region Non-Public Member(s)
		private IEnumerable<Skill> _skills;
		private IEnumerable<GrowthCategory> _growthCategories;
		private Dictionary<string, int> _wildnessLevels = new Dictionary<string, int>();
		private Dictionary<string, List<SkillResistance>> _skillResistances = new Dictionary<string, List<SkillResistance>>();
		#endregion



		#region Constructor(s)
		public MonsterDataLoader(IEnumerable<Skill> skills, IEnumerable<GrowthCategory> growthCategories)
		{
			_skills = skills;
			_growthCategories = growthCategories;
		}
		#endregion



		#region Abstract Implementation
		public override Func<Monster, int> KeySelector => monster => monster.MonsterId;


		public override IEnumerable<Monster> ReadDataIntoMemory()
		{
			Logger.LogInfo(INFO_BEGIN_RECRUITMENT_LEVEL_LOADING);

			ParseRecruitmentLevels();
			ParseSkillResistances();

			Logger.LogInfo(INFO_BEGIN_MONSTER_LOADING);

			return ParseMonsterData();
			
		}
		#endregion



		#region Override(s)
		public override int ExpectedRecordCount => MONSTER_COUNT;
		#endregion



		#region Non-Public Method(s)
		private List<Monster> ParseMonsterData()
		{
			List<Monster> monsters = new List<Monster>();

			string[] allMonsterData = File.ReadAllLines(MONSTER_STATS_FILEPATH);

			foreach
			(
				string currentMonsterData in
				allMonsterData
					.Skip(7)
					.Take(MONSTER_COUNT)
			)
			{
				Monster currentMonster = new Monster();
				string[] monsterDataPoints = currentMonsterData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				currentMonster.MonsterName = monsterDataPoints[0];
				currentMonster.FamilyId = Enum.Parse<Family>(monsterDataPoints[1]);
				currentMonster.MaxLevel = int.Parse(monsterDataPoints[2]);

				if (_wildnessLevels.ContainsKey(currentMonster.MonsterName))
				{
					currentMonster.WildnessLevel = _wildnessLevels[currentMonster.MonsterName];
				}
				else
				{
					//If no wildness level is provided for a given monster,
					//it are considered 'non-recruitable' (and must be bred).
					currentMonster.WildnessLevel = 7;
				}

				if(_skillResistances.ContainsKey(currentMonster.MonsterName))
				{
					currentMonster.SkillResistances = _skillResistances[currentMonster.MonsterName];
				}

				currentMonster.GenderProbabilityId = Enum.Parse<GenderProbability>(monsterDataPoints[4]);
				currentMonster.IsFlying = monsterDataPoints[5] == "1";
				currentMonster.IsMetal = monsterDataPoints[6] == "1";
				currentMonster.StatGrowth = new List<MonsterGrowth>();
				currentMonster.NaturalSkills = new List<NaturalSkill>();

				GrowthCategory xpGrowthCategory = _growthCategories.ElementAt(int.Parse(monsterDataPoints[3]));
				
					MonsterGrowth xpGrowth =
					new MonsterGrowth
					{
						StatId = Stat.Experience,
						GrowthCategoryId = xpGrowthCategory.GrowthCategoryId,
						GrowthCategory = xpGrowthCategory,
						Monster = currentMonster
					};

				currentMonster.StatGrowth.Add(xpGrowth);

				for (int i = 7; i <= 12; i++)
				{
					GrowthCategory currentStatGrowthCategory = _growthCategories.ElementAt(int.Parse(monsterDataPoints[i]));
					MonsterGrowth statGrowth =
						new MonsterGrowth
						{ 
							StatId = STAT_GROWTH_CATEGORIES[i - 7],
							GrowthCategoryId = currentStatGrowthCategory.GrowthCategoryId,
							GrowthCategory = currentStatGrowthCategory,
							Monster = currentMonster
						};

					currentMonster.StatGrowth.Add(statGrowth);
				}

				for (int i = 13; i <= 15; i++)
				{
					NaturalSkill naturalSkill =
						new NaturalSkill
						{
							Skill = _skills.Single(skill => skill.SkillName.Equals(monsterDataPoints[i]))
						};

					currentMonster.NaturalSkills.Add(naturalSkill);
				}

				monsters.Add(currentMonster);
			}

			return monsters;
		}
		
		
		private void ParseRecruitmentLevels()
		{
			int currentWildnessLevel = 0;

			foreach
			(
				string recruitmentLevelData in 
				File
					.ReadAllLines(RECRUITMENT_LEVELS_FILEPATH)
					.Skip(FILE_OFFSET_RECRUITMENT_LEVELS)
					.Select(data => data.Trim())
			)
			{
				if(recruitmentLevelData == string.Empty || recruitmentLevelData.StartsWith("WLD"))
				{
					//Skip over header and blank rows
					continue;
				}
				else if(recruitmentLevelData.StartsWith("---"))
				{
					//Increment the wildness level just before parsing a new set of monsters
					currentWildnessLevel++;
					continue;
				}
				else
				{
					_wildnessLevels.Add(recruitmentLevelData, currentWildnessLevel);
				}
			}
		}


		private void ParseSkillResistances()
		{
			foreach
			(
				string skillResistanceData in
				File
					.ReadAllLines(MONSTER_STATS_FILEPATH)
					.Skip(FILE_OFFSET_SKILL_RESISTANCES)
			)
			{
				string[] skillResistanceDataPoints = 
					skillResistanceData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				int currentResistanceIndex = 0;

				List<SkillResistance> skillResistances =
					skillResistanceDataPoints
						.Skip(1)
						.Select
						(
							skillResistanceDataPoint =>
							{
								SkillResistance skillResistance =
									new SkillResistance
									{
										SkillClassificationId =
											(SkillClassification)(currentResistanceIndex + OFFENSIVE_SKILL_ENUM_OFFSET),
										ResistanceLevel = int.Parse(skillResistanceDataPoint)
									};

								currentResistanceIndex++;

								return skillResistance;
							}
						)
						.ToList();

				_skillResistances.Add(skillResistanceDataPoints[0], skillResistances);
			}
		}
		#endregion
	}
}
