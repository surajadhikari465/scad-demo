CREATE PROCEDURE [dbo].[TaxHosting_GetTaxJurisdictions]
AS
begin
	SELECT TaxJurisdictionID, 
		TaxJurisdictionDesc, 
		LastUpdate,
		LastUpdateUserID,
		[StoreCount] =
			(SELECT COUNT(1) 
	 		 FROM dbo.Store S
	 		 WHERE S.TaxJurisdictionID = TJ.TaxJurisdictionID),
		[TaxFlagCount] =
			(SELECT COUNT(1)
	 		 FROM dbo.TaxFlag TF
	 		 WHERE TF.TaxJurisdictionID = TJ.TaxJurisdictionID),
		[RegionalJurisdictionID] = tj.RegionalJurisdictionID
	FROM dbo.TaxJurisdiction TJ
	ORDER BY TaxJurisdictionDesc
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxJurisdictions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxJurisdictions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxJurisdictions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxJurisdictions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_GetTaxJurisdictions] TO [IRMAReportsRole]
    AS [dbo];

