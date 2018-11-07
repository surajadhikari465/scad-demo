create procedure dbo.EInvoicing_GetKnownSACCodes
as
begin
	SELECT ElementName, ChargeOrAllowance, Subteam_No, SACCodeType, NeedsConfig
	FROM EInvoicing_Config (nolock)
	WHERE IsSACCode=1 and disabled = 0
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownSACCodes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetKnownSACCodes] TO [IRMAClientRole]
    AS [dbo];

