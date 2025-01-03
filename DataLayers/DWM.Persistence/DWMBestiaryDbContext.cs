using DWM.Models;
using DWM.Models.Enums;
using Microsoft.EntityFrameworkCore;
using TGH.Common.Persistence.Implementations;

namespace DWM.Persistence
{
	public class DWMBestiarySqliteDbContext : SqliteDbContext
	{
		#region Constructor(s)
		public DWMBestiarySqliteDbContext(string targetDatabaseFilePath, bool enableDebugLogging = false) :
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


	public class DWMBestiarySqlServerDbContext : EFCoreDatabaseContextBase
	{
		#region Constructor(s)
		public DWMBestiarySqlServerDbContext()
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
		public DbSet<MonsterHierarchy> MonsterHierarchies { get; set; }
		#endregion


		#region Overrides
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DWMCompanion;Trusted_Connection=True;MultipleActiveResultSets=true");

			base.OnConfiguring(optionsBuilder);
		}


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Monster>().HasKey(m => m.MonsterId);
			modelBuilder.Entity<Monster>().Property(m => m.MonsterId).ValueGeneratedOnAdd();
			modelBuilder.Entity<Monster>().HasMany(m => m.StatGrowth).WithOne(mg => mg.Monster).HasForeignKey(mg => mg.MonsterId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Monster>().HasMany(m => m.MonsterLocations).WithOne(ml => ml.Monster).HasForeignKey(ml => ml.MonsterId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Monster>().HasMany(m => m.NaturalSkills).WithOne().HasForeignKey(ns => ns.MonsterId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Monster>().HasMany(m => m.SkillResistances).WithOne().HasForeignKey(sr => sr.MonsterId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Monster>().Property(m => m.MonsterName).HasMaxLength(100);

			modelBuilder.Entity<GrowthCategory>().HasKey(gc => gc.GrowthCategoryId);
			modelBuilder.Entity<GrowthCategory>().Property(gc => gc.GrowthCategoryId).ValueGeneratedNever();
			modelBuilder.Entity<GrowthCategory>().Property(gc => gc.GrowthStatistics).HasMaxLength(1000);

			modelBuilder.Entity<MonsterGrowth>().HasKey(mg => mg.MonsterGrowthId);
			modelBuilder.Entity<MonsterGrowth>().Property(mg => mg.MonsterGrowthId).ValueGeneratedOnAdd();
			modelBuilder.Entity<MonsterGrowth>().HasOne(mg => mg.GrowthCategory).WithMany().HasForeignKey(mg => mg.GrowthCategoryId).OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<SkillResistance>().HasKey(sr => sr.SkillResistanceId);
			modelBuilder.Entity<SkillResistance>().Property(sr => sr.SkillResistanceId).ValueGeneratedOnAdd();

			modelBuilder.Entity<Skill>().HasKey(s => s.SkillId);
			modelBuilder.Entity<Skill>().Property(s => s.SkillId).ValueGeneratedOnAdd();
			modelBuilder.Entity<Skill>().HasMany(s => s.StatRequirements).WithOne(ssr => ssr.TargetSkill).HasForeignKey(ssr => ssr.TargetSkillId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Skill>().HasMany(s => s.ComboRequirements).WithOne(scr => scr.TargetSkill).HasForeignKey(scr => scr.TargetSkillId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Skill>().HasMany<SkillComboRequirement>().WithOne(scr => scr.RequiredSkill).HasForeignKey(scr => scr.RequiredSkillId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Skill>().HasMany(s => s.AvailableUpgrades).WithOne(su => su.BaseSkill).HasForeignKey(su => su.BaseSkillId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<Skill>().HasMany<SkillUpgrade>().WithOne(su => su.UpgradedSkill).HasForeignKey(su => su.UpgradedSkillId).OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<NaturalSkill>().HasKey(ns => ns.NaturalSkillId);
			modelBuilder.Entity<NaturalSkill>().Property(ns => ns.NaturalSkillId).ValueGeneratedOnAdd();
			modelBuilder.Entity<NaturalSkill>().HasOne(ns => ns.Skill).WithMany().HasForeignKey(ns => ns.SkillId).OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<SkillComboRequirement>().HasKey(scr => scr.SkillComboRequirementId);
			modelBuilder.Entity<SkillComboRequirement>().Property(scr => scr.SkillComboRequirementId).ValueGeneratedOnAdd();

			modelBuilder.Entity<SkillStatRequirement>().HasKey(ssr => ssr.SkillStatRequirementId);
			modelBuilder.Entity<SkillStatRequirement>().Property(ssr => ssr.SkillStatRequirementId).ValueGeneratedOnAdd();

			modelBuilder.Entity<SkillUpgrade>().HasKey(su => su.SkillUpgradeId);
			modelBuilder.Entity<SkillUpgrade>().Property(su => su.SkillUpgradeId).ValueGeneratedOnAdd();
			modelBuilder.Entity<SkillUpgrade>().HasOne(su => su.BaseSkill).WithMany(s => s.AvailableUpgrades).HasForeignKey(su => su.BaseSkillId).OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<BreedingPair>().HasKey(bp => bp.BreedingPairId);
			modelBuilder.Entity<BreedingPair>().Property(bp => bp.BreedingPairId).ValueGeneratedOnAdd();
			modelBuilder.Entity<BreedingPair>().HasOne(bp => bp.OffspringMonster).WithMany().HasForeignKey(bp => bp.OffspringMonsterId).OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<BreedingPair>().HasOne(bp => bp.PedigreeMonster).WithMany().HasForeignKey(bp => bp.PedigreeMonsterId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);
			modelBuilder.Entity<BreedingPair>().HasOne(bp => bp.PartnerMonster).WithMany().HasForeignKey(bp => bp.PartnerMonsterId).OnDelete(DeleteBehavior.NoAction).IsRequired(false);

			modelBuilder.Entity<MonsterLocation>().HasKey(ml => ml.MonsterLocationId);
			modelBuilder.Entity<MonsterLocation>().Property(ml => ml.MonsterLocationId).ValueGeneratedOnAdd();
			modelBuilder.Entity<MonsterLocation>().Property(m => m.TravelersGateName).HasMaxLength(50);

			modelBuilder.Entity<MonsterHierarchy>().HasNoKey();

			base.OnModelCreating(modelBuilder);
		}
		#endregion
	}
}