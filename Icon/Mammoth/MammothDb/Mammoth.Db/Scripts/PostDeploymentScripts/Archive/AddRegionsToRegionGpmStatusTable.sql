DECLARE @scriptKey varchar(128)

SET @scriptKey = 'AddRegionsToRegionGpmStatusTable';

IF(NOT exists(SELECT * FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
	Print 'running script ' + @scriptKey 

	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'FL')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('FL', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'MA')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('MA', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'MW')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('MW', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'NA')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('NA', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'NC')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('NC', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'NE')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('NE', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'PN')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('PN', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'RM')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('RM', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'SO')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('SO', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'SP')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('SP', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'SW')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('SW', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'TS')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('TS', 0)
	IF NOT EXISTS (SELECT * FROM dbo.RegionGpmStatus WHERE Region = 'UK')
		INSERT INTO dbo.RegionGpmStatus (Region, IsGpmEnabled) VALUES ('UK', 0)

	INSERT INTO app.PostDeploymentScriptHistory VALUES(@scriptKey, getdate())

END
ELSE
BEGIN
	PRINT 'Skipping script ' + @scriptKey
END
GO