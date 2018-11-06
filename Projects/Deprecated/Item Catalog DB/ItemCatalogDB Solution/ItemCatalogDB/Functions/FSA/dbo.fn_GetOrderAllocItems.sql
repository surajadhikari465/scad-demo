

/****** Object:  UserDefinedFunction [dbo].[fn_GetOrderAllocItems]    Script Date: 10/04/2012 16:26:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetOrderAllocItems]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetOrderAllocItems]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_GetOrderAllocItems]    Script Date: 10/04/2012 16:26:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_GetOrderAllocItems] 
(
    @Store_No			int,
    @SubTeam_No			int,
	@UserName			varchar(35),
    @NonRetail			int,
    @AdjustBOH			bit,
	@IncludeInboundQty	bit,
	@ExpectedDateStart	datetime,
	@ExpectedDateEnd	datetime,
	@PreOrderOption		int
)

RETURNS @Table TABLE 
(
	Store_No			int,
	SubTeam_No			int,
	UserName			varchar(35),
	Item_Key			int, 
	Identifier			varchar(13), 
	Item_Description	varchar(60), 
	Category_Name		varchar(35), 
	Pre_Order			tinyint,
    PackSize			decimal,
    BOH					decimal,
    WOO					decimal,
    SOO					decimal,
    Alloc				decimal,
    EOH					decimal,
    FiFoDate			datetime
 )
AS

-- **************************************************************************
-- Procedure: fn_GetOrderAllocItems
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/16	KM		3744	Added update history template; code formatting; moved grants
--								to SecurityGrants.sql
-- 10/03/2012	AlexB		Removed all references to ItemCaseHistory
-- **************************************************************************

BEGIN

    -- Get list of Orders first for simplicity to avoid repeating the conditions below
    DECLARE
		@Orders TABLE (OrderHeader_ID int PRIMARY KEY)

    INSERT INTO 
		@Orders
	SELECT
		OrderHeader_ID
	FROM
		(SELECT
			OrderHeader_ID,
			Retail				=	CASE 
										WHEN Transfer_SubTeam = Transfer_To_SubTeam THEN 0 
										ELSE 1 
									END
		FROM 
			OrderHeader			(nolock) oh
			INNER JOIN Vendor	(nolock) v	ON oh.Vendor_ID		= v.Vendor_ID 
											AND @Store_No		= v.Store_No
		WHERE 
			Transfer_SubTeam = @SubTeam_No
			AND CloseDate IS NULL 
			AND DATEDIFF(day, GETDATE(), Expected_Date) = 1 
			AND Sent = 1 AND WarehouseSent = 0
			AND NOT EXISTS (SELECT * 
							FROM
								OrderItem	(nolock) oi
							WHERE
								oi.OrderHeader_ID = oh.OrderHeader_ID
								AND DateReceived IS NOT NULL)
		) AS Orders
	WHERE Retail =	(CASE
						WHEN @NonRetail = -1 THEN Retail 
						ELSE @NonRetail
					END)

    -- Need to match up the BOH with the WOO by Pack Size
    DECLARE
    @BOH_WOO TABLE (Item_Key	int,
					PackSize	decimal(9,4),
					OH			decimal(18,4),
					OO			decimal(18,4))

	BEGIN
		IF @IncludeInboundQty = 1
			
			-- First get the WOO
			INSERT INTO
				@BOH_WOO
			SELECT
				Item_Key,
				Package_Desc1,
				0,
				SUM(QuantityOrdered)
			FROM
				OrderHeader				(nolock) oh
				INNER JOIN Vendor		(nolock) rl	ON	oh.ReceiveLocation_ID	=	rl.Vendor_ID
													AND @Store_No				=	rl.Store_No
				INNER JOIN OrderItem	(nolock) oi ON	oh.OrderHeader_ID		=	oi.OrderHeader_ID
													AND oh.CloseDate			IS NULL
													AND oh.Sent					=	1
													AND oh.Expected_Date		>=	@ExpectedDateStart 
													AND oh.Expected_Date		<=	@ExpectedDateEnd
													AND oi.DateReceived			IS NULL
													AND EXISTS (SELECT * 
																FROM
																	@Orders O 
																	INNER JOIN OrderItem (nolock) ON O.OrderHeader_ID = OrderItem.OrderHeader_ID
																WHERE
																	OrderItem.Item_Key = oi.Item_Key)
			GROUP BY
				Item_Key,
				Package_Desc1
	END

    -- Get the Item Unit ID's so we can call CostConverion
    DECLARE
		@Case	int,
		@Pound	int
    
    SELECT
		@Case = Unit_ID
	FROM 
		ItemUnit (nolock)
	WHERE
		Unit_Name = 'Case'
    
    SELECT
		@Pound = Unit_ID
	FROM 
		ItemUnit (nolock)
	WHERE 
		Unit_Name = 'Pound'

    -- Next get the BOH for the items on the selected Orders
    INSERT INTO
		@BOH_WOO
    SELECT
		ih.Item_Key,
		 Package_Desc1,
		OnHand = SUM(	CASE 
												WHEN ISNULL(ih.Quantity, 0) > 0 THEN ih.Quantity /	CASE
																															WHEN Package_Desc1 <> 0 THEN Package_Desc1 
																															ELSE 1
																														END
												ELSE ISNULL(ih.Weight, 0) /	CASE
																							WHEN Package_Desc1 * Package_Desc2 <> 0 THEN (Package_Desc1 * Package_Desc2)
																							ELSE 1 
																						END
											END * ia.Adjustment_Type),
		0
    FROM 
		OnHand						(nolock) oa
		INNER JOIN ItemHistory		(nolock) ih		ON	oa.Item_Key				= ih.Item_Key 
													AND oa.Store_No				= ih.Store_No
													AND oa.SubTeam_No			= ih.SubTeam_No
		INNER JOIN ItemAdjustment	(nolock) ia     ON	ih.Adjustment_ID		= ia.Adjustment_ID
		INNER JOIN Item				(nolock) i		ON	oa.Item_Key				= i.Item_Key
		INNER JOIN ItemIdentifier	(nolock) ii     ON  i.Item_Key				= ii.Item_Key
													AND ii.Default_Identifier	= 1		
    WHERE
		oa.Store_No =	@Store_No
					AND oa.SubTeam_No = @SubTeam_No
					AND ih.DateStamp >= ISNULL(oa.LastReset, ih.DateStamp)
					AND Deleted_Item = 0
					AND EXISTS (SELECT * 
								FROM
									@Orders O 
									INNER JOIN	OrderItem	(nolock) oi	ON	O.OrderHeader_ID = oi.OrderHeader_ID
								WHERE
									oi.Item_Key = i.Item_Key)
    GROUP BY
		ih.Item_Key,
		 Package_Desc1
    HAVING
		SUM(CASE
										WHEN ISNULL(ih.Quantity, 0) > 0 THEN ih.Quantity /	CASE 
																								WHEN Package_Desc1 <> 0 THEN Package_Desc1 
																								ELSE 1
																							END
										ELSE ISNULL(ih.Weight, 0) /	CASE
																		WHEN Package_Desc1 * Package_Desc2 <> 0 THEN (Package_Desc1 * Package_Desc2) 
																		ELSE 1
																	END
									END * ia.Adjustment_Type) <> 0

	-- TFS#9856 - this is altered based on current SO functionality as requested by the MA and FL regions.
	-- Now we want the BOH qty to be reduced by the qty already allocated regardless of subteam type when specified
	
	-- TFS#12234 - Always reduce the BOH by any qty allocated and already sent to the warehouse for the
	-- current date.
	
    INSERT INTO
		@BOH_WOO
    SELECT
		oi.Item_Key,
		oi.Package_Desc1,
		SUM(ISNULL(QuantityAllocated, 0)) * -1, 
        0
    FROM
		OrderHeader				(nolock) oh
		INNER JOIN Vendor		(nolock) v		ON	oh.Vendor_ID			= v.Vendor_ID
												AND @Store_No				= v.Store_No
		INNER JOIN Vendor		(nolock) rv		ON	oh.ReceiveLocation_ID	= rv.Vendor_ID 
		INNER JOIN OrderItem	(nolock) oi		ON  oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN Item			(nolock) i		ON	oi.Item_Key				= i.Item_Key
    WHERE 
		(Transfer_SubTeam = @SubTeam_No 
			AND CloseDate IS NULL AND DATEDIFF(DAY, GETDATE(), Expected_Date) = 1)
			AND EXISTS (SELECT * 
						FROM
							@BOH_WOO
						WHERE
							Item_Key = oi.Item_Key)
			AND warehousesent = 1
    GROUP BY
		oi.Item_Key, 
		oi.Package_Desc1

    -- Finally put it all together and get the result set
	INSERT INTO
		@Table
		(
			Store_No,
			SubTeam_No,
			UserName,
			Item_Key,
			Identifier,
			Item_Description, 
			Category_Name, 
			Pre_Order, 
			PackSize,
			BOH,
			WOO,
			SOO,
			FiFoDate
		)
    SELECT
		@Store_No, 
		@SubTeam_No, 
		@UserName,
		oi.Item_Key, 
		Identifier, 
		Item_Description, 
		Category_Name, 
		Pre_Order			= CONVERT(tinyint, Pre_Order),
		PackSize			= ISNULL(BW.PackSize, oi.Package_Desc1), 
        BOH					= ROUND((SELECT
										ISNULL(SUM(BW2.OH), 0) 
									FROM 
										@BOH_WOO BW2
									WHERE
										Item_Key			= oi.Item_Key 
										AND BW2.PackSize	= BW.PackSize), 4), 
		WOO					= ROUND((SELECT
										ISNULL(SUM(BW2.OO), 0)
									FROM
										@BOH_WOO BW2
									WHERE
										Item_Key			= oi.Item_Key
										AND BW2.PackSize	= BW.PackSize), 4),
        SOO					= ROUND(SUM(QuantityOrdered), 4),
		FIFO.FiFoDate
    FROM
		
    OrderItem					(nolock) oi
    INNER JOIN OrderHeader		(nolock) oh		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
    INNER JOIN Item				(nolock) i		ON	oi.Item_Key				= i.Item_Key
												AND i.Pre_Order				=	CASE
																					WHEN @PreOrderOption = -1 THEN i.Pre_Order
																					ELSE @PreOrderOption
																				END
	INNER JOIN ItemIdentifier	(nolock) ii		ON	i.Item_Key				= ii.Item_Key
												AND ii.Default_Identifier	= 1
	INNER JOIN ItemCategory		(nolock) ic		ON	i.Category_ID			= ic.Category_ID
    INNER JOIN @Orders					 O		ON	oh.OrderHeader_ID		= O.OrderHeader_ID
    LEFT  JOIN (SELECT
					Item_Key, 
					PackSize,
					OH = SUM(OH),
					OO = SUM(OO)
				FROM
					@BOH_WOO T1
				GROUP BY
					Item_Key,
					PackSize
				) BW							ON  oi.Item_Key				= BW.Item_Key
												AND oi.Package_Desc1		= BW.PackSize
    LEFT  JOIN (SELECT
					oi.Item_Key,	
					FifoDate = MAX(OI.DateReceived),
					PackSize = OI.Package_Desc1
				FROM
					OrderItem				(nolock) oi
					INNER JOIN OrderHeader	(nolock) oh	ON	oi.OrderHeader_ID = oh.OrderHeader_ID
				WHERE
					oh.ReceiveLocation_ID		=	(SELECT
														Vendor_ID
													FROM
														Vendor
													WHERE
														Store_No = @Store_No)
					AND oh.Transfer_To_Subteam	=	@SubTeam_No
				GROUP BY
					oi.Item_Key,
					oi.Package_Desc1
				) AS FIFO						ON	 BW.Item_Key = FIFO.Item_Key
												AND	 BW.PackSize = FIFO.PackSize   
    GROUP BY
		oi.Item_Key,
		Identifier,
		Item_Description, 
		Category_Name, 
		Pre_Order, 
		BW.PackSize, 
		oi.Package_Desc1, 
		FIFO.FiFoDate
    
    UNION
    
    SELECT
		@Store_No,
		@SubTeam_No, 
		@UserName, 
		BW.Item_Key, 
		Identifier, 
		Item_Description, 
		Category_Name, 
		Pre_Order		= CONVERT(tinyint, Pre_Order),
		BW.PackSize, 
		BOH				= OH,
		WOO				= OO,
		SOO				= 0,
		FIFO.FiFoDate
    FROM 
        (SELECT
			Item_Key, 
			PackSize, 
			OH = SUM(OH),
			OO = SUM(OO) 
         FROM
			@BOH_WOO T1
         GROUP BY
			Item_Key, 
			PackSize
        ) BW
    
		LEFT JOIN	(SELECT
						Item_Key, 
						Package_Desc1
					FROM
						OrderItem			(nolock)	oi
						INNER JOIN @Orders				O	ON oi.OrderHeader_ID = O.OrderHeader_ID
					) oi	ON	BW.Item_Key = oi.Item_Key
							AND BW.PackSize = oi.Package_Desc1 
		LEFT JOIN	(SELECT
						oi.Item_Key,	
						FifoDate = MAX(oi.DateReceived),
						PackSize = oi.Package_Desc1
					FROM
						OrderItem					(nolock)	oi
						INNER JOIN OrderHeader		(nolock)	oh	ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
						INNER JOIN OnHand			(nolock)	oa	ON	oi.Item_Key				= oa.Item_Key
																	AND @Store_No				= oa.Store_No
																	AND oh.Transfer_To_Subteam	= oa.SubTeam_No
					WHERE
						oh.ReceiveLocation_ID =	(SELECT
													Vendor_ID 
												FROM 
													Vendor 
												WHERE 
													Store_No = @Store_No)
						AND oh.Transfer_To_Subteam						=	@Subteam_No
						AND (DateReceived								>=	DATEADD(day, -180, GETDATE())
						AND UnitsReceived								>	0
						AND (ReceivedItemCost + ReceivedItemFreight)	>	0)
						AND (oa.Quantity + oa.Weight)					>	0
						AND Return_Order								=	0
					GROUP BY
						oi.Item_Key,
						oi.Package_Desc1
					) AS FIFO	ON	BW.Item_Key		= FIFO.Item_Key
								AND BW.PackSize	= FIFO.PackSize
		
		INNER JOIN Item				(nolock) i	ON	BW.Item_Key = i.Item_Key
												AND i.Pre_Order =	CASE
																		WHEN @PreOrderOption = -1 THEN i.Pre_Order
																		ELSE @PreOrderOption
																	END
		INNER JOIN ItemIdentifier	(nolock) ii	ON	BW.Item_Key = ii.Item_Key
												AND ii.Default_Identifier = 1
		LEFT  JOIN ItemCategory		(nolock) ic	ON i.Category_ID = ic.Category_ID
		
    WHERE
		oi.Item_Key IS NULL 
		AND EXISTS (SELECT * FROM @Orders)

	RETURN
END
GO


