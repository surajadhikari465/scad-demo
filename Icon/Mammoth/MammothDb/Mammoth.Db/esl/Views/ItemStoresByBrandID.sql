CREATE VIEW [esl].[ItemStoresByBrandID]
AS
	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_FL ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_MW ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_NA ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_NC ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_NE ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_PN ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_RM ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_SO ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_SP ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_SW ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1

	UNION ALL

	SELECT i.ItemID, ia.BusinessUnitID, i.BrandHCID as BrandID
	FROM dbo.Items i INNER JOIN dbo.ItemAttributes_Locale_TS ia ON i.ItemID = ia.ItemID
	WHERE ia.Authorized =1
GO

GRANT SELECT ON [dbo].[ItemsStoresByBrandID] TO dds_esl_role
GO