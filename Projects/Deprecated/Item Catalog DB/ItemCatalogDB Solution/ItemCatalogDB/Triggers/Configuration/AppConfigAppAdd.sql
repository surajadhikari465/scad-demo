IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'AppConfigAppAdd')
BEGIN
	DROP  TRIGGER [dbo].[AppConfigAppAdd]
END
GO

CREATE Trigger [dbo].[AppConfigAppAdd] 
ON [dbo].[AppConfigApp]
FOR INSERT
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO AppConfigHistory(
			[Action],
			EnvironmentID,
			EnvironmentName,
			ApplicationID,
			ApplicationName,
			SystemTime,
			User_ID)
	SELECT
			'CREATE APPLICATION',
			Inserted.EnvironmentID,
			AppConfigEnv.Name,
			Inserted.ApplicationID,
			Inserted.[Name],
			GetDate(),
			Inserted.LastUpdateUserID
    FROM Inserted
    	INNER JOIN AppConfigEnv ON AppConfigEnv.EnvironmentID = Inserted.EnvironmentID

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('AppConfigAppAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
