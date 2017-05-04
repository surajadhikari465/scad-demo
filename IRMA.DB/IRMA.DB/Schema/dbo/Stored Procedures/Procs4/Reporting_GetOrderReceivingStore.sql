-- =============================================
-- Author:		Dave Stacey
-- Create date: 01/15/2009
-- Description:	SP used for parameter list for reports 
-- =============================================
CREATE PROCEDURE [dbo].[Reporting_GetOrderReceivingStore]
AS
BEGIN

	Select V.Store_No, S.Store_Name
	FROM dbo.Vendor V
		JOIN dbo.OrderHeader AS OH ON OH.ReceiveLocation_ID = V.Vendor_ID
		JOIN dbo.Store S on S.Store_No = V.Store_No
	WHERE  (S.Mega_Store = 1 OR S.WFM_Store = 1)
		AND dbo.fn_GetCustomerType(S.Store_No, S.Internal, S.BusinessUnit_ID) = 3 -- Regional  
	GROUP BY V.Store_No, S.Store_Name  
	ORDER BY V.Store_No, S.Store_Name  

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderReceivingStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderReceivingStore] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderReceivingStore] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_GetOrderReceivingStore] TO [IRMAReportsRole]
    AS [dbo];

