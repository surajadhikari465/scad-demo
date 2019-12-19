CREATE TABLE dbo.Item
(
	ItemId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Items PRIMARY KEY,
	ItemTypeId INT NOT NULL CONSTRAINT ItemType_Item_FK1 FOREIGN KEY REFERENCES dbo.ItemType(itemTypeId),
	ItemAttributesJson NVARCHAR(MAX) NOT NULL CONSTRAINT DF_ItemAttributesJson DEFAULT ('{}'),
	SysStartTimeUtc datetime2 GENERATED ALWAYS AS ROW START HIDDEN
		CONSTRAINT DF_Item_SysStart DEFAULT SYSUTCDATETIME(),
	SysEndTimeUtc datetime2 GENERATED ALWAYS AS ROW END HIDDEN
		CONSTRAINT DF_Item_SysEnd DEFAULT CONVERT(DATETIME2(7), '9999-12-31 23:59:59.99999999'),
	PERIOD FOR SYSTEM_TIME (SysStartTimeUtc, SysEndTimeUtc),
	Inactive AS JSON_VALUE(ItemAttributesJson,'$.Inactive'),
	ProductDescription AS JSON_VALUE(ItemAttributesJson,'$.ProductDescription'),
	CustomerFriendlyDescription AS JSON_VALUE(ItemAttributesJson,'$.CustomerFriendlyDescription'),
	POSDescription AS JSON_VALUE(ItemAttributesJson,'$.POSDescription'),
	FoodStamp AS JSON_VALUE(ItemAttributesJson,'$.FoodStampEligible'),
	CONSTRAINT CK_ItemAttributesJson CHECK (ISJSON(ItemAttributesJson) > 0)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ItemHistory));
GO

CREATE NONCLUSTERED INDEX IX_Item_Inactive ON dbo.Item (Inactive) INCLUDE (
	[ItemId]
	)
GO

CREATE NONCLUSTERED INDEX IX_Item_ProductDescription ON dbo.Item (ProductDescription)
GO

CREATE NONCLUSTERED INDEX IX_Item_CustomerFriendlyDescription ON dbo.Item (CustomerFriendlyDescription)
GO

CREATE NONCLUSTERED INDEX IX_Item_POSDescription ON dbo.Item (POSDescription)
GO

CREATE NONCLUSTERED INDEX IX_Item_FoodStamp ON dbo.Item (FoodStamp)
GO