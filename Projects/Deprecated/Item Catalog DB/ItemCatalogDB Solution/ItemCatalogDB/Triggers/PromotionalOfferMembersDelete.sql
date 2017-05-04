if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PromotionalOfferMembersDelete]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[PromotionalOfferMembersDelete]
GO

PRINT N'CREATE TRIGGER [PromotionalOfferMembersDelete]'
GO
 
/**
	Tracks deletions in PromotionalOfferMembers
**/
CREATE TRIGGER [PromotionalOfferMembersDelete] ON [dbo].[PromotionalOfferMembers] 
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Deleted records, including date of deletion
	**/
    INSERT INTO PromotionalOfferMembersHistory (
			[PromotionalOfferMembersHistory].[OfferMember_ID]
			,[PromotionalOfferMembersHistory].[Offer_ID]
			,[PromotionalOfferMembersHistory].[Group_ID]
			,[PromotionalOfferMembersHistory].[Quantity]
			,[PromotionalOfferMembersHistory].[Purpose]
			,[PromotionalOfferMembersHistory].[JoinLogic]
			,[PromotionalOfferMembersHistory].[modified]
			,[PromotionalOfferMembersHistory].[User_ID]
			,[PromotionalOfferMembersHistory].[User_Name]
			,[PromotionalOfferMembersHistory].[Host_Name]
			,[PromotionalOfferMembersHistory].[Effective_Date]
			,[PromotionalOfferMembersHistory].[Deleted])
    SELECT	 [Deleted].[OfferMember_ID]
			,[Deleted].[Offer_ID]
			,[Deleted].[Group_ID]
			,[Deleted].[Quantity]
			,[Deleted].[Purpose]
			,[Deleted].[JoinLogic]
			,[Deleted].[modified]
			,[Deleted].[User_ID]
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
        RAISERROR ('PromotionalOfferMembersDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO


