CREATE PROCEDURE dbo.GetInstanceDataFlagValue
	@FlagKey varchar(50),
	@Store_No int
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @FlagValue bit
    SET @FlagValue = NULL

	--GETS THE TRUE/FALSE VALUE OF A GIVEN FLAG KEY; LOOKS UP ANY POSSIBLE STORE OVERRIDES IF A STORE VALUE IS PASSED IN
	IF @Store_No IS NULL
		--GET REGIONAL DEFAULT VALUE FROM InstanceDataFlags TABLE
		SELECT @FlagValue = FlagValue
		FROM InstanceDataFlags
		WHERE FlagKey = @FlagKey		
	ELSE
		BEGIN
			--LOOK UP POSSIBLE STORE OVERRIDE VALUE IN InstanceDataFlagsStoreOverride TABLE
			SELECT @FlagValue = FlagValue
			FROM InstanceDataFlagsStoreOverride
			WHERE FlagKey = @FlagKey
				AND Store_No = @Store_No

			--GET REGIONAL DEFAULT FROM InstanceDataFlags TABLE
			IF @FlagValue IS NULL
				SELECT @FlagValue = FlagValue
				FROM InstanceDataFlags
				WHERE FlagKey = @FlagKey
		END
    
    --RETURN FLAG VALUE
    SELECT ISNULL(@FlagValue, 0) As FlagValue
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagValue] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagValue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagValue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagValue] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagValue] TO [IRMASLIMRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInstanceDataFlagValue] TO [IConInterface]
    AS [dbo];
