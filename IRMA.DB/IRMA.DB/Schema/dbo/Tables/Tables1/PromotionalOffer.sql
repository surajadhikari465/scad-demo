CREATE TABLE [dbo].[PromotionalOffer] (
    [Offer_ID]         INT           IDENTITY (1, 1) NOT NULL,
    [Description]      VARCHAR (20)  NULL,
    [PricingMethod_ID] TINYINT       NULL,
    [StartDate]        SMALLDATETIME NULL,
    [EndDate]          SMALLDATETIME NULL,
    [RewardType]       INT           NULL,
    [RewardQuantity]   DECIMAL (18)  NULL,
    [RewardGroupID]    INT           NULL,
    [createdate]       SMALLDATETIME NULL,
    [modifieddate]     SMALLDATETIME NULL,
    [User_ID]          INT           NULL,
    [ReferenceCode]    VARCHAR (20)  NULL,
    [TaxClass_ID]      INT           NULL,
    [SubTeam_No]       INT           NULL,
    [RewardAmount]     MONEY         NULL,
    [IsEdited]         INT           CONSTRAINT [DF_PromotionalOffer_IsEdited] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PromotionalOffer] PRIMARY KEY CLUSTERED ([Offer_ID] ASC),
    CONSTRAINT [FK_PromotionalOffer_PricingMethod] FOREIGN KEY ([PricingMethod_ID]) REFERENCES [dbo].[PricingMethod] ([PricingMethod_ID]),
    CONSTRAINT [FK_PromotionalOffer_SubTeam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


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
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOffer] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOffer] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOffer] TO [IRMAReportsRole]
    AS [dbo];

