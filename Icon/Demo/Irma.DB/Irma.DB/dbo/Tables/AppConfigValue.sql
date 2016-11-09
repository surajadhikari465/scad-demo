CREATE TABLE [dbo].[AppConfigValue] (
    [EnvironmentID]    UNIQUEIDENTIFIER NOT NULL,
    [ApplicationID]    UNIQUEIDENTIFIER NOT NULL,
    [KeyID]            INT              NOT NULL,
    [Value]            VARCHAR (350)    NOT NULL,
    [Deleted]          BIT              CONSTRAINT [DF_AppConfigValue_Deleted] DEFAULT ((0)) NOT NULL,
    [LastUpdate]       DATETIME         NOT NULL,
    [LastUpdateUserID] INT              NOT NULL,
    CONSTRAINT [PK_AppConfigValue] PRIMARY KEY CLUSTERED ([EnvironmentID] ASC, [ApplicationID] ASC, [KeyID] ASC),
    CONSTRAINT [FK_AppConfigValue_AppConfigKey] FOREIGN KEY ([KeyID]) REFERENCES [dbo].[AppConfigKey] ([KeyID]),
    CONSTRAINT [FK_AppConfigValue_UserID] FOREIGN KEY ([LastUpdateUserID]) REFERENCES [dbo].[Users] ([User_ID])
);


GO
CREATE Trigger [dbo].[AppConfigValueUpdate] 
ON [dbo].[AppConfigValue]
FOR UPDATE
AS
BEGIN

    DECLARE @Error_No int
    SELECT @Error_No = 0

	DECLARE @UpdateValue bit
	DECLARE @Delete bit

	IF UPDATE([Value])
		SELECT @UpdateValue = 1

	IF UPDATE(Deleted)
		SELECT @Delete = 1
 
    INSERT INTO AppConfigHistory(
			[Action],
			ApplicationID,
			ApplicationName,
			EnvironmentID,
			EnvironmentName,
			KeyID,
			KeyName,
			[Value],
			Deleted,
			SystemTime,
			User_ID)
	SELECT
			CASE 
				WHEN @UpdateValue = 1 THEN 'KEY/PAIR VALUE UPDATED'
				WHEN @Delete = 1 THEN 'KEY/VALUE PAIR DELETED'
				ELSE 'ADD KEY VALUE PAIR'
			END,
			Inserted.ApplicationID,
			AppConfigApp.Name,
			Inserted.EnvironmentID,
			AppConfigEnv.Name,
			Inserted.KeyID,
			AppConfigKey.Name,
			Inserted.Value,
			Inserted.Deleted,
			Inserted.LastUpdate,
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
        RAISERROR ('AppConfigValueUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
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
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigValue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigValue] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigValue] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigValue] TO [IConInterface]
    AS [dbo];

