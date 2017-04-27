IF EXISTS (
       SELECT *
       FROM   sysobjects
       WHERE  TYPE = 'TR'
              AND NAME = 'ItemUpdate'
   )
BEGIN
    PRINT 'Dropping Trigger ItemUpdate'
    DROP TRIGGER ItemUpdate
END
GO


PRINT 'Creating Trigger ItemUpdate'
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
		BEGIN
			-- Queue for Price Modeling if necessary
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
		BEGIN
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
	
	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED
	IF @error_no = 0
		BEGIN
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
				CROSS JOIN (
						SELECT Store_No
						FROM   Store(NOLOCK)
						WHERE  WFM_Store = 1
								OR  Mega_Store = 1
					) Store
			WHERE  
				(INSERTED.Remove_Item = 0 AND INSERTED.Deleted_Item = 0)
				AND (
						-- Don't allow maintenance to be created if Icon is doing the update, unless it's a subteam or package unit update.
						(ISNULL(inserted.LastModifiedUser_ID, 0) <> ISNULL(@IconControllerUserId, 0)) 
						OR (ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND inserted.SubTeam_No <> deleted.SubTeam_No)
						OR (ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND inserted.Package_Desc1 <> deleted.Package_Desc1)
						OR (ISNULL(inserted.LastModifiedUser_ID, 0) = ISNULL(@IconControllerUserId, 0) AND inserted.Package_Unit_ID <> deleted.Package_Unit_ID)
					)
				AND (
						INSERTED.Item_Description <> DELETED.Item_Description
						OR INSERTED.POS_Description <> DELETED.POS_Description
						OR INSERTED.Sign_Description <> DELETED.Sign_Description
						OR INSERTED.Food_Stamps <> DELETED.Food_Stamps
						OR INSERTED.Price_Required <> DELETED.Price_Required
						OR INSERTED.Quantity_Required <> DELETED.Quantity_Required
						OR INSERTED.Organic <> DELETED.Organic
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

	-- If Retail_Sale flag was flipped from 1 to 0 then remove all the identifiers (default and alternate) from the
	-- ValidatedScanCode table.
	IF @error_no = 0
		BEGIN
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

	-- If Retail_Sale flag was flipped from 0 to 1 then insert the Item as a new Item into IconItemChangeQueue so 
	-- that Icon can manage the Item's information.
	IF @error_no = 0
		BEGIN
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
		BEGIN
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
	
	-- Insert non-batchable changes when GloCon makes changes to Items
	IF @error_no = 0
		BEGIN

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
		BEGIN
			UPDATE ITEM
			   SET LastModifiedUser_ID = NULL
			  FROM INSERTED
			 WHERE INSERTED.Item_Key = ITEM.Item_Key
			   AND INSERTED.LastModifiedUser_ID = @IconControllerUserId

			SELECT @error_no = @@ERROR
		END

	IF @error_no <> 0
		BEGIN
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
