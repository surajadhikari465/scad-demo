CREATE PROCEDURE [dbo].[Replenishment_POSPush_PopulateIconPOSPushPublish]
	@ApplyChanges INT = 0
AS
BEGIN
-- ******************************************************************************************************************************************************************************************************************************
-- Procedure: Replenishment_POSPush_PopulateIconPOSPushPublish
--    Author: Denis Ng
--      Date: 06/12/2014
--
-- Description:
-- This procedure will populate IconPOSPushPublish table with Push data from 
-- IconPOSPushStaging table
--
-- Modification History:
-- Date       	Init  			TFS/PBI   			Comment
-- 05/19/2014	DN   			15056			Created
-- 07/15/2014	DN				15287			Added new column LinkCode_ItemIdentifier
-- 07/15/2014	DN				15314			Added new column POSTare
-- 08/15/2014	DN				15371			Updated to only allow validated scan codes to be published--
-- 08/24/2014	DN				15406			Replacing @Staging with #Staging
-- 09/11/2014	DN				15406			Added TRY.. CATCH
-- 02/06/2015	KM				15794	(7313)	Emergency fix for SP push - break out the calls to Replenishment_POSPush_UpdatePriceBatchProcessedDel and
--												Replenishment_POSPush_UpdatePriceBatchProcessedChg into their own loops;	
-- 04/22/2015	KM				16084	(8650)	Hotfix for SP to allow case discount to be driven by the price table.	
-- 05/14/2015	KM				16158	(9319)	Remove the SP hotfix.	
-- 05/21/2015	DN				16167	(8740)	Add a new event "CancelAllSales"
-- 08/14/2015	BJ				16167	(8740)	Update StoreItem.Refresh to 0 for ScanCodeAdd
-- 08/27/2015	DN				16509	(11087)	Update the WHERE clause in the INSERT.. SELECT statement to allow Sale Off records 
--												with non-null Sale End Date to be inserted into IconPOSPushPublish table  
-- 09/04/2015	KM				10720			Filter non-retail items when inserting into the publish table.
-- 09/18/2015	KM				11637			Raise an error on failure so that this procedure can't fail silently;
-- 10/21/2015	DN				17446			Allowing non-retail items to be batched and processed but will not allow them to send to R10. 	
-- 12/08/2015	KM				13092			Change the join to PBD in the publish table insert in order to avoid publishing alternate IDs when we shouldn't.
-- 12/22/2015   MZ              17892   (13365) Set the status of new item batches to R10 stores to processed after the 
--                                              corresponding IConPOSPushPublish table record is poputed.
-- 01/20/2016	DN				18172	(13886)	Added keyword DISTINCT to the SELECT.. FROM IConPOSPUshStaging statement. 
-- 04/21/2016	DN				19165	(15108)	1. Added logic to use only the most current row (latest PriceBatchDetailID) for each store_no/item_key/identifier/changetype combination
--												2. Added @ObsoletePBD temp table to help process duplicated batches.
-- 07/18/2016  Jamali			PBI16851/16860	1. Replace the table variable with the temp table in the procedure Replenishment_POSPush_UpdatePriceBatchProcessedChg for better statistics generation
--												2. move away from doing data modification statements like insert and update one row at a time
-- 08/10/2016	Jamali			PBI17634		Added the update statement to the staging table
-- 08/29/2016	Jamali			PBI17924		Separated out the writes to the publish table based on batch versus non-batch data
-- 10/31/2016	Jamali			PBI18900		Using the data type BatchIdsType instead of INTTYPE to pass to  procedure Replenishment_POSPush_UpdatePriceBatchProcessedChg and Replenishment_POSPush_UpdatePriceBatchProcessedDel
-- 02/03/2017   MZ          22797::20283        Corrected a wrong assumption in the stored proc when joining the IconPOSPushStaging table and #BatchDataStaging table to insert records into the IConPOSPushPublish table.
-- ******************************************************************************************************************************************************************************************************************************
SET NOCOUNT ON

DECLARE @RegionCode			VARCHAR(2)
DECLARE @RowsToProcess		INT = 0
DECLARE @CurrentRow			INT = 0
DECLARE	@PBH_ID_Parm		INT
DECLARE @PBD_ID_Parm		INT
DECLARE @Store_No_Parm		INT
DECLARE @Item_Key_Parm		INT
DECLARE @Identifier_Parm	VARCHAR(13)
DECLARE @ChangeType			VARCHAR(30)
DECLARE @ObsoletePBD		TABLE (PriceBatchHeaderID INT, PriceBatchDetailID INT)

IF OBJECT_ID('tempdb..#BatchDataStaging') IS NOT NULL
BEGIN
	DROP TABLE #BatchDataStaging
END 
IF OBJECT_ID('tempdb..#ObsoletePBD') IS NOT NULL
BEGIN
	DROP TABLE #ObsoletePBD
END 				

IF OBJECT_ID('tempdb..#ScanCodeDelete') IS NOT NULL
BEGIN
	DROP TABLE #ScanCodeDelete
END 

IF OBJECT_ID('tempdb..#PriceChange') IS NOT NULL
BEGIN
	DROP TABLE #PriceChange
END
IF OBJECT_ID('tempdb..#StagingChangeTypes') IS NOT NULL
BEGIN
	DROP TABLE #StagingChangeTypes
END
			
CREATE TABLE #BatchDataStaging
(
	PriceBatchHeaderID		INT, 
	PriceBatchDetailID		INT,
	Store_No				INT,
	Item_Key				INT,
	Identifier				VARCHAR(13),
	ChangeType				VARCHAR(30)
)
	
create nonclustered  index idx_staging on #BatchDataStaging (PriceBatchDetailID, ChangeType)

CREATE TABLE #StagingChangeTypes
(
	ChangeType				VARCHAR(30)
)
		
CREATE TABLE #ObsoletePBD
(
	PriceBatchHeaderID INT
	, PriceBatchDetailID INT
)

declare @ScanCodeAdd VARCHAR(32) = 'ScanCodeAdd'
declare @ScanCodeDelete VARCHAR(32) = 'ScanCodeDelete'
DECLARE @ScanCodeDeauthorization VARCHAR(32) = 'ScanCodeDeauthorization'
declare @ItemLocaleAttributeChange VARCHAR(32) = 'ItemLocaleAttributeChange'

SELECT @RegionCode = RegionCode FROM Region 
			
IF @ApplyChanges = 1
BEGIN
	INSERT INTO #BatchDataStaging
	SELECT
		IPS.PriceBatchHeaderID,
		IPS.PriceBatchDetailID,
		IPS.Store_No,
		IPS.Item_Key,
		IPS.Identifier,
		IPS.ChangeType
	FROM (SELECT PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType
			FROM (
				SELECT	PriceBatchHeaderID, PriceBatchDetailID, Store_No, Item_Key, Identifier, ChangeType, 
					ROW_NUMBER () OVER (PARTITION BY Store_No, Item_Key, Identifier, ChangeType ORDER BY IPP.PriceBatchDetailID DESC
				) AS RecentPBDID
			FROM IConPOSPUshStaging IPP 
			--we are getting only the batchable changes. the PriceBatchHeaderID is 0 for non batchable changes
			where IPP.PriceBatchHeaderID > 0) AS RecentPBD
			WHERE RecentPBD.RecentPBDID <= 1) IPS 
			
	INSERT INTO #ObsoletePBD (PriceBatchHeaderID, PriceBatchDetailID)
	SELECT 
		IPS.PriceBatchHeaderID, IPS.PriceBatchDetailID
	FROM IconPOSPushStaging IPS 
	LEFT JOIN #BatchDataStaging STA ON IPS.PriceBatchDetailID = STA.PriceBatchDetailID
	WHERE STA.PriceBatchDetailID IS NULL AND
	IPS.PriceBatchDetailID > 0
	
	--store just the different change types in this temp table, so that we do not have to 
	--filter on the IConPOSPUshStaging table to check if a change type is present
	INSERT INTO #StagingChangeTypes			
	SELECT DISTINCT ChangeType
		FROM IConPOSPUshStaging
END

--Do a mass update to all the records in the IConPOSPushStaging table to make sure ScaleForcedTare value is the latest value.
Update ips
SET ScaleForcedTare = isc.ForceTare
FROM IConPOSPushStaging ips
INNER JOIN (select max(its.ItemScale_ID) as ItemScale_ID, item_key from ItemScale its group by Item_Key ) MaxItemScale ON MaxItemScale.Item_Key = ips.Item_Key
INNER JOIN ItemScale isc on isc.ItemScale_ID = MaxItemScale.ItemScale_ID

