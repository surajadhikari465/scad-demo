CREATE PROCEDURE [dbo].[GetCompetitiveBatchItemDateChecked] 
	@KeyList varchar(max), 
	@CheckDate varchar(50) OUTPUT
AS
BEGIN
-- 20101210 - Dave Stacey removed default identifier = 1 
-- 201004-0816 - Dave Stacey created for v4 to find out what the greatest date checked date is for 
--competitive items within a competitive batch
-- if any item is not already uploaded via the competitive pricing module this returns 'missing' 
--to throw the appropriate error on the pricebatch item search screen.
--20101207 - Dave Stacey - Added avoidance of deleted identifiers
DECLARE @StoreItem TABLE(Store_No INT, Item_Key INT)
DECLARE @StoreItemCount INT, @ActualCount INT

INSERT @StoreItem SELECT Comp.Key_Value1, ii.Item_Key
	FROM dbo.ItemIdentifier ii (NOLOCK)
	JOIN (Select Key_Value1, Key_Value2 from dbo.fn_ParseIntStringList (@KeyList, '|', ',')) AS Comp ON Comp.Key_Value2 = ii.Identifier
	WHERE ii.Deleted_Identifier = 0
	SELECT @StoreItemCount = COUNT(*) FROM @StoreItem
	SELECT * FROM @StoreItem
SELECT 
	@CheckDate = MIN(CP.CheckDate), @ActualCount = COUNT(*)
	FROM
	competitorprice cp (nolock)
	JOIN competitorstore cs (nolock) on cs.CompetitorStoreID = cp.competitorstoreid
	JOIN competitor c (nolock) on cs.CompetitorID = C.CompetitorID
	JOIN competitorlocation cl (nolock) on cl.CompetitorLocationID = CS.CompetitorLocationID
	JOIN store s (nolock) on s.store_name = cl.Name
	JOIN @StoreItem AS Comp ON Comp.Store_No = s.Store_No AND Comp.Item_Key = CP.Item_Key

IF @StoreItemCount > @ActualCount
	SELECT @CheckDate = 'Missing'
	
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitiveBatchItemDateChecked] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitiveBatchItemDateChecked] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitiveBatchItemDateChecked] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitiveBatchItemDateChecked] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCompetitiveBatchItemDateChecked] TO [IRMAPromoRole]
    AS [dbo];

