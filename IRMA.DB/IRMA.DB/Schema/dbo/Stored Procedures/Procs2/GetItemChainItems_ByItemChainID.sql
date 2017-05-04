CREATE PROCEDURE [dbo].[GetItemChainItems_ByItemChainID] 
	@Chain_ID int
AS

BEGIN

select 
	a.Item_Key,
	rtrim(b.Item_Description) [Item_Description], 
	identifier, 
	(SELECT TOP 1 ItemChainID FROM ItemChainItem where Item_Key=b.Item_Key) [Chain_ID] 
from 
	ItemChainItem a,
	Item b,
	ItemIdentifier c
where 
	a.Item_Key=b.item_key
	and 
	a.Item_Key = c.Item_Key 
	and 
	a.ItemChainID=@Chain_ID
order by 
	b.Item_Description

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemChainItems_ByItemChainID] TO [IRMAClientRole]
    AS [dbo];

