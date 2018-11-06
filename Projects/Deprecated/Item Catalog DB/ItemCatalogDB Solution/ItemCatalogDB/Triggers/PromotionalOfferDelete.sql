if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PromotionalOfferDelete]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[PromotionalOfferDelete]
GO

PRINT N'CREATE TRIGGER [PromotionalOfferDelete]'
GO
 
/**
	Tracks deletions in PromotionalOffer
**/
CREATE TRIGGER [PromotionalOfferDelete] ON [dbo].[PromotionalOffer] 
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Deleted records, including date of deletion
	**/
    INSERT INTO PromotionalOfferHistory ([PromotionalOfferHistory].[Offer_ID]
										,[PromotionalOfferHistory].[Description]
										,[PromotionalOfferHistory].[PricingMethod_ID]
										,[PromotionalOfferHistory].[StartDate]
										,[PromotionalOfferHistory].[EndDate]
										,[PromotionalOfferHistory].[RewardType]
										,[PromotionalOfferHistory].[RewardQuantity]
										,[PromotionalOfferHistory].[RewardAmount]
										,[PromotionalOfferHistory].[RewardGroupID]
										,[PromotionalOfferHistory].[createdate]
										,[PromotionalOfferHistory].[modifieddate]
										,[PromotionalOfferHistory].[User_ID]
										,[PromotionalOfferHistory].[ReferenceCode]
										,[PromotionalOfferHistory].[TaxClass_ID]
										,[PromotionalOfferHistory].[SubTeam_No]
										,[PromotionalOfferHistory].[User_Name]
										,[PromotionalOfferHistory].[Host_Name]
										,[PromotionalOfferHistory].[Effective_Date]
										,[PromotionalOfferHistory].[Deleted])
    SELECT	 [Deleted].[Offer_ID]
			,[Deleted].[Description]
			,[Deleted].[PricingMethod_ID]
			,[Deleted].[StartDate]
			,[Deleted].[EndDate]
			,[Deleted].[RewardType]
			,[Deleted].[RewardQuantity]
			,[Deleted].[RewardAmount]
			,[Deleted].[RewardGroupID]
			,[Deleted].[createdate]
			,[Deleted].[modifieddate]
			,[Deleted].[User_ID]
			,[Deleted].[ReferenceCode]
			,[Deleted].[TaxClass_ID]
			,[Deleted].[SubTeam_No]
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
        RAISERROR ('PromotionalOfferDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

