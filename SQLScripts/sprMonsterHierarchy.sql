CREATE OR ALTER PROCEDURE sprMonsterHierarchy @targetMonsterName NVARCHAR(100), @maxHierarchyLevels SMALLINT = NULL
AS
	--TEST:
	--EXEC sprMonsterHierarchy '1EyeClown'

--Select a single breeding pair for each offspring (to prevent 'decision fatigue')
;WITH LimitedBreedingPairs AS 
(
	SELECT *
	FROM
		(
			SELECT
				ROW_NUMBER() OVER (PARTITION BY OffspringMonsterId ORDER BY PedigreeFamilyId DESC, PartnerFamilyId DESC, PedigreeMonsterId, PartnerMonsterId) AS Precedence,
				*
			FROM 
				BreedingPairs BP
		) PRT_BP
	WHERE
		Precedence = 1
)

--Project each type of pair (monster/monster vs monster/family)
,BreedingOptions AS
(
	--Monster Pedigree; Monster Partner
	SELECT
		BP.OffspringMonsterId,
		M_OFF.MonsterName AS OffspringMonsterName,
		'Monster' AS PedigreeType,
		BP.PedigreeMonsterId AS PedigreeId,
		M_PED.MonsterName AS PedigreeName,
		'Monster' AS PartnerType,
		BP.PartnerMonsterId AS PartnerId,
		M_PRT.MonsterName AS PartnerName
	FROM
		LimitedBreedingPairs BP
		INNER JOIN Monsters M_OFF
			ON M_OFF.MonsterId = BP.OffspringMonsterId
		INNER JOIN Monsters M_PED
			ON M_PED.MonsterId = BP.PedigreeMonsterId
		INNER JOIN Monsters M_PRT
			ON M_PRT.MonsterId = BP.PartnerMonsterId

	UNION ALL

	--Family Pedigree; Family Partner
	SELECT
		BP.OffspringMonsterId,
		M_OFF.MonsterName AS OffspringMonsterName,
		'Family' AS PedigreeType,
		BP.PedigreeFamilyId AS PedigreeId,
		F_PED.FamilyDescription AS PedigreeName,
		'Family' AS PartnerType,
		BP.PartnerFamilyId AS PartnerId,
		F_PRT.FamilyDescription AS PartnerName
	FROM
		LimitedBreedingPairs BP
		INNER JOIN Monsters M_OFF
			ON M_OFF.MonsterId = BP.OffspringMonsterId
		INNER JOIN C_Family F_PED
			ON F_PED.FamilyId = BP.PedigreeFamilyId
		INNER JOIN C_Family F_PRT
			ON F_PRT.FamilyId = BP.PartnerFamilyId

	UNION ALL 

	--Monster Pedigree; Family Partner
	SELECT
		BP.OffspringMonsterId,
		M_OFF.MonsterName AS OffspringMonsterName,
		'Monster' AS PedigreeType,
		BP.PedigreeMonsterId AS PedigreeId,
		M_PED.MonsterName AS PedigreeName,
		'Family' AS PartnerType,
		BP.PartnerFamilyId AS PartnerId,
		F_PRT.FamilyDescription AS PartnerName
	FROM
		LimitedBreedingPairs BP
		INNER JOIN Monsters M_OFF
			ON M_OFF.MonsterId = BP.OffspringMonsterId
		INNER JOIN Monsters M_PED
			ON M_PED.MonsterId = BP.PedigreeMonsterId
		INNER JOIN C_Family F_PRT
			ON F_PRT.FamilyId = BP.PartnerFamilyId

	UNION ALL

	--Family Pedigree; Monster Partner
	SELECT
		BP.OffspringMonsterId,
		M_OFF.MonsterName AS OffspringMonsterName,
		'Family' AS PedigreeType,
		BP.PedigreeFamilyId AS PedigreeId,
		F_PED.FamilyDescription AS PedigreeName,
		'Monster' AS PartnerType,
		BP.PartnerMonsterId AS PartnerId,
		M_PRT.MonsterName AS PartnerName
	FROM
		LimitedBreedingPairs BP
		INNER JOIN Monsters M_OFF
			ON M_OFF.MonsterId = BP.OffspringMonsterId
		INNER JOIN C_Family F_PED
			ON F_PED.FamilyId = BP.PedigreeFamilyId
		INNER JOIN Monsters M_PRT
			ON M_PRT.MonsterId = BP.PartnerMonsterId
)

,RootMonster AS
(
	SELECT TOP 1
		BO_ROOT.*
	FROM 
		BreedingOptions BO_ROOT
	WHERE
		BO_ROOT.OffspringMonsterName = @targetMonsterName
)

,BreedingHierarchy AS
(
	SELECT
		1 AS HierarchyLevel,
		1 AS NodeOrdinal,
		CONVERT(VARCHAR(30), 'Target Monster') AS RecordType,
		CONVERT(VARCHAR(1000), NULL) AS ParentHierarchyKey,
		CONVERT(VARCHAR(1000), CONVERT(VARCHAR, RM.OffspringMonsterId) + '-1') AS HierarchyKey,
		RM.*
	FROM
		RootMonster RM

	UNION ALL

	SELECT
		1 + BH.HierarchyLevel AS HierarchyLevel,
		((BH.NodeOrdinal - 1) * 2) + 1 AS NodeOrdinal,
		CONVERT(VARCHAR(30), 'Pedigree') AS RecordType,
		BH.HierarchyKey AS ParentHierarchyKey,
		CONVERT
		(
			VARCHAR(1000), 
			BH.HierarchyKey + '/' + 
				CONVERT(VARCHAR, BO_PED.OffspringMonsterId) + '-' + 
				CONVERT(VARCHAR, ((BH.NodeOrdinal - 1) * 2) + 1)
		) AS HierarchyKey,
		BO_PED.*
	FROM
		BreedingHierarchy BH
		INNER JOIN BreedingOptions BO_PED
			ON BO_PED.OffspringMonsterId = BH.PedigreeId
	WHERE
		(
			--Terminate the recursion when all monsters can be made with simple family combinatins (or when the specified number of layers have been fetched)
			BH.PedigreeType <> 'Family'
		)

	UNION ALL

	SELECT
		1 + BH.HierarchyLevel AS HierarchyLevel,
		((BH.NodeOrdinal - 1) * 2) + 2 AS NodeOrdinal,
		CONVERT(VARCHAR(30), 'Partner') AS RecordType,
		BH.HierarchyKey AS ParentHierarchyKey,
		CONVERT
		(
			VARCHAR(1000), 
			BH.HierarchyKey + '/' + 
				CONVERT(VARCHAR, BO_PRT.OffspringMonsterId) + '-' + 
				CONVERT(VARCHAR, ((BH.NodeOrdinal - 1) * 2) + 2)		
		) AS HierarchyKey,
		BO_PRT.*
	FROM
		BreedingHierarchy BH
		INNER JOIN BreedingOptions BO_PRT
			ON BO_PRT.OffspringMonsterId = BH.PartnerId
	WHERE
		(
			--Terminate the recursion when all monsters can be made with simple family combinatins (or when the specified number of layers have been fetched)
			BH.PartnerType <> 'Family'
		)
)

SELECT *
FROM
	BreedingHierarchy
WHERE
	(
		@maxHierarchyLevels IS NULL OR
		HierarchyLevel <= @maxHierarchyLevels
	)
ORDER BY
	HierarchyLevel,
	ParentHierarchyKey