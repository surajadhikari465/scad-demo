DECLARE @client_name varchar(50)    = 'USER AUDIT';
DECLARE @env varchar(5)                --= 'TST';  --'QA' 'TST' 'PRD'
DECLARE @key_1 varchar(150)            = 'ExportBy'
DECLARE @key_2 varchar(150)			   = 'BasePath'
DECLARE @TestEnvironmentID UNIQUEIDENTIFIER
DECLARE @QAEnvironmentID   UNIQUEIDENTIFIER
DECLARE @PrdEnvironmentID  UNIQUEIDENTIFIER
DECLARE @TestValue         VARCHAR(350) = '\\sites\global\IRMA\IRMA User Audit Test'
DECLARE @QAValue           VARCHAR(350) = '\\sites\global\IRMA\IRMA User Audit QA'
DECLARE @PrdValue          VARCHAR(350) = '\\sites\global\IRMA\IRMA User Review'

SELECT @TestEnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')
SELECT @QAEnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')
SELECT @PrdEnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')

UPDATE acv
SET Value = 'region'
FROM [dbo].[AppConfigValue] acv
INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
    WHERE aca.[Name]=@client_name
     AND (ack.[Name] = @key_1)

UPDATE acv
SET Value = @TestValue
FROM [dbo].[AppConfigValue] acv
INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
    WHERE aca.[Name]=@client_name
     AND (ack.[Name] = @key_2)
	 AND acv.EnvironmentID = @TestEnvironmentID

UPDATE acv
SET Value = @QAValue
FROM [dbo].[AppConfigValue] acv
INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
    WHERE aca.[Name]=@client_name
     AND (ack.[Name] = @key_2)
	 AND acv.EnvironmentID = @QAEnvironmentID

UPDATE acv
SET Value = @PrdValue
FROM [dbo].[AppConfigValue] acv
INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
    WHERE aca.[Name]=@client_name
     AND (ack.[Name] = @key_2)
	 AND acv.EnvironmentID = @PrdEnvironmentID

 GO 