BEGIN
	DECLARE @Result INT
	DECLARE @KeyName VARCHAR(50)
	DECLARE @ApplicationName VARCHAR(50)
	DECLARE @User_ID INT = 0
	DECLARE @ApplicationID UNIQUEIDENTIFIER
	DECLARE @EnvironmentID UNIQUEIDENTIFIER
	DECLARE @UpdateExistingKeyValue BIT
	DECLARE @KeyID INT
	DECLARE @ValueDev VARCHAR(350)
	DECLARE @ValueQA VARCHAR(350)
	DECLARE @ValueProd VARCHAR(350)
	DECLARE @RegionCode VARCHAR(2)

	SELECT @User_ID = ISNULL((
				SELECT user_id
				FROM Users
				WHERE UserName = 'System'
				), 0)

	SELECT @UpdateExistingKeyValue = 1

	SELECT @RegionCode = RegionCode
	FROM Region

	SELECT @KeyName = 'EnableErrorAlertsForBatchesInSent'

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigKey
			WHERE Name = @KeyName
			)
		EXEC AppConfig_AddKey @Result
			,@KeyName
			,@User_ID

	SELECT @KeyID = (
			SELECT KeyID
			FROM AppConfigKey
			WHERE Name = @KeyName
			)

	SET @ValueDev = '0'
	SET @ValueQA = '0'
	SET @ValueProd = '1'

	SELECT @ApplicationName = 'POS PUSH JOB'

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'TEST'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueDev
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'QUALITY ASSURANCE'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueQA
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'PRODUCTION'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueProd
			,@User_ID
END
GO

BEGIN
	DECLARE @Result INT
	DECLARE @KeyName VARCHAR(50)
	DECLARE @ApplicationName VARCHAR(50)
	DECLARE @User_ID INT = 0
	DECLARE @ApplicationID UNIQUEIDENTIFIER
	DECLARE @EnvironmentID UNIQUEIDENTIFIER
	DECLARE @UpdateExistingKeyValue BIT
	DECLARE @KeyID INT
	DECLARE @ValueDev VARCHAR(350)
	DECLARE @ValueQA VARCHAR(350)
	DECLARE @ValueProd VARCHAR(350)
	DECLARE @RegionCode VARCHAR(2)

	SELECT @User_ID = ISNULL((
				SELECT user_id
				FROM Users
				WHERE UserName = 'System'
				), 0)

	SELECT @UpdateExistingKeyValue = 1

	SELECT @RegionCode = RegionCode
	FROM Region

	SELECT @KeyName = 'EnableErrorAlerts'

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigKey
			WHERE Name = @KeyName
			)
		EXEC AppConfig_AddKey @Result
			,@KeyName
			,@User_ID

	SELECT @KeyID = (
			SELECT KeyID
			FROM AppConfigKey
			WHERE Name = @KeyName
			)

	SET @ValueDev = '0'
	SET @ValueQA = '0'
	SET @ValueProd = '1'

	SELECT @ApplicationName = 'POS PUSH JOB'

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'TEST'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueDev
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'QUALITY ASSURANCE'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueQA
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'PRODUCTION'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueProd
			,@User_ID
END
GO

BEGIN
	DECLARE @Result INT
	DECLARE @KeyName VARCHAR(50)
	DECLARE @ApplicationName VARCHAR(50)
	DECLARE @User_ID INT = 0
	DECLARE @ApplicationID UNIQUEIDENTIFIER
	DECLARE @EnvironmentID UNIQUEIDENTIFIER
	DECLARE @UpdateExistingKeyValue BIT
	DECLARE @KeyID INT
	DECLARE @ValueDev VARCHAR(350)
	DECLARE @ValueQA VARCHAR(350)
	DECLARE @ValueProd VARCHAR(350)
	DECLARE @RegionCode VARCHAR(2)

	SELECT @User_ID = ISNULL((
				SELECT user_id
				FROM Users
				WHERE UserName = 'System'
				), 0)

	SELECT @UpdateExistingKeyValue = 1

	SELECT @RegionCode = RegionCode
	FROM Region

	SELECT @KeyName = 'ErrorAlertServiceKey'

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigKey
			WHERE Name = @KeyName
			)
		EXEC AppConfig_AddKey @Result
			,@KeyName
			,@User_ID

	SELECT @KeyID = (
			SELECT KeyID
			FROM AppConfigKey
			WHERE Name = @KeyName
			)

	SET @ValueDev = '5c9df61f-ff16-4eef-8533-6fd8ed9664a3'
	SET @ValueQA = '5c9df61f-ff16-4eef-8533-6fd8ed9664a3'
	SET @ValueProd = '5c9df61f-ff16-4eef-8533-6fd8ed9664a3'

	SELECT @ApplicationName = 'POS PUSH JOB'

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'TEST'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueDev
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'QUALITY ASSURANCE'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueQA
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'PRODUCTION'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueProd
			,@User_ID
