--PBI27833-Generate email when POS PUSH Job fails
--PosPush_FromEmailAddress
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

    SELECT @User_ID = ISNULL((SELECT user_id FROM Users WHERE UserName = 'System'),0)
    SELECT @UpdateExistingKeyValue = 1 
	
	SELECT @ApplicationName = 'POS PUSH JOB'

    SELECT @KeyName = 'PosPush_FromEmailAddress'
    IF NOT EXISTS(SELECT * FROM AppConfigKey WHERE Name = @KeyName)
        EXEC AppConfig_AddKey @Result,@KeyName,@User_ID

    SELECT @KeyID = (SELECT KeyID from AppConfigKey where Name = @KeyName)    

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'PosPush-Dev@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'PosPush-QA@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'PosPush@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

----PosPush_ToEmailAddress

    SELECT @KeyName = 'PosPush_ToEmailAddress'
    IF NOT EXISTS(SELECT * FROM AppConfigKey WHERE Name = @KeyName)
        EXEC AppConfig_AddKey @Result,@KeyName,@User_ID

    SELECT @KeyID = (SELECT KeyID from AppConfigKey where Name = @KeyName)    

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'scms.l2.qa@wholefoods.opsgenie.net'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'scms.l2.qa@wholefoods.opsgenie.net'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'scms.l2@wholefoods.opsgenie.net'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

--PosPush_CcEmailAddress

    SELECT @KeyName = 'PosPush_CcEmailAddress'
    IF NOT EXISTS(SELECT * FROM AppConfigKey WHERE Name = @KeyName)
        EXEC AppConfig_AddKey @Result,@KeyName,@User_ID

    SELECT @KeyID = (SELECT KeyID from AppConfigKey where Name = @KeyName)    

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'SCMS-L2@wholefoods.com, SCM-L3@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'SCMS-L2@wholefoods.com, SCM-L3@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'SCMS-L2@wholefoods.com, SCM-L3@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

--SMTPHost

    SELECT @KeyName = 'SMTPHost'
    IF NOT EXISTS(SELECT * FROM AppConfigKey WHERE Name = @KeyName)
        EXEC AppConfig_AddKey @Result,@KeyName,@User_ID

    SELECT @KeyID = (SELECT KeyID from AppConfigKey where Name = @KeyName)    

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'smtp1.wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'smtp1.wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'smtp1.wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID
END
GO