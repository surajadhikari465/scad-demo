CREATE TABLE [dbo].[PromotionalOfferStoreHistory] (
    [PromotionalOfferStoreHistory_ID] INT          IDENTITY (1, 1) NOT NULL,
    [Store_No]                        INT          NULL,
    [Offer_ID]                        INT          NULL,
    [Active]                          BIT          NULL,
    [User_Name]                       VARCHAR (20) NULL,
    [Host_Name]                       VARCHAR (20) NULL,
    [Effective_Date]                  DATETIME     CONSTRAINT [DF_PromotionalOfferStoreHistory_Effective_Date] DEFAULT (getdate()) NOT NULL,
    [Deleted]                         BIT          CONSTRAINT [DF_PromotionalOfferStoreHistory_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PromotionalOfferStoreHistory] PRIMARY KEY CLUSTERED ([PromotionalOfferStoreHistory_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferStoreHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferStoreHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferStoreHistory] TO [IRMAReportsRole]
    AS [dbo];

