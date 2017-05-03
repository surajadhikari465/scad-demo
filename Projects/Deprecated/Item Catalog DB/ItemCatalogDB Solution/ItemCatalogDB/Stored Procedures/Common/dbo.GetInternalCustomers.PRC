SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInternalCustomers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetInternalCustomers]
GO

CREATE PROCEDURE dbo.GetInternalCustomers
    @Exception_ID int = 0
AS 
	-- **************************************************************************
	-- Procedure: GetInternalCustomers()
	--    Author: n/a
	--      Date: n/a
	--
	-- Description:
	-- This procedure is called from Global.vb to load values across the code base.
	--
	-- Modification History:
	-- Date        Init	Comment
	-- 11/17/2009  BBB	converted call to Store to Left Join so that the exception
	--					id was truely treated as an exception; reformatted for
	--					readability;
	-- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		Vendor_ID, 
		CompanyName
    FROM 
		Vendor				(nolock) 
		LEFT JOIN	Store	(nolock) ON Vendor.Store_No = Store.Store_No
    WHERE 
		dbo.fn_GetCustomerType(Vendor.Store_No, Store.Internal, Store.BusinessUnit_ID) IN (2,3) -- WFM or Regional
        OR 
		Vendor_ID = @Exception_ID
    ORDER BY 
		Mega_Store DESC, 
		WFM_Store DESC, 
		Distribution_Center DESC, 
		Manufacturer DESC, 
		CompanyName
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


