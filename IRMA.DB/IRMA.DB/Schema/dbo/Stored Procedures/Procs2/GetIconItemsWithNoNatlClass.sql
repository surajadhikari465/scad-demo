
CREATE PROCEDURE [dbo].[GetIconItemsWithNoNatlClass]
	@ValidatedItemList dbo.IconUpdateItemType readonly
AS
BEGIN
	SET NOCOUNT ON;
	
	select vi.*
		from  @ValidatedItemList  vi
		where not exists
			(	select 1 
				from NatItemClass	nc
				where vi.NationalClassCode = nc.ClassID
			)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetIconItemsWithNoNatlClass] TO [IConInterface]
    AS [dbo];

