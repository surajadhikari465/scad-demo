CREATE PROCEDURE dbo.EInvoicing_GetEInvoiceDisplay_Charges
	@EInvoiceId INT
AS
BEGIN
	SET NOCOUNT ON  
	
	
	SELECT ISNULL(ec.Label, eid.ElementName) AS ChargeName,
	       SUM(
	           CAST(eid.ElementValue AS DECIMAL(18, 4)) * chargeorallowance
	       ) AS ChargeValue
	FROM   EInvoicing_ItemData eid
	       INNER JOIN EInvoicing_Config ec
	            ON  eid.ElementName = ec.ElementName
	WHERE  eid.EInvoice_Id = @EInvoiceId
	       AND eid.ElementValue IS NOT NULL
	GROUP BY
	       eid.ElementName,
	       ec.Label 
	
	UNION ALL  
	
	SELECT ISNULL(ec.Label, eid.ElementName) AS ChargeName,
	       SUM(
	           CAST(eid.ElementValue AS DECIMAL(18, 4)) * chargeorallowance
	       ) AS ChargeValue
	FROM   EInvoicing_summaryData eid
	       INNER JOIN EInvoicing_Config ec
	            ON  eid.ElementName = ec.ElementName
	WHERE  eid.EInvoice_Id = @EInvoiceId
	       AND eid.ElementValue IS NOT NULL
	GROUP BY
	       eid.ElementName,
	       ec.Label  
	
	SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Charges] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EInvoicing_GetEInvoiceDisplay_Charges] TO [IRMAClientRole]
    AS [dbo];

