CREATE PROCEDURE dbo.Administration_UpdateStore
	@Store_No int,
	@POSSystemId int
AS 

BEGIN
	--updates STORE table w/ new data
	UPDATE Store
		SET POSSystemId = @POSSystemId
	WHERE Store_No = @Store_No
	 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UpdateStore] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UpdateStore] TO [IRMAClientRole]
    AS [dbo];

