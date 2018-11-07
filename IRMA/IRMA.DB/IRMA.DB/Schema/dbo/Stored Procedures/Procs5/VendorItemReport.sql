CREATE PROCEDURE dbo.VendorItemReport 
	@Vendor_ID int,
	@Zone_ID int,
	@Store_No int,
	@Team_No int,
	@SubTeam_No int,
	@Category_ID int,
	@IsRegional int,
	@Brand_ID int,
	@Identifier varchar(14)
AS 

-- **************************************************************************
-- Procedure: VendorItemReport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/11/2013  BAS	Update i.Discontinue_Item to use siv.DiscontinueItem to
--					account for schema change. Renamed file to .sql. Coding Standards.
-- **************************************************************************

BEGIN

SET NOCOUNT ON

    DECLARE @CurrDate smalldatetime
    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))

    DECLARE @AvailStoreItemVendor table(Store_Name Varchar(50), Zone_Name varchar(100), Item_Key int, Vendor_ID int, Cost smallmoney, Price smallmoney, PromotionStatus varchar(7))

	if @Store_no is null
	begin
		INSERT INTO @AvailStoreItemVendor
		SELECT
			Store.Store_Name, 
			Zone.Zone_Name, 
			SIV.Item_Key, 
			SIV.Vendor_ID,
			null as price,
			null as PromotionStatus,
			null
		FROM
			Store (nolock)
			INNER JOIN StoreItemVendor SIV	(nolock) ON SIV.Store_No	= Store.Store_no
			INNER JOIN Zone					(nolock) ON Store.Zone_ID	= Zone.Zone_ID                       
		WHERE
			Store.Zone_ID			= ISNULL(@Zone_ID, Store.Zone_ID)
			AND Vendor_ID			= @Vendor_ID
			AND Store.Store_no		= ISNULL(@Store_No, Store.Store_no)
			AND (SIV.DeleteDate		> CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)) or SIV.DeleteDate is null)
			AND SIV.DiscontinueItem = 0
	end
	else
	begin
		INSERT INTO @AvailStoreItemVendor
		SELECT
			Store.Store_Name, 
			Zone.Zone_Name, 
			SIV.Item_Key, 
			SIV.Vendor_ID,
			VC.UnitCost,
			case when PCT.On_Sale = 1 
			   then
				  dbo.fn_Price(Price.PriceChgTypeID, Price.Multiple, Price.Price, Price.PricingMethod_ID, Price.Sale_Multiple, Price.Sale_Price) 
			   else
				  price.Price
			   end as price,
			PCT.PriceChgTypeDesc as PromotionStatus            
		FROM
			Store (nolock)
			INNER JOIN StoreItemVendor	SIV	(nolock)	ON SIV.Store_No		= Store.Store_no
			INNER JOIN Zone					(nolock)	ON Store.Zone_ID	= Zone.Zone_ID                       
			INNER JOIN fn_VendorCostItemsStores(@Vendor_ID, @CurrDate) VC ON VC.Vendor_ID = SIV.Vendor_ID and VC.Store_No = SIV.Store_No and VC.Item_Key = SIV.Item_Key
			INNER JOIN price							on SIV.Store_no		= Price.Store_no and SIV.Item_Key = Price.Item_Key
			INNER JOIN PriceChgType		PCT				ON PCT.PriceChgTypeID = Price.PriceChgTypeID
		WHERE
		Store.Zone_ID			= ISNULL(@Zone_ID, Store.Zone_ID)
			AND SIV.Vendor_ID		= @Vendor_ID
			AND Store.Store_no		= ISNULL(@Store_No, Store.Store_no)
			AND (SIV.DeleteDate		> CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)) or SIV.DeleteDate is null)
			AND SIV.DiscontinueItem = 0
	 end


	SELECT distinct
		II.Identifier,             
		IB.Brand_Name,
		IV.Item_ID,
		I.Item_Description, 
		I.Package_Desc1, 
		I.Package_Desc2, 
		IU.Unit_Name AS Package_Unit,
		ASIV.Cost,
		ASIV.Price,
		ASIV.PromotionStatus,
		(case when @SubTeam_No is not null
				then ''
				else ST.SubTeam_Name
				END ) as SubTeam_Name,
		(case when (@Zone_ID is null) 
				then '' 
				else ''-- ASIV.Store_Name 
			end) as Store_Name,

		(case when @IsRegional = 1
				then ''
				else case when @Store_No is null 
							then '' -- ASIV.zone_name
							else ''
							end
				end) as Zone_Name
	FROM
		Item								I		(nolock) 
		left join ItemBrand					IB					ON	I.Brand_ID					= IB.Brand_ID
		INNER JOIN @AvailStoreItemVendor	ASIV                ON	I.Item_Key					= ASIV.Item_Key
		INNER JOIN SubTeam					ST		(nolock)	ON	ST.SubTeam_No				= I.SubTeam_No
		INNER JOIN  ItemIdentifier			II		(nolock)	ON	II.Item_Key					= I.Item_Key 
																	AND II.Default_Identifier	=	case
																										when @Identifier is null then 1
																										else default_identifier
																									end
																	AND II.Deleted_Identifier	= 0
		Left JOIN ItemUnit					IU		(nolock)	ON	IU.Unit_ID					= I.Package_Unit_ID
		INNER JOIN ItemVendor				IV					on	IV.Item_Key					= I.Item_Key
	WHERE
		I.Deleted_Item				= 0
		AND ST.Team_No				= ISNULL(@Team_No, ST.Team_No)
		AND ST.SubTeam_No			= ISNULL(@SubTeam_No, ST.SubTeam_No)
		AND ISNULL(Category_ID, 0)	= ISNULL(@Category_ID, ISNULL(Category_ID, 0))
		AND II.Identifier			like isnull(@Identifier,II.identifier)
		AND isnull(I.Brand_ID,0)	= isnull(@Brand_ID,isnull(I.Brand_ID,0))
		AND IV.Vendor_ID			= @Vendor_ID and IV.DeleteDate is null
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorItemReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorItemReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorItemReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VendorItemReport] TO [IRMAReportsRole]
    AS [dbo];

