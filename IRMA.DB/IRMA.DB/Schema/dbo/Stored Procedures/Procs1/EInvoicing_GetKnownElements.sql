create procedure dbo.EInvoicing_GetKnownElements
as
begin
	SELECT DISTINCT ElementName, NeedsConfig
	FROM EInvoicing_Config (nolock) where disabled = 0
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownElements] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownElements] TO [IRMAClientRole]
    AS [dbo];

