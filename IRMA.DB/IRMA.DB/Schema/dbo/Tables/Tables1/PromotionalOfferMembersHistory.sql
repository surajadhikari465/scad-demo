CREATE TABLE [dbo].[PromotionalOfferMembersHistory] (
    [OfferMemberHistory_ID] INT           IDENTITY (1, 1) NOT NULL,
    [OfferMember_ID]        INT           NULL,
    [Offer_ID]              INT           NULL,
    [Group_ID]              INT           NULL,
    [Quantity]              INT           NULL,
    [Purpose]               TINYINT       NULL,
    [JoinLogic]             TINYINT       NULL,
    [modified]              SMALLDATETIME NULL,
    [User_ID]               INT           NULL,
    [User_Name]             VARCHAR (20)  NULL,
    [Host_Name]             VARCHAR (20)  NULL,
    [Effective_Date]        DATETIME      CONSTRAINT [DF_PromotionalOfferMembersHistory_Effective_Date] DEFAULT (getdate()) NOT NULL,
    [Deleted]               BIT           CONSTRAINT [DF_PromotionalOfferMembersHistory_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PromotionalOfferMemberHistory] PRIMARY KEY CLUSTERED ([OfferMemberHistory_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferMembersHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferMembersHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromotionalOfferMembersHistory] TO [IRMAReportsRole]
    AS [dbo];

