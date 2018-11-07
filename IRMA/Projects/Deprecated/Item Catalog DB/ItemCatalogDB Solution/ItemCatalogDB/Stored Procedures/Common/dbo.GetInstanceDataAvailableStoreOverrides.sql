 /****** Object:  StoredProcedure [dbo].[GetInstanceDataAvailableStoreOverrides]    Script Date: 12/12/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetInstanceDataAvailableStoreOverrides]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstanceDataAvailableStoreOverrides]
GO

/****** Object:  StoredProcedure [dbo].[GetInstanceDataAvailableStoreOverrides]    Script Date: 12/12/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

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
 