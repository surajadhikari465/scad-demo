CREATE TYPE infor.HierarchyClassTraitType AS TABLE
(
	HierarchyClassId INT NOT NULL,
	TraitId INT NOT NULL,
	TraitValue NVARCHAR(255) NOT NULL
)