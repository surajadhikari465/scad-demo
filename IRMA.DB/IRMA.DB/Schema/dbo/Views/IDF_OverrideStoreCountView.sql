CREATE VIEW dbo.IDF_OverrideStoreCountView 
AS
SELECT 
	 IDF.FlagKey
	,IDF.FlagValue AS RegionalFlagValue
	,COUNT(OV.Store_No) AS NumStoresWithOverrides
	,S.NumStoresTotal
FROM dbo.InstanceDataFlags IDF
LEFT OUTER JOIN dbo.InstanceDataFlagsStoreOverride OV ON 
	OV.FlagKey = IDF.FlagKey AND IDF.CanStoreOverride=1 AND OV.FlagValue <> IDF.FlagValue
CROSS APPLY (
	SELECT COUNT(Store_No) AS NumStoresTotal
	FROM dbo.Store
	WHERE Internal = 1 AND (WFM_Store = 1 OR Mega_Store = 1)
	) S
GROUP BY IDF.FlagKey, IDF.CanStoreOverride, IDF.FlagValue, S.NumStoresTotal

GO
GRANT SELECT ON OBJECT::dbo.IDF_OverrideStoreCountView
	TO IRMAAdminRole AS dbo;
GO
GRANT SELECT ON OBJECT::dbo.IDF_OverrideStoreCountView
	TO IRMASupportRole AS dbo;
GO
GRANT SELECT ON OBJECT::dbo.IDF_OverrideStoreCountView
	TO IRMAClientRole AS dbo;
GO
GRANT SELECT ON OBJECT::dbo.IDF_OverrideStoreCountView
	TO IRMASchedJobsRole AS dbo;
