  /****** Object:  StoredProcedure [dbo].[DeleteInstanceDataFlagsStoreOverride]    Script Date: 12/12/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DeleteInstanceDataFlagsStoreOverride]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[DeleteInstanceDataFlagsStoreOverride]
GO

/****** Object:  StoredProcedure [dbo].[DeleteInstanceDataFlagsStoreOverride]    Script Date: 12/12/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.DeleteInstanceDataFlagsStoreOverride
	@Store_No int,
	@FlagKey varchar(50)
AS
BEGIN
    SET NOCOUNT ON

	DELETE FROM InstanceDataFlagsStoreOverride
    WHERE Store_No = @Store_No
		AND FlagKey = @FlagKey
    
    SET NOCOUNT OFF
END
GO
 