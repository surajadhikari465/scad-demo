declare @scriptKey varchar(128)

-- Product Backlog Item 17157: As IRMA I need to purge NoTagThreshold table automatically
set @scriptKey = 'FixSupplementsAndSpiritSubTeams'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + @scriptKey
	
	--This script will align the trait value of the Financial Code of these two Hierarchy Classes.
	--Right now they are incorrectly set as 2500 and 3500 but they should be 2220 and 3000
	update HierarchyClassTrait 
	set traitValue = '2220'
	where hierarchyClassID = 84350 -- HierarchyClass ID for Spirits (2220)
		and traitID = 53 -- Financial Code Trait ID
	update HierarchyClassTrait 
	set traitValue = '3000'
	where hierarchyClassID = 84236 -- HierarchyClass ID for Supplements (3000)
		and traitID = 53 -- Financial Code Trait ID

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO