CREATE TABLE [dbo].[POSWriterPricingMethodId] (
    [POSFileWriterKey]  INT NOT NULL,
    [PricingMethod_Key] INT NOT NULL,
    [PricingMethod_ID]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([POSFileWriterKey] ASC, [PricingMethod_Key] ASC)
);

