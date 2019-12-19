CREATE TYPE [dbo].[UpdateItemsType] AS TABLE (
	ItemId INT NOT NULL,
	ScanCode NVARCHAR(13) NULL,
	BrandsHierarchyClassId INT NULL, 
	FinancialHierarchyClassId INT NULL, 
	MerchandiseHierarchyClassId INT NULL, 
	NationalHierarchyClassId INT NULL, 
	TaxHierarchyClassId INT NULL, 
	ManufacturerHierarchyClassId INT NULL, 
	ItemAttributesJson NVARCHAR(MAX) NULL, 
	ItemTypeId INT NULL
)
GO