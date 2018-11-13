declare @scriptKey varchar(128)

set @scriptKey = 'PopulateRewardItemPriceType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT [dbo].[ItemPriceType] ON 
	
	IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'RWD')
		INSERT [dbo].[ItemPriceType] ([ItemPriceTypeId], [ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (11, N'RWD', N'Rewards')
	
	SET IDENTITY_INSERT [dbo].[ItemPriceType] OFF

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
