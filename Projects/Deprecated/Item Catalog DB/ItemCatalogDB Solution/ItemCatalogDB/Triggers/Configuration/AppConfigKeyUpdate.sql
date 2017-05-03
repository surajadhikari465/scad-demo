 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'AppConfigValueUpdate')
BEGIN
	DROP  TRIGGER [dbo].[AppConfigValueUpdate]
END
GO

CREATE Trigger [dbo].[AppConfigValueUpdate] 
ON [dbo].[AppConfigKey]
FOR UPDATE
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO AppConfigHistory(
			[Action],
			KeyID,
			KeyName,
			Deleted,
			SystemTime,
			User_ID)
	SELECT
			CASE 
				WHEN UPDATE(Deleted) THEN 'KEY DELETED' 
				ELSE 'CREATE NEW KEY' 
			END,
			Inserted.KeyID,
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
        RAISERROR ('AppConfigKeyUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO