CREATE FUNCTION [dbo].[fn_VendorCostItemsStores] 
(
	@Vendor_ID int
	,@Date smalldatetime
)
RETURNS @CostData TABLE (
	Item_Key int
	,Store_No int
	,Vendor_ID int
	,UnitCost smallmoney
	,CostUnit_ID int
	,UnitFreight smallmoney
	,FreightUnit_ID int
	,Package_Desc1 decimal(9, 4)
	,MSRP smallmoney
	,StartDate smalldatetime
	,EndDate smalldatetime
)
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	06/11/2008	dave stacey		?		Removed redundant inline sql call for performance reasons.
	01/19/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
										Had to update RETURNS clause after adding line in body of FN that updates the @Date variable (syntax requirement).
										So, we insert into table var and then just return.
*/
BEGIN

	-- Update target cost date that includes any lead-time for the vendor.
	SELECT @Date = @Date + dbo.fn_GetLeadTimeDays(@Vendor_ID)

	INSERT into @CostData
		SELECT 
			Item_Key,
			Store_No,
			Vendor_ID = @Vendor_ID,
			UnitCost,
			VCH.CostUnit_ID,
			UnitFreight = ISNULL(UnitFreight, 0),
			VCH.FreightUnit_ID,
			Package_Desc1,
			MSRP,
			StartDate,
			EndDate
		FROM
			VendorCostHistory VCH (nolock)
			INNER JOIN
			(
				SELECT
					Store_No
					,Item_Key
					,MaxVCHID = (
						SELECT TOP 1
							VendorCostHistoryID
						FROM
							dbo.VendorCostHistory (nolock)
							JOIN dbo.StoreItemVendor (nolock)
								ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						WHERE
							Store_No = SIV.Store_No 
							AND Item_Key = SIV.Item_Key 
							AND Vendor_ID = @Vendor_ID
							AND @Date BETWEEN StartDate AND EndDate
							AND
							(
								DeleteDate IS NULL 
								OR @Date < DeleteDate
							)
						ORDER BY
							Promotional DESC
							,VendorCostHistoryID DESC
					)
				FROM
					StoreItemVendor SIV (nolock)
				WHERE
					Vendor_ID = @Vendor_ID
				GROUP BY
					Store_No, Item_Key
			) MVCH
				ON MVCH.MaxVCHID = VCH.VendorCostHistoryID

	return
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItemsStores] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItemsStores] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItemsStores] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItemsStores] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItemsStores] TO [IRMAReportsRole]
    AS [dbo];

