CREATE PROCEDURE [dbo].[InsertItemHistoryCycleCount] 
    @MasterCountID int
AS 

   -- Modification History:
   -- Date			Init		Comment
   -- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
BEGIN
    IF EXISTS (SELECT * FROM CycleCountMaster WHERE MasterCountID = @MasterCountID AND (ISNULL(IHUpdated, 0) = 1 OR ISNULL(UpdateIH, 0) = 0 OR ClosedDate IS NULL))
        RETURN
        
    BEGIN TRY

        BEGIN TRAN
    
        -- First, do the items with no case information
        INSERT INTO ItemHistory(Store_No, SubTeam_No, Item_Key, DateStamp, Quantity, Weight, Adjustment_ID, CreatedBy)
        SELECT 
			Store_No, 
			SubTeam_No, 
			Item_Key, 
			EndScan, 
			--SUM([Count]),
			--SUM(Weight),
			CASE 
				WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item_key) = 1 THEN
					0
				ELSE
					SUM([Count])
				END, 
			CASE 
				WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item_key) = 1 THEN
					SUM([Count] * dbo.fn_GetAverageUnitWeightByItemKey(Item_Key))
				ELSE	
					SUM(Weight)
				END, 
			2, 
			0
        FROM CycleCountMaster M
        INNER JOIN
            CycleCountHeader H
            ON H.MasterCountID = M.MasterCountID
        INNER JOIN
            CycleCountItems I
            ON I.CycleCountID = H.CycleCountID
        INNER JOIN
            CycleCountHistory D
            ON D.CycleCountItemID = I.CycleCountItemID
        WHERE M.MasterCountID = @MasterCountID
            AND IsCaseCnt = 0
        GROUP BY Store_No, SubTeam_No, Item_Key, EndScan

        DECLARE @unit int, @pound int, @case int
        SELECT @unit = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'UN'
        SELECT @pound = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'LB'
        SELECT @case = Unit_ID FROM ItemUnit (nolock) WHERE EDISysCode = 'CA'

        -- Second, do the items, one at a time, that do have case information
        DECLARE case_info CURSOR
        READ_ONLY
        FOR  SELECT Store_No, M.SubTeam_No, I.Item_Key, EndScan, PackSize, SUM(ISNULL([Count],0)), SUM(ISNULL(Weight, 0)),
                    CASE WHEN Item.CostedByWeight = 1 THEN 1 ELSE PackSize END as Package_Desc1,
                    CASE WHEN Item.CostedByWeight = 1 THEN PackSize ELSE Item.Package_Desc2 END as Package_Desc2,
                    Item.Package_Unit_ID, Item.CostedByWeight
                FROM CycleCountMaster M
                INNER JOIN
                    CycleCountHeader H
                    ON H.MasterCountID = M.MasterCountID
                INNER JOIN
                    CycleCountItems I
                    ON I.CycleCountID = H.CycleCountID
                INNER JOIN
                    Item (nolock)
                    ON Item.Item_Key = I.Item_Key
                INNER JOIN
                    CycleCountHistory D
                    ON D.CycleCountItemID = I.CycleCountItemID
                WHERE M.MasterCountID = @MasterCountID
                    AND IsCaseCnt = 1
                GROUP BY Store_No, M.SubTeam_No, I.Item_Key, EndScan, PackSize,  
                         CASE WHEN Item.CostedByWeight = 1 THEN 1 ELSE PackSize END,
                         CASE WHEN Item.CostedByWeight = 1 THEN PackSize ELSE Item.Package_Desc2 END,
                         Item.Package_Unit_ID, Item.CostedByWeight
                ORDER BY Item_Key
            
        DECLARE @Store_No int, @SubTeam_No int, @Item_Key int, @EndScan datetime, @PackSize decimal(9,4), @Quantity decimal(18,4), @Weight decimal(18,4), @Package_Desc1 decimal(9,4), @Package_Desc2 decimal(9,4), @Package_Unit_ID int, @CostedByWeight bit

        DECLARE @CurrItem_Key int, @CurrItemHistoryID int
        SET @CurrItem_Key = -1

        OPEN case_info
        FETCH NEXT FROM case_info INTO @Store_No, @SubTeam_No, @Item_Key, @EndScan, @PackSize, @Quantity, @Weight, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, @CostedByWeight
        
        WHILE @@fetch_status <> -1
        BEGIN
    	    IF @@fetch_status <> -2
    	    BEGIN
                IF @CurrItem_Key <> @Item_Key
                BEGIN
                    SET @CurrItem_Key = @Item_Key

                    INSERT INTO ItemHistory(Store_No, SubTeam_No, Item_Key, DateStamp, Quantity, Weight, Adjustment_ID, CreatedBy)
                    SELECT 
						Store_No, 
						SubTeam_No, 
						Item_Key, 
						EndScan, 
						--SUM(ISNULL([Count], 0)), 
						--SUM(ISNULL(Weight, 0)), 
						CASE 
							WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item_key) = 1 THEN
								0
							ELSE
								SUM(ISNULL([Count], 0))
							END, 
						CASE 
							WHEN dbo.fn_IsRetailUnitNotCostedByWeight(Item_key) = 1 THEN
								SUM([Count] * dbo.fn_GetAverageUnitWeightByItemKey(Item_Key))
							ELSE	
								SUM(ISNULL(Weight, 0))
							END, 
						2, 
						0
                    FROM CycleCountMaster M
                    INNER JOIN
                        CycleCountHeader H
                        ON H.MasterCountID = M.MasterCountID
                    INNER JOIN
                        CycleCountItems I
                        ON I.CycleCountID = H.CycleCountID
                    INNER JOIN
                        CycleCountHistory D
                        ON D.CycleCountItemID = I.CycleCountItemID
                    WHERE M.MasterCountID = @MasterCountID
                        AND IsCaseCnt = 1
                        AND I.Item_Key = @Item_Key
                    GROUP BY Store_No, SubTeam_No, Item_Key, EndScan
        
                    SELECT @CurrItemHistoryID = SCOPE_IDENTITY()
                END

               
    	    END
    	    FETCH NEXT FROM case_info INTO @Store_No, @SubTeam_No, @Item_Key, @EndScan, @PackSize, @Quantity, @Weight, @Package_Desc1, @Package_Desc2, @Package_Unit_ID, @CostedByWeight
        END
        
        CLOSE case_info
        DEALLOCATE case_info

        INSERT INTO ItemHistory(Store_No, SubTeam_No, Item_Key, DateStamp, Quantity, Weight, Adjustment_ID, CreatedBy)
        SELECT M.Store_No, M.SubTeam_No, Item_Key, EndScan, 0, 0, 2, 0
        FROM CycleCountMaster M
        INNER JOIN
            OnHand
            ON OnHand.Store_No = M.Store_No AND OnHand.SubTeam_No = M.SubTeam_No
        WHERE M.MasterCountID = @MasterCountID
            AND SetNonCountedToZero = 1
            AND OnHand.LastReset < M.EndScan AND ((OnHand.Quantity + OnHand.Weight) <> 0)

        UPDATE CycleCountMaster
        SET IHUpdated = 1
        WHERE MasterCountID = @MasterCountID
   
        COMMIT TRAN 

    END TRY
    BEGIN CATCH

        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
       
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN
     
        RAISERROR ('InsertItemHistoryCycleCount failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
    END CATCH
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistoryCycleCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistoryCycleCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertItemHistoryCycleCount] TO [IRMAReportsRole]
    AS [dbo];

