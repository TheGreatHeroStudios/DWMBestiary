CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "GrowthCategories" (
    "GrowthCategoryId" INTEGER NOT NULL CONSTRAINT "PK_GrowthCategories" PRIMARY KEY AUTOINCREMENT,
    "GrowthStatistics" TEXT NOT NULL
);

CREATE TABLE "Monsters" (
    "MonsterId" INTEGER NOT NULL CONSTRAINT "PK_Monsters" PRIMARY KEY AUTOINCREMENT,
    "MonsterName" TEXT NOT NULL,
    "FamilyId" INTEGER NOT NULL,
    "MaxLevel" INTEGER NOT NULL,
    "WildnessLevel" INTEGER NOT NULL,
    "GenderProbabilityId" INTEGER NOT NULL,
    "IsFlying" INTEGER NOT NULL,
    "IsMetal" INTEGER NOT NULL
);

CREATE TABLE "Skills" (
    "SkillId" INTEGER NOT NULL CONSTRAINT "PK_Skills" PRIMARY KEY AUTOINCREMENT,
    "SkillName" TEXT NOT NULL,
    "SkillDescription" TEXT NOT NULL,
    "SkillClassificationId" INTEGER NOT NULL
);

CREATE TABLE "BreedingPairs" (
    "BreedingPairId" INTEGER NOT NULL CONSTRAINT "PK_BreedingPairs" PRIMARY KEY AUTOINCREMENT,
    "OffspringMonsterId" INTEGER NOT NULL,
    "OffspringPlusRequirement" INTEGER NULL,
    "PedigreeFamilyId" INTEGER NULL,
    "PedigreeMonsterId" INTEGER NULL,
    "PedigreePlusRequirement" INTEGER NULL,
    "PartnerFamilyId" INTEGER NULL,
    "PartnerMonsterId" INTEGER NULL,
    CONSTRAINT "FK_BreedingPairs_Monsters_OffspringMonsterId" FOREIGN KEY ("OffspringMonsterId") REFERENCES "Monsters" ("MonsterId") ON DELETE CASCADE,
    CONSTRAINT "FK_BreedingPairs_Monsters_PartnerMonsterId" FOREIGN KEY ("PartnerMonsterId") REFERENCES "Monsters" ("MonsterId"),
    CONSTRAINT "FK_BreedingPairs_Monsters_PedigreeMonsterId" FOREIGN KEY ("PedigreeMonsterId") REFERENCES "Monsters" ("MonsterId")
);

CREATE TABLE "MonsterGrowths" (
    "MonsterGrowthId" INTEGER NOT NULL CONSTRAINT "PK_MonsterGrowths" PRIMARY KEY AUTOINCREMENT,
    "StatId" INTEGER NOT NULL,
    "MonsterId" INTEGER NOT NULL,
    "GrowthCategoryId" INTEGER NOT NULL,
    CONSTRAINT "FK_MonsterGrowths_GrowthCategories_GrowthCategoryId" FOREIGN KEY ("GrowthCategoryId") REFERENCES "GrowthCategories" ("GrowthCategoryId") ON DELETE CASCADE,
    CONSTRAINT "FK_MonsterGrowths_Monsters_MonsterId" FOREIGN KEY ("MonsterId") REFERENCES "Monsters" ("MonsterId") ON DELETE CASCADE
);

CREATE TABLE "MonsterLocations" (
    "MonsterLocationId" INTEGER NOT NULL CONSTRAINT "PK_MonsterLocations" PRIMARY KEY AUTOINCREMENT,
    "MonsterId" INTEGER NOT NULL,
    "TravelersGateName" TEXT NOT NULL,
    "MinLevelEncountered" INTEGER NOT NULL,
    "MaxLevelEncountered" INTEGER NOT NULL,
    CONSTRAINT "FK_MonsterLocations_Monsters_MonsterId" FOREIGN KEY ("MonsterId") REFERENCES "Monsters" ("MonsterId") ON DELETE CASCADE
);

CREATE TABLE "SkillResistances" (
    "SkillResistanceId" INTEGER NOT NULL CONSTRAINT "PK_SkillResistances" PRIMARY KEY AUTOINCREMENT,
    "MonsterId" INTEGER NOT NULL,
    "SkillClassificationId" INTEGER NOT NULL,
    "ResistanceLevel" INTEGER NOT NULL,
    CONSTRAINT "FK_SkillResistances_Monsters_MonsterId" FOREIGN KEY ("MonsterId") REFERENCES "Monsters" ("MonsterId") ON DELETE CASCADE
);

