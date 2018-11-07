CREATE TABLE [dbo].[MobilePrinter] (
    [MobilePrinterID] INT          IDENTITY (1, 1) NOT NULL,
    [NetworkName]     VARCHAR (17) NOT NULL,
    CONSTRAINT [PK_MobilePrinter] PRIMARY KEY CLUSTERED ([MobilePrinterID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[MobilePrinter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[MobilePrinter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[MobilePrinter] TO [IRMAReportsRole]
    AS [dbo];

