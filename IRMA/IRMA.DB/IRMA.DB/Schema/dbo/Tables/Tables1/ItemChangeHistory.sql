CREATE TABLE [dbo].[ItemChangeHistory] (
    [Item_Key]                       INT            NULL,
    [Item_Description]               VARCHAR (60)   NULL,
    [Sign_Description]               VARCHAR (60)   NULL,
    [Ingredients]                    VARCHAR (3500) NULL,
    [SubTeam_No]                     INT            NULL,
    [Sales_Account]                  VARCHAR (6)    NULL,
    [Package_Desc1]                  DECIMAL (9, 4) NULL,
    [Package_Desc2]                  DECIMAL (9, 4) NULL,
    [Package_Unit_ID]                INT            NULL,
    [Min_Temperature]                SMALLINT       NULL,
    [Max_Temperature]                SMALLINT       NULL,
    [Units_Per_Pallet]               SMALLINT       NULL,
    [Average_Unit_Weight]            DECIMAL (9, 4) NULL,
    [Tie]                            TINYINT        NULL,
    [High]                           TINYINT        NULL,
    [Yield]                          DECIMAL (9, 4) NULL,
    [Brand_ID]                       INT            NULL,
    [Category_ID]                    INT            NULL,
    [Origin_ID]                      INT            NULL,
    [ShelfLife_Length]               SMALLINT       NULL,
    [ShelfLife_ID]                   INT            NULL,
    [Retail_Unit_ID]                 INT            NULL,
    [Vendor_Unit_ID]                 INT            NULL,
    [Distribution_Unit_ID]           INT            NULL,
    [Cost_Unit_ID]                   INT            NULL,
    [Freight_Unit_ID]                INT            NULL,
    [Discontinue_Item]               BIT            NULL,
    [WFM_Item]                       BIT            NULL,
    [Not_Available]                  BIT            NULL,
    [Pre_Order]                      BIT            NULL,
    [Remove_Item]                    TINYINT        NULL,
    [NoDistMarkup]                   BIT            NULL,
    [Organic]                        BIT            NULL,
    [Refrigerated]                   BIT            NULL,
    [Keep_Frozen]                    BIT            NULL,
    [Shipper_Item]                   BIT            NULL,
    [Full_Pallet_Only]               BIT            NULL,
    [POS_Description]                VARCHAR (26)   NULL,
    [Retail_Sale]                    BIT            NULL,
    [Food_Stamps]                    BIT            NULL,
    [Discountable]                   BIT            NULL,
    [Price_Required]                 BIT            NULL,
    [Quantity_Required]              BIT            NULL,
    [ItemType_ID]                    INT            NULL,
    [HFM_Item]                       BIT            NULL,
    [ScaleDesc1]                     VARCHAR (64)   NULL,
    [ScaleDesc2]                     VARCHAR (64)   NULL,
    [Not_AvailableNote]              VARCHAR (255)  NULL,
    [CountryProc_ID]                 INT            NULL,
    [Host_Name]                      VARCHAR (20)   CONSTRAINT [DF_ItemChangeHistory_Host_Name] DEFAULT (host_name()) NULL,
    [Effective_Date]                 DATETIME       CONSTRAINT [DF_ItemChangeHistory_Effective_Date] DEFAULT (getdate()) NOT NULL,
    [Manufacturing_Unit_ID]          INT            NULL,
    [EXEDistributed]                 BIT            CONSTRAINT [DF_ItemChangeHistory_EXEDistributed] DEFAULT ((0)) NOT NULL,
    [ClassID]                        INT            NULL,
    [DistSubTeam_No]                 INT            NULL,
    [CostedByWeight]                 BIT            NULL,
    [TaxClassID]                     INT            NULL,
    [Deleted_Item]                   BIT            CONSTRAINT [DF_ItemChangeHistory_DeletedItem] DEFAULT ((0)) NOT NULL,
    [User_ID]                        INT            NULL,
    [User_ID_Date]                   DATETIME       NULL,
    [LabelType_ID]                   INT            NULL,
    [Insert_Date]                    DATETIME       CONSTRAINT [DF_ItemChangeHistory_Insert_Date] DEFAULT (getdate()) NOT NULL,
    [QtyProhibit]                    BIT            NULL,
    [GroupList]                      INT            NULL,
    [Case_Discount]                  BIT            NULL,
    [Coupon_Multiplier]              BIT            NULL,
    [Misc_Transaction_Sale]          SMALLINT       NULL,
    [Misc_Transaction_Refund]        SMALLINT       NULL,
    [Recall_Flag]                    BIT            NULL,
    [Manager_ID]                     TINYINT        NULL,
    [Ice_Tare]                       INT            NULL,
    [PurchaseThresholdCouponAmount]  SMALLMONEY     NULL,
    [PurchaseThresholdCouponSubTeam] BIT            NULL,
    [Product_Code]                   VARCHAR (15)   NULL,
    [Unit_Price_Category]            INT            NULL,
    [StoreJurisdictionID]            INT            NULL,
    [CatchweightRequired]            BIT            CONSTRAINT [DF_ItemChangeHistory_CatchweightRequired] DEFAULT ((0)) NOT NULL,
    [SustainabilityRankingRequired]  BIT            NULL,
    [SustainabilityRankingID]        INT            NULL,
    [GiftCard]                       BIT            NULL,
    CONSTRAINT [FK__Item__Manager__0E898877] FOREIGN KEY ([Manager_ID]) REFERENCES [dbo].[ItemManager] ([Manager_ID]),
    CONSTRAINT [FK_ItemChangeHistory_TaxClass] FOREIGN KEY ([TaxClassID]) REFERENCES [dbo].[TaxClass] ([TaxClassID])
);


GO
CREATE NONCLUSTERED INDEX [ICHItemKeyInsertDateSubteamUserID]
    ON [dbo].[ItemChangeHistory]([Item_Key] ASC, [Insert_Date] ASC, [SubTeam_No] ASC, [User_ID] ASC)
    INCLUDE([Item_Description], [Sign_Description]) WITH (FILLFACTOR = 90);


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChangeHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChangeHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemChangeHistory] TO [IRMAReportsRole]
    AS [dbo];

