CREATE PROCEDURE dbo.GetTaxClass_ByDescExact
	@TaxClassDesc varchar(50)
AS 

SELECT 
	TaxClassID,
	TaxClassDesc
FROM 
	TaxClass
WHERE 
	TaxClassDesc = @TaxClassDesc
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTaxClass_ByDescExact] TO [IRMASLIMRole]
    AS [dbo];

