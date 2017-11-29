CREATE TYPE infor.ValidateHierarchyClassType AS TABLE
(
	ActionName NVARCHAR(20) NOT NULL,
	HierarchyClassId INT NOT NULL,
	HierarchyClassName NVARCHAR(255) NOT NULL,
	HierarchyLevelName NVARCHAR(100) NOT NULL,
	HierarchyName NVARCHAR(100) NOT NULL,
	HierarchyParentClassId INT NULL,
	SubBrickCode NVARCHAR(255) NULL,
	SequenceId NUMERIC(22, 0) NULL
)