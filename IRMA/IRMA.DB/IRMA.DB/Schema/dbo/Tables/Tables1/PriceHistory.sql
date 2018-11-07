CREATE TABLE [dbo].[PriceHistory] (
    [Item_Key]                       INT           NULL,
    [Store_No]                       INT           NULL,
    [Multiple]                       TINYINT       NULL,
    [Price]                          SMALLMONEY    NULL,
    [MSRPPrice]                      SMALLMONEY    NULL,
    [MSRPMultiple]                   TINYINT       NULL,
    [PricingMethod_ID]               INT           NULL,
    [Sale_Multiple]                  TINYINT       NULL,
    [Sale_Price]                     SMALLMONEY    NULL,
    [Sale_Start_Date]                SMALLDATETIME NULL,
    [Sale_End_Date]                  SMALLDATETIME NULL,
    [Sale_Max_Quantity]              TINYINT       NULL,
    [Sale_Mix_Match]                 SMALLINT      NULL,
    [Sale_Earned_Disc1]              TINYINT       NULL,
    [Sale_Earned_Disc2]              TINYINT       NULL,
    [Sale_Earned_Disc3]              TINYINT       NULL,
    [User_Name]                      VARCHAR (20)  NULL,
    [Host_Name]                      VARCHAR (20)  NULL,
    [Effective_Date]                 DATETIME      CONSTRAINT [DF__PriceHist__Effec__72AC6B71] DEFAULT (getdate()) NOT NULL,
    [PriceHistoryID]                 INT           IDENTITY (1, 1) NOT NULL,
    [IBM_Discount]                   BIT           NULL,
    [Restricted_Hours]               BIT           NULL,
    [AvgCostUpdated]                 DATETIME      NULL,
    [POSPrice]                       SMALLMONEY    NULL,
    [POSSale_Price]                  SMALLMONEY    NULL,
    [NotAuthorizedForSale]           BIT           NULL,
    [CompFlag]                       BIT           CONSTRAINT [DF_PriceHistory_CompFlag] DEFAULT ((0)) NULL,
    [PosTare]                        INT           NULL,
    [LinkedItem]                     INT           NULL,
    [GrillPrint]                     BIT           NULL,
    [AgeCode]                        INT           NULL,
    [VisualVerify]                   BIT           NULL,
    [SrCitizenDiscount]              BIT           NULL,
    [PriceChgTypeId]                 TINYINT       NULL,
    [ExceptionSubteam_No]            INT           NULL,
    [POSLinkCode]                    VARCHAR (10)  NULL,
    [KitchenRoute_ID]                INT           NULL,
    [Routing_Priority]               TINYINT       NULL,
    [Consolidate_Price_To_Prev_Item] BIT           NULL,
    [Print_Condiment_On_Receipt]     BIT           NULL,
    [Age_Restrict]                   BIT           NULL,
    [CompetitivePriceTypeID]         INT           NULL,
    [BandwidthPercentageHigh]        TINYINT       NULL,
    [BandwidthPercentageLow]         TINYINT       NULL,
    [MixMatch]                       INT           NULL,
    [Discountable]                   BIT           NULL,
    [LocalItem]                      BIT           NULL,
    [ItemSurcharge]                  INT           NULL,
    CONSTRAINT [PK_PriceHistory] PRIMARY KEY CLUSTERED ([PriceHistoryID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__KitchenRoute__ID__0E899010] FOREIGN KEY ([KitchenRoute_ID]) REFERENCES [dbo].[KitchenRoute] ([KitchenRoute_ID]),
    CONSTRAINT [FK_ExceptionSubTeam_SubTeam_Hist] FOREIGN KEY ([ExceptionSubteam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
CREATE NONCLUSTERED INDEX [idxPriceHistoryKey]
    ON [dbo].[PriceHistory]([Item_Key] ASC, [Store_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxPriceHistoryDate]
    ON [dbo].[PriceHistory]([Effective_Date] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxPriceHistoryStore]
    ON [dbo].[PriceHistory]([Store_No] ASC, [Item_Key] ASC, [Effective_Date] ASC, [Multiple] ASC, [Price] ASC) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceHistory] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceHistory] TO [ExtractRole]
    AS [dbo];

