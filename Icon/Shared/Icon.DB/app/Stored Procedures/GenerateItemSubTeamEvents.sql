CREATE PROCEDURE [app].[GenerateItemSubTeamEvents] 
	@updatedItemIDs app.UpdatedItemIDsType readonly
AS
BEGIN
	DECLARE @distinctItemIDs app.UpdatedItemIDsType,			
			@itemSubteamUpdateEventType int,			
			@merchandiseHierarcyId int,
			@itemUpdateSetting int;			


	SET @itemSubteamUpdateEventType = (select EventId from app.EventType where EventName = 'Item Sub Team Update')

	SET @merchandiseHierarcyId = (select hierarchyID from Hierarchy where hierarchyName = 'Merchandise');

	INSERT @distinctItemIDs 
	SELECT DISTINCT uii.itemID 
	FROM @updatedItemIDs uii
	JOIN ItemHierarchyClass ihc
		ON uii.itemID = ihc.itemID
	JOIN HierarchyClass hc
		ON hc.hierarchyClassID = ihc.hierarchyClassID
		AND hc.hierarchyID = @merchandiseHierarcyId
	
	SET @itemUpdateSetting = (select s.SettingsId from app.Settings s
	                      join app.SettingSection ss on s.SettingSectionId = ss.SettingSectionId and ss.SectionName = 'Item'
						  where s.KeyName = 'SendItemSubTeamupdatesToIRMA');

	WITH config_CTE AS
	( 
		SELECT r.RegionCode  from app.RegionalSettings rs 
			join app.Regions r on rs.RegionId = r.RegionId
			where rs.SettingsId = @itemUpdateSetting and rs.Value = 1
	)		

	INSERT INTO app.EventQueue
	SELECT @itemSubteamUpdateEventType, sc.scanCode, dii.itemID, config.regionCode, sysdatetime(), null, null
	FROM @distinctItemIDs dii
	JOIN ScanCode sc
		ON sc.itemID = dii.itemID 
	JOIN Item i
		ON sc.itemID = i.itemID
	JOIN  config_CTE config 
		ON config.RegionCode = config.RegionCode
END