﻿CREATE TABLE [irma].[Item] (
    [Region]                         NCHAR (2)      NOT NULL,
    [Item_Key]                       INT            NOT NULL,
    [Item_Description]               VARCHAR (60)   NULL,
    [Sign_Description]               VARCHAR (60)   NULL,
    [Ingredients]                    VARCHAR (3500) NULL,
    [SubTeam_No]                     INT            NULL,
    [Sales_Account]                  VARCHAR (6)    NULL,
    [Package_Desc1]                  DECIMAL (9, 4) CONSTRAINT [DF__Item__Package_De__0CA1479E] DEFAULT ((0)) NULL,
    [Package_Desc2]                  DECIMAL (9, 4) CONSTRAINT [DF__Item__Package_De__0D956BD7] DEFAULT ((0)) NULL,
    [Package_Unit_ID]                INT            NULL,
    [Min_Temperature]                SMALLINT       CONSTRAINT [DF__Item__Min_Temper__0F7DB449] DEFAULT ((0)) NULL,
    [Max_Temperature]                SMALLINT       CONSTRAINT [DF__Item__Max_Temper__1071D882] DEFAULT ((0)) NULL,
    [Units_Per_Pallet]               SMALLINT       CONSTRAINT [DF__Item__Units_Per___1165FCBB] DEFAULT ((0)) NULL,
    [Average_Unit_Weight]            DECIMAL (9, 4) NULL,
    [Tie]                            TINYINT        CONSTRAINT [DF__Item__Tie__125A20F4] DEFAULT ((0)) NULL,
    [High]                           TINYINT        CONSTRAINT [DF__Item__High__134E452D] DEFAULT ((0)) NULL,
    [Yield]                          DECIMAL (9, 4) CONSTRAINT [DF__Item__Yield__14426966] DEFAULT ((100)) NULL,
    [Brand_ID]                       INT            NULL,
    [Category_ID]                    INT            NULL,
    [Origin_ID]                      INT            NULL,
    [ShelfLife_Length]               SMALLINT       CONSTRAINT [DF__Item__ShelfLife___19FB42BC] DEFAULT ((0)) NULL,
    [ShelfLife_ID]                   INT            NULL,
    [Retail_Unit_ID]                 INT            NULL,
    [Vendor_Unit_ID]                 INT            NULL,
    [Distribution_Unit_ID]           INT            NULL,
    [Cost_Unit_ID]                   INT            NULL,
    [Freight_Unit_ID]                INT            NULL,
    [Deleted_Item]                   BIT            CONSTRAINT [DF__Item__Deleted_It__219C6484] DEFAULT ((0)) NULL,
    [WFM_Item]                       BIT            CONSTRAINT [DF__Item__HIAH_Item__2478D12F] DEFAULT ((1)) NULL,
    [Not_Available]                  BIT            CONSTRAINT [DF__Item__Not_Availa__266119A1] DEFAULT ((0)) NULL,
    [Pre_Order]                      BIT            CONSTRAINT [DF__Item__Pre_Order__27553DDA] DEFAULT ((0)) NULL,
    [Remove_Item]                    TINYINT        CONSTRAINT [DF__Item__Remove_Ite__2B25CEBE] DEFAULT ((0)) NULL,
    [NoDistMarkup]                   BIT            CONSTRAINT [DF__Item__Average_Co__2FEA83DB] DEFAULT ((0)) NULL,
    [Organic]                        BIT            CONSTRAINT [DF__Item__Organic__31D2CC4D] DEFAULT ((0)) NULL,
    [Refrigerated]                   BIT            CONSTRAINT [DF__Item__Refrigerat__32C6F086] DEFAULT ((0)) NULL,
    [Keep_Frozen]                    BIT            CONSTRAINT [DF__Item__Keep_Froze__33BB14BF] DEFAULT ((0)) NULL,
    [Shipper_Item]                   BIT            CONSTRAINT [DF__Item__Shipper_It__35A35D31] DEFAULT ((0)) NULL,
    [Full_Pallet_Only]               BIT            CONSTRAINT [DF__Item__Full_Palle__3697816A] DEFAULT ((0)) NULL,
    [User_ID]                        INT            NULL,
    [POS_Description]                VARCHAR (26)   NULL,
    [Retail_Sale]                    BIT            CONSTRAINT [DF__Item__Retail_Sal__3973EE15] DEFAULT ((0)) NULL,
    [Food_Stamps]                    BIT            CONSTRAINT [DF__Item__Food_Stamp__3A68124E] DEFAULT ((0)) NULL,
    [Discountable]                   BIT            CONSTRAINT [DF__Item__Discountab__3B5C3687] DEFAULT ((0)) NULL,
    [Price_Required]                 BIT            CONSTRAINT [DF__Item__Price_Requ__41150FDD] DEFAULT ((0)) NULL,
    [Quantity_Required]              BIT            CONSTRAINT [DF__Item__Quantity_R__42093416] DEFAULT ((0)) NULL,
    [ItemType_ID]                    INT            CONSTRAINT [DF__Item__ItemType_I__43F17C88] DEFAULT ((0)) NULL,
    [HFM_Item]                       BIT            CONSTRAINT [DF__item__HFM_Item__32695FD8] DEFAULT ((1)) NULL,
    [ScaleDesc1]                     VARCHAR (64)   NULL,
    [ScaleDesc2]                     VARCHAR (64)   NULL,
    [Not_AvailableNote]              VARCHAR (255)  NULL,
    [CountryProc_ID]                 INT            NULL,
    [Insert_Date]                    DATETIME       CONSTRAINT [DF_Item_Insert_Date] DEFAULT (getdate()) NULL,
    [Manufacturing_Unit_ID]          INT            NULL,
    [EXEDistributed]                 BIT            CONSTRAINT [DF_Item_EXEDistributed] DEFAULT ((0)) NULL,
    [ClassID]                        INT            NULL,
    [User_ID_Date]                   DATETIME       NULL,
    [DistSubTeam_No]                 INT            NULL,
    [CostedByWeight]                 BIT            CONSTRAINT [DF_Item_CostedByWeight] DEFAULT ((0)) NULL,
    [TaxClassID]                     INT            NULL,
    [LabelType_ID]                   INT            NULL,
    [ScaleDesc3]                     VARCHAR (64)   NULL,
    [ScaleDesc4]                     VARCHAR (64)   NULL,
    [ScaleTare]                      INT            NULL,
    [ScaleUseBy]                     INT            NULL,
    [ScaleForcedTare]                CHAR (1)       NULL,
    [QtyProhibit]                    BIT            NULL,
    [GroupList]                      INT            NULL,
    [ProdHierarchyLevel4_ID]         INT            NULL,
    [Case_Discount]                  BIT            NULL,
    [Coupon_Multiplier]              BIT            NULL,
    [Misc_Transaction_Sale]          SMALLINT       NULL,
    [Misc_Transaction_Refund]        SMALLINT       NULL,
    [Recall_Flag]                    BIT            CONSTRAINT [DF_Item_Recall_Flag] DEFAULT ((0)) NULL,
    [Manager_ID]                     TINYINT        NULL,
    [Ice_Tare]                       INT            NULL,
    [LockAuth]                       BIT            CONSTRAINT [DF_Item_LockAuth] DEFAULT ((0)) NULL,
    [PurchaseThresholdCouponAmount]  SMALLMONEY     NULL,
    [PurchaseThresholdCouponSubTeam] BIT            NULL,
    [Product_Code]                   VARCHAR (15)   NULL,
    [Unit_Price_Category]            INT            NULL,
    [StoreJurisdictionID]            INT            NULL,
    [CatchweightRequired]            BIT            CONSTRAINT [DF_Item_CatchweightRequired] DEFAULT ((0)) NULL,
    [COOL]                           BIT            DEFAULT ((0)) NULL,
    [BIO]                            BIT            DEFAULT ((0)) NULL,
    [LastModifiedUser_ID]            INT            NULL,
    [LastModifiedDate]               DATETIME       NULL,
    [CatchWtReq]                     BIT            CONSTRAINT [DF_Item_CatchWtReq] DEFAULT ((0)) NULL,
    [SustainabilityRankingRequired]  BIT            DEFAULT ((0)) NULL,
    [SustainabilityRankingID]        INT            NULL,
    [Ingredient]                     BIT            DEFAULT ((0)) NULL,
    [FSA_Eligible]                   BIT            DEFAULT ((0)) NULL,
    [TaxClassModifiedDate]           DATETIME       NULL,
    [UseLastReceivedCost]            BIT            NULL,
    [GiftCard]                       BIT            CONSTRAINT [DF_Item_GiftCard] DEFAULT ((0)) NULL
);



