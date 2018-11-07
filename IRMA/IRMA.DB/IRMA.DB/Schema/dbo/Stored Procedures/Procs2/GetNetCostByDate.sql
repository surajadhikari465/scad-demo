CREATE PROCEDURE dbo.GetNetCostByDate
	@Item_Key int, 
	@Store_No int,
	@Vendor_ID int, 
	@EffectiveDate datetime
AS
BEGIN
	select * from dbo.fn_getnetcost(@Item_Key, @Store_No, @Vendor_ID, @EffectiveDate)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNetCostByDate] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNetCostByDate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNetCostByDate] TO [IRMASchedJobs]
    AS [dbo];

