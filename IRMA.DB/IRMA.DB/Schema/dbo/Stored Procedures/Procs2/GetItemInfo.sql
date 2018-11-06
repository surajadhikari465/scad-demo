
CREATE PROCEDURE [dbo].[GetItemInfo]
    @Item_Key	int,
    @User_ID	int

AS 

/*********************************************************************************************
CHANGE LOG
DEV		DATE		TASK	Description
----------------------------------------------------------------------------------------------
DBS		20110125	1241	Merge Up FSA Changes
BAS		20130103	8755	Removed reference to Item.Discontinue_Item since it was moved
							to StoreItemVendor. Also updated query to coding standards and
							changed extension from .PRC to .sql
KM		2013-04-14	11774	Add PCMSGiftCard to the selection set;
KM		2013-04-30	11774	Rename PCMSGiftCard to just GiftCard;
BJ		2014-04-04	1873	Add an IsValidated value to the result of this SPROC
DN		2014-10-08	15447	Add function dbo.fn_ValidatedScanCodeExists to assign to IsValidated
MZ      2015-08-20  16352 (10976) Add check to see if the item has any primary or alternate identifiers 
                                  that are reserved identifiers for non-retail ingredient items
JA		2016-08-19	20599 (17483) Added Not available for 365 field 
MZ      2017-12-14  23567   Added Subteam Name to the query.
***********************************************************************************************/

BEGIN
   
	UPDATE Item SET 
		User_ID = @User_ID, User_ID_Date = GETDATE()
    WHERE 
		Item_Key = @Item_Key AND User_ID IS NULL
    
	SELECT
		Identifier,
		Item_Description,
		Sign_Description,
		i.SubTeam_No,
		st.SubTeam_Name,
		Sales_Account,
		Package_Desc1,
		Package_Desc2, 
		Package_Unit_ID,
		Min_Temperature,
		Max_Temperature,
		Units_Per_Pallet,
		Average_Unit_Weight,
		Tie,
		High,
		Yield,
		i.Brand_ID, 
		Category_ID,
		l4.ProdHierarchyLevel3_ID,
		i.ProdHierarchyLevel4_ID,
		Origin_ID,
		CountryProc_ID,
		Retail_Unit_ID, 
		Vendor_Unit_ID,
		Distribution_Unit_ID,
		Deleted_Item,
		WFM_Item, 
		i.Not_Available,
		Pre_Order,
		Remove_Item, 
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
		UserName,
		i.Item_Key,
		HFM_Item,
		[Primary_Vendor_Count]		= (	SELECT COUNT(DISTINCT Vendor_ID) 
										FROM dbo.StoreItemVendor (NOLOCK)
										WHERE
											Item_Key = @Item_Key
											AND PrimaryVendor = 1),            
		Not_AvailableNote,
		Manufacturing_Unit_ID, 
		i.EXEDistributed,
		[IsEXEDistributed]			= dbo.fn_IsEXEDistributed(@Item_Key),
		[IsSubTeamEXEDistributed]	= st.EXEDistributed,
		i.User_ID,
		[User_ID_Date]				= CONVERT(varchar(255),ISNULL(User_ID_Date, ''), 121),
		[Insert_Date]				= CONVERT(varchar(255), ISNULL(Insert_Date, ''), 121),
		i.ClassID,
		[DistSubTeam_No]			= ISNULL(i.DistSubTeam_No, 0),
		i.CostedByWeight,
		i.TaxClassID,
		i.LabelType_ID,
		[ScaleIdentifierCount]		= (	SELECT COUNT(*)
										FROM dbo.ItemIdentifier (NOLOCK)
										WHERE 
											Item_Key = i.Item_Key 
											AND (dbo.fn_IsScaleItem(Identifier) = 1)),
		ScaleDesc1,
		ScaleDesc2,
		ScaleDesc3,
		ScaleDesc4,
		Ingredients,
		ShelfLife_Length,
		ShelfLife_ID,
		ScaleTare,
		ScaleUseBy,
		ScaleForcedTare,
		QtyProhibit,
		GroupList,
		Case_Discount,
		Coupon_Multiplier,
		FSA_Eligible,
		Misc_Transaction_Sale,
		Misc_Transaction_Refund,
		Ice_Tare,
		Recall_Flag,
		Manager_ID,
		LockAuth,
		PurchaseThresholdCouponSubTeam,
		PurchaseThresholdCouponAmount,
		i.StoreJurisdictionID,
		SJ.StoreJurisdictionDesc,
		Product_Code,
		Unit_Price_Category,
		CatchweightRequired,
		[FacilityHandlingCharge]			= dbo.fn_GetFacilityHandlingCharge(@Item_Key,null),
		[FacilityHandlingChargeOverride]	= dbo.fn_GetFacilityHandlingChargeOverride(@Item_Key,null), 
		COOL,
		BIO,
		Ingredient,
		SustainabilityRankingRequired,
        SustainabilityRankingID,
		UseLastReceivedCost,
		GiftCard,
		IsValidated = (SELECT dbo.fn_ValidatedScanCodeExists(@Item_Key)),
		HasIngredientIdentifier = (SELECT dbo.fn_HasIngredientIdentifier(@Item_Key)),
		ISNULL(iov.Not_Available, 0) AS Not_Available_365

	FROM
		dbo.Item								i	(NOLOCK)
		INNER JOIN dbo.ItemIdentifier			ii	(NOLOCK) ON ii.Item_Key					= i.Item_Key
																AND ii.Default_Identifier	= 1
		LEFT OUTER JOIN dbo.ProdHierarchyLevel4 l4	(NOLOCK) ON l4.ProdHierarchyLevel4_ID	= i.ProdHierarchyLevel4_ID
		INNER JOIN dbo.SubTeam					st	(NOLOCK) ON st.SubTeam_No				= i.SubTeam_No
		LEFT JOIN  dbo.Users					u	(NOLOCK) ON u.User_ID					= i.User_ID
		LEFT JOIN dbo.StoreJurisdiction			SJ	(NOLOCK) ON SJ.StoreJurisdictionID		= i.StoreJurisdictionID
		LEFT JOIN dbo.ItemOverride365			iov (NOLOCK) ON iov.Item_Key				= i.Item_Key
	WHERE
		i.Item_Key = @Item_Key
    
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfo] TO [IRMAReportsRole]
    AS [dbo];

