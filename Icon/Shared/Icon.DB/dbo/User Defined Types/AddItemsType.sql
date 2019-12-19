CREATE TYPE dbo.AddItemsType AS TABLE 
(
	ScanCode NVARCHAR(13) NULL,
	BarCodeTypeId INT NOT NULL,
	ItemTypeId INT NOT NULL,
	ItemAttributesJson NVARCHAR(MAX) NOT NULL,
	BrandsHierarchyClassId INT NOT NULL,
	FinancialHierarchyClassId INT NOT NULL,
	MerchandiseHierarchyClassId INT NOT NULL,
	NationalHierarchyClassId INT NOT NULL,
	TaxHierarchyClassId INT NOT NULL,
	ManufacturerHierarchyClassId INT NULL
)
