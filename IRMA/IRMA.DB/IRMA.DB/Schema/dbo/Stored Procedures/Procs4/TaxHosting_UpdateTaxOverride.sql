CREATE PROCEDURE dbo.TaxHosting_UpdateTaxOverride
	@ItemKey int,
	@StoreNo int,
	@TaxFlagKey varchar(1),
	@TaxFlagValue bit
AS 

BEGIN
    SET NOCOUNT ON
    
    -- First set all tax flags for the item to Inactive (so that only one will be active at once)
    UPDATE TaxOverride SET TaxFlagValue = 0
    WHERE Item_Key = @ItemKey
	AND Store_No = @StoreNo

    -- Set the selected flag to active
    UPDATE TaxOverride SET TaxFlagValue = @TaxFlagValue
    WHERE Item_Key = @ItemKey
	AND Store_No = @StoreNo
	AND TaxFlagKey = @TaxFlagKey

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_UpdateTaxOverride] TO [IRMAReportsRole]
    AS [dbo];

