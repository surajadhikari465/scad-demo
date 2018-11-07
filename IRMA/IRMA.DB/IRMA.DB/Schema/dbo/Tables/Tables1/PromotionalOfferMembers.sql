CREATE TABLE [dbo].[PromotionalOfferMembers] (
    [OfferMember_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Offer_ID]       INT           NOT NULL,
    [Group_ID]       INT           NOT NULL,
    [Quantity]       INT           NULL,
    [Purpose]        TINYINT       NULL,
    [JoinLogic]      TINYINT       NULL,
    [modified]       SMALLDATETIME NULL,
    [User_ID]        INT           NULL,
    CONSTRAINT [FK_PromotionalOfferMember_ItemGroup] FOREIGN KEY ([Group_ID]) REFERENCES [dbo].[ItemGroup] ([Group_ID]),
    CONSTRAINT [FK_PromotionalOfferMember_PromotionalOffer] FOREIGN KEY ([Offer_ID]) REFERENCES [dbo].[PromotionalOffer] ([Offer_ID]),
    CONSTRAINT [PK_PromotionalOfferMember] UNIQUE NONCLUSTERED ([OfferMember_ID] ASC)
);


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
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferMembers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferMembers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferMembers] TO [IRMAReportsRole]
    AS [dbo];

