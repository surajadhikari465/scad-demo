create procedure dbo.EInvoicing_getSACTypes
as
begin
	select * from einvoicing_sactypes (nolock)
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getSACTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_getSACTypes] TO [IRMAClientRole]
    AS [dbo];

