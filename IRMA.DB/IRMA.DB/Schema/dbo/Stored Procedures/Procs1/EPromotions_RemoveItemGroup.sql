CREATE  PROCEDURE [dbo].[EPromotions_RemoveItemGroup]
 	@GroupId Int
AS 

BEGIN
	SET NOCOUNT ON
	
	BEGIN TRAN RemoveItemGroup
	
	DECLARE @error_no int
	SELECT @error_no = 0
    
	-- Remove All Items associated with the Group being Deleted.
	
	DELETE FROM ItemGroupMembers 
	WHERE Group_Id = @GroupId
	
	-- Remove the ItemGroup Record
	DELETE FROM ItemGroup 
	WHERE Group_Id = @GroupId

	SELECT @error_no = @@ERROR

	SET NOCOUNT OFF
	
	IF @error_no = 0
		COMMIT TRAN RemoveItemGroup
	ELSE
		BEGIN
			IF @@TRANCOUNT <> 0
	    			ROLLBACK TRAN CreateNewItemGroup
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
			RAISERROR ('EPromotions_RemoveItemGroup failed with @@ERROR: %d', @Severity, 1, @error_no)
		END
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_RemoveItemGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_RemoveItemGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EPromotions_RemoveItemGroup] TO [IRMAReportsRole]
    AS [dbo];

