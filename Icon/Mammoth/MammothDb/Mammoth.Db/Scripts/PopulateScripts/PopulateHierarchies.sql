declare @scriptKey varchar(128)

set @scriptKey = 'PopulateHierarchies'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Merchandise')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (1, N'Merchandise', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
	
	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Brands')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (2, N'Brands', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
	
	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Tax')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (3, N'Tax', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
	
	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Browsing')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (4, N'Browsing', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
	
	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Financial')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (5, N'Financial', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
	
	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'National')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (6, N'National', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
	
	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Certification Agency Management')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (7, N'Certification Agency Management', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
	
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO