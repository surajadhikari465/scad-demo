USE Icon
GO

IF OBJECT_ID('dbo.[IconRebootTraitCodesData]', 'U') IS NOT NULL
	DROP TABLE dbo.[IconRebootTraitCodesData];

CREATE TABLE [dbo].[IconRebootTraitCodesData] (
	[NAME] [nvarchar](255) NULL
	,TraitCode [nvarchar](255) NULL
	)

-- Creating this in case there are any items with bar code types not in the Infor file
-- These will be loaded later in the Item conversion file
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES t WHERE t.TABLE_SCHEMA = 'dbo' AND t.TABLE_NAME = 'ScanCodeBackup')
	DROP TABLE dbo.ScanCodeBackup
CREATE TABLE dbo.ScanCodeBackup
(
	[scanCodeID]     INT           NOT NULL,
	[itemID]         INT           NOT NULL,
    [scanCode]       NVARCHAR (13) NOT NULL,
    [scanCodeTypeID] INT           NOT NULL,
    [localeID]       INT           NULL,
	[barcodeTypeID]  INT           NULL
)
CREATE NONCLUSTERED INDEX IX_ItemID_ScanCodeBackup on dbo.ScanCodeBackup (itemID ASC);

-- Clear out any PSG Attribute data
RAISERROR ('Updating app.ProductSelectionGroup to NULL attributeId and attributeValue...', 0, 1) WITH NOWAIT
UPDATE app.ProductSelectionGroup
SET AttributeId = NULL
	,AttributeValue = NULL


-- DELETE data that will be loaded in and reseed table
RAISERROR ('Deleting data from tables that will be inserted into...', 0, 1) WITH NOWAIT
IF EXISTS(SELECT 1 FROM dbo.AttributeCharacterSets)
BEGIN
	DELETE FROM AttributeCharacterSets;
	DBCC CHECKIDENT (AttributeCharacterSets, RESEED, 0);
END

IF EXISTS(SELECT 1 FROM dbo.PickListData)
BEGIN
	DELETE FROM PickListData;
	DBCC CHECKIDENT (PickListData, RESEED, 0);
END

IF EXISTS(SELECT 1 FROM dbo.AttributesWebConfiguration)
BEGIN
	DELETE FROM AttributesWebConfiguration;
	DBCC CHECKIDENT (AttributesWebConfiguration, RESEED, 0);
END

IF EXISTS(SELECT 1 FROM dbo.BarcodeTypeRangePool)
BEGIN
	DELETE FROM BarcodeTypeRangePool;
	DBCC CHECKIDENT (BarcodeTypeRangePool, RESEED, 0);
END

IF EXISTS(SELECT 1 FROM dbo.BarcodeType)
BEGIN
	INSERT INTO dbo.ScanCodeBackup SELECT * FROM ScanCode;
	UPDATE dbo.ScanCode SET barCodeTypeID = NULL;
	DELETE FROM BarcodeType;
	DBCC CHECKIDENT (BarcodeType, RESEED, 0);
END

IF EXISTS(SELECT 1 FROM infor.HistoricalAttributes)
BEGIN
	DELETE FROM infor.HistoricalAttributes;
	DBCC CHECKIDENT ('infor.HistoricalAttributes', RESEED, 0);
END

IF EXISTS(SELECT 1 FROM dbo.Attributes)
BEGIN
	DELETE FROM Attributes;
	DBCC CHECKIDENT (Attributes, RESEED, 0)
END

IF EXISTS(SELECT 1 FROM dbo.DataType)
BEGIN
	DELETE FROM DataType;
	DBCC CHECKIDENT (DataType, RESEED, 0);
END

IF EXISTS(SELECT 1 FROM dbo.AttributeGroup)
BEGIN
	DELETE FROM AttributeGroup;
	DBCC CHECKIDENT (AttributeGroup, RESEED, 0);
END

IF OBJECT_ID('dbo.IconRebootDataAfterParsingJson', 'U') IS NOT NULL
	DROP TABLE dbo.IconRebootDataAfterParsingJson;

CREATE TABLE dbo.IconRebootDataAfterParsingJson (
	AttributeId NVARCHAR(max)
	,GeneratorType NVARCHAR(max)
	,InitialValue BIGINT NULL
	,IncrementBy BIGINT NULL
	,InitialMax BIGINT NULL
	,SelectLowestValue BIT NULL
	,DisplayType NVARCHAR(100)
	,RuleValue NVARCHAR(100)
	,RuleType NVARCHAR(100)
	,CharacterSets NVARCHAR(max)
	,ListValues NVARCHAR(max)
	,attributeType NVARCHAR(100)
	,Name NVARCHAR(100)
	,DataType NVARCHAR(100)
	,Description NVARCHAR(max)
	,MultiValued BIT NULL
	,ExternamSystemId NVARCHAR(100)
	,ValidationRequired BIT NULL
	,CreatedBy NVARCHAR(255) NULL
	,CreatedByUserName NVARCHAR(255) NULL
	,CreatedDate NVARCHAR(255) NULL
	,UpdatedBy NVARCHAR(255) NULL
	,UpdatedByUserName NVARCHAR(255) NULL
	,DefaultValue NVARCHAR(max)
	,specialCharacters NVARCHAR(max)
	,DisplayOrder INT
	)

DECLARE @jsonData AS NVARCHAR(MAX)
SELECT @jsonData = BulkColumn
FROM OPENROWSET(BULK '\\ODWD6801\Temp\IconConversion\attributeConfigs.json', SINGLE_CLOB) AS j

SELECT *
INTO #tmpParsedData
FROM OPENJSON(@jsonData)

