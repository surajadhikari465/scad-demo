CREATE Procedure DeleteLinkGroupByLinkGroupId
@linkGroupId int
AS
BEGIN

DELETE LinkGroupItem 
WHERE LinkGroupId = @linkGroupId

DELETE LinkGroup 
WHERE LinkGroupId = @linkGroupId

END