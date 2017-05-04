if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemChain_ByItemChainID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemChain_ByItemChainID]
GO

CREATE PROCEDURE [dbo].[GetItemChain_ByItemChainID] 
	@Chain_ID int
AS
BEGIN

select 
	rtrim(ItemChainDesc) [Value] 
from 
	ItemChain 
where 
	ItemChainID = @Chain_ID

END 
go