SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoresAndDist]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoresAndDist]
GO
CREATE PROCEDURE dbo.GetStoresAndDist

-- ****************************************************************************************************************
-- Procedure: GetStoresAndDist()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- Returns all retail stores along with distribution and manufacturing facilities
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012-03-16	KM		5323	Added Store.BusinessUnit_ID to result set (needed for EInvoicing_ViewInvoices.vb); Coding standards; Extension change to .sql;
-- ****************************************************************************************************************

AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		s.StoreAbbr, 
		z.Zone_id, 
		z.Zone_Name, 
		s.Store_Name, 
		s.Mega_Store, 
		s.WFM_Store, 
		z.Region_id, 
		s.Store_No, 
		s.BusinessUnit_ID,
		v.State,
    	CustomerType = dbo.fn_getCustomerType(s.Store_No, s.Internal, s.BusinessUnit_ID), -- 3 = Regional
		pst.POSSystemId, 
		pst.POSSystemType, 
		srm.Region_Code

    FROM 
        Zone							(nolock)	z
        INNER JOIN	Store				(nolock)	s	ON	z.Zone_Id		= s.Zone_Id
        LEFT JOIN	Vendor				(nolock)	v	ON	s.Store_No		= v.Store_No 
		LEFT JOIN	POSSystemTypes		(nolock)	pst	ON	s.POSSystemId	= pst.POSSystemId
		LEFT JOIN	StoreRegionMapping	(nolock)	srm	ON	s.store_no		= srm.store_No            
            
    WHERE 
		(Mega_Store = 1 OR WFM_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1)
    
    GROUP BY 
		z.Zone_Id, 
		s.Store_No, 
		z.Zone_Name, 
		s.Store_Name, 
		z.Region_Id, 
		s.Mega_Store, 
		s.WFM_Store, 
		s.BusinessUnit_ID,
		v.State, 
		s.StoreAbbr, 
		dbo.fn_getCustomerType(s.Store_No, s.Internal, s.BusinessUnit_ID), 
		pst.POSSystemId, 
		pst.POSSystemType,
		srm.Region_Code
    
    ORDER BY 
		z.Zone_Id, 
		s.Store_Name, 
		s.Store_No, 
		z.Zone_Name, 
		z.Region_Id, 
		s.Mega_Store, 
		s.WFM_Store
     
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO