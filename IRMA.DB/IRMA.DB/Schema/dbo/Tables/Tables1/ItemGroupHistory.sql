CREATE TABLE [dbo].[ItemGroupHistory] (
    [GroupHistory_ID] INT           IDENTITY (1, 1) NOT NULL,
    [Group_ID]        INT           NULL,
    [GroupName]       CHAR (50)     NULL,
    [GroupLogic]      BIT           NULL,
    [createdate]      SMALLDATETIME NULL,
    [modifieddate]    SMALLDATETIME NULL,
    [User_ID]         INT           NULL,
    [User_Name]       VARCHAR (20)  NULL,
    [Host_Name]       VARCHAR (20)  NULL,
    [Effective_Date]  DATETIME      CONSTRAINT [DF_ItemGroupHistory_Effective_Date] DEFAULT (getdate()) NOT NULL,
    [Deleted]         BIT           CONSTRAINT [DF_ItemGroupHistory_Deleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ItemGroupHistory] PRIMARY KEY CLUSTERED ([GroupHistory_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemGroupHistory] TO [IRMAReportsRole]
    AS [dbo];

