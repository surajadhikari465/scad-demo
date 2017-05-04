 if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemChains]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemChains]
GO

CREATE PROCEDURE [dbo].[GetItemChains] 

AS
BEGIN

select 
	ItemChainID, rtrim(ItemChainDesc) as ItemChainDesc
from 
	ItemChain (NOLOCK)


END 
go