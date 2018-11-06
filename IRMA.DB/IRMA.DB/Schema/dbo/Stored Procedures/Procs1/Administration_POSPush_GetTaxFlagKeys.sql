CREATE PROCEDURE dbo.Administration_POSPush_GetTaxFlagKeys AS
-- Reads the unique list of TaxFlagKey values from the TaxFlag table for the
-- configuration of the POS Push application.
BEGIN
SELECT DISTINCT TaxFlagKey 
FROM TaxFlag  
ORDER BY TaxFlagKey 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetTaxFlagKeys] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetTaxFlagKeys] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetTaxFlagKeys] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetTaxFlagKeys] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetTaxFlagKeys] TO [IRMAReportsRole]
    AS [dbo];

