declare @scriptKey varchar(128)

set @scriptKey = 'PopulatePromoRewardItemPriceType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT [dbo].[ItemPriceType] ON 
	
	IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'PMI')
		INSERT [dbo].[ItemPriceType] ([ItemPriceTypeId], [ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (12, N'PMI', N'Prime Incremental')

	IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'PMD')
		INSERT [dbo].[ItemPriceType] ([ItemPriceTypeId], [ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (13, N'PMD', N'Prime Member Deal')

	SET IDENTITY_INSERT [dbo].[ItemPriceType] OFF

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
