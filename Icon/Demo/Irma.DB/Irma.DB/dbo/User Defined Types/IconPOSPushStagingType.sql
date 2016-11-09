CREATE TYPE [dbo].[IconPOSPushStagingType] AS TABLE (
    [PriceBatchHeaderID]      INT            NULL,
    [Store_No]                INT            NULL,
    [Item_Key]                INT            NULL,
    [Identifier]              VARCHAR (13)   NULL,
    [ChangeType]              VARCHAR (30)   NULL,
    [InsertDate]              DATETIME       NULL,
    [RetailSize]              DECIMAL (9, 4) NULL,
    [RetailUOM]               VARCHAR (5)    NULL,
    [TMDiscountEligible]      BIT            NULL,
    [Case_Discount]           BIT            NULL,
    [AgeCode]                 INT            NULL,
    [Recall_Flag]             BIT            NULL,
    [Restricted_Hours]        BIT            NULL,
    [Sold_By_Weight]          BIT            NULL,
    [ScaleForcedTare]         BIT            NULL,
    [Quantity_Required]       BIT            NULL,
    [Price_Required]          BIT            NULL,
    [QtyProhibit]             BIT            NULL,
    [VisualVerify]            BIT            NULL,
    [RestrictSale]            BIT            NULL,
    [Price]                   SMALLMONEY     NULL,
    [Multiple]                INT            NULL,
    [Sale_Multiple]           INT            NULL,
    [Sale_Price]              SMALLMONEY     NULL,
    [Sale_Start_Date]         SMALLDATETIME  NULL,
    [Sale_End_Date]           SMALLDATETIME  NULL,
    [LinkCode_ItemIdentifier] VARCHAR (13)   NULL,
    [POSTare]                 INT            NULL);


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IconPOSPushStagingType] TO [IRMAClientRole];


GO
GRANT EXECUTE
    ON TYPE::[dbo].[IconPOSPushStagingType] TO [IRSUser];

