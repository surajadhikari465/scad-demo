declare @scriptKey varchar(128)

set @scriptKey = 'PopulateItemType'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	SET IDENTITY_INSERT [dbo].[ItemTypes] ON 

	IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'RTL')
		INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (1, N'RTL', N'Retail Sale')

	IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'DEP')
		INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (2, N'DEP', N'Deposit')

	IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'TAR')
		INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (3, N'TAR', N'Tare')

	IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'RTN')
		INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (4, N'RTN', N'Return')

	IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'CPN')
		INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (5, N'CPN', N'Coupon')

	IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'NRT')
		INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (6, N'NRT', N'Non-Retail')

	IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'FEE')
		INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (7, N'FEE', N'Fee')

	SET IDENTITY_INSERT [dbo].[ItemTypes] OFF


	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO