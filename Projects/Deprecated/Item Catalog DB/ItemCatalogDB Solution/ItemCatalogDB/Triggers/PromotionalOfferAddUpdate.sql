if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PromotionalOfferAddUpdate]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[PromotionalOfferAddUpdate]
GO

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

PRINT N'CREATE TRIGGER [PromotionalOfferAddUpdate]'
GO
 
/**
	Track Creates and Updates to Promotional Offer
**/
CREATE TRIGGER [PromotionalOfferAddUpdate] ON [dbo].[PromotionalOffer] 
FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Inserted records 
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
										,[PromotionalOfferHistory].[Effective_Date])
    SELECT	 [PromotionalOffer].[Offer_ID]
			,[PromotionalOffer].[Description]
			,[PromotionalOffer].[PricingMethod_ID]
			,[PromotionalOffer].[StartDate]
			,[PromotionalOffer].[EndDate]
			,[PromotionalOffer].[RewardType]
			,[PromotionalOffer].[RewardQuantity]
			,[PromotionalOffer].[RewardAmount]
			,[PromotionalOffer].[RewardGroupID]
			,[PromotionalOffer].[createdate]
			,[PromotionalOffer].[modifieddate]
			,[PromotionalOffer].[User_ID]
			,[PromotionalOffer].[ReferenceCode]
			,[PromotionalOffer].[TaxClass_ID]
			,[PromotionalOffer].[SubTeam_No]
			,SUSER_NAME()
			,HOST_NAME()
			,GETDATE()
    FROM PromotionalOffer
    INNER JOIN
        Inserted
        ON Inserted.Offer_ID = PromotionalOffer.Offer_ID 

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PromotionalOfferAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO


