
-- This script was created using WinSQL Professional
-- Timestamp: 1/10/2009 5:24:32 PM

-- Total Objects:    1
-- Total Tables:     0
-- Total Views:      0
-- Total Procedures: 1

--Object: Procedure: EIM_UploadSession_DeleteItems;1 - Script Date: 1/10/2009 5:24:32 PM
CREATE PROCEDURE [dbo].[EIM_UploadSession_DeleteItems]
	@UploadSession_ID int,
	@UploadRow_ID int,
	@RetryCount int,
	@LoggingLevel varchar(10),
	@Item_key int,	
	@UserID int				-- Task 2178
AS
	set nocount on

	-- ******************************************************
	-- Called by EIM to delete items.
	--
	-- Calls DeleteItemInventory stored procedure
	-- and supplies current date as delete date
	--
	-- Alex Z - 01/08/2009
	-- @UploadSession_ID, @UploadRow_ID, @RetryCount, @Item_Key
	-- ******************************************************
	
	-- ******************************************************
	-- Declare and Set Variables
	
	Declare @DeleteDate datetime
	
	Set @DeleteDate = convert(date, getdate()) 
	-- ******************************************************
	
	
	-- ******************************************************
		
	EXEC dbo.EIM_Log 
		@LoggingLevel, 
		'TRACE', 
		@UploadSession_ID, 
		@UploadRow_ID, 
		@RetryCount, 
		@Item_key, 
		NULL, 
		'Delete Item Process - [Begin]'
	
	
	
	BEGIN TRY
			
	EXEC dbo.DeleteItemInventory
		@Item_Key,
		@UserID,				 -- Task 2178
		@DeleteDate			
			
	EXEC dbo.EIM_Log 
		@LoggingLevel, 
		'TRACE', 
		@UploadSession_ID, 
		@UploadRow_ID, 
		@RetryCount, 
		@Item_key, 
		NULL, 
		'Executing SP - Success  - [DeleteItemInventory]'
				
	END TRY
	BEGIN CATCH

	EXEC dbo.EIM_Log 
		@LoggingLevel, 
		'ERROR', 
		@UploadSession_ID, 
		@UploadRow_ID, 
		@RetryCount, 
		@Item_key, 
		NULL, 
		'Error Executing - [DeleteItemInventory]'
	
	END CATCH
		
	EXEC dbo.EIM_Log 
	@LoggingLevel, 
	'TRACE', 
	@UploadSession_ID, 
	@UploadRow_ID, 
	@RetryCount, 
	@Item_key, 
	NULL,
	'Delete Item Process - [End]'
  
	-- ****************************************************** 


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_UploadSession_DeleteItems] TO [IRMAClientRole]
    AS [dbo];

