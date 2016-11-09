CREATE TABLE [dbo].[PromotionalOfferHistory] (
    [OfferHistory_ID]  INT           IDENTITY (1, 1) NOT NULL,
    [Offer_ID]         INT           NULL,
    [Description]      VARCHAR (100) NULL,
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
    [User_Name]        VARCHAR (20)  NULL,
    [Host_Name]        VARCHAR (20)  NULL,
    [Effective_Date]   DATETIME      CONSTRAINT [DF_PromotionalOfferHistory_Effective_Date] DEFAULT (getdate()) NOT NULL,
    [Deleted]          BIT           CONSTRAINT [DF_PromotionalOfferHistory_Deleted] DEFAULT ((0)) NOT NULL,
    [RewardAmount]     MONEY         NULL,
    CONSTRAINT [PK_PromotionalOfferHistory] PRIMARY KEY CLUSTERED ([OfferHistory_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferHistory] TO [IRMAReportsRole]
    AS [dbo];

