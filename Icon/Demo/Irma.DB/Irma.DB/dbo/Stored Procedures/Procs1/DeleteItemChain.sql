CREATE PROCEDURE [dbo].[DeleteItemChain] 
@Chain_ID int

AS

BEGIN

delete from ItemChainItem where ItemChainID=@Chain_ID

delete from ItemChain where ItemChainID=@Chain_ID

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteItemChain] TO [IRMAClientRole]
    AS [dbo];

