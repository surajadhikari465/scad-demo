/****** Object:  StoredProcedure [dbo].[GetStoreJurisdictions]  ******/
CREATE PROCEDURE dbo.GetStoreJurisdictions AS

-- ****************************************************************************************************************
-- Procedure: GetStoreJurisdictions()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Returns jurisdiction information.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2012/12/27	KM		9251	Add update history template; Order the results by StoreJurisdictionID;
-- ****************************************************************************************************************

SELECT 
	StoreJurisdictionID, 
	StoreJurisdictionDesc 
FROM 
	StoreJurisdiction 
ORDER BY 
	StoreJurisdictionID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreJurisdictions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreJurisdictions] TO [IRMAClientRole]
    AS [dbo];

