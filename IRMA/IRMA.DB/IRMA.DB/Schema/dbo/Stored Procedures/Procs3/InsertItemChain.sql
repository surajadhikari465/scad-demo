CREATE PROCEDURE [dbo].[InsertItemChain] 
@Chain_Name varchar(100),
@Chain_ID int OUTPUT

AS

BEGIN

Insert into ItemChain (ItemChainDesc) values (@Chain_Name)

SET @Chain_ID = SCOPE_IDENTITY()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemChain] TO [IRMAClientRole]
    AS [dbo];

