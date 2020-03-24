DECLARE @scriptKey VARCHAR(128) = 'PopulateContactType_24426'

IF (NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN

IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'AR/AP')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('AR/AP')
IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'Chief Executive')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('Chief Executive')
IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'Marketing')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('Marketing')
IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'Operations Management')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('Operations Management')
IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'Product Safety Emergency Contact')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('Product Safety Emergency Contact')
IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'QA')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('QA')
IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'Sales Management')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('Sales Management')
IF NOT EXISTS (select 1 from dbo.ContactType where ContactTypeName = 'WFM Account Manager (Primary)')  
	INSERT INTO dbo.ContactType ( ContactTypeName ) Values ('WFM Account Manager (Primary)')

    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO