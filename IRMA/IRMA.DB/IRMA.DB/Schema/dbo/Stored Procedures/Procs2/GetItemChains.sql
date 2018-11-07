CREATE PROCEDURE [dbo].[GetItemChains] 

AS
BEGIN

select 
	ItemChainID, rtrim(ItemChainDesc) as ItemChainDesc
from 
	ItemChain (NOLOCK)


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemChains] TO [IRMAClientRole]
    AS [dbo];

