CREATE TABLE [dbo].[SignQueue] (
    [Item_Key]                       INT            NOT NULL,
    [Store_No]                       INT            NOT NULL,
    [Sign_Description]               VARCHAR (60)   NOT NULL,
    [Ingredients]                    VARCHAR (3500) NULL,
    [Identifier]                     VARCHAR (13)   NOT NULL,
    [Sold_By_Weight]                 BIT            NOT NULL,
    [Multiple]                       TINYINT        NOT NULL,
    [Price]                          SMALLMONEY     NOT NULL,
    [MSRPMultiple]                   TINYINT        NOT NULL,
    [MSRPPrice]                      SMALLMONEY     NOT NULL,
    [Case_Price]                     MONEY          NOT NULL,
    [Sale_Multiple]                  TINYINT        NOT NULL,
    [Sale_Price]                     SMALLMONEY     NOT NULL,
    [Sale_Start_Date]                SMALLDATETIME  NULL,
    [Sale_End_Date]                  SMALLDATETIME  NULL,
    [Sale_Earned_Disc1]              TINYINT        NOT NULL,
    [Sale_Earned_Disc2]              TINYINT        NOT NULL,
    [Sale_Earned_Disc3]              TINYINT        NOT NULL,
    [PricingMethod_ID]               INT            NULL,
    [SubTeam_No]                     INT            NULL,
    [Origin_Name]                    VARCHAR (25)   NULL,
    [Brand_Name]                     VARCHAR (25)   NULL,
    [Retail_Unit_Abbr]               VARCHAR (5)    NULL,
    [Retail_Unit_Full]               VARCHAR (25)   NULL,
    [Package_Unit]                   VARCHAR (5)    NULL,
    [Package_Desc1]                  DECIMAL (9, 4) NOT NULL,
    [Package_Desc2]                  DECIMAL (9, 4) NOT NULL,
    [Sign_Printed]                   TINYINT        CONSTRAINT [DF__SignQueue__Sign___01EEAF01] DEFAULT ((0)) NOT NULL,
    [Organic]                        BIT            NOT NULL,
    [Vendor_Id]                      INT            NULL,
    [User_ID]                        INT            NOT NULL,
    [User_ID_Date]                   DATETIME       NOT NULL,
    [ItemType_ID]                    INT            NOT NULL,
    [ScaleDesc1]                     VARCHAR (64)   NULL,
    [ScaleDesc2]                     VARCHAR (64)   NULL,
    [POS_Description]                VARCHAR (26)   NULL,
    [Restricted_Hours]               BIT            NOT NULL,
    [Quantity_Required]              BIT            NOT NULL,
    [Price_Required]                 BIT            NOT NULL,
    [Retail_Sale]                    BIT            NOT NULL,
    [Discountable]                   BIT            NOT NULL,
    [Food_Stamps]                    BIT            NOT NULL,
    [IBM_Discount]                   BIT            NOT NULL,
    [New_Item]                       BIT            CONSTRAINT [DF__SignQueue__New_I__33AD2C35] DEFAULT ((0)) NOT NULL,
    [Price_Change]                   BIT            CONSTRAINT [DF__SignQueue__Price__34A1506E] DEFAULT ((0)) NOT NULL,
    [Item_Change]                    BIT            CONSTRAINT [DF__SignQueue__Item___359574A7] DEFAULT ((0)) NOT NULL,
    [LastQueuedType]                 TINYINT        NOT NULL,
    [POSPrice]                       SMALLMONEY     NULL,
    [POSSale_Price]                  SMALLMONEY     NULL,
    [PriceChgTypeId]                 TINYINT        NULL,
    [TagTypeID]                      INT            NULL,
    [TagTypeID2]                     INT            NULL,
    [Retail_Unit_ID]                 INT            NULL,
    [Package_Unit_ID]                INT            NULL,
    [Item_Description]               VARCHAR (60)   NULL,
    [Case_Discount]                  BIT            NULL,
    [Coupon_Multiplier]              BIT            NULL,
    [Misc_Transaction_Sale]          SMALLINT       NULL,
    [Misc_Transaction_Refund]        SMALLINT       NULL,
    [Recall_Flag]                    BIT            NULL,
    [Ice_Tare]                       INT            NULL,
    [Product_Code]                   VARCHAR (15)   NULL,
    [Unit_Price_Category]            INT            NULL,
    [NotAuthorizedForSale]           BIT            NULL,
    [KitchenRoute_ID]                INT            NULL,
    [Routing_Priority]               TINYINT        NULL,
    [Consolidate_Price_To_Prev_Item] BIT            NULL,
    [Print_Condiment_On_Receipt]     BIT            NULL,
    [Age_Restrict]                   BIT            NULL,
    [LocalItem]                      BIT            NULL,
    [ItemSurcharge]                  INT            NULL,
    CONSTRAINT [PK_SignQueue] PRIMARY KEY CLUSTERED ([Item_Key] ASC, [Store_No] ASC),
    CONSTRAINT [FK__SignQueue__Item___7588D81C] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK__SignQueue__Store__767CFC55] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No])
);


GO
CREATE NONCLUSTERED INDEX [IX_SignQueue_Store_No]
    ON [dbo].[SignQueue]([Store_No] ASC)
    INCLUDE([Sign_Description], [SubTeam_No], [Identifier], [Brand_Name], [Multiple], [Sale_Multiple], [Price], [Sale_Price], [PriceChgTypeId], [Sign_Printed], [LastQueuedType]) WITH (FILLFACTOR = 80);


GO
GRANT SELECT
    ON OBJECT::[dbo].[SignQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SignQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SignQueue] TO [IRMAReportsRole]
    AS [dbo];

