
/****** Object:  StoredProcedure [dbo].[Reporting_GetOrderReceivingStore]    Script Date: 01/15/2009 17:33:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reporting_GetOrderReceivingStore]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Reporting_GetOrderReceivingStore]
GO
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