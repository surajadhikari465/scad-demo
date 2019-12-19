CREATE PROCEDURE [dbo].[GetItemsByIdsSearch]
	@ItemIds app.IntList READONLY
AS
BEGIN
	IF OBJECT_ID('tempdb..#tempIds') IS NOT NULL
		DROP TABLE #tempIds

	CREATE TABLE #tempIds (ID INT NOT NULL);

	CREATE NONCLUSTERED INDEX IX_#temp_Id ON #tempIds (ID ASC)

	INSERT INTO #tempIds (ID)
	SELECT I
	FROM @ItemIds;

	SELECT i.ItemId
		,i.ItemTypeId
		,i.ItemTypeCode
		,i.ItemTypeDescription
		,i.ScanCode
		,i.BarcodeTypeId AS BarcodeTypeId
		,i.BarcodeType AS BarcodeType
		,i.MerchandiseHierarchyClassId
		,i.BrandsHierarchyClassId
		,i.TaxHierarchyClassId
		,i.FinancialHierarchyClassId
		,i.NationalHierarchyClassId
		,i.ManufacturerHierarchyClassId
		,i.ItemAttributesJson AS ItemAttributesJson
		,i.Brand AS Brands
		,i.Merchandise
		,i.Tax
		,i.NationalClass AS [National]
		,i.Financial
		,i.Manufacturer
	FROM ItemView i
	INNER JOIN #tempIds ti ON ti.ID = i.ItemId
END