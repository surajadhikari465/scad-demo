CREATE FUNCTION [dbo].[fn_VendorCostItems] 
(
	@Vendor_ID int
	,@Store_No int
	,@Date smalldatetime -- Caller must make sure date has no time.
)
RETURNS @CostData TABLE (
	Item_Key int
	,UnitCost smallmoney
	,UnitFreight smallmoney
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
			Item_Key
			,UnitCost
			,UnitFreight = ISNULL(UnitFreight, 0)
			,Package_Desc1
			,MSRP
			,StartDate
			,EndDate
		FROM 
			VendorCostHistory VCH (nolock)
			INNER JOIN
			(
				SELECT
					Item_Key
					,MaxVCHID = (
						SELECT TOP 1
							VendorCostHistoryID
						FROM
							dbo.VendorCostHistory (nolock)
							JOIN dbo.StoreItemVendor (nolock)
								ON StoreItemVendor.StoreItemVendorID = VendorCostHistory.StoreItemVendorID
						WHERE
							Store_No = @Store_No 
							AND Item_Key = SIV.Item_Key 
							AND Vendor_ID = @Vendor_ID
							AND @Date BETWEEN StartDate AND EndDate
							AND (
								DeleteDate IS NULL 
								OR @Date < DeleteDate
							)
						ORDER BY
							Promotional DESC
							,VendorCostHistoryID DESC
					)
				FROM
					dbo.StoreItemVendor SIV (nolock)
				WHERE
					Vendor_ID = @Vendor_ID
					AND Store_No = @Store_No
				GROUP BY
					Item_Key
			) MVCH
				ON MVCH.MaxVCHID = VCH.VendorCostHistoryID

	return
END
GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItems] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[fn_VendorCostItems] TO [IRMAReportsRole]
    AS [dbo];

