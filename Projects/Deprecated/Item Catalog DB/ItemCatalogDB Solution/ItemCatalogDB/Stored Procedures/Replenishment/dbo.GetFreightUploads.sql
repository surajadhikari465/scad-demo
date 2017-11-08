SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFreightUploads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[GetFreightUploads]
GO


CREATE PROCEDURE [dbo].[GetFreightUploads]
    
    @AccountNum int,
    @FreightAccountNum int,
	@Currency char(3),
	@APUploadDateDiff int
AS
	-- ******************************************************************************************************
	-- Procedure: GetFreightUploads()
	--    Author: Victoria Afonina
	--      Date: 09/01/2012
	--
	-- Description:
	-- Procedure provides Freight charges (for Hawaiian stores) for AP Uploaded orders with freight   
	-- It will create fixed length delimited data file to be uploaded to PeopleSoft.
	--
	-- Run it using Data Monkey job scheduled in Tidal every day after AP Upload job is completed
	-- Pull freight for all AP-Uploaded orders that day since those orders can't be re-opened again
    --
    -- @APUploadDateDiff = 0 if freight is pulled for today's uploadedDate, 
    -- 1 for yesterday, 2 for day before yesterday etc  
	-- Should be set to 0 for daily automatic freight job
	--
	-- Modification History:
	-- Date       	Init  	TFS   	Comment
	-- 09/01/2012	VA		7370	SP created
	-- 09/17/2012   VA              Cursors removed 
	
	-- ******************************************************************************************************
BEGIN
	SET NOCOUNT ON  
		
	SET @AccountNum = CASE @AccountNum WHEN 0 THEN 540000 ELSE @AccountNum END
	SET @FreightAccountNum = CASE @FreightAccountNum WHEN 0 THEN 201110 ELSE @FreightAccountNum END
	IF  len(@Currency) <> 3 SELECT @Currency = 'USD'


	--SET STATISTICS IO ON


-------------------------------------------------------------------------------------------------        
 -- Filter down to AP Uploaded Orders       
 -------------------------------------------------------------------------------------------------
			
	DECLARE @APUploadedOrderHeader TABLE (OrderHeader_ID int)

	INSERT INTO @APUploadedOrderHeader (OrderHeader_ID) 
		SELECT OrderHeader_ID
		FROM orderHeader (NOLOCK)
		WHERE cast(UploadedDate as date)  = cast(DateAdd(d,-1*@APUploadDateDiff,GETDATE()) as date)
		AND Return_Order = 0			-- exclude credits
		AND Transfer_SubTeam IS NULL	-- exclude transfers
			        
 -------------------------------------------------------------------------------------------------        
 -- Filter down to only AP-Uploaded Orders with freight       
 -------------------------------------------------------------------------------------------------
 
	DECLARE @OrderHeader_ID int = 0

	DECLARE @UploadedOrderHeader TABLE (OrderHeader_ID int, ReceivedItemFreight money,BusinessUnit_ID int, Transfer_To_SubTeam int)

 	SELECT @OrderHeader_ID = MIN(OrderHeader_ID) FROM @APUploadedOrderHeader

	WHILE ISNULL(@OrderHeader_ID, 0) > 0
	BEGIN
	
		INSERT INTO @UploadedOrderHeader (OrderHeader_ID, ReceivedItemFreight, BusinessUnit_ID, Transfer_To_SubTeam)
			SELECT @OrderHeader_ID, SUM(Freight * quantityReceived), s.BusinessUnit_ID, oh.Transfer_To_SubTeam
			FROM OrderItem (NOLOCK) oi 
			INNER JOIN Item (NOLOCK) i on oi.Item_Key = i.item_key
			INNER JOIN OrderHeader (NOLOCK) oh on oi.OrderHeader_ID = oh.OrderHeader_ID
			INNER JOIN Vendor v (NOLOCK) on oh.ReceiveLocation_ID = v.Vendor_ID
			INNER JOIN Store s (NOLOCK) on v.Store_no = s.store_no
			WHERE  oi.OrderHeader_ID = @OrderHeader_ID 
			AND Freight * quantityReceived > 0   
			AND ISNULL(i.Sales_Account, '') <> 891000 -- exclude bottle deposit
			GROUP BY oi.OrderHeader_ID, s.BusinessUnit_ID, oh.Transfer_To_SubTeam
													 
		SELECT @OrderHeader_ID = MIN(OrderHeader_ID) FROM @APUploadedOrderHeader WHERE OrderHeader_ID > @OrderHeader_ID
	END

 -------------------------------------------------------------------------------------------------        
 -- Get Fixed Length GL Freight Output for PeopleSoft      
 ------------------------------------------------------------------------------------------------- 

	DECLARE @Locations TABLE(BusinessUnit_ID int)

	INSERT INTO @Locations(BusinessUnit_ID)
		SELECT BusinessUnit_ID
		FROM @UploadedOrderHeader
		GROUP BY BusinessUnit_ID		
	 
	DECLARE @Subteams TABLE(BusinessUnit_ID int, Transfer_To_SubTeam int)

	INSERT INTO @Subteams	(BusinessUnit_ID, Transfer_To_SubTeam)
		SELECT BusinessUnit_ID, Transfer_To_SubTeam
		FROM @UploadedOrderHeader
		GROUP BY BusinessUnit_ID, Transfer_To_SubTeam
				
	DECLARE @BusinessUnit_ID int = -1
	DECLARE @Transfer_To_SubTeam int = -1
	DECLARE @JournalLineNumber int = 0

	DECLARE @Freight TABLE(Record varchar (5000))

 -------------------------------------------------------------------------------------------------        
 -- Get Jornal Header for Business Unit     
 -------------------------------------------------------------------------------------------------
			
	SELECT @BusinessUnit_ID = MIN(BusinessUnit_ID) FROM @Locations

	WHILE ISNULL(@BusinessUnit_ID,0) > 0
	
	BEGIN
		INSERT INTO @Freight (Record)
		SELECT 
		'H' + 
		left(CAST(@BusinessUnit_ID as varchar)+replicate(' ', 5), 5) +                       -- BusinessUnit_ID
		left('NEXT'+replicate(' ', 10), 10) +                                                -- Journal ID
		left(REPLACE(CONVERT(VARCHAR(10), GETDATE(), 101), '/', '') +replicate(' ', 8), 8) + -- Journal Date
		left('GLBOOK'+replicate(' ', 10), 10) +                                              -- Ledger Group
		'N' +                                                                                -- Reversal Code
		left(' '+replicate(' ', 8), 8) +                                                     -- Reversal Date
		'XLS' +                                                                              -- Source
		left(' '+replicate(' ', 8), 8) +                                                     -- Transaction Ref Num
		left('OPSAUTO FREIGHT FOR BU ' + CAST(@BusinessUnit_ID as varchar) 
		+ replicate(' ', 30), 30) +                                                          -- Journal Desc
		left(@Currency+replicate(' ', 3), 3) +											     -- Currency Type
		'CRRNT' +                                                                            -- Rate Type
		left(' '+replicate(' ', 8), 8) + 
		left(' '+replicate(' ', 16), 16) +                                                   -- Rate Mult
		left(' '+replicate(' ', 8), 8) +                                                     -- Document Type
		left(' '+replicate(' ', 12), 12) +                                                   -- Document Seq Num
		left(' '+replicate(' ', 8), 8) +                                                     -- Blank Field
		'EXT'                                                                                -- System Source 
	    	       						                        
-------------------------------------------------------------------------------------------------        
-- Get Jornal Line for Freight GL Account for each PeopleSoft subteam in the Business Unit   
-------------------------------------------------------------------------------------------------	                     
		          	          			          
 
		SELECT @Transfer_To_SubTeam = MIN(Transfer_To_SubTeam) FROM @Subteams where BusinessUnit_ID = @BusinessUnit_ID 

		WHILE ISNULL(@Transfer_To_SubTeam,0) > 0
			BEGIN
                
			SELECT @JournalLineNumber = @JournalLineNumber + 1    
             		                 
			INSERT INTO @Freight (Record)    
			SELECT 
			'L' +  
			left(CAST(@BusinessUnit_ID as varchar)+replicate(' ', 5), 5)  +                  -- BusinessUnit_ID 
			left(CAST(@JournalLineNumber as varchar)+replicate(' ', 9), 9) +                 -- LINE NUMBER
			left('ACTUAL'+replicate(' ', 10), 10) +                                          -- Ledger 
			left(CAST(@AccountNum as varchar) + replicate(' ', 6), 6) +                      -- Account 
			left(CAST(ss.PS_Team_No as varchar) + replicate(' ', 10), 10) +                  -- PSFT Dept 
			left(CAST(ss.PS_SubTeam_No as varchar) + replicate(' ', 6), 6) +                 -- PSFT Subteam
			left(' ' + replicate(' ', 15), 15) +                                             -- Proj
			left(' ' + replicate(' ', 5), 5) +                                               -- Affiliate
			left(' ' + replicate(' ', 3), 3) +                                               -- Statistics Code
			left(@Currency+replicate(' ', 3), 3)+                                            -- Curr
			left(CAST(SUM(oh.ReceivedItemFreight) as varchar)+replicate(' ', 28), 28) +      -- Amount
			left(' ' + replicate(' ', 4), 4) +                                               -- Blank field
			left(' ' + replicate(' ', 10), 10) +                                             -- Journal Line Ref
			left('IRMA' + replicate(' ', 30), 30) +                                          -- 
			'CRRNT' +                                                                        -- Rate Type
			left('1' + replicate(' ', 16), 16) +                                             -- Rate Mult 	  
			left(CAST(SUM(oh.ReceivedItemFreight) as varchar)+replicate(' ', 16), 16) +      -- Foreign Amount
			'N' +
			left(' ' + replicate(' ', 30), 30)                                               -- Open Item Key

			FROM @UploadedOrderHeader oh
			inner join StoreSubteam ss (NOLOCK) ON oh.Transfer_To_Subteam = ss.SubTeam_no
			inner join Store s (NOLOCK) ON oh.BusinessUnit_ID = s.BusinessUnit_ID AND ss.store_no = s.Store_No

			WHERE  (
				s.WFM_Store = 1 OR
				s.Mega_Store = 1 OR
				s.Distribution_Center = 1 OR
				s.Manufacturer = 1
			)
		
			AND oh.BusinessUnit_ID = @BusinessUnit_ID 
			AND oh.Transfer_To_SubTeam = @Transfer_To_SubTeam
			GROUP BY ss.PS_Team_No, ss.PS_SubTeam_No			                    
                
			SELECT @JournalLineNumber = @JournalLineNumber + 1    

-------------------------------------------------------------------------------------------------        
-- Get Jornal Line for Asset Account for each PeopleSoft subteam in the Business Unit     
-------------------------------------------------------------------------------------------------			                 
             
			INSERT INTO @Freight (Record)    
			SELECT 
			'L' +  
			left(CAST(@BusinessUnit_ID as varchar)+replicate(' ', 5), 5)  +                  -- BusinessUnit_ID 
			left(CAST(@JournalLineNumber as varchar)+replicate(' ', 9), 9) +                 -- LINE NUMBER
			left('ACTUAL'+replicate(' ', 10), 10) +                                         -- Ledger 
			left(CAST(@FreightAccountNum as varchar) + replicate(' ', 6), 6) +               -- Freight Account 
			left(CAST(ss.PS_Team_No as varchar) + replicate(' ', 10), 10) +                  -- PSFT Dept 
			left(CAST(ss.PS_SubTeam_No as varchar) + replicate(' ', 6), 6) +                 -- PSFT Subteam
			left(' ' + replicate(' ', 15), 15) +                                             -- Proj
			left(' ' + replicate(' ', 5), 5) +                                               -- Affiliate
			left(' ' + replicate(' ', 3), 3) +                                               -- Statistics Code
			left(@Currency+replicate(' ', 3), 3)+                                            -- Curr
			left(CAST(-1 * SUM(oh.ReceivedItemFreight) as varchar)+replicate(' ', 28), 28) + -- Amount
			left(' ' + replicate(' ', 4), 4) +                                               -- Blank field
			left(' ' + replicate(' ', 10), 10) +                                             -- Journal Line Ref
			left('IRMA' + replicate(' ', 30), 30) +                                          -- 
			'CRRNT' +                                                                        -- Rate Type
			left('1' + replicate(' ', 16), 16) +                                             -- Rate Mult 	  
			left(CAST(-1 * SUM(oh.ReceivedItemFreight) as varchar)+replicate(' ', 16), 16) + -- Foreign Amount
			'N' +
			left(' ' + replicate(' ', 30), 30)                                               -- Open Item Key

			FROM @UploadedOrderHeader oh
			inner join StoreSubteam ss (NOLOCK) ON oh.Transfer_To_Subteam = ss.SubTeam_no
			inner join Store s (NOLOCK) ON oh.BusinessUnit_ID = s.BusinessUnit_ID and ss.store_no = s.Store_No

			WHERE 									
			(
				s.WFM_Store = 1 OR
				s.Mega_Store = 1 OR
				s.Distribution_Center = 1 OR
				s.Manufacturer = 1
			)
		
			AND oh.BusinessUnit_ID = @BusinessUnit_ID
			AND oh.Transfer_To_SubTeam = @Transfer_To_SubTeam
			GROUP BY ss.PS_Team_No, ss.PS_SubTeam_No
														                    	                    
				SELECT @Transfer_To_SubTeam = MIN(Transfer_To_SubTeam) FROM @Subteams 
				WHERE BusinessUnit_ID = @BusinessUnit_ID AND Transfer_To_SubTeam > @Transfer_To_SubTeam 
			END
		          
-------------------------------------------------------------------------------------------------        
-- End of Records for the Business Unit. Start next Business Unit    
------------------------------------------------------------------------------------------------- 		                     
			
		SELECT @JournalLineNumber = 0
				
		SELECT @BusinessUnit_ID = MIN(BusinessUnit_ID) FROM @Locations WHERE BusinessUnit_ID > @BusinessUnit_ID
	END
						
	SELECT * from @Freight

    --SET STATISTICS IO OFF
	
	SET NOCOUNT OFF
END

GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO