create procedure dbo.EInvoicing_GetKnownItemElements
as
begin
	SELECT ElementName, ChargeOrAllowance, Subteam_No, SACCodeType, NeedsConfig
	FROM EInvoicing_Config (nolock)
	WHERE IsItemElement=1 and issaccode = 0  and disabled = 0 
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownItemElements] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownItemElements] TO [IRMAClientRole]
    AS [dbo];

