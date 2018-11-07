CREATE PROCEDURE [dbo].[EPromotions_ValidateGroupName]
	@GroupName varchar(255),
	@DeletedGroupIds varchar(255)
	
AS 

BEGIN
    SET NOCOUNT ON

	-- Return False if GroupName already exists. Return True if it does not.
	declare @sql varchar(1000)
	
	set @sql = 'SELECT CASE WHEN EXISTS (SELECT GroupName FROM ItemGroup WHERE GroupName = ''' + @GroupName + ''' and Group_Id not in (' + @DeletedGroupIds + ')) THEN ''false'' ELSE ''true'' END AS isValid '
	exec(@sql)


END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ValidateGroupName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ValidateGroupName] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_ValidateGroupName] TO [IRMAReportsRole]
    AS [dbo];

