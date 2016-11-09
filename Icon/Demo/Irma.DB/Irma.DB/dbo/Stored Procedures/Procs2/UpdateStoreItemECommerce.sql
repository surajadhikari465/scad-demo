
CREATE PROCEDURE [dbo].[UpdateStoreItemECommerce] 
    @Item_Key		int, 
    @Store_No		int, 
	@Ecommerce		bit
AS 

/***************************************************************************
Procedure:		UpdateStoreItemECommerce
Author:			Faisal Ahmed
Date:			10/17/2013
Description:    Updates the ECommerce flag in store item table

Modification History:

Date		Init		Task		Comment
10.17.13	FA			14298		Initial Version
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
				ECommerce	=	@ECommerce
			WHERE Item_Key = @Item_Key AND Store_No = @Store_No
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
			RAISERROR ('UpdateStoreItemECommerce failed with @@ERROR: %d', @Severity, 1, @Error_No)           
		END   
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateStoreItemECommerce] TO [IRMASLIMRole]
    AS [dbo];