RAISERROR ('Inserting into dbo.IconRebootDataAfterParsingJson...', 0, 1) WITH NOWAIT
INSERT INTO dbo.IconRebootDataAfterParsingJson (
	AttributeId
	,GeneratorType
	,InitialValue
	,IncrementBy
	,InitialMax
	,SelectLowestValue
	,DisplayType
	,RuleValue
	,RuleType
	,CharacterSets
	,ListValues
	,attributeType
	,Name
	,DataType
	,Description
	,MultiValued
	,ExternamSystemId
	,ValidationRequired
	,CreatedBy
	,CreatedByUserName
	,CreatedDate
	,UpdatedBy
	,UpdatedByUserName
	,DefaultValue
	,specialCharacters
	,DisplayOrder
	)
SELECT id
	,valueGenerator.generatorType
	,valueGenerator.InitialValue
	,valueGenerator.IncrementBy
	,valueGenerator.InitialMax
	,valueGenerator.SelectLowestValue
	,displayType.DisplayType
	,validationRules.Value
	,validationRules.RuleType
	,validationRules.CharacterSets
	,validationRules.[values]
	,attributeType
	,Name
	,DataType
	,Description
	,MultiValued
	,ExternalSystemId
	,ValidationRequired
	,CreatedBy
	,CreatedByUserName
	,CreatedDate
	,UpdatedBy
	,UpdatedByUserName
	,DefaultValue
	,validationRules.specialCharacters
	,DisplayOrder
FROM #tmpParsedData
CROSS APPLY OPENJSON(value) WITH (
		id NVARCHAR(1000)
		,attributeType [nvarchar](255)
		,name [nvarchar](255)
		,dataType [nvarchar](255)
		,description NVARCHAR(Max)
		,multiValued [nvarchar](255)
		,defaultValue [nvarchar](max) AS Json
		,validationRules [nvarchar](max) AS Json
		,externalSystemId [nvarchar](max)
		,validationRequired BIT
		,createdBy [nvarchar](255)
		,createdByUsername [nvarchar](255)
		,createdDate [nvarchar](255)
		,updatedBy [nvarchar](255)
		,updatedByUsername [nvarchar](255)
		,updatedDate [nvarchar](255)
		,displayType [nvarchar](max) AS Json
		,valueGenerator [nvarchar](max) AS Json
		,displayOrder INT
		) AS attributeData
OUTER APPLY openjson(attributeData.validationRules) WITH (
		value NVARCHAR(100)
		,ruleType NVARCHAR(100)
		,characterSets [nvarchar](max) AS Json
		,[values] [nvarchar](max) AS Json
		,specialCharacters [nvarchar](max) AS Json
		) AS validationRules
OUTER APPLY openjson(attributeData.displayType) WITH (displayType NVARCHAR(100)) AS displayType
OUTER APPLY openjson(attributeData.valueGenerator) WITH (
		generatorType NVARCHAR(100)
		,initialValue NVARCHAR(100)
		,incrementBy NVARCHAR(100)
		,initialMax NVARCHAR(100)
		,selectLowestValue NVARCHAR(100)
		) AS valueGenerator
WHERE NAME IS NOT NULL

DROP TABLE #tmpParsedData

-- Insert into Infor Historical Attribute Table
-- This is needed for Infor history lookups
RAISERROR ('Inserting into infor.HistoricalAttribute...', 0, 1) WITH NOWAIT
INSERT INTO infor.HistoricalAttributes (AttributeName,AttributeType, AttributeGuid)
SELECT
	i.[Name] as AttributeName,
	i.attributeType as AttributeType,
	CAST(i.AttributeId as uniqueidentifier) as AttributeGuid
FROM
	(SELECT [Name], AttributeId,attributeType
	FROM IconRebootDataAfterParsingJson
	GROUP BY [Name], AttributeId, attributeType) i
WHERE NOT EXISTS
(
	SELECT 1
	FROM infor.HistoricalAttributes h
	WHERE h.AttributeGuid = CAST(i.AttributeId as uniqueidentifier)
)

SELECT substring(Name, CHARINDEX('(', Name) + 1, CHARINDEX(')', Name) - CHARINDEX('(', Name) - 1) AS range
	,*
INTO #tmpBarCodeType
FROM IconRebootDataAfterParsingJson
WHERE DisplayType = 'Barcode'
	AND name != 'UPC'

RAISERROR ('Inserting into BarcodeType...', 0, 1) WITH NOWAIT
INSERT INTO dbo.[BarcodeType] (
	[BarCodeType]
	,[BeginRange]
	,[EndRange]
	)
SELECT DISTINCT name AS categoryName
	,substring(Range, 0, charindex('-', Range)) AS minimum
	,substring(Range, CHARINDEX('-', Range) + 1, len(Range) - CHARINDEX('-', Range)) AS maximum
FROM #tmpBarCodeType
WHERE substring(name, 0, charindex('(', name)) NOT IN (
		SELECT [BarCodeType]
		FROM dbo.[BarcodeType]
		)

INSERT INTO dbo.[BarcodeType] (
	[BarCodeType]
	,[BeginRange]
	,[EndRange]
	)

VALUES ('UPC',
        Null,
		Null
		)

DROP TABLE #tmpBarCodeType

RAISERROR ('Delete from IconRebootDataAfterParsingJson to remove attributes that aren''t valid...', 0, 1) WITH NOWAIT
DELETE dbo.IconRebootDataAfterParsingJson
WHERE DisplayType = 'Barcode'
	OR attributeType IN  ('REFERENCE_DATA', 'HIERARCHY')
	OR [Name] IN ('dadlkj', 'validated', 'Item Status','displayId','Manufacturer','Pct Tare Weight','Ownership (Brand Level)')
	OR DataType = 'referenceDataSet'

-- Populate DataType table
SET IDENTITY_INSERT dbo.DataType ON
IF NOT EXISTS (SELECT 1 FROM dbo.DataType WHERE DataType = 'Text')
	INSERT INTO dbo.DataType (DataTypeId,DataType)
	VALUES (1,'Text')

IF NOT EXISTS (SELECT 1 FROM dbo.DataType WHERE DataType = 'Number')
	INSERT INTO dbo.DataType (DataTypeId,DataType)
	VALUES (2,'Number')

IF NOT EXISTS (SELECT 1 FROM dbo.DataType WHERE DataType = 'Boolean')
	INSERT INTO dbo.DataType (DataTypeId,DataType)
	VALUES (3,'Boolean')

IF NOT EXISTS (SELECT 1 FROM dbo.DataType WHERE DataType = 'Date')
	INSERT INTO dbo.DataType (DataTypeId,DataType)
	VALUES (4,'Date')
SET IDENTITY_INSERT dbo.DataType OFF

-- Populate AttributeGroup table
SET IDENTITY_INSERT dbo.AttributeGroup ON
IF NOT EXISTS (SELECT 1 FROM AttributeGroup WHERE AttributeGroupName = 'Global Item')
	INSERT INTO AttributeGroup (AttributeGroupId, AttributeGroupName)
	VALUES (1,'Global Item')

IF NOT EXISTS (SELECT 1 FROM AttributeGroup WHERE AttributeGroupName = 'Nutrition')
	INSERT INTO AttributeGroup (AttributeGroupId, AttributeGroupName)
	VALUES(2,'Nutrition')
SET IDENTITY_INSERT dbo.AttributeGroup OFF

DECLARE @attributeGroupId int;
SET @attributeGroupId = (SELECT AttributeGroupId FROM AttributeGroup WHERE AttributeGroupName = 'Global Item')

UPDATE dbo.IconRebootDataAfterParsingJson
SET characterSets = REPLACE(REPLACE(CharacterSets, '[', ''), ']', '')

UPDATE dbo.IconRebootDataAfterParsingJson
SET ListValues = REPLACE(REPLACE(ListValues, '[', ''), ']', '')

UPDATE dbo.IconRebootDataAfterParsingJson
SET DefaultValue = REPLACE(REPLACE(DefaultValue, '[', ''), ']', '')

