IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'AppConfigEnvUpdate')
BEGIN
	DROP  TRIGGER [dbo].[AppConfigEnvUpdate]
END
GO

CREATE Trigger [dbo].[AppConfigEnvUpdate] 
ON [dbo].[AppConfigEnv]
FOR UPDATE
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO AppConfigHistory(
			[Action],
			EnvironmentID,
			EnvironmentName,
			Deleted,
			SystemTime,
			User_ID)
	SELECT
			CASE 
				WHEN UPDATE(Deleted) THEN 'ENVIRONMENT DELETED'
				ELSE 'CREATE ENVIRONMENT'
			END,
			Inserted.EnvironmentID,
			Inserted.[Name],
			Inserted.Deleted,
			GetDate(),
			Inserted.LastUpdateUserID
    FROM Inserted

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AppConfigEnvUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO