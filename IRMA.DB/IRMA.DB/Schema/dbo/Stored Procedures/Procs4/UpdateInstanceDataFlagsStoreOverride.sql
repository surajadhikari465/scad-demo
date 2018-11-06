-- =============================================
-- Author:		Brian Robichaud
-- Create date: 11-10-09
-- Description:	Update InstanceDataFlag StoreOverride values
-- =============================================
CREATE PROCEDURE dbo.UpdateInstanceDataFlagsStoreOverride
	@Store_No int,
	@FlagKey varchar(50),
	@FlagValue bit
AS
BEGIN
	update InstanceDataFlagsStoreOverride
	set FlagValue = @FlagValue
	where Store_No = @Store_No
	and FlagKey = @FlagKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInstanceDataFlagsStoreOverride] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateInstanceDataFlagsStoreOverride] TO [IRMAClientRole]
    AS [dbo];

