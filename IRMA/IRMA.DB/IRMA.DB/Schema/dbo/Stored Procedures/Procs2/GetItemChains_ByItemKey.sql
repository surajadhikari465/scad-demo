CREATE PROCEDURE [dbo].[GetItemChains_ByItemKey] 
	@ItemID int
AS

BEGIN

select 
	* 
from 
	ItemChain 
where 
	ItemChainId in
		(select 
			ItemChainId 
		from 
			ItemChainItem 
		where 
			Item_Key=@ItemID)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemChains_ByItemKey] TO [IRMAClientRole]
    AS [dbo];

