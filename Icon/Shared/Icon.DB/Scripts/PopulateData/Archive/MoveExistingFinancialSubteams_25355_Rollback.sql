SET NOCOUNT ON;

DECLARE @NumTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NUM');--TeamNumber: 63

	DECLARE @NamTraitId INT = (
			SELECT TraitID
			FROM Trait
			WHERE TraitCode = 'NAM'); --TeamName: 64

DELETE FROM HierarchyClassTrait
WHERE hierarchyClassID IN(84348, 84349) AND traitID IN(@NumTraitId, @NamTraitId);

DELETE FROM app.PostDeploymentScriptHistory WHERE ScriptKey = 'MoveExistingFinancialSubteams_25355';

SET NOCOUNT OFF;