CREATE TABLE [dbo].[IConPOSPushStaging] (
    [PriceBatchHeaderID]      INT            NULL,
    [Store_No]                INT            NOT NULL,
    [Item_Key]                INT            NOT NULL,
    [Identifier]              VARCHAR (13)   NOT NULL,
    [ChangeType]              VARCHAR (30)   NOT NULL,
    [InsertDate]              DATETIME       CONSTRAINT [DF_IConPOSPushStaging_InsertDate] DEFAULT (getdate()) NOT NULL,
    [RetailSize]              DECIMAL (9, 4) NULL,
    [RetailUom]               VARCHAR (5)    NULL,
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
    [Price]                   MONEY          NULL,
    [Sale_Price]              MONEY          NULL,
    [Multiple]                INT            NULL,
    [SaleMultiple]            INT            NULL,
    [Sale_Start_Date]         SMALLDATETIME  NULL,
    [Sale_End_Date]           SMALLDATETIME  NULL,
    [LinkCode_ItemIdentifier] VARCHAR (13)   NULL,
    [POSTare]                 INT            NULL,
    [PriceBatchDetailID]      INT            NULL
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IConPOSPushStaging] TO [IRSUser]
    AS [dbo];

