/****** Object:  StoredProcedure [dbo].[TaxHosting_GetTaxOverride]    Script Date: 08/14/2006 16:33:41 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_GetTaxOverrideForItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[TaxHosting_GetTaxOverrideForItem]
GO

/****** Object:  StoredProcedure [dbo].[TaxHosting_GetTaxOverrideForItem]    Script Date: 08/14/2006 16:33:41 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE    PROCEDURE dbo.TaxHosting_GetTaxOverrideForItem(@ItemKey int) AS	

	SELECT Store.Store_Name, TaxOverride.TaxFlagKey, TaxOverride.TaxFlagValue
	FROM TaxOverride, Store, Item
	WHERE TaxOverride.Store_No = Store.Store_No
		AND TaxOverride.Item_Key = Item.Item_Key		
		AND Item.Item_Key = @ItemKey
	ORDER BY Store.Store_Name, TaxOverride.TaxFlagKey

GO

 