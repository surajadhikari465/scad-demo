CREATE VIEW [dbo].[SLIM_ItemView] 
AS
	-- ********************************************************************************************************************************
	-- Procedure: SLIM_ItemView()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from both SLIM and EIM.
	--
	-- Modification History:
	-- Date			Init	TFS		Comment
	-- 08/27/2010	BBB		13358	added ingredient, sustainabilityrankingrequired,
	--								sustainabilityrankingid columns to output for EIM SLIM search
	-- 02/19/2009	AZ		xxxxx	Add two columns (cool, bio) to this View
	-- 01/20/2011   AZ		13846	Added FSA Eligible column. Not in the SLIM item request table yet, default to NULL.
	-- 10/12/2012	KM		xxxxx	Select UseLastReceivedCost column;
	-- ********************************************************************************************************************************
	SELECT
			ItemRequest.ItemRequest_ID as item_key,
			item_description as item_description,
			brand_id as brand_id,
			category_id as category_id,
			classid as classid,
			costedbyweight as costedbyweight,
			CAST(CASE WHEN countryofproc = 0 THEN NULL ELSE countryofproc END As int) as countryproc_id,
			distributionsubteam as distsubteam_no,
			distributionunits as distribution_unit_id,
			foodstamp as food_stamps,
			CAST(itemsize As decimal(9, 4)) as package_desc2,
			CAST(itemunit As int) as package_unit_id,
			keepfrozen as keep_frozen,
			manufacturingunits as manufacturing_unit_id,
			notavailable as not_available,
			notavailablenote as not_availablenote,
			organic as organic,
			CAST(CASE WHEN origin = 0 THEN NULL ELSE origin END As int) as origin_id,
			CAST(packsize As decimal(9, 4)) as package_desc1,
			pos_description as pos_description,
			pricerequired as price_required,
			quantityprohibit as qtyprohibit,
			quantityrequired as quantity_required,
			refrigerated as refrigerated,
			retailunits as retail_unit_id,
			shelflabeltype as labeltype_id,
			subteam_no as subteam_no,
			CAST(taxclass_id As int) as taxclassid,
			vendorunits as vendor_unit_id,
			NULL as Deleted_Item,
			NULL as Remove_Item,
			CAST(NULL AS decimal(9, 4)) as average_unit_weight,
			CAST(NULL as bit) as discontinue_item,
			CAST(0 as bit) as full_pallet_only,
			CAST(NULL as int) as grouplist,
			CAST(0 as tinyint) as high,
			CAST(NULL as varchar(3500)) as ingredients,
			CAST(NULL as varchar(3500)) as ingredient,
			CAST(NULL as datetime) as insert_date,
			CAST(NULL as bit) as lockauth,
			CAST(0 as smallint) as max_temperature,
			CAST(0 as smallint) as min_temperature,
			CAST(NULL as bit) as pre_order,
			CAST(NULL as int) as prodhierarchylevel4_id,
			CAST(NULL as bit) as retail_sale,
			CAST(NULL as bit) as shipper_item,
			CAST(NULL as varchar(60)) as sign_description,
			CAST(0 as tinyint) as tie,
			CAST(0 as smallint) as units_per_pallet,
			CAST(100 as decimal(9, 4)) as yield,
			CAST(1 as tinyint) as manager_id,
			CAST(NULL as varchar(15)) As Product_code,
			CAST(NULL as bit) as Case_Discount,
			CAST(NULL as int) As Unit_price_category,
			CAST(NULL as bit) as sustainabilityrankingrequired,
			CAST(NULL as int) As sustainabilityrankingid,
			CAST(1 as bit) as isdefaultjurisdiction,
			Store.storejurisdictionid as storejurisdictionid,
			CatchWeightRequired as catchweightrequired,
			Cool as Cool,
			Bio as Bio,
			CAST(NULL as bit) as FSA_Eligible,
			CAST(NULL AS BIT) AS UseLastReceivedCost

	FROM ItemRequest (NOLOCK)
			LEFT JOIN ItemScaleRequest (NOLOCK)
					ON ItemScaleRequest.ItemRequest_ID = ItemRequest.ItemRequest_ID
			INNER JOIN Store (NOLOCK)
					ON Store.Store_No = ItemRequest.User_Store
	
	WHERE ItemStatus_ID = 2
GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_ItemView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SLIM_ItemView] TO [IRMAReportsRole]
    AS [dbo];

