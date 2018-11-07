CREATE PROCEDURE  [dbo].[EPromotions_InsertGroupData]
	@GroupName varchar(255),
	@GroupLogic int, 
	@CreateDate datetime, 
	@ModifiedDate datetime, 
	@User_Id int,
	@GroupID int OUTPUT 
AS 

BEGIN
    SET NOCOUNT ON

    BEGIN TRAN

    DECLARE @error_no int
    SELECT @error_no = 0

	-- Make sure no existing offer has the same description - return error if it does
	Select @error_no = COUNT(Group_Id)* -1
	FROM ItemGroup
	WHERE [GroupName] = @Groupname

    -- insert new Promotional Offer
    IF (@error_no = 0)
    BEGIN
	INSERT INTO ItemGroup
		(GroupName,
		GroupLogic ,
		CreateDate ,
		ModifiedDate ,
		User_Id )
	VALUES (@GroupName ,
		@GroupLogic ,
		@CreateDate ,
		@ModifiedDate ,
		@User_Id )
		
	SELECT @error_no = @@ERROR, @GroupID = SCOPE_IDENTITY()
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	COMMIT TRAN
    ELSE
	BEGIN
		DECLARE @Severity smallint
		If @error_no < 0 
			-- User defined error
			RAISERROR ('EPromotions_InsertGroupData failed: ''GroupName'' must be unique. ', 15, 1, @error_no)
		Else
		BEGIN
			IF @@TRANCOUNT <> 0
				ROLLBACK TRAN
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('EPromotions_InsertGroupData failed with @@ERROR: %d', @Severity, 1, @error_no)
		END
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertGroupData] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertGroupData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_InsertGroupData] TO [IRMAReportsRole]
    AS [dbo];

