CREATE TABLE [dbo].[PricingMethod] (
    [PricingMethod_ID]          TINYINT      NOT NULL,
    [PricingMethod_Name]        VARCHAR (50) NOT NULL,
    [EnableOfferEditor]         BIT          CONSTRAINT [DF_PricingMethod_EnableOfferEditor] DEFAULT ((1)) NOT NULL,
    [EnablePromoScreen]         BIT          NULL,
    [EnableSaleMultiple]        BIT          NULL,
    [EnableEarnedRegMultiple]   BIT          NULL,
    [EarnedRegMultipleDefault]  INT          NULL,
    [EnableEarnedSaleMultiple]  BIT          NULL,
    [EarnedSaleMultipleDefault] INT          NULL,
    [EnableEarnedLimit]         BIT          NULL,
    [EarnedLimitDefault]        INT          NULL,
    [POS_Code]                  VARCHAR (50) NULL,
    [UseRegPrice]               BIT          NULL,
    [UseSalePrice]              BIT          NULL,
    CONSTRAINT [PK_PricingMethod_PricingMethod_ID] PRIMARY KEY CLUSTERED ([PricingMethod_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PricingMethod] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PricingMethod] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PricingMethod] TO [IRMAReportsRole]
    AS [dbo];

