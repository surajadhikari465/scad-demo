CREATE Procedure DeleteKitByKitId
@KitId int
AS
BEGIN

    DELETE KitInstructionList
	WHERE KitId = @KitId

	DELETE KitLinkGroupItem
	WHERE KitLinkGroupId in ( SELECT KitLinkGroupId FROM KitLinkGroup
							  WHERE kitid = @KitId ) 
	DELETE KitLinkGroup 
	WHERE kitid = @KitId

	DELETE Kit
	WHERE kitid = @KitId
END