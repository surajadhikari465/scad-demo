CREATE TABLE [dbo].[RuleDef] (
    [AppID]            INT           NOT NULL,
    [RuleID]           INT           NOT NULL,
    [RuleName]         VARCHAR (255) NOT NULL,
    [RuleDescTemplate] VARCHAR (255) NULL,
    CONSTRAINT [PK_RuleDef] PRIMARY KEY CLUSTERED ([AppID] ASC, [RuleID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ExType_App] FOREIGN KEY ([AppID]) REFERENCES [dbo].[App] ([AppID])
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[RuleDef] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[RuleDef] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[RuleDef] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[RuleDef] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[RuleDef] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[RuleDef] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[RuleDef] TO [IRMAAVCIRole]
    AS [dbo];

