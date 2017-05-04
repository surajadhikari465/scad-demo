-- DEV
DECLARE @devAppId UNIQUEIDENTIFIER
SELECT @devAppId = ApplicationID FROM AppConfigApp WHERE Name ='USER AUDIT' AND EnvironmentID = '20C5DDAC-659C-4B81-84F6-5F79CC390D10'

SELECT @devAppId

IF (@devAppId IS NULL)
BEGIN

SET @devAppId = '687D6E35-E575-4CC4-84F7-712BA3BB29B4'
	
INSERT INTO [AppConfigApp]
           ([ApplicationID]
           ,[EnvironmentID]
           ,[TypeID]
           ,[Name]
           ,[Configuration]
           ,[Deleted]
           ,[LastUpdate]
           ,[LastUpdateUserID])
     VALUES
           (@devAppId
           ,'20C5DDAC-659C-4B81-84F6-5F79CC390D10'
           ,1
           ,'USER AUDIT'
           ,'<configuration>
		  <appSettings>
			<add key="BasePath" value="E:\ScheduledJobs\IRMAUserAudit\BasePath" />
			<add key="ExportBy" value="store" />
			<add key="NextRunAction" value="export" />
			<add key="NextRunDate" value="1/1/2012" />
			<add key="RestorePath" value="E:\ScheduledJobs\IRMAUserAudit\restore" />
		  </appSettings>
		</configuration>'
           ,0
           ,GETDATE()
           ,0)
 END          
--QA
DECLARE @qaAppId UNIQUEIDENTIFIER
SELECT @qaAppId = ApplicationID FROM AppConfigApp WHERE Name ='USER AUDIT' AND EnvironmentID = '6A62316D-B6D2-4607-848A-047588CDA1A8'

select @qaAppId

IF (@qaAppId IS NULL)
BEGIN

SET @qaAppId = 'C203CD23-86C6-44EB-BB32-C00D5C2D9AD7'
	
INSERT INTO [AppConfigApp]
           ([ApplicationID]
           ,[EnvironmentID]
           ,[TypeID]
           ,[Name]
           ,[Configuration]
           ,[Deleted]
           ,[LastUpdate]
           ,[LastUpdateUserID])
     VALUES
           (@qaAppId
           ,'6A62316D-B6D2-4607-848A-047588CDA1A8'
           ,1
           ,'USER AUDIT'
           ,'<configuration>
		  <appSettings>
			<add key="BasePath" value="\\sites\global\IRMA\IRMA User Audit\Results_QA" />
			<add key="ExportBy" value="store" />
			<add key="NextRunAction" value="export" />
			<add key="NextRunDate" value="1/1/2012" />
			<add key="RestorePath" value="E:\ScheduledJobs\IRMAUserAudit\restore" />
		  </appSettings>
		</configuration>'
           ,0
           ,GETDATE()
           ,0)
END

--PROD
DECLARE @prodAppId UNIQUEIDENTIFIER
SELECT @prodAppId = ApplicationID FROM AppConfigApp WHERE Name ='USER AUDIT' AND EnvironmentID = '8A5FF21E-9ACE-403E-8BFB-7338C71151F5'

SELECT @prodAppId

IF (@prodAppId IS NULL)
BEGIN
	
SET @prodAppId = '8C1EB424-C9B7-4ABB-AC8B-02D3CF7D2560'
	
INSERT INTO [AppConfigApp]
           ([ApplicationID]
           ,[EnvironmentID]
           ,[TypeID]
           ,[Name]
           ,[Configuration]
           ,[Deleted]
           ,[LastUpdate]
           ,[LastUpdateUserID])
     VALUES
           (@prodAppId
           ,'8A5FF21E-9ACE-403E-8BFB-7338C71151F5'
           ,1
           ,'USER AUDIT'
           ,'<configuration>
		  <appSettings>
			<add key="BasePath" value="\\sites\global\IRMA\IRMA User Audit" />
			<add key="ExportBy" value="store" />
			<add key="NextRunAction" value="export" />
			<add key="NextRunDate" value="1/1/2012" />
			<add key="RestorePath" value="E:\ScheduledJobs\IRMAUserAudit\restore" />
		  </appSettings>
		</configuration>'
           ,0
           ,GETDATE()
           ,0)
 END          

