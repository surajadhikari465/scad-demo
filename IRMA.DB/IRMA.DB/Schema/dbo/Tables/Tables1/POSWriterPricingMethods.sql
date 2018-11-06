CREATE TABLE [dbo].[POSWriterPricingMethods] (
    [POSFileWriterKey] INT     NULL,
    [PricingMethod_ID] TINYINT NULL,
    CONSTRAINT [FK_POSWriterPricingMethods_POSWriter] FOREIGN KEY ([POSFileWriterKey]) REFERENCES [dbo].[POSWriter] ([POSFileWriterKey]),
    CONSTRAINT [FK_POSWriterPricingMethods_PricingMethod] FOREIGN KEY ([PricingMethod_ID]) REFERENCES [dbo].[PricingMethod] ([PricingMethod_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterPricingMethods] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterPricingMethods] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[POSWriterPricingMethods] TO [IRMAReportsRole]
    AS [dbo];

