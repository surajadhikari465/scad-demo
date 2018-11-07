CREATE TABLE [dbo].[PaymentGroup] (
    [PaymentGroup_ID] SMALLINT     NOT NULL,
    [Description]     VARCHAR (50) NULL,
    CONSTRAINT [PK_PaymentGroup_PaymentGroup_ID] PRIMARY KEY NONCLUSTERED ([PaymentGroup_ID] ASC) WITH (FILLFACTOR = 80) ON [Warehouse]
) ON [Warehouse];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PaymentGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PaymentGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PaymentGroup] TO [IRMAReportsRole]
    AS [dbo];

