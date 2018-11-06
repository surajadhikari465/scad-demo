 /****** Object:  StoredProcedure [dbo].[TaxHosting_ConfirmDeleteTaxFlag]    Script Date: 10/03/2006 16:33:28 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_ConfirmDeleteTaxFlag]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[TaxHosting_ConfirmDeleteTaxFlag]
GO

/****** Object:  StoredProcedure [dbo].[TaxHosting_ConfirmDeleteTaxFlag]    Script Date: 10/03/2006 16:33:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE dbo.TaxHosting_ConfirmDeleteTaxFlag
	@TaxClassID int,	
	@TaxJurisdictionID int,
	@TaxFlagKey varchar(1)	
AS 

BEGIN
    SET NOCOUNT ON
    
    -- THIS PROCEDURE LOOKS UP THE NUMBER OF ITEMS THAT HAVE BEEN OVERRIDDEN 
    -- IN THE TaxOverride TABLE FOR THE TAX FLAG BEING DELETED.  THE NUMBER OF ITEMS
    -- IS PASSED BACK TO THE APPLICATION, WHERE THE USER CAN DECIDE IF THEY WISH
    -- TO CONTINUE TO DELETE A SPECIFIC TAX FLAG
    
    SELECT COUNT(TaxOverride.Item_Key) AS TaxOverrideCount
	FROM TaxOverride
	INNER JOIN
		Store
		ON Store.Store_No = TaxOverride.Store_No
	INNER JOIN
		Item
		ON Item.Item_Key = TaxOverride.Item_Key
	INNER JOIN
		TaxClass
		ON TaxClass.TaxClassID = Item.TaxClassID
	INNER JOIN
		TaxJurisdiction
		ON TaxJurisdiction.TaxJurisdictionID = Store.TaxJurisdictionID
	INNER JOIN
		TaxFlag
		ON TaxFlag.TaxJurisdictionID = TaxJurisdiction.TaxJurisdictionID
		AND TaxFlag.TaxClassID = TaxClass.TaxClassID
		AND TaxFlag.TaxFlagKey = TaxOverride.TaxFlagKey
	WHERE TaxOverride.TaxFlagKey = @TaxFlagKey
		AND TaxFlag.TaxJurisdictionID = @TaxJurisdictionID
		AND TaxFlag.TaxClassID = @TaxClassID

    SET NOCOUNT OFF
END
GO

