--PBI21615-Generate email when e-invoicing XML file moves to ERROR folder
--E-Invoicing_FromEmailAddress
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
	
	SELECT @ApplicationName = 'E-INVOICING JOB'

    SELECT @KeyName = 'E-Invoicing_FromEmailAddress'
    IF NOT EXISTS(SELECT * FROM AppConfigKey WHERE Name = @KeyName)
        EXEC AppConfig_AddKey @Result,@KeyName,@User_ID

    SELECT @KeyID = (SELECT KeyID from AppConfigKey where Name = @KeyName)    

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'E-Invoicing-Dev@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'E-Invoicing-QA@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'E-Invoicing@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

----E-Invoicing_ToEmailAddress

    SELECT @KeyName = 'E-Invoicing_ToEmailAddress'
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
	SET @Value = 'scms.l2@wholefoods.opsgenie.net, SCMS-L2@wholefoods.com'
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

--E-Invoicing_CcEmailAddress

    SELECT @KeyName = 'E-Invoicing_CcEmailAddress'
    IF NOT EXISTS(SELECT * FROM AppConfigKey WHERE Name = @KeyName)
        EXEC AppConfig_AddKey @Result,@KeyName,@User_ID

    SELECT @KeyID = (SELECT KeyID from AppConfigKey where Name = @KeyName)    

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'TEST')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = ''
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'QUALITY ASSURANCE')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = ''
    IF NOT EXISTS(SELECT * FROM AppConfigValue WHERE KeyID = @KeyID AND EnvironmentID = @EnvironmentID AND ApplicationID = @ApplicationID)
        EXEC AppConfig_AddKeyValue @ApplicationID,@EnvironmentID,@UpdateExistingKeyValue,@KeyID,@Value,@User_ID

    SELECT @EnvironmentID = (SELECT EnvironmentID FROM AppConfigEnv WHERE Name = 'PRODUCTION')	
    SELECT @ApplicationID = (SELECT ApplicationID FROM AppConfigApp WHERE Name = @ApplicationName AND EnvironmentID = @EnvironmentID)
	SET @Value = 'SCM-L3@wholefoods.com'
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