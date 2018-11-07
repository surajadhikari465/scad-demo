CREATE   PROCEDURE dbo.TaxHosting_DeleteTaxOverride
	@ItemKey int,	
	@StoreNo int,
	@TaxFlagKey varchar(1)	
AS 

BEGIN
    SET NOCOUNT ON
    
    -- delete TaxOverride data
    DELETE FROM TaxOverride
    WHERE Item_Key = @ItemKey
	AND Store_No = @StoreNo
	AND TaxFlagKey = @TaxFlagKey

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_DeleteTaxOverride] TO [IRMAReportsRole]
    AS [dbo];

