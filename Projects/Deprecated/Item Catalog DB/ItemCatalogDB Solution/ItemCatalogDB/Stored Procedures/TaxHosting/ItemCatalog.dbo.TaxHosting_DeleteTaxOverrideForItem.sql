/****** Object:  StoredProcedure [dbo].[TaxHosting_DeleteTaxOverrideForItem]    Script Date: 12/29/2006 16:33:28 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[TaxHosting_DeleteTaxOverrideForItem]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[TaxHosting_DeleteTaxOverrideForItem]
GO

/****** Object:  StoredProcedure [dbo].[TaxHosting_DeleteTaxOverrideForItem]    Script Date: 12/29/2006 16:33:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE dbo.TaxHosting_DeleteTaxOverrideForItem
	@ItemKey int
AS 

BEGIN
    SET NOCOUNT ON
    
    -- delete ALL TaxOverride data for the given item key
    DELETE FROM TaxOverride
    WHERE Item_Key = @ItemKey

    SET NOCOUNT OFF
END
GO

 