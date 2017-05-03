 /****** Object:  StoredProcedure [dbo].[GetInstanceDataFlagsStoreOverrideList]    Script Date: 12/12/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetInstanceDataFlagsStoreOverrideList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstanceDataFlagsStoreOverrideList]
GO

/****** Object:  StoredProcedure [dbo].[GetInstanceDataFlagsStoreOverrideList]    Script Date: 12/12/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

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
  