if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PromotionalOfferStoreAddUpdate]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[PromotionalOfferStoreAddUpdate]
GO

-- TRIGGERS PromotionalOfferStore
PRINT N'CREATE TRIGGER [PromotionalOfferStoreAddUpdate]'
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