-- build keys for AppConfigKey
DECLARE @keyId INT

-- 1st key:  BasePath
IF NOT EXISTS(SELECT KeyID FROM AppConfigKey WHERE Name = 'BasePath')
	INSERT INTO AppConfigKey (Name, LastUpdate, LastUpdateUserID) VALUES 	('BasePath', GETDATE(), 0)
	
SELECT @keyId = KeyID FROM AppConfigKey WHERE Name = 'BasePath'

SELECT @keyId
-- test
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '20C5DDAC-659C-4B81-84F6-5F79CC390D10')
	INSERT INTO [AppConfigValue] VALUES ('20C5DDAC-659C-4B81-84F6-5F79CC390D10', @devAppId, @keyId, 'E:\ScheduledJobs\IRMAUserAudit\BasePath', 0, GETDATE(), 0)
-- QA
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '6A62316D-B6D2-4607-848A-047588CDA1A8')
	INSERT INTO [AppConfigValue] VALUES ('6A62316D-B6D2-4607-848A-047588CDA1A8', @qaAppId, @keyId, '\\sites\global\IRMA\IRMA User Audit\Results_QA', 0, GETDATE(), 0)
-- Prod.  Cattle Prod.  
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '8A5FF21E-9ACE-403E-8BFB-7338C71151F5')
	INSERT INTO [AppConfigValue] VALUES ('8A5FF21E-9ACE-403E-8BFB-7338C71151F5', @prodAppId, @keyId, '\\sites\global\IRMA\IRMA User Audit', 0, GETDATE(), 0)


	
-- 2nd key:  ExportBy (valid values here are 'region' or 'store'.  Defaults to 'store'
IF NOT EXISTS(SELECT KeyID FROM AppConfigKey WHERE Name = 'ExportBy')
	INSERT INTO AppConfigKey (Name, LastUpdate, LastUpdateUserID) VALUES 	('ExportBy', GETDATE(), 0)
	
SELECT @keyId = KeyID FROM AppConfigKey WHERE Name = 'ExportBy'

-- test
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '20C5DDAC-659C-4B81-84F6-5F79CC390D10')
	INSERT INTO [AppConfigValue] VALUES ('20C5DDAC-659C-4B81-84F6-5F79CC390D10', @devAppId, @keyId, 'store', 0, GETDATE(), 0)
-- QA
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '6A62316D-B6D2-4607-848A-047588CDA1A8')
	INSERT INTO [AppConfigValue] VALUES ('6A62316D-B6D2-4607-848A-047588CDA1A8', @qaAppId, @keyId, 'store', 0, GETDATE(), 0)
-- Prod.  Cattle Prod.  
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '8A5FF21E-9ACE-403E-8BFB-7338C71151F5')
	INSERT INTO [AppConfigValue] VALUES ('8A5FF21E-9ACE-403E-8BFB-7338C71151F5', @prodAppId, @keyId, 'store', 0, GETDATE(), 0)
	



-- 3rd key:  Next Run Action (acceptable values are:  export, import, restore)  Defaults to export.
IF NOT EXISTS(SELECT KeyID FROM AppConfigKey WHERE Name = 'NextRunAction')
	INSERT INTO AppConfigKey (Name, LastUpdate, LastUpdateUserID) VALUES 	('NextRunAction', GETDATE(), 0)
	
SELECT @keyId = KeyID FROM AppConfigKey WHERE Name = 'NextRunAction'

-- test
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '20C5DDAC-659C-4B81-84F6-5F79CC390D10')
	INSERT INTO [AppConfigValue] VALUES ('20C5DDAC-659C-4B81-84F6-5F79CC390D10', @devAppId, @keyId, 'export', 0, GETDATE(), 0)
