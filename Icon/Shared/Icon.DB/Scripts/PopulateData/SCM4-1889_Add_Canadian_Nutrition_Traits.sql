DECLARE @key VARCHAR(128) = 'AddCanadianNutritionTraits';

IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = @key
			)
		)
BEGIN
	DECLARE @ProfitCenter nvarchar(3) = 'PFC',
	        @CanadaAllergen nvarchar(3) = 'CDA',
			@CanadaIngredient nvarchar(3) = 'CDI',
			@CanadaSugarPercentage nvarchar(3) = 'CDS',
			@CanadaCustomerFriendlyDescription nvarchar(3) = 'CDC' 

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Trait
			WHERE traitCode = @ProfitCenter
			)
	BEGIN
			INSERT INTO [dbo].[Trait]
			   ([traitCode]
			   ,[traitPattern]
			   ,[traitDesc]
			   ,[traitGroupID])
		 VALUES
			   (@ProfitCenter
			   ,''
			   ,'Profit Center'
			   ,1)
	END

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Trait
			WHERE traitCode = @CanadaAllergen
			)
	BEGIN
			INSERT INTO [dbo].[Trait]
			   ([traitCode]
			   ,[traitPattern]
			   ,[traitDesc]
			   ,[traitGroupID])
		 VALUES
			   (@CanadaAllergen
			   ,''
			   ,'Canada Allergens'
			   ,1)
	END

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Trait
			WHERE traitCode = @CanadaIngredient
			)
	BEGIN
			INSERT INTO [dbo].[Trait]
			   ([traitCode]
			   ,[traitPattern]
			   ,[traitDesc]
			   ,[traitGroupID])
		 VALUES
			   (@CanadaIngredient
			   ,''
			   ,'Canada Ingredients'
			   ,1)
	END

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Trait
			WHERE traitCode = @CanadaSugarPercentage
			)
	BEGIN
			INSERT INTO [dbo].[Trait]
			   ([traitCode]
			   ,[traitPattern]
			   ,[traitDesc]
			   ,[traitGroupID])
		 VALUES
			   (@CanadaSugarPercentage
			   ,''
			   ,'Canada Sugar Percentage'
			   ,1)
	END

	IF NOT EXISTS (
			SELECT 1
			FROM dbo.Trait
			WHERE traitCode = @CanadaCustomerFriendlyDescription
			)
	BEGIN
			INSERT INTO [dbo].[Trait]
			   ([traitCode]
			   ,[traitPattern]
			   ,[traitDesc]
			   ,[traitGroupID])
		 VALUES
			   (@CanadaCustomerFriendlyDescription
			   ,''
			   ,'Canada Customer Friendly Description'
			   ,1)
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