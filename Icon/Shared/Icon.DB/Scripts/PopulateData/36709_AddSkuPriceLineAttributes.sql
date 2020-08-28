DECLARE @key VARCHAR(128) = 'AddSkuAndPriceLineAttributes';
DECLARE @scopeIdentity INT

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN
	DECLARE @SkuAttributeGroupId INT = (
			SELECT AttributeGroupID
			FROM AttributeGroup
			WHERE AttributeGroupName = 'SKU'
			)
	DECLARE @PriceLineAttributeGroupId INT = (
			SELECT AttributeGroupID
			FROM AttributeGroup
			WHERE AttributeGroupName = 'PriceLine'
			)
	DECLARE @DataTypeTextID INT = (
			SELECT DataTypeId
			FROM DataType
			WHERE DataType = 'Text'
			)
	DECLARE @DataTypeNumberID INT = (
			SELECT DataTypeId
			FROM DataType
			WHERE DataType = 'Number'
			)
	DECLARE @MaxDisplayOrder INT = (
			SELECT max(DisplayOrder)
			FROM Attributes
			)
	DECLARE @MaxItemColumnDisplayOrder INT = (
			SELECT max(DisplayOrder)
			FROM ItemColumnDisplayOrder
			)

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Attributes
			WHERE AttributeName = 'SKUDescription'
				AND AttributeGroupId = @SkuAttributeGroupId
			)
	BEGIN
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
			,[IsSpecialTransform]
			,[IsActive]
			,[LastModifiedBy]
			)
		VALUES (
			'SKU Description'
			,'SKUDescription'
			,@SkuAttributeGroupId
			,0
			,'Sku Description'
			,NULL
			,1
			,' [,./'';]=-\`~!@#$%^&*()_+|}{":?><'
			,'SKD'
			,@DataTypeTextID
			,@MaxDisplayOrder + 1
			,NULL
			,NULL
			,NULL
			,NULL
			,255
			,NULL
			,NULL
			,NULL
			,0
			,'Sku Description'
			,NULL
			,0
			,1
			,'Script'
			)

		SET @scopeIdentity = SCOPE_IDENTITY()

		INSERT INTO dbo.AttributesWebConfiguration (
			AttributeId
			,GridColumnWidth
			,IsReadOnly
			,CharacterSetRegexPattern
			)
		VALUES (
			@scopeIdentity
			,200
			,0
			,'^[A-Za-z0-9\s!"#\$%&''\(\)\*\+,\-\./:;<=>\?@\[\\\]\^_`\{\|\}~]*$'
			)

		INSERT INTO [dbo].[AttributeCharacterSets] (
			[AttributeId]
			,[CharacterSetId]
			)
		SELECT @scopeIdentity
			,[CharacterSetId]
		FROM [dbo].[CharacterSets]
	END

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Attributes
			WHERE AttributeName = 'PriceLineDescription'
				AND AttributeGroupId = @PriceLineAttributeGroupId
			)
	BEGIN
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
			,[IsSpecialTransform]
			,[IsActive]
			,[LastModifiedBy]
			)
		VALUES (
			'Price Line Description'
			,'PriceLineDescription'
			,@PriceLineAttributeGroupId
			,0
			,'Price Line Description'
			,NULL
			,1
			,' [,./'';]=-\`~!@#$%^&*()_+|}{":?><'
			,'PLD'
			,@DataTypeTextID
			,@MaxDisplayOrder + 2
			,NULL
			,NULL
			,NULL
			,NULL
			,255
			,NULL
			,NULL
			,NULL
			,0
			,'Price Line Description'
			,NULL
			,0
			,1
			,'Script'
			)

		SET @scopeIdentity = SCOPE_IDENTITY()

		INSERT INTO dbo.AttributesWebConfiguration (
			AttributeId
			,GridColumnWidth
			,IsReadOnly
			,CharacterSetRegexPattern
			)
		VALUES (
			@scopeIdentity
			,200
			,0
			,'^[A-Za-z0-9\s!"#\$%&''\(\)\*\+,\-\./:;<=>\?@\[\\\]\^_`\{\|\}~]*$'
			)

		INSERT INTO [dbo].[AttributeCharacterSets] (
			[AttributeId]
			,[CharacterSetId]
			)
		SELECT @scopeIdentity
			,[CharacterSetId]
		FROM [dbo].[CharacterSets]
	END

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Attributes
			WHERE AttributeName = 'PriceLineSize'
				AND AttributeGroupId = @PriceLineAttributeGroupId
			)
	BEGIN
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
			,[IsSpecialTransform]
			,[IsActive]
			,[LastModifiedBy]
			)
		SELECT 'Price Line Size'
			,'PriceLineSize'
			,@PriceLineAttributeGroupId
			,[HasUniqueValues]
			,'Price Line Size'
			,[DefaultValue]
			,1
			,[SpecialCharactersAllowed]
			,'PLS'
			,[DataTypeId]
			,@MaxDisplayOrder + 3
			,[InitialValue]
			,[IncrementBy]
			,[InitialMax]
			,[DisplayType]
			,[MaxLengthAllowed]
			,[MinimumNumber]
			,[MaximumNumber]
			,[NumberOfDecimals]
			,[IsPickList]
			,'Price Line Size'
			,Null
			,0
			,[IsActive]
			,'Script'
		FROM Attributes
		WHERE AttributeName = 'RetailSize'

		SET @scopeIdentity = SCOPE_IDENTITY()

		INSERT INTO dbo.AttributesWebConfiguration (
			AttributeId
			,GridColumnWidth
			,IsReadOnly
			,CharacterSetRegexPattern
			)
		VALUES (
			@scopeIdentity
			,200
			,0
			,'^[a-zA-Z0-9\s]*$'
			)
	END

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Attributes
			WHERE AttributeName = 'PriceLineUOM'
				AND AttributeGroupId = @PriceLineAttributeGroupId
			)
	BEGIN
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
			,[IsSpecialTransform]
			,[IsActive]
			,[LastModifiedBy]
			)
		SELECT 'Price Line UOM'
			,'PriceLineUOM'
			,@PriceLineAttributeGroupId
			,[HasUniqueValues]
			,'Price Line UOM'
			,[DefaultValue]
			,1
			,[SpecialCharactersAllowed]
			,'PLU'
			,[DataTypeId]
			,@MaxDisplayOrder + 4
			,[InitialValue]
			,[IncrementBy]
			,[InitialMax]
			,[DisplayType]
			,[MaxLengthAllowed]
			,[MinimumNumber]
			,[MaximumNumber]
			,[NumberOfDecimals]
			,[IsPickList]
			,'Price Line UOM'
			,Null
			,0
			,[IsActive]
			,'Script'
		FROM Attributes
		WHERE AttributeName = 'UOM'

		SET @scopeIdentity = SCOPE_IDENTITY()

		INSERT INTO dbo.AttributesWebConfiguration (
			AttributeId
			,GridColumnWidth
			,IsReadOnly
			,CharacterSetRegexPattern
			)
		VALUES (
			@scopeIdentity
			,200
			,0
			,'^[a-zA-Z0-9\s]*$'
			)

		INSERT INTO [dbo].[PickListData] (
			[AttributeId]
			,[PickListValue]
			)
		SELECT @scopeIdentity
			,[PickListValue]
		FROM [dbo].[PickListData]
		WHERE AttributeId IN (
				SELECT AttributeId
				FROM Attributes
				WHERE AttributeName = 'UOM'
				)

		INSERT INTO [dbo].[AttributeCharacterSets] (
			[AttributeId]
			,[CharacterSetId]
			)
		SELECT @scopeIdentity
			,[CharacterSetId]
		FROM [dbo].[CharacterSets]
	END

	INSERT INTO app.PostDeploymentScriptHistory (
		ScriptKey
		,RunTime
		)
	VALUES (
		@key
		,GetDate()
		);
END
ELSE
BEGIN
	PRINT '[' + Convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @key;
END
GO