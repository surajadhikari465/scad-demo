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
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UpdateInstanceDataFlags] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_UpdateInstanceDataFlags] TO [IRMAClientRole]
    AS [dbo];

