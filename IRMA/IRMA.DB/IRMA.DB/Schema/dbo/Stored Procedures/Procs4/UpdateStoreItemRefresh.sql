CREATE PROCEDURE dbo.UpdateStoreItemRefresh
    @Item_Key int, 
    @Store_No int, 
	@Refresh bit,
	@Reason varchar(50),
	@UserID int
AS 
	-- **************************************************************************
	-- Procedure: UpdateStoreItemRefresh()
	--    Author: Billy Blackerby
	--      Date: 12/29/2009
	--
	-- Description:
	-- This procedure is called from a SLIM.RefreshPOS
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 12/29/2009	BBB		Created
	-- **************************************************************************
BEGIN
   
    -- Create the StoreItem relationship if it does not already exist for the Store-Item
    IF NOT EXISTS(SELECT * FROM StoreItem WHERE Item_Key = @Item_Key AND Store_No = @Store_No)
		INSERT INTO StoreItem (Item_Key, Store_No) VALUES (@Item_Key, @Store_No)
		
	-- Check to see if a refresh was requested
	IF @Refresh <> (SELECT Refresh FROM StoreItem WHERE Item_Key  = @Item_Key AND Store_No = @Store_No)
		BEGIN
		    -- Update the values on the StoreItem table that maintains Store-Item relationship data
			UPDATE 
				StoreItem
			SET
				Refresh			=	@Refresh
			WHERE 
				Item_Key		= @Item_Key 
				AND Store_No	= @Store_No
		
			-- Collect StoreItemAuthorizationID
			DECLARE @StoreItemAuthID int 
			SET @StoreItemAuthID = (SELECT TOP 1 StoreItemAuthorizationID FROM StoreItem WHERE Item_Key = @Item_Key AND Store_No = @Store_No)

			-- Add Reason to reason tracking table		
			INSERT INTO
				StoreItemRefresh
			(
				StoreItemAuthorizationID,
				UserID,
				Reason
			)
			VALUES
			(
				@StoreItemAuthID,
				@UserID,
				@Reason
			)
		END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoreItemRefresh] TO [IRMASLIMRole]
    AS [dbo];

