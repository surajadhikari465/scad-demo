/*
PBI 22265: As GDT I do NOT want to assign tax classes via the merchandise hierarchy associated to an item.
Remove the data from the HierarchyClassTrait table that stores this data
  TraitCode MDT, TraitDesc 'Merch Default Tax Associatation'
  The merchandise hierarchy no longer needs this trait to drive tax class associations if it exists.
*/
DECLARE @scriptKey VARCHAR(128) = 'Remove_MDT_Trait';
IF NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey)
BEGIN
  print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Executing: ' + @scriptKey;
  declare @mdtID INT = (select traitID from Trait where traitCode = 'MDT');
  delete from HierarchyClassTrait where traitID = @mdtID;
  INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey, GETDATE());
  print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Completed: ' + @scriptKey;
END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Already applied: ' + @scriptKey
END
GO