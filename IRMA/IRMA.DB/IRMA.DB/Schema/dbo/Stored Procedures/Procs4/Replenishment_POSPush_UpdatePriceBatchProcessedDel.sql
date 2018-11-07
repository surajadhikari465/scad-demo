CREATE PROCEDURE  [dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedDel]
	@PriceBatchHeaderIds BatchIdsType READONLY
AS

BEGIN
/*********************************************************************************************************************************************
CHANGE LOG
DEV				DATE					TASK				Description
----------------------------------------------------------------------------------------------
DBS				20110302				13835				Remove cursor, call to POSDeleteItem, Create Table Var for performance
DBS				20110210				13835				Speed up delete process by removing locks for deletes
Bill Carswell	1/14/2008									When an item is deleted, price batch detail delete records are generated for
															all the stores, even those stores where the item is not authorized for sale
															(if there are any).  The result was that the item never got marked for deletion 
															because those extra price batch detail records (for the unauthorized stores) never 
															got processed (they are not picked up by the batching process).  The INNER JOIN on 
															StoreItem in the NOT EXISTS clause below was added to handle that situation.  Now, 
															once the batched deletion records for all the authorized stores get processed, the 
															others will be deleted in the subsequent logic.
BAS				01/07/2013				8755				Removed reference to Item.DiscontinueItem because of schema change.  This flag
															doesn't need to get updated because the StoreItemVendor record is being removed
															which is where the Discontinue flag is hosted now
BJL				04/26/2013				12047				Added delete for NutriFacts
MZ              01/22/2013              14400               Wipe out Brand ID from Item and ItemOverride when item is deleted.
Jamali			07/18/2016				PBI16852, 17025		Added the procedure @PriceBatchHeaderIds to process the changes at one time
															Removed the @Error clause with Begin Try, End Try statement
MJS				09/19/2016				PBI 8814			Removing identifiers from validated scan codes.
Jamali			11/1/2016				PBI 18900			Removed the PriceBatchHeaderId and POSBatchId parameters and changed the datatype for 
															@PriceBatchHeaderIds to BatchIdsType data type
Jamali			11/15/2016				PBI 19144			Added the code to check the nutrition data in all the tables before deleting it
BLJ				03/02/2018				PBI 21600			Updated the procedure to only null out Brand_ID and Category_ID when Icon integration
															enabled so that the non integrated UK region won't have their reports broken
***********************************************************************************************************************************************/
SET NOCOUNT ON

IF OBJECT_ID('tempdb..#PriceBatchHeaders') IS NOT NULL
BEGIN
	DROP TABLE #PriceBatchHeaders
END 

IF OBJECT_ID('tempdb..#Item_Keys') IS NOT NULL
BEGIN
	DROP TABLE #Item_Keys
END 

IF OBJECT_ID('tempdb..#Nutrifacts') IS NOT NULL
BEGIN
	DROP TABLE #Nutrifacts
END 
	
CREATE TABLE #PriceBatchHeaders
(
	PriceBatchHeaderID INT
	, BatchId INT
)

CREATE TABLE #Item_Keys
(
	Item_Key INT PRIMARY KEY
	, NutriFact_ID INT
)

CREATE TABLE #Nutrifacts
(
	NutrifactId INT
)

DECLARE @EnablePLUIRMAIConFlow BIT = (SELECT CAST(dbo.fn_GetAppConfigValue('EnablePLUIRMAIConFlow', 'IRMA Client') AS BIT))
	,@EnableUPCIRMAToIConFlow BIT = (SELECT CAST(dbo.fn_GetAppConfigValue('EnableUPCIRMAToIConFlow', 'IRMA Client') AS BIT))
	,@EnableUPCIConToIRMAFlow BIT = (SELECT CAST(dbo.fn_GetAppConfigValue('EnableUPCIConToIRMAFlow', 'IRMA Client') AS BIT))
DECLARE @DeleteBrandIDAndCategoryID BIT = CASE 
		WHEN @EnablePLUIRMAIConFlow = 1
			AND @EnableUPCIRMAToIConFlow = 1
			AND @EnableUPCIConToIRMAFlow = 1
			THEN 1
		ELSE 0
		END

--Put the incoming data into the temp table for better performance
INSERT INTO #PriceBatchHeaders
SELECT PriceBatchHeaderId, BatchId FROM @PriceBatchHeaderIds 

BEGIN TRANSACTION

BEGIN TRY
	--UPDATE THE PRICE BATCH HEADER   
   UPDATE PriceBatchHeader 
	SET PriceBatchStatusID = 6,
		ProcessedDate = GETDATE(),
		POSBatchID = pbhtemp.BatchId
	FROM PriceBatchHeader pbh
	INNER JOIN #PriceBatchHeaders pbhtemp ON pbh.PriceBatchHeaderID = pbhtemp.PriceBatchHeaderID
		    
	INSERT #Item_Keys (Item_Key, NutriFact_ID)
    SELECT Item.Item_Key, iscale.NutriFact_ID
    FROM dbo.Item 
    INNER JOIN dbo.PriceBatchDetail PBD  ON PBD.Item_Key = Item.Item_Key
    INNER JOIN #PriceBatchHeaders pbhtemp on pbd.PriceBatchHeaderID = pbhtemp.PriceBatchHeaderId
	LEFT JOIN dbo.ItemScale iscale  ON iscale.Item_Key = PBD.Item_key
    WHERE ItemChgTypeID = 3
        AND NOT EXISTS (SELECT d.PriceBatchDetailID
                        FROM dbo.PriceBatchDetail D 
                        INNER JOIN dbo.StoreItem SI ON D.Store_No = SI.Store_No AND D.Item_Key = SI.Item_Key
                        LEFT JOIN dbo.PriceBatchHeader H ON H.PriceBatchHeaderID = D.PriceBatchHeaderID
                        WHERE D.Item_Key = Item.Item_Key
                            AND D.ItemChgTypeID = 3
                            AND ISNULL(H.PriceBatchStatusID, 0) < 6
                            AND SI.Authorized = 1
						)
	GROUP BY  Item.Item_Key, iscale.NutriFact_ID

	DELETE VendorDealHistory
	FROM dbo.VendorDealHistory VDH
	INNER JOIN dbo.StoreItemVendor SIV  ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
    INNER JOIN #Item_Keys ik ON SIV.Item_Key = ik.Item_Key
	
	DELETE VendorCostHistory
	FROM dbo.VendorCostHistory VCH
	INNER JOIN dbo.StoreItemVendor SIV  ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
    INNER JOIN #Item_Keys ik ON SIV.Item_Key = ik.Item_Key
    
	DELETE dbo.StoreItemVendor 
	FROM dbo.StoreItemVendor SIV
    INNER JOIN #Item_Keys ik ON SIV.Item_Key = ik.Item_Key
	
	DELETE dbo.ItemVendor 
	FROM dbo.ItemVendor IV 
    INNER JOIN #Item_Keys ik ON IV.Item_Key = ik.Item_Key
    
	DELETE dbo.Price 
	FROM dbo.Price p
    INNER JOIN #Item_Keys ik ON p.Item_Key = ik.Item_Key
	
	DELETE dbo.ValidatedScanCode
	FROM dbo.ValidatedScanCode vsc
	INNER JOIN dbo.ItemIdentifier ii ON vsc.ScanCode = ii.Identifier 
	INNER JOIN #Item_Keys ik ON ii.Item_Key = ik.Item_Key		
	
	DELETE dbo.ItemIdentifier 
	FROM dbo.ItemIdentifier II
	INNER JOIN #Item_Keys ik ON ii.Item_Key = ik.Item_Key
	WHERE Default_Identifier = 0
	
	UPDATE dbo.ItemIdentifier 
	SET Deleted_Identifier = 1, Add_Identifier = 0, Remove_Identifier = 0
	FROM dbo.ItemIdentifier ii
	INNER JOIN #Item_Keys ik ON ii.Item_Key = ik.Item_Key
    
	UPDATE dbo.Item 
	SET Deleted_Item = 1, 
		Remove_Item = 0, 
		Not_Available = 0,
		Brand_ID = CASE WHEN @DeleteBrandIDAndCategoryID = 1 THEN NULL ELSE Brand_ID END,   -- Wipe out Brand form the deleted item
		Category_ID = CASE WHEN @DeleteBrandIDAndCategoryID = 1 THEN NULL ELSE Category_ID END -- Allows categories to be deleted
	FROM dbo.Item i
	INNER JOIN #Item_Keys ik ON i.Item_Key = ik.Item_Key

	UPDATE dbo.ItemOverride 
	SET Brand_ID = CASE WHEN @DeleteBrandIDAndCategoryID = 1 THEN NULL ELSE Brand_ID END   -- Wipe out Brand form the deleted item
	FROM dbo.ItemOverride ior
	INNER JOIN #Item_Keys ik ON ior.Item_Key = ik.Item_Key

	--Look in the other tables to see if the Nutrifacts data is present
	INSERT INTO #Nutrifacts
	SELECT inn.NutrifactsId
	FROM ItemNutrition inn
	INNER JOIN #Item_Keys ik ON inn.NutrifactsId = ik.Nutrifact_Id
	UNION
	SELECT iss.Nutrifact_Id
	FROM ItemScale iss
	INNER JOIN #Item_Keys ik ON iss.Nutrifact_Id = ik.Nutrifact_Id
	UNION
	SELECT nfq.NutrifactsId
	FROM NutriFactsChgQueue nfq
	INNER JOIN #Item_Keys ik ON nfq.NutrifactsId = ik.Nutrifact_Id
	UNION 
	SELECT iso.Nutrifact_ID
	FROM ItemScaleOverride iso
	INNER JOIN #Item_Keys ik ON iso.Nutrifact_ID = ik.Nutrifact_Id

	--delete the nutrifact record from the nutrifact table only 
	-- also checking to see that the Nutrifact data is not present in any other table
	DELETE dbo.NutriFacts
	FROM dbo.NutriFacts NF
	INNER JOIN #Item_Keys IK ON IK.NutriFact_ID = NF.NutriFactsID
	WHERE NOT EXISTS 
	(SELECT 1 FROM #Nutrifacts n where n.NutrifactId = IK.NutriFact_ID)
	
    DELETE PriceBatchDetail
    FROM dbo.PriceBatchDetail PBD
    LEFT JOIN dbo.PriceBatchHeader PBH ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN #Item_Keys ik ON PBD.Item_Key = ik.Item_Key
    WHERE ISNULL(PriceBatchStatusID, 0) < 6
    
    COMMIT TRANSACTION	
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION

	DECLARE @ErrorMessage NVARCHAR(MAX);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	
	SELECT 
		@ErrorMessage = '[Replenishment_POSPush_UpdatePriceBatchProcessedDel] failed with error: ' + ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE()

	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)	
END CATCH

SET NOCOUNT OFF

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedDel] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedDel] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedDel] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedDel] TO [IRMAReportsRole]
    AS [dbo];

