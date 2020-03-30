CREATE PROCEDURE [dbo].[AddAttributes] @DisplayName NVARCHAR(255),
    @AttributeName NVARCHAR(255),
	@Description NVARCHAR(MAX),
	@TraitCode NVARCHAR(10),
	@DataTypeId INT,
	@MaxLengthAllowed INT = NULL,
	@MinimumNumber NVARCHAR(14) = NULL,
	@MaximumNumber NVARCHAR(14) = NULL,
	@NumberOfDecimals NVARCHAR(1) = NULL,
	@IsPickList BIT,
	@SpecialCharactersAllowed NVARCHAR(255) = NULL,
	@IsRequired bit ,
	@CharacterSetRegexPattern NVARCHAR(255) = NULL,
	@DefaultValue NVARCHAR(255) = NULL,
	@AttributeId INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @AttributeGroupId INT = (
			SELECT attributeGroupId
			FROM dbo.attributeGroup
			WHERE attributeGroupName = 'Global Item'
			)

	IF EXISTS (
			SELECT 1
			FROM [dbo].[Attributes]
			WHERE DisplayName = @DisplayName
				OR TraitCode = @TraitCode
				OR AttributeName = @AttributeName
			)
	BEGIN
		RAISERROR (
				'Attribute with this trait code or name already exists.',
				16,
				1
				)
	END
	ELSE
	BEGIN
		DECLARE @scopeIdentity INT = NULL;
		DECLARE @displayOrder INT = (
				SELECT MAX(DisplayOrder) +1
				FROM Attributes
				)

		INSERT INTO [dbo].[Attributes] (
			[DisplayName],
			[AttributeName],
			[AttributeGroupId],
			[Description],
			[TraitCode],
			[DataTypeId],
			[MaxLengthAllowed],
			[MinimumNumber],
			[MaximumNumber],
			[NumberOfDecimals],
			[IsPickList],
			[SpecialCharactersAllowed],
			[DisplayOrder],
			[IsRequired],
			[XmlTraitDescription],
			[DefaultValue]
			)
		VALUES (
			@DisplayName,
			@AttributeName,
			@AttributeGroupId,
			@Description,
			@TraitCode,
			@DataTypeId,
			@MaxLengthAllowed,
			@MinimumNumber,
			@MaximumNumber,
			@NumberOfDecimals,
			@IsPickList,
			@SpecialCharactersAllowed,
			@displayOrder,
			@IsRequired,
			@DisplayName,
			@DefaultValue
			)

		SET @scopeIdentity = SCOPE_IDENTITY()

		INSERT INTO dbo.AttributesWebConfiguration (
			AttributeId,
			GridColumnWidth,
			IsReadOnly,
			CharacterSetRegexPattern
			)
		VALUES (
			@scopeIdentity,
			200,
			0,
			@CharacterSetRegexPattern
			)

		SELECT @scopeIdentity
	END
END
