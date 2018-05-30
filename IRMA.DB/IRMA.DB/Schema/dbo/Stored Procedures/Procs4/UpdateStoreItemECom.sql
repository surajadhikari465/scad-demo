CREATE PROCEDURE dbo.UpdateStoreItemECom
    @Item_Key		int, 
    @Store_No		int, 
	@AuthorizedItem bit,
	@Refresh		bit,
	@ECommerce		bit = 0

AS 

/***************************************************************************
Procedure:		UpdateStoreItemEom

Description:    Updates the store item table with various flags

Modification History:

Date		Init		Task		Comment
12.28.09	shurbetr				Add update to new Refresh column
05.02.12	BJL						Added transactional logic to the code and added 
									insert code to update the nutrifact/extratext change
									queues when an item is authorized for a store
2013-02-01	KM			9393		Check ItemScaleOverride when inserting to the Nutrifact change queue (taking the same
									logic that's used in the ExtraText change queue insert);
10/22/2013	DN			13402		Add ECommerce parameter. Use only by IRMA Client. 
**************************************************************************/

BEGIN
    DECLARE @Error_No int    
    SELECT @Error_No = 0    
    
BEGIN TRAN
	SELECT @Error_No = @@ERROR  
	
	--Using the regional scale file?
	DECLARE @UseRegionalScaleFile bit
	SELECT @UseRegionalScaleFile = (SELECT FlagValue FROM InstanceDataFlags WHERE FlagKey='UseRegionalScaleFile')

	-- Using store jurisdictions for override values?
	DECLARE @UseStoreJurisdictions int
	SELECT @UseStoreJurisdictions = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'UseStoreJurisdictions'  

	IF @Error_No = 0    
		BEGIN    
			-- Create the StoreItem relationship if it does not already exist for the Store-Item
			IF NOT EXISTS(SELECT * FROM StoreItem WHERE Item_Key = @Item_Key AND Store_No = @Store_No)
				INSERT INTO StoreItem (Item_Key, Store_No) VALUES (@Item_Key, @Store_No)
			SELECT @Error_No = @@ERROR    
		END
   
   	IF @Error_No = 0    
		BEGIN
			-- Update the values on the StoreItem table that maintains Store-Item relationship data
			UPDATE StoreItem
			SET
				Authorized	=	@AuthorizedItem,
				Refresh		=	@Refresh,
				ECommerce	=	@ECommerce
			WHERE Item_Key = @Item_Key AND Store_No = @Store_No
		END

	IF @Error_No = 0    
		BEGIN    
			
			-- And add it to NutrifactsChgQueue
			IF @AuthorizedItem = 1
				BEGIN
				
					INSERT INTO NutrifactsChgQueue (NutriFactsID, ActionCode, Store_No)
						SELECT
							ISNULL(ISO.Nutrifact_ID, ItemScale.Nutrifact_ID), 
							'A', 
							@Store_No
						
						FROM 
							Store S
							INNER JOIN	StoreItem SI					ON S.Store_No							= SI.Store_No
																		AND SI.Item_Key							= @Item_Key
																		AND SI.Store_No							= @Store_No
							INNER JOIN	ItemIdentifier					ON ItemIdentifier.Item_Key				= SI.Item_Key
																		AND ItemIdentifier.Scale_Identifier		= 1		-- this is a scale item
							INNER JOIN	ItemScale						ON ItemIdentifier.Item_Key				= ItemScale.Item_Key
							LEFT JOIN	ItemScaleOverride ISO (nolock)	ON	ISO.Item_Key						= ItemScale.Item_Key
																		AND ISO.StoreJurisdictionID				= S.StoreJurisdictionID
																		AND @UseRegionalScaleFile				= 0
																		AND @UseStoreJurisdictions				= 1
						
						WHERE 
							(S.WFM_Store = 1 OR S.Mega_Store = 1)
							AND ItemScale.Nutrifact_ID IS NOT NULL			
							AND NOT EXISTS (SELECT * FROM NutrifactsChgQueue NQ WHERE NQ.NutriFactsID = ItemScale.Nutrifact_ID AND NQ.ActionCode = 'A' AND NQ.Store_No = @Store_No)
							AND NOT EXISTS (SELECT * FROM NutrifactsChgQueueTmp NQT WHERE NQT.Nutrifact_ID = ItemScale.Nutrifact_ID AND NQT.ActionCode = 'A' AND NQT.Store_No = @Store_No)
				END
		END
	
	IF @Error_No = 0    
		BEGIN    
			-- And add it to Scale_ExtraTextChgQueue
			IF @AuthorizedItem = 1
				BEGIN
							
					INSERT INTO Scale_ExtraTextChgQueue (Scale_ExtraText_ID, ActionCode, Store_No)
						SELECT 
							ISNULL(ISO.Scale_ExtraText_ID, ItemScale.Scale_ExtraText_ID), 
							'A', 
							@Store_No
						
						FROM 
							Store S
							INNER JOIN	StoreItem SI					ON	S.Store_No						= SI.Store_No
																		AND SI.Item_Key						= @Item_Key
																		AND SI.Store_No						= @Store_No
							INNER JOIN	ItemIdentifier					ON  ItemIdentifier.Item_Key			= SI.Item_Key
																		AND ItemIdentifier.Scale_Identifier = 1		-- this is a scale item
							INNER JOIN	ItemScale						ON  ItemIdentifier.Item_Key			= ItemScale.Item_Key
							LEFT JOIN	ItemScaleOverride ISO (nolock)	ON	ISO.Item_Key					= ItemScale.Item_Key
																		AND ISO.StoreJurisdictionID			= S.StoreJurisdictionID
																		AND @UseRegionalScaleFile			= 0
																		AND @UseStoreJurisdictions			= 1
						
						WHERE 
							(S.WFM_Store = 1 OR S.Mega_Store = 1)
							AND ISNULL(ISO.Scale_ExtraText_ID, ItemScale.Scale_ExtraText_ID) IS NOT NULL		
							AND NOT EXISTS (SELECT * FROM Scale_ExtraTextChgQueue SETQ WHERE SETQ.Scale_ExtraText_ID = ItemScale.Scale_ExtraText_ID AND SETQ.ActionCode = 'A' AND SETQ.Store_No = @Store_No)
							AND NOT EXISTS (SELECT * FROM Scale_ExtraTextChgQueueTmp SETQT WHERE SETQT.Scale_ExtraText_ID = ItemScale.Scale_ExtraText_ID AND SETQT.ActionCode = 'A' AND SETQT.Store_No = @Store_No)
				END
		END
    
    IF @Error_No = 0    
		BEGIN    
			COMMIT TRAN    
			SET NOCOUNT OFF    
		END    
    ELSE    
		BEGIN    
			ROLLBACK TRAN    
			DECLARE @Severity smallint    
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)    
			SET NOCOUNT OFF    
			RAISERROR ('UpdateStoreItemECom failed with @@ERROR: %d', @Severity, 1, @Error_No)           
		END   
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoreItemECom] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoreItemECom] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoreItemECom] TO [IRMAReportsRole]
    AS [dbo];

