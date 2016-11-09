CREATE TYPE infor.ItemHierarchyClassAddOrUpdateType AS TABLE
(
	ItemId INT NOT NULL,
	HierarchyId INT NOT NULL,
	HierarchyClassId nvarchar(255) NOT NULL
)