
/****** Object:  StoredProcedure [dbo].[UpdateInstanceDataFlagsValues]    Script Date: 11/10/2009 08:25:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateInstanceDataFlagsValues]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateInstanceDataFlagsValues]
GO
-- =============================================
-- Author:		Brian Robichaud
-- Create date: 11-10-09
-- Description:	Update FlagValue and CanStoreOverride values
-- =============================================
CREATE PROCEDURE dbo.UpdateInstanceDataFlagsValues
	@FlagKey varchar(50),
	@FlagValue bit,
	@CanStoreOverride bit
AS
BEGIN
	UPDATE InstanceDataFlags
	SET FlagValue = @FlagValue,
		CanStoreOverride = @CanStoreOverride
	WHERE FlagKey = @FlagKey
END
GO
--grant exec on dbo.UpdateInstanceDataFlagsValues to IRMAAdminRole