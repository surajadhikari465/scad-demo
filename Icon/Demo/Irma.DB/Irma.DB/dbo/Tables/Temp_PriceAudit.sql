CREATE TABLE [dbo].[Temp_PriceAudit] (
    [BusinessUnit_ID]      INT             NULL,
    [CompanyName]          VARCHAR (50)    NULL,
    [SubTeam_No]           INT             NULL,
    [Identifier]           VARCHAR (13)    NULL,
    [Item_Description]     VARCHAR (60)    NULL,
    [CostUnitAmount]       DECIMAL (9, 4)  NULL,
    [CostUnit]             VARCHAR (25)    NULL,
    [RetailUnit]           VARCHAR (25)    NULL,
    [Cost]                 SMALLMONEY      NULL,
    [Price]                SMALLMONEY      NULL,
    [Margin]               DECIMAL (9, 4)  NULL,
    [Cycle_Cnt_Dist]       DECIMAL (18, 4) NULL,
    [Cycle_Cnt_Store]      DECIMAL (18, 4) NULL,
    [POS_Unit_Price]       SMALLMONEY      NULL,
    [POS_PricingMethod_ID] INT             NULL,
    [Sale_Price]           SMALLMONEY      NULL,
    [On_Sale]              BIT             NULL,
    [Multiple]             INT             NULL,
    [Sale_Multiple]        INT             NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Temp_PriceAudit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Temp_PriceAudit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Temp_PriceAudit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Temp_PriceAudit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Temp_PriceAudit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Temp_PriceAudit] TO [IRMAReportsRole]
    AS [dbo];

