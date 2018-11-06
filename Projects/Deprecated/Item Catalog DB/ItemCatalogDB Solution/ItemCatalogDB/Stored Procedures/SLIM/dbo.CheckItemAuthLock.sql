if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckItemAuthLock]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckItemAuthLock]
GO

CREATE procedure dbo.CheckItemAuthLock
    @Item_Key int

AS

-- ****************************************************************************************************************
-- Procedure: CheckItemAuthLock()
--    Author: unknown
--      Date: unknown
--
-- Description:
-- Used in SLIM to check each jurisdiction's LockAuth status for a given item.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-04-09	KM		11745	Check all jurisdictions' LockAuth status;
-- 2013-09-10   FA		13661	Add transaction isolation level
-- ****************************************************************************************************************

BEGIN
	
	DECLARE @AltJurisdictions TABLE (StoreJurisdictionId INT)

	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	INSERT INTO @AltJurisdictions SELECT sj.StoreJurisdictionID FROM StoreJurisdiction sj WHERE StoreJurisdictionID <> 1

	-- For the first part of the union, select the LockAuth status of the primary (US) jurisdiction.
	SELECT
		i.StoreJurisdictionID,
		sj.StoreJurisdictionDesc,
		ISNULL(i.LockAuth, 0) AS LockAuth
	FROM
		Item i
		INNER JOIN StoreJurisdiction sj ON i.StoreJurisdictionID = sj.StoreJurisdictionID
	WHERE
		i.Item_Key = @Item_Key

	UNION

	-- For the 2nd part of the union, select the LockAuth status of every other alternate jurisdiction.
	SELECT
		aj.StoreJurisdictionId,
		sj.StoreJurisdictionDesc,
		ISNULL(iov.LockAuth, 0) AS LockAuth
	FROM
		ItemOverride iov 
		inner join StoreJurisdiction sj on iov.StoreJurisdictionID = sj.StoreJurisdictionID
		inner join @AltJurisdictions aj on aj.StoreJurisdictionId = iov.StoreJurisdictionID
	WHERE
		iov.Item_Key = @Item_Key
	
	COMMIT TRAN
END
GO