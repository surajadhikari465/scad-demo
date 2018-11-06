if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemChains_ByItemKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemChains_ByItemKey]
GO

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
go