CREATE TABLE [dbo].[UOM_Conversion] (
    [UOM_Abbreviation]     VARCHAR (5)  NOT NULL,
    [UOM_Conversion]       VARCHAR (25) NOT NULL,
    [UOM_ConversionFactor] FLOAT (53)   NULL,
    CONSTRAINT [PK_UOM_Conversion] PRIMARY KEY CLUSTERED ([UOM_Abbreviation] ASC, [UOM_Conversion] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UOM_Conversion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UOM_Conversion] TO [IRMAReportsRole]
    AS [dbo];

