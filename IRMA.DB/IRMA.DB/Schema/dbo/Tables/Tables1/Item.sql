CREATE TABLE [dbo].[Item] (
    [Item_Key]                       INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Item_Description]               VARCHAR (60)   NOT NULL,
    [Sign_Description]               VARCHAR (60)   NOT NULL,
    [Ingredients]                    VARCHAR (3500) NULL,
    [SubTeam_No]                     INT            NOT NULL,
    [Sales_Account]                  VARCHAR (6)    NULL,
    [Package_Desc1]                  DECIMAL (9, 4) CONSTRAINT [DF__Item__Package_De__0CA1479E] DEFAULT ((0)) NOT NULL,
    [Package_Desc2]                  DECIMAL (9, 4) CONSTRAINT [DF__Item__Package_De__0D956BD7] DEFAULT ((0)) NOT NULL,
    [Package_Unit_ID]                INT            NOT NULL,
    [Min_Temperature]                SMALLINT       CONSTRAINT [DF__Item__Min_Temper__0F7DB449] DEFAULT ((0)) NOT NULL,
    [Max_Temperature]                SMALLINT       CONSTRAINT [DF__Item__Max_Temper__1071D882] DEFAULT ((0)) NOT NULL,
    [Units_Per_Pallet]               SMALLINT       CONSTRAINT [DF__Item__Units_Per___1165FCBB] DEFAULT ((0)) NOT NULL,
    [Average_Unit_Weight]            DECIMAL (9, 4) NULL,
    [Tie]                            TINYINT        CONSTRAINT [DF__Item__Tie__125A20F4] DEFAULT ((0)) NOT NULL,
    [High]                           TINYINT        CONSTRAINT [DF__Item__High__134E452D] DEFAULT ((0)) NOT NULL,
    [Yield]                          DECIMAL (9, 4) CONSTRAINT [DF__Item__Yield__14426966] DEFAULT ((100)) NOT NULL,
    [Brand_ID]                       INT            NULL,
    [Category_ID]                    INT            NULL,
    [Origin_ID]                      INT            NULL,
    [ShelfLife_Length]               SMALLINT       CONSTRAINT [DF__Item__ShelfLife___19FB42BC] DEFAULT ((0)) NOT NULL,
    [ShelfLife_ID]                   INT            NULL,
    [Retail_Unit_ID]                 INT            NOT NULL,
    [Vendor_Unit_ID]                 INT            NOT NULL,
    [Distribution_Unit_ID]           INT            NOT NULL,
    [Cost_Unit_ID]                   INT            NULL,
    [Freight_Unit_ID]                INT            NULL,
    [Deleted_Item]                   BIT            CONSTRAINT [DF__Item__Deleted_It__219C6484] DEFAULT ((0)) NOT NULL,
    [WFM_Item]                       BIT            CONSTRAINT [DF__Item__HIAH_Item__2478D12F] DEFAULT ((1)) NOT NULL,
    [Not_Available]                  BIT            CONSTRAINT [DF__Item__Not_Availa__266119A1] DEFAULT ((0)) NOT NULL,
    [Pre_Order]                      BIT            CONSTRAINT [DF__Item__Pre_Order__27553DDA] DEFAULT ((0)) NOT NULL,
    [Remove_Item]                    TINYINT        CONSTRAINT [DF__Item__Remove_Ite__2B25CEBE] DEFAULT ((0)) NOT NULL,
    [NoDistMarkup]                   BIT            CONSTRAINT [DF__Item__Average_Co__2FEA83DB] DEFAULT ((0)) NOT NULL,
    [Organic]                        BIT            CONSTRAINT [DF__Item__Organic__31D2CC4D] DEFAULT ((0)) NOT NULL,
    [Refrigerated]                   BIT            CONSTRAINT [DF__Item__Refrigerat__32C6F086] DEFAULT ((0)) NOT NULL,
    [Keep_Frozen]                    BIT            CONSTRAINT [DF__Item__Keep_Froze__33BB14BF] DEFAULT ((0)) NOT NULL,
    [Shipper_Item]                   BIT            CONSTRAINT [DF__Item__Shipper_It__35A35D31] DEFAULT ((0)) NOT NULL,
    [Full_Pallet_Only]               BIT            CONSTRAINT [DF__Item__Full_Palle__3697816A] DEFAULT ((0)) NOT NULL,
    [User_ID]                        INT            NULL,
    [POS_Description]                VARCHAR (26)   NOT NULL,
    [Retail_Sale]                    BIT            CONSTRAINT [DF__Item__Retail_Sal__3973EE15] DEFAULT ((0)) NOT NULL,
    [Food_Stamps]                    BIT            CONSTRAINT [DF__Item__Food_Stamp__3A68124E] DEFAULT ((0)) NOT NULL,
    [Discountable]                   BIT            CONSTRAINT [DF__Item__Discountab__3B5C3687] DEFAULT ((0)) NOT NULL,
    [Price_Required]                 BIT            CONSTRAINT [DF__Item__Price_Requ__41150FDD] DEFAULT ((0)) NOT NULL,
    [Quantity_Required]              BIT            CONSTRAINT [DF__Item__Quantity_R__42093416] DEFAULT ((0)) NOT NULL,
    [ItemType_ID]                    INT            CONSTRAINT [DF__Item__ItemType_I__43F17C88] DEFAULT ((0)) NOT NULL,
    [HFM_Item]                       BIT            CONSTRAINT [DF__item__HFM_Item__32695FD8] DEFAULT ((0)) NOT NULL,
    [ScaleDesc1]                     VARCHAR (64)   NULL,
    [ScaleDesc2]                     VARCHAR (64)   NULL,
    [Not_AvailableNote]              VARCHAR (255)  NULL,
    [CountryProc_ID]                 INT            NULL,
    [Insert_Date]                    DATETIME       CONSTRAINT [DF_Item_Insert_Date] DEFAULT (getdate()) NOT NULL,
    [Manufacturing_Unit_ID]          INT            NULL,
    [EXEDistributed]                 BIT            CONSTRAINT [DF_Item_EXEDistributed] DEFAULT ((0)) NOT NULL,
    [ClassID]                        INT            NULL,
    [User_ID_Date]                   DATETIME       NULL,
    [DistSubTeam_No]                 INT            NULL,
    [CostedByWeight]                 BIT            CONSTRAINT [DF_Item_CostedByWeight] DEFAULT ((0)) NOT NULL,
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
    [CatchweightRequired]            BIT            CONSTRAINT [DF_Item_CatchweightRequired] DEFAULT ((0)) NOT NULL,
    [COOL]                           BIT            DEFAULT ((0)) NOT NULL,
    [BIO]                            BIT            DEFAULT ((0)) NOT NULL,
    [LastModifiedUser_ID]            INT            NULL,
    [LastModifiedDate]               DATETIME       NULL,
    [CatchWtReq]                     BIT            CONSTRAINT [DF_Item_CatchWtReq] DEFAULT ((0)) NOT NULL,
    [SustainabilityRankingRequired]  BIT            DEFAULT ((0)) NULL,
    [SustainabilityRankingID]        INT            NULL,
    [Ingredient]                     BIT            DEFAULT ((0)) NOT NULL,
    [FSA_Eligible]                   BIT            DEFAULT ((0)) NOT NULL,
    [TaxClassModifiedDate]           DATETIME       NULL,
    [UseLastReceivedCost]            BIT            NULL,
    [GiftCard]                       BIT            CONSTRAINT [DF_Item_GiftCard] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Item_ItemKey] PRIMARY KEY CLUSTERED ([Item_Key] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK__Item__Brand_ID__15368D9F] FOREIGN KEY ([Brand_ID]) REFERENCES [dbo].[ItemBrand] ([Brand_ID]),
    CONSTRAINT [FK__Item__Category_I__162AB1D8] FOREIGN KEY ([Category_ID]) REFERENCES [dbo].[ItemCategory] ([Category_ID]),
    CONSTRAINT [FK__Item__Cost_Unit___1FB41C12] FOREIGN KEY ([Cost_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__Item__Distributi__1EBFF7D9] FOREIGN KEY ([Distribution_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__Item__Freight_Un__20A8404B] FOREIGN KEY ([Freight_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__Item__Manager__0E899010] FOREIGN KEY ([Manager_ID]) REFERENCES [dbo].[ItemManager] ([Manager_ID]),
    CONSTRAINT [FK__Item__Origin_ID__1812FA4A] FOREIGN KEY ([Origin_ID]) REFERENCES [dbo].[ItemOrigin] ([Origin_ID]),
    CONSTRAINT [FK__Item__Package_Un__0E899010] FOREIGN KEY ([Package_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E] FOREIGN KEY ([Retail_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK__Item__ShelfLife___1AEF66F5] FOREIGN KEY ([ShelfLife_ID]) REFERENCES [dbo].[ItemShelfLife] ([ShelfLife_ID]),
    CONSTRAINT [FK__Item__SubTeam_No__0BAD2365] FOREIGN KEY ([SubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK__Item__User_ID__378BA5A3] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[Users] ([User_ID]),
    CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0] FOREIGN KEY ([Vendor_Unit_ID]) REFERENCES [dbo].[ItemUnit] ([Unit_ID]),
    CONSTRAINT [FK_Item_ItemOrigin_CountryProc_ID] FOREIGN KEY ([CountryProc_ID]) REFERENCES [dbo].[ItemOrigin] ([Origin_ID]),
    CONSTRAINT [FK_Item_LabelType_ID] FOREIGN KEY ([LabelType_ID]) REFERENCES [dbo].[LabelType] ([LabelType_ID]),
    CONSTRAINT [FK_Item_ProdHierarchyLevel4] FOREIGN KEY ([ProdHierarchyLevel4_ID]) REFERENCES [dbo].[ProdHierarchyLevel4] ([ProdHierarchyLevel4_ID]),
    CONSTRAINT [FK_Item_StoreJurisdictionID] FOREIGN KEY ([StoreJurisdictionID]) REFERENCES [dbo].[StoreJurisdiction] ([StoreJurisdictionID]),
    CONSTRAINT [FK_Item_SubTeam] FOREIGN KEY ([DistSubTeam_No]) REFERENCES [dbo].[SubTeam] ([SubTeam_No]),
    CONSTRAINT [FK_Item_SustainabilityRanking] FOREIGN KEY ([SustainabilityRankingID]) REFERENCES [dbo].[SustainabilityRanking] ([ID]),
    CONSTRAINT [FK_Item_TaxClass1] FOREIGN KEY ([TaxClassID]) REFERENCES [dbo].[TaxClass] ([TaxClassID])
);


GO
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK_Item_SustainabilityRanking];


GO
ALTER TABLE [dbo].[Item] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [idxItemPOSDescription]
    ON [dbo].[Item]([POS_Description] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemDescription]
    ON [dbo].[Item]([Item_Description] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemSubTeamNo]
    ON [dbo].[Item]([SubTeam_No] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemBrand]
    ON [dbo].[Item]([Brand_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemCategory]
    ON [dbo].[Item]([Category_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemOrigin]
    ON [dbo].[Item]([Origin_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemPackageUnit]
    ON [dbo].[Item]([Package_Unit_ID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [idxItemShelfLife]
    ON [dbo].[Item]([ShelfLife_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemRetailUnit]
    ON [dbo].[Item]([Retail_Unit_ID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [idxItemCostUnitID]
    ON [dbo].[Item]([Cost_Unit_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [idxItemOrderingUnit]
    ON [dbo].[Item]([Vendor_Unit_ID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [idxItemDistributionUnit]
    ON [dbo].[Item]([Distribution_Unit_ID] ASC) WITH (FILLFACTOR = 90);


GO
CREATE NONCLUSTERED INDEX [idxItemFreightUnitID]
    ON [dbo].[Item]([Freight_Unit_ID] ASC);


GO
CREATE NONCLUSTERED INDEX [idxItemUserID]
    ON [dbo].[Item]([User_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemCountryProc_ID]
    ON [dbo].[Item]([CountryProc_ID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [idxItemRemoveItem]
    ON [dbo].[Item]([Remove_Item] ASC) WITH (FILLFACTOR = 80);


GO
CREATE STATISTICS [_dta_stat_Item_001]
    ON [dbo].[Item]([Full_Pallet_Only], [Item_Key]);


GO

CREATE TRIGGER [dbo].[ItemUpdate]
ON [dbo].[Item] FOR UPDATE
AS

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes
DN		20130114	8755	Using the function dbo.fn_GetDiscontinueStatus instead of the 
							Discontinue_Item field in the Item table.
KM		2013-04-14	11774	Add PCMSGiftCard to the ItemChangeHistory INSERT; Remove Discontinue_Item 
							from the change record (column has moved to StoreItemVendor);
KM		2013-04-30	11774	Rename PCMSGiftCard to just GiftCard;
MZ      2014-04-21  2802    Modified the trigger so that if the item update is done by the iConController 
                            job, then no PriceBatchDetail and PLUMCorpChgQueue records will be created. 
							Also, at the end of the trigger, the iConController user id will be wiped
							out from the LastModifiedUser_ID field. This is done because not all the 
							Item Updates will re-set the LastModifiedUser_ID value. An item update done
							after the iConController job can retain the LastModifiedUser_ID value, and 
							not having PriceBatchDetail and PLUMCorpChgQueue records generated.
KM		2015-03-17	15935	Allow batchable maintenance to be created for subteam-related updates.
DN		2015-03-20	15940	Remove all scan codes (default and alternate) from ValidatedScanCode table when 
							Retail_Sale flag changes from 1 to 0. 
							Add all the scan codes (default and alternate) to IconItemChangeQueue when
							Retail_Sale flag changes from 0 to 1.
KM		2015-12-24	13326	Allow batchable maintenance to be created when Icon updates the Item Pack (Package Unit, Package_Desc1).
MU		2015-01-06	13476	Removing Product_Code from the fields that generate PBD inserts.
CM		2016-01-12	13691	Enable batchable maintenance when UOM changes
MU		2016-01-27	13661	Removing ItemLabelType_ID from the fields that generate PBD inserts.
EM      2017-11-06  23919   Disable item maintenance when GPM is active 
***********************************************************************************************/

BEGIN

	DECLARE 
		@error_no int = 0, 	
		@IconControllerUserId int = (SELECT User_ID FROM Users WHERE UserName = 'iconcontrolleruser')

	DECLARE @Identifiers TABLE (Item_Key int, Identifier varchar(13), IdentifierType varchar(3));
		
	-- Add item to EXE item change queue table if it is supplied by a warehouse with the EXE system installed
	INSERT INTO WarehouseItemChange
	(
		Store_No,
		Item_Key,
		ChangeType
	)
	SELECT 
		Supplier_Store_No,
	    INSERTED.Item_Key,
	    CASE 
	        WHEN (INSERTED.Deleted_Item = 1) OR (INSERTED.EXEDistributed = 0) THEN 
	                'D'
	        ELSE CASE 
	                    WHEN INSERTED.EXEDistributed <> DELETED.EXEDistributed THEN 
	                        'A'
	                    ELSE 'M'
	                END
	    END
	FROM   
		INSERTED
	    INNER JOIN DELETED
	        ON  DELETED.Item_Key = INSERTED.Item_Key
	    INNER JOIN (
	            SELECT Supplier_Store_No,
	                    SubTeam_No
	            FROM   ZoneSubTeam Z(NOLOCK)
	                    INNER JOIN Store(NOLOCK)
	                        ON  Store.Store_No = Z.Supplier_Store_No
	            WHERE  EXEWarehouse IS NOT NULL
	            GROUP BY
	                    Supplier_Store_No,
	                    SubTeam_No
	        ) ZS
	        ON  ZS.SubTeam_No = INSERTED.SubTeam_No
	WHERE  
		(
	        INSERTED.Deleted_Item <> DELETED.Deleted_Item
	        OR INSERTED.Not_Available <> DELETED.Not_Available
	        OR INSERTED.Item_Description <> DELETED.Item_Description
	        OR INSERTED.POS_Description <> DELETED.POS_Description
	        OR INSERTED.Package_Desc2 <> DELETED.Package_Desc2
	        OR INSERTED.Package_Desc1 <> DELETED.Package_Desc1
	        OR INSERTED.EXEDistributed <> DELETED.EXEDistributed
            OR INSERTED.Cool <> DELETED.Cool
            OR INSERTED.Bio <> DELETED.Bio
            OR INSERTED.[CatchweightRequired] <> DELETED.[CatchweightRequired]
	    )
	    AND (INSERTED.EXEDistributed = 1 OR DELETED.EXEDistributed = 1)
	    AND INSERTED.SubTeam_No = DELETED.SubTeam_No

	UNION

	SELECT 
		Supplier_Store_No,
	    INSERTED.Item_Key,
	    'A'
	FROM   
		INSERTED
	    INNER JOIN DELETED
	        ON  DELETED.Item_Key = INSERTED.Item_Key
	    INNER JOIN (
	            SELECT Supplier_Store_No,
	                    SubTeam_No
	            FROM   ZoneSubTeam Z(NOLOCK)
	                    INNER JOIN Store(NOLOCK)
	                        ON  Store.Store_No = Z.Supplier_Store_No
	            WHERE  EXEWarehouse IS NOT NULL
	            GROUP BY
	                    Supplier_Store_No,
	                    SubTeam_No
	        ) ZS
	        ON  ZS.SubTeam_No = INSERTED.SubTeam_No
	WHERE  
		INSERTED.SubTeam_No <> DELETED.SubTeam_No
	    AND INSERTED.Deleted_Item = 0
	    AND INSERTED.EXEDistributed = 1

	UNION

	SELECT 
		Supplier_Store_No,
	    INSERTED.Item_Key,
	    'D'
	FROM   
		INSERTED
	    INNER JOIN DELETED
	        ON  DELETED.Item_Key = INSERTED.Item_Key
	    INNER JOIN (
	            SELECT Supplier_Store_No,
	                    SubTeam_No
	            FROM   ZoneSubTeam Z(NOLOCK)
	                    INNER JOIN Store(NOLOCK)
	                        ON  Store.Store_No = Z.Supplier_Store_No
	            WHERE  EXEWarehouse IS NOT NULL
	            GROUP BY
	                    Supplier_Store_No,
	                    SubTeam_No
	        ) ZS
	        ON  ZS.SubTeam_No = DELETED.SubTeam_No
	WHERE  
		INSERTED.SubTeam_No <> DELETED.SubTeam_No
	    AND DELETED.EXEDistributed = 1
	
	SELECT @error_no = @@ERROR
	
	IF @error_no = 0
		BEGIN -- Queue for Price Modeling if necessary			
			INSERT INTO PMProductChg
			(
				HierLevel,
				Item_Key,
				ItemID,
				ItemDescription,
				ParentID,
				ParentDescription,
				ActionID,
				STATUS
			)
			SELECT 
				'Product',
				INSERTED.Item_Key,
				Identifier,
				INSERTED.Item_Description,
				ISNULL(
					ItemCategory.Category_ID,
					CONVERT(VARCHAR(255), INSERTED.SubTeam_No) + '1'
				),
				ISNULL(Category_Name, 'NO CATEGORY'),
				CASE 
					WHEN INSERTED.Deleted_Item = 1 THEN 'DELETE'
					ELSE 'CHANGE'
				END,
				CASE 
					WHEN dbo.fn_GetDiscontinueStatus(INSERTED.Item_Key, NULL, NULL) = 1 THEN 'DISCONTINUED'
					ELSE 'ACTIVE'
				END
			FROM   
				INSERTED
				INNER JOIN DELETED
					ON  DELETED.Item_Key = INSERTED.Item_Key
				INNER JOIN ItemIdentifier II
					ON  II.Item_Key = INSERTED.Item_Key
					AND Default_Identifier = 1
				LEFT JOIN ItemCategory
					ON  INSERTED.Category_ID = ItemCategory.Category_ID
			WHERE  
				INSERTED.Retail_Sale = 1
				AND (
						(INSERTED.Deleted_Item <> DELETED.Deleted_Item)
						OR (DELETED.Retail_Sale <> INSERTED.Retail_Sale)
						OR (DELETED.Item_Description <> INSERTED.Item_Description)
						OR (DELETED.Deleted_Item <> INSERTED.Deleted_Item)
						OR (
								ISNULL(DELETED.ProdHierarchyLevel4_ID, 0) <> 
								ISNULL(INSERTED.ProdHierarchyLevel4_ID, 0)
							)
						OR (
								ISNULL(DELETED.Category_ID, 0) <> ISNULL(INSERTED.Category_ID, 0)
							)
					)
				AND (
						INSERTED.SubTeam_No IN (SELECT SubTeam_No
												FROM   PMSubTeamInclude(NOLOCK))
						OR DELETED.SubTeam_No IN (SELECT SubTeam_No
													FROM   PMSubTeamInclude(NOLOCK))
					)
				AND NOT EXISTS (
						SELECT *
						FROM   PMExcludedItem
						WHERE  Item_Key = INSERTED.Item_Key
					)
	    
			SELECT @error_no = @@ERROR
		END
	
	IF @error_no = 0
		BEGIN -- Insert ItemChangeHistory
			INSERT INTO ItemChangeHistory
			(
				Item_Key,
				Item_Description,
				Sign_Description,
				Ingredients,
				SubTeam_No,
				Sales_Account,
				Package_Desc1,
				Package_Desc2,
				Package_Unit_ID,
				Min_Temperature,
				Max_Temperature,
				Units_Per_Pallet,
				Average_Unit_Weight,
				Tie,
				HIGH,
				Yield,
				Brand_ID,
				Category_ID,
				Origin_ID,
				ShelfLife_Length,
				ShelfLife_ID,
				Retail_Unit_ID,
				Vendor_Unit_ID,
				Distribution_Unit_ID,
				WFM_Item,
				Not_Available,
				Pre_Order,
				NoDistMarkup,
				Organic,
				Refrigerated,
				Keep_Frozen,
				Shipper_Item,
				Full_Pallet_Only,
				POS_Description,
				Retail_Sale,
				Food_Stamps,
				Price_Required,
				Quantity_Required,
				ItemType_ID,
				HFM_Item,
				ScaleDesc1,
				ScaleDesc2,
				Not_AvailableNote,
				CountryProc_ID,
				Manufacturing_Unit_ID,
				EXEDistributed,
				DistSubTeam_No,
				CostedByWeight,
				TaxClassID,
				USER_ID,
				User_ID_Date,
				LabelType_ID,
				QtyProhibit,
				GroupList,
				Case_Discount,
				Coupon_Multiplier,
				Misc_Transaction_Sale,
				Misc_Transaction_Refund,
				Recall_Flag,
				Manager_ID,
				Ice_Tare,
				PurchaseThresholdCouponAmount,
				PurchaseThresholdCouponSubTeam,
				Product_Code,
				Unit_Price_Category,
				StoreJurisdictionID,
				CatchweightRequired,
				Cost_Unit_ID,
				Freight_Unit_ID,
				Discountable,
				ClassID,
				SustainabilityRankingRequired,
				SustainabilityRankingID,
				GiftCard
			)
			SELECT 
				INSERTED.Item_Key,
				INSERTED.Item_Description,
				INSERTED.Sign_Description,
				INSERTED.Ingredients,
				INSERTED.SubTeam_No,
				INSERTED.Sales_Account,
				INSERTED.Package_Desc1,
				INSERTED.Package_Desc2,
				INSERTED.Package_Unit_ID,
				INSERTED.Min_Temperature,
				INSERTED.Max_Temperature,
				INSERTED.Units_Per_Pallet,
				INSERTED.Average_Unit_Weight,
				INSERTED.Tie,
				INSERTED.High,
				INSERTED.Yield,
				INSERTED.Brand_ID,
				INSERTED.Category_ID,
				INSERTED.Origin_ID,
				INSERTED.ShelfLife_Length,
				INSERTED.ShelfLife_ID,
				INSERTED.Retail_Unit_ID,
				INSERTED.Vendor_Unit_ID,
				INSERTED.Distribution_Unit_ID,
				INSERTED.WFM_Item,
				INSERTED.Not_Available,
				INSERTED.Pre_Order,
				INSERTED.NoDistMarkup,
				INSERTED.Organic,
				INSERTED.Refrigerated,
				INSERTED.Keep_Frozen,
				INSERTED.Shipper_Item,
				INSERTED.Full_Pallet_Only,
				INSERTED.POS_Description,
				INSERTED.Retail_Sale,
				INSERTED.Food_Stamps,
				INSERTED.Price_Required,
				INSERTED.Quantity_Required,
				INSERTED.ItemType_ID,
				INSERTED.HFM_Item,
				INSERTED.ScaleDesc1,
				INSERTED.ScaleDesc2,
				INSERTED.Not_AvailableNote,
				INSERTED.CountryProc_ID,
				INSERTED.Manufacturing_Unit_ID,
				INSERTED.EXEDistributed,
				INSERTED.DistSubTeam_No,
				INSERTED.CostedByWeight,
				INSERTED.TaxClassID,
				INSERTED.User_ID,
				INSERTED.User_ID_Date,
				INSERTED.LabelType_ID,
				INSERTED.QtyProhibit,
				INSERTED.GroupList,
				INSERTED.Case_Discount,
				INSERTED.Coupon_Multiplier,
				INSERTED.Misc_Transaction_Sale,
				INSERTED.Misc_Transaction_Refund,
				INSERTED.Recall_Flag,
				INSERTED.Manager_ID,
				INSERTED.Ice_Tare,
				INSERTED.PurchaseThresholdCouponAmount,
				INSERTED.PurchaseThresholdCouponSubTeam,
				INSERTED.Product_Code,
				INSERTED.Unit_Price_Category,
				INSERTED.StoreJurisdictionID,
				INSERTED.CatchweightRequired,
				INSERTED.Cost_Unit_ID,
   				INSERTED.Freight_Unit_ID,
				INSERTED.Discountable,
				INSERTED.ClassID,
				INSERTED.SustainabilityRankingRequired,
				INSERTED.SustainabilityRankingID,
				INSERTED.GiftCard

			FROM   
				INSERTED
				INNER JOIN DELETED ON DELETED.Item_Key = INSERTED.Item_Key
	    
			WHERE  
				INSERTED.Item_Description <> DELETED.Item_Description
				OR  INSERTED.Sign_Description <> DELETED.Sign_Description
				OR  ISNULL(INSERTED.Ingredients, '') <> ISNULL(DELETED.Ingredients, '')
				OR  INSERTED.SubTeam_No <> DELETED.SubTeam_No
				OR  ISNULL(INSERTED.Sales_Account, '') <> ISNULL(DELETED.Sales_Account, '')
				OR  INSERTED.Package_Desc1 <> DELETED.Package_Desc1
				OR  INSERTED.Package_Desc2 <> DELETED.Package_Desc2
				OR  ISNULL(INSERTED.Package_Unit_ID, 0) <> ISNULL(DELETED.Package_Unit_ID, 0)
				OR  INSERTED.Min_Temperature <> DELETED.Min_Temperature
				OR  INSERTED.Max_Temperature <> DELETED.Max_Temperature
				OR  INSERTED.Units_Per_Pallet <> DELETED.Units_Per_Pallet
				OR  ISNULL(INSERTED.Average_Unit_Weight, 0) <> ISNULL(DELETED.Average_Unit_Weight, 0)
				OR  INSERTED.Tie <> DELETED.Tie
				OR  INSERTED.High <> DELETED.High
				OR  INSERTED.Yield <> DELETED.Yield
				OR  ISNULL(INSERTED.Brand_ID, 0) <> ISNULL(DELETED.Brand_ID, 0)
				OR  ISNULL(INSERTED.Category_ID, 0) <> ISNULL(DELETED.Category_ID, 0)
				OR  ISNULL(INSERTED.ProdHierarchyLevel4_ID, 0) <> ISNULL(DELETED.ProdHierarchyLevel4_ID, 0)
				OR  ISNULL(INSERTED.Origin_ID, 0) <> ISNULL(DELETED.Origin_ID, 0)
				OR  INSERTED.ShelfLife_Length <> DELETED.ShelfLife_Length
				OR  ISNULL(INSERTED.ShelfLife_ID, 0) <> ISNULL(DELETED.ShelfLife_ID, 0)
				OR  ISNULL(INSERTED.Retail_Unit_ID, 0) <> ISNULL(DELETED.Retail_Unit_ID, 0)
				OR  ISNULL(INSERTED.Vendor_Unit_ID, 0) <> ISNULL(DELETED.Vendor_Unit_ID, 0)
				OR  ISNULL(INSERTED.Distribution_Unit_ID, 0) <> ISNULL(DELETED.Distribution_Unit_ID, 0)
				OR  INSERTED.WFM_Item <> DELETED.WFM_Item
				OR  INSERTED.Not_Available <> DELETED.Not_Available
				OR  INSERTED.Pre_Order <> DELETED.Pre_Order
				OR  INSERTED.NoDistMarkup <> DELETED.NoDistMarkup
				OR  INSERTED.Organic <> DELETED.Organic
				OR  INSERTED.Refrigerated <> DELETED.Refrigerated
				OR  INSERTED.Keep_Frozen <> DELETED.Keep_Frozen
				OR  INSERTED.Shipper_Item <> DELETED.Shipper_Item
				OR  INSERTED.Full_Pallet_Only <> DELETED.Full_Pallet_Only
				OR  INSERTED.POS_Description <> DELETED.POS_Description
				OR  INSERTED.Retail_Sale <> DELETED.Retail_Sale
				OR  INSERTED.Food_Stamps <> DELETED.Food_Stamps
				OR  INSERTED.Price_Required <> DELETED.Price_Required
				OR  INSERTED.Quantity_Required <> DELETED.Quantity_Required
				OR  INSERTED.ItemType_ID <> DELETED.ItemType_ID
				OR  INSERTED.HFM_Item <> DELETED.HFM_Item
				OR  ISNULL(INSERTED.ScaleDesc1, '') <> ISNULL(DELETED.ScaleDesc1, '')
				OR  ISNULL(INSERTED.ScaleDesc2, '') <> ISNULL(DELETED.ScaleDesc2, '')
				OR  ISNULL(INSERTED.Not_AvailableNote, '') <> ISNULL(DELETED.Not_AvailableNote, '')
				OR  ISNULL(INSERTED.CountryProc_ID, 0) <> ISNULL(DELETED.CountryProc_ID, 0)
				OR  ISNULL(INSERTED.Manufacturing_Unit_ID, 0) <> ISNULL(DELETED.Manufacturing_Unit_ID, 0)
				OR  INSERTED.EXEDistributed <> DELETED.EXEDistributed
				OR  INSERTED.DistSubTeam_No <> DELETED.DistSubTeam_No
				OR  INSERTED.CostedByWeight <> DELETED.CostedByWeight
				OR  INSERTED.TaxClassID <> DELETED.TaxClassID
				OR  INSERTED.LabelType_ID <> DELETED.LabelType_ID
				OR  ISNULL(INSERTED.QtyProhibit, 0) <> ISNULL(DELETED.QtyProhibit, 0)
				OR  ISNULL(INSERTED.GroupList, 0) <> ISNULL(DELETED.GroupList, 0)
				OR  ISNULL(INSERTED.Case_Discount, 0) <> ISNULL(DELETED.Case_Discount, 0)
				OR  ISNULL(INSERTED.Coupon_Multiplier, 0) <> ISNULL(DELETED.Coupon_Multiplier, 0)
				OR  ISNULL(INSERTED.Misc_Transaction_Sale, 0) <> ISNULL(DELETED.Misc_Transaction_Sale, 0)
				OR  ISNULL(INSERTED.Misc_Transaction_Refund, 0) <> ISNULL(DELETED.Misc_Transaction_Refund, 0)
				OR  ISNULL(INSERTED.Recall_Flag, 0) <> ISNULL(DELETED.Recall_Flag, 0)
				OR  ISNULL(INSERTED.Manager_ID, 0) <> ISNULL(DELETED.Manager_ID, 0)
				OR  ISNULL(INSERTED.Ice_Tare, 0) <> ISNULL(DELETED.Ice_Tare, 0)
				OR  ISNULL(INSERTED.PurchaseThresholdCouponAmount, 0) <> ISNULL(DELETED.PurchaseThresholdCouponAmount, 0)
				OR  ISNULL(INSERTED.PurchaseThresholdCouponSubTeam, 0) <> ISNULL(DELETED.PurchaseThresholdCouponSubTeam, 0)
				OR  ISNULL(INSERTED.Product_Code, 0) <> ISNULL(DELETED.Product_Code, 0)
				OR  ISNULL(INSERTED.Unit_Price_Category, 0) <> ISNULL(DELETED.Unit_Price_Category, 0)
				OR  ISNULL(INSERTED.StoreJurisdictionID, 0) <> ISNULL(DELETED.StoreJurisdictionID, 0)
				OR  INSERTED.CatchweightRequired <> DELETED.CatchweightRequired
				OR  INSERTED.Cost_Unit_ID <> DELETED.Cost_Unit_ID
   				OR	INSERTED.Freight_Unit_ID <> DELETED.Freight_Unit_ID
				OR	INSERTED.Discountable <> DELETED.Discountable
				OR	INSERTED.ClassID <> DELETED.ClassID
				OR	INSERTED.SustainabilityRankingRequired <> DELETED.SustainabilityRankingRequired
				OR	INSERTED.SustainabilityRankingID <> DELETED.SustainabilityRankingID
				OR	INSERTED.GiftCard <> DELETED.GiftCard
	    
			SELECT @error_no = @@ERROR
		END
	
	DECLARE @BatchOrganicChanges BIT = (SELECT dbo.fn_InstanceDataValue('BatchOrganicChanges', NULL)); -- Organic changes are controlled by 'BatchOrganicChanges' IDF
	IF @error_no = 0
		BEGIN -- send down PriceBatchDetail records to allow item changes to be batched
			INSERT INTO PriceBatchDetail
			(
				Store_No,
				Item_Key,
				ItemChgTypeID,
				InsertApplication
			)
			SELECT 
				Store_No,
				INSERTED.Item_Key,
				2,
				'ItemUpdate Trigger'
			FROM   
				INSERTED
				INNER JOIN DELETED
					ON  DELETED.Item_Key = INSERTED.Item_Key
				LEFT OUTER JOIN dbo.InstanceDataFlagsStoreOverride IDFO
					ON IDFO.Store_No = Store_No
				LEFT OUTER JOIN dbo.InstanceDataFlagsStore IDF
					ON IDF.Store_No = Store_No
				CROSS JOIN (
						SELECT Store_No
						FROM   Store(NOLOCK)
						WHERE  WFM_Store = 1
								OR  Mega_Store = 1
					) Store
			WHERE  
				(INSERTED.Remove_Item = 0 AND INSERTED.Deleted_Item = 0)
				AND IDFO.Flag_Key = 'GlobalPriceManagement' AND IDF.Flag_Key = 'GlobalPriceManagement'
				AND (
						-- Don't allow maintenance to be created if Icon is doing the update, unless...                       
						(ISNULL(inserted.LastModifiedUser_ID, 0) <> ISNULL(@IconControllerUserId, 0)) 
						  -- ... icon is doing the update and it's a subteam update (then do create maintenance)... 
						OR (ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND inserted.SubTeam_No <> deleted.SubTeam_No)
						  -- ... or if icon is doing the update and it's a package unit update
						  -- ... but only if it's for a store not under GPM
						OR ( ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND 
							(inserted.Package_Desc1 <> deleted.Package_Desc1 OR inserted.Package_Unit_ID <> deleted.Package_Unit_ID) AND
							COALESCE(IDFO.FlagValue, IDF.FlagValue, 0) = 0 )
					)
				AND (
						INSERTED.Item_Description <> DELETED.Item_Description
						OR INSERTED.POS_Description <> DELETED.POS_Description
						OR INSERTED.Sign_Description <> DELETED.Sign_Description
						OR INSERTED.Food_Stamps <> DELETED.Food_Stamps
						OR INSERTED.Price_Required <> DELETED.Price_Required
						OR INSERTED.Quantity_Required <> DELETED.Quantity_Required
						OR (INSERTED.Organic <> DELETED.Organic
							AND @BatchOrganicChanges = 1)
						OR INSERTED.Retail_Sale <> DELETED.Retail_Sale
						OR INSERTED.ItemType_ID <> DELETED.ItemType_ID
						OR ISNULL(INSERTED.Retail_Unit_ID, 0) <> ISNULL(DELETED.Retail_Unit_ID, 0)
						OR INSERTED.SubTeam_No <> DELETED.SubTeam_No
						OR ISNULL(INSERTED.Origin_ID, 0) <> ISNULL(DELETED.Origin_ID, 0)
						OR ISNULL(INSERTED.Brand_ID, 0) <> ISNULL(DELETED.Brand_ID, 0)
						OR INSERTED.Package_Desc1 <> DELETED.Package_Desc1
						OR INSERTED.Package_Desc2 <> DELETED.Package_Desc2
						OR INSERTED.TaxClassID <> DELETED.TaxClassID
						OR ISNULL(INSERTED.QtyProhibit, 0) <> ISNULL(DELETED.QtyProhibit, 0)
						OR ISNULL(INSERTED.GroupList, 0) <> ISNULL(DELETED.GroupList, 0)
						OR ISNULL(INSERTED.Package_Unit_ID, 0) <> ISNULL(DELETED.Package_Unit_ID, 0)
						OR ISNULL(INSERTED.Case_Discount, 0) <> ISNULL(DELETED.Case_Discount, 0)
						OR ISNULL(INSERTED.Coupon_Multiplier, 0) <> ISNULL(DELETED.Coupon_Multiplier, 0)
						OR ISNULL(INSERTED.Misc_Transaction_Sale, 0) <> ISNULL(DELETED.Misc_Transaction_Sale, 0)
						OR ISNULL(INSERTED.Misc_Transaction_Refund, 0) <> ISNULL(DELETED.Misc_Transaction_Refund, 0)
						OR ISNULL(INSERTED.Recall_Flag, 0) <> ISNULL(DELETED.Recall_Flag, 0)
						OR ISNULL(INSERTED.Ice_Tare, 0) <> ISNULL(DELETED.Ice_Tare, 0)
						OR ISNULL(INSERTED.PurchaseThresholdCouponAmount, 0) <> 
							ISNULL(DELETED.PurchaseThresholdCouponAmount, 0)
						OR ISNULL(INSERTED.PurchaseThresholdCouponSubTeam, 0) <> 
							ISNULL(DELETED.PurchaseThresholdCouponSubTeam, 0)
						OR ISNULL(INSERTED.Unit_Price_Category, 0) <> ISNULL(DELETED.Unit_Price_Category, 0)
						OR ISNULL(INSERTED.FSA_Eligible, 0) <> ISNULL(DELETED.FSA_Eligible, 0)
					)
				AND (
						dbo.fn_HasPendingItemChangePriceBatchDetailRecord(INSERTED.Item_Key, Store.Store_No) = 0
					)
	    
			SELECT @error_no = @@ERROR
		END

	IF @error_no = 0
		BEGIN -- If Retail_Sale flag 1->0 then delete all identifiers (default and alternate) from VSC
			DECLARE @ItemsChangedFromRetailSale TABLE (Item_Key int, RetailSaleChanged bit);
			
			INSERT INTO 
				@ItemsChangedFromRetailSale
			SELECT
				inserted.Item_Key,
				CASE
					WHEN INSERTED.Retail_Sale = 0 AND DELETED.Retail_Sale = 1 THEN 1
					ELSE 0
				END
			FROM 
				INSERTED 
				INNER JOIN DELETED ON DELETED.Item_Key = INSERTED.Item_Key

			IF EXISTS (SELECT * FROM @ItemsChangedFromRetailSale icr WHERE icr.RetailSaleChanged = 1)
				BEGIN
					DECLARE @EnableUPCIRMAToIConFlow_RS bit
					SELECT  @EnableUPCIRMAToIConFlow_RS = acv.Value
							FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
							ON acv.EnvironmentID = ace.EnvironmentID 
							INNER JOIN AppConfigApp aca
							ON acv.ApplicationID = aca.ApplicationID 
							INNER JOIN AppConfigKey ack
							ON acv.KeyID = ack.KeyID 
							WHERE aca.Name = 'IRMA Client' AND
							ack.Name = 'EnableUPCIRMAToIConFlow' and
							SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
	
					DECLARE @EnablePLUIRMAIConFlow_RS bit
					SELECT @EnablePLUIRMAIConFlow_RS = acv.Value
							FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
							ON acv.EnvironmentID = ace.EnvironmentID 
							INNER JOIN AppConfigApp aca
							ON acv.ApplicationID = aca.ApplicationID 
							INNER JOIN AppConfigKey ack
							ON acv.KeyID = ack.KeyID 
							WHERE aca.Name = 'IRMA Client' AND
							ack.Name = 'EnablePLUIRMAIConFlow' and
							SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
			
					DELETE FROM @Identifiers -- Clear any existing identifiers
					INSERT INTO @Identifiers
					SELECT
								   ii.Item_Key as Item_Key,
								   ii.Identifier as Identifier,
										  CASE
												 WHEN (len(ii.Identifier) <= 6) OR (len(ii.Identifier) = 11 and ii.Identifier like '2%00000') then 'PLU'
												 ELSE 'UPC'
										  END    
							 FROM ItemIdentifier ii JOIN inserted ON ii.Item_Key = inserted.Item_Key
							 -- WHERE ii.Default_Identifier = 1
					
					DELETE FROM ValidatedScanCode WHERE ScanCode IN (SELECT Identifier FROM @Identifiers)
				END

			SELECT @error_no = @@ERROR
		END

	IF @error_no = 0
		BEGIN -- If Retail_Sale flag 0->1 then insert as new Item(s) into IconItemChangeQueue so icon can manage the Item's information. 
			DECLARE @ItemsChangedToRetailSale TABLE (Item_Key int, RetailSaleChanged bit);
			
			INSERT INTO 
				@ItemsChangedToRetailSale
			SELECT
				inserted.Item_Key,
				CASE
					WHEN INSERTED.Retail_Sale = 1 AND DELETED.Retail_Sale = 0 THEN 1
					ELSE 0
				END
			FROM 
				INSERTED 
				INNER JOIN DELETED ON DELETED.Item_Key = INSERTED.Item_Key

			IF EXISTS (SELECT * FROM @ItemsChangedToRetailSale icr WHERE icr.RetailSaleChanged = 1)
				BEGIN
					DECLARE @EnableUPCIRMAToIConFlow bit
					SELECT  @EnableUPCIRMAToIConFlow = acv.Value
							FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
							ON acv.EnvironmentID = ace.EnvironmentID 
							INNER JOIN AppConfigApp aca
							ON acv.ApplicationID = aca.ApplicationID 
							INNER JOIN AppConfigKey ack
							ON acv.KeyID = ack.KeyID 
							WHERE aca.Name = 'IRMA Client' AND
							ack.Name = 'EnableUPCIRMAToIConFlow' and
							SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
	
					DECLARE @EnablePLUIRMAIConFlow bit
					SELECT @EnablePLUIRMAIConFlow = acv.Value
							FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
							ON acv.EnvironmentID = ace.EnvironmentID 
							INNER JOIN AppConfigApp aca
							ON acv.ApplicationID = aca.ApplicationID 
							INNER JOIN AppConfigKey ack
							ON acv.KeyID = ack.KeyID 
							WHERE aca.Name = 'IRMA Client' AND
							ack.Name = 'EnablePLUIRMAIConFlow' and
							SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
			
					DELETE FROM @Identifiers -- Clear any existing identifers
					INSERT INTO @Identifiers
					SELECT
								   ii.Item_Key as Item_Key,
								   ii.Identifier as Identifier,
										  CASE
												 WHEN (len(ii.Identifier) <= 6) OR (len(ii.Identifier) = 11 and ii.Identifier like '2%00000') then 'PLU'
												 ELSE 'UPC'
										  END    
							 FROM ItemIdentifier ii JOIN inserted ON ii.Item_Key = inserted.Item_Key
							 -- WHERE ii.Default_Identifier = 1


					DECLARE @newItemChgTypeID tinyint
					SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like 'new'

					INSERT INTO IconItemChangeQueue 
					(
						Item_Key,
						Identifier,
						ItemChgTypeID
					)
					SELECT
						inserted.Item_Key    as Item_Key,
						i.Identifier         as Identifier,
						@newItemChgTypeID    as ItemChgTypeID
					FROM
						inserted
						JOIN @Identifiers i on inserted.Item_Key = i.Item_Key
					WHERE
						(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1)
						OR (@EnableUPCIRMAToIConFlow = 1 AND i.IdentifierType = 'UPC')
						OR (@EnablePLUIRMAIConFlow = 1 AND i.IdentifierType = 'PLU')
				END

			SELECT @error_no = @@ERROR
		END
	
	IF @error_no = 0
		BEGIN -- insert to PLUMCorpChgQueue if needed
			INSERT INTO PLUMCorpChgQueue
			(
				Item_Key,
				ActionCode,
				Store_No
			)
			SELECT 
				INSERTED.Item_Key,
				'C',
				s.Store_No
			FROM   
				INSERTED
				INNER JOIN DELETED ON  DELETED.Item_Key = INSERTED.Item_Key
				CROSS JOIN Store s
				JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
			WHERE  
				INSERTED.Remove_Item = 0
				AND INSERTED.Deleted_Item = 0
				AND (
						-- Don't allow maintenance to be created if Icon is doing the update, unless it's a subteam update.
						(ISNULL(inserted.LastModifiedUser_ID, 0) <> ISNULL(@IconControllerUserId, 0)) 
						OR (ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND inserted.SubTeam_No <> deleted.SubTeam_No)
					)
				AND (
						ISNULL(INSERTED.Ingredients, '') <> ISNULL(DELETED.Ingredients, '')
						OR ISNULL(INSERTED.ScaleDesc1, '') <> ISNULL(DELETED.ScaleDesc1, '')
						OR ISNULL(INSERTED.ScaleDesc2, '') <> ISNULL(DELETED.ScaleDesc2, '')
						OR ISNULL(INSERTED.ScaleDesc3, '') <> ISNULL(DELETED.ScaleDesc3, '')
						OR ISNULL(INSERTED.ScaleDesc4, '') <> ISNULL(DELETED.ScaleDesc4, '')
						OR ISNULL(INSERTED.Retail_Unit_ID, 0) <> ISNULL(DELETED.Retail_Unit_ID, 0)
						OR INSERTED.SubTeam_No <> DELETED.SubTeam_No
						OR INSERTED.Package_Desc1 <> DELETED.Package_Desc1
						OR INSERTED.Package_Desc2 <> DELETED.Package_Desc2
						OR ISNULL(INSERTED.Package_Unit_ID, 0) <> ISNULL(DELETED.Package_Unit_ID, 0)
						OR ISNULL(INSERTED.ShelfLife_Length, 0) <> ISNULL(DELETED.ShelfLife_Length, 0)
						OR ISNULL(INSERTED.ScaleTare, 0) <> ISNULL(DELETED.ScaleTare, 0)
						OR ISNULL(INSERTED.ScaleUseBy, 0) <> ISNULL(DELETED.ScaleUseBy, 0)
						OR ISNULL(INSERTED.ScaleForcedTare, 0) <> ISNULL(DELETED.ScaleForcedTare, 0)
					)
				AND EXISTS (
						SELECT *
						FROM   ItemIdentifier II
						WHERE  II.Item_Key = INSERTED.Item_Key
								AND dbo.fn_IsScaleItem(II.Identifier) = 1
								AND II.Scale_Identifier = 1
					) --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
	               
				AND s.WFM_Store = 1 AND si.Authorized = 1 AND
				NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C') AND
				NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C')
	    
			SELECT @error_no = @@ERROR
		END
	
	IF @error_no = 0
		BEGIN -- Insert non-batchable changes when GloCon makes changes to Items

			DECLARE @EnableIconItemNonBatchableChanges bit = 0
			SELECT @EnableIconItemNonBatchableChanges = FlagValue
			FROM dbo.InstanceDataFlags
			WHERE FlagKey = 'EnableIconItemNonBatchableChanges'

			IF(@EnableIconItemNonBatchableChanges = 1)
			BEGIN
				DECLARE @priceBatchStatusIdProcessed int = (select PriceBatchStatusID from dbo.PriceBatchStatus where PriceBatchStatusDesc = 'Processed')

				;WITH changedItems AS
				(
					SELECT
						ins.Item_Key,
						ins.POS_Description,
						ins.Food_Stamps,
						ins.TaxClassID
					FROM INSERTED ins
					JOIN DELETED d on ins.Item_Key = d.Item_Key
					WHERE ins.LastModifiedUser_ID = @IconControllerUserId
						AND ins.Retail_Sale = 1
						AND ins.Remove_Item = 0 
						AND ins.Deleted_Item = 0
						AND ((ins.POS_Description <> d.POS_Description)
							OR (ins.Food_Stamps <> d.Food_Stamps)
							OR (ins.TaxClassID <> d.TaxClassID))
				)

				MERGE dbo.ItemNonBatchableChanges AS inbc
				USING 
					(SELECT * FROM changedItems) AS i
				ON i.Item_Key = inbc.Item_Key
				WHEN MATCHED THEN
					UPDATE SET inbc.POS_Description = i.POS_Description,
								inbc.Food_Stamps = i.Food_Stamps,
								inbc.TaxClassID = i.TaxClassID
				WHEN NOT MATCHED THEN
					INSERT (Item_Key, POS_Description, Food_Stamps, TaxClassID)
					VALUES (i.Item_Key, i.POS_Description, i.Food_Stamps, i.TaxClassID);
				
				;WITH changedItems AS
				(
					SELECT
						ins.Item_Key,
						ins.POS_Description,
						ins.Food_Stamps,
						ins.TaxClassID
					FROM INSERTED ins
					JOIN DELETED d on ins.Item_Key = d.Item_Key
					WHERE ins.LastModifiedUser_ID = @IconControllerUserId
						AND ins.Retail_Sale = 1
						AND ins.Remove_Item = 0 
						AND ins.Deleted_Item = 0
						AND ((ins.POS_Description <> d.POS_Description)
							OR (ins.Food_Stamps <> d.Food_Stamps)
							OR (ins.TaxClassID <> d.TaxClassID))
				)

				UPDATE pbd
				SET POS_Description = i.POS_Description,
					Food_Stamps = i.Food_Stamps
				FROM changedItems i
				JOIN dbo.PriceBatchDetail pbd on pbd.Item_Key = i.Item_Key
				JOIN dbo.PriceBatchHeader pbh on pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
				WHERE pbh.PriceBatchStatusID <> @priceBatchStatusIdProcessed
			END							
		END

	IF @error_no = 0
		BEGIN -- update Item.LastModifiedUser_ID
			UPDATE ITEM
			   SET LastModifiedUser_ID = NULL
			  FROM INSERTED
			 WHERE INSERTED.Item_Key = ITEM.Item_Key
			   AND INSERTED.LastModifiedUser_ID = @IconControllerUserId

			SELECT @error_no = @@ERROR
		END

	IF @error_no <> 0
		BEGIN -- rollback transaction because of error
			ROLLBACK TRAN
			DECLARE @Severity SMALLINT
			SELECT @Severity = ISNULL(
					   (
						   SELECT severity
						   FROM   MASTER.dbo.sysmessages
						   WHERE  ERROR = @error_no
					   ),
					   16
				   )
	    
			RAISERROR 
			(
				'ItemUpdate Trigger failed with @@ERROR: %d',
				@Severity,
				1,
				@error_no
			)
		END
END
GO
CREATE Trigger ItemAdd 
ON [dbo].[Item]
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Queue for Price Modeling if necessary
    INSERT INTO PMProductChg (HierLevel, Item_Key, ItemDescription, ParentID, ParentDescription, ActionID, Status)
    SELECT 'Product', Inserted.Item_Key, Inserted.Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Inserted.SubTeam_No) + '1'), ISNULL(Category_Name, 'NO CATEGORY'), 
           'ADD', 'ACTIVE'
    FROM Inserted
    LEFT JOIN
        ItemCategory
        ON Inserted.Category_ID = ItemCategory.Category_ID
    WHERE Inserted.Retail_Sale = 1
          AND (Inserted.SubTeam_No IN (SELECT SubTeam_No FROM PMSubTeamInclude (nolock)))

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
CREATE TRIGGER ItemDelete
ON [dbo].[Item]
FOR DELETE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMProductChg (HierLevel, Item_Key, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'Product', Deleted.Item_Key, Identifier, Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Deleted.SubTeam_No) + '1'), ISNULL(Category_Name, 'NO CATEGORY'), 
           'DELETE'
    FROM Deleted
        LEFT JOIN
            ItemIdentifier II
            ON Deleted.Item_Key = II.Item_Key AND Default_Identifier = 1
        LEFT JOIN
            ItemCategory
            ON Deleted.Category_ID = ItemCategory.Category_ID
    WHERE Deleted.Retail_Sale = 1
          AND Deleted.SubTeam_No IN (SELECT SubTeam_No FROM PMSubTeamInclude (nolock))
          AND NOT EXISTS (SELECT * FROM PMExcludedItem WHERE Item_Key = Deleted.Item_Key)

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemDelete trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Item] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Item] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Item] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT REFERENCES
    ON OBJECT::[dbo].[Item] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Item] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [IRMAReports]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IMHARole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [ExtractRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Item] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Item] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Item] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [BizTalk]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IConInterface]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Item] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [spice_user]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [NutriChefDataWriter]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Item] TO [iCONReportingRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [IRMAPDXExtractRole]
    AS [dbo];

GO

GRANT SELECT
    ON OBJECT::[dbo].[Item] TO [TibcoDataWriter]
    AS [dbo];
