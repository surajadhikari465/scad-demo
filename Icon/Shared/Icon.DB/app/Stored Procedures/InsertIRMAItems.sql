-- =============================================
-- Author:		Min Zhao
-- Create date: 2014-10-15
-- Description:	Bulk insert IRMAItem entities.
-- =============================================
CREATE PROCEDURE [app].[InsertIRMAItems]
	@IRMAItems app.IRMAItemType readonly
AS
BEGIN	
	insert into [app].[IRMAItem]
      ([regioncode],
      [identifier],
      [defaultIdentifier],
      [brandName],
      [itemDescription],
      [posDescription],
      [packageUnit],
      [retailSize],
      [retailUom],
      [foodStamp],
      [posScaleTare],
      [departmentSale],
      [giftCard],
      [taxClassID],
      [merchandiseClassID],
      [insertDate],
	  [irmaSubTeamName],
	  [nationalClassID])
	select 
		[RegionCode],
		[Identifier],
		[DefaultIdentifier],
		[BrandName],
		[ItemDescription],
		[PosDescription],
		[PackageUnit],
		[RetailSize],
		[RetailUom],
		[FoodStamp],
		[PosScaleTare],
		[DepartmentSale],
		[GiftCard],
		[TaxClassID],
		[MerchandiseClassID],
		GetDate(),
		[IrmaSubTeamName],
		[NationalClassID]  
	from @IRMAItems
END
GO