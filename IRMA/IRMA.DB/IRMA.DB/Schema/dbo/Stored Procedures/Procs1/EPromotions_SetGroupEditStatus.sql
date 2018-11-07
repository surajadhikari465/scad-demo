CREATE PROCEDURE  dbo.EPromotions_SetGroupEditStatus
	@GroupId int,
	@Status int
AS 

BEGIN
    SET NOCOUNT ON
    
	--if @Status = -99 then the group is being marked Available to edit. (null)	
    if @Status=-99 	 set @Status = null
	

	-- sets the IsEdited field for a specific Group. null = Available to be edited. integer > 0 = UserId of the user that is currently editing the group.
	Update ItemGroup 
	Set IsEdited = @Status
	Where Group_Id = @GroupId

    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_SetGroupEditStatus] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_SetGroupEditStatus] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_SetGroupEditStatus] TO [IRMAReportsRole]
    AS [dbo];

