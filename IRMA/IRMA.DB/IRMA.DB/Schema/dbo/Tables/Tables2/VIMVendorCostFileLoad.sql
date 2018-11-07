CREATE TABLE [dbo].[VIMVendorCostFileLoad] (
    [UPC]              VARCHAR (26)   NULL,
    [REGION]           VARCHAR (2)    NULL,
    [PS_BU]            INT            NULL,
    [VEND_ITEM_NUM]    VARCHAR (24)   NULL,
    [REG_VEND_NUM_CZ]  INT            NULL,
    [REG_COST]         SMALLMONEY     NULL,
    [EFF_COST]         SMALLMONEY     NULL,
    [COST_ADJUSTMENTS] INT            NULL,
    [CASE_SIZE]        DECIMAL (9, 4) NULL,
    [ITEM_UOM]         VARCHAR (25)   NULL,
    [REG_COST_DATE]    VARCHAR (10)   NULL
);

