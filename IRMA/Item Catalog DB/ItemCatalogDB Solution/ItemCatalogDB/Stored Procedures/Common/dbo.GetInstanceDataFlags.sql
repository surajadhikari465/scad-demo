/****** Object:  StoredProcedure [dbo].[GetInstanceDataFlags]    Script Date: 11/22/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetInstanceDataFlags]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetInstanceDataFlags]
GO

/****** Object:  StoredProcedure [dbo].[GetInstanceDataFlags]    Script Date: 11/22/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

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
 