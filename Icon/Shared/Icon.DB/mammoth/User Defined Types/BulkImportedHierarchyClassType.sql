CREATE TYPE [mammoth].[BulkImportedHierarchyClassType] AS TABLE (
    [HierarchyClassId] INT NULL,
    [HierarchyClassName] NVARCHAR (100) NOT NULL,
	[MammothEventTypeId] INT NOT NULL);