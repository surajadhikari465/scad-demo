if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemChainItems_ByItemChainID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemChainItems_ByItemChainID]
GO

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
go