-- Make sure Sale_Price data includes Sale_Start_Date and Sale_End_End.
-- If not, all there data fields should be NULL.
UPDATE IconPOSPushStaging
	SET	Sale_Price = NULL,
		Sale_Start_Date = NULL,
		Sale_End_Date = NULL
WHERE	Sale_Start_Date IS NULL OR
		Sale_End_Date IS NULL OR
		Sale_Price IS NULL
		
BEGIN TRANSACTION PopulateStaging
BEGIN TRY
	INSERT INTO IConPOSPushPublish 
	(
		PriceBatchHeaderID,
		RegionCode,
		Store_No,
		Item_Key,
		Identifier,
		ChangeType,
		InsertDate,
		BusinessUnit_ID,
		RetailSize,
		RetailPackageUOM,
		TMDiscountEligible,
		Case_Discount,
		AgeCode,
		Recall_Flag,
		Restricted_Hours,
		Sold_By_Weight,
		ScaleForcedTare,
		Quantity_Required,
		Price_Required,
		QtyProhibit,
		VisualVerify,
		RestrictSale,
		Price,
		RetailUom,
		Multiple,
		SaleMultiple,
		Sale_Price,
		Sale_Start_Date,
		Sale_End_Date,
		LinkCode_ItemIdentifier,
		POSTare
	)
	--this select statement gets the data for the batchable data		
	SELECT 
		IPS.PriceBatchHeaderID,
		@RegionCode,
		IPS.Store_No,
		IPS.Item_Key,
		IPS.Identifier,
		IPS.ChangeType,
		IPS.InsertDate,
		S.BusinessUnit_ID,
		IPS.RetailSize,
		CASE WHEN IPS.Sold_By_Weight = 1 THEN 'LB' ELSE 'EA' END AS RetailPackageUOM,
		IPS.TMDiscountEligible,
		IPS.Case_Discount,
		IPS.AgeCode,
		IPS.Recall_Flag,
		IPS.Restricted_Hours,
		IPS.Sold_By_Weight,
		IPS.ScaleForcedTare,
		IPS.Quantity_Required,
		IPS.Price_Required,
		IPS.QtyProhibit,
		IPS.VisualVerify,
		IPS.RestrictSale,
		IPS.Price,
		IPS.RetailUom,
		IPS.Multiple,
		IPS.SaleMultiple,
		IPS.Sale_Price,
		IPS.Sale_Start_Date,
		IPS.Sale_End_Date,
		IPS.LinkCode_ItemIdentifier,
		IPS.POSTare
	FROM 
		IconPOSPushStaging  IPS
		--The staging table contains only the batchable changes
		INNER JOIN #BatchDataStaging STA ON IPS.PriceBatchDetailID = STA.PriceBatchDetailID 
		       AND IPS.Store_No = STA.Store_No 
			   AND IPS.Item_Key = STA.Item_Key
			   AND IPS.Identifier = STA.Identifier
			   AND IPS.ChangeType = STA.ChangeType
		INNER JOIN Item  i  ON ips.Item_Key = i.Item_Key
		INNER JOIN ValidatedScanCode VSC   ON IPS.Identifier = VSC.ScanCode
		INNER JOIN Store S   ON IPS.Store_No = S.Store_No
		INNER JOIN PriceBatchDetail PBD  ON IPS.PriceBatchHeaderID = PBD.PriceBatchHeaderID AND IPS.Item_Key = PBD.Item_Key
	WHERE 
		i.Retail_Sale = 1 
		AND ISNULL(PBD.InsertApplication, '') != 'Sale Off' OR (ISNULL(PBD.InsertApplication, '') = 'Sale Off' AND PBD.Sale_End_Date IS NOT NULL) -- Filter out the "Sale Off" records with NULL Sale_End_Date before populating the publishing table
				
	UNION
	--this select statement will only return data for non batchable data
	SELECT 
		IPS.PriceBatchHeaderID,
		@RegionCode,
		IPS.Store_No,
		IPS.Item_Key,
		IPS.Identifier,
		IPS.ChangeType,
		IPS.InsertDate,
		S.BusinessUnit_ID,
		IPS.RetailSize,
		CASE WHEN IPS.Sold_By_Weight = 1 THEN 'LB' ELSE 'EA' END AS RetailPackageUOM,
		IPS.TMDiscountEligible,
		IPS.Case_Discount,
		IPS.AgeCode,
		IPS.Recall_Flag,
		IPS.Restricted_Hours,
		IPS.Sold_By_Weight,
		IPS.ScaleForcedTare,
		IPS.Quantity_Required,
		IPS.Price_Required,
		IPS.QtyProhibit,
		IPS.VisualVerify,
		IPS.RestrictSale,
		IPS.Price,
		IPS.RetailUom,
		IPS.Multiple,
		IPS.SaleMultiple,
		IPS.Sale_Price,
		IPS.Sale_Start_Date,
		IPS.Sale_End_Date,
		IPS.LinkCode_ItemIdentifier,
		IPS.POSTare
	FROM 
		IconPOSPushStaging  IPS
		INNER JOIN Item  i  ON ips.Item_Key = i.Item_Key
		INNER JOIN ValidatedScanCode VSC   ON IPS.Identifier = VSC.ScanCode
		INNER JOIN Store S   ON IPS.Store_No = S.Store_No	
	WHERE 
		i.Retail_Sale = 1 
		AND IPS.PriceBatchHeaderId = 0 --get only the data for the non-batchable changes
	
	--begin code for ScanCodeAdd change type
	IF EXISTS (SELECT 1 FROM #StagingChangeTypes WHERE ChangeType = @ScanCodeAdd)
	BEGIN			
		UPDATE ItemIdentifier 
			SET Add_Identifier = 0 
		FROM ItemIdentifier  ii
		INNER JOIN IconPOSPushStaging stg on ii.Identifier = stg.Identifier
		WHERE stg.ChangeType = @ScanCodeAdd
			and stg.PriceBatchHeaderID = 0 --only non batchable data will have the ScanCodeAdd
		
		UPDATE StoreItem 
			SET Refresh = 0 
		FROM StoreItem si  
		INNER JOIN IconPOSPushStaging stg on si.Item_Key = stg.Item_Key AND si.Store_No = stg.Store_No
		WHERE si.Refresh = 1
			and stg.ChangeType = @ScanCodeAdd
			and stg.PriceBatchHeaderID = 0 --only non batchable data will have the ScanCodeAdd
	
		UPDATE PriceBatchHeader 
		SET 
			PriceBatchStatusID = 6 
		FROM PriceBatchHeader pbh  
		INNER JOIN IconPOSPushStaging stg on  pbh.PriceBatchHeaderID = stg.PriceBatchHeaderId
		WHERE 
			stg.ChangeType = @ScanCodeAdd
			and stg.PriceBatchHeaderID = 0 --only non batchable data will have the ScanCodeAdd
	END
	--END OF CODE FOR SCAN CODE ADD
	
	--BEGIN CODE FOR ScanCodeDeauthorization
	--the ScanCodeDeauthorization only exists for the non batch staging
	IF EXISTS (SELECT 1 FROM #StagingChangeTypes where ChangeType = @ScanCodeDeauthorization)
	BEGIN
		UPDATE StoreItem 
			SET POSDeAuth = 0 
		FROM StoreItem si  
		INNER JOIN IconPOSPushStaging stg on si.Store_No = stg.Store_No AND si.Item_Key = stg.Item_Key
		where stg.ChangeType = @ScanCodeDeauthorization
		AND stg.PriceBatchHeaderId = 0				
	END
	--END CODE FOR ScanCodeDeauthorization
	
	--BEIGN CODE FOR ItemLocaleAttributeChange
	IF EXISTS (SELECT 1 FROM #StagingChangeTypes where ChangeType = @ItemLocaleAttributeChange)
	BEGIN
		UPDATE StoreItem 
		SET Refresh = 0 
		FROM StoreItem SI  
		INNER JOIN #BatchDataStaging s on si.Store_No = s.Store_No AND si.Item_Key = s.Item_Key	
		WHERE si.Refresh = 1
			AND s.ChangeType = @ItemLocaleAttributeChange	
			
		UPDATE PriceBatchHeader 
		SET  PriceBatchStatusID = 6
		FROM PriceBatchHeader pbh  
		INNER JOIN #BatchDataStaging s ON pbh.PriceBatchHeaderID = s.PriceBatchHeaderId
		WHERE pbh.PriceBatchHeaderID NOT IN (SELECT PriceBatchHeaderID 
										FROM #BatchDataStaging s 
										WHERE (s.ChangeType in( 'RegularPriceChange', 'NonRegularPriceChange')))
			AND	s.ChangeType = @ItemLocaleAttributeChange								
	END
	--END CODE FOR ItemLocaleAttributeChange
	
	--BEGIN CODE FOR ScanCodeDelete
	IF EXISTS (SELECT 1 FROM #StagingChangeTypes WHERE ChangeType = @ScanCodeDelete)
	BEGIN

		--BEGIN CODE FOR ScanCodeDelete (non-batchable)
		--This section of the code deletes the data from the item identifier table
		--the data that is deleted from the item identifiers table comes from the non-batch data
		--and is for identifiers where the Default_Identifier is false
		SELECT
			ii.Identifier AS Identifier,
			ii.Identifier_ID AS Identifier_ID
		INTO #NonBatchableIdentifierDeletes
		FROM IconPOSPushStaging s
		INNER JOIN ItemIdentifier ii on s.Identifier = ii.Identifier
		WHERE ii.Remove_Identifier = 1
			AND ii.Default_Identifier = 0
			AND s.PriceBatchHeaderId = 0
		
		DELETE vsc
		FROM ValidatedScanCode vsc
		INNER JOIN #NonBatchableIdentifierDeletes d on vsc.ScanCode = d.Identifier

		DELETE ii 
		FROM ItemIdentifier  ii  
		INNER JOIN #NonBatchableIdentifierDeletes d on ii.Identifier_ID = d.Identifier_ID
	
		--END CODE FOR SCAN CODE DELETE (non-batchable)

		--BEGIN CODE FOR ScanCodeDelete (batchable)
		--this section of the code deals with the items that have been deleted
		--The item delete can happen only for the batchable data	
		DECLARE @ScanCodeDeleteTable BatchIdsType 
	
		INSERT INTO @ScanCodeDeleteTable
		SELECT DISTINCT PriceBatchHeaderID, 0 AS BatchId
		FROM IconPOSPushStaging s
		INNER JOIN Item  i  ON s.Item_Key = i.Item_Key
		INNER JOIN ItemIdentifier  ii  ON ii.Item_Key = s.Item_Key
		WHERE  s.ChangeType = @ScanCodeDelete
			AND  i.Remove_Item = 1
			AND s.PriceBatchHeaderId > 0

		IF EXISTS (SELECT 1 FROM @ScanCodeDeleteTable)
		BEGIN
			EXEC dbo.Replenishment_POSPush_UpdatePriceBatchProcessedDel @PriceBatchHeaderIds = @ScanCodeDeleteTable
		END
		--END CODE FOR ScanCodeDelete (batchable)
	END
	--END CODE FOR ScanCodedelete

	--BEGIN CODE FOR processing RegularPriceChange, NonRegularPriceChange, CancelAllSales change types.					
	DECLARE @PriceChanges BatchIdsType
	
	INSERT INTO @PriceChanges
	SELECT DISTINCT PriceBatchHeaderID, 0 as BatchId
	FROM #BatchDataStaging
	WHERE ChangeType IN ('RegularPriceChange','NonRegularPriceChange','CancelAllSales')
	
	IF EXISTS (SELECT 1 FROM @PriceChanges)
	BEGIN
		EXEC dbo.Replenishment_POSPush_UpdatePriceBatchProcessedChg @PriceBatchHeaderIds = @PriceChanges			
	END
	--END CODE FOR CODE FOR processing RegularPriceChange, NonRegularPriceChange, CancelAllSales change types.					
	
	-- Update PBD and PBD using @ObsoletePBD
	UPDATE PBH SET PBH.PriceBatchStatusID = 6 
	FROM PriceBatchHeader  PBH 
	JOIN #ObsoletePBD OBS ON PBH.PriceBatchHeaderID = OBS.PriceBatchHeaderID
	
	--the data in the IConPOSPushStaging table is not needed anymore after the processing has been done
	DELETE FROM IConPOSPushStaging
	
	COMMIT TRANSACTION PopulateStaging

END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION PopulateStaging

	DECLARE @ErrorMessage NVARCHAR(MAX);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	
	SELECT 
		@ErrorMessage = '[Replenishment_POSPush_PopulateIconPOSPushPublish] failed with error: ' + ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE()

	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushPublish] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushPublish] TO [IRSUser]
    AS [dbo];

