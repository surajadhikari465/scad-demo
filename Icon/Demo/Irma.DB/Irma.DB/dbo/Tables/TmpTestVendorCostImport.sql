CREATE TABLE [dbo].[TmpTestVendorCostImport] (
    [Vendor_ID]          INT          NULL,
    [Item_ID]            NUMERIC (18) NULL,
    [Item_Key]           NUMERIC (10) NULL,
    [UPC]                NUMERIC (18) NULL,
    [Item_Description]   VARCHAR (60) NULL,
    [MSRP]               MONEY        NULL,
    [Case_Size]          INT          NULL,
    [Case_Price]         MONEY        NULL,
    [Case_Freight]       MONEY        NULL,
    [Status_Flag]        CHAR (1)     NULL,
    [Start_Date]         DATETIME     NULL,
    [End_Date]           DATETIME     NULL,
    [Deal_Case_Quantity] VARCHAR (50) NULL,
    [Deal_Case_Amt]      VARCHAR (50) NULL,
    [Deal_Start_Date]    DATETIME     NULL,
    [Deal_End_Date]      DATETIME     NULL,
    [Deal_Type]          CHAR (1)     NULL,
    [Filler]             CHAR (10)    NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[TmpTestVendorCostImport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TmpTestVendorCostImport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TmpTestVendorCostImport] TO [IRMAReportsRole]
    AS [dbo];

