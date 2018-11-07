CREATE PROCEDURE dbo.GetInstanceDataFlagsStoreOverrideList
	@FlagKey varchar(50)
AS
BEGIN
    SET NOCOUNT ON

	--GETS THE LIST OF STORES THAT ARE OVERRIDDEN FOR A GIVEN FLAG KEY VALUE
	SELECT Store.Store_No, Store.Store_Name, FlagValue
    FROM InstanceDataFlagsStoreOverride
    INNER JOIN
        Store (nolock)
        ON Store.Store_No = InstanceDataFlagsStoreOverride.Store_No
    WHERE FlagKey = @FlagKey
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList] TO [IRMASchedJobsRole]
    AS [dbo];

