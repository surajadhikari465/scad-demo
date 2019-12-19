CREATE PROCEDURE [extract].ExtractHierarchy @HierarchyId INT
	,@TraitCodes VARCHAR(255)
	,@TraitGroupCodes VARCHAR(255)
AS
BEGIN
	DECLARE @cols AS NVARCHAR(MAX)
		,@colsWithIsNull AS NVARCHAR(MAX)
		,@query AS NVARCHAR(MAX)

	CREATE TABLE #Traits (TraitId INT PRIMARY KEY)

	INSERT INTO #Traits
	SELECT traitid
	FROM (
		SELECT traitID
		FROM Trait t
		INNER JOIN TraitGroup tg ON t.traitGroupID = tg.traitGroupID
		INNER JOIN fn_ParseStringList(@TraitGroupCodes, ',') tgc ON tg.traitGroupCode = tgc.Key_Value
		
		UNION ALL
		
		SELECT traitID
		FROM Trait t
		INNER JOIN fn_ParseStringList(@TraitCodes, ',') tc ON t.traitCode = tc.Key_Value
		) p1
	ORDER BY p1.traitid ASC

	SET @cols = STUFF((
				SELECT DISTINCT ',' + QUOTENAME([traitDesc])
				FROM trait t
				INNER JOIN #Traits bt ON t.traitid = bt.TraitId
				FOR XML PATH('')
					,TYPE
				).value('.', 'NVARCHAR(MAX)'), 1, 1, '')
	SET @colsWithIsNull = STUFF((
				SELECT DISTINCT ', isnull(' + QUOTENAME([traitDesc]) + ','''') as ' + QUOTENAME([traitDesc])
				FROM trait t
				INNER JOIN #Traits bt ON t.traitid = bt.TraitId
				FOR XML PATH('')
					,TYPE
				).value('.', 'NVARCHAR(MAX)'), 1, 1, '')
	SET @query = 'SELECT hierarchyClassID, hierarchyClassName,  ' + @colsWithIsNull + ' from 
            (
				select hc.hierarchyClassID, hierarchyClassName, traitDesc, traitValue [Value]
				from Hierarchyclass hc inner join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
				inner join Trait t on hct.traitID = t.traitID
				inner join #Traits bt on t.traitID = bt.TraitId
				inner join Hierarchy h on h.hierarchyID = hc.hierarchyID
				where h.hierarchyid = ' + cast(@HierarchyId AS VARCHAR(100)) + '
            ) x
            pivot 
            (
                max([Value])
                for [traitDesc] in (' + @cols + ')
            ) p '

	EXEC (@query)

	DROP TABLE #Traits
END
GO