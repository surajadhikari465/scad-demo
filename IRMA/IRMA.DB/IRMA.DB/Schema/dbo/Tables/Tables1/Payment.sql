CREATE TABLE [dbo].[Payment] (
    [Payment_Type]    SMALLINT     NOT NULL,
    [PaymentGroup_ID] SMALLINT     NOT NULL,
    [Description]     VARCHAR (50) NULL,
    [PosSystemId]     INT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Payment_Payment_Type] PRIMARY KEY CLUSTERED ([Payment_Type] ASC, [PosSystemId] ASC) ON [Warehouse],
    CONSTRAINT [FK_Payment_PaymentGroupId] FOREIGN KEY ([PaymentGroup_ID]) REFERENCES [dbo].[PaymentGroup] ([PaymentGroup_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Payment] TO [IRMAReportsRole]
    AS [dbo];

