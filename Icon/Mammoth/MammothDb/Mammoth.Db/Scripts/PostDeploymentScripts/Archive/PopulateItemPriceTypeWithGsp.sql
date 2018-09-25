DECLARE @scriptKey varchar(128);
SET @scriptKey = 'PopulateItemPriceTypeWithGsp'

IF(NOT EXISTS(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey
	
	IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'GSP')
		INSERT [dbo].[ItemPriceType] ([ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (N'GSP', N'Globally Set Price')

	INSERT INTO app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
