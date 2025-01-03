using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DWM.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrowthCategories",
                columns: table => new
                {
                    GrowthCategoryId = table.Column<int>(type: "int", nullable: false),
                    GrowthStatistics = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowthCategories", x => x.GrowthCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    MonsterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonsterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FamilyId = table.Column<int>(type: "int", nullable: false),
                    MaxLevel = table.Column<int>(type: "int", nullable: false),
                    WildnessLevel = table.Column<int>(type: "int", nullable: false),
                    GenderProbabilityId = table.Column<int>(type: "int", nullable: false),
                    IsFlying = table.Column<bool>(type: "bit", nullable: false),
                    IsMetal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.MonsterId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillClassificationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "BreedingPairs",
                columns: table => new
                {
                    BreedingPairId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OffspringMonsterId = table.Column<int>(type: "int", nullable: false),
                    OffspringPlusRequirement = table.Column<int>(type: "int", nullable: true),
                    PedigreeFamilyId = table.Column<int>(type: "int", nullable: true),
                    PedigreeMonsterId = table.Column<int>(type: "int", nullable: true),
                    PedigreePlusRequirement = table.Column<int>(type: "int", nullable: true),
                    PartnerFamilyId = table.Column<int>(type: "int", nullable: true),
                    PartnerMonsterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedingPairs", x => x.BreedingPairId);
                    table.ForeignKey(
                        name: "FK_BreedingPairs_Monsters_OffspringMonsterId",
                        column: x => x.OffspringMonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId");
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
                    MonsterGrowthId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatId = table.Column<int>(type: "int", nullable: false),
                    MonsterId = table.Column<int>(type: "int", nullable: false),
                    GrowthCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterGrowths", x => x.MonsterGrowthId);
                    table.ForeignKey(
                        name: "FK_MonsterGrowths_GrowthCategories_GrowthCategoryId",
                        column: x => x.GrowthCategoryId,
                        principalTable: "GrowthCategories",
                        principalColumn: "GrowthCategoryId");
                    table.ForeignKey(
                        name: "FK_MonsterGrowths_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId");
                });

            migrationBuilder.CreateTable(
                name: "MonsterLocations",
                columns: table => new
                {
                    MonsterLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonsterId = table.Column<int>(type: "int", nullable: false),
                    TravelersGateName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MinLevelEncountered = table.Column<int>(type: "int", nullable: false),
                    MaxLevelEncountered = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterLocations", x => x.MonsterLocationId);
                    table.ForeignKey(
                        name: "FK_MonsterLocations_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId");
                });

            migrationBuilder.CreateTable(
                name: "SkillResistances",
                columns: table => new
                {
                    SkillResistanceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonsterId = table.Column<int>(type: "int", nullable: false),
                    SkillClassificationId = table.Column<int>(type: "int", nullable: false),
                    ResistanceLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillResistances", x => x.SkillResistanceId);
                    table.ForeignKey(
                        name: "FK_SkillResistances_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId");
                });

            migrationBuilder.CreateTable(
                name: "NaturalSkills",
                columns: table => new
                {
                    NaturalSkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonsterId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalSkills", x => x.NaturalSkillId);
                    table.ForeignKey(
                        name: "FK_NaturalSkills_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "MonsterId");
                    table.ForeignKey(
                        name: "FK_NaturalSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                });

            migrationBuilder.CreateTable(
                name: "SkillComboRequirements",
                columns: table => new
                {
                    SkillComboRequirementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetSkillId = table.Column<int>(type: "int", nullable: false),
                    RequiredSkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillComboRequirements", x => x.SkillComboRequirementId);
                    table.ForeignKey(
                        name: "FK_SkillComboRequirements_Skills_RequiredSkillId",
                        column: x => x.RequiredSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                    table.ForeignKey(
                        name: "FK_SkillComboRequirements_Skills_TargetSkillId",
                        column: x => x.TargetSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                });

            migrationBuilder.CreateTable(
                name: "SkillStatRequirements",
                columns: table => new
                {
                    SkillStatRequirementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetSkillId = table.Column<int>(type: "int", nullable: false),
                    StatId = table.Column<int>(type: "int", nullable: false),
                    RequiredValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillStatRequirements", x => x.SkillStatRequirementId);
                    table.ForeignKey(
                        name: "FK_SkillStatRequirements_Skills_TargetSkillId",
                        column: x => x.TargetSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                });

            migrationBuilder.CreateTable(
                name: "SkillUpgrades",
                columns: table => new
                {
                    SkillUpgradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseSkillId = table.Column<int>(type: "int", nullable: false),
                    UpgradedSkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillUpgrades", x => x.SkillUpgradeId);
                    table.ForeignKey(
                        name: "FK_SkillUpgrades_Skills_BaseSkillId",
                        column: x => x.BaseSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                    table.ForeignKey(
                        name: "FK_SkillUpgrades_Skills_UpgradedSkillId",
                        column: x => x.UpgradedSkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
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
