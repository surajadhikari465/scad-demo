CREATE TYPE infor.HierarchyClassType AS TABLE
(
	HierarchyClassId INT NOT NULL,
	HierarchyClassName NVARCHAR(255) NOT NULL,
	HierarchyId INT NOT NULL,
	HierarchyLevelName nvarchar(255) NOT NULL,
	ParentHierarchyClassId INT NULL
)