CREATE TABLE [dbo].[AppConfigEnv] (
    [EnvironmentID]    UNIQUEIDENTIFIER CONSTRAINT [DF_AppConfigEnv_Identifier] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [Name]             VARCHAR (50)     NOT NULL,
    [ShortName]        VARCHAR (5)      NOT NULL,
    [Deleted]          BIT              CONSTRAINT [DF_AppConfigEnv_Deleted] DEFAULT ((0)) NOT NULL,
    [LastUpdate]       DATETIME         NOT NULL,
    [LastUpdateUserID] INT              NOT NULL,
    CONSTRAINT [PK_AppConfigEnv] PRIMARY KEY CLUSTERED ([EnvironmentID] ASC),
    CONSTRAINT [FK_AppConfigEnv_UserID] FOREIGN KEY ([LastUpdateUserID]) REFERENCES [dbo].[Users] ([User_ID])
);


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
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigEnv] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigEnv] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigEnv] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigEnv] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigEnv] TO [IConInterface]
    AS [dbo];

