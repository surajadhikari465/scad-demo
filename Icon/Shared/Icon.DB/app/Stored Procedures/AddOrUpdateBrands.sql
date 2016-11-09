CREATE PROCEDURE [app].[AddOrUpdateBrands]
	@brands app.BrandImportType READONLY
AS
	DECLARE @taskName nvarchar(32) = 'app.AddOrUpdateBrands',
			@brandAbbreviationTraitId int = (select traitID from Trait where traitCode = 'BA'),
			@brandId int = (select hierarchyID from Hierarchy where hierarchyName = 'Brands')
	DECLARE @newBrands table (
				HierarchyClassId int,
				HierarchyClassName nvarchar(255)
			);

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Inserting new brands...';
			
	--Insert new Brands
	INSERT INTO HierarchyClass(hierarchyLevel, hierarchyID, hierarchyParentClassID, hierarchyClassName)
		OUTPUT	inserted.hierarchyClassID,
				inserted.hierarchyClassName
			INTO @newBrands
	SELECT	1,
			@brandId,
			NULL,
			b.BrandName
	FROM @brands b
	WHERE b.BrandId = '0'
		AND NOT EXISTS (SELECT 1 FROM HierarchyClass hc WHERE hc.hierarchyID = @brandId AND hc.hierarchyClassName = b.BrandName)
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Inserting Brand Abbreviations for new Brands...';
	
	--Insert Brand Abbreviations for new Brands
	INSERT HierarchyClassTrait(hierarchyClassID, traitID, traitValue, uomID)
	SELECT	nb.HierarchyClassId,
			@brandAbbreviationTraitId,
			b.BrandAbbreviation,
			null
	FROM @brands b
	JOIN @newBrands nb on nb.HierarchyClassName = b.BrandName
	WHERE b.BrandId = '0'
		AND b.BrandAbbreviation <> ''

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Inserting new Brand Abbreviations for existing Brands...';
	
	--Insert new Brand Abbreviations for existing Brands
	INSERT HierarchyClassTrait(hierarchyClassID, traitID, traitValue, uomID)
	SELECT	b.BrandId,
			@brandAbbreviationTraitId,
			b.BrandAbbreviation,
			null
	FROM @brands b
	LEFT JOIN HierarchyClassTrait hct on hct.hierarchyClassId = b.BrandId
		AND hct.traitID = @brandAbbreviationTraitId
	WHERE b.BrandAbbreviation <> ''	
		AND b.BrandId <> '0'
		AND hct.traitID IS NULL
	
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Updating Existing Brand Abbreviations...';

	--Update Existing Brand Abbreviations
	UPDATE HierarchyClassTrait 
	SET traitValue = b.BrandAbbreviation
	FROM HierarchyClassTrait hct
	JOIN @brands b on hct.hierarchyClassID = b.BrandId
		AND b.BrandId <> '0'
		AND b.BrandAbbreviation <> ''
		AND hct.traitID = @brandAbbreviationTraitId

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Inserting sent to ESB Trait for new Brands...';
	
	DECLARE @sentToEsbTraitId int = (SELECT traitID FROM Trait WHERE traitCode = 'ESB')
	INSERT INTO
		HierarchyClassTrait(hierarchyClassID, traitID, traitValue, uomID)
	SELECT
		nb.HierarchyClassId,
		@SentToEsbTraitId,
		null,
		null
	FROM
		@newBrands nb

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + '[' + @taskName + '] ' + 'Adding ESB messages for new Brands...';

	DECLARE @hierarchyMessageTypeId int = (select MessageTypeId from app.MessageType where MessageTypeName = 'Hierarchy'), 
			@readyStatusId int = (select MessageStatusId from app.MessageStatus where MessageStatusName = 'Ready'), 
			@messageActionId int = (select MessageActionId from app.MessageAction where MessageActionName = 'AddOrUpdate'),
			@brandsHierarchyId int = (select hierarchyID from Hierarchy where hierarchyName = 'Brands'),
			@brandsHierarchyLevelName nvarchar(16) = (select hierarchyLevelName from HierarchyPrototype where hierarchyID = @brandId),
			@brandsItemsAttached bit = (select itemsAttached from HierarchyPrototype where hierarchyID = @brandId),
			@brandsHierarchyLevel int = (select hierarchyLevel from HierarchyPrototype where hierarchyID = @brandId),
			@brandsParentClassId int = null
			
	INSERT INTO
		app.MessageQueueHierarchy
	SELECT
		MessageTypeId			= @hierarchyMessageTypeId,
		MessageStatusId			= @readyStatusId,
		MessageHistoryId		= null,
		MessageActionId			= @messageActionId,
		InsertDate				= sysdatetime(),
		HierarchyId				= @brandsHierarchyId,
		HierarchyName			= 'Brands',
		HierarchyLevelName		= @brandsHierarchyLevelName,
		ItemsAttached			= @brandsItemsAttached,
		HierarchyClassId		= nb.HierarchyClassId,
		HierarchyClassName		= nb.HierarchyClassName,
		HierarchyLevel			= @brandsHierarchyLevel,
		HierarchyParentClassId	= @brandsParentClassId,
		null,
		null
	FROM
		@newBrands nb