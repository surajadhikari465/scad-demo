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
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInstanceDataFlagsValues] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInstanceDataFlagsValues] TO [IRMAClientRole]
    AS [dbo];

