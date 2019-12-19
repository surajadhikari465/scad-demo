CREATE PROCEDURE [app].[GetNationalClassForClassCode]	
    @NationalClassCode NVARCHAR(255)

AS
BEGIN
    SET NOCOUNT ON;				
		DECLARE @classCodeTraitId INT;
		SELECT 	@classCodeTraitId = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'NCC');
		
		DECLARE @nationalHierarchyId INT;
		SELECT 	@nationalHierarchyId = (SELECT hierarchyID FROM dbo.Hierarchy WHERE hierarchyName = 'National');

				
		select hc.* from HierarchyClass hc
		join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID and traitID = @classCodeTraitId
		where hc.hierarchyID = @nationalHierarchyId
			and hct.traitValue = @NationalClassCode
		

    SET NOCOUNT OFF;
END;

GO