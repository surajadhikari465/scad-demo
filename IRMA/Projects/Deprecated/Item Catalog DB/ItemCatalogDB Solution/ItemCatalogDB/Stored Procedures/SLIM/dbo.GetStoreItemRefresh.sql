
/****** Object:  StoredProcedure [dbo].[GetStoreItemRefresh]    Script Date: 03/24/2010 13:23:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetStoreItemRefresh]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetStoreItemRefresh]
GO

CREATE PROCEDURE [dbo].[GetStoreItemRefresh]
		@Item_Key int

AS
	-- **************************************************************************
	-- Procedure: GetStoreItemRefresh()
	--    Author: Billy Blackerby 
	--      Date: 12/29/2009
	--
	-- Description:
	-- This procedure is called from a SLIM.RefreshPOS.
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 12/29/2009	BBB		Created
	-- 03/24/2009	BSR		Added Refresh Column Name and added this to the DB 
	--						solution since someone thought it would be cool to remove it
	-- 03/18/2011   VAA     Added Authorized to disable checkbox if item is not auth for the store
	-- 2013-04-09	KM		Add StoreJurisdictionID to the result set;
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************

BEGIN

	SET NOCOUNT ON;
	
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	SELECT 
		s.Store_No,
		s.Store_Name,
		s.StoreJurisdictionID, 
		ISNULL(si.Refresh, 0) AS Refresh,
			(SELECT TOP 1 
				StoreItemVendorID
			FROM
				StoreItemVendor
			WHERE
				Store_No			= s.Store_No
				AND Item_Key		= @Item_Key
				AND PrimaryVendor	= 1) AS StoreItemVendorID,
		-- bug # 1666 disable checkbox if item is not auth for the store
		si.Authorized
	FROM 
		Store						(nolock) s
		LEFT OUTER JOIN StoreItem	(nolock) si ON s.Store_No = si.Store_No
	WHERE 
		Item_Key = @Item_Key
		AND s.Distribution_Center = 0
		AND s.Regional = 0
		AND (s.EXEWarehouse = 0 OR s.EXEWarehouse IS NULL)
		AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
	ORDER BY 
		s.Store_Name

	COMMIT TRAN
END
GO