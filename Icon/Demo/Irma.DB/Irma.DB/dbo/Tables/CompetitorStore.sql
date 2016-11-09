CREATE TABLE [dbo].[CompetitorStore] (
    [CompetitorStoreID]    INT           IDENTITY (1, 1) NOT NULL,
    [CompetitorID]         INT           NOT NULL,
    [CompetitorLocationID] INT           NOT NULL,
    [Name]                 VARCHAR (50)  NOT NULL,
    [UpdateUserID]         INT           NOT NULL,
    [UpdateDateTime]       SMALLDATETIME NOT NULL,
    CONSTRAINT [PK_CompetitorStore] PRIMARY KEY CLUSTERED ([CompetitorStoreID] ASC),
    CONSTRAINT [FK_CompetitorStore_Competitor] FOREIGN KEY ([CompetitorID]) REFERENCES [dbo].[Competitor] ([CompetitorID]),
    CONSTRAINT [FK_CompetitorStore_CompetitorLocation] FOREIGN KEY ([CompetitorLocationID]) REFERENCES [dbo].[CompetitorLocation] ([CompetitorLocationID])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_CompetitorStore]
    ON [dbo].[CompetitorStore]([CompetitorID] ASC, [CompetitorLocationID] ASC, [Name] ASC);


GO
CREATE TRIGGER [CompetitorStoreAdd]
ON [CompetitorStore]
FOR INSERT 
AS 
BEGIN
    DECLARE @Error_No int
    SET @Error_No = 0

	INSERT INTO
		CompetitorStoreIdentifier
	SELECT
		I.CompetitorStoreID, 
		I.Name
	FROM
		Inserted I

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('CompetitorStoreAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CompetitorStore] TO [IRMAReportsRole]
    AS [dbo];

