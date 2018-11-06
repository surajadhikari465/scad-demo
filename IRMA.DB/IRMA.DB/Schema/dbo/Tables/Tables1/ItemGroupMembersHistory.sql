CREATE TABLE [dbo].[ItemGroupMembersHistory] (
    [GroupHistory_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Group_ID]        INT           NULL,
    [Item_Key]        INT           NULL,
    [modifieddate]    SMALLDATETIME NULL,
    [User_ID]         INT           NULL,
    [OfferChgTypeID]  TINYINT       NULL,
    [User_Name]       VARCHAR (20)  NULL,
    [Host_Name]       VARCHAR (20)  NULL,
    [Effective_Date]  DATETIME      CONSTRAINT [DF_ItemGroupMembersHistory_Effective_Date] DEFAULT (getdate()) NOT NULL,
    [Deleted]         BIT           CONSTRAINT [DF_ItemGroupMembersHistory_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ItemGroupMembersHistory] PRIMARY KEY CLUSTERED ([GroupHistory_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupMembersHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupMembersHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupMembersHistory] TO [IRMAReportsRole]
    AS [dbo];

