CREATE PROCEDURE [dbo].[LinkGroupsSearch]
	@LinkGroupName varchar(255),
	@LinkGroupDesc varchar(255),
	@ModifierPLU varchar(13),
	@ModifierName varchar(255),
	@Regions varchar(255)
AS
BEGIN

IF @LinkGroupName IS NULL SET @LinkGroupName = ''
IF @LinkGroupDesc IS NULL SET @LinkGroupDesc = ''
IF @ModifierPLU IS NULL SET @ModifierPLU = ''
IF @ModifierName IS NULL SET @ModifierName = ''
IF @Regions IS NULL SET @Regions = ''


SELECT 
	lg.LinkGroupId
	,lg.GroupName
	,lg.GroupDescription
	,RegionalAssociation = 'TBD'
	,Action = 'edit delete copy'
FROM LinkGroup lg
INNER JOIN LinkGroupItem lgi ON lg.LinkGroupId = lgi.LinkGroupId
INNER JOIN Items i ON lgi.ItemId = i.ItemId
WHERE (
		(@LinkGroupName = '')
		OR (lg.GroupName LIKE '%' + @LinkGroupName + '%')
	    )
	AND (
		(@LinkGroupDesc = '')
		OR (lg.GroupDescription LIKE '%' + @LinkGroupDesc + '%')
		)
	AND (
		(@ModifierPLU = '')
		OR (i.ScanCode LIKE '%' + @ModifierPLU + '%')
		)
	AND (
		(@ModifierName = '')
		OR (i.ProductDesc LIKE '%' + @ModifierName + '%')
		)

END

