
declare @scriptKey varchar(128)

set @scriptKey = 'PopulateUom'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	SET IDENTITY_INSERT dbo.Uom ON

	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'EA')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (1, N'EA', N'EACH')

	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'LB')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (2, N'LB', N'POUND')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'CT')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (3, N'CT', N'COUNT')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'OZ')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (4, N'OZ', N'OUNCE')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'CS')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (5, N'CS', N'CASE')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'PK')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (6, N'PK', N'PACK')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'LT')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (7, N'LT', N'LITER')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'PT')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (8, N'PT', N'PINT')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'KG')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (9, N'KG', N'KILOGRAM')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'ML')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (10, N'ML', N'MILLILITER')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'GL')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (11, N'GL', N'GALLON')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'GR')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (12, N'GR', N'GRAM')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'CG')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (13, N'CG', N'CENTIGRAM')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'FT')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (14, N'FT', N'FEET')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'YD')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (15, N'YD', N'YARD')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'QT')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (16, N'QT', N'QUART')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'SQFT')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (17, N'SQFT', N'SQUARE FOOT')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'MT')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (18, N'MT', N'METER')
	
	IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'FZ')
		INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (19, N'FZ', N'FLUID OUNCES')
	

	SET IDENTITY_INSERT dbo.Uom OFF
	
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO