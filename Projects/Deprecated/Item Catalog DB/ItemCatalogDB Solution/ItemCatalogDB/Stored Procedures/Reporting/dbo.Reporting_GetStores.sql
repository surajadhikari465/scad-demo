
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reporting_GetStores]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reporting_GetStores]
GO

CREATE PROCEDURE dbo.Reporting_GetStores
	@blnAll	AS BIT=NULL
AS
   -- **************************************************************************
   -- Procedure: Reporting_GetStores
   --    Author: Hussain Hashim
   --      Date: 8/23/2007
   --
   -- Description:
   -- To be used for parameter list for reports.  
   -- Parameter is used if All needs to be included for reports (to have all Stores selected)

   -- Modification History:
   -- Date        Init	Comment
   -- 11/06/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- **************************************************************************

BEGIN
	

IF @blnAll = 1
BEGIN
    SELECT	' All' AS Store_Name,
			' All' AS Store_No
	UNION
    SELECT Store.Store_Name,
		CONVERT(VARCHAR, Store.Store_No)
    FROM Store (NOLOCK) 
	INNER JOIN 
		Zone (NOLOCK) 
		ON Store.Zone_Id = Zone.Zone_Id
    LEFT JOIN 
		Vendor (nolock) 
		ON Store.Store_No = Vendor.Store_No    
	LEFT JOIN
		POSSystemTypes (nolock)
		ON POSSystemTypes.POSSystemId = Store.POSSystemId
	LEFT JOIN 
		StoreRegionMapping SRM (nolock)
		ON store.store_no = SRM.store_No
    WHERE (Mega_Store = 1 OR WFM_Store = 1)
		AND dbo.fn_GetCustomerType(Store.Store_No, Internal, Store.BusinessUnit_ID) = 3 -- Regional    
    ORDER BY Store_Name
END
ELSE
BEGIN
    SELECT Store_Name,
		Store.Store_No
    FROM Store (NOLOCK) 
	INNER JOIN 
		Zone (NOLOCK) 
		ON Store.Zone_Id = Zone.Zone_Id
    LEFT JOIN 
		Vendor (nolock) 
		ON Store.Store_No = Vendor.Store_No    
	LEFT JOIN
		POSSystemTypes (nolock)
		ON POSSystemTypes.POSSystemId = Store.POSSystemId
	LEFT JOIN 
		StoreRegionMapping SRM (nolock)
		ON store.store_no = SRM.store_No
    WHERE (Mega_Store = 1 OR WFM_Store = 1)
		AND dbo.fn_GetCustomerType(Store.Store_No, Internal, Store.BusinessUnit_ID) = 3 -- Regional    
    ORDER BY Store_Name
END

END
GO
