BEGIN
    DECLARE @Result					INT
    DECLARE @KeyName				VARCHAR(50)
    DECLARE @ApplicationName		VARCHAR(50)
    DECLARE @User_ID				INT = 0
    DECLARE @ApplicationID			UNIQUEIDENTIFIER
    DECLARE @EnvironmentID			UNIQUEIDENTIFIER
    DECLARE @UpdateExistingKeyValue BIT
    DECLARE @KeyID					INT
    DECLARE @Value					VARCHAR(350)
    DECLARE @RegionCode				VARCHAR(2)

    SELECT @User_ID = ISNULL((SELECT user_id FROM Users WHERE UserName = 'System'),0)
    SELECT @UpdateExistingKeyValue = 1
    SELECT @RegionCode = RegionCode FROM Region


    SELECT @KeyName = 'StoreBUsForAMZExtract'
    IF NOT EXISTS(SELECT * FROM AppConfigKey WHERE Name = @KeyName)
        EXEC AppConfig_AddKey @Result,@KeyName,@User_ID

    SELECT @KeyID = (SELECT KeyID from AppConfigKey where Name = @KeyName)

	IF @RegionCode = 'PN' 
		SET @Value = '10275|10103'
	ELSE
	IF @RegionCode = 'RM' 
		SET @Value = '10654'
	ELSE
	IF @RegionCode = 'NC' 
		SET @Value = '10396'
	ELSE
		SET @Value = ''

    SELECT @ApplicationName = 'IRMA CLIENT'

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID
END
GO
