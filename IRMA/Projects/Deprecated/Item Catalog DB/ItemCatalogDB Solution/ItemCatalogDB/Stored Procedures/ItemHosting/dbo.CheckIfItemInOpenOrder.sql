IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CheckIfItemInOpenOrder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CheckIfItemInOpenOrder]
GO

CREATE PROCEDURE [dbo].[CheckIfItemInOpenOrder] 
    @Item_Key int

AS 

BEGIN

SELECT count(OH.OrderHeader_ID) as OpenOrderCount 
FROM OrderHeader OH (nolock) 
inner join OrderItem OI (nolock) 
	on OH.OrderHeader_ID = OI.OrderHeader_ID
WHERE 
OI.Item_Key = @Item_Key
and OH.CloseDate IS NULL

END

GO