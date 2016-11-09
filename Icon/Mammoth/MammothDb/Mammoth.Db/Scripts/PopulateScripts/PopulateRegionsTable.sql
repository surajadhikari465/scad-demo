declare @scriptKey varchar(128)

set @scriptKey = 'PopulateRegionsTable'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey

	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'FL')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Florida','FL');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'MA')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Mid Atlantic','MA');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'MW')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Mid West','MW');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'NA')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('North Atlantic','NA');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'NC')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Northern California','NC');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'NE')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('North East','NE');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'PN')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Pacific Northwest','PN');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'RM')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Rocky Mountain','RM');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'SO')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('South','SO');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'SP')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Southern Pacific','SP');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'SW')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Southwest','SW');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'TS')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('365','TS');
	IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'UK')
		INSERT INTO dbo.Regions (RegionName, Region) VALUES ('United Kingdom','UK');

	insert into app.PostDeploymentScriptHistory values(@scriptKey, getdate())
END
ELSE
BEGIN
	print 'Skipping script ' + @scriptKey
END
GO
