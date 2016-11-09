CREATE PROCEDURE dbo.[GetTaxClasses] AS

BEGIN
    SET NOCOUNT ON

	SELECT [TaxClassID],
		[TaxClassDesc]
	FROM [TaxClass]
	ORDER BY [TaxClassDesc]
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTaxClasses] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTaxClasses] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTaxClasses] TO [IRMAExcelRole]
    AS [dbo];

