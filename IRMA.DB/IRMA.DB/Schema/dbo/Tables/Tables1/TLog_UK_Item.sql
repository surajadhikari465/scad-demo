CREATE TABLE [dbo].[TLog_UK_Item] (
    [ItemId]         INT            IDENTITY (1, 1) NOT NULL,
    [TimeKey]        SMALLDATETIME  NULL,
    [Transaction_No] INT            NULL,
    [Store_No]       INT            NULL,
    [Register_No]    INT            NULL,
    [Item_Key]       INT            NULL,
    [Food_Stamp]     INT            NULL,
    [Sales_Quantity] INT            NULL,
    [Sales_Amount]   MONEY          NULL,
    [Identifier]     VARCHAR (50)   NULL,
    [SubTeam_No]     INT            NULL,
    [Dept_No]        INT            NULL,
    [Vat_Code]       INT            NULL,
    [Turnover_Dept]  INT            NULL,
    [Row_No]         INT            NULL,
    [Weight]         DECIMAL (4, 3) NULL,
    [Retail_Price]   MONEY          NULL,
    [Trans_Type]     VARCHAR (10)   NULL,
    CONSTRAINT [PK_TLog_UK_Item] PRIMARY KEY CLUSTERED ([ItemId] ASC),
    CONSTRAINT [FK_TLog_UK_Item_TLog_UK_Transaction] FOREIGN KEY ([TimeKey], [Transaction_No], [Store_No], [Register_No]) REFERENCES [dbo].[TLog_UK_Transaction] ([TimeKey], [Transaction_No], [Store_No], [Register_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Item] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Item] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Item] TO [IRMAReportsRole]
    AS [dbo];

