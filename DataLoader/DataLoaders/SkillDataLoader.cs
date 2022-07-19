using DWM.Models;
using DWM.Models.Enums;
using Microsoft.Extensions.Logging;
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
	public class SkillDataLoader : DataLoader<Skill>
	{
		#region File-Specific Constant(s)
		//Token(s)
		private const int SKILL_COUNT = 153;
		private const int UPGRADE_COUNT = 28;
		private const int COMBO_COUNT = 24;

		private const int FILE_OFFSET_SKILL = 42;
		private const int FILE_OFFSET_UPGRADE = 200;
		private const int FILE_OFFSET_COMBO = 233;

		//File Path(s)
		private static readonly string SKILLS_FILEPATH =
			$@"{FileConstants.EXECUTABLE_DIRECTORY}\SourceFiles\Skills.txt";

		//Message(s)
		private const string INFO_SKILL_LOADING_SKIPPED =
			"Skill data has already been loaded into the target database.  Skipping...";

		private const string INFO_BEGIN_SKILL_LOADING = "Loading skill data...";
		#endregion



		#region Abstract Implementation
		public override Func<Skill, int> KeySelector => skill => skill.SkillId;


		public override IEnumerable<Skill> ReadDataIntoMemory()
		{
			Logger.LogInfo(INFO_BEGIN_SKILL_LOADING);

			return ParseSkillData();
		}
		#endregion



		#region Override(s)
		public override int ExpectedRecordCount => SKILL_COUNT;
		#endregion



		#region Non-Public Method(s)
		private IEnumerable<Skill> ParseSkillData()
		{
			List<Skill> skills = new List<Skill>();

			string[] allSkillData = File.ReadAllLines(SKILLS_FILEPATH);

			//Parse each line of the file to a skill object.
			foreach
			(
				string currentSkillData in
				allSkillData
					.Skip(FILE_OFFSET_SKILL)
					.Take(SKILL_COUNT)
			)
			{
				Skill currentSkill = new Skill();
				string[] skillDataPoints = currentSkillData.Split('|', StringSplitOptions.RemoveEmptyEntries);

				currentSkill.SkillName = skillDataPoints[0].Trim();
				currentSkill.SkillDescription = skillDataPoints[4].Trim();
				currentSkill.SkillClassificationId = (SkillClassification)(int)skillDataPoints[1].Trim()[0];

				ParseStatRequirements(currentSkill, skillDataPoints);
				skills.Add(currentSkill);
			}

			//Once all skills have been parsed, parse and associate combos and upgrades
			foreach
			(
				string upgradeData in
				allSkillData
					.Skip(FILE_OFFSET_UPGRADE)
					.Take(UPGRADE_COUNT)
			)
			{
				ParseUpgradeData(upgradeData.Trim(), skills);
			}

			foreach
			(
				string comboData in
				allSkillData
					.Skip(FILE_OFFSET_COMBO)
					.Take(COMBO_COUNT)
			)
			{
				ParseComboData(comboData.Trim(), skills);
			}

			return skills;
		}


		private void ParseStatRequirements(Skill currentSkill, string[] skillDataPoints)
		{
			currentSkill.StatRequirements = new List<SkillStatRequirement>();

			//Add a stat requirement for level
			currentSkill
				.StatRequirements
				.Add
				(
					new SkillStatRequirement
					{
						StatId = Stat.Level,
						RequiredValue = int.Parse(skillDataPoints[2].Trim())
					}
				);

			//Add a stat requirement for each additional required stat
			string[] statRequirements = skillDataPoints[3].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			
			for(int i = 0; i < statRequirements.Length; i += 2)
			{
				Stat statType = Stat.Unknown;
				int requiredValue = int.Parse(statRequirements[i + 1]);

				switch (statRequirements[i])
				{
					case "HPs":
					{
						statType = Stat.Health;
						break;
					}
					case "MPs":
					{
						statType = Stat.Magic;
						break;
					}
					case "ATK":
					{
						statType = Stat.Attack;
						break;
					}
					case "DEF":
					{
						statType = Stat.Defense;
						break;
					}
					case "AGL":
					{
						statType = Stat.Agility;
						break;
					}
					case "INT":
					{
						statType = Stat.Intelligence;
						break;
					}
				}

				currentSkill
					.StatRequirements
					.Add
					(
						new SkillStatRequirement
						{
							StatId = statType,
							RequiredValue = requiredValue
						}
					);
			}
		}
		
		
		private void ParseUpgradeData(string upgradeData, List<Skill> allSkills)
		{
			List<Skill> skillUpgrades = 
				upgradeData
					.Split(',')
					.Join
					(
						allSkills,
						skillName => skillName,
						skill => skill.SkillName,
						(skillName, skill) => skill
					)
					.ToList();

			
			for(int i = 0; i < skillUpgrades.Count - 1; i++)
			{
				Skill baseSkill = skillUpgrades[i];
				baseSkill.AvailableUpgrades = new List<SkillUpgrade>();

				for(int j = i + 1; j < skillUpgrades.Count; j++)
				{
					Skill upgradedSkill = skillUpgrades[j];
					SkillUpgrade skillUpgrade =
						new SkillUpgrade
						{
							UpgradedSkill = upgradedSkill
						};

					baseSkill.AvailableUpgrades.Add(skillUpgrade);
				}
			}
		}
		
		
		private void ParseComboData(string comboData, List<Skill> allSkills)
		{
			string[] comboDataPoints = comboData.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			Skill targetSkill = allSkills.Single(skill => skill.SkillName.Equals(comboDataPoints[0]));

			List<SkillComboRequirement> comboRequirements =
				comboDataPoints[1]
					.Split(',')
					.Join
					(
						allSkills,
						requiredSkillName => requiredSkillName,
						skill => skill.SkillName,
						(requiredSkillName, skill) => skill
					)
					.Select
					(
						requiredSkill =>
							new SkillComboRequirement
							{
								RequiredSkill = requiredSkill,
								TargetSkill = targetSkill
							}
					)
					.ToList();

			targetSkill.ComboRequirements = comboRequirements;
		}
		#endregion
	}
}
