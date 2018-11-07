CREATE TABLE [dbo].[IConPOSPushPublish] (
    [IConPOSPushPublishID]    INT            IDENTITY (1, 1) NOT NULL,
    [PriceBatchHeaderID]      INT            NULL,
    [RegionCode]              VARCHAR (4)    NOT NULL,
    [Store_No]                INT            NOT NULL,
    [Item_Key]                INT            NOT NULL,
    [Identifier]              VARCHAR (13)   NOT NULL,
    [ChangeType]              VARCHAR (30)   NOT NULL,
    [InsertDate]              DATETIME       CONSTRAINT [DF_IConPOSPushPublish_InsertDate] DEFAULT (getdate()) NOT NULL,
    [BusinessUnit_ID]         INT            NOT NULL,
    [RetailSize]              DECIMAL (9, 4) NULL,
    [RetailPackageUom]        VARCHAR (5)    NULL,
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
    [RetailUom]               VARCHAR (5)    NULL,
    [Sale_Price]              MONEY          NULL,
    [Multiple]                INT            NULL,
    [SaleMultiple]            INT            NULL,
    [Sale_Start_Date]         SMALLDATETIME  NULL,
    [Sale_End_Date]           SMALLDATETIME  NULL,
    [InProcessBy]             INT            NULL,
    [ProcessedDate]           DATETIME2 (7)  NULL,
    [ProcessingFailedDate]    DATETIME2 (7)  NULL,
    [LinkCode_ItemIdentifier] VARCHAR (13)   NULL,
    [POSTare]                 INT            NULL,
    CONSTRAINT [PK_IConPOSPushPublish_IConPOSPushPublishID] PRIMARY KEY CLUSTERED ([IConPOSPushPublishID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_IConPOSPushPublish_Store_No] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [UQ_IConPOSPushPublish_keys] UNIQUE NONCLUSTERED ([Store_No] ASC, [Item_Key] ASC, [Identifier] ASC, [ChangeType] ASC, [InsertDate] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IX_IConPOSPushPublish_ConcurrencyColumns]
    ON [dbo].[IConPOSPushPublish]([InProcessBy] ASC, [ProcessedDate] ASC, [ProcessingFailedDate] ASC)
    INCLUDE([IConPOSPushPublishID]) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRSUser]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRSUser]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRSUser]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IRSUser]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IConInterface]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IConInterface]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[IConPOSPushPublish] TO [IConInterface]
    AS [dbo];

