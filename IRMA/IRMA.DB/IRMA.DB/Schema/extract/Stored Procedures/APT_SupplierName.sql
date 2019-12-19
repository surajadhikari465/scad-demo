CREATE PROCEDURE [extract].[APT_SupplierName]
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	DECLARE @region CHAR(2)

	SELECT @region = runmode
	FROM conversion_runmode

	SELECT @region AS REGION
		,PS_Vendor_ID
		,CompanyName
	FROM Vendor
END
GO

GRANT EXECUTE
    ON OBJECT:: [extract].[APT_SupplierName] TO [IConInterface]
    AS [dbo];
