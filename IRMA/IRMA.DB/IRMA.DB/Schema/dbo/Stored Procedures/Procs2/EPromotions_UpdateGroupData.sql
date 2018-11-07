CREATE PROCEDURE [dbo].[EPromotions_UpdateGroupData]
	@GroupId int,
	@GroupName varchar(255),
	@GroupLogic tinyint,
	@Createdate smalldatetime,
	@ModifiedDate smalldatetime,
	@User_Id int
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

    -- update PromotionOffer data
    UPDATE ItemGroup
    SET GroupName=@GroupName,
	GroupLogic=@GroupLogic,
	ModifiedDate=@ModifiedDate ,
	User_id=@User_id 
    WHERE Group_Id = @GroupId
    SELECT @error_no = @@ERROR


    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdateGroupData failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdateGroupData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdateGroupData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_UpdateGroupData] TO [IRMAReportsRole]
    AS [dbo];

