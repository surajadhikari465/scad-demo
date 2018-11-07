SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER FUNCTION dbo.fn_InstanceDataValue
(
	@FlagKey varchar(50),
	@Store_No int
)
RETURNS bit
AS

BEGIN  
	DECLARE @FlagValue bit
    SET @FlagValue = NULL

	--GETS THE TRUE/FALSE VALUE OF A GIVEN FLAG KEY; LOOKS UP ANY POSSIBLE STORE OVERRIDES IF A STORE VALUE IS PASSED IN
	IF @Store_No IS NULL
		BEGIN
			--GET REGIONAL DEFAULT VALUE FROM InstanceDataFlags TABLE
			SELECT @FlagValue = FlagValue
			FROM InstanceDataFlags
			WHERE FlagKey = @FlagKey		
		END
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
        
	RETURN @FlagValue
END
GO
