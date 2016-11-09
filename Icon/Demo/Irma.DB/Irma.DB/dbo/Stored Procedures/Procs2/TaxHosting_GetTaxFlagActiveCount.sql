CREATE PROCEDURE dbo.TaxHosting_GetTaxFlagActiveCount(@TaxClassID int, @TaxJurisdictionID int) AS	
	
	--THIS PROCEDURE IS USED FOR UK ONLY; UK REQUIRES ONLY 1 TaxFlag BE ACTIVE AT A TIME
	--QUERY BELOW DOES NOT CHECK COUNT FOR OVERRIDE TAX FLAGS AS UK DOES NOT ALLOW OVERRIDES
	SELECT COUNT(1) As ActiveCount
	FROM TaxFlag
	WHERE TaxClassID = @TaxClassID
		AND TaxJurisdictionID = @TaxJurisdictionID
		AND TaxFlagValue = 1
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlagActiveCount] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlagActiveCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlagActiveCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlagActiveCount] TO [IRMAReportsRole]
    AS [dbo];

