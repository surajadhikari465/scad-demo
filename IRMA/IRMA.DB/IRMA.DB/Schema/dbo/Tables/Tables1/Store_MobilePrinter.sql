CREATE TABLE [dbo].[Store_MobilePrinter] (
    [Store_No]        INT NOT NULL,
    [MobilePrinterID] INT NOT NULL,
    CONSTRAINT [PK_Store_MobilePrinter] PRIMARY KEY CLUSTERED ([Store_No] ASC, [MobilePrinterID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Store_MobilePrinter_MobilePrinter] FOREIGN KEY ([MobilePrinterID]) REFERENCES [dbo].[MobilePrinter] ([MobilePrinterID]),
    CONSTRAINT [FK_Store_MobilePrinter_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store_MobilePrinter] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store_MobilePrinter] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Store_MobilePrinter] TO [IRMAReportsRole]
    AS [dbo];

