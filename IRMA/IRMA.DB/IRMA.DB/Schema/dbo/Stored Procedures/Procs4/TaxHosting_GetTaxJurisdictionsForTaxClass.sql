CREATE   PROCEDURE dbo.TaxHosting_GetTaxJurisdictionsForTaxClass 
	@TaxClassID int
AS
	SELECT DISTINCT TaxJurisdiction.TaxJurisdictionID, TaxJurisdiction.TaxJurisdictionDesc 
	FROM TaxJurisdiction
	INNER JOIN
		TaxFlag
		ON TaxFlag.TaxJurisdictionID = TaxJurisdiction.TaxJurisdictionID
	INNER JOIN
		TaxClass
		ON TaxClass.TaxClassID = TaxFlag.TaxClassID
	WHERE TaxClass.TaxClassID = @TaxClassID
	ORDER BY TaxJurisdictionDesc
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxJurisdictionsForTaxClass] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxJurisdictionsForTaxClass] TO [IRMAClientRole]
    AS [dbo];

