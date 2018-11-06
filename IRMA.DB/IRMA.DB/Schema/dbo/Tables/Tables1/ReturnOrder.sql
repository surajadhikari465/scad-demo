CREATE TABLE [dbo].[ReturnOrder] (
    [Instance]               INT           NOT NULL,
    [Item_Key]               INT           NOT NULL,
    [OrderItem_ID]           INT           NOT NULL,
    [User_ID]                INT           NOT NULL,
    [Identifier]             NVARCHAR (20) NULL,
    [Item_Description]       NVARCHAR (60) NULL,
    [Units_Per_Pallet]       INT           NULL,
    [Quantity]               MONEY         NULL,
    [Quantity_Previous]      MONEY         NULL,
    [QuantityUnit_Text]      NVARCHAR (25) NULL,
    [QuantityUnit]           INT           NULL,
    [Package_Desc1]          MONEY         NULL,
    [Package_Desc2]          MONEY         NULL,
    [Package_Unit_ID]        INT           NULL,
    [LandedCost]             MONEY         NULL,
    [MarkupPercent]          MONEY         NULL,
    [MarkupCost]             MONEY         NULL,
    [Cost]                   MONEY         NULL,
    [CostUnit]               INT           NULL,
    [Freight]                MONEY         NULL,
    [FreightUnit]            INT           NULL,
    [QuantityDiscount]       MONEY         NULL,
    [DiscountType]           INT           NULL,
    [Total_Weight]           MONEY         NULL,
    [Total_Weight_Previous]  MONEY         NULL,
    [Retail_Unit_ID]         INT           NULL,
    [CreditReason]           NVARCHAR (25) NULL,
    [CreditReason_ID]        INT           NULL,
    [Total_ReceivedItemCost] MONEY         NULL,
    [NetVendorItemDiscount]  MONEY         NULL,
    [HandlingCharge]         SMALLMONEY    NULL,
    CONSTRAINT [PK_ReturnOrder] PRIMARY KEY CLUSTERED ([Instance] ASC, [Item_Key] ASC, [OrderItem_ID] ASC, [User_ID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ReturnOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ReturnOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReturnOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ReturnOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ReturnOrder] TO [IRMAReportsRole]
    AS [dbo];

