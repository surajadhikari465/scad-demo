CREATE VIEW [dbo].[EIM_Jurisdiction_ItemView] AS

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes
DN		2013.01.03	8755	Updated Disco Ordering Logic
KM		2013-01-008	9251	Select the new 4.8 ItemOverride columns in the ItemOverride 
							portion of the UNION;
MZ      2017-07-21  22360   Added two alternate jurisdiction fields Sign Romance Short and Sign Romance Long 
                            to EIM
***********************************************************************************************/

SELECT
                itm.Item_Key
                ,itmo.Item_Description
                ,itmo.Sign_Description
                ,itm.Ingredients As Ingredients
                ,itm.SubTeam_No As SubTeam_No
                ,itm.Sales_Account As Sales_Account
                ,itmo.Package_Desc1
                ,itmo.Package_Desc2
                ,itmo.Package_Unit_ID
                ,itm.Min_Temperature As Min_Temperature
                ,itm.Max_Temperature As Max_Temperature
                ,itm.Units_Per_Pallet As Units_Per_Pallet
                ,itm.Average_Unit_Weight As Average_Unit_Weight
                ,itm.Tie As Tie
                ,itm.High As High
                ,itm.Yield As Yield
                ,itmo.Brand_ID As Brand_ID
                ,itm.Category_ID As Category_ID
                ,itmo.Origin_ID As Origin_ID
                ,itm.ShelfLife_Length As ShelfLife_Length
                ,itm.ShelfLife_ID As ShelfLife_ID
                ,itmo.Retail_Unit_ID
                ,itmo.Vendor_Unit_ID
                ,itmo.Distribution_Unit_ID
                ,itm.Cost_Unit_ID As Cost_Unit_ID
                ,itm.Freight_Unit_ID As Freight_Unit_ID
                ,itm.Deleted_Item As Deleted_Item
                ,dbo.fn_GetDiscontinueStatus(itm.Item_Key, NULL, NULL) As Discontinue_Item
                ,itm.WFM_Item As WFM_Item
                ,itmo.Not_Available As Not_Available
                ,itm.Pre_Order As Pre_Order
                ,itm.Remove_Item As Remove_Item
                ,itm.NoDistMarkup As NoDistMarkup
                ,itm.Organic As Organic
                ,itm.Refrigerated As Refrigerated
                ,itm.Keep_Frozen As Keep_Frozen
                ,itm.Shipper_Item As Shipper_Item
                ,itm.Full_Pallet_Only As Full_Pallet_Only
                ,itm.User_ID As User_ID
                ,itmo.POS_Description
                ,itm.Retail_Sale As Retail_Sale
                ,itmo.Food_Stamps
                ,itmo.Price_Required
                ,itmo.Quantity_Required
                ,itm.ItemType_ID As ItemType_ID
                ,itm.HFM_Item As HFM_Item
                ,itm.ScaleDesc1 As ScaleDesc1
                ,itm.ScaleDesc2 As ScaleDesc2
                ,itmo.Not_AvailableNote As Not_AvailableNote
                ,itmo.CountryProc_ID As CountryProc_ID
                ,itm.Insert_Date As Insert_Date
                ,itmo.Manufacturing_Unit_ID
                ,itm.EXEDistributed As EXEDistributed
                ,itm.ClassID As ClassID
                ,itm.User_ID_Date As User_ID_Date
                ,itm.DistSubTeam_No As DistSubTeam_No
                ,itmo.CostedByWeight As CostedByWeight
                ,itm.TaxClassID As TaxClassID
                ,itmo.LabelType_ID As LabelType_ID
                ,itm.ScaleDesc3 As ScaleDesc3
                ,itm.ScaleDesc4 As ScaleDesc4
                ,itm.ScaleTare As ScaleTare
                ,itm.ScaleUseBy As ScaleUseBy
                ,itm.ScaleForcedTare As ScaleForcedTare
                ,itmo.QtyProhibit
                ,itmo.GroupList
                ,itm.ProdHierarchyLevel4_ID As ProdHierarchyLevel4_ID
                ,itmo.Case_Discount
                ,itmo.Coupon_Multiplier
                ,itmo.Misc_Transaction_Sale
                ,itmo.Misc_Transaction_Refund
                ,itmo.Recall_Flag As Recall_Flag
                ,itm.Manager_ID As Manager_ID
                ,itmo.Ice_Tare
                ,itmo.LockAuth As LockAuth
                ,itm.PurchaseThresholdCouponAmount As PurchaseThresholdCouponAmount
                ,itm.PurchaseThresholdCouponSubTeam As PurchaseThresholdCouponSubTeam
                ,itmo.Product_code As Product_code
                ,itmo.Unit_price_category As Unit_price_category
                ,itmo.StoreJurisdictionID
                ,CAST(0 As bit) As IsDefaultJurisdiction
                ,itm.CatchweightRequired
				,itm.COOL
                ,itm.BIO
				,itmo.Ingredient
				,itmo.SustainabilityRankingRequired
				,itmo.SustainabilityRankingID
				,itmo.FSA_Eligible
				,itm.UseLastReceivedCost
				,itmo.SignRomanceTextLong
				,itmo.SignRomanceTextShort
        
		FROM 
			dbo.Item itm (NOLOCK)
            INNER JOIN dbo.ItemOverride itmo (NOLOCK) ON itmo.Item_Key = itm.Item_Key

        UNION

        SELECT
                itm.Item_Key
                ,itm.Item_Description
                ,itm.Sign_Description
                ,itm.Ingredients As Ingredients
                ,itm.SubTeam_No As SubTeam_No
                ,itm.Sales_Account As Sales_Account
                ,itm.Package_Desc1
                ,itm.Package_Desc2
                ,itm.Package_Unit_ID
                ,itm.Min_Temperature As Min_Temperature
                ,itm.Max_Temperature As Max_Temperature
                ,itm.Units_Per_Pallet As Units_Per_Pallet
                ,itm.Average_Unit_Weight As Average_Unit_Weight
                ,itm.Tie As Tie
                ,itm.High As High
                ,itm.Yield As Yield
                ,itm.Brand_ID As Brand_ID
                ,itm.Category_ID As Category_ID
                ,itm.Origin_ID As Origin_ID
                ,itm.ShelfLife_Length As ShelfLife_Length
                ,itm.ShelfLife_ID As ShelfLife_ID
                ,itm.Retail_Unit_ID
                ,itm.Vendor_Unit_ID
                ,itm.Distribution_Unit_ID
                ,itm.Cost_Unit_ID As Cost_Unit_ID
                ,itm.Freight_Unit_ID As Freight_Unit_ID
                ,itm.Deleted_Item As Deleted_Item
                ,dbo.fn_GetDiscontinueStatus(itm.Item_Key, NULL, NULL) As Discontinue_Item
                ,itm.WFM_Item As WFM_Item
                ,itm.Not_Available As Not_Available
                ,itm.Pre_Order As Pre_Order
                ,itm.Remove_Item As Remove_Item
                ,itm.NoDistMarkup As NoDistMarkup
                ,itm.Organic As Organic
                ,itm.Refrigerated As Refrigerated
                ,itm.Keep_Frozen As Keep_Frozen
                ,itm.Shipper_Item As Shipper_Item
                ,itm.Full_Pallet_Only As Full_Pallet_Only
                ,itm.User_ID As User_ID
                ,itm.POS_Description
                ,itm.Retail_Sale As Retail_Sale
                ,itm.Food_Stamps
                ,itm.Price_Required
                ,itm.Quantity_Required
                ,itm.ItemType_ID As ItemType_ID
                ,itm.HFM_Item As HFM_Item
                ,itm.ScaleDesc1 As ScaleDesc1
                ,itm.ScaleDesc2 As ScaleDesc2
                ,itm.Not_AvailableNote As Not_AvailableNote
                ,itm.CountryProc_ID As CountryProc_ID
                ,itm.Insert_Date As Insert_Date
                ,itm.Manufacturing_Unit_ID
                ,itm.EXEDistributed As EXEDistributed
                ,itm.ClassID As ClassID
                ,itm.User_ID_Date As User_ID_Date
                ,itm.DistSubTeam_No As DistSubTeam_No
                ,itm.CostedByWeight As CostedByWeight
                ,itm.TaxClassID As TaxClassID
                ,itm.LabelType_ID As LabelType_ID
                ,itm.ScaleDesc3 As ScaleDesc3
                ,itm.ScaleDesc4 As ScaleDesc4
                ,itm.ScaleTare As ScaleTare
                ,itm.ScaleUseBy As ScaleUseBy
                ,itm.ScaleForcedTare As ScaleForcedTare
                ,itm.QtyProhibit
                ,itm.GroupList
                ,itm.ProdHierarchyLevel4_ID As ProdHierarchyLevel4_ID
                ,itm.Case_Discount
                ,itm.Coupon_Multiplier
                ,itm.Misc_Transaction_Sale
                ,itm.Misc_Transaction_Refund
                ,itm.Recall_Flag As Recall_Flag
                ,itm.Manager_ID As Manager_ID
                ,itm.Ice_Tare
                ,itm.LockAuth As LockAuth
                ,itm.PurchaseThresholdCouponAmount As PurchaseThresholdCouponAmount
                ,itm.PurchaseThresholdCouponSubTeam As PurchaseThresholdCouponSubTeam
                ,itm.Product_code As Product_code
                ,itm.Unit_price_category As Unit_price_category
                ,itm.StoreJurisdictionID
                ,CAST(1 As bit) As IsDefaultJurisdiction
                ,itm.CatchweightRequired
                ,itm.COOL
                ,itm.BIO
				,itm.Ingredient
				,itm.SustainabilityRankingRequired
				,itm.SustainabilityRankingID
				,itm.FSA_Eligible
				,itm.UseLastReceivedCost
				,isa.SignRomanceTextLong
				,isa.SignRomanceTextShort
        
		FROM 
			dbo.Item itm (NOLOCK)
			LEFT JOIN itemsignattribute isa (NOLOCK) on itm.Item_Key = isa.Item_Key
GO
GRANT SELECT
    ON OBJECT::[dbo].[EIM_Jurisdiction_ItemView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EIM_Jurisdiction_ItemView] TO [IRMAReportsRole]
    AS [dbo];

