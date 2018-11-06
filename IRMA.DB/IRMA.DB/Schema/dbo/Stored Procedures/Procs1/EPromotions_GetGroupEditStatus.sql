CREATE PROCEDURE  dbo.EPromotions_GetGroupEditStatus
	@GroupId int
AS 

BEGIN
    SET NOCOUNT ON
	
	SELECT EditUserId=IsEdited, IsEdited=case when IsEdited is null then 'No' else 'Yes' end , EditedBy=case when IsEdited is not null then IsNull(FullName, 'Unknown') else null end
	FROM ItemGroup left join Users on ItemGroup.IsEdited = Users.User_Id
	WHERE Group_Id =  @GroupId
    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetGroupEditStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetGroupEditStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_GetGroupEditStatus] TO [IRMAReportsRole]
    AS [dbo];

