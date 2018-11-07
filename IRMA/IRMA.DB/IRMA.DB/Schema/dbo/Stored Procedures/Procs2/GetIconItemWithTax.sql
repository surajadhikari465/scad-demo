
CREATE PROCEDURE [dbo].[GetIconItemWithTax]
	@ValidatedItemList dbo.IconUpdateItemType readonly
AS
BEGIN
	SET NOCOUNT ON;
	
	select vi.*
		from  @ValidatedItemList  vi
		where exists
			(	select 1 
				from TaxClass	tc
				where	vi.TaxClassName = tc.TaxClassDesc or substring(vi.TaxClassName, 1, 7) = substring(tc.TaxClassDesc, 1, 7)
			)
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIconItemWithTax] TO [IConInterface]
    AS [dbo];

