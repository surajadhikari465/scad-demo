CREATE TABLE [dbo].[AppConfigKey] (
    [KeyID]            INT           IDENTITY (1, 1) NOT NULL,
    [Name]             VARCHAR (150) NOT NULL,
    [Deleted]          BIT           CONSTRAINT [DF_AppConfigKey_Deleted] DEFAULT ((0)) NOT NULL,
    [LastUpdate]       DATETIME      NOT NULL,
    [LastUpdateUserID] INT           NOT NULL,
    CONSTRAINT [PK_AppConfigKey] PRIMARY KEY CLUSTERED ([KeyID] ASC),
    CONSTRAINT [FK_AppConfigKey_UserID] FOREIGN KEY ([LastUpdateUserID]) REFERENCES [dbo].[Users] ([User_ID])
);


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
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigKey] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigKey] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigKey] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigKey] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigKey] TO [IConInterface]
    AS [dbo];

