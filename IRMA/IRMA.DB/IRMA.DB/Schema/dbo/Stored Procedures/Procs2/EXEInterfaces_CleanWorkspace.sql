create procedure dbo.EXEInterfaces_CleanWorkspace
(
@uniqueid varchar(255)
) as
begin
	delete from EXEInterfaces_ZeroShippedOrdersValidationWorkspace
	where uniqueid = @uniqueid
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_CleanWorkspace] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EXEInterfaces_CleanWorkspace] TO [IRMAClientRole]
    AS [dbo];

