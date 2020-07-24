DECLARE @key VARCHAR(128) = 'UpdateItemGroupKeywords';

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key ))
BEGIN

	Update [dbo].[ItemGroup]
	SET KeyWords = concat(
				ig.[ItemGroupId]
				,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.SkuDescription')
				,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineDescription')
				,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineSize')
				,' ', JSON_VALUE(ig.[ItemGroupAttributesJson],'$.PriceLineUOM')
				,' ', sc.[ScanCode]	)
		FROM [dbo].[ItemGroup] ig 
			INNER JOIN [dbo].[ItemGroupMember] img ON (img.[ItemGroupId] = ig.ItemGroupId AND img.[IsPrimary] =1)
			INNER JOIN [dbo].[ScanCode] sc  ON (sc.[ItemId] = img.[ItemId])

		
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