CREATE TYPE infor.ValidateItemType AS TABLE
(
	ItemId int not null,
	BrandHierarchyClassId int not null,
	FinancialHierarchyClassId nvarchar(4) not null,
	MerchandiseHierarchyClassId int not null,
	NationalHierarchyClassId int not null,
	TaxHierarchyClassId nvarchar(7) not null
)