/****** Object:  StoredProcedure [dbo].[TaxHosting_GetTaxJurisdictionsForTaxClass]    Script Date: 08/14/2006 16:33:39 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_GetTaxJurisdictionsForTaxClass]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[TaxHosting_GetTaxJurisdictionsForTaxClass]
GO
/****** Object:  StoredProcedure [dbo].[TaxHosting_GetTaxJurisdictionsForTaxClass]    Script Date: 08/14/2006 16:33:39 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

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

 