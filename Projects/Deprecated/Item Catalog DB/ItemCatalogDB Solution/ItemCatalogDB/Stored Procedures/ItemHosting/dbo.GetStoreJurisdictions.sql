/****** Object:  StoredProcedure [dbo].[GetStoreJurisdictions]  ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetStoreJurisdictions]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetStoreJurisdictions]
GO

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