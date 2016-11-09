Create FUNCTION dbo.fn_IsPayByAgreedCostStoreVendor
(
	@Store_No int
	,@Vendor_ID int
	,@Date smalldatetime
)
RETURNS bit
AS

BEGIN 

RETURN
	CASE
		WHEN EXISTS 
			   (SELECT * FROM PayOrderedCost PC (nolock) 
			   WHERE PC.Vendor_ID = @Vendor_ID 
			   AND PC.Store_No = @Store_No 
			   AND isnull(@Date, getdate()) >= PC.BeginDate) 
		THEN 1
		ELSE 0
	END			        
	 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsPayByAgreedCostStoreVendor] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsPayByAgreedCostStoreVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsPayByAgreedCostStoreVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsPayByAgreedCostStoreVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsPayByAgreedCostStoreVendor] TO [IRMAReportsRole]
    AS [dbo];

