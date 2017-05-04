
CREATE PROCEDURE [dbo].[GetIconItemsWithNoRetailUOM]
	@ValidatedItemList dbo.IconUpdateItemType readonly
AS
BEGIN
	SET NOCOUNT ON;
	
	select vi.*
		from  @ValidatedItemList  vi
		where not exists
			(	select 1 
				from ItemUnit	iu
				where vi.RetailUom  = iu.Unit_Abbreviation
			)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIconItemsWithNoRetailUOM] TO [IConInterface]
    AS [dbo];

