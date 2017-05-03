 /****** Object:  StoredProcedure [dbo].[Administration_UpdateInstanceDataFlags]    Script Date: 11/22/2006 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_UpdateInstanceDataFlags]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_UpdateInstanceDataFlags]
GO

/****** Object:  StoredProcedure [dbo].[Administration_UpdateInstanceDataFlags]    Script Date: 11/22/2006 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Administration_UpdateInstanceDataFlags (
	@FlagKey as varchar(50), 
	@FlagValue as bit
)
AS
BEGIN
   UPDATE InstanceDataFlags SET FlagValue = @FlagValue
   WHERE FlagKey = @FlagKey    
END
GO
 