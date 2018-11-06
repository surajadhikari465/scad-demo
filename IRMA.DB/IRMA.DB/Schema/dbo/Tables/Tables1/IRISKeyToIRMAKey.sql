CREATE TABLE [dbo].[IRISKeyToIRMAKey] (
    [Item_Key]       INT          NOT NULL,
    [IRIS_Prod_Code] VARCHAR (13) NOT NULL,
    CONSTRAINT [PK_IKTI_Iris_Prod_Code] PRIMARY KEY CLUSTERED ([IRIS_Prod_Code] ASC),
    CONSTRAINT [FK_IRISKeyToIRMAKey_Item_Key] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[IRISKeyToIRMAKey] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IRISKeyToIRMAKey] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IRISKeyToIRMAKey] TO [IRMAReportsRole]
    AS [dbo];

