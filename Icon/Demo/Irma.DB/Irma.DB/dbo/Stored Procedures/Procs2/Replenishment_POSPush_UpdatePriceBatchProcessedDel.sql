
CREATE PROCEDURE [dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedDel]
    @PriceBatchHeaderID int,
    @POSBatchID int
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
***********************************************************************************************************************************************/

    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN

    UPDATE PriceBatchHeader 
    SET PriceBatchStatusID = 6,
        ProcessedDate = GETDATE(),
        POSBatchID = @POSBatchID
    WHERE PriceBatchHeaderID = @PriceBatchHeaderID
    
    SELECT @error_no = @@ERROR

	DECLARE @Item_Key table(Item_Key INT PRIMARY KEY, NutriFact_ID INT);

	INSERT @Item_Key (Item_Key, NutriFact_ID)
            SELECT Item.Item_Key, SI.NutriFact_ID
            FROM dbo.Item (NOLOCK)
            INNER JOIN dbo.PriceBatchDetail PBD (NOLOCK) ON PBD.Item_Key = Item.Item_Key
			LEFT JOIN dbo.ItemScale SI (NOLOCK) ON SI.Item_Key = PBD.Item_key
            WHERE PriceBatchHeaderID = @PriceBatchHeaderID
                AND ItemChgTypeID = 3
                AND NOT EXISTS (SELECT *
                                FROM dbo.PriceBatchDetail D (NOLOCK)
                                INNER JOIN 
                                    dbo.StoreItem SI (NOLOCK)
                                    ON  D.Item_Key   = SI.Item_Key AND
                                        D.Store_No   = SI.Store_No AND
                                       SI.Authorized = 1
                                LEFT JOIN
                                    dbo.PriceBatchHeader H (NOLOCK)
                                    ON H.PriceBatchHeaderID = D.PriceBatchHeaderID
                                WHERE D.Item_Key = Item.Item_Key
                                    AND D.ItemChgTypeID = 3
                                    AND ISNULL(H.PriceBatchStatusID, 0) < 6)
                GROUP BY  Item.Item_Key, SI.NutriFact_ID

        SELECT @error_no = @@ERROR

				IF @Error_No = 0

					DELETE VendorDealHistory
					FROM dbo.VendorDealHistory VDH
					INNER JOIN dbo.StoreItemVendor SIV (NOLOCK) ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
                    JOIN @Item_Key ik ON SIV.Item_Key = ik.Item_Key
			    
				SELECT @Error_No = @@ERROR

				IF @Error_No = 0
				BEGIN
					DELETE VendorCostHistory
					FROM dbo.VendorCostHistory VCH
					INNER JOIN dbo.StoreItemVendor SIV (NOLOCK) ON SIV.StoreItemVendorID = VCH.StoreItemVendorID
                    JOIN @Item_Key ik ON SIV.Item_Key = ik.Item_Key
                    
					SELECT @Error_No = @@ERROR
				END

				IF @Error_No = 0
				BEGIN
					DELETE dbo.StoreItemVendor 
					FROM dbo.StoreItemVendor SIV
                    JOIN @Item_Key ik ON SIV.Item_Key = ik.Item_Key
                    
					SELECT @Error_No = @@ERROR
				END

				IF @Error_No = 0
				BEGIN
					DELETE dbo.ItemVendor 
					FROM dbo.ItemVendor IV 
                    JOIN @Item_Key ik ON IV.Item_Key = ik.Item_Key
                    
					SELECT @Error_No = @@ERROR
				END

				IF @Error_No = 0
				BEGIN
					DELETE dbo.Price 
					FROM dbo.Price p
                    JOIN @Item_Key ik ON p.Item_Key = ik.Item_Key
					SELECT @Error_No = @@ERROR
				END

				IF @Error_No = 0
				BEGIN
					DELETE dbo.ItemIdentifier 
					FROM dbo.ItemIdentifier II
						JOIN @Item_Key ik ON ii.Item_Key = ik.Item_Key
					 WHERE Default_Identifier = 0
					SELECT @Error_No = @@ERROR
				END

				IF @Error_No = 0
				BEGIN
					UPDATE dbo.ItemIdentifier 
					SET Deleted_Identifier = 1, Add_Identifier = 0, Remove_Identifier = 0
					FROM dbo.ItemIdentifier ii
						JOIN @Item_Key ik ON ii.Item_Key = ik.Item_Key
			        
					SELECT @Error_No = @@ERROR
				END
			        
				IF @Error_No = 0
				BEGIN
					UPDATE dbo.Item 
					SET Deleted_Item = 1, 
						Remove_Item = 0, 
						Not_Available = 0,
						Brand_ID = NULL,   -- Wipe out Brand form the deleted item
						Category_ID = NULL -- Allows categories to be deleted
					FROM dbo.Item i
						JOIN @Item_Key ik ON i.Item_Key = ik.Item_Key

					SELECT @Error_No = @@ERROR
				END

				IF @Error_No = 0
				BEGIN
					UPDATE dbo.ItemOverride 
					SET Brand_ID = NULL   -- Wipe out Brand form the deleted item
					FROM dbo.ItemOverride ior
						JOIN @Item_Key ik ON ior.Item_Key = ik.Item_Key

					SELECT @Error_No = @@ERROR
				END

			IF @Error_No = 0
			BEGIN
				--delete the nutrifact record from the nutrifact table only 
				-- if the nutrifact is not associated with any non-deleted items
				DELETE dbo.NutriFacts
				FROM dbo.NutriFacts NF
					INNER JOIN @Item_Key IK ON IK.NutriFact_ID = NF.NutriFactsID
				WHERE NOT EXISTS (SELECT 1 
									FROM dbo.ItemScale SIC (NOLOCK)
									 INNER JOIN dbo.Item I (NOLOCK) ON I.Item_Key = SIC.Item_key
									WHERE SIC.NutriFact_ID = IK.NutriFact_ID
										AND I.Deleted_Item = 0)
				SELECT @Error_No = @@ERROR
			END
        
        SELECT @error_no = @@ERROR

                IF @error_no = 0
                BEGIN
                    DELETE PriceBatchDetail
                    FROM dbo.PriceBatchDetail PBD
                    LEFT JOIN
                        dbo.PriceBatchHeader PBH (NOLOCK) 
                        ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
                    JOIN @Item_Key ik ON PBD.Item_Key = ik.Item_Key
                    WHERE ISNULL(PriceBatchStatusID, 0) < 6
                
                    SELECT @error_no = @@ERROR
                END


    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('UpdatePriceBatchProcessedDel failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
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

