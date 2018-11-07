CREATE TABLE [dbo].[TLog_UK_Offers] (
    [OfferId]           INT           IDENTITY (1, 1) NOT NULL,
    [TimeKey]           SMALLDATETIME NOT NULL,
    [Transaction_No]    INT           NOT NULL,
    [Store_No]          INT           NOT NULL,
    [Register_No]       INT           NOT NULL,
    [Barcode]           VARCHAR (13)  NULL,
    [Offer_Quantity]    INT           NULL,
    [Offer_Amount]      MONEY         NULL,
    [Table_Number]      INT           NULL,
    [Offer_Description] VARCHAR (20)  NULL,
    [Offer_Reference]   VARCHAR (12)  NULL,
    CONSTRAINT [PK_TLog_UK_Offers] PRIMARY KEY CLUSTERED ([OfferId] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Offers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Offers] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TLog_UK_Offers] TO [IRMAReportsRole]
    AS [dbo];

