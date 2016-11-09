DECLARE @taxRomanceId INT
Declare @taxHierarchyID int
			SELECT @taxRomanceId = (SELECT traitID from [dbo].[Trait] WHERE TraitCode = 'TRM')
			SET @taxHierarchyID			= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Tax');

INSERT INTO dbo.HierarchyClassTrait (traitID, hierarchyClassID, uomID, traitValue)
			SELECT @taxRomanceId, hc.hierarchyClassID, null, hc.hierarchyClassName
			FROM dbo.hierarchyClass hc
			WHERE hc.hierarchyID = @taxHierarchyID
			AND NOT EXISTS (SELECT 1 FROM dbo.hierarchyClassTrait hct WHERE hc.hierarchyClassID = hct.hierarchyClassID and hct.traitid = @taxRomanceId)