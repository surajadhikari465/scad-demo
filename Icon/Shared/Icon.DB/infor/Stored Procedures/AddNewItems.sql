CREATE PROCEDURE [infor].[AddNewItems]
	@items app.IRMAItemType READONLY
AS
BEGIN

	MERGE app.IRMAItemSubscription iis
	USING @items i
	ON iis.RegionCode = i.RegionCode
		AND iis.Identifier = i.Identifier
	WHEN MATCHED THEN
		UPDATE 
		SET insertDate = GETDATE(),
			deleteDate = NULL
	WHEN NOT MATCHED THEN
		INSERT (regioncode, identifier, insertDate, deleteDate)
		VALUES (RegionCode, Identifier, GETDATE(), NULL);

	INSERT INTO app.IRMAItem
      (regioncode,
      identifier,
      defaultIdentifier,
      brandName,
      itemDescription,
      posDescription,
      packageUnit,
      retailSize,
      retailUom,
      foodStamp,
      posScaleTare,
      departmentSale,
      giftCard,
      taxClassID,
      merchandiseClassID,
      insertDate,
	  irmaSubTeamName,
	  nationalClassID,
	  OrganicAgencyId)
	SELECT 
		RegionCode,
		Identifier,
		DefaultIdentifier,
		BrandName,
		ItemDescription,
		PosDescription,
		PackageUnit,
		RetailSize,
		RetailUom,
		FoodStamp,
		PosScaleTare,
		DepartmentSale,
		GiftCard,
		TaxClassID,
		MerchandiseClassID,
		GetDate(),
		IrmaSubTeamName,
		NationalClassID,
		OrganicAgencyId  
	FROM @items i
	WHERE NOT EXISTS
	(
		SELECT 1
		FROM app.IRMAItem (NOLOCK) ii
		WHERE i.Identifier = ii.identifier
	)
	AND
	NOT EXISTS
	(
		SELECT 1
		FROM ScanCode (NOLOCK) sc
		WHERE sc.ScanCode = i.Identifier
	)
END