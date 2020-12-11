SET NOCOUNT ON;
DECLARE @scriptKey VARCHAR(128) = 'SCM2-292_UpdateCoffeeSubteamFinancialHierarchy';
IF (NOT EXISTS (SELECT *FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
  BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + @scriptKey;

	DECLARE @hierarchyId INT = (
			SELECT [HierarchyId]
			FROM Hierarchy
			WHERE HierarchyName = 'Financial');--5

	DECLARE @NumTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NUM');--TeamNumber: 63

	DECLARE @NamTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NAM') --TeamName: 64

	DECLARE @subteamId INT = (
			SELECT hierarchyClassID
			FROM HierarchyClass
			WHERE hierarchyClassName = 'Coffee (2400)')

	DECLARE @financialHierarchyClassLevel INT = (
		    SELECT hierarchyLevel
			FROM HierarchyPrototype
			WHERE hierarchyID = @hierarchyId);

	BEGIN TRY
      BEGIN TRANSACTION;

	  UPDATE hct
	  SET traitValue = '100'
	  from HierarchyClassTrait hct
	  WHERE hierarchyClassID = @subteamId AND traitID = @NumTraitId

	  UPDATE hct
	  SET traitValue = 'Grocery'
	  from HierarchyClassTrait hct
	  WHERE hierarchyClassID = @subteamId AND traitID = @NamTraitId

	  INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime)
	  VALUES (@scriptKey , GETDATE());

      COMMIT TRANSACTION;
	  PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Completed: ' + @scriptKey;

    END TRY
    BEGIN CATCH
	  PRINT 'Error:';
      SELECT ERROR_NUMBER() AS ErrorNumber
	  ,ERROR_SEVERITY() AS ErrorSeverity
	  ,ERROR_STATE() AS ErrorState
	  ,ERROR_PROCEDURE() AS ErrorProcedure
	  ,ERROR_LINE() AS ErrorLine
	  ,ERROR_MESSAGE() AS ErrorMessage;
      ROLLBACK TRANSACTION;
    END CATCH
  END
ELSE
BEGIN
  PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Update data already applied: ' + @scriptKey;
END