DECLARE @scriptKey varchar(128) = 'SCM4-1906_AddCanadaNutritionTraitCodes';

IF(NOT exists(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	DECLARE @ProfitCenter nvarchar(3) = 'PFC',
	        @CanadaAllergens nvarchar(3) = 'CDA',
			@CanadaIngredients nvarchar(3) = 'CDI',
			@CanadaSugarPercentage nvarchar(3) = 'CDS',
			@CanadaCustomerFriendlyDescription nvarchar(3) = 'CDC'

	IF NOT EXISTS (SELECT 1 FROM dbo.Attributes WHERE AttributeCode = @ProfitCenter)
	BEGIN
        INSERT INTO [dbo].[Attributes]
           ([AttributeGroupID]
           ,[AttributeCode]
           ,[AttributeDesc]
           ,[AddedDate]
           ,[ModifiedDate])
        VALUES
           (3
           ,@ProfitCenter
           ,'Profit Center'
           ,GETDATE()
           ,GETDATE())
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Attributes WHERE AttributeCode = @CanadaAllergens)
	BEGIN
        INSERT INTO [dbo].[Attributes]
           ([AttributeGroupID]
           ,[AttributeCode]
           ,[AttributeDesc]
           ,[AddedDate]
           ,[ModifiedDate])
        VALUES
           (3
           ,@CanadaAllergens
           ,'Canada Allergens'
           ,GETDATE()
           ,GETDATE())
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Attributes WHERE AttributeCode = @CanadaIngredients)
	BEGIN
        INSERT INTO [dbo].[Attributes]
           ([AttributeGroupID]
           ,[AttributeCode]
           ,[AttributeDesc]
           ,[AddedDate]
           ,[ModifiedDate])
        VALUES
           (3
           ,@CanadaIngredients
           ,'Canada Ingredients'
           ,GETDATE()
           ,GETDATE())
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Attributes WHERE AttributeCode = @CanadaSugarPercentage)
	BEGIN
        INSERT INTO [dbo].[Attributes]
           ([AttributeGroupID]
           ,[AttributeCode]
           ,[AttributeDesc]
           ,[AddedDate]
           ,[ModifiedDate])
        VALUES
           (3
           ,@CanadaSugarPercentage
           ,'Canada Sugar Percentage'
           ,GETDATE()
           ,GETDATE())
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.Attributes WHERE AttributeCode = @CanadaCustomerFriendlyDescription)
	BEGIN
        INSERT INTO [dbo].[Attributes]
           ([AttributeGroupID]
           ,[AttributeCode]
           ,[AttributeDesc]
           ,[AddedDate]
           ,[ModifiedDate])
        VALUES
           (3
           ,@CanadaCustomerFriendlyDescription
           ,'Canada Customer Friendly Description'
           ,GETDATE()
           ,GETDATE())
    END

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO