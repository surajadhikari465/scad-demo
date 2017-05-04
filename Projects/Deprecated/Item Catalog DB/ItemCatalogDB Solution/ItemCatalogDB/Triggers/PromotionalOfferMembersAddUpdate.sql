if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PromotionalOfferMembersAddUpdate]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[PromotionalOfferMembersAddUpdate]
GO

-- TRIGGERS PromotionalOfferMembers
PRINT N'CREATE TRIGGER [PromotionalOfferMembersAddUpdate]'
GO
 
/**
	Track Creates and Updates to PromotionalOfferMembers
**/
CREATE TRIGGER [PromotionalOfferMembersAddUpdate] ON [dbo].[PromotionalOfferMembers] 
FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	/**
		Create History records for all Inserted records 
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
			,[PromotionalOfferMembersHistory].[Effective_Date])
    SELECT	 [PromotionalOfferMembers].[OfferMember_ID]
			,[PromotionalOfferMembers].[Offer_ID]
			,[PromotionalOfferMembers].[Group_ID]
			,[PromotionalOfferMembers].[Quantity]
			,[PromotionalOfferMembers].[Purpose]
			,[PromotionalOfferMembers].[JoinLogic]
			,[PromotionalOfferMembers].[modified]
			,[PromotionalOfferMembers].[User_ID]
			,SUSER_NAME()
			,HOST_NAME()
			,GETDATE()
    FROM PromotionalOfferMembers
    INNER JOIN
        Inserted
        ON Inserted.OfferMember_ID = PromotionalOfferMembers.OfferMember_ID 

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PromotionalOfferMembersAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

