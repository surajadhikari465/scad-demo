CREATE TYPE infor.ValidateHierarchyClassType AS TABLE
(
	HierarchyClassId INT NOT NULL,
	HierarchyClassName NVARCHAR(255) NOT NULL,
	HierarchyLevelName NVARCHAR(100) NOT NULL,
	HierarchyName NVARCHAR(100) NOT NULL,
	HierarchyParentClassId INT NULL,
	SubBrickCode NVARCHAR(255) NULL
)