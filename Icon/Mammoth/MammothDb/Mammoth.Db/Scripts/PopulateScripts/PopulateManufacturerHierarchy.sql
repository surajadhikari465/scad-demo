declare @scriptKey varchar(128)

set @scriptKey = 'PopulateManufacturerHierarchy'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Manufacturer')
		INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (8, N'Manufacturer', CAST(N'2020-02-24 23:51:27.587' AS DateTime), NULL)	
	
	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO