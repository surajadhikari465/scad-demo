CREATE TABLE [dbo].[ShelfTagRuleType] (
    [ShelfTagRuleTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [ShelfTagRuleDesc]   VARCHAR (50) NOT NULL,
    [CreateDate]         DATETIME     CONSTRAINT [DF_ShelfTagRuleType_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]         DATETIME     CONSTRAINT [DF_ShelfTagRuleType_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [UserID]             INT          NULL,
    CONSTRAINT [PK_ShelfTagRuleType] PRIMARY KEY CLUSTERED ([ShelfTagRuleTypeID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagRuleType] TO [IRMAReportsRole]
    AS [dbo];

