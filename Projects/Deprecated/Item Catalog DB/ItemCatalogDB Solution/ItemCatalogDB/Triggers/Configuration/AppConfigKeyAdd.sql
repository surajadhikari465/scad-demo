IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'AppConfigKeyAdd')
BEGIN
	DROP  TRIGGER [dbo].[AppConfigKeyAdd]
END
GO

CREATE Trigger [dbo].[AppConfigKeyAdd] 
ON [dbo].[AppConfigKey]
FOR INSERT
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO AppConfigHistory(
			[Action],
			KeyID,
			KeyName,
			SystemTime,
			User_ID)
	SELECT
			'CREATE NEW KEY',
			Inserted.KeyID,
			Inserted.[Name],
			GetDate(),
			Inserted.LastUpdateUserID
	FROM Inserted

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AppConfigKeyAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO