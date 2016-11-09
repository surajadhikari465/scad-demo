CREATE PROCEDURE [dbo].[UpdateAverageCost]
    @InStore_No int,
    @InSubTeam_No int,
    @InStartDate smalldatetime  -- not used
AS

-- **************************************************************************
-- Procedure: [UpdateAverageCost]
--    Author: n/a
--      Date: 06/30/2010
--
-- Description:
-- This procedure is utilized to calculate the average cost of (regular retail) items
-- and update OnHand inventory numbers.
--
-- Modification History:
-- Date			Init		Comment
-- 06/30/2010	Alex Z		Created V4 stored procedure based on existing V1 South version
--							Added Modification history and comments for clarification.
-- 07/10/2010   Alex Z		Added Formatting to SQL Code
-- 12/09/2010   Mugdha D	Bug 13792: Updated the filter for - STORE RECORDS THAT HAVE NO SALES LOADED 
-- 04/18/2011   Mugdha D    Bug 13867: Added performance improvements, index hints
-- 04/27/2011   Mugdha D    Bug 13869/1836:	For new items use VCH PD1 and Cost Unit to get prior Avg Cost
-- 07/02/2012   Min Z       TFS 6650: Remove Average Cost Suspension logic.   
-- 09/07/2012   Mugdha D	Bug 8018: Applied fix for computing the variance to not go over limit for numeric (7,4)
-- 01/24/2013   Tom L		SO OI Index Sync: Removed index hint for "idxOrderItem" -- this index was SO-specific and the PK index replaces it.
-- **************************************************************************

BEGIN 
    SET NOCOUNT ON
  
-- **************************************************************************
-- SET UP ALL TEMP TABLES
-- **************************************************************************

    CREATE TABLE #ItemHistoryInsertedQueue 
        (
	    Store_No int,
	    Item_Key int,
	    DateStamp datetime,
	    SubTeam_No int,
	    ItemHistoryID int,
	    Adjustment_ID int,
	    ID int
	    )
	    
	CREATE INDEX idxIHIQ_StoreSubTeamItem ON #ItemHistoryInsertedQueue (Store_No, SubTeam_No, Item_Key)
	CREATE INDEX idxIHIQ_Adjustment_ID ON #ItemHistoryInsertedQueue (Adjustment_ID, DateStamp, Store_No, SubTeam_No, Item_Key)
	CREATE INDEX idxIHIQ_ItemHistoryID ON #ItemHistoryInsertedQueue (ItemHistoryID)
	CREATE INDEX idxIHIQ_ID ON #ItemHistoryInsertedQueue (ID)
	CREATE NONCLUSTERED INDEX IHIQRecommended ON [dbo].[#ItemHistoryInsertedQueue] 
    (
    [Adjustment_ID]
    )
	INCLUDE ([Store_No],[Item_Key],[DateStamp],[SubTeam_No],[ItemHistoryID])
    
    CREATE TABLE #ItemHistoryDeletedQueue 
        (
	    Store_No int,
	    Item_Key int,
	    DateStamp datetime,
	    SubTeam_No int,
	    ItemHistoryID int,
	    Adjustment_ID int,
	    ID int
	    )
	    
	CREATE INDEX idxIHDQ_StoreSubTeamItem ON #ItemHistoryDeletedQueue (Store_No, SubTeam_No, Item_Key)
	CREATE INDEX idxIHDQ_Adjustment_ID ON #ItemHistoryDeletedQueue (Adjustment_ID, DateStamp, Store_No, SubTeam_No, Item_Key)
	CREATE INDEX idxIHDQ_ItemHistoryID ON #ItemHistoryDeletedQueue (ItemHistoryID)
	CREATE INDEX idxIHDQ_ID ON #ItemHistoryDeletedQueue (ID)

    CREATE TABLE #ItemsOH 
        (
        Store_No int, SubTeam_No int, Item_Key int, MinDateStamp smalldatetime, StartOnHandDate smalldatetime,
        OnHandQty decimal(18,4), OnHandWt decimal(18,4),
        PRIMARY KEY (Store_No, SubTeam_No, Item_Key)
        )
        
    CREATE TABLE #ItemsOH_MDR -- MID-DAY INVENTORY RESET (EXACT COPY OF #ITEMSOH)
        (
        Store_No int, SubTeam_No int, Item_Key int, MinDateStamp smalldatetime, StartOnHandDate smalldatetime,
        OnHandQty decimal(18,4), OnHandWt decimal(18,4),
        PRIMARY KEY (Store_No, SubTeam_No, Item_Key)
        )   
             
    CREATE TABLE #ItemsAC 
        (
        Store_No int, SubTeam_No int, Item_Key int, MinDateStamp smalldatetime, 
        OnHand decimal(18,4),
        AvgCost smallmoney,
        PRIMARY KEY (Store_No, SubTeam_No, Item_Key)
        )
        
    CREATE TABLE #AvgCost
        (
        Store_No int, SubTeam_No int, Item_Key int, Date_Key smalldatetime,
        AvgCost smallmoney,
        PriorAvgCost smallmoney,
        Suspended bit,
        Variance_Pct decimal(7,4),
        PRIMARY KEY (Store_No, SubTeam_No, Item_Key, Date_Key)
        )
        
    CREATE TABLE #OnHand
        (
        Store_No int, SubTeam_No int, Item_Key int, Date_Key smalldatetime, 
        OnHandQty decimal(18,4), OnHandWt decimal(18,4)
        )
        
    CREATE NONCLUSTERED INDEX [idxOHHPKQtyWt] ON [dbo].[#OnHand] 
	(
		[Store_No] ASC,
		[SubTeam_No] ASC,
		[Item_Key] ASC,
		[Date_Key] ASC
	)
	INCLUDE (OnHandQty, OnHandWt)

	CREATE NONCLUSTERED INDEX [OnHandTempRecommended] ON [dbo].[#OnHand] 
	(
	[Date_Key]
	)
	INCLUDE ([Store_No],[SubTeam_No],[Item_Key],[OnHandQty],[OnHandWt])

	CREATE TABLE #Suspend (OrderItem_ID int, Store_No int, SubTeam_No int, Item_Key int, Date_Key smalldatetime, PriorAvgCost smallmoney, NewAvgCost smallmoney, Variance_Pct decimal(7,4))
-- ********************** LOCAL VARIABLES ****************************************************

						DECLARE										
												@Store_No int, 
												@SubTeam_No int, 
												@Item_Key int, 
												@Date_Key smalldatetime,
												@LastStore_No int, 
												@LastSubTeam_No int, 
												@LastItem_Key int, 
												@LastDate_Key smalldatetime,
												@Tomorrow smalldatetime, 
												@Today smalldatetime,
												@PeriodBeginDate smalldatetime, 
												@NextDate smalldatetime
    
  BEGIN TRY
						SET						@Tomorrow = DATEADD(day, 1, CONVERT(varchar(255), GETDATE(), 101))

						SET						@Today = CONVERT(varchar(255), GETDATE(), 101)
        
						SELECT					
												@PeriodBeginDate = dbo.fn_PeriodBeginDate(GETDATE())

						--Get the list of stores that have sales loaded from ItemHistory table along (union) with the facilities from Store table
						DECLARE  @StoresLoaded Table (store_no int)
						
						INSERT INTO @StoresLoaded 
							SELECT DISTINCT Store_No FROM ItemHistory with (index(idxItemHistoryAdjID), NoLock) WHERE DateStamp >= DATEADD(day, -1, @Today) AND Adjustment_ID = 3
							UNION
							SELECT Store_No FROM Store (NoLock) WHERE Manufacturer = 1 or Distribution_Center = 1
						

-- ****** GRAB THE APPLICABLE INSERTED ITEMHISTORY -- FILTER OUT THE STORE RECORDS THAT HAVE NO SALES LOADED FOR THAT DAY IN ITEMHISTORY *****

						INSERT INTO								
												#ItemHistoryInsertedQueue	
												(
												Store_No,
												Item_Key,
												DateStamp,
												SubTeam_No,
												ItemHistoryID,
												Adjustment_ID,
												ID
												)
						SELECT									
												Store_No,
												Item_Key,
												DateStamp,
												SubTeam_No,
												ItemHistoryID,
												Adjustment_ID,
												ID
						FROM									
												ItemHistoryInsertedQueue iq
						WHERE										
												DateStamp < @Tomorrow
												AND iq.Store_No = ISNULL(@InStore_No, Store_No)
												AND iq.SubTeam_No = ISNULL(@InSubTeam_No,SubTeam_No)
												AND iq.Store_No IN (SELECT SL.Store_No FROM @StoresLoaded SL)


-- ******* GRAB THE APPLICABLE DELETED ITEMHISTORY - FILTER OUT RECORDS LIKE ABOVE ************************************************

						INSERT INTO 
												#ItemHistoryDeletedQueue 
												(
												Store_No, 
												Item_Key,
												DateStamp,
												SubTeam_No, 
												ItemHistoryID,
												Adjustment_ID,	
												ID
												)
						SELECT 
												Store_No,
												Item_Key,
												DateStamp,
												SubTeam_No,
												ItemHistoryID,
												Adjustment_ID,
												ID
						FROM									
												ItemHistoryDeletedQueue iq
						WHERE									
												DateStamp < @Tomorrow
												AND Store_No = ISNULL(@InStore_No, Store_No)
												AND SubTeam_No = ISNULL(@InSubTeam_No, SubTeam_No)
												AND iq.Store_No IN (SELECT SL.Store_No FROM @StoresLoaded SL)

-- ******* INSERT ITEMS TO BE PROCESSED INTO THE ONHAND TEMP TABLE **************************************************************

						INSERT INTO 
												#ItemsOH 
												(
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												MinDateStamp
												)
						SELECT						
												Store_No,
												T.SubTeam_No,
												T.Item_Key,
												CONVERT(smalldatetime, CONVERT(varchar(255), MIN(DateStamp), 101))
						FROM 
												(

								SELECT								
												Store_No,
												SubTeam_No,
												Item_Key,
												DateStamp
								FROM 
												#ItemHistoryInsertedQueue IQ
								UNION ALL

								SELECT 
												Store_No,
												SubTeam_No,
												Item_Key,
												DateStamp
								FROM									
												#ItemHistoryDeletedQueue DQ) T
													
						GROUP BY								
												Store_No,
												T.SubTeam_No,
												T.Item_Key

        
-- ********* GET THE STARTING ONHAND FOR EACH ITEM TO BE PROCESSED ************************************************************

						UPDATE 
												#ItemsOH
						SET										
												StartOnHandDate = (SELECT MAX(Date_Key) 

						FROM			
												OnHandHistory H (nolock)
						WHERE 
												H.Store_No = I.Store_No 
												AND H.SubTeam_No = I.SubTeam_No 
												AND H.Item_Key = I.Item_Key
												AND H.Date_Key < I.MinDateStamp)
						FROM 
												#ItemsOH I

-- *****************************************************************************************************************************

						UPDATE 
												#ItemsOH
						SET 
												OnHandQty = OHH.Quantity,
												OnHandWt = OHH.Weight
						FROM 
												#ItemsOH IOH
								INNER JOIN		   
												OnHandHistory OHH (nolock)
								ON				
												OHH.Store_No = IOH.Store_No 
												AND OHH.SubTeam_No = IOH.SubTeam_No 
												AND OHH.Item_Key = IOH.Item_Key 
												AND OHH.Date_Key = IOH.StartOnHandDate

-- ************************************************************************************************************************************	  
 
						UPDATE 
												#ItemsOH
						SET 
												OnHandQty = ISNULL(OnHandQty, 0),
												OnHandWt = ISNULL(OnHandWt, 0)
						WHERE 
												(OnHandQty IS NULL) OR (OnHandWt IS NULL)

-- ********* SPLIT OUT ANY ITEMS THAT HAD A INVENTORY RESET IN THE MIDDLE OF THE DAY. THESE MUST BE PROCESSED ONE RECORD AT A TIME ******* 

						INSERT INTO 
												#ItemsOH_MDR
						SELECT				
												* 
						FROM									
												#ItemsOH IOH
						WHERE						
							EXISTS 
								(SELECT * 
										FROM 
												ItemHistory (nolock) IH WHERE						
												IH.Store_No = IOH.Store_No 
												AND IH.Subteam_NO = IOH.SubTeam_No 
												AND IH.Item_Key = IOH.Item_Key
												AND IH.DateStamp >= IOH.MinDateStamp 
												AND IH.Adjustment_ID = 2 
												AND DATEDIFF(day, IH.DateStamp, DATEADD(minute, 1, IH.DateStamp)) = 0)
        

						DELETE 
												#ItemsOH
						FROM 
												#ItemsOH IL
								INNER JOIN
												#ItemsOH_MDR MDR
								ON 
												MDR.Store_No = IL.Store_No 
												AND MDR.SubTeam_No = IL.SubTeam_No 
												AND MDR.Item_Key = IL.Item_Key
		    
-- ********* ON HAND UPDATE MUST BE DONE BEFORE THE AVERAGE COST UPDATE BECAUSE THE LATTER USES THE DATA FROM THE FORMER *********************
-- ********* PROCESS THE ITEMS THAT DID NOT HAVE A RESET IN THE MIDDLE OF THE DAY ************************************************************
        
-- **************** TO START THE LOOP, GET THE MINIMUM DATE ACROSS ALL ITEMS *****************************************************************

						SELECT 
												@NextDate = MIN(MinDateStamp)
						FROM 
												#ItemsOH
        

-- ************** ADD UP A RUNNING TOTAL A DAY AT A TIME UNTIL WE HIT TOMORROW, SAVING A DAILY TOTAL PER ITEM *********************************
			WHILE									
												@NextDate < @Tomorrow
				BEGIN
-- **************** GET MOVEMENT TOTAL FOR THAT DAY **********************************
						INSERT INTO 
												#OnHand 
												(
												Store_No, 
												SubTeam_No,
												Item_Key, 
												Date_Key, 
												OnHandQty,
												OnHandWt)
						SELECT 
												IH.Store_No, 
												IH.SubTeam_No, 
												IH.Item_Key, 
												@NextDate,
-- **************** ADD IN THE PRIOR ON-HAND VALUES TO MAKE THIS A RUNNING TOTAL ******************************************************
												ISNULL(SUM(IH.Quantity * IA.Adjustment_Type), 0) + ISNULL(MAX(IL.OnHandQty), 0),
												ISNULL(SUM(IH.Weight * IA.Adjustment_Type), 0) + ISNULL(MAX(IL.OnHandWt), 0)
						FROM 
												#ItemsOH IL
								INNER JOIN
												ItemHistory IH with (index(idxItemHistoryAdjID), NoLock) 
								ON 
												IH.Store_No = IL.Store_No 
												AND IH.SubTeam_No = IL.SubTeam_No 
												AND IH.Item_Key = IL.Item_Key 
												AND IH.DateStamp >= @NextDate AND IH.DateStamp < DATEADD(day, 1, @NextDate)
								INNER JOIN
												ItemAdjustment IA (nolock) 
								ON				
												IA.Adjustment_ID = IH.Adjustment_ID
						WHERE

-- ************ GO ONLY AS FAR BACK PER ITEM AS THE MINIMUM MOVEMENT DATE *******************************

												IL.MinDateStamp <= @NextDate

-- ***** EXCLUDE IF THERE IS A RESET AT THE END OF THE DAY **************************
							AND NOT EXISTS 
								(SELECT			* 
									FROM	
												ItemHistory RIH (nolock) 
								WHERE			
												RIH.Store_No = IL.Store_No 
												AND RIH.SubTeam_No = IL.SubTeam_No 
												AND RIH.Item_Key = IL.Item_Key 
												AND RIH.Adjustment_ID = 2 
												AND RIH.DateStamp = DATEADD(minute, -1, DATEADD(day, 1, @NextDate)))
						GROUP BY 
												IH.Store_No,
												IH.SubTeam_No,
												IH.Item_Key
			

-- ******************** INSERT THE INVENTORY RESETS WHEN RESET IS AT THE END OF DAY ****************************************

						INSERT INTO 
												#OnHand 
												(
												Store_No,
												SubTeam_No,
												Item_Key, 
												Date_Key, 
												OnHandQty, 
												OnHandWt)

						SELECT					
												ItemHistory.Store_No, 
												ItemHistory.SubTeam_No, 
												ItemHistory.Item_Key, 
												@NextDate, 
												ISNULL(ItemHistory.Quantity, 0), 
												ISNULL(ItemHistory.Weight, 0)
						FROM 
							(SELECT 
												IH.Store_No, 
												IH.SubTeam_No,
												IH.Item_Key,
												MAX(ItemHistoryID) As MaxItemHistoryID
							FROM 
												#ItemsOH IL
								INNER JOIN
												ItemHistory IH with (index(idxItemHistoryAdjID), NoLock)
								ON				
												IH.Store_No = IL.Store_No 
												AND IH.SubTeam_No = IL.SubTeam_No 
												AND IH.Item_Key = IL.Item_Key
												AND IH.Adjustment_ID = 2 
												AND IH.DateStamp = DATEADD(minute, -1, DATEADD(day, 1, @NextDate))
						WHERE 
												IL.MinDateStamp <= @NextDate
						GROUP BY 
												IH.Store_No, IH.SubTeam_No, IH.Item_Key) IL
								INNER JOIN
												ItemHistory (nolock) 
								ON 
												ItemHistory.ItemHistoryID = IL.MaxItemHistoryID
								
-- **************** UPDATE THE PRIOR ON-HAND VALUES FOR THE NEXT ITERATION OF THIS LOOP *****************************
-- ************************* WHERE WE JUST INSERTED A NEW RECORD INTO #ONHAND ***************************************

							UPDATE 
												#ItemsOH
							SET 
												OnHandQty = OHH.OnHandQty,
												OnHandWt = OHH.OnHandWt
							FROM 
												#ItemsOH IOH
								INNER JOIN
												#OnHand OHH
								ON 
												OHH.Store_No = IOH.Store_No 
												AND OHH.SubTeam_No = IOH.SubTeam_No 
												AND OHH.Item_Key = IOH.Item_Key 
												AND OHH.Date_Key = @NextDate

-- *****************  NEXT DAY   *************************

							SET					@NextDate = DATEADD(day, 1, @NextDate)

-- ***************** END WHILE   *************************
				END
		


-- ******* CALCULATE ONHAND BY DAY FOR ITEMS THAT HAD A RESET IN THE MIDDLE OF THE DAY **************************************************
-- ******** DAILY ONHAND MUST BE CALCULATED IN A CURSOR SO THAT CYCLE COUNT ITEMHISTORY RECORDS CAN RESET THE COUNT AS WE GO ALONG ******

							DECLARE									
												@OnHandQty decimal(38, 28), 
												@OnHandWt decimal(38, 28)
		
							SELECT 
												@LastStore_No = 0, 
												@LastSubTeam_No = 0, 
												@LastItem_Key = 0,
												@LastDate_Key = 0, 
												@OnHandQty = 0, 
												@OnHandWt = 0
        
							DECLARE							
												IHL CURSOR
												FORWARD_ONLY STATIC
		
							FOR 
							SELECT 
												IH.Store_No, 
												IH.SubTeam_No, 
												IH.Item_Key, 
												CONVERT(smalldatetime, CONVERT(varchar(255), IH.DateStamp, 101)), 
												IH.Adjustment_ID, 
												ISNULL(IH.Quantity, 0) * IA.Adjustment_Type, 
												ISNULL(IH.Weight, 0) * IA.Adjustment_Type
							FROM 
												#ItemsOH_MDR IL
								INNER JOIN
												ItemHistory IH (nolock) 
								ON 
												IH.Store_No = IL.Store_No	
												AND IH.SubTeam_No = IL.SubTeam_No 
												AND IH.Item_Key = IL.Item_Key
												AND IH.DateStamp >= IL.MinDateStamp 
												AND IH.DateStamp < @Tomorrow
								INNER JOIN
												ItemAdjustment IA (nolock) 
								ON 
												IA.Adjustment_ID = IH.Adjustment_ID
							ORDER BY 
												IH.Store_No, 
												IH.SubTeam_No, 
												IH.Item_Key,
												IH.DateStamp
        
-- ********************************************************************************************************************************************

							DECLARE 
												@Adjustment_ID int, 
												@Quantity decimal(38, 28), 
												@Weight decimal(38, 28)
							OPEN IHL
        
							FETCH NEXT FROM IHL 
										INTO	@Store_No, 
												@SubTeam_No,
												@Item_Key, 
												@Date_Key,
												@Adjustment_ID,
												@Quantity, 
												@Weight
					WHILE (@@fetch_status <> -1)
						BEGIN
							IF NOT 
												((@LastDate_Key = @Date_Key) 
												AND (@LastItem_Key = @Item_Key) 
												AND (@LastSubTeam_No = @SubTeam_No) 
												AND (@LastStore_No = @Store_No))
						BEGIN
							IF						
												@LastStore_No > 0
						BEGIN
							INSERT INTO				
												#OnHand 
												(
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												Date_Key, 
												OnHandQty, 
												OnHandWt
												)
							VALUES 
												(
												@LastStore_No, 
												@LastSubTeam_No, 
												@LastItem_Key, 
												@LastDate_Key, 
												@OnHandQty, 
												@OnHandWt
												)
					
-- ************ DO THIS UPDATE TO MAKE THE UPDATE TO THE ONHAND TABLE LATER MORE EFFICIENT *********************

							UPDATE 
												#ItemsOH_MDR
							SET 
												OnHandQty = @OnHandQty,
												OnHandWt = @OnHandWt
							FROM
												#ItemsOH_MDR IOH
							WHERE 
												IOH.Store_No = @LastStore_No 
												AND IOH.SubTeam_No = @LastSubTeam_No 
												AND IOH.Item_Key = @LastItem_Key
						END
                    

-- *********** DON'T RESET THE TOTALS IF THIS IS JUST A CHANGE IN DATE *************

								IF NOT 
												((@LastItem_Key = @Item_Key) 
												AND (@LastSubTeam_No = @SubTeam_No) 
												AND (@LastStore_No = @Store_No))
							SELECT 
												@OnHandQty = ISNULL(OnHandQty, 0), 
												@OnHandWt = ISNULL(OnHandWt, 0)
							FROM 
												#ItemsOH_MDR IL
							WHERE 
												IL.Store_No = @Store_No 
												AND IL.SubTeam_No = @SubTeam_No 
												AND IL.Item_Key = @Item_Key
                
								SET				@LastStore_No = @Store_No
								SET				@LastSubTeam_No = @SubTeam_No
								SET				@LastItem_Key = @Item_Key
								SET				@LastDate_Key = @Date_Key
						END
            
								IF 
												@Adjustment_ID = 2
						BEGIN
								SET				@OnHandQty = @Quantity
								SET				@OnHandWt = @Weight
						END
						ELSE
						BEGIN
								SET				@OnHandQty = @OnHandQty + @Quantity
								SET				@OnHandWt = @OnHandWt + @Weight
						END
                    
							FETCH NEXT FROM 
								IHL INTO 
												@Store_No, 
												@SubTeam_No, 
												@Item_Key, 
												@Date_Key, 
												@Adjustment_ID, 
												@Quantity, 
												@Weight
				END
        
								IF 
												@LastStore_No > 0
							INSERT INTO 
												#OnHand 
												(
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												Date_Key, 
												OnHandQty, 
												OnHandWt
												)
							VALUES 
												(@LastStore_No, 
												@LastSubTeam_No, 
												@LastItem_Key, 
												@LastDate_Key, 
												@OnHandQty, 
												@OnHandWt)

			CLOSE IHL
			DEALLOCATE IHL
		
-- ******* UPDATE ON HAND *******
BEGIN TRAN

-- *********************************************************************************************************************************************        
-- ******** THE NEXT 3 STEPS USED TO BE 2: DELETE AND INSERT.  CHANGED USE UPDATE IN MINIMIZE THE DELETES AND INSERTS. **********************

						UPDATE 
												OnHandHistory
						SET 
												Quantity = O.OnHandQty, 
												Weight = O.OnHandWt
						FROM 
												OnHandHistory OH
							INNER JOIN 
												#OnHand O 
							ON 
												OH.Store_No = O.Store_No 
												AND OH.SubTeam_No = O.SubTeam_No 
												AND OH.Item_Key = O.Item_Key 
												AND OH.Date_Key = O.Date_Key
        

						INSERT INTO 
												OnHandHistory 
												(
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												Date_Key, 
												Quantity, 
												Weight
												)
						SELECT 
												O.Store_No, 
												O.SubTeam_No, 
												O.Item_Key, 
												O.Date_Key, 
												O.OnHandQty, 
												O.OnHandWt
						FROM 
												#OnHand O
							LEFT JOIN 
												OnHandHistory OH 
							ON 
												OH.Store_No = O.Store_No 
												AND OH.SubTeam_No = O.SubTeam_No 
												AND OH.Item_Key = O.Item_Key 
												AND OH.Date_Key = O.Date_Key
						WHERE 
												OH.Store_No IS NULL
        


						DELETE									
												OnHandHistory
						FROM 
												OnHandHistory OH
							INNER JOIN
								(SELECT			* 
									FROM 
												#ItemsOH
										UNION
								SELECT			*
									FROM 
												#ItemsOH_MDR) I 
							ON 
												I.Store_No = OH.Store_No 
												AND I.SubTeam_No = OH.SubTeam_No 
												AND I.Item_Key = OH.Item_Key
							LEFT JOIN 
												#OnHand O 
							ON 
												OH.Store_No = O.Store_No 
												AND OH.SubTeam_No = O.SubTeam_No 
												AND OH.Item_Key = O.Item_Key 
												AND OH.Date_Key = O.Date_Key
							WHERE 
												(OH.Date_Key >= I.MinDateStamp) 
												AND (O.Store_No IS NULL)
        
-- ***************** UPDATE THE CURRENT ONHAND RECORDS WITH THE LAST VALUES WRITTEN TO ONHANDHISTORY *********************

							UPDATE 
												OnHand
							SET 
												Quantity = ISNULL(IL.OnHandQty, 0), 
												Weight = ISNULL(IL.OnHandWt, 0)
							FROM 
												OnHand
								INNER JOIN
									(SELECT		* 
										FROM 
												#ItemsOH
									UNION
									SELECT		*
										FROM 
												#ItemsOH_MDR) IL 
								ON 
												IL.Store_No = OnHand.Store_No 
												AND IL.SubTeam_No = OnHand.SubTeam_No 
												AND IL.Item_Key = OnHand.Item_Key

COMMIT TRAN

-- ************************************************************************************************************************************        
        
-- ************************************ AVERAGE COST CALCULATION STARTS HERE **********************************************
        
							DECLARE				@pound int, 
												@unit int
        
							SELECT 
												@pound = Unit_ID 
							FROM				ItemUnit (nolock) 
							WHERE 
												EDISysCode = 'LB'

							SELECT 
												@unit = Unit_ID 
							FROM 
												ItemUnit (nolock) 
							WHERE 
												EDISysCode = 'EA'    

        
-- ********************** GET THE ITEMS THAT WERE RECEIVED ******************************************************

							INSERT INTO 
												#ItemsAC 
												(
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												MinDateStamp
												)
							SELECT 
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												CONVERT(smalldatetime, 
												CONVERT(varchar(255), 
												MIN(DateStamp), 101))
							FROM
								(SELECT 
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												DateStamp
								FROM 
												#ItemHistoryInsertedQueue tIHIQ
								WHERE 
												Adjustment_ID = 5 
												AND DateStamp >= DATEADD(day, -84, @PeriodBeginDate) -- DON'T GO BACK FURTHER THAN 3 FP'S
					UNION ALL
								SELECT 
												Store_No, 
												SubTeam_No, 
												Item_Key,
												DateStamp
								FROM 
												#ItemHistoryDeletedQueue tIHDQ
								WHERE 
												Adjustment_ID = 5 
												AND DateStamp >= DATEADD(day, -84, @PeriodBeginDate) -- DON'T GO BACK FURTHER THAN 3 FP'S
					UNION ALL
-- ****** ALSO RECALC AVERAGE COST FOR RECEIVING THAT OCCURRED IN THE CURRENT PERIOD AFTER A CYCLE COUNT INSERT
-- ****** FOR THE END OF THE PRIOR PERIOD.  THESE CYCLE COUNTS ARE INSERTED AFTER THE END OF THE PRIOR PERIOD AND AFTER
-- ****** RECEIVING HAS ALREADY BEEN RECORDED FOR THE CURRENT PERIOD.
								SELECT 
												IH.Store_No, 
												IH.SubTeam_No, 
												IH.Item_Key, 
												IH.DateStamp
								FROM 
												#ItemHistoryInsertedQueue Q
									INNER JOIN 
												ItemHistory IH (nolock)
									ON				
												IH.Store_No = Q.Store_No 
												AND IH.SubTeam_No = Q.SubTeam_No 
												AND IH.Item_Key = Q.Item_Key 
												AND IH.Adjustment_ID = 5 AND IH.DateStamp >= Q.DateStamp
								WHERE 
												Q.Adjustment_ID = 2 
												AND Q.DateStamp < @PeriodBeginDate
												AND Q.DateStamp >= DATEADD(day, -7, @PeriodBeginDate)) R
								GROUP BY 
												Store_No, 
												SubTeam_No, 
												Item_Key
                    
-- ********* GET THE STARTING ON-HAND AND AVERAGE COST FOR ITEMS THAT WERE RECEIVED ****************************
        
	-- ***VALUE AT THE MOST RECENT AVERAGE COST, FOR NEW ITEMS THIS VALUE WILL BE ZERO THE SECOND UPDATE BELOW ***
-- ******************WILL POPULATE THOSE WITH CURRENT PRIMARY VENDOR COST ***        
						UPDATE #ItemsAC
						SET	OnHand = (SELECT TOP 1 Quantity + Weight
										FROM	OnHandHistory OHH (nolock)
										WHERE 	OHH.Store_No = I.Store_No 
												AND OHH.SubTeam_No = I.SubTeam_No 
												AND OHH.Item_Key = I.Item_Key
												AND OHH.Date_Key < I.MinDateStamp
										ORDER BY OHH.Date_Key DESC),
							AvgCost = (SELECT TOP 1 AvgCost
										FROM	AvgCostHistory (nolock)
										WHERE 	Item_Key = I.Item_Key
                                                AND Store_No = I.Store_No
                                                AND SubTeam_No = I.SubTeam_No
                                                AND Effective_Date <= DATEADD(day, -1, I.MinDateStamp)
										ORDER BY Effective_Date DESC)
						FROM #ItemsAC I
								INNER JOIN	Item (nolock)
								ON Item.Item_Key = I.Item_Key

-- ******************POPULATE THOSE WITH CURRENT PRIMARY VENDOR COST FOR NEW ITEMS*****************************************  
-- ******************Bug 13869/1836: For new items use VCH PD1 and Cost Unit (instead of ITEM) to get prior Avg Cost*******************************      
              			UPDATE #ItemsAC
						SET AvgCost = dbo.fn_CostConversion (vc.NetCost,
														vc.CostUnit_ID, 
														CASE WHEN Item.CostedByWeight = 1 THEN @pound ELSE @unit END, 
														vc.Package_Desc1, 
														Item.Package_Desc2, 
														Item.Package_Unit_ID) 
						FROM #ItemsAC I
							INNER JOIN	Item (nolock)
								ON Item.Item_Key = I.Item_Key	
							INNER JOIN 	StoreItemVendor SIV (nolock)
								ON SIV.Item_Key = I.Item_Key 
									AND SIV.Store_No = I.Store_No 
									AND SIV.PrimaryVendor = 1
							INNER JOIN dbo.fn_VendorCostAll ( GETDATE() ) vc 
								ON vc.Item_Key = I.Item_Key	
									AND vc.Vendor_ID = SIV.Vendor_ID	
									AND vc.Store_no = I.Store_No
						WHERE ISNULL(AvgCost,0) = 0																					             
			        	        
					UPDATE	
												#ItemsAC
					SET		
												OnHand = CASE WHEN ISNULL(OnHand, 0) <= 0 THEN 0 ELSE OnHand END,
												AvgCost = ISNULL(AvgCost, 0)

        
-- *************** ADD UP A RUNNING TOTAL A DAY AT A TIME UNTIL WE HIT TOMORROW, SAVING A DAILY TOTAL PER ITEM ********************	
-- ******************** TO START THE LOOP, GET THE MINIMUM DATE ACROSS ALL ITEMS **************************************************

					SELECT 
												@NextDate = MIN(MinDateStamp)
					FROM 
												#ItemsAC
        
				WHILE 
												@NextDate < @Tomorrow
				BEGIN
					INSERT INTO 
												#AvgCost 
												(Store_No, 
												SubTeam_No, 
												Item_Key, 
												Date_Key, 
												AvgCost
												)
					SELECT 
												Store_No, 
												SubTeam_No, 
												Item_Key, 
												@NextDate,
												(MAX(AvgCost * OnHand) + SUM(RCost)) / (MAX(OnHand) + SUM(UnitsReceived))
					FROM 
						(SELECT 
												IL.Store_No, 
												IL.SubTeam_No, 
												IL.Item_Key, 
												IL.AvgCost, 
									CASE WHEN	IL.AvgCost > 0 THEN		IL.OnHand 
										 ELSE	0 
										 END	As OnHand, 
									CASE WHEN	Shipper.Shipper_Key IS NOT NULL THEN Shipper.Quantity * OI.QuantityReceived 
										 ELSE	UnitsReceived END As UnitsReceived,
												((ReceivedItemCost + ReceivedItemFreight) / UnitsReceived) * 
									CASE WHEN	Shipper.Shipper_Key IS NOT NULL THEN Shipper.Quantity * OI.QuantityReceived 
									ELSE		UnitsReceived END  As RCost
						FROM 
												#ItemsAC IL
								INNER JOIN
												ItemHistory IH with (index(idxItemHistoryAdjID), NoLock)
								ON 
												IH.Store_No = IL.Store_No 
												AND IH.SubTeam_No = IL.SubTeam_No 
												AND IH.Item_Key = IL.Item_Key
												AND IH.Adjustment_ID = 5 
												AND (IH.DateStamp >= @NextDate AND IH.DateStamp < DATEADD(day, 1, @NextDate)) 
								INNER JOIN 
												OrderItem OI (NOLOCK) 
								ON 
												OI.OrderItem_ID = IH.OrderItem_ID
								INNER JOIN
												Item (nolock)
								ON 
												Item.Item_Key = IH.Item_Key
								LEFT JOIN
												Shipper (nolock)
								ON 
												Shipper.Item_Key = IH.Item_Key 
												AND Shipper.Shipper_Key = OI.Item_Key
							
						WHERE
												OI.UnitsReceived > 0
-- ************* GO ONLY AS FAR BACK PER ITEM AS THE MINIMUM MOVEMENT DATE *********
												AND IL.MinDateStamp <= @NextDate
												) T
					GROUP BY 
												Store_No, 
												SubTeam_No, 
												Item_Key
           
            
            
            
            
            
                -- Suspend bad average costs
				UPDATE	A
				SET		Suspended = 1,
						PriorAvgCost = I.AvgCost,
						Variance_Pct = CASE WHEN (ROUND(ABS(A.AvgCost - I.AvgCost) / CASE WHEN I.AvgCost = 0 THEN 1 ELSE I.AvgCost END, 4) < 999.9999) 
											THEN (ROUND(ABS(A.AvgCost - I.AvgCost) / CASE WHEN I.AvgCost = 0 THEN 1 ELSE I.AvgCost END, 4)) 
											ELSE 999 END
				FROM	#AvgCost A
						INNER JOIN	#ItemsAC I
							ON	A.Store_No = I.Store_No 
							AND A.SubTeam_No = I.SubTeam_No 
							AND A.Item_Key = I.Item_Key 
							AND ISNULL(I.AvgCost, 0) > 0
						INNER JOIN	Store (nolock)
							ON	Store.Store_No = A.Store_No 
							AND Store.Distribution_Center = 0 -- Exclude all DC's because they review all PO costs on a line item basis and they do have significant fluctuation for many items and operationally they cannot keep up with Suspended Average Cost reconciliation
				WHERE	Date_Key = @NextDate
						AND  I.AvgCost > 0 AND A.AvgCost > 0
						AND ROUND(ABS(A.AvgCost - I.AvgCost) / CASE WHEN I.AvgCost = 0 THEN 1 ELSE I.AvgCost END, 2) >= 0.1
						AND EXISTS (	SELECT	*
										FROM	#ItemHistoryInsertedQueue Q
												INNER JOIN ItemHistory IH (nolock)
													ON IH.ItemHistoryID = Q.ItemHistoryID 
										WHERE	A.Store_No = Q.Store_No AND A.SubTeam_No = Q.SubTeam_No AND A.Item_Key = Q.Item_Key AND A.Date_Key = CONVERT(smalldatetime, CONVERT(varchar(255), Q.DateStamp, 101)) AND Q.Adjustment_ID = 5
									)
            
            
            
            

-- ******* UPDATE THE STARTING ON HAND AND AVERAGE COST FOR THE NEXT ITERATION OF THIS LOOP ***********
					UPDATE 
												#ItemsAC
					SET 
												OnHand = ISNULL(
							(SELECT TOP 1		
												Quantity + Weight
                             FROM 
												OnHandHistory OHH (nolock)
                             WHERE 
												OHH.Store_No = I.Store_No 
												AND OHH.SubTeam_No = I.SubTeam_No 
												AND OHH.Item_Key = I.Item_Key
                                                AND OHH.Date_Key <= @NextDate
                             ORDER BY 
												OHH.Date_Key DESC), 0),
												AvgCost = ISNULL(
							(SELECT 
												AvgCost 
                             FROM 
												#AvgCost A 
                             WHERE 
												A.Store_No = I.Store_No 
												AND A.SubTeam_No = I.SubTeam_No 
												AND A.Item_Key = I.Item_Key
                                                AND A.Date_Key = @NextDate
                                                ), AvgCost)
					FROM #ItemsAC I	

-- ****************************************************************************************************************************            

						UPDATE 
												#ItemsAC
						SET 
												OnHand = CASE WHEN ISNULL(OnHand, 0) <= 0 THEN 0 ELSE OnHand END,
												AvgCost = ISNULL(AvgCost, 0)
        

						-- NEXT DAY
						SET	
												@NextDate = DATEADD(day, 1, @NextDate)
				END
        
 /*
		#####################################################################################################################################
		RDE 10.4.2010: This logic was found in V1 but did not exist in v4. Copying from V1 to V4 but leaving commeted out as a place holder.
		This may need to be uncommented in the future. I found this while researching SuspnededAvgCost logic.
		#####################################################################################################################################
		-- Update the store SIV records so that outbound DC PO's will get their costs refreshed 
        UPDATE SIV
        SET LastCostAddedDate = AC.MinDate_Key,
            LastCostRefreshedDate = DATEADD(day, -1, AC.MinDate_Key) -- Make this date less than LastCostAddedDate so that the refresh PO's cost process will pick up the applicable PO's
        FROM StoreItemVendor SIV
        INNER JOIN Vendor (nolock) ON Vendor.Vendor_ID = SIV.Vendor_ID
        INNER JOIN Store (nolock) ON Store.Store_No = Vendor.Store_No AND Store.Distribution_Center = 1
        INNER JOIN 
            (SELECT A.Store_No, A.Item_Key, MIN(A.Date_Key) As MinDate_Key
             FROM #AvgCost A
             WHERE (A.AvgCost IS NOT NULL) AND ISNULL(A.Suspended, 0) = 0
                AND (ISNULL((SELECT TOP 1 AvgCost
                                     FROM AvgCostHistory (nolock)
                                     WHERE Item_Key = A.Item_Key
                                         AND Store_No = A.Store_No
                                         AND SubTeam_No = A.SubTeam_No
                                         AND Effective_Date < A.Date_Key
                                     ORDER BY Effective_Date DESC), 0) <> A.AvgCost) 
             GROUP BY A.Store_No, A.Item_Key) AC ON AC.Store_No = Vendor.Store_No AND AC.Item_Key = SIV.Item_Key
        WHERE SIV.DeleteDate IS NULL 
*/


        
-- ***************** UPDATE AVERAGE COST ******************
                
						UPDATE 
												AvgCostHistory
						SET										
												AvgCost = A.AvgCost
						FROM 
												AvgCostHistory H
							INNER JOIN 
												#AvgCost A 
							ON					A.Item_Key = H.Item_Key 
												AND A.Store_No = H.Store_No 
												AND A.SubTeam_No = H.SubTeam_No 
												AND A.Date_Key = H.Effective_Date 
						WHERE 
												A.AvgCost <> H.AvgCost
        
-- ***************** INSERT INTO AVERAGE COST ******************

						INSERT INTO 
												AvgCostHistory 
												(
												Item_Key, 
												Store_No, 
												SubTeam_No, 
												Effective_Date, 
												AvgCost
												)
						SELECT 
												A.Item_Key, 
												A.Store_No, 
												A.SubTeam_No, 
												A.Date_Key, 
												A.AvgCost
						FROM 
												#AvgCost A
								LEFT JOIN 
												AvgCostHistory H
								ON 
												A.Item_Key = H.Item_Key 
												AND A.Store_No = H.Store_No 
												AND A.SubTeam_No = H.SubTeam_No 
												AND A.Date_Key = H.Effective_Date 
						WHERE 
												(H.Item_Key IS NULL) 
												AND (A.AvgCost IS NOT NULL) 
												AND (ISNULL(
							(SELECT TOP 1		
												AvgCost
                             FROM 
												AvgCostHistory (nolock)
                             WHERE 
												Item_Key = A.Item_Key
												AND Store_No = A.Store_No
												AND SubTeam_No = A.SubTeam_No
												AND Effective_Date < A.Date_Key
                             ORDER BY 
												Effective_Date DESC), 0) <> A.AvgCost)
        
-- **************************************************************************************************************************** 
							DELETE 
												AvgCostHistory
							FROM 
												AvgCostHistory H
								INNER JOIN
												#ItemsAC I
								ON 
												I.Item_Key = H.Item_Key 
												AND I.Store_No = H.Store_No 
												AND I.SubTeam_No = H.SubTeam_No
								LEFT JOIN 
												#AvgCost A 
								ON		
												A.Item_Key = H.Item_Key 
												AND A.Store_No = H.Store_No 
												AND A.SubTeam_No = H.SubTeam_No 
												AND A.Date_Key = H.Effective_Date 
							WHERE
												(H.Effective_Date >= I.MinDateStamp) 
												AND (A.Item_Key IS NULL)
        

-- ####################################### SUSPENDED AVG COST ####################
  -- Before updating the Suspended Average Costs, handle Shipper Items.  If more than one item in a Shipper is affected, keep only the one with the biggest new average cost difference
        -- since table, SuspendedAvgCost, has primary key of OrderItem_ID and a Shipper can cause a PK violation because it is one Order Item but multiple ItemHistory records for each Shipper Item
        INSERT INTO 	#Suspend 	(
										OrderItem_ID,
										Store_No, 
										SubTeam_No,
										Item_Key,
										Date_Key, 
										PriorAvgCost, 
										NewAvgCost, 
										Variance_Pct
									)
        SELECT  					IH.OrderItem_ID, 
                                    A.Store_No,
                                    A.SubTeam_No,
                                    A.Item_Key,
                                    A.Date_Key,
									A.PriorAvgCost, 
									A.AvgCost, 
									A.Variance_Pct
        FROM 			#AvgCost A
						INNER JOIN 	#ItemHistoryInsertedQueue Q
									ON 	A.Store_No = Q.Store_No 
										AND A.SubTeam_No = Q.SubTeam_No 
										AND A.Item_Key = Q.Item_Key 
										AND A.Date_Key = CONVERT(smalldatetime, CONVERT(varchar(255), Q.DateStamp, 101)) 
										AND Q.Adjustment_ID = 5
						INNER JOIN 	ItemHistory IH (nolock)
									ON 	IH.ItemHistoryID = Q.ItemHistoryID
        WHERE 			A.Suspended = 1
        
		DELETE 	S
        FROM 	#Suspend S
				INNER JOIN 
					(
						SELECT 	Store_No, 
						        SubTeam_No, 
						        Item_Key,
								MAX(Date_Key) As MaxDate_Key
						FROM 	#Suspend
						GROUP BY Store_No, 
						         SubTeam_No, 
						         Item_Key
					) M 
					ON M.Store_No   = S.Store_No
				   AND M.SubTeam_No = S.SubTeam_No
				   AND M.Item_Key   = S.Item_Key
         WHERE  S.Date_Key <> M.MaxDate_Key   
        
		DELETE 	S
        FROM 	#Suspend S
				INNER JOIN 
					(
						SELECT 	Store_No, 
						        SubTeam_No, 
						        Item_Key,
								MAX(OrderItem_ID) As MaxOrderItem_ID
						FROM 	#Suspend
						GROUP BY Store_No, 
						         SubTeam_No, 
						         Item_Key
					) M 
					ON M.Store_No   = S.Store_No
				   AND M.SubTeam_No = S.SubTeam_No
				   AND M.Item_Key   = S.Item_Key
         WHERE  S.OrderItem_ID <> M.MaxOrderItem_ID

        UPDATE 	SAC
        SET 	OrderItem_ID   = N.OrderItem_ID,
                OrderHeader_ID = OI.OrderHeader_ID,
                PriorAvgCost   = N.PriorAvgCost,
				NewAvgCost     = N.NewAvgCost,
				Effective_Date = N.Date_Key,
				Variance_Pct   = N.Variance_Pct,
				LastUpdateDt   = GETDATE()
        FROM    SuspendedAvgCost SAC
		INNER JOIN  #Suspend N  
					ON SAC.Store_No   = N.Store_No 
				   AND SAC.SubTeam_No = N.SubTeam_No  
				   AND SAC.Item_Key   = N.Item_Key
	    INNER JOIN OrderItem OI
		            ON N.OrderItem_ID = OI.OrderItem_ID  
        
        INSERT INTO SuspendedAvgCost (
										OrderItem_ID,
										OrderHeader_ID,
										Store_No, 
										SubTeam_No,
										Item_Key,
										Effective_Date,
										PriorAvgCost, 
										NewAvgCost,
										Variance_Pct 
									)
				SELECT 					Distinct
				                        S.OrderItem_ID,
				                        OI.OrderHeader_ID, 
				                        S.Store_No,
				                        S.SubTeam_No,
				                        S.Item_Key,
				                        S.Date_Key,
										S.PriorAvgCost, 
										S.NewAvgCost,
										S.Variance_Pct 
				FROM 	#Suspend S
				INNER JOIN OrderItem OI
				        ON S.OrderItem_ID = OI.OrderItem_ID
				WHERE 	NOT EXISTS (	SELECT 	* 
										FROM 	SuspendedAvgCost SAC 
										WHERE 	SAC.Store_No       = S.Store_No
										  AND   SAC.SubTeam_No     = S.SubTeam_No
										  AND   SAC.Item_Key       = S.Item_Key
									)															
                    
-- ****************** DELETE ALL RECORDS FROM THE QUEUE TABLES  ******************************


							DELETE 
												ItemHistoryInsertedQueue
							FROM 
												ItemHistoryInsertedQueue Q
								INNER JOIN 
												#ItemHistoryInsertedQueue QT 
								ON				
												Q.ID = QT.ID

							DELETE									
												ItemHistoryDeletedQueue
							FROM 
												ItemHistoryDeletedQueue Q
								INNER JOIN 
												#ItemHistoryDeletedQueue QT 
								ON 
												Q.ID = QT.ID
            

-- **************************************************************************************************************************** 
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN
       
        DECLARE @err_no int, @err_sev int, @err_msg nvarchar(4000)
        SELECT @err_no = ERROR_NUMBER(), @err_sev = ERROR_SEVERITY(), @err_msg = ERROR_MESSAGE()
        RAISERROR ('UpdateAverageCost failed with @@ERROR: %d - %s', @err_sev, 1, @err_no, @err_msg)
    END CATCH
	
	DROP TABLE #ItemHistoryInsertedQueue
	DROP TABLE #ItemHistoryDeletedQueue
	DROP TABLE #ItemsOH
	DROP TABLE #ItemsOH_MDR
	DROP TABLE #ItemsAC
	DROP TABLE #AvgCost
	DROP TABLE #OnHand
	DROP TABLE #Suspend
	        
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateAverageCost] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateAverageCost] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateAverageCost] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateAverageCost] TO [IRMAReportsRole]
    AS [dbo];

