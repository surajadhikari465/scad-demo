CREATE PROCEDURE dbo.Scale_DeleteLabelStyle
	@ID int

AS 

/**********************************************************************************************************************************************************************************************************************************
Revision History
DEV				DATE			TASK		Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Min Zhao		Aug 3, 2011	    2621		The stored proc was modified to check if there're any active items using the label style first. If there still are, the stored proc will raise an error since the  
                                            label style cannot be deleted. Otherwise, it will remove the label style reference on the scale item, and then the label style can be deleted.  
Kyle Milner		2013-01-31		9382		Check the ItemScaleOverride for any active LabelStyles before allowing them to be deleted;
**********************************************************************************************************************************************************************************************************************************/

BEGIN 
    SET NOCOUNT ON
    SET XACT_ABORT ON
	
	DECLARE @Error int
	DECLARE @ActiveItemCount int
	
	SELECT @ActiveItemCount = COUNT(*) FROM Item
     WHERE 
		Deleted_Item = 0 AND
		(Item_Key IN (SELECT Item_Key FROM ItemScale WHERE Scale_LabelStyle_ID = @ID) OR
			(Item_Key IN (SELECT Item_Key FROM ItemScaleOverride WHERE Scale_LabelStyle_ID = @ID)))	
	
	IF @ActiveItemCount > 0
	    BEGIN
			RAISERROR('Scale label style cannot be deleted. This style is still used by active items.', 4, 1)
			GOTO Done
		END
	ELSE	
		BEGIN TRAN 
			
			UPDATE ItemScale
			   SET Scale_LabelStyle_ID = NULL
			 WHERE Scale_LabelStyle_ID = @ID
			
			SET @Error = @@ERROR
			
			IF @Error <> 0
			BEGIN
				GOTO Done
			END 
			
			UPDATE ItemScaleOverride
			   SET Scale_LabelStyle_ID = NULL
			 WHERE Scale_LabelStyle_ID = @ID	 
			
			SET @Error = @@ERROR
			
			IF @Error <> 0
			BEGIN
				GOTO Done
			END 
			
			DELETE  
				Scale_LabelStyle 
			WHERE 
				Scale_LabelStyle_ID = @ID
			
			SET @Error = @@ERROR
			
			IF @Error <> 0
			BEGIN
				GOTO Done
			END 
			
		COMMIT
		
		Done:
			IF @Error <> 0
			BEGIN
				ROLLBACK
			END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Scale_DeleteLabelStyle] TO [IRMAClientRole]
    AS [dbo];