END
GO

BEGIN
	DECLARE @Result INT
	DECLARE @KeyName VARCHAR(50)
	DECLARE @ApplicationName VARCHAR(50)
	DECLARE @User_ID INT = 0
	DECLARE @ApplicationID UNIQUEIDENTIFIER
	DECLARE @EnvironmentID UNIQUEIDENTIFIER
	DECLARE @UpdateExistingKeyValue BIT
	DECLARE @KeyID INT
	DECLARE @ValueDev VARCHAR(350)
	DECLARE @ValueQA VARCHAR(350)
	DECLARE @ValueProd VARCHAR(350)
	DECLARE @RegionCode VARCHAR(2)

	SELECT @User_ID = ISNULL((
				SELECT user_id
				FROM Users
				WHERE UserName = 'System'
				), 0)

	SELECT @UpdateExistingKeyValue = 1

	SELECT @RegionCode = RegionCode
	FROM Region

	SELECT @KeyName = 'ErrorAlertUrl'

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigKey
			WHERE Name = @KeyName
			)
		EXEC AppConfig_AddKey @Result
			,@KeyName
			,@User_ID

	SELECT @KeyID = (
			SELECT KeyID
			FROM AppConfigKey
			WHERE Name = @KeyName
			)

	SET @ValueDev = 'https://api.opsgenie.com/v2/alerts'
	SET @ValueQA = 'https://api.opsgenie.com/v2/alerts'
	SET @ValueProd = 'https://api.opsgenie.com/v2/alerts'

	SELECT @ApplicationName = 'POS PUSH JOB'

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'TEST'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueDev
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'QUALITY ASSURANCE'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueQA
			,@User_ID

	SELECT @EnvironmentID = (
			SELECT EnvironmentID
			FROM AppConfigEnv
			WHERE Name = 'PRODUCTION'
			)

	SELECT @ApplicationID = (
			SELECT ApplicationID
			FROM AppConfigApp
			WHERE Name = @ApplicationName
				AND EnvironmentID = @EnvironmentID
			)

	IF NOT EXISTS (
			SELECT *
			FROM AppConfigValue
			WHERE KeyID = @KeyID
				AND EnvironmentID = @EnvironmentID
				AND ApplicationID = @ApplicationID
			)
		EXEC AppConfig_AddKeyValue @ApplicationID
			,@EnvironmentID
			,@UpdateExistingKeyValue
			,@KeyID
			,@ValueProd
			,@User_ID
END
GO

DECLARE @client_name varchar(50)		 = 'POS PUSH JOB';
DECLARE @key_1 varchar(150)            = 'PagerDutyServiceKey'    
DECLARE @key_2 varchar(150)            = 'PagerDutyUrl'

DELETE acv
FROM [dbo].[AppConfigValue] acv
	INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
    INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
	INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
	WHERE aca.[Name]=@client_name
		  AND (ack.[Name] = @key_1)

DELETE acv
FROM [dbo].[AppConfigValue] acv
	INNER JOIN [dbo].[AppConfigEnv] ace ON acv.EnvironmentID = ace.EnvironmentID 
    INNER JOIN [dbo].[AppConfigApp] aca ON acv.ApplicationID = aca.ApplicationID 
	INNER JOIN [dbo].[AppConfigKey] ack ON acv.KeyID = ack.KeyID 
	WHERE aca.[Name]=@client_name
		  AND (ack.[Name] = @key_2)