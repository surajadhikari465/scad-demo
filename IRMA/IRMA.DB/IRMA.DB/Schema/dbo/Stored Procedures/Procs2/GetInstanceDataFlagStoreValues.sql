CREATE PROCEDURE dbo.GetInstanceDataFlagStoreValues
	@FlagKey VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON

	IF OBJECT_ID('tempdb..#storeFlagStatus') IS NOT NULL
		DROP TABLE #storeFlagStatus

	SELECT s.Store_No
		,s.BusinessUnit_ID
		,idf.FlagKey
		,idf.FlagValue
	INTO #storeFlagStatus
	FROM dbo.InstanceDataFlags idf
	CROSS APPLY (
		SELECT Store_No
			,BusinessUnit_ID
		FROM Store
		WHERE WFM_Store = 1
			OR Mega_Store = 1
		) s
	WHERE idf.FlagKey = @FlagKey

	UPDATE sf
	SET sf.FlagValue = so.FlagValue
	FROM #storeFlagStatus sf
	INNER JOIN dbo.InstanceDataFlagsStoreOverride so ON sf.Store_No = so.Store_No
		AND sf.FlagKey = so.FlagKey

	SELECT Store_No as IrmaStoreNumber
		,BusinessUnit_ID as BusinessUnitId
		,FlagKey as FlagKey
		,FlagValue as FlagValue
	FROM #storeFlagStatus

	SET NOCOUNT OFF
END
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList]
	TO [IRMAAdminRole] AS [dbo];
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList]
	TO [IRMASupportRole] AS [dbo];
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList]
	TO [IRMAClientRole] AS [dbo];
GO

GRANT EXECUTE
	ON OBJECT::[dbo].[GetInstanceDataFlagsStoreOverrideList]
	TO [IRMASchedJobsRole] AS [dbo];