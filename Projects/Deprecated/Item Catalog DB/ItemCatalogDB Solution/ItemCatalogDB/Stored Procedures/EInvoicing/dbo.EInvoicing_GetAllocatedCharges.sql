
IF EXISTS (
       SELECT *
       FROM   sysobjects
       WHERE  NAME = 'Einvoicing_GetAllocatedCharges'
              AND xtype = 'P'
   )
    DROP PROCEDURE dbo.Einvoicing_GetAllocatedCharges
GO 

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

GRANT EXEC ON Einvoicing_GetAllocatedCharges TO IRMAAdminRole
GRANT EXEC ON Einvoicing_GetAllocatedCharges TO IRMAClientRole
GO