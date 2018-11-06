if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_VendorCost]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_VendorCost]
GO

CREATE FUNCTION dbo.fn_VendorCost 
(
	@Item_Key int
	,@Vendor_ID int
	,@Store_No int
	,@Date smalldatetime
)
RETURNS @CostData TABLE (
	UnitCost smallmoney
	,UnitFreight smallmoney
	,Package_Desc1 decimal(9, 4)
	,MSRP smallmoney
	,StartDate smalldatetime
	,EndDate smalldatetime
	,CostUnit_ID int
	,VendorCostHistoryID int
)
AS
/*
	[ Modification History ]
	--------------------------------------------
	Date		Developer		TFS		Comment
	--------------------------------------------
	01/19/2011	Tom Lux			759		Updated to include any vendor lead-time in the date used to pull the cost value.  Reformatted.
										Had to update RETURNS clause after adding line in body of FN that updates the @Date variable (syntax requirement).
										So, we insert into table var and then just return.
*/
BEGIN

	-- Update target cost date that includes any lead-time for the vendor.
	SELECT @Date = @Date + dbo.fn_GetLeadTimeDays(@Vendor_ID)

	INSERT into @CostData
		SELECT TOP 1
			UnitCost
			,UnitFreight = ISNULL(UnitFreight, 0)
			,Package_Desc1
			,MSRP
			,StartDate
			,EndDate
			,CostUnit_ID
			,VendorCostHistoryID
		FROM 
			VendorCostHistory VCH (nolock)
			INNER JOIN StoreItemVendor SIV (nolock)
				ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
		WHERE
			Item_Key = @Item_Key 
			AND Vendor_ID = @Vendor_ID 
			AND Store_No = @Store_No 
			AND (
				(@Date >= StartDate)
				AND (@Date <= EndDate)
			)
			AND @Date < ISNULL(DeleteDate, DATEADD(day, 1, @Date))
		ORDER BY
			Promotional DESC
			,VendorCostHistoryID DESC

	return
END
GO
