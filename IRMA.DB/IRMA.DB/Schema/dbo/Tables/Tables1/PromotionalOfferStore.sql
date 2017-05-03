CREATE TABLE [dbo].[PromotionalOfferStore] (
    [Store_No]       INT     NOT NULL,
    [Offer_ID]       INT     NOT NULL,
    [Active]         BIT     CONSTRAINT [DF_PromotionalOfferStore_Active] DEFAULT ((0)) NOT NULL,
    [OfferChgTypeID] TINYINT CONSTRAINT [DF_PromotionalOfferStore_OfferChgTypeID] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_PromotionalOfferStore] PRIMARY KEY CLUSTERED ([Store_No] ASC, [Offer_ID] ASC),
    CONSTRAINT [FK_PromotionalOfferStore_OfferChgTypeID] FOREIGN KEY ([OfferChgTypeID]) REFERENCES [dbo].[OfferChgType] ([OfferChgTypeID]),
    CONSTRAINT [FK_PromotionalOfferStore_OfferID] FOREIGN KEY ([Offer_ID]) REFERENCES [dbo].[PromotionalOffer] ([Offer_ID])
);


GO
/**
	Tracks deletions in PromotionalOfferStore
**/
CREATE TRIGGER [PromotionalOfferStoreDelete] ON [dbo].[PromotionalOfferStore] 
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Deleted records, including date of deletion
	**/
    INSERT INTO PromotionalOfferStoreHistory (
			[PromotionalOfferStoreHistory].[Store_No]
			,[PromotionalOfferStoreHistory].[Offer_ID]
			,[PromotionalOfferStoreHistory].[Active]
			,[PromotionalOfferStoreHistory].[User_Name]
			,[PromotionalOfferStoreHistory].[Host_Name]
			,[PromotionalOfferStoreHistory].[Effective_Date]
			,[PromotionalOfferStoreHistory].[Deleted])
    SELECT	 [Deleted].[Store_No]
			,[Deleted].[Offer_ID]
			,[Deleted].[Active]
			,SUSER_NAME()
			,HOST_NAME()
			,GETDATE()
 			,1		-- DELETED
    FROM [Deleted]

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PromotionalOfferStoreDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
/**
	Track Creates and Updates to PromotionalOfferStore
**/
CREATE TRIGGER [PromotionalOfferStoreAddUpdate] ON [dbo].[PromotionalOfferStore]
FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Inserted records 
	**/
    INSERT INTO PromotionalOfferStoreHistory (
			 [PromotionalOfferStoreHistory].[Store_No]
			,[PromotionalOfferStoreHistory].[Offer_ID]
			,[PromotionalOfferStoreHistory].[Active]
			,[PromotionalOfferStoreHistory].[User_Name]
			,[PromotionalOfferStoreHistory].[Host_Name]
			,[PromotionalOfferStoreHistory].[Effective_Date])
    SELECT	 [PromotionalOfferStore].[Store_No]
			,[PromotionalOfferStore].[Offer_ID]
			,[PromotionalOfferStore].[Active]
			,SUSER_NAME()
			,HOST_NAME()
			,GETDATE()
    FROM PromotionalOfferStore
    INNER JOIN
        Inserted
        ON Inserted.Store_No = PromotionalOfferStore.Store_No AND Inserted.Offer_ID = PromotionalOfferStore.Offer_ID

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PromotionalOfferStoreAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferStore] TO [IRMAReportsRole]
    AS [dbo];