UPDATE dbo.IconRebootDataAfterParsingJson
SET DefaultValue = Replace(DefaultValue, '''', '')

UPDATE dbo.IconRebootDataAfterParsingJson
SET specialCharacters = REPLACE(REPLACE(specialCharacters, '[', ''), ']', '')

RAISERROR ('Starting cursor to add each attributes.', 0, 1) WITH NOWAIT
---------------------
DECLARE @i INT, @ch CHAR, @specialSet NVARCHAR(max)
DECLARE @OldAttributeId NVARCHAR(max) = ''
DECLARE @AttributeId NVARCHAR(max)
	,@GeneratorType NVARCHAR(max)
	,@InitialValue BIGINT
	,@IncrementBy BIGINT
	,@InitialMax BIGINT
	,@SelectLowestValue BIT
	,@DisplayType NVARCHAR(100)
	,@RuleValue NVARCHAR(100)
	,@RuleType NVARCHAR(100)
	,@CharacterSets NVARCHAR(max)
	,@ListValues NVARCHAR(max)
	,@attributeType NVARCHAR(100)
	,@Name NVARCHAR(100)
	,@DataType NVARCHAR(100)
	,@Description NVARCHAR(max)
	,@MultiValued BIT
	,@ExternamSystemId NVARCHAR(100)
	,@ValidationRequired BIT
	,@CreatedBy NVARCHAR(255)
	,@CreatedByUserName NVARCHAR(255)
	,@CreatedDate NVARCHAR(255)
	,@UpdatedBy NVARCHAR(255)
	,@UpdatedByUserName NVARCHAR(255)
	,@DefaultValue NVARCHAR(255)
	,@specialCharacters NVARCHAR(max)
	,@DisplayOrder INT

DECLARE IconRebootDataAfterParsingJsonCursor CURSOR
FOR
SELECT AttributeId
	,generatorType
	,InitialValue
	,IncrementBy
	,InitialMax
	,SelectLowestValue
	,DisplayType
	,RuleValue
	,RuleType
	,CharacterSets
	,ListValues
	,attributeType
	,Name
	,DataType
	,Description
	,MultiValued
	,ExternamSystemId
	,ValidationRequired
	,CreatedBy
	,CreatedByUserName
	,CreatedDate
	,UpdatedBy
	,UpdatedByUserName
	,DefaultValue
	,specialCharacters
	,DisplayOrder
FROM dbo.IconRebootDataAfterParsingJson

OPEN IconRebootDataAfterParsingJsonCursor

DECLARE @AttributeIdIcon INT

FETCH NEXT
FROM IconRebootDataAfterParsingJsonCursor
INTO @AttributeId
	,@GeneratorType
	,@InitialValue
	,@IncrementBy
	,@InitialMax
	,@SelectLowestValue
	,@DisplayType
	,@RuleValue
	,@RuleType
	,@CharacterSets
	,@ListValues
	,@attributeType
	,@Name
	,@DataType
	,@Description
	,@MultiValued
	,@ExternamSystemId
	,@ValidationRequired
	,@CreatedBy
	,@CreatedByUserName
	,@CreatedDate
	,@UpdatedBy
	,@UpdatedByUserName
	,@DefaultValue
	,@specialCharacters
	,@DisplayOrder
WHILE @@FETCH_STATUS = 0
BEGIN
	IF (@RuleType = 'CharacterSet')
	BEGIN
		INSERT INTO CharacterSets (
			Name
			,RegEx
			)
		SELECT Replace(VALUE, '"', '')
			,'' AS VALUE
		FROM STRING_SPLIT(@CharacterSets, ',')
		WHERE Replace(VALUE, '"', '') NOT IN (
				SELECT Name
				FROM CharacterSets
				)
			AND Replace(VALUE, '"', '') != 'SPECIAL'
	END

	UPDATE CharacterSets
	SET RegEx = '[A-Z]*'
	WHERE Name = 'UPPERCASE'

	UPDATE CharacterSets
	SET RegEx = '[a-z]*'
	WHERE Name = 'LOWERCASE'

	UPDATE CharacterSets
	SET RegEx = '[0-9]*'
	WHERE Name = 'NUMERIC'

	UPDATE CharacterSets
	SET RegEx = '\s'
	WHERE Name = 'WHITESPACE'

	DECLARE @IsUnique BIT
	DECLARE @DataTypeId INT = (
			SELECT TOP 1 datatypeId
			FROM DataType
			WHERE DataType = @DataType
			)
	DECLARE @MaxLengthAllowed INT = (
			SELECT rulevalue
			FROM dbo.IconRebootDataAfterParsingJson
			WHERE attributeId = @AttributeId
				AND RuleType = 'MaxLength'
			)
	DECLARE @HasPickList BIT = 0
	DECLARE @Min NVARCHAR(14)
	DECLARE @Max NVARCHAR(14)
	DECLARE @NumberOfDecimal NVARCHAR(1)
	DECLARE @SpecialCharactersAllowed NVARCHAR(max) = NULL
	DECLARE @TraitDescription NVARCHAR(255)

	IF (@OldAttributeId <> @AttributeId)
	BEGIN
		IF EXISTS (
				SELECT 1
				FROM dbo.IconRebootDataAfterParsingJson
				WHERE attributeId = @AttributeId
					AND RuleType = 'ListConstraint'
				)
		BEGIN
			SET @HasPickList = 1
		END
		ELSE
			SET @HasPickList = 0

		IF EXISTS (
				SELECT 1
				FROM dbo.IconRebootDataAfterParsingJson
				WHERE attributeId = @AttributeId
					AND RuleType = 'CharacterSet'
					AND specialCharacters IS NOT NULL
				)
		BEGIN
			SET @specialCharactersAllowed = (
					SELECT TOP 1 Replace(specialCharacters, '"', '')
					FROM dbo.IconRebootDataAfterParsingJson
					WHERE attributeId = @AttributeId
						AND RuleType = 'CharacterSet'
						AND specialCharacters IS NOT NULL
					)
			--Extract unique characters
			SET @specialSet = ''
			SET @i = (SELECT CHARINDEX(',,,', @specialCharactersAllowed))
			SET @specialCharactersAllowed = REPLACE(replace(@specialCharactersAllowed,' ', '') ,',','') + (case when @i > 0 then ',' else '' end)
			SET @i = 1

			WHILE(@i < LEN(@specialCharactersAllowed) + 1)
			BEGIN
				SELECT @ch = SUBSTRING(@specialCharactersAllowed, @i, 1)
				if(CHARINDEX(@ch, @specialSet) = 0) SET @specialSet += @ch;
				SET @i += 1
			END

			SET @specialCharactersAllowed = CASE WHEN Len(@specialSet) > 0 THEN @specialSet ELSE NULL END
		END

		IF EXISTS (
				SELECT 1
				FROM dbo.IconRebootDataAfterParsingJson
				WHERE attributeId = @AttributeId
					AND RuleType = 'UniqueValue'
				)
		BEGIN
			SET @IsUnique = 1
		END
		ELSE
			SET @IsUnique = 0

		IF EXISTS (
				SELECT 1
				FROM dbo.IconRebootDataAfterParsingJson
				WHERE attributeId = @AttributeId
					AND (
						RuleType = 'MinValue'
						OR RuleType = 'MaxValue'
						OR RuleType = 'NumberOfDecimals'
						)
				)
		BEGIN
			IF EXISTS (
					SELECT 1
					FROM dbo.IconRebootDataAfterParsingJson
					WHERE attributeId = @AttributeId
						AND RuleType = 'MinValue'
					)
				SET @Min =	(SELECT RuleValue
							FROM dbo.IconRebootDataAfterParsingJson
							WHERE attributeId = @AttributeId
								AND RuleType = 'MinValue')

			IF EXISTS (
					SELECT 1
					FROM dbo.IconRebootDataAfterParsingJson
					WHERE attributeId = @AttributeId
						AND RuleType = 'MaxValue'
					)
				SET @Max =	(SELECT RuleValue
							FROM dbo.IconRebootDataAfterParsingJson
							WHERE attributeId = @AttributeId
									AND RuleType = 'MaxValue')

			IF EXISTS (
					SELECT 1
					FROM dbo.IconRebootDataAfterParsingJson
					WHERE attributeId = @AttributeId
						AND RuleType = 'NumberOfDecimals'
					)
				SET @NumberOfDecimal = 
							(SELECT RuleValue
							FROM dbo.IconRebootDataAfterParsingJson
							WHERE attributeId = @AttributeId
								AND RuleType = 'NumberOfDecimals')
		END

		SET @TraitDescription = (SELECT COALESCE((SELECT traitDesc from dbo.Trait where traitCode=@ExternamSystemId),@Name))
		-- Remove special characters
		DECLARE @attributeNameKey NVARCHAR(100);
		SET @attributeNameKey = REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@Name, ' ', ''), '/', ''), '(', ''), ')', ''), '-', ''), ':', '');

		INSERT INTO [dbo].[Attributes] (
			[DisplayName]
			,[AttributeName]
			,[AttributeGroupId]
			,[HasUniqueValues]
			,[Description]
			,[DefaultValue]
			,[IsRequired]
			,[SpecialCharactersAllowed]
			,[TraitCode]
			,[DataTypeId]
			,[DisplayOrder]
			,[InitialValue]
			,[IncrementBy]
			,[InitialMax]
			,[DisplayType]
			,[MaxLengthAllowed]
			,[MinimumNumber]
			,[MaximumNumber]
			,[NumberOfDecimals]
			,[IsPickList]
			,[XmlTraitDescription]
			,[AttributeGuid]
			)
		VALUES (
			@Name
			,@attributeNameKey
			,@attributeGroupId
			,@IsUnique
			,@Description
			,NULLIF(@DefaultValue, '') -- prevent inserting empty string
			,@ValidationRequired
			,@specialCharactersAllowed
			,@ExternamSystemId
			,@DataTypeId
			,@DisplayOrder
			,@InitialValue
			,@IncrementBy
			,@InitialMax
			,@DisplayType
			,CASE 
				WHEN @DataType = 'Text' THEN
					CASE
						WHEN @HasPickList = 0 THEN
							CASE WHEN @MaxLengthAllowed IS NULL THEN 255 ELSE @MaxLengthAllowed END
						ELSE
							CASE WHEN ISNULL(@MaxLengthAllowed, 255) > 50 THEN 50 ELSE @MaxLengthAllowed END --Set any Max LEN for picklists which are > 50 char to 50
						END
				END
			,CASE
				WHEN @DataType = 'Number' THEN ISNULL(@Min, '0')
				ELSE @Min
				END
			,CASE
				WHEN @DataType = 'Number' THEN ISNULL(@Max, '9999999999.9999')
				ELSE @Max
				END
			,CASE
				WHEN @DataType = 'Number' THEN ISNULL(@NumberOfDecimal, '4')
				ELSE @NumberOfDecimal
				END
			,@HasPickList
			,@TraitDescription
			,CAST(@AttributeId as uniqueidentifier)
			)

		SET @AttributeIdIcon = SCOPE_IDENTITY()
	END

	IF (@RuleType = 'CharacterSet')
	BEGIN
		SELECT Replace(VALUE, '"', '') AS CharacterSet
		INTO #tmp2
		FROM STRING_SPLIT(@CharacterSets, ',');

		INSERT INTO [dbo].[AttributeCharacterSets] (
			[AttributeId]
			,[CharacterSetId]
			)
		SELECT @AttributeIdIcon
			,CharacterSets.CharacterSetId
		FROM #tmp2
		INNER JOIN CharacterSets ON #tmp2.CharacterSet = CharacterSets.Name

		DROP TABLE #tmp2
	END

	IF (@RuleType = 'ListConstraint')
	BEGIN
		SELECT Replace(VALUE, '"', '') AS PickListValue
		INTO #tmp3
		FROM STRING_SPLIT(@ListValues, ',')

		INSERT INTO [dbo].[PickListData] (
			[AttributeId]
			,[PickListValue]
			)
		SELECT @AttributeIdIcon
			,PickListValue
		FROM #tmp3

		DROP TABLE #tmp3
	END

	SET @OldAttributeId = @AttributeId
	SET @AttributeId = null
	SET @Name  = null
	SET @attributeNameKey  = null
	SET @IsUnique  = null
	SET @Description  = null
	SET @ValidationRequired  = null
	SET @specialCharactersAllowed  = null
	SET @ExternamSystemId  = null
	SET @DataTypeId  = null
	SET @DisplayOrder  = null
	SET @InitialValue  = null
	SET @IncrementBy  = null
	SET @InitialMax  = null
	SET @DisplayType  = null
	SET @MaxLengthAllowed  = null
	SET @HasPickList = null
	SET @Description = null
	SET @AttributeId = null
	SET @TraitDescription = null
	SET @Max = null
	SET @Min = null
	SET @NumberOfDecimal = null

	FETCH NEXT
	FROM IconRebootDataAfterParsingJsonCursor
	INTO @AttributeId
		,@GeneratorType
		,@InitialValue
		,@IncrementBy
		,@InitialMax
		,@SelectLowestValue
		,@DisplayType
		,@RuleValue
		,@RuleType
		,@CharacterSets
		,@ListValues
		,@attributeType
		,@Name
		,@DataType
		,@Description
		,@MultiValued
		,@ExternamSystemId
		,@ValidationRequired
		,@CreatedBy
		,@CreatedByUserName
		,@CreatedDate
		,@UpdatedBy
		,@UpdatedByUserName
		,@DefaultValue
		,@specialCharacters
		,@DisplayOrder
END

CLOSE IconRebootDataAfterParsingJsonCursor;

DEALLOCATE IconRebootDataAfterParsingJsonCursor;

RAISERROR ('Done adding each attribute in cursor.', 0, 1) WITH NOWAIT

RAISERROR ('Inserting into IconRebootTraitCodesData table...', 0, 1) WITH NOWAIT

IF EXISTS (SELECT 1 FROM IconRebootTraitCodesData)
	DELETE FROM IconRebootTraitCodesData

DECLARE @nutritionAttributeGroupId INT = (
		SELECT TOP 1 AttributeGroupId
		FROM AttributeGroup
		WHERE AttributeGroupName = 'Nutrition'
		)
DECLARE @dataTypeTextId INT = (
		SELECT TOP 1 DataTypeId
		FROM datatype
		WHERE DataType = 'Text'
		)

RAISERROR ('Inserting nutrition traits into dbo.Attributes table...', 0, 1) WITH NOWAIT
INSERT INTO [dbo].[Attributes] (
	[DisplayName]
	,[AttributeName]
	,[AttributeGroupId]
	,[HasUniqueValues]
	,[Description]
	,[DefaultValue]
	,[IsRequired]
	,[SpecialCharactersAllowed]
	,[TraitCode]
	,[DataTypeId]
	,[DisplayOrder]
	,[InitialValue]
	,[IncrementBy]
	,[InitialMax]
	,[DisplayType]
	,[MaxLengthAllowed]
	,[IsPickList]
	,[XmlTraitDescription]
	,[AttributeGuid]
	)
SELECT traitDesc
	,Replace(traitDesc, ' ', '')
	,@nutritionAttributeGroupId
	,0
	,traitDesc
	,NULL
	,0
	,NULL
	,traitCode
	,@dataTypeTextId
	,NULL
	,NULL
	,NULL
	,NULL
	,NULL
	,NULL
	,0
	,traitDesc
	,@AttributeId
FROM trait
WHERE traitcode IN (
		'RCN'
		,'ALR'
		,'ING'
		,'HSH'
		,'PUF'
		,'MUF'
		,'PTW'
		,'PTP'
		,'DFP'
		,'SBF'
		,'ISF'
		,'SAL'
		,'OCH'
		,'PPT'
		,'BCN'
		,'VTD'
		,'VTE'
		,'THM'
		,'RFN'
		,'NAC'
		,'VB6'
		,'FLT'
		,'B12'
		,'BTN'
		,'PAD'
		,'PPH'
		,'IDN'
		,'MGM'
		,'ZNC'
		,'CPR'
		,'TSF'
		,'O6F'
		,'O3F'
		,'STR'
		,'CHR'
		,'CHM'
		,'VTK'
		,'MGE'
		,'MBD'
		,'SLM'
		,'TFW'
		,'CTF'
		,'CSF'
		,'SPC'
		,'SSD'
		,'SPP'
		,'SUT'
		,'SWT')

RAISERROR ('Updating ProductSelectionGroup table...', 0, 1) WITH NOWAIT
UPDATE ps
SET AttributeId = a.AttributeId
	,AttributeValue = ps.traitValue
FROM app.ProductSelectionGroup ps
INNER JOIN Trait ON ps.TraitId = trait.traitID
INNER JOIN Attributes a ON a.TraitCode = Trait.traitCode

-- Insert attributes that weren't on the attribute file but on the item file
-- or attributes that didn't map one to one
-- Inactive; TraitCode: HID
-- Created By; CreatedBy; TraitCode: ?
-- Created On; CreatedDateTime; TraitCode: INS
-- Modified By; ModifiedBy; TraitCode: USR
-- Modified On; ModifiedDateTime; TraitCode: MOD
RAISERROR ('Inserting special attributes Inactive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn into Attributes table...', 0, 1) WITH NOWAIT
DECLARE @dataTypeBoolId INT = (
		SELECT DataTypeId
		FROM DataType
		WHERE DataType = 'Boolean'
		);
DECLARE @dataTypeDateId INT = (
		SELECT DataTypeId
		FROM DataType
		WHERE DataType = 'Date'
		);
DECLARE @maxDisplayOrder INT = (
		SELECT MAX(DisplayOrder)
		FROM Attributes
		);

INSERT INTO [dbo].[Attributes] (
	[DisplayName]
	,[AttributeName]
	,[AttributeGroupId]
	,[HasUniqueValues]
	,[Description]
	,[DefaultValue]
	,[IsRequired]
	,[SpecialCharactersAllowed]
	,[TraitCode]
	,[DataTypeId]
	,[DisplayOrder]
	,[InitialValue]
	,[IncrementBy]
	,[InitialMax]
	,[DisplayType]
	,[MaxLengthAllowed]
	,[IsPickList]
	,[XmlTraitDescription]
	)
VALUES
	('Inactive'
	,'Inactive'
	,@attributeGroupId
	,0
	,'Indicates if the item is marked as inactive so it doesn''t appear by default when searching for items.  Previously part of Item Status and published out as the ''Hidden'' trait.'
	,NULL
	,1
	,NULL
	,'HID'
	,@dataTypeBoolId
	,(@maxDisplayOrder + 1)
	,NULL
	,NULL
	,NULL
	,NULL
	,NULL
	,0
	,'Hidden Item'
),
(	
	'Created By'
	,'CreatedBy'
	,@attributeGroupId
	,0
	,'User who created the item'
	,NULL
	,NULL
	,'All'
	,NULL -- TODO:  Add TraitCode
	,@dataTypeTextId
	,(@maxDisplayOrder + 2)
	,NULL
	,NULL
	,NULL
	,NULL
	,NULL
	,0
	,'Created By'
),
(
	'Created On'
	,'CreatedDateTimeUtc'
	,@attributeGroupId
	,0
	,'Date & Time of when the item was created.'
	,NULL
	,NULL
	,NULL
	,'INS'
	,@dataTypeDateId
	,(@maxDisplayOrder + 3)
	,NULL
	,NULL
	,NULL
	,NULL
	,NULL
	,0
	,'Insert Date'
),
(
	'Modified By'
	,'ModifiedBy'
	,@attributeGroupId
	,0
	,'User who last modified the item.'
	,NULL
	,NULL
	,'All'
	,'USR'
	,@dataTypeTextId
	,(@maxDisplayOrder + 4)
	,NULL
	,NULL
	,NULL
	,NULL
	,NULL
	,0
	,'Modified User'
),
(
	'Modified On'
	,'ModifiedDateTimeUtc'
	,@attributeGroupId
	,0
	,'DateTime when item was last modified.'
	,NULL
	,NULL
	,'All'
	,'MOD'
	,@dataTypeDateId
	,(@maxDisplayOrder + 5)
	,NULL
	,NULL
	,NULL
	,NULL
	,NULL
	,0
	,'Modified Date'
),
(
	'Department Sale'
	,'DepartmentSale'
	,@attributeGroupId
	,0
	,'Indicates if an item is a department sale item'
	,NULL
	,NULL
	,'All'
	,'DPT'
	,@dataTypeTextId
	,(@maxDisplayOrder + 6)
	,NULL
	,NULL
	,NULL
	,NULL
	,50
	,1
	,'Department Sale'
)

INSERT INTO [dbo].[PickListData] (
			[AttributeId]
			,[PickListValue]
			)
VALUES  (
(SELECT AttributeId from dbo.Attributes where AttributeName = 'DepartmentSale')
,'No'
)

INSERT INTO [dbo].[PickListData] (
			[AttributeId]
			,[PickListValue]
			)
VALUES  (
(SELECT AttributeId from dbo.Attributes where AttributeName = 'DepartmentSale')
,'Yes'
)

INSERT INTO dbo.AttributeCharacterSets (
[AttributeId]
,[CharacterSetId])
VALUES
(
	(SELECT AttributeId From Attributes where AttributeName = 'DepartmentSale'),
	(SELECT CharacterSetId from CharacterSets WHERE Name = 'UPPERCASE')
)

INSERT INTO dbo.AttributeCharacterSets (
[AttributeId]
,[CharacterSetId])
VALUES
(
	(SELECT AttributeId From Attributes where AttributeName = 'DepartmentSale'),
	(SELECT CharacterSetId from CharacterSets WHERE Name = 'LOWERCASE')
)


INSERT INTO dbo.AttributeCharacterSets (
[AttributeId]
,[CharacterSetId])
VALUES
(
	(SELECT AttributeId From Attributes where AttributeName = 'DepartmentSale'),
	(SELECT CharacterSetId from CharacterSets WHERE Name = 'NUMERIC')
)

INSERT INTO dbo.AttributeCharacterSets (
[AttributeId]
,[CharacterSetId])
VALUES
(
	(SELECT AttributeId From Attributes where AttributeName = 'DepartmentSale'),
	(SELECT CharacterSetId from CharacterSets WHERE Name = 'WHITESPACE')
)


RAISERROR ('Inserting into dbo.AttributesWebConfiguration table...', 0, 1) WITH NOWAIT
INSERT INTO dbo.AttributesWebConfiguration (
	AttributeId
	,GridColumnWidth
	)
SELECT AttributeId
	,200
FROM Attributes
WHERE AttributeId NOT IN (SELECT AttributeId FROM dbo.AttributesWebConfiguration)


SET @maxDisplayOrder = (SELECT MAX(DisplayOrder) FROM Attributes)

RAISERROR ('Updating display order for attributes that don''t have one...', 0, 1) WITH NOWAIT
UPDATE dbo.Attributes
SET @maxDisplayOrder = DisplayOrder = @maxDisplayOrder + 1
WHERE DisplayOrder IS NULL

--Set ReadOnly on attributes that should not be edited directly by a user through Icon Web
RAISERROR ('Setting ReadOnly property on AttributesWebConfiguration...', 0, 1) WITH NOWAIT
UPDATE dbo.AttributesWebConfiguration
SET IsReadOnly = 1
WHERE AttributeId IN (SELECT AttributeId FROM dbo.Attributes WHERE AttributeName IN ('ProhibitDiscount','CreatedBy','CreatedDateTimeUtc','ModifiedBy','ModifiedDateTimeUtc'))

--Set IsReadOnly for Nutrition attributes
RAISERROR ('Setting ReadOnly property on AttributesWebConfiguration for nutrition attributes...', 0, 1) WITH NOWAIT
UPDATE awc SET IsReadOnly = 1
  FROM AttributesWebConfiguration awc
  INNER JOIN Attributes a on a.AttributeId = awc.AttributeId
  INNER JOIN AttributeGroup ag on ag.AttributeGroupId = a.AttributeGroupId
  WHERE ag.AttributeGroupName = 'Nutrition';

--Update all text fields to set SpecialCharactersAllowed to All unless the special characters are specified
RAISERROR ('Updating special characters allowed for any that don''t have it set...', 0, 1) WITH NOWAIT
UPDATE dbo.Attributes
SET SpecialCharactersAllowed = 'All'
WHERE DataTypeId = 1
	AND ISNULL(SpecialCharactersAllowed, '') = ''

IF OBJECT_ID('tempdb..#characterSetRegexPattern') IS NOT NULL DROP TABLE #characterSetRegexPattern
GO

--Build the CharacterSetRegexPattern on Attributes from CharacterSet so that the regex pattern can be used to validate items
DECLARE @UpperCasePattern NVARCHAR(3) = 'A-Z'
DECLARE @LowerCaseCasePattern NVARCHAR(3) = 'a-z'
DECLARE @NumericPattern NVARCHAR(3) = '0-9'
DECLARE @WhitespacePattern NVARCHAR(3) = '\s'

IF OBJECT_ID('tempdb..#characterSetRegexPattern') IS NOT NULL
	DROP TABLE #characterSetRegexPattern

CREATE TABLE #characterSetRegexPattern
(
	AttributeId INT,
	SpecialCharactersAllowed NVARCHAR(100),
	UpperCasePattern NVARCHAR(3),
	LowerCasePattern NVARCHAR(3),
	NumericPattern NVARCHAR(3),
	WhitespacePattern NVARCHAR(3)
)

--Need to replace regex special characters with the escaped version of them so that we build correct regex from special characters
INSERT INTO #characterSetRegexPattern(AttributeId, SpecialCharactersAllowed)
SELECT 
	AttributeId,
	REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(SpecialCharactersAllowed, '.', '\.'), '$', '\$'), '^', '\^'), '{', '\{'), '}', '\}'), '[', '\['), ']', '\]'), '(', '\('), ')', '\)'), '*', '\*'), '+', '\+'), '?', '\?'), '\', '\\'), '-', '\-')
FROM Attributes

UPDATE #characterSetRegexPattern
SET UpperCasePattern = @UpperCasePattern
WHERE EXISTS 
(
	SELECT 1 
	FROM AttributeCharacterSets a 
	JOIN CharacterSets cs ON a.CharacterSetId = cs.CharacterSetId
	WHERE a.AttributeId = AttributeId
		AND cs.Name = 'UPPERCASE'
)

UPDATE #characterSetRegexPattern
SET LowerCasePattern = @LowerCaseCasePattern
WHERE EXISTS 
(
	SELECT 1 
	FROM AttributeCharacterSets a 
	JOIN CharacterSets cs ON a.CharacterSetId = cs.CharacterSetId
	WHERE a.AttributeId = AttributeId
		AND cs.Name = 'LOWERCASE'
)

UPDATE #characterSetRegexPattern
SET NumericPattern = @NumericPattern
WHERE EXISTS 
(
	SELECT 1 
	FROM AttributeCharacterSets a 
	JOIN CharacterSets cs ON a.CharacterSetId = cs.CharacterSetId
	WHERE a.AttributeId = AttributeId
		AND cs.Name = 'NUMERIC'
)

UPDATE #characterSetRegexPattern
SET WhitespacePattern = @WhitespacePattern
WHERE EXISTS 
(
	SELECT 1 
	FROM AttributeCharacterSets a 
	JOIN CharacterSets cs ON a.CharacterSetId = cs.CharacterSetId
	WHERE a.AttributeId = AttributeId
		AND cs.Name = 'WHITESPACE'
)

RAISERROR ('Updating CharacterSetRegexPattern table...', 0, 1) WITH NOWAIT
UPDATE awc
SET CharacterSetRegexPattern =
	CASE 
		WHEN c.SpecialCharactersAllowed = 'All' THEN '^.*$' -- If Special Characters Allowed is ALL then allow every character.
		ELSE CONCAT('^[', c.LowerCasePattern, c.UpperCasePattern, c.NumericPattern, c.WhitespacePattern, c.SpecialCharactersAllowed, ']*$')
	END
FROM dbo.AttributesWebConfiguration awc
JOIN #characterSetRegexPattern c ON awc.AttributeId = c.AttributeId

GO

-- TODO: this is a temporary fix to remove the duplicate CountryofOrigin attribute that keeps getting converted twice and breaking everything. 
-- We're deleting the record that does not have pick list data.
delete FROM dbo.Attributes
where attributeId in (SELECT attributeId from Attributes wHERE AttributeGuid = '633873B1-03B3-4E28-8093-8F06BFE44BCF')
AND NOT EXISTS (SELECT * FROM dbo.PickListData pld where pld.AttributeId = dbo.Attributes.AttributeId)


IF NOT EXISTS (SELECT 1 FROM dbo.[Hierarchy] WHERE [hierarchyName] = 'Manufacturer')
BEGIN
	SET IDENTITY_INSERT dbo.Hierarchy ON
		INSERT INTO [dbo].[Hierarchy] ([hierarchyID], [hierarchyName]) VALUES (8, 'Manufacturer')
	SET IDENTITY_INSERT dbo.Hierarchy OFF
END

--Update dbo.BarCodetype Table Set flag to 1 for Scale Plu
UPDATE dbo.barcodetype
SET ScalePLU = 1
WHERE BarcodeType LIKE '%Scale PLU%'

-- IsSpecialTransform is a flag we use to determine if we should use attribute transformation when sending to the ESB.
-- When this is enabled we will send 0/1 for Yes/No and there are other exceptions.
-- Eventually when the rest of the company settles on the same data types this column will be deprecated.
-- We only want to apply this special transformation to attributes that are know to othe consumers which is this list. 
UPDATE dbo.Attributes 
SET IsSpecialTransform=1
WHERE TraitCode IN
('PRD',
'POS',
'PKG',
'FSE',
'RSZ',
'RUM',
'PRH',
'GF',
'PBC',
'WT',
'NGM',
'VGN',
'VEG',
'KSH',
'ECO',
'OG',
'INS',
'MOD',
'HID',
'AWR',
'BIO',
'CMT',
'CR',
'HER',
'ACH',
'DAG',
'FRR',
'GRF',
'MIH',
'MSC',
'PAS',
'SFT',
'SFF',
'HSH',
'RCN',
'ALR',
'ING',
'SLM',
'PUF',
'MUF',
'PTW',
'PTP',
'DFP',
'SBF',
'ISF',
'SAL',
'OCH',
'PPT',
'BCN',
'VTD',
'VTE',
'THM',
'RFN',
'NAC',
'VB6',
'FLT',
'B12',
'BTN',
'PAD',
'PPH',
'IDN',
'MGM',
'ZNC',
'CPR',
'TSF',
'O6F',
'O3F',
'STR',
'CHR',
'CHM',
'VTK',
'MGE',
'MBD',
'CTF',
'CSF',
'SPC',
'SSD',
'SPP',
'SUT',
'SWT',
'TFW',
'DS',
'CF',
'FTC',
'HEM',
'OPC',
'NR',
'PFT',
'PLO',
'LLP',
'CFD',
'GPP',
'FXT',
'MOG',
'PRB',
'RFA',
'RFD',
'SMF',
'WIC',
'SLF',
'ITG',
'LIN',
'SKU',
'PL',
'VS',
'ESN',
'PNE',
'ESE',
'TSE',
'WFE',
'OTE',
'DAT',
'NGT',
'IDP',
'IHT',
'IWD',
'CUB',
'IWT',
'TDP',
'THT',
'TWD',
'LBL',
'COO',
'PG',
'PGT',
'PRL',
'APL',
'FT',
'GFC',
'NGC',
'OC',
'VAR',
'BES',
'LEX')