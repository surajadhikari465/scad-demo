 /****** Object:  StoredProcedure [dbo].[InsertInstanceDataFlagsStoreOverride]    Script Date: 12/13/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[InsertInstanceDataFlagsStoreOverride]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[InsertInstanceDataFlagsStoreOverride]
GO

/****** Object:  StoredProcedure [dbo].[InsertInstanceDataFlagsStoreOverride]    Script Date: 12/13/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.InsertInstanceDataFlagsStoreOverride
	@Store_No int,
	@FlagKey varchar(50),
	@FlagValue bit
AS
BEGIN
    SET NOCOUNT ON
		
	INSERT INTO InstanceDataFlagsStoreOverride(Store_No, FlagKey, FlagValue)
	VALUES (@Store_No, @FlagKey, @FlagValue)
    
    SET NOCOUNT OFF
END
GO
  