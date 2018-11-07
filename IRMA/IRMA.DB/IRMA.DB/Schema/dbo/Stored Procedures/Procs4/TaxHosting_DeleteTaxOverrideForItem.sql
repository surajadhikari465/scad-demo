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
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxOverrideForItem] TO [IRMAClientRole]
    AS [dbo];

