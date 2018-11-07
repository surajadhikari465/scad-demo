CREATE TABLE [dbo].[ExSeverityDef] (
    [AppID]       INT          NOT NULL,
    [RuleID]      INT          NOT NULL,
    [Severity]    TINYINT      NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ExSeverityDef] PRIMARY KEY CLUSTERED ([AppID] ASC, [RuleID] ASC, [Severity] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_ExSeverityDef_RuleDef] FOREIGN KEY ([AppID], [RuleID]) REFERENCES [dbo].[RuleDef] ([AppID], [RuleID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExSeverityDef] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExSeverityDef] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ExSeverityDef] TO [IRMAReportsRole]
    AS [dbo];

