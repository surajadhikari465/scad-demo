CREATE PROCEDURE dbo.DeleteInstanceDataFlagsStoreOverride
	@Store_No int,
	@FlagKey varchar(50)
AS
BEGIN
    SET NOCOUNT ON

	DELETE FROM InstanceDataFlagsStoreOverride
    WHERE Store_No = @Store_No
		AND FlagKey = @FlagKey
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInstanceDataFlagsStoreOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteInstanceDataFlagsStoreOverride] TO [IRMAClientRole]
    AS [dbo];

