DECLARE @key VARCHAR(128) = 'DeduplicateNationalMerchandiseHierarchy';

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @key))
BEGIN
  SET NOCOUNT ON;
  DECLARE @ids AS TABLE(HierarchyClassID INT);
  
  if(object_id('tempdb..#tmpNational') IS NOT NULL) drop table #tmpNational;
  if(object_id('tempdb..#tmpMerchandise') IS NOT NULL) drop table #tmpMerchandise;
  
  BEGIN TRY
    BEGIN TRANSACTION;

    --32371: Deduplicate Hierarchy_NationalClass
    --Delete invalid records
    DELETE FROM dbo.Hierarchy_NationalClass
    OUTPUT deleted.*
    WHERE FamilyHCID IS NULL
      OR (CategoryHCID IS NULL AND SubcategoryHCID IS NULL AND ClassHCID IS NULL)
      OR (CategoryHCID IS NULL AND SubcategoryHCID IS NOT NULL)
      OR (SubcategoryHCID IS NULL AND ClassHCID IS NOT NULL);
    
    --Delete duplicate records
    ;WITH cte AS(
      SELECT HierarchyNationalClassID, Row_Number() OVER(PARTITION BY FamilyHCID, CategoryHCID, SubcategoryHCID, ClassHCID ORDER BY HierarchyNationalClassID ) rowID
      FROM dbo.Hierarchy_NationalClass)
        SELECT * INTO #tmpNational FROM cte WHERE rowID > 1;

    DELETE hn
    OUTPUT deleted.*
    FROM dbo.Hierarchy_NationalClass hn
    JOIN #tmpNational n on n.HierarchyNationalClassID = hn.HierarchyNationalClassID;
    
    --Reset invalid Items.HierarchyNationalClassID
    UPDATE i SET HierarchyNationalClassID = NULL
    FROM dbo.Items i
    LEFT JOIN dbo.Hierarchy_NationalClass h ON h.HierarchyNationalClassID = i.HierarchyNationalClassID
    WHERE i.HierarchyNationalClassID IS NOT NULL AND h.HierarchyNationalClassID IS NULL;
    
    
    --32842_Deduplicate_Hierarchy_Merchandise
    --Delete invalid records
    DELETE FROM dbo.Hierarchy_Merchandise
    OUTPUT deleted.*
    WHERE SegmentHCID IS NULL
      OR FamilyHCID IS NULL
      OR ClassHCID IS NULL
      OR BrickHCID IS NULL; --SubBrickHCID IS NULL; SubBrickHCID can be NULL. Verified with Ben and Kevin
    
    --Delete duplicate records
    ;WITH cte AS(
      SELECT HierarchyMerchandiseID, Row_Number() OVER(PARTITION BY SegmentHCID, FamilyHCID, ClassHCID, BrickHCID, SubBrickHCID ORDER BY HierarchyMerchandiseID) rowID
      FROM dbo.Hierarchy_Merchandise)
      SELECT * INTO #tmpMerchandise FROM cte WHERE rowID > 1;
    
    DELETE hm
    OUTPUT deleted.*
    FROM dbo.Hierarchy_Merchandise hm
    JOIN #tmpMerchandise m on m.HierarchyMerchandiseID = hm.HierarchyMerchandiseID;

    UPDATE i SET HierarchyMerchandiseID = NULL
    FROM dbo.Items i
    LEFT JOIN dbo.Hierarchy_Merchandise h ON h.HierarchyMerchandiseID = i.HierarchyMerchandiseID
    WHERE i.HierarchyMerchandiseID IS NOT NULL AND h.HierarchyMerchandiseID IS NULL;

    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@key ,GETDATE());
    COMMIT TRAN;
  END TRY
  BEGIN CATCH
	  ROLLBACK TRAN;

      DECLARE @ErrorMessage NVARCHAR(4000);
	  DECLARE @ErrorSeverity INT;
	  DECLARE @ErrorState INT;

	 SELECT 
	    @ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();
  END CATCH
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @key + ' already applied.'
END