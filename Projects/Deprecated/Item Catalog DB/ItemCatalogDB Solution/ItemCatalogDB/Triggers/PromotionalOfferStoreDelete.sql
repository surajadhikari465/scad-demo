if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PromotionalOfferStoreDelete]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[PromotionalOfferStoreDelete]
GO

PRINT N'CREATE TRIGGER [PromotionalOfferStoreDelete]'
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

