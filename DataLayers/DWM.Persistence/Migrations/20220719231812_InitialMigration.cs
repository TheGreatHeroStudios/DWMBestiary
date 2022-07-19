using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWM.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrowthCategories",
                columns: table => new
                {
                    GrowthCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GrowthStatistics = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowthCategories", x => x.GrowthCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    MonsterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonsterName = table.Column<string>(type: "TEXT", nullable: false),
                    FamilyId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    WildnessLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    GenderProbabilityId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsFlying = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMetal = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.MonsterId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SkillName = table.Column<string>(type: "TEXT", nullable: false),
                    SkillDescription = table.Column<string>(type: "TEXT", nullable: false),
                    SkillClassificationId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "BreedingPairs",
                columns: table => new
                {
                    BreedingPairId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OffspringMonsterId = table.Column<int>(type: "INTEGER", nullable: false),
                    OffspringPlusRequirement = table.Column<int>(type: "INTEGER", nullable: true),
                    PedigreeFamilyId = table.Column<int>(type: "INTEGER", nullable: true),
                    PedigreeMonsterId = table.Column<int>(type: "INTEGER", nullable: true),
                    PedigreePlusRequirement = table.Column<int>(type: "INTEGER", nullable: true),
                    PartnerFamilyId = table.Column<int>(type: "INTEGER", nullable: true),
                    PartnerMonsterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingPairs", x => x.BreedingPairId);
                    table.ForeignKey(
                        name: "FK_BreedingPairs_Monsters_OffspringMonsterId",
                        column: x => x.OffspringMonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BreedingPairs_Monsters_PartnerMonsterId",
                        column: x => x.PartnerMonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId");
                    table.ForeignKey(
                        name: "FK_BreedingPairs_Monsters_PedigreeMonsterId",
                        column: x => x.PedigreeMonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId");
                });

            migrationBuilder.CreateTable(
                name: "MonsterGrowths",
                columns: table => new
                {
                    MonsterGrowthId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StatId = table.Column<int>(type: "INTEGER", nullable: false),
                    MonsterId = table.Column<int>(type: "INTEGER", nullable: false),
                    GrowthCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterGrowths", x => x.MonsterGrowthId);
                    table.ForeignKey(
                        name: "FK_MonsterGrowths_GrowthCategories_GrowthCategoryId",
                        column: x => x.GrowthCategoryId,
                        principalTable: "GrowthCategories",
                        principalColumn: "GrowthCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonsterGrowths_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonsterLocations",
                columns: table => new
                {
                    MonsterLocationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonsterId = table.Column<int>(type: "INTEGER", nullable: false),
                    TravelersGateName = table.Column<string>(type: "TEXT", nullable: false),
                    MinLevelEncountered = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxLevelEncountered = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterLocations", x => x.MonsterLocationId);
                    table.ForeignKey(
                        name: "FK_MonsterLocations_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillResistances",
                columns: table => new
                {
                    SkillResistanceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonsterId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillClassificationId = table.Column<int>(type: "INTEGER", nullable: false),
                    ResistanceLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillResistances", x => x.SkillResistanceId);
                    table.ForeignKey(
                        name: "FK_SkillResistances_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NaturalSkills",
                columns: table => new
                {
                    NaturalSkillId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MonsterId = table.Column<int>(type: "INTEGER", nullable: false),
                    SkillId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalSkills", x => x.NaturalSkillId);
                    table.ForeignKey(
                        name: "FK_NaturalSkills_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NaturalSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillComboRequirements",
                columns: table => new
                {
                    SkillComboRequirementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TargetSkillId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredSkillId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillComboRequirements", x => x.SkillComboRequirementId);
                    table.ForeignKey(
                        name: "FK_SkillComboRequirements_Skills_RequiredSkillId",
                        column: x => x.RequiredSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillComboRequirements_Skills_TargetSkillId",
                        column: x => x.TargetSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillStatRequirements",
                columns: table => new
                {
                    SkillStatRequirementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TargetSkillId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredValue = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillStatRequirements", x => x.SkillStatRequirementId);
                    table.ForeignKey(
                        name: "FK_SkillStatRequirements_Skills_TargetSkillId",
                        column: x => x.TargetSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillUpgrades",
                columns: table => new
                {
                    SkillUpgradeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BaseSkillId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpgradedSkillId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillUpgrades", x => x.SkillUpgradeId);
                    table.ForeignKey(
                        name: "FK_SkillUpgrades_Skills_BaseSkillId",
                        column: x => x.BaseSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SkillUpgrades_Skills_UpgradedSkillId",
                        column: x => x.UpgradedSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BreedingPairs_OffspringMonsterId",
                table: "BreedingPairs",
                column: "OffspringMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingPairs_PartnerMonsterId",
                table: "BreedingPairs",
                column: "PartnerMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_BreedingPairs_PedigreeMonsterId",
                table: "BreedingPairs",
                column: "PedigreeMonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterGrowths_GrowthCategoryId",
                table: "MonsterGrowths",
                column: "GrowthCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterGrowths_MonsterId",
                table: "MonsterGrowths",
                column: "MonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterLocations_MonsterId",
                table: "MonsterLocations",
                column: "MonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalSkills_MonsterId",
                table: "NaturalSkills",
                column: "MonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalSkills_SkillId",
                table: "NaturalSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillComboRequirements_RequiredSkillId",
                table: "SkillComboRequirements",
                column: "RequiredSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillComboRequirements_TargetSkillId",
                table: "SkillComboRequirements",
                column: "TargetSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillResistances_MonsterId",
                table: "SkillResistances",
                column: "MonsterId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillStatRequirements_TargetSkillId",
                table: "SkillStatRequirements",
                column: "TargetSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUpgrades_BaseSkillId",
                table: "SkillUpgrades",
                column: "BaseSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillUpgrades_UpgradedSkillId",
                table: "SkillUpgrades",
                column: "UpgradedSkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BreedingPairs");

            migrationBuilder.DropTable(
                name: "MonsterGrowths");

            migrationBuilder.DropTable(
                name: "MonsterLocations");

            migrationBuilder.DropTable(
                name: "NaturalSkills");

            migrationBuilder.DropTable(
                name: "SkillComboRequirements");

            migrationBuilder.DropTable(
                name: "SkillResistances");

            migrationBuilder.DropTable(
                name: "SkillStatRequirements");

            migrationBuilder.DropTable(
                name: "SkillUpgrades");

            migrationBuilder.DropTable(
                name: "GrowthCategories");

            migrationBuilder.DropTable(
                name: "Monsters");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
