CREATE TABLE [dbo].[AppConfigApp] (
    [ApplicationID]    UNIQUEIDENTIFIER                               CONSTRAINT [DF_AppConfigApp_ApplicationID] DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [EnvironmentID]    UNIQUEIDENTIFIER                               NOT NULL,
    [TypeID]           INT                                            NOT NULL,
    [Name]             VARCHAR (50)                                   NOT NULL,
    [Configuration]    XML(DOCUMENT [dbo].[ApplicationConfiguration]) CONSTRAINT [DF_AppConfigApp_Configuration] DEFAULT ('<?xml version="1.0" encoding="utf-8" ?><configuration><appSettings><add key="Default" value="null"/></appSettings></configuration>') NOT NULL,
    [Deleted]          BIT                                            CONSTRAINT [DF_AppConfigApp_Deleted] DEFAULT ((0)) NOT NULL,
    [LastUpdate]       DATETIME                                       NOT NULL,
    [LastUpdateUserID] INT                                            NOT NULL,
    CONSTRAINT [PK_AppConfigApp] PRIMARY KEY CLUSTERED ([ApplicationID] ASC, [EnvironmentID] ASC),
    CONSTRAINT [FK_AppConfigApp_EnvID] FOREIGN KEY ([EnvironmentID]) REFERENCES [dbo].[AppConfigEnv] ([EnvironmentID]),
    CONSTRAINT [FK_AppConfigApp_TypeID] FOREIGN KEY ([TypeID]) REFERENCES [dbo].[AppConfigType] ([TypeID]),
    CONSTRAINT [FK_AppConfigApp_UserID] FOREIGN KEY ([LastUpdateUserID]) REFERENCES [dbo].[Users] ([User_ID])
);


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
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigApp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigApp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigApp] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigApp] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigApp] TO [IConInterface]
    AS [dbo];

