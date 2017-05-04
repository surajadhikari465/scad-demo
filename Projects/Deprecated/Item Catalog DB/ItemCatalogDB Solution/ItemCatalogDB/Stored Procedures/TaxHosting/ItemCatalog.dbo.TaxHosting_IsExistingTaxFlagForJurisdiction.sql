 /****** Object:  StoredProcedure [dbo].[TaxHosting_IsExistingTaxFlagForJurisdiction]    Script Date: 09/29/2006 16:33:41 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_IsExistingTaxFlagForJurisdiction]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[TaxHosting_IsExistingTaxFlagForJurisdiction]
GO

/****** Object:  StoredProcedure [dbo].[TaxHosting_IsExistingTaxFlagForJurisdiction]    Script Date: 09/29/2006 16:33:41 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE    PROCEDURE dbo.TaxHosting_IsExistingTaxFlagForJurisdiction
	@TaxJurisdictionID int, 
	@TaxFlagKey char(1),
	@TaxClassID int
AS	

	-- LOOK FOR EXISTING TAX FLAGS FOR THE GIVEN JURISDICTION AND TAX FLAG KEY WHERE THE TAX CLASS ID IS NOT THE VALUE PASSED IN
	-- NOTE: THIS IS USED BY THE TAX FLAG CREATION SCREEN TO WARN THE USER THAT EXISTING TAX DEFINITIONS WILL BE APPLIED TO ALL
	--		 NEW TAX FLAGS CREATED, BECAUSE THEY ARE SHARED ACROSS TAX JURISDICTIONS.  IT'S ALSO USED TO WARN USERS THAT UPDATES
	--		 TO EXISTING TAX FLAGS WILL FORCE THE TAX % AND POS_ID CHANGES TO BE APPLIED TO ALL EXISTING TAX FLAGS W/ THE SAME KEY
	
	SELECT count(1) AS TaxFlagCount
	FROM TaxFlag 
	WHERE TaxJurisdictionID = @TaxJurisdictionID
		AND TaxFlagKey = @TaxFlagKey
		AND TaxClassID <> @TaxClassID

GO

