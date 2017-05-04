CREATE PROCEDURE dbo.TaxHosting_GetTaxFlag
(
	@TaxClassID int
	,@TaxJurisdictionID int
)
AS	
BEGIN
	SELECT
		[TaxClassID] = TaxFlag.TaxClassID
		,[TaxClassDesc] = TaxClass.TaxClassDesc
		,[TaxJurisdictionID] = TaxFlag.TaxJurisdictionID
		,[TaxJurisdictionDesc] = TaxJurisdiction.TaxJurisdictionDesc
		,[TaxFlagKey] = TaxFlag.TaxFlagKey
		,[TaxFlagValue] = TaxFlag.TaxFlagValue
		,[TaxPercent] = TaxDefinition.TaxPercent
		,[POSID] = TaxDefinition.POSID
		,[ExternalTaxGroupCode] = TaxClass.ExternalTaxGroupCode
	FROM
		TaxFlag
		,TaxClass
		,TaxJurisdiction
		,TaxDefinition
	WHERE
		TaxFlag.TaxClassID = TaxClass.TaxClassID
		AND TaxFlag.TaxJurisdictionID = TaxJurisdiction.TaxJurisdictionID
		AND TaxFlag.TaxJurisdictionID = TaxDefinition.TaxJurisdictionID
		AND TaxFlag.TaxFlagKey = TaxDefinition.TaxFlagKey
		AND TaxFlag.TaxClassID = @TaxClassID
		AND TaxFlag.TaxJurisdictionID = @TaxJurisdictionID
	ORDER BY TaxFlag.TaxFlagKey
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlag] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlag] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxFlag] TO [IRMAReportsRole]
    AS [dbo];

