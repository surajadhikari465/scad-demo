IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'AppConfigAppUpdate')
BEGIN
	DROP  TRIGGER [dbo].[AppConfigAppUpdate]
END
GO

CREATE Trigger [dbo].[AppConfigAppUpdate] 
ON [dbo].[AppConfigApp]
FOR UPDATE
AS
BEGIN

DECLARE @Deleted as bit
DECLARE @Build As bit


IF UPDATE(Deleted)
	SELECT @Deleted = 1
	
IF UPDATE(Configuration)
	SELECT @Build = 1
	

    DECLARE @Error_No int
    SELECT @Error_No = 0
    
    IF @Build = 1 OR @Deleted = 1
		BEGIN
			INSERT INTO AppConfigHistory(
					[Action],
					ApplicationID,
					ApplicationName,
					EnvironmentID,
					EnvironmentName,
					Configuration,
					Deleted,
					SystemTime,
					User_ID)
			SELECT
					CASE 
						WHEN @Deleted = 1 THEN 'CONFIG DELETED'
						WHEN @Build = 1 THEN 'CONFIGURATION BUILD'
					END,
					Inserted.ApplicationID,
					AppConfigApp.Name,
					Inserted.EnvironmentID,
					AppConfigEnv.Name,
					Inserted.Configuration,
					Inserted.Deleted,
					GetDate(),
					Inserted.LastUpdateUserID
			FROM Inserted
				INNER JOIN AppConfigEnv ON AppConfigEnv.EnvironmentID = Inserted.EnvironmentID
				INNER JOIN AppConfigApp ON AppConfigApp.ApplicationID = Inserted.ApplicationID
		END

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AppConfigAppUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO