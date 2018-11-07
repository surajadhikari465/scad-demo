CREATE PROCEDURE dbo.SOG_GetStoreList

WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetStoreList()
--    Author: Billy Blackerby
--      Date: 3/13/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of stores for filters
--
-- Modification History:
-- Date			Init	Comment
-- 03/13/2009	BBB		Creation
-- 03/17/2009	BBB		Added in 'All' option
-- 03/18/2009	BBB		Added in StoreAbbr to data return
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT
		[StoreID]	= 0,
		[StoreName]	= 'All Stores',
		[StoreAbbr]	= 'All Stores'
		
	UNION
	
	 SELECT 
		[StoreID]	= s.Store_No,
		[StoreName]	= s.Store_Name,
		[StoreAbbr]	= s.StoreAbbr
	FROM 
		Store (nolock) s
	WHERE
		s.WFM_Store		= 1
		OR s.Mega_Store	= 1
		
	ORDER BY 
		StoreID, 
		StoreName

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetStoreList] TO [IRMASLIMRole]
    AS [dbo];