-- QA
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '6A62316D-B6D2-4607-848A-047588CDA1A8')
	INSERT INTO [AppConfigValue] VALUES ('6A62316D-B6D2-4607-848A-047588CDA1A8', @qaAppId, @keyId, 'export', 0, GETDATE(), 0)
-- Prod.  Cattle Prod.  
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '8A5FF21E-9ACE-403E-8BFB-7338C71151F5')
	INSERT INTO [AppConfigValue] VALUES ('8A5FF21E-9ACE-403E-8BFB-7338C71151F5', @prodAppId, @keyId, 'export', 0, GETDATE(), 0)
	


-- 4th key:  Next Run Date - this is defaulting to 2012/1/1 which means until this value is altered, this app will effectively be disabled 
IF NOT EXISTS(SELECT KeyID FROM AppConfigKey WHERE Name = 'NextRunDate')
	INSERT INTO AppConfigKey (Name, LastUpdate, LastUpdateUserID) VALUES 	('NextRunDate', GETDATE(), 0)
	
SELECT @keyId = KeyID FROM AppConfigKey WHERE Name = 'NextRunDate'

-- test
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '20C5DDAC-659C-4B81-84F6-5F79CC390D10')
	INSERT INTO [AppConfigValue] VALUES ('20C5DDAC-659C-4B81-84F6-5F79CC390D10', @devAppId, @keyId, '2012/1/1', 0, GETDATE(), 0)
-- QA
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '6A62316D-B6D2-4607-848A-047588CDA1A8')
	INSERT INTO [AppConfigValue] VALUES ('6A62316D-B6D2-4607-848A-047588CDA1A8', @qaAppId, @keyId, '2012/1/1', 0, GETDATE(), 0)
-- Prod.  Cattle Prod.  
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '8A5FF21E-9ACE-403E-8BFB-7338C71151F5')
	INSERT INTO [AppConfigValue] VALUES ('8A5FF21E-9ACE-403E-8BFB-7338C71151F5', @prodAppId, @keyId, '2012/1/1', 0, GETDATE(), 0)
	

-- 5th key:  Restore Path.  Only used in the event a Restore is needed.  Since restores are done manually, the user running the restore
--			can place the restore files on his local system and set the path here.  
IF NOT EXISTS(SELECT KeyID FROM AppConfigKey WHERE Name = 'RestorePath')
	INSERT INTO AppConfigKey (Name, LastUpdate, LastUpdateUserID) VALUES 	('RestorePath', GETDATE(), 0)
	
SELECT @keyId = KeyID FROM AppConfigKey WHERE Name = 'RestorePath'

-- test
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '20C5DDAC-659C-4B81-84F6-5F79CC390D10')
	INSERT INTO [AppConfigValue] VALUES ('20C5DDAC-659C-4B81-84F6-5F79CC390D10', @devAppId, @keyId, 'E:\ScheduledJobs\IRMAUserAudit\restore', 0, GETDATE(), 0)
-- QA
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '6A62316D-B6D2-4607-848A-047588CDA1A8')
	INSERT INTO [AppConfigValue] VALUES ('6A62316D-B6D2-4607-848A-047588CDA1A8', @qaAppId, @keyId, 'E:\ScheduledJobs\IRMAUserAudit\restore', 0, GETDATE(), 0)
-- Prod.  Cattle Prod.  
IF NOT EXISTS (SELECT TOP 1 KeyID FROM AppConfigValue WHERE KeyID = @keyId AND EnvironmentID = '8A5FF21E-9ACE-403E-8BFB-7338C71151F5')
	INSERT INTO [AppConfigValue] VALUES ('8A5FF21E-9ACE-403E-8BFB-7338C71151F5', @prodAppId, @keyId, 'E:\ScheduledJobs\IRMAUserAudit\restore', 0, GETDATE(), 0)



