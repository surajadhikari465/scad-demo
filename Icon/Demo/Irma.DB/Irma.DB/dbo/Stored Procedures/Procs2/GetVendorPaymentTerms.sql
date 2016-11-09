CREATE PROCEDURE [dbo].[GetVendorPaymentTerms] 
AS

BEGIN

	SET NOCOUNT ON

	SELECT PaymentTermID, [Description], DateLoaded, [Default]
	FROM VendorPaymentTerms

	SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorPaymentTerms] TO [IRMAClientRole]
    AS [dbo];

