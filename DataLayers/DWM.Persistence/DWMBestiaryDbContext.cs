using DWM.Models;
using Microsoft.EntityFrameworkCore;
using TGH.Common.Persistence.Implementations;

namespace DWM.Persistence
{
	public class DWMBestiaryDbContext : SqliteDbContext
	{
		#region Constructor(s)
		public DWMBestiaryDbContext(string targetDatabaseFilePath, bool enableDebugLogging = false) :
			base
			(
				targetDatabaseFilePath, 
				PersistenceConstants.SQLITE_TEMPLATE_FILEPATH, 
				null,
				enableDebugLogging
			)
		{
		}
		#endregion



		#region DbSet Propert(ies)
		public DbSet<Monster> Monsters { get; set; }
		public DbSet<GrowthCategory> GrowthCategories { get; set; }
		public DbSet<MonsterGrowth> MonsterGrowths { get; set; }
		public DbSet<SkillResistance> SkillResistances { get; set; }
		public DbSet<Skill> Skills { get; set; }
		public DbSet<NaturalSkill> NaturalSkills { get; set; }
		public DbSet<SkillComboRequirement> SkillComboRequirements { get; set; }
		public DbSet<SkillStatRequirement> SkillStatRequirements { get; set; }
		public DbSet<SkillUpgrade> SkillUpgrades { get; set; }
		public DbSet<BreedingPair> BreedingPairs { get; set; }
		public DbSet<MonsterLocation> MonsterLocations { get; set; }
		#endregion
	}
}