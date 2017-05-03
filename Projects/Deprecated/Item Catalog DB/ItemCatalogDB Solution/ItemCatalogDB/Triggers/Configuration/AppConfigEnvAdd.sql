IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'AppConfigEnvAdd')
BEGIN
	DROP  TRIGGER [dbo].[AppConfigEnvAdd]
END
GO

CREATE Trigger [dbo].[AppConfigEnvAdd] 
ON [dbo].[AppConfigEnv]
FOR INSERT
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO AppConfigHistory(
			[Action],
			EnvironmentID,
			EnvironmentName,
			SystemTime,
			User_ID)
	SELECT
			'CREATE ENVIRONMENT',
			Inserted.EnvironmentID,
			Inserted.[Name],
			GetDate(),
			Inserted.LastUpdateUserID
    FROM Inserted

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AppConfigEnvAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO