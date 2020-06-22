DECLARE @key VARCHAR(128) = 'PopulateSkuPriceLineAttributeGroups';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN
	IF NOT EXISTS (
			SELECT 1
			FROM [dbo].[AttributeGroup]
			WHERE [AttributeGroupName] = 'SKU'
			)
	BEGIN
		INSERT INTO [dbo].[AttributeGroup] ([AttributeGroupName])
		VALUES ('SKU')
	END

	IF NOT EXISTS (
			SELECT 1
			FROM [dbo].[AttributeGroup]
			WHERE [AttributeGroupName] = 'PriceLine'
			)
	BEGIN
		INSERT INTO [dbo].[AttributeGroup] ([AttributeGroupName])
		VALUES ('PriceLine')
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