CREATE TABLE "NaturalSkills" (
    "NaturalSkillId" INTEGER NOT NULL CONSTRAINT "PK_NaturalSkills" PRIMARY KEY AUTOINCREMENT,
    "MonsterId" INTEGER NOT NULL,
    "SkillId" INTEGER NOT NULL,
    CONSTRAINT "FK_NaturalSkills_Monsters_MonsterId" FOREIGN KEY ("MonsterId") REFERENCES "Monsters" ("MonsterId") ON DELETE CASCADE,
    CONSTRAINT "FK_NaturalSkills_Skills_SkillId" FOREIGN KEY ("SkillId") REFERENCES "Skills" ("SkillId") ON DELETE CASCADE
);

CREATE TABLE "SkillComboRequirements" (
    "SkillComboRequirementId" INTEGER NOT NULL CONSTRAINT "PK_SkillComboRequirements" PRIMARY KEY AUTOINCREMENT,
    "TargetSkillId" INTEGER NOT NULL,
    "RequiredSkillId" INTEGER NOT NULL,
    CONSTRAINT "FK_SkillComboRequirements_Skills_RequiredSkillId" FOREIGN KEY ("RequiredSkillId") REFERENCES "Skills" ("SkillId") ON DELETE CASCADE,
    CONSTRAINT "FK_SkillComboRequirements_Skills_TargetSkillId" FOREIGN KEY ("TargetSkillId") REFERENCES "Skills" ("SkillId") ON DELETE CASCADE
);

CREATE TABLE "SkillStatRequirements" (
    "SkillStatRequirementId" INTEGER NOT NULL CONSTRAINT "PK_SkillStatRequirements" PRIMARY KEY AUTOINCREMENT,
    "TargetSkillId" INTEGER NOT NULL,
    "StatId" INTEGER NOT NULL,
    "RequiredValue" INTEGER NOT NULL,
    CONSTRAINT "FK_SkillStatRequirements_Skills_TargetSkillId" FOREIGN KEY ("TargetSkillId") REFERENCES "Skills" ("SkillId") ON DELETE CASCADE
);

CREATE TABLE "SkillUpgrades" (
    "SkillUpgradeId" INTEGER NOT NULL CONSTRAINT "PK_SkillUpgrades" PRIMARY KEY AUTOINCREMENT,
    "BaseSkillId" INTEGER NOT NULL,
    "UpgradedSkillId" INTEGER NOT NULL,
    CONSTRAINT "FK_SkillUpgrades_Skills_BaseSkillId" FOREIGN KEY ("BaseSkillId") REFERENCES "Skills" ("SkillId") ON DELETE CASCADE,
    CONSTRAINT "FK_SkillUpgrades_Skills_UpgradedSkillId" FOREIGN KEY ("UpgradedSkillId") REFERENCES "Skills" ("SkillId") ON DELETE CASCADE
);

CREATE INDEX "IX_BreedingPairs_OffspringMonsterId" ON "BreedingPairs" ("OffspringMonsterId");

CREATE INDEX "IX_BreedingPairs_PartnerMonsterId" ON "BreedingPairs" ("PartnerMonsterId");

CREATE INDEX "IX_BreedingPairs_PedigreeMonsterId" ON "BreedingPairs" ("PedigreeMonsterId");

CREATE INDEX "IX_MonsterGrowths_GrowthCategoryId" ON "MonsterGrowths" ("GrowthCategoryId");

CREATE INDEX "IX_MonsterGrowths_MonsterId" ON "MonsterGrowths" ("MonsterId");

CREATE INDEX "IX_MonsterLocations_MonsterId" ON "MonsterLocations" ("MonsterId");

CREATE INDEX "IX_NaturalSkills_MonsterId" ON "NaturalSkills" ("MonsterId");

CREATE INDEX "IX_NaturalSkills_SkillId" ON "NaturalSkills" ("SkillId");

CREATE INDEX "IX_SkillComboRequirements_RequiredSkillId" ON "SkillComboRequirements" ("RequiredSkillId");

CREATE INDEX "IX_SkillComboRequirements_TargetSkillId" ON "SkillComboRequirements" ("TargetSkillId");

CREATE INDEX "IX_SkillResistances_MonsterId" ON "SkillResistances" ("MonsterId");

CREATE INDEX "IX_SkillStatRequirements_TargetSkillId" ON "SkillStatRequirements" ("TargetSkillId");

CREATE INDEX "IX_SkillUpgrades_BaseSkillId" ON "SkillUpgrades" ("BaseSkillId");

CREATE INDEX "IX_SkillUpgrades_UpgradedSkillId" ON "SkillUpgrades" ("UpgradedSkillId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220719231812_InitialMigration', '6.0.7');

COMMIT;

