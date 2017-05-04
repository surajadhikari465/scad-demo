IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AllocationReport]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[AllocationReport]
GO

/****** Object:  StoredProcedure [dbo].[AllocationReport]    Script Date: 07/25/2012 14:57:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[AllocationReport]
    @Store_No int,
    @Transfers int,
    @SubTeam_No int,
    @Pre_Order int,
    @BOH int,
    @IncludeWOO bit,
    @ShipDate varchar(255),
    @ExpectedDateStart varchar(255),
    @ExpectedDateEnd varchar(255),
    @WarehouseSent int
AS
   -- **************************************************************************
   -- Procedure: AllocationReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date			Init		Comment
   -- 06/16/2009	BBB			Updated SP to be more readable
   -- 06/24/2009	BBB			Removed check between ExpectedDate <= CloseDate in @Orders;
   --							removed check for items in @Orders from @BOH_WOO insert	
   -- 07/01/2009	BSR			Added criteria to pull only items in current FSA session
   --							Added criteria to sums for total OH
   --							EXEC AllocationReport 808,0,4,2,0,'7/2/2009',0		
   -- 11/17/2009	RDS			Added criteria to specify whether to include inbound PO quantities
   -- 							in @BOH_WOO totals and use a date range when true. Also added
   --							additional BOH filtering options.
   -- 10/01/2012	Damon F		Removed all references to ItemCaseHistory
   -- **************************************************************************
BEGIN
	--**************************************************************************
	--Create date variables
	--**************************************************************************
    DECLARE @Date datetime
    SELECT @Date = CONVERT(datetime, @ShipDate)

	--**************************************************************************
    -- Get list of Orders first for simplicity to avoid repeating the conditions below
	--**************************************************************************
    DECLARE @Orders TABLE (OrderHeader_ID int PRIMARY KEY)

    INSERT INTO @Orders
	SELECT
		OrderHeader_ID
	FROM
			(
				SELECT 
					oh.OrderHeader_ID,
					CASE 
						WHEN oh.Transfer_SubTeam = oh.Transfer_To_SubTeam THEN 0 
						ELSE 1 
					END AS Transfer
				FROM 
					OrderHeader			(nolock) oh
					INNER JOIN  Vendor	(nolock) v	ON	oh.Vendor_ID	= v.Vendor_ID 
													AND v.Store_No		= @Store_No
			WHERE 
					oh.Transfer_SubTeam					=	ISNULL(@SubTeam_No, oh.Transfer_SubTeam)
					AND oh.Expected_Date				=	@Date 
					AND oh.Sent							=	1 
					AND oh.WarehouseSent				=	CASE @WarehouseSent 
																WHEN -1 THEN 
																	WarehouseSent 
																ELSE @WarehouseSent
															END
			) AS Orders
	WHERE Transfer = (CASE WHEN @Transfers = -1 THEN Transfer ELSE @Transfers END)								
													
	--**************************************************************************
	-- Need to match up the BOH with the WOO by Pack Size
	--**************************************************************************
	DECLARE @BOH_WOO TABLE (Item_Key int, PackSize decimal(9,4), OH decimal(18,4), OO decimal(18,4))

	--**************************************************************************
	-- First get the WOO
	--**************************************************************************
	
	BEGIN
		IF @IncludeWOO = 1
			INSERT INTO @BOH_WOO
				SELECT
					oi.Item_Key, 
					oi.Package_Desc1, 
					0, 
					SUM(oi.QuantityOrdered)
				FROM 
					OrderHeader				(nolock) oh
					INNER JOIN Vendor		(nolock) rl	ON	rl.Vendor_ID		= oh.ReceiveLocation_ID 
														AND rl.Store_No			= @Store_No
					INNER JOIN OrderItem	(nolock) oi	ON	oi.OrderHeader_ID	= oh.OrderHeader_ID
														AND oh.CloseDate		IS NULL 
														AND oh.Sent				= 1
														AND oh.Expected_Date	>= @ExpectedDateStart
														AND oh.Expected_Date	<= @ExpectedDateEnd
														AND oi.DateReceived		IS NULL
				GROUP BY 
					oi.Item_Key, 
					oi.Package_Desc1
	END

	--**************************************************************************
	--Create date variables
	--**************************************************************************
    DECLARE @EndDate datetime
    SELECT @EndDate = CONVERT(datetime, CONVERT(varchar(12), DATEADD(day, -1, @Date), 101) + ' 6:00:00 PM')

	--**************************************************************************
    -- Get the Item Unit ID's so we can call CostConverion
	--**************************************************************************
    DECLARE @Case int, @Pound int
    
    SELECT @Case = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Case'
    SELECT @Pound = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Pound'

	--**************************************************************************
    -- Next get the BOH for the items on the selected Orders
	--**************************************************************************
    INSERT INTO @BOH_WOO
		SELECT
			ih.Item_Key, 
			i.Package_Desc1,
			SUM(CASE 
					WHEN ISNULL(ih.Quantity, 0) > 0 THEN 
						ih.Quantity /	CASE 
													WHEN i.Package_Desc1 <> 0 THEN 
														i.Package_Desc1 
													ELSE 
														1 
												END
					ELSE ISNULL(ih.Weight, 0) /	CASE 
																WHEN i.Package_Desc1 * i.Package_Desc2 <> 0 THEN 
																	(i.Package_Desc1 * i.Package_Desc2) 
																ELSE 
																	1 
															END 
				 END * ia.Adjustment_Type) AS OnHand,
			0
		FROM
			Item						(nolock) i
			INNER JOIN	ItemIdentifier	(nolock) ii		ON	ii.Item_Key				= i.Item_Key 
														AND ii.Default_Identifier	= 1
			INNER JOIN	OnHand			(nolock) oh		ON	oh.Item_Key				= i.Item_Key 
														AND oh.Store_No				= @Store_No 
														AND oh.SubTeam_No			= ISNULL(@SubTeam_No, oh.SubTeam_No)
			INNER JOIN	ItemHistory		(nolock) ih		ON	i.Item_Key				= ih.Item_Key 
														AND ih.Store_No				= @Store_No 
														AND ih.SubTeam_No			= ISNULL(@SubTeam_No, ih.SubTeam_No)
			INNER JOIN	ItemAdjustment	(nolock) ia		ON	ih.Adjustment_ID		= ia.Adjustment_ID
			--LEFT JOIN	ItemCaseHistory	(nolock) ich	ON	ich.ItemHistoryID		= ih.ItemHistoryID
		WHERE 
			ih.DateStamp		>=	oh.LastReset 
			AND i.Deleted_Item	=	0 
			AND ih.DateStamp	<=	CASE 
										WHEN oh.LastReset > @EndDate THEN 
											oh.LastReset 
										ELSE 
											@EndDate 
									END
            AND EXISTS			(SELECT * FROM @Orders O INNER JOIN OrderItem OI ON OI.OrderHeader_ID = O.OrderHeader_ID WHERE OI.Item_Key = i.Item_Key)
		GROUP BY 
			ih.Item_Key, 
			i.Package_Desc1

	--**************************************************************************
    -- If doing Transfer orders, adjust the BOH by what has been allocated to Non-Transfer orders, which are done first in the business process
	--**************************************************************************
    IF @Transfers <> 0
        INSERT INTO @BOH_WOO
			SELECT 
				oi.Item_Key, 
				oi.Package_Desc1, 
				SUM(ISNULL(QuantityAllocated, QuantityOrdered)) * -1,
				0
			FROM 
				Item					(nolock) i
				INNER JOIN	OrderItem	(nolock) oi	ON	i.Item_Key			= oi.Item_Key
				INNER JOIN	OrderHeader (nolock) oh ON	oi.OrderHeader_ID	= oh.OrderHeader_ID
				INNER JOIN	Vendor		(nolock) vr	ON	vr.Vendor_ID		= oh.ReceiveLocation_ID
				INNER JOIN	Vendor		(nolock) v	ON	oh.Vendor_ID		= v.Vendor_ID 
													AND v.Store_No			= @Store_No
			WHERE 
				oh.Transfer_SubTeam			=	ISNULL(@SubTeam_No, oh.Transfer_SubTeam)
				AND oh.Transfer_To_SubTeam	=	oh.Transfer_SubTeam
				AND oh.CloseDate			IS	NULL 
				AND oh.Expected_Date		=	@Date
			GROUP BY 
				oi.Item_Key, 
				oi.Package_Desc1
        
	--**************************************************************************
    -- SQL for report output
	--**************************************************************************

    SELECT 
		[Category_Name]		=	ic.Category_Name, 
		[Identifier]		=	ii.Identifier, 
		[Item_Description]	=	i.Item_Description, 
		[PackSize]			=	bw.PackSize, 
		[OnHand]			=	ROUND(SUM(bw.OH), 4), 
        [WOO]				=	ROUND(SUM(bw.OO), 4),
        [CompanyName]		=	v.CompanyName, 
		[SubTeam_Name]		=	st.SubTeam_Name,
        [QuantityOrdered]	=	SUM(oi.QuantityOrdered),
        [QuantityAllocated]	=	SUM(oi.QuantityAllocated),
        [Tot_OnHand]		=	(SELECT SUM(bw.OH) FROM @BOH_WOO bw WHERE bw.Item_Key = i.Item_Key and bw.PackSize = t1.Package_Desc1 ),
        [Tot_WOO]			=	(SELECT SUM(bw.OO) FROM @BOH_WOO bw WHERE bw.Item_Key = i.Item_Key and bw.PackSize = t1.Package_Desc1),
        [Tot_SOO]			=	ISNULL((SELECT SUM(QuantityOrdered) FROM OrderItem INNER JOIN @Orders O ON O.OrderHeader_ID = OrderItem.OrderHeader_ID WHERE OrderItem.Item_Key = i.Item_Key AND QuantityAllocated IS NULL AND DateReceived IS NULL), 0),
        [Tot_Alloc]			=	ISNULL((SELECT SUM(QuantityAllocated) FROM OrderItem INNER JOIN @Orders O ON O.OrderHeader_ID = OrderItem.OrderHeader_ID WHERE OrderItem.Item_Key = i.Item_Key AND QuantityAllocated IS NOT NULL AND DateReceived IS NULL), 0)
    FROM 
		OrderHeader					(nolock) oh
		INNER JOIN	Vendor			(nolock) v		ON	v.Vendor_ID				= oh.ReceiveLocation_ID
		INNER JOIN	OrderItem		(nolock) oi		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
		INNER JOIN	Item			(nolock) i		ON	oi.Item_Key				= i.Item_Key
		INNER JOIN	ItemIdentifier	(nolock) ii		ON	i.Item_Key				= ii.Item_Key
													AND	ii.Default_Identifier	= 1
		RIGHT JOIN	SubTeam			(nolock) st		ON	st.SubTeam_No			= oh.Transfer_To_SubTeam
		INNER JOIN	ItemCategory	(nolock) ic		ON	ic.Category_ID			= i.Category_ID
		INNER JOIN	@Orders					 o		ON	o.OrderHeader_ID		= oh.OrderHeader_ID
		INNER JOIN	(
					SELECT 
						Item_Key, 
						Package_Desc1,
						SUM(QuantityOrdered) As TotalQuantityOrdered
					FROM 
						OrderHeader				(nolock) oh2
						INNER JOIN	OrderItem	(nolock) oi2 ON oi2.OrderHeader_ID	= oh2.OrderHeader_ID
						INNER JOIN	@Orders				 o	 ON o.OrderHeader_ID	= oh2.OrderHeader_ID
					GROUP BY 
						Item_Key,
						Package_Desc1
					)						 t1		ON t1.Item_Key				= i.Item_Key
		LEFT JOIN	(
					SELECT 
						Item_Key, 
						PackSize, 
						SUM(OH) AS OH, 
						SUM(OO) AS OO
					FROM 
						@BOH_WOO T1
					GROUP BY 
						Item_Key, 
						PackSize
					)						 bw		ON	bw.Item_Key				= oi.Item_Key 
													AND bw.PackSize				= oi.Package_Desc1 
		WHERE 
			(
				(@BOH = 0) 
			OR 
				((ISNULL((SELECT SUM(BW.OH) FROM @BOH_WOO BW WHERE BW.Item_Key = i.Item_Key), 0) - TotalQuantityOrdered > 0) AND (@BOH = 1)) 
			OR	
				((ISNULL((SELECT SUM(BW.OH) FROM @BOH_WOO BW WHERE BW.Item_Key = i.Item_Key), 0) - TotalQuantityOrdered <= 0) AND (@BOH = 2))
			OR	
				((ISNULL((SELECT SUM(BW.OH) FROM @BOH_WOO BW WHERE BW.Item_Key = i.Item_Key), 0) - TotalQuantityOrdered < 0) AND (@BOH = 3))
			OR	
				((ISNULL((SELECT SUM(BW.OH) FROM @BOH_WOO BW WHERE BW.Item_Key = i.Item_Key), 0) - TotalQuantityOrdered >= 0) AND (@BOH = 4))
			)
			AND EXISTS (
						SELECT 
							* 
						FROM 
							OrderItem OI 
							INNER JOIN Item POI ON POI.Item_Key = OI.Item_Key 
						WHERE 
							OI.OrderHeader_ID = oh.OrderHeader_ID 
							AND POI.Pre_Order = CASE @Pre_Order 
													WHEN -1 THEN 
														POI.Pre_Order 
													ELSE
														@Pre_Order 
												END
						)
			AND oh.Orderheader_ID IN (SELECT Distinct orderheader_ID from tmpordersallocateOrderitems)

    GROUP BY
		ic.Category_Name, 
		i.Item_Key, 
		ii.Identifier, 
		i.Item_Description, 
		bw.PackSize,
		v.CompanyName, 
		st.SubTeam_Name,	
		t1.Package_Desc1

    UNION

    SELECT 
		[Category_Name]		=	ic.Category_Name, 
		[Identifier]		=	ii.Identifier, 
		[Item_Description]	=	i.Item_Description, 
		[PackSize]			=	bw.PackSize, 
        [OnHand]			=	ROUND(SUM(BW.OH), 4), 
        [WOO]				=	ROUND(SUM(BW.OO), 4),
        [CompanyName]		=	NULL, 
		[SubTeam_Name]		=	NULL,
        [QuantityOrdered]	=	NULL,
        [QuantityAllocated]	=	NULL,
        [Tot_OnHand]		=	(SELECT SUM(BW.OH) FROM @BOH_WOO BW WHERE BW.Item_Key = i.Item_Key AND BW.PackSize = tmp.PackSize),
        [Tot_WOO]			=	(SELECT SUM(BW.OO) FROM @BOH_WOO BW WHERE BW.Item_Key = i.Item_Key AND BW.PackSize = tmp.PackSize),
		[Tot_SOO]			=   ISNULL((SELECT SUM(QuantityOrdered) FROM OrderItem INNER JOIN @Orders O ON O.OrderHeader_ID = OrderItem.OrderHeader_ID WHERE OrderItem.Item_Key = i.Item_Key AND QuantityAllocated IS NULL AND DateReceived IS NULL), 0),
		[Tot_Alloc]			=   ISNULL((SELECT SUM(QuantityAllocated) FROM OrderItem INNER JOIN @Orders O ON O.OrderHeader_ID = OrderItem.OrderHeader_ID WHERE OrderItem.Item_Key = i.Item_Key AND QuantityAllocated IS NOT NULL AND DateReceived IS NULL), 0)
    FROM 
		(
		SELECT 
			Item_Key,
			PackSize, 
			SUM(OH) AS OH, 
			SUM(OO) AS OO
		FROM 
			@BOH_WOO T1
		GROUP BY 
			Item_Key, 
			PackSize
         ) BW
		INNER JOIN	Item			(nolock) i	ON	i.Item_Key				= bw.Item_Key
		INNER JOIN  ItemIdentifier	(nolock) ii	ON	ii.Item_Key				= i.Item_Key 
												AND ii.Default_Identifier	= 1
		LEFT JOIN	ItemCategory	(nolock) ic	ON	ic.Category_ID			= i.Category_ID
		INNER JOIN (SELECT Distinct Item_Key, PackSize FROM tmpordersallocateitems) tmp
												ON  tmp.Item_Key = i.Item_Key
												AND tmp.PackSize = bw.PackSize
	WHERE 
		bw.PackSize NOT IN (SELECT Package_Desc1 FROM OrderItem INNER JOIN @Orders O ON O.OrderHeader_ID = OrderItem.OrderHeader_ID WHERE OrderItem.Item_Key = bw.Item_Key)
    AND EXISTS			(SELECT * FROM @Orders O INNER JOIN OrderItem OI ON OI.OrderHeader_ID = O.OrderHeader_ID WHERE OI.Item_Key = i.Item_Key)
    GROUP BY 
		ic.Category_Name, 
		i.Item_Key, 
		ii.Identifier, 
		i.Item_Description, 
		bw.PackSize,
		tmp.PackSize

END