create procedure dbo.EInvoicing_GetKnownHeaderElements
as
begin
	SELECT ElementName, ChargeOrAllowance, Subteam_No, SACCodeType, NeedsConfig
	FROM EInvoicing_Config (nolock)
	WHERE IsHeaderElement=1 and issaccode = 0 and disabled = 0
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownHeaderElements] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownHeaderElements] TO [IRMAClientRole]
    AS [dbo];

