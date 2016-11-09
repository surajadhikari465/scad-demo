﻿CREATE TABLE [dbo].[PriceBatchDenorm] (
    [PriceBatchDenormID]                                                    INT            IDENTITY (1, 1) NOT NULL,
    [InsertDate]                                                            DATETIME       DEFAULT (getdate()) NOT NULL,
    [Item_Key]                                                              INT            NULL,
    [Identifier]                                                            VARCHAR (13)   NULL,
    [IdentifierWithCheckDigit]                                              VARCHAR (14)   NULL,
    [RBX_IdentifierWithCheckDigit]                                          VARCHAR (14)   NULL,
    [PriceBatchHeaderID]                                                    INT            NULL,
    [RetailUnit_WeightUnit]                                                 BIT            NULL,
    [Sold_By_Weight]                                                        BIT            NULL,
    [PIRUS_Sold_By_Weight]                                                  CHAR (1)       NULL,
    [Restricted_Hours]                                                      BIT            NULL,
    [LocalItem]                                                             BIT            NULL,
    [Quantity_Required]                                                     BIT            NULL,
    [Price_Required]                                                        BIT            NULL,
    [Retail_Sale]                                                           BIT            NULL,
    [NotRetail_Sale]                                                        BIT            NULL,
    [Discountable]                                                          BIT            NULL,
    [NotDiscountable]                                                       BIT            NULL,
    [OHIO_Emp_Discount]                                                     BIT            NULL,
    [Food_Stamps]                                                           BIT            NULL,
    [ItemType_ID]                                                           INT            NULL,
    [PIRUS_ItemTypeID]                                                      CHAR (1)       NULL,
    [SubTeam_No]                                                            INT            NULL,
    [Store_No]                                                              INT            NULL,
    [IBM_Discount]                                                          BIT            NULL,
    [MixMatch]                                                              INT            NULL,
    [On_Sale]                                                               BIT            NULL,
    [Case_Price]                                                            MONEY          NULL,
    [POSCase_Price]                                                         MONEY          NULL,
    [Sale_Earned_Disc1]                                                     TINYINT        NULL,
    [Sale_Earned_Disc2]                                                     TINYINT        NULL,
    [Sale_Earned_Disc3]                                                     TINYINT        NULL,
    [MSRPMultiple]                                                          TINYINT        NULL,
    [MSRPPrice]                                                             MONEY          NULL,
    [Item_Desc]                                                             VARCHAR (18)   NULL,
    [POS_Description]                                                       VARCHAR (26)   NULL,
    [Item_Description]                                                      VARCHAR (60)   NULL,
    [ScaleDesc1]                                                            VARCHAR (64)   NULL,
    [ScaleDesc2]                                                            VARCHAR (64)   NULL,
    [ScaleDesc3]                                                            VARCHAR (64)   NULL,
    [ScaleDesc4]                                                            VARCHAR (64)   NULL,
    [Ingredients]                                                           VARCHAR (4200) NULL,
    [IngredientNumber]                                                      VARCHAR (5)    NULL,
    [Retail_Unit_Abbr]                                                      VARCHAR (5)    NULL,
    [UnitOfMeasure]                                                         VARCHAR (5)    NULL,
    [ScaleUnitOfMeasure]                                                    VARCHAR (5)    NULL,
    [PlumUnitAbbr]                                                          VARCHAR (5)    NULL,
    [ScaleTare_Int]                                                         INT            NULL,
    [AltScaleTare_Int]                                                      INT            NULL,
    [PLUMStoreScaleTareZone1]                                               INT            NULL,
    [PLUMStoreScaleTareZone2]                                               INT            NULL,
    [PLUMStoreScaleTareZone3]                                               INT            NULL,
    [PLUMStoreScaleTareZone4]                                               INT            NULL,
    [PLUMStoreScaleTareZone5]                                               INT            NULL,
    [PLUMStoreScaleTareZone6]                                               INT            NULL,
    [PLUMStoreScaleTareZone7]                                               INT            NULL,
    [PLUMStoreScaleTareZone8]                                               INT            NULL,
    [PLUMStoreScaleTareZone9]                                               INT            NULL,
    [PLUMStoreScaleTareZone10]                                              INT            NULL,
    [PLUMStoreALTScaleTareZone1]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone2]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone3]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone4]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone5]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone6]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone7]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone8]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone9]                                            INT            NULL,
    [PLUMStoreALTScaleTareZone10]                                           INT            NULL,
    [UseBy_ID]                                                              INT            NULL,
    [ScaleForcedTare]                                                       CHAR (1)       NULL,
    [ShelfLife_Length]                                                      SMALLINT       NULL,
    [Scale_FixedWeight]                                                     VARCHAR (25)   NULL,
    [Scale_ByCount]                                                         INT            NULL,
    [Grade]                                                                 SMALLINT       NULL,
    [Package_Desc1]                                                         DECIMAL (9, 4) NULL,
    [Package_Desc2]                                                         DECIMAL (9, 4) NULL,
    [Package_Unit_Abbr]                                                     VARCHAR (5)    NULL,
    [PackSize]                                                              VARCHAR (30)   NULL,
    [New_Item]                                                              TINYINT        NULL,
    [Price_Change]                                                          TINYINT        NULL,
    [Item_Change]                                                           TINYINT        NULL,
    [Remove_Item]                                                           TINYINT        NULL,
    [IsScaleItem]                                                           TINYINT        NULL,
    [Multiple]                                                              TINYINT        NULL,
    [Sale_Multiple]                                                         TINYINT        NULL,
    [CurrMultiple]                                                          TINYINT        NULL,
    [Price]                                                                 MONEY          NULL,
    [Sale_Price]                                                            MONEY          NULL,
    [Sale_End_Date]                                                         SMALLDATETIME  NULL,
    [RBX_Sale_End_Date]                                                     VARCHAR (10)   NULL,
    [CurrPrice]                                                             MONEY          NULL,
    [POSPrice]                                                              MONEY          NULL,
    [POSSale_Price]                                                         MONEY          NULL,
    [POSCurrPrice]                                                          MONEY          NULL,
    [MultipleWithPOSPrice]                                                  VARCHAR (15)   NULL,
    [SaleMultipleWithPOSSalePrice]                                          VARCHAR (15)   NULL,
    [PricingMethod_ID]                                                      INT            NULL,
    [Sale_Start_Date]                                                       SMALLDATETIME  NULL,
    [RBX_Sale_Start_Date]                                                   VARCHAR (10)   NULL,
    [Category_ID]                                                           INT            NULL,
    [UnitCost]                                                              MONEY          NULL,
    [RBX_UnitCost]                                                          MONEY          NULL,
    [Target_Margin]                                                         DECIMAL (9, 4) NULL,
    [Vendor_Key]                                                            VARCHAR (10)   NULL,
    [Vendor_Item_ID]                                                        VARCHAR (20)   NULL,
    [RBX_Vendor_Item_ID]                                                    VARCHAR (20)   NULL,
    [Compulsory_Price_Input]                                                CHAR (1)       NULL,
    [Calculated_Cost_Item]                                                  CHAR (1)       NULL,
    [Availability_Flag]                                                     CHAR (1)       NULL,
    [PIRUS_StartDate]                                                       INT            NULL,
    [PIRUS_InsertDate]                                                      INT            NULL,
    [PIRUS_CurrentDate]                                                     INT            NULL,
    [PIRUS_DeleteDate]                                                      INT            NULL,
    [Barcode_Type]                                                          CHAR (1)       NULL,
    [LabelTypeDesc]                                                         VARCHAR (30)   NULL,
    [NotAuthorizedForSale]                                                  CHAR (1)       NULL,
    [NCR_RestrictedCode]                                                    VARCHAR (2)    NULL,
    [NCR_NENA_RestrictedCode]                                               VARCHAR (2)    NULL,
    [PIRUS_OnSale]                                                          CHAR (1)       NULL,
    [PIRUS_SaleEndDate]                                                     INT            NULL,
    [PIRUS_HeaderAction]                                                    VARCHAR (2)    NULL,
    [Dept_No]                                                               INT            NULL,
    [IBM_Dept_No]                                                           INT            NULL,
    [IBM_Dept_No_3Chrs]                                                     INT            NULL,
    [Brand_Name]                                                            VARCHAR (25)   NULL,
    [RBX_PriceType]                                                         VARCHAR (20)   NULL,
    [CaseSize]                                                              DECIMAL (9, 4) NULL,
    [CaseCost]                                                              MONEY          NULL,
    [ChangeDate]                                                            SMALLDATETIME  NULL,
    [RBX_ChangeDate]                                                        VARCHAR (10)   NULL,
    [RBX_Coupon]                                                            CHAR (1)       NULL,
    [FX_DepositItem]                                                        CHAR (1)       NULL,
    [FX_RefundItem]                                                         CHAR (1)       NULL,
    [FX_DepositReturn]                                                      CHAR (1)       NULL,
    [FX_MfgCoupon]                                                          CHAR (1)       NULL,
    [FX_StoreCoupon]                                                        CHAR (1)       NULL,
    [FX_MiscSale]                                                           CHAR (1)       NULL,
    [FX_MiscRefund]                                                         CHAR (1)       NULL,
    [FX_Retalix_NegativeItem]                                               CHAR (1)       NULL,
    [FX_NCR_NegativeItem]                                                   CHAR (1)       NULL,
    [QtyProhibit]                                                           CHAR (1)       NULL,
    [QtyProhibit_Boolean]                                                   BIT            NULL,
    [GrillPrint]                                                            CHAR (1)       NULL,
    [SrCitizenDiscount]                                                     CHAR (1)       NULL,
    [VisualVerify]                                                          CHAR (1)       NULL,
    [GroupList]                                                             INT            NULL,
    [PosTare]                                                               INT            NULL,
    [LinkCode_ItemIdentifier]                                               VARCHAR (13)   NULL,
    [LinkCode_ItemIdentifier_MA]                                            VARCHAR (4)    NULL,
    [LinkCode_Value]                                                        VARCHAR (10)   NULL,
    [AgeCode]                                                               INT            NULL,
    [ScaleDept]                                                             INT            NULL,
    [ScalePLU]                                                              VARCHAR (5)    NULL,
    [ScaleUPC]                                                              VARCHAR (13)   NULL,
    [PLUMStoreNo]                                                           INT            NULL,
    [PLUM_ItemStatus]                                                       CHAR (1)       NULL,
    [IBM_NoPrice_NotScaleItem]                                              SMALLINT       NULL,
    [IBM_Offset09_Length1]                                                  VARCHAR (5)    NULL,
    [IBM_Offset09_Length1_MA]                                               VARCHAR (5)    NULL,
    [IBM_Offset15_Length1]                                                  SMALLINT       NULL,
    [IBM_Offset15_Length1_MA]                                               SMALLINT       NULL,
    [IBM_Offset16_Length1]                                                  SMALLINT       NULL,
    [IBM_Offset16_Length1_MA]                                               SMALLINT       NULL,
    [IBM_Offset17_Length5]                                                  VARCHAR (10)   NULL,
    [IBM_Offset17_Length5_MA]                                               VARCHAR (10)   NULL,
    [Case_Discount]                                                         BIT            NULL,
    [Coupon_Multiplier]                                                     BIT            NULL,
    [FSA_Eligible]                                                          BIT            NULL,
    [Misc_Transaction_Sale]                                                 SMALLINT       NULL,
    [Misc_Transaction_Refund]                                               SMALLINT       NULL,
    [MiscTransactionSaleAndRefund]                                          VARCHAR (20)   NULL,
    [MA_CasePrice]                                                          MONEY          NULL,
    [Recall_Flag]                                                           BIT            NULL,
    [Age_Restrict]                                                          BIT            NULL,
    [Routing_Priority]                                                      SMALLINT       NULL,
    [Consolidate_Price_To_Prev_Item]                                        CHAR (1)       NULL,
    [Print_Condiment_On_Receipt]                                            CHAR (1)       NULL,
    [JDA_Dept]                                                              INT            NULL,
    [KitchenRouteValue]                                                     VARCHAR (50)   NULL,
    [SavingsAmount]                                                         MONEY          NULL,
    [PurchaseThresholdCouponAmount]                                         MONEY          NULL,
    [PurchaseThresholdCouponAmount_ReversedHex]                             VARCHAR (10)   NULL,
    [PurchaseThresholdCouponSubTeam]                                        BIT            NULL,
    [SmartX_DeletePendingName]                                              CHAR (16)      NULL,
    [SmartX_MaintenanceDateTime]                                            CHAR (16)      NULL,
    [SmartX_EffectiveDate]                                                  CHAR (8)       NULL,
    [StoreItemAuthorizationID]                                              INT            NULL,
    [Product_Code]                                                          VARCHAR (15)   NULL,
    [Unit_Price_Category]                                                   INT            NULL,
    [POSPrice_AsHex]                                                        VARCHAR (10)   NULL,
    [PurchaseThresholdCouponAmountReversedHex_GrillPrint_FileWriterElement] VARCHAR (10)   NULL,
    [RBX_Promo]                                                             BIT            NULL,
    [RBX_BasePlusOne]                                                       BIT            NULL,
    [RBX_GroupThreshold]                                                    BIT            NULL,
    [RBX_GroupAdjusted]                                                     BIT            NULL,
    [RBX_UnitAdjusted]                                                      BIT            NULL,
    [RBX_GroupThresholdPrice]                                               VARCHAR (15)   NULL,
    [RBX_GroupAdjustedPrice]                                                VARCHAR (15)   NULL,
    [RBX_UnitAdjustedPrice]                                                 VARCHAR (15)   NULL,
    [Sign_Description]                                                      VARCHAR (60)   NULL,
    [ItemSurcharge]                                                         INT            NULL,
    [ItemSurcharge_AsHex]                                                   VARCHAR (10)   NULL,
    [Digi_LNU]                                                              VARCHAR (20)   NULL,
    [ApplyDate]                                                             SMALLDATETIME  NULL,
    [Nutrifact_ID]                                                          INT            NULL,
    [GiftCard]                                                              BIT            NULL,
    [BusinessUnit_ID]                                                       INT            NULL,
    [Organic]                                                               BIT            NULL,
    [ClassID]                                                               INT            NULL,
    [PriceChgTypeDesc]                                                      VARCHAR (3)    NULL,
    [ECommerce]                                                             BIT            NULL,
    [TaxClassID]                                                            INT            NULL,
    [DiscontinueItem]                                                       BIT            NULL,
    [Check_Box_1]                                                           BIT            NULL,
    [Check_Box_2]                                                           BIT            NULL,
    [Check_Box_3]                                                           BIT            NULL,
    [Check_Box_4]                                                           BIT            NULL,
    [Check_Box_5]                                                           BIT            NULL,
    [Check_Box_6]                                                           BIT            NULL,
    [Check_Box_7]                                                           BIT            NULL,
    [Check_Box_8]                                                           BIT            NULL,
    [Check_Box_9]                                                           BIT            NULL,
    [Check_Box_10]                                                          BIT            NULL,
    [Check_Box_11]                                                          BIT            NULL,
    [Check_Box_12]                                                          BIT            NULL,
    [Check_Box_13]                                                          BIT            NULL,
    [Check_Box_14]                                                          BIT            NULL,
    [Check_Box_15]                                                          BIT            NULL,
    [Check_Box_16]                                                          BIT            NULL,
    [Check_Box_17]                                                          BIT            NULL,
    [Check_Box_18]                                                          BIT            NULL,
    [Check_Box_19]                                                          BIT            NULL,
    [Check_Box_20]                                                          BIT            NULL,
    [Text_1]                                                                VARCHAR (50)   NULL,
    [Text_2]                                                                VARCHAR (50)   NULL,
    [Text_3]                                                                VARCHAR (50)   NULL,
    [Text_4]                                                                VARCHAR (50)   NULL,
    [Text_5]                                                                VARCHAR (50)   NULL,
    [Text_6]                                                                VARCHAR (50)   NULL,
    [Text_7]                                                                VARCHAR (50)   NULL,
    [Text_8]                                                                VARCHAR (50)   NULL,
    [Text_9]                                                                VARCHAR (50)   NULL,
    [Text_10]                                                               VARCHAR (50)   NULL,
    [IsDeleted]                                                             BIT            NULL,
    [IsAuthorized]                                                          BIT            NULL,
    [NatCatID]                                                              INT            NULL,
    [NatFamilyID]                                                           INT            NULL,
    [Brand_ID]                                                              INT            NULL
);




GO
GRANT DELETE
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PriceBatchDenorm] TO [WFM\ESB_ClickAndCollect]
    AS [dbo];


GO


