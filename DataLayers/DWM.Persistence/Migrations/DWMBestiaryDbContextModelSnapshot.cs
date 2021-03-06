// <auto-generated />
using System;
using DWM.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DWM.Persistence.Migrations
{
    [DbContext(typeof(DWMBestiaryDbContext))]
    partial class DWMBestiaryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("DWM.Models.BreedingPair", b =>
                {
                    b.Property<int>("BreedingPairId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OffspringMonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OffspringPlusRequirement")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PartnerFamilyId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PartnerMonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PedigreeFamilyId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PedigreeMonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PedigreePlusRequirement")
                        .HasColumnType("INTEGER");

                    b.HasKey("BreedingPairId");

                    b.HasIndex("OffspringMonsterId");

                    b.HasIndex("PartnerMonsterId");

                    b.HasIndex("PedigreeMonsterId");

                    b.ToTable("BreedingPairs");
                });

            modelBuilder.Entity("DWM.Models.GrowthCategory", b =>
                {
                    b.Property<int>("GrowthCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("GrowthStatistics")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("GrowthCategoryId");

                    b.ToTable("GrowthCategories");
                });

            modelBuilder.Entity("DWM.Models.Monster", b =>
                {
                    b.Property<int>("MonsterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FamilyId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GenderProbabilityId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFlying")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMetal")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MonsterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WildnessLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("MonsterId");

                    b.ToTable("Monsters");
                });

            modelBuilder.Entity("DWM.Models.MonsterGrowth", b =>
                {
                    b.Property<int>("MonsterGrowthId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GrowthCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StatId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MonsterGrowthId");

                    b.HasIndex("GrowthCategoryId");

                    b.HasIndex("MonsterId");

                    b.ToTable("MonsterGrowths");
                });

            modelBuilder.Entity("DWM.Models.MonsterLocation", b =>
                {
                    b.Property<int>("MonsterLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxLevelEncountered")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MinLevelEncountered")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TravelersGateName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("MonsterLocationId");

                    b.HasIndex("MonsterId");

                    b.ToTable("MonsterLocations");
                });

            modelBuilder.Entity("DWM.Models.NaturalSkill", b =>
                {
                    b.Property<int>("NaturalSkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkillId")
                        .HasColumnType("INTEGER");

                    b.HasKey("NaturalSkillId");

                    b.HasIndex("MonsterId");

                    b.HasIndex("SkillId");

                    b.ToTable("NaturalSkills");
                });

            modelBuilder.Entity("DWM.Models.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkillClassificationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SkillDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SkillName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SkillId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("DWM.Models.SkillComboRequirement", b =>
                {
                    b.Property<int>("SkillComboRequirementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequiredSkillId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetSkillId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SkillComboRequirementId");

                    b.HasIndex("RequiredSkillId");

                    b.HasIndex("TargetSkillId");

                    b.ToTable("SkillComboRequirements");
                });

            modelBuilder.Entity("DWM.Models.SkillResistance", b =>
                {
                    b.Property<int>("SkillResistanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MonsterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ResistanceLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkillClassificationId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SkillResistanceId");

                    b.HasIndex("MonsterId");

                    b.ToTable("SkillResistances");
                });

            modelBuilder.Entity("DWM.Models.SkillStatRequirement", b =>
                {
                    b.Property<int>("SkillStatRequirementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequiredValue")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StatId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetSkillId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SkillStatRequirementId");

                    b.HasIndex("TargetSkillId");

                    b.ToTable("SkillStatRequirements");
                });

            modelBuilder.Entity("DWM.Models.SkillUpgrade", b =>
                {
                    b.Property<int>("SkillUpgradeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseSkillId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UpgradedSkillId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SkillUpgradeId");

                    b.HasIndex("BaseSkillId");

                    b.HasIndex("UpgradedSkillId");

                    b.ToTable("SkillUpgrades");
                });

            modelBuilder.Entity("DWM.Models.BreedingPair", b =>
                {
                    b.HasOne("DWM.Models.Monster", "OffspringMonster")
                        .WithMany()
                        .HasForeignKey("OffspringMonsterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWM.Models.Monster", "PartnerMonster")
                        .WithMany()
                        .HasForeignKey("PartnerMonsterId");

                    b.HasOne("DWM.Models.Monster", "PedigreeMonster")
                        .WithMany()
                        .HasForeignKey("PedigreeMonsterId");

                    b.Navigation("OffspringMonster");

                    b.Navigation("PartnerMonster");

                    b.Navigation("PedigreeMonster");
                });

            modelBuilder.Entity("DWM.Models.MonsterGrowth", b =>
                {
                    b.HasOne("DWM.Models.GrowthCategory", "GrowthCategory")
                        .WithMany()
                        .HasForeignKey("GrowthCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWM.Models.Monster", null)
                        .WithMany("StatGrowth")
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GrowthCategory");
                });

            modelBuilder.Entity("DWM.Models.MonsterLocation", b =>
                {
                    b.HasOne("DWM.Models.Monster", "Monster")
                        .WithMany()
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Monster");
                });

            modelBuilder.Entity("DWM.Models.NaturalSkill", b =>
                {
                    b.HasOne("DWM.Models.Monster", null)
                        .WithMany("NaturalSkills")
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWM.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("DWM.Models.SkillComboRequirement", b =>
                {
                    b.HasOne("DWM.Models.Skill", "RequiredSkill")
                        .WithMany()
                        .HasForeignKey("RequiredSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWM.Models.Skill", "TargetSkill")
                        .WithMany("ComboRequirements")
                        .HasForeignKey("TargetSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RequiredSkill");

                    b.Navigation("TargetSkill");
                });

            modelBuilder.Entity("DWM.Models.SkillResistance", b =>
                {
                    b.HasOne("DWM.Models.Monster", null)
                        .WithMany("SkillResistances")
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DWM.Models.SkillStatRequirement", b =>
                {
                    b.HasOne("DWM.Models.Skill", "TargetSkill")
                        .WithMany("StatRequirements")
                        .HasForeignKey("TargetSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TargetSkill");
                });

            modelBuilder.Entity("DWM.Models.SkillUpgrade", b =>
                {
                    b.HasOne("DWM.Models.Skill", "BaseSkill")
                        .WithMany("AvailableUpgrades")
                        .HasForeignKey("BaseSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DWM.Models.Skill", "UpgradedSkill")
                        .WithMany()
                        .HasForeignKey("UpgradedSkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseSkill");

                    b.Navigation("UpgradedSkill");
                });

            modelBuilder.Entity("DWM.Models.Monster", b =>
                {
                    b.Navigation("NaturalSkills");

                    b.Navigation("SkillResistances");

                    b.Navigation("StatGrowth");
                });

            modelBuilder.Entity("DWM.Models.Skill", b =>
                {
                    b.Navigation("AvailableUpgrades");

                    b.Navigation("ComboRequirements");

                    b.Navigation("StatRequirements");
                });
#pragma warning restore 612, 618
        }
    }
}
