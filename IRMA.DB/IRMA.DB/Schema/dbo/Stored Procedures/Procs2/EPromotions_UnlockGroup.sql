CREATE PROCEDURE  dbo.EPromotions_UnlockGroup
	@GroupId int
AS 

BEGIN
    SET NOCOUNT ON
 
 UPDATE Itemgroup 
 SET IsEdited=null
 WHERE Group_ID = @GroupId
 
 SET NOCOUNT OFF
 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UnlockGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UnlockGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UnlockGroup] TO [IRMAReportsRole]
    AS [dbo];

