CREATE PROCEDURE [dbo].[InsertItemChainItem] 
@Chain_ID int,
@Item_Key int

AS

BEGIN

Insert into ItemChainItem (ItemChainId,Item_Key) values (@Chain_ID,@Item_Key)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemChainItem] TO [IRMAClientRole]
    AS [dbo];

