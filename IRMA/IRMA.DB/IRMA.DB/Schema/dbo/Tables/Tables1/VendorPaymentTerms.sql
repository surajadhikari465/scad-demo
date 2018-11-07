CREATE TABLE [dbo].[VendorPaymentTerms] (
    [PaymentTermID] INT           IDENTITY (1, 1) NOT NULL,
    [Description]   VARCHAR (128) NOT NULL,
    [DateLoaded]    DATETIME      NOT NULL,
    [Default]       BIT           NOT NULL,
    CONSTRAINT [PK_PaymentTermID] PRIMARY KEY CLUSTERED ([PaymentTermID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[VendorPaymentTerms] TO [IRMASchedJobs]
    AS [dbo];

