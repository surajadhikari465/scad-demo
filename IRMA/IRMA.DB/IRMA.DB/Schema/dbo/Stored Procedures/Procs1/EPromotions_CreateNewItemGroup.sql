CREATE  PROCEDURE [dbo].[EPromotions_CreateNewItemGroup]
 	@GroupName varchar(255),
 	@GroupLogic int, 
 	@UserId int
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN CreateNewItemGroup

    DECLARE @error_no int
    SELECT @error_no = 0
    

	Insert Into ItemGroup
	(
		GroupName,
		GroupLogic, 
		createdate, 
		modifieddate, 
		[User_Id]
	)
	 VALUES
	(	
		@GroupName,
		@GroupLogic,
		getdate(),
		getdate(),
		@UserId
	);

	
	 select scope_identity() as NewId
	
    SELECT @error_no = @@ERROR

    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN CreateNewItemGroup
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN CreateNewItemGroup
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('EPromotions_CreateNewItemGroup failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_CreateNewItemGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_CreateNewItemGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_CreateNewItemGroup] TO [IRMAReportsRole]
    AS [dbo];

