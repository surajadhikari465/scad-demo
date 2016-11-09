CREATE TABLE [dbo].[ShelfTagRule] (
    [ShelfTagRuleID]         INT          IDENTITY (1, 1) NOT NULL,
    [ShelfTagRuleTypeID]     INT          NULL,
    [AttributeIdentifier_id] INT          NULL,
    [RuleDesc]               VARCHAR (50) NOT NULL,
    [RulePriority]           INT          NOT NULL,
    [LabelType_ID]           INT          NULL,
    [PriceChgTypeID]         TINYINT      NULL,
    [SubTeam_No]             INT          NULL,
    [TaxJurisdictionID]      INT          NULL,
    [Store_No]               INT          NULL,
    [Zone_ID]                INT          NULL,
    [ShelfTag_Type]          VARCHAR (5)  NULL,
    [Exempt_ShelfTag_Type]   VARCHAR (5)  NULL,
    [CreateDate]             DATETIME     CONSTRAINT [DF_ShelfTagRule_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]             DATETIME     CONSTRAINT [DF_ShelfTagRule_ModifyDate] DEFAULT (getdate()) NOT NULL,
    [ItemChgTypeID]          TINYINT      NULL,
    CONSTRAINT [PK_ShelfTagRule] PRIMARY KEY CLUSTERED ([ShelfTagRuleID] ASC),
    CONSTRAINT [FK_ShelfTagRule_AttributeIdentifier] FOREIGN KEY ([AttributeIdentifier_id]) REFERENCES [dbo].[AttributeIdentifier] ([AttributeIdentifier_ID]),
    CONSTRAINT [FK_ShelfTagRule_ItemChgTypeID] FOREIGN KEY ([ItemChgTypeID]) REFERENCES [dbo].[ItemChgType] ([ItemChgTypeID]),
    CONSTRAINT [FK_ShelfTagRule_LabelType] FOREIGN KEY ([LabelType_ID]) REFERENCES [dbo].[LabelType] ([LabelType_ID]),
    CONSTRAINT [FK_ShelfTagRule_PriceChgType] FOREIGN KEY ([PriceChgTypeID]) REFERENCES [dbo].[PriceChgType] ([PriceChgTypeID]),
    CONSTRAINT [FK_ShelfTagRule_ShelfTagRuleType] FOREIGN KEY ([ShelfTagRuleTypeID]) REFERENCES [dbo].[ShelfTagRuleType] ([ShelfTagRuleTypeID]),
    CONSTRAINT [FK_ShelfTagRule_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_ShelfTagRule_Subteam] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_ShelfTagRule_TaxJurisdiction] FOREIGN KEY ([TaxJurisdictionID]) REFERENCES [dbo].[TaxJurisdiction] ([TaxJurisdictionID]),
    CONSTRAINT [FK_ShelfTagRule_Zone] FOREIGN KEY ([Zone_ID]) REFERENCES [dbo].[Zone] ([Zone_ID])
);




GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTagRule] TO [IRMAReportsRole]
    AS [dbo];


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ShelfTagRule]
    ON [dbo].[ShelfTagRule]([RulePriority] ASC, [LabelType_ID] ASC, [PriceChgTypeID] ASC, [Zone_ID] ASC, [TaxJurisdictionID] ASC, [ShelfTag_Type] ASC, [Exempt_ShelfTag_Type] ASC);

