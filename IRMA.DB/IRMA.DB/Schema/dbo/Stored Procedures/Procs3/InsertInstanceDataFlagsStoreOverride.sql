CREATE PROCEDURE dbo.InsertInstanceDataFlagsStoreOverride
	@Store_No int,
	@FlagKey varchar(50),
	@FlagValue bit
AS
BEGIN
    SET NOCOUNT ON
		
	INSERT INTO InstanceDataFlagsStoreOverride(Store_No, FlagKey, FlagValue)
	VALUES (@Store_No, @FlagKey, @FlagValue)
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInstanceDataFlagsStoreOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertInstanceDataFlagsStoreOverride] TO [IRMAClientRole]
    AS [dbo];

