
CREATE PROCEDURE dbo.DeletePLUMCorpChgQueueTmp
    @StoreList varchar(8000),
    @StoreListSeparator char(1)

AS

BEGIN
    -- If this region is using store scale systems, only clear the data from the pending queues if all stores that are
    -- authorized to sell the item have received the update.  If some stores still need to receive the update, leave the
    -- record in PLUMCorpChgQueueTmp for re-processing.
    -- This will result in duplicate records being sent to stores that were successful during the first attempt, but it 
    -- prevents stores from missing out on an update.
    
	-- Using the regional scale file?
	DECLARE @UseRegionalScaleFile bit
	SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey='UseRegionalScaleFile')
	
	IF @UseRegionalScaleFile = 1
	BEGIN
		-- Clear the queue.  This stored proc is not called if the regional store did not successfully receive the file.
		DELETE PLUMCorpChgQueueTmp
	END
	ELSE
	BEGIN
		DELETE FROM PLUMCorpChgQueueTmp 
		WHERE PLUMCorpChgQueueId IN (
									 SELECT pt.PLUMCorpChgQueueId
									 FROM PLUMCorpChgQueueTmp pt
									 JOIN PriceBatchDetail pbd ON pbd.Item_Key = pt.Item_Key
									 JOIN PriceBatchHeader pbh ON pbh.PriceBatchHeaderid = pbd.PriceBatchHeaderId
									 WHERE pbh.PriceBatchStatusId = (SELECT PriceBatchStatusId          
																	 FROM PriceBatchStatus 
																	 WHERE PriceBatchStatusDesc = 'Sent') AND		         
																	 pbd.Store_No = pt.Store_No AND
																	 pt.ActionCode = 'A'
				  					 )
				  					 
		DELETE FROM PLUMCorpChgQueueTmp 
		WHERE PLUMCorpChgQueueId IN (
									 SELECT pt.PLUMCorpChgQueueId
									 FROM PLUMCorpChgQueueTmp pt
									 WHERE pt.ActionCode in ('C','D')
				  					 )				  		
									 
		DELETE FROM dbo.PLUMCorpChgQueueTmp
		WHERE ActionCode = 'A' 
			AND Item_Key IN (
								SELECT Item_Key 
								FROM dbo.ItemCustomerFacingScale
							)

	END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePLUMCorpChgQueueTmp] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePLUMCorpChgQueueTmp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePLUMCorpChgQueueTmp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeletePLUMCorpChgQueueTmp] TO [IRMAReportsRole]
    AS [dbo];

