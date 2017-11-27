DECLARE @scriptKey VARCHAR(128)

SET @scriptKey = 'AddGpmPriceTypes'

IF NOT EXISTS (SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey

	SET IDENTITY_INSERT dbo.ItemPriceType ON

	IF NOT EXISTS (SELECT 1 FROM dbo.ItemPriceType WHERE ItemPriceTypeId = 3 AND ItemPriceTypeCode = 'CMP')
		INSERT INTO dbo.ItemPriceType (ItemPriceTypeId, ItemPriceTypeCode, ItemPriceTypeDesc) VALUES (3, 'CMP', 'Competitive')
	
	IF NOT EXISTS (SELECT 1 FROM dbo.ItemPriceType WHERE ItemPriceTypeId = 4 AND ItemPriceTypeCode = 'EDV')
		INSERT INTO dbo.ItemPriceType (ItemPriceTypeId, ItemPriceTypeCode, ItemPriceTypeDesc) VALUES (4, 'EDV', 'Everyday Value')
		
	IF NOT EXISTS (SELECT 1 FROM dbo.ItemPriceType WHERE ItemPriceTypeId = 5 AND ItemPriceTypeCode = 'CLR')
		INSERT INTO dbo.ItemPriceType (ItemPriceTypeId, ItemPriceTypeCode, ItemPriceTypeDesc) VALUES (5, 'CLR', 'Clearance')

	IF NOT EXISTS (SELECT 1 FROM dbo.ItemPriceType WHERE ItemPriceTypeId = 6 AND ItemPriceTypeCode = 'DIS')
		INSERT INTO dbo.ItemPriceType (ItemPriceTypeId, ItemPriceTypeCode, ItemPriceTypeDesc) VALUES (6, 'DIS', 'Discontinued')

	IF NOT EXISTS (SELECT 1 FROM dbo.ItemPriceType WHERE ItemPriceTypeId = 7 AND ItemPriceTypeCode = 'MSAL')
		INSERT INTO dbo.ItemPriceType (ItemPriceTypeId, ItemPriceTypeCode, ItemPriceTypeDesc) VALUES (7, 'MSAL', 'Market Sale')

	IF NOT EXISTS (SELECT 1 FROM dbo.ItemPriceType WHERE ItemPriceTypeId = 8 AND ItemPriceTypeCode = 'SSAL')
		INSERT INTO dbo.ItemPriceType (ItemPriceTypeId, ItemPriceTypeCode, ItemPriceTypeDesc) VALUES (8, 'SSAL', 'Special Sale')

	IF NOT EXISTS (SELECT 1 FROM dbo.ItemPriceType WHERE ItemPriceTypeId = 9 AND ItemPriceTypeCode = 'ISS')
		INSERT INTO dbo.ItemPriceType (ItemPriceTypeId, ItemPriceTypeCode, ItemPriceTypeDesc) VALUES (9, 'ISS', 'In Store Special')

	SET IDENTITY_INSERT dbo.ItemPriceType OFF

	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO

