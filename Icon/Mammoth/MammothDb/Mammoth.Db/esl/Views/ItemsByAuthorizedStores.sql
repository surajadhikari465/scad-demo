CREATE VIEW [esl].[ItemsByAuthorizedStores]
AS
	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_FL
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_MW
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_NA
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_NC
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_NE
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_PN
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_RM
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_SO
	WHERE Authorized =1


	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_SP
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_SW
	WHERE Authorized =1

	UNION ALL

	SELECT ItemID, BusinessUnitID 
	FROM dbo.ItemAttributes_Locale_TS
	WHERE Authorized =1
GO

GRANT SELECT ON [dbo].[ItemsByAuthorizedStores] TO dds_esl_role
GO