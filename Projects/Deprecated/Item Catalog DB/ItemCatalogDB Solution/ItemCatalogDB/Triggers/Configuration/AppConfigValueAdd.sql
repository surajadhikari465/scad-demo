IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'AppConfigValueAdd')
BEGIN
	DROP  TRIGGER [dbo].[AppConfigValueAdd]
END
GO

CREATE Trigger [dbo].[AppConfigValueAdd] 
ON [dbo].[AppConfigValue]
FOR INSERT
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0
    
    INSERT INTO AppConfigHistory(
			[Action],
			ApplicationID,
			ApplicationName,
			EnvironmentID,
			EnvironmentName,
			KeyID,
			KeyName,
			[Value],
			SystemTime,
			User_ID)
	SELECT
			'ADD KEY VALUE PAIR',
			Inserted.ApplicationID,
			AppConfigApp.Name,
			Inserted.EnvironmentID,
			AppConfigEnv.Name,
			Inserted.KeyID,
			AppConfigKey.Name,
			Inserted.Value,
			GetDate(),
			Inserted.LastUpdateUserID
    FROM Inserted
		INNER JOIN AppConfigEnv ON AppConfigEnv.EnvironmentID = Inserted.EnvironmentID
		INNER JOIN AppConfigApp ON AppConfigApp.ApplicationID = Inserted.ApplicationID
		INNER JOIN AppConfigKey ON AppConfigKey.KeyID = Inserted.KeyID

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AppConfigValueAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO