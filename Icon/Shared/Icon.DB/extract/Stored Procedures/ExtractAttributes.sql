CREATE PROCEDURE [extract].[ExtractAttributes]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON;

	--Create temp tables for picklists and character sets
	IF OBJECT_ID('tempdb..#pickLists') IS NOT NULL
		DROP TABLE #pickLists

	CREATE TABLE #pickLists (
		AttributeId INT
		,PickListValues NVARCHAR(max)
		)

	IF OBJECT_ID('tempdb..#characterSets') IS NOT NULL
		DROP TABLE #characterSets

	CREATE TABLE #characterSets (
		AttributeId INT
		,CharacterSets NVARCHAR(max)
		)

	INSERT INTO #pickLists (
		AttributeId
		,PickListValues
		)
	SELECT a.AttributeId
		,''
	FROM AttributesView a
	WHERE EXISTS (
			SELECT 1
			FROM PickListData pld
			WHERE pld.AttributeId = a.AttributeId
			)

	INSERT INTO #characterSets (
		AttributeId
		,CharacterSets
		)
	SELECT a.AttributeId
		,''
	FROM AttributesView a
	WHERE EXISTS (
			SELECT 1
			FROM AttributeCharacterSets acs
			WHERE acs.AttributeId = a.AttributeId
			)

	--Populate temp tables
	DECLARE @AttributeId INT
		,@PickListValue NVARCHAR(50)
		,@CharacterSet NVARCHAR(50)

	DECLARE pick_list_cursor CURSOR
	FOR
	SELECT AttributeId
		,PickListValue
	FROM dbo.PickListData
	ORDER BY AttributeId
		,PickListValue

	OPEN pick_list_cursor

	FETCH NEXT
	FROM pick_list_cursor
	INTO @AttributeId
		,@PickListValue

	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE #pickLists
		SET PickListValues = CASE 
				WHEN @PickListValue LIKE '%''%'
					THEN PickListValues + ', ' + '"' + @PickListValue + '"'
				ELSE PickListValues + ', ''' + @PickListValue + ''''
				END
		WHERE AttributeId = @AttributeId

		FETCH NEXT
		FROM pick_list_cursor
		INTO @AttributeId
			,@PickListValue
	END

	CLOSE pick_list_cursor

	UPDATE #pickLists
	SET PickListValues = '[' + SUBSTRING(PickListValues, 3, LEN(PickListValues)) + ']'

	DECLARE character_set_cursor CURSOR
	FOR
	SELECT AttributeId
		,cs.Name
	FROM dbo.AttributeCharacterSets acs
	JOIN dbo.CharacterSets cs ON acs.CharacterSetId = cs.CharacterSetId

	OPEN character_set_cursor

	FETCH NEXT
	FROM character_set_cursor
	INTO @AttributeId
		,@CharacterSet

	WHILE @@FETCH_STATUS = 0
	BEGIN
		UPDATE #characterSets
		SET CharacterSets = CharacterSets + ', ' + '''' + @CharacterSet + ''''
		WHERE AttributeId = @AttributeId

		FETCH NEXT
		FROM character_set_cursor
		INTO @AttributeId
			,@CharacterSet
	END

	CLOSE character_set_cursor

	UPDATE ch
	SET CharacterSets = CharacterSets + ', ''SPECIAL'''
	FROM #characterSets ch
	WHERE EXISTS (
			SELECT 1
			FROM AttributesView a
			WHERE a.AttributeId = ch.AttributeId
				AND a.SpecialCharactersAllowed IS NOT NULL
			)

	UPDATE #characterSets
	SET CharacterSets = '[' + SUBSTRING(CharacterSets, 3, LEN(CharacterSets)) + ']'

	SELECT TraitCode
		,AttributeName
		,DisplayName
		,dt.DataType
		,a.Description
		,a.DefaultValue
		,ag.AttributeGroupName
		,a.MaxLengthAllowed
		,a.MinimumNumber
		,a.MaximumNumber
		,a.NumberOfDecimals
		,a.IsRequired
		,a.SpecialCharactersAllowed
		,p.PickListValues
		,c.CharacterSets
		,a.DisplayOrder
	FROM Attributes a
	JOIN AttributeGroup ag ON a.AttributeGroupId = ag.AttributeGroupId
	JOIN DataType dt ON a.DataTypeId = dt.DataTypeId
	LEFT JOIN #pickLists p ON a.AttributeId = p.AttributeId
	LEFT JOIN #characterSets c ON a.AttributeId = c.AttributeId
	ORDER BY AttributeGroupName
		,a.AttributeId
END
GO
