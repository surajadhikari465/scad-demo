CREATE PROCEDURE dbo.TaxHosting_InsertTaxOverride
	@ItemKey int,
	@StoreNo int,
	@TaxFlagKey varchar(1),
	@TaxFlagValue bit
AS 

BEGIN
    SET NOCOUNT ON

	IF EXISTS (SELECT TaxFlagKey FROM TaxOverride WHERE Item_Key = @ItemKey AND Store_No = @StoreNo AND TaxFlagKey = @TaxFlagKey)
		BEGIN
			-- First set all tax flags for the item to Inactive (so that only one will be active at once)
			UPDATE TaxOverride SET TaxFlagValue = 0
			WHERE Item_Key = @ItemKey
			AND Store_No = @StoreNo
			
			-- First set all tax flags for the item to Inactive (so that only one will be active at once)
			UPDATE TaxOverride SET TaxFlagValue = @TaxFlagValue
			WHERE Item_Key = @ItemKey
			AND Store_No = @StoreNo
			AND TaxFlagKey = @TaxFlagKey
		END
	ELSE
		BEGIN		
			IF @TaxFlagValue = 1
				-- First set all tax flags for the item to Inactive (so that only one will be active at once)
				UPDATE TaxOverride SET TaxFlagValue = 0
				WHERE Item_Key = @ItemKey
				AND Store_No = @StoreNo
			
			-- Insert new flag and make it active
			INSERT INTO TaxOverride (Item_Key, Store_No, TaxFlagKey, TaxFlagValue)
			VALUES (@ItemKey, @StoreNo, @TaxFlagKey, @TaxFlagValue)
		END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_InsertTaxOverride] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_InsertTaxOverride] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[TaxHosting_InsertTaxOverride] TO [IRMAReportsRole]
    AS [dbo];

