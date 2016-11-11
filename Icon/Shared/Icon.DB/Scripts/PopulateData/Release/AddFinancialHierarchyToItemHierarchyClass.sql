-- Product Backlog Item 17189: In the Infor New Item Service unaligned sub teams are generating empty sub teams in item message to Infor
-- Author: Matthew Scherping & Blake Jones
-- Date: 18-JUL-2016
-- Adds item to financial hierarchy data into the itemhiearchyclass table for infor

declare @scriptKey varchar(128)

set @scriptKey = 'Add Financial Hierarchy to ItemHierarchyClass'

IF(NOT exists(Select * from app.PostDeploymentScriptHistory where ScriptKey = @scriptKey))
BEGIN	

	declare @wfmLocaleID int = (select localeID from Locale l where l.localeName = 'Whole Foods')
	declare @merchandiseClassID	int	= (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Merchandise')
	declare @financialClassID int = (SELECT h.hierarchyID FROM Hierarchy h WHERE h.hierarchyName = 'Financial')
	declare @merchFinMappingTraitID	int = (SELECT t.traitID FROM Trait t WHERE t.traitCode = 'MFM')
	declare @subBrickHierarchyLevel int = (SELECT hp.hierarchyLevel FROM HierarchyPrototype hp WHERE hierarchyLevelName = 'Sub Brick')

	insert into dbo.ItemHierarchyClass(itemID, hierarchyClassID, localeID)
	select 
		i.itemID,
		fin.hierarchyClassID,
		@wfmLocaleID
	from Item i
	join ScanCode sc on i.itemID = sc.itemID
	join ItemHierarchyClass ihc on i.itemID = ihc.itemID
	join HierarchyClass hc on ihc.hierarchyClassID = hc.hierarchyClassID
		and hc.hierarchyID = @merchandiseClassID
		and hc.hierarchyLevel = @subBrickHierarchyLevel
	join HierarchyClassTrait hct on hc.hierarchyClassID = hct.hierarchyClassID
		and hct.traitID = @merchFinMappingTraitID
	join HierarchyClass fin on hct.traitValue = fin.hierarchyClassName
		and fin.hierarchyID = @financialClassID
	where not exists
	(
		select * from ItemHierarchyClass existingFin
		where sc.itemID = existingFin.itemID
			and fin.hierarchyClassID = existingFin.hierarchyClassID
	)

	insert into app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@scriptKey, GETDATE())

END
ELSE
BEGIN
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @scriptKey
END
GO