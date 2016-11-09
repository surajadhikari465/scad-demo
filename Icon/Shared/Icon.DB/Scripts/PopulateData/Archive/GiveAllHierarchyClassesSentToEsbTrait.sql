declare @HierarchyTraitGroupId int = (select traitGroupID from TraitGroup where traitGroupCode = 'HYT')

if exists (select traitID from Trait t where t.traitDesc = 'Sent To ESB')
begin
	update trait set traitpattern = '^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$' where traitDesc = 'Sent To ESB'
end
else
begin
	insert into Trait values ('ESB', '^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$', 'Sent To ESB', @HierarchyTraitGroupId)
end

DECLARE @traitID int;
SET @traitID = (select traitID from Trait t where t.traitDesc = 'Sent To ESB');

INSERT INTO HierarchyClassTrait
SELECT
       @traitID as traitID,
       hc.hierarchyClassID as hierarchyClassID,
       NULL as uomID,
       sysdatetime() as traitValue
FROM
       HierarchyClass hc
