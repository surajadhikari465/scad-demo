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
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemChain_ByItemChainID] TO [IRMAClientRole]
    AS [dbo];

