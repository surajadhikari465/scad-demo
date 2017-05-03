
create procedure [dbo].[EInvoicing_RemoveConfigValue]
	@ElementName varchar(255)
as
begin

	DELETE FROM EInvoicing_Config
	WHERE ElementName = @ElementName

end


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_RemoveConfigValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_RemoveConfigValue] TO [IRMAClientRole]
    AS [dbo];

