/****** Object:  StoredProcedure [dbo].[TaxHosting_GetAvailableTaxFlagsForItem]    Script Date: 08/15/2006 16:33:33 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_GetAvailableTaxFlagsForItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[TaxHosting_GetAvailableTaxFlagsForItem]
GO

/****** Object:  StoredProcedure [dbo].[TaxHosting_GetAvailableTaxFlagsForItem]    Script Date: 08/15/2006 16:33:33 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE   PROCEDURE dbo.TaxHosting_GetAvailableTaxFlagsForItem(@Item_Key int, @Store_No int) AS	
BEGIN
	-- THIS PROCEDURE RETURNS THE AVAILABLE TAX FLAG VALUES FOR A GIVEN ITEM/STORE COMBINATION
	-- THE RESULTS ARE THE ONLY VALID TAX FLAGS THAT CAN BE OVERRIDDEN IN THE TAX OVERRIDE SCREEN
	SELECT DISTINCT TaxFlag.TaxFlagKey
	FROM TaxFlag, TaxClass, TaxJurisdiction, Item, Store
	WHERE TaxFlag.TaxClassID = TaxClass.TaxClassID
		AND TaxFlag.TaxJurisdictionID = TaxJurisdiction.TaxJurisdictionID
		AND TaxFlag.TaxClassID = Item.TaxClassID
		AND TaxFlag.TaxJurisdictionID = Store.TaxJurisdictionID
		AND Item_Key = @Item_Key
		AND Store_No = @Store_No
	ORDER BY TaxFlag.TaxFlagKey
END
GO

 