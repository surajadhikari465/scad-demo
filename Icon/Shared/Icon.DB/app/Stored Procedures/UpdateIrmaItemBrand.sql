
CREATE PROCEDURE [app].[UpdateIrmaItemBrand]
	@currentBrandName varchar(35),
	@updatedBrandName varchar(35)
AS
BEGIN
	update app.IRMAItem set brandName = @updatedBrandName where brandName = @currentBrandName
END
GO
