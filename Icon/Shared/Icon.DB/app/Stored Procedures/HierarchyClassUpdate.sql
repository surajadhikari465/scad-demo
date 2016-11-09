
CREATE PROCEDURE app.[HierarchyClassUpdate]
	@HierarchyClassID	INT,
	@HierarchyClassName NVARCHAR(255)
AS
	UPDATE HierarchyClass
		SET hierarchyClassName = @HierarchyClassName
		WHERE @HierarchyClassID = @HierarchyClassID