CREATE TYPE [dbo].[NoTagRuleThresholdType] AS TABLE (
    [RuleName]       NVARCHAR (64) NOT NULL,
    [ThresholdValue] INT           NOT NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[NoTagRuleThresholdType] TO [IRMAClientRole];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[NoTagRuleThresholdType] TO [IRSUser];

