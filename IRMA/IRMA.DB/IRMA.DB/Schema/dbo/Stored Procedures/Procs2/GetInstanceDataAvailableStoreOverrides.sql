CREATE PROCEDURE dbo.GetInstanceDataAvailableStoreOverrides
	@FlagKey varchar(50)
AS
BEGIN
    SET NOCOUNT ON
		
	--GETS THE LIST OF STORES THAT ARE NOT ALREADY OVERRIDDEN IN THE InstanceDataFlagsStoreOverride TABLE
	--FOR A GIVEN FlagKey VALUE
	SELECT Store_No, Store_Name
	FROM Store
	WHERE Store_No NOT IN (SELECT Store_No 
							FROM InstanceDataFlagsStoreOverride
							WHERE FlagKey = @FlagKey)
		AND (WFM_Store = 1 OR Mega_Store = 1)
		AND Regional <> 1
	ORDER BY Store_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataAvailableStoreOverrides] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataAvailableStoreOverrides] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataAvailableStoreOverrides] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataAvailableStoreOverrides] TO [IRMASchedJobsRole]
    AS [dbo];

