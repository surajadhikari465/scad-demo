CREATE PROCEDURE dbo.GetInstanceDataFlags
	@LimitByStoreOverride bit
AS
BEGIN
    SET NOCOUNT ON

	IF @LimitByStoreOverride = 1
		SELECT FlagKey, FlagValue, CanStoreOverride
		FROM InstanceDataFlags
		WHERE CanStoreOverride = 1
	ELSE
		SELECT FlagKey, FlagValue, CanStoreOverride
		FROM InstanceDataFlags
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlags] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlags] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlags] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlags] TO [IRMAExcelRole]
    AS [dbo];

