CREATE TABLE [dbo].[CreditReasons] (
    [CreditReason_ID] INT          NOT NULL,
    [CreditReason]    VARCHAR (25) NOT NULL,
    CONSTRAINT [PK_CreditReasons_CreditReason_ID] PRIMARY KEY CLUSTERED ([CreditReason_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[CreditReasons] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CreditReasons] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[CreditReasons] TO [IRMAReportsRole]
    AS [dbo];

