IF (
		NOT EXISTS (
			SELECT 1
			FROM app.PostDeploymentScriptHistory
			WHERE ScriptKey = 'PopulateFeatureFlags_Sku_andPriceline'
			)
		)
BEGIN

  IF NOT EXISTS( SELECT 1 FROM [dbo].[FeatureFlag]  WHERE [FlagName] ='SkuManagement')
  BEGIN
	INSERT INTO [dbo].[FeatureFlag] 
		([FlagName],[Enabled],[Description],[CreatedDateUtc],[LastModifiedDateUtc],[LastModifiedBy])
     VALUES
        ('SkuManagement', 0, 'Sku Management', GETUTCDATE(), GETUTCDATE(), 'Script')
  END

  IF NOT EXISTS( SELECT 1 FROM [dbo].[FeatureFlag]  WHERE [FlagName] ='SkuManagement')
  BEGIN
	INSERT INTO [dbo].[FeatureFlag] 
		([FlagName],[Enabled],[Description],[CreatedDateUtc],[LastModifiedDateUtc],[LastModifiedBy])
     VALUES
        ('PriceLineManagement', 0,'Price Line Management', GETUTCDATE(), GETUTCDATE(), 'Script')
  END

  INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
  VALUES ('PopulateFeatureFlags_Sku_andPriceline', GetDate());

END
ELSE
BEGIN
	PRINT '[' + Convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + 'PopulateFeatureFlags_Sku_andPriceline';
END
GO


