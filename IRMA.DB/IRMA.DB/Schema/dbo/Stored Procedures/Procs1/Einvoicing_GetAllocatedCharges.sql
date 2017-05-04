CREATE PROCEDURE dbo.Einvoicing_GetAllocatedCharges
AS
BEGIN
	SELECT ISNULL(ec.Label, ec.ElementName) AS AllocatedCharge, ec.ChargeOrAllowance
	FROM   EInvoicing_Config ec
	WHERE  ec.IsSacCode = 1
	       AND ec.SacCodeType = 1
	ORDER BY
	       ISNULL(ec.Label, ec.ElementName)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Einvoicing_GetAllocatedCharges] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Einvoicing_GetAllocatedCharges] TO [IRMAClientRole]
    AS [dbo];

