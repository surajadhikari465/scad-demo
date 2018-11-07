 /****** Object:  StoredProcedure [dbo].[GetInstanceDataFlagValue]    Script Date: 12/13/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetInstanceDataFlagValue]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstanceDataFlagValue]
GO

/****** Object:  StoredProcedure [dbo].[GetInstanceDataFlagValue]    Script Date: 12/13/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

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
   