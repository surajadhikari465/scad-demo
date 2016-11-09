declare @scriptKey varchar(128)

set @scriptKey = 'PopulateItemPriceType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT [dbo].[ItemPriceType] ON 
	
	IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'REG')
		INSERT [dbo].[ItemPriceType] ([ItemPriceTypeId], [ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (1, N'REG', N'Regular Price')
	
	IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'TPR')
		INSERT [dbo].[ItemPriceType] ([ItemPriceTypeId], [ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (2, N'TPR', N'Temporary Price Reduction')
	
	SET IDENTITY_INSERT [dbo].[ItemPriceType] OFF
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
