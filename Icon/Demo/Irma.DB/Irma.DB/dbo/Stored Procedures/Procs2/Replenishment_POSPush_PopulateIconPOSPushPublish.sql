
CREATE PROCEDURE [dbo].[Replenishment_POSPush_PopulateIconPOSPushPublish]
	@ApplyChanges INT = 0
AS

-- **************************************************************************
-- Procedure: Replenishment_POSPush_PopulateIconPOSPushPublish
--    Author: Denis Ng
--      Date: 06/12/2014
--
-- Description:
-- This procedure will populate IconPOSPushPublish table with Push data from 
-- IconPOSPushStaging table
--
-- Modification History:
-- Date       	Init  			TFS   			Comment
-- 05/19/2014	DN   			15056			Created
-- 07/15/2014	DN				15287			Added new column LinkCode_ItemIdentifier
-- 07/15/2014	DN				15314			Added new column POSTare
-- 08/15/2014	DN				15371			Updated to only allow validated scan codes to be published
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
-- **************************************************************************

BEGIN
	DECLARE @RegionCode			VARCHAR(2)
	DECLARE @RowsToProcess		INT = 0
	DECLARE @CurrentRow			INT = 0
	DECLARE	@PBH_ID_Parm		INT
	DECLARE @Store_No_Parm		INT
	DECLARE @Item_Key_Parm		INT
	DECLARE @Identifier_Parm	VARCHAR(13)
	DECLARE @ChangeType			VARCHAR(30)

	IF OBJECT_ID('tempdb..#Staging') IS NOT NULL
	BEGIN
		DROP TABLE #Staging
	END 

	CREATE TABLE #Staging
	(
		StagingID				INT IDENTITY(1,1),
		PriceBatchHeaderID		INT, 
		Store_No				INT,
		Item_Key				INT,
		Identifier				VARCHAR(13),
		ChangeType				VARCHAR(30)
	)

	CREATE CLUSTERED INDEX IDX_C_Staging_StagingID ON #Staging(StagingID)
	CREATE NONCLUSTERED INDEX IDX_NC_Staging_PriceBatchHeaderID_ChangeType ON #Staging(PriceBatchHeaderID, ChangeType, Item_Key)

	SELECT @RegionCode = RegionCode FROM Region (NOLOCK)
	
	IF @ApplyChanges = 1
		BEGIN
			INSERT INTO #Staging
			SELECT DISTINCT
				IPS.PriceBatchHeaderID,
				IPS.Store_No,
				IPS.Item_Key,
				IPS.Identifier,
				IPS.ChangeType
			FROM IConPOSPUshStaging IPS (NOLOCK) 

			SET @RowsToProcess = @@ROWCOUNT
		END

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
		SELECT DISTINCT
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
			IConPOSPUshStaging IPS (NOLOCK) 
			INNER JOIN Item (NOLOCK) i ON ips.Item_Key = i.Item_Key
			INNER JOIN ValidatedScanCode VSC (NOLOCK) ON IPS.Identifier = VSC.ScanCode
			INNER JOIN Store S (NOLOCK) ON IPS.Store_No = S.Store_No

			-- The left join is important so that we don't lose ItemRefresh events which have no entry in PBD (non-batchable).
			LEFT JOIN PriceBatchDetail PBD (NOLOCK) ON IPS.PriceBatchHeaderID = PBD.PriceBatchHeaderID AND IPS.Item_Key = PBD.Item_Key
		WHERE 
			i.Retail_Sale = 1 
			AND ISNULL(PBD.InsertApplication, '') != 'Sale Off' OR (ISNULL(PBD.InsertApplication, '') = 'Sale Off' AND PBD.Sale_End_Date IS NOT NULL) -- Filter out the "Sale Off" records with NULL Sale_End_Date before populating the publishing table

		WHILE @CurrentRow < @RowsToProcess
			BEGIN
				SET @CurrentRow = @CurrentRow + 1
				SELECT 
					@PBH_ID_Parm		= PriceBatchHeaderID,
					@Store_No_Parm		= Store_No,
					@Item_Key_Parm		= Item_Key,
					@Identifier_Parm	= Identifier,
					@ChangeType			= ChangeType
				FROM 
					#Staging 
				WHERE 
					StagingID = @CurrentRow
				
				IF @ChangeType = 'ScanCodeAdd'
					BEGIN
						UPDATE ItemIdentifier SET Add_Identifier = 0 WHERE Identifier = @Identifier_Parm 
						UPDATE StoreItem SET Refresh = 0 WHERE Item_Key = @Item_Key_Parm AND Store_No = @Store_No_Parm AND Refresh = 1
						UPDATE PriceBatchHeader 
						SET 
							PriceBatchStatusID = 6 
						WHERE 
							PriceBatchHeaderID = @PBH_ID_Parm
					END

				IF @ChangeType = 'ScanCodeDelete'
					BEGIN
						-- Remove an item						
						IF NOT EXISTS (SELECT I.Item_Key FROM Item I (NOLOCK) INNER JOIN ItemIdentifier II (NOLOCK) ON I.Item_Key = II.Item_Key WHERE II.Identifier = @Identifier_Parm AND I.Remove_Item = 1)
							BEGIN
								-- Remove only the alternate identifer
								DELETE FROM ItemIdentifier WHERE Identifier = @Identifier_Parm AND Remove_Identifier = 1 AND Default_Identifier = 0
							END
					END

				IF @ChangeType = 'ScanCodeDeauthorization'
					BEGIN
						UPDATE StoreItem SET POSDeAuth = 0 WHERE Store_No = @Store_No_Parm AND Item_Key = @Item_Key_Parm
					END

				IF @ChangeType = 'ItemLocaleAttributeChange'
					BEGIN
						UPDATE StoreItem SET Refresh = 0 WHERE Store_No = @Store_No_Parm AND Item_Key = @Item_Key_Parm AND Refresh = 1
						UPDATE PriceBatchHeader 
						SET 
							PriceBatchStatusID = 6 
						WHERE 
							PriceBatchHeaderID = @PBH_ID_Parm 
							AND NOT EXISTS (SELECT PriceBatchHeaderID FROM #Staging s WHERE s.PriceBatchHeaderID = @PBH_ID_Parm AND (s.ChangeType = 'RegularPriceChange' OR s.ChangeType = 'NonRegularPriceChange'))
					END
			END

		-- Process ScanCodeDelete change types.
		declare @ScanCodeDelete table (PriceBatchHeaderId int)

		insert into
			@ScanCodeDelete
		select distinct
			PriceBatchHeaderID
		from 
			#Staging s
			join Item (nolock) i on s.Item_Key = i.Item_Key
			join ItemIdentifier (nolock) ii on i.Item_Key = s.Item_Key
		where 
			s.ChangeType = 'ScanCodeDelete' and
			i.Remove_Item = 1

		WHILE exists (select 1 from @ScanCodeDelete)
			BEGIN
				SET @PBH_ID_Parm = (select top(1) PriceBatchHeaderId from @ScanCodeDelete)
					
				EXEC dbo.Replenishment_POSPush_UpdatePriceBatchProcessedDel
								@PriceBatchHeaderID = @PBH_ID_Parm,
								@POSBatchID			= 0

				delete from @ScanCodeDelete where PriceBatchHeaderId = @PBH_ID_Parm
			END

		-- Process RegularPriceChange and NonRegularPriceChange change types.
		declare @PriceChange table (PriceBatchHeaderId int)

		insert into
			@PriceChange
		select distinct
			PriceBatchHeaderID
		from
			#Staging
		where
			ChangeType in ('RegularPriceChange','NonRegularPriceChange','CancelAllSales')

		WHILE exists (select 1 from @PriceChange)
			BEGIN
				SET @PBH_ID_Parm = (select top(1) PriceBatchHeaderId from @PriceChange)

				EXEC dbo.Replenishment_POSPush_UpdatePriceBatchProcessedChg
					@PriceBatchHeaderID = @PBH_ID_Parm,
					@POSBatchID			= 0			

				delete from @PriceChange where PriceBatchHeaderId = @PBH_ID_Parm
			END
				
		
		DELETE FROM IConPOSPushStaging

		IF OBJECT_ID('tempdb..#Staging') IS NOT NULL
		BEGIN
			DROP TABLE #Staging
		END 

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

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [ItemCatalog.dbo.Replenishment_POSPush_PopulateIconPOSPushPublish.sql]'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushPublish] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_PopulateIconPOSPushPublish] TO [IRSUser]
    AS [dbo];

