CREATE TABLE [C_Family] ([FamilyId] INTEGER NOT NULL CONSTRAINT [PK_C_Family] PRIMARY KEY, [FamilyDescription] NVARCHAR(8) NOT NULL);

INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (0, 'Slime');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (1, 'Dragon');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (2, 'Beast');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (3, 'Bird');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (4, 'Plant');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (5, 'Bug');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (6, 'Devil');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (7, 'Zombie');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (8, 'Material');
INSERT INTO [C_Family] ([FamilyId], [FamilyDescription]) VALUES (9, 'Boss');

CREATE TABLE [C_GenderProbability] ([GenderProbabilityId] INTEGER NOT NULL CONSTRAINT [PK_C_GenderProbability] PRIMARY KEY, [GenderProbabilityDescription] NVARCHAR(16) NOT NULL);

INSERT INTO [C_GenderProbability] ([GenderProbabilityId], [GenderProbabilityDescription]) VALUES (0, 'AlwaysMale');
INSERT INTO [C_GenderProbability] ([GenderProbabilityId], [GenderProbabilityDescription]) VALUES (1, 'UsuallyMale');
INSERT INTO [C_GenderProbability] ([GenderProbabilityId], [GenderProbabilityDescription]) VALUES (2, 'EvenDistribution');
INSERT INTO [C_GenderProbability] ([GenderProbabilityId], [GenderProbabilityDescription]) VALUES (3, 'UsuallyFemale');

CREATE TABLE [C_MonsterFamily] ([MonsterFamilyId] INTEGER NOT NULL CONSTRAINT [PK_C_MonsterFamily] PRIMARY KEY, [MonsterFamilyDescription] NVARCHAR(8) NOT NULL);

INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (0, 'Slime');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (1, 'Dragon');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (2, 'Beast');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (3, 'Bird');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (4, 'Plant');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (5, 'Bug');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (6, 'Devil');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (7, 'Zombie');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (8, 'Material');
INSERT INTO [C_MonsterFamily] ([MonsterFamilyId], [MonsterFamilyDescription]) VALUES (9, 'Boss');

CREATE TABLE [C_SkillClassification] ([SkillClassificationId] INTEGER NOT NULL CONSTRAINT [PK_C_SkillClassification] PRIMARY KEY, [SkillClassificationDescription] NVARCHAR(10) NOT NULL);

INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (48, 'Attack');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (49, 'Summons');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (50, 'Healing');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (51, 'Recovery');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (52, 'Revive');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (53, 'Defense');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (54, 'Support');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (65, 'Blaze');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (66, 'Fireball');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (67, 'Bang');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (68, 'Infernos');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (69, 'Bolt');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (70, 'IceBolt');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (71, 'Surround');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (72, 'Sleep');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (73, 'Beat');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (74, 'OddDance');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (75, 'StopSpell');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (76, 'Panic');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (77, 'Sap');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (78, 'Slow');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (79, 'Sacrifice');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (80, 'MegaMagic');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (81, 'Flame');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (82, 'Blizzard');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (83, 'Poison');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (84, 'Paralysis');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (85, 'Curse');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (86, 'LoseATurn');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (87, 'DanceTrap');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (88, 'BreathSeal');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (89, 'Allied');
INSERT INTO [C_SkillClassification] ([SkillClassificationId], [SkillClassificationDescription]) VALUES (90, 'GigaSlash');

CREATE TABLE [C_Stat] ([StatId] INTEGER NOT NULL CONSTRAINT [PK_C_Stat] PRIMARY KEY, [StatDescription] NVARCHAR(12) NOT NULL);

INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (0, 'Level');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (1, 'Experience');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (2, 'Health');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (3, 'Magic');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (4, 'Attack');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (5, 'Defense');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (6, 'Agility');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (7, 'Intelligence');
INSERT INTO [C_Stat] ([StatId], [StatDescription]) VALUES (-1, 'Unknown');

