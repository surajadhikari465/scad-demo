CREATE PROCEDURE [dbo].[UpdateAttributes] @AttributeId INT
	,@DisplayName NVARCHAR(255)
	,@Description NVARCHAR(MAX)
	,@MaxLengthAllowed INT = NULL
	,@MinimumNumber NVARCHAR(14) = NULL
	,@MaximumNumber NVARCHAR(14) = NULL
	,@NumberOfDecimals NVARCHAR(1) = NULL
	,@IsPickList BIT
	,@SpecialCharactersAllowed NVARCHAR(255) = NULL
	,@IsRequired BIT
	,@CharacterSetRegexPattern NVARCHAR(255) = NULL
	,@DefaultValue NVARCHAR(255) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (
			SELECT 1
			FROM [dbo].[Attributes]
			WHERE DisplayName = @DisplayName
				AND AttributeId != @AttributeId
			)
	BEGIN
		RAISERROR (
				'Attribute with this Display Name already exists.'
				,16
				,1
				)
	END
	ELSE
	BEGIN
		UPDATE [dbo].[Attributes]
		SET [DisplayName] = @DisplayName
			,[Description] = @Description
			,[MaxLengthAllowed] = @MaxLengthAllowed
			,[MinimumNumber] = @MinimumNumber
			,[MaximumNumber] = @MaximumNumber
			,[NumberOfDecimals] = @NumberOfDecimals
			,[IsPickList] = @IsPickList
			,[SpecialCharactersAllowed] = @SpecialCharactersAllowed
			,[IsRequired] = @IsRequired
			,[DefaultValue] = @DefaultValue
		WHERE AttributeId = @AttributeId

		UPDATE [dbo].[AttributesWebConfiguration]
		SET [CharacterSetRegexPattern] = @CharacterSetRegexPattern
		WHERE AttributeId = @AttributeId

		IF @IsPickList = 0
		BEGIN
			DELETE dbo.PickListData
			WHERE AttributeId = @AttributeId
		END
	END
END