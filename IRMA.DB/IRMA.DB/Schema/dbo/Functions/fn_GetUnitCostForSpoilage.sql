-- ==========================================================================
-- Author:		Faisal Ahmed
-- Create date: 11/11/2009
-- Function to calculate item cost for spoilage
-- 
-- ===========================================================================
CREATE FUNCTION [dbo].[fn_GetUnitCostForSpoilage] 
	(@Item_Key int, 
	 @Store_No int, 
	 @SubTeam_No int,
	 @CostDate smalldatetime)
RETURNS smallmoney
AS
BEGIN
	Declare @Cost smallmoney
	Declare @UseAverageCost bit

	Select @UseAverageCost = FlagValue From InstanceDataFlags Where flagKey = 'UseAverageCostForWaste' 
			
	Select @CostDate = isnull(@CostDate, getdate())
        
	-- Chop the time in case we used GETDATE() above      
    SELECT @CostDate = CONVERT(smalldatetime, CONVERT(varchar(255), @CostDate, 101))      

	If (@UseAverageCost = 0)
		Begin
			-- find primary vendor id
			Declare @PrimaryVendorID int

			Select TOP 1 @PrimaryVendorID = siv.Vendor_ID
			From StoreItem si
			Inner Join StoreItemVendor siv on si.Store_No = siv.Store_No and si.Item_Key = siv.Item_Key
			Where si.Item_Key = @Item_key and si.Store_No = @Store_No and siv.PrimaryVendor = 1

			-- calculate cost from the primary vendor
			Select @Cost = (UnitCost - NetDiscount)/isnull(package_Desc1,1)
			from  dbo.fn_VendorCostAll(@CostDate) vc 
			where vc.Item_Key = @Item_Key and vc.Store_No = @Store_No and vc.Vendor_ID = @PrimaryVendorID 
		End
	else
		Begin
            Select @Cost = dbo.fn_AvgCostHistory(@Item_Key, @Store_No, @SubTeam_No, @CostDate)
		End

    Return @Cost
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetUnitCostForSpoilage] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetUnitCostForSpoilage] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetUnitCostForSpoilage] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetUnitCostForSpoilage] TO [IRMASchedJobs]
    AS [dbo];

