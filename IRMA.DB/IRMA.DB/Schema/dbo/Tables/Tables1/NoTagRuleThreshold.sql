CREATE TABLE [dbo].[NoTagRuleThreshold] (
    [RuleName]           NVARCHAR (64) NOT NULL,
    [ThresholdValueDays] INT           NOT NULL,
    CONSTRAINT [PK_NoTagRuleThreshold] PRIMARY KEY CLUSTERED ([RuleName] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NoTagRuleThreshold] TO [IRSUser]
    AS [dbo];

