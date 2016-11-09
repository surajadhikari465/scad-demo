CREATE PROCEDURE [dbo].[DoFSAAutoAllocate]
	@WarehouseNo int,
	@SubTeam_No int,
	@UserName varchar(35),
	@PreOrder int,
	@PerformCasePackMoves bit
AS

/***************************************************************************
Procedure:		DoFSAAutoAllocate()
Author:			Robert Shurbet
Date:			07.01.09
IRMA Version:	3.4.6
Description:    Auto-allocate available BOH to facility orders.
				The logic here was moved out of the OrdersAllocate.vb form to
				simplify and improve performance.

Modification History:

Date		Init		Comment
07.01.09	shurbetr	Creation
11.27.09	shurbetr	Add Case Pack Move logic to allocated by Pack FIFO
03.25.09	shurbetr	Extensive revisions to casepack move and allocation logic

**************************************************************************/

BEGIN TRY

	DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int, @ErrLine int
	DECLARE @Item_Key int
	DECLARE @OrderHeader_ID int
	DECLARE @Package_Desc1 decimal(18,4)
	
	DECLARE @QuantityOrdered decimal(18,4)
	DECLARE @QuantityAllocated decimal(18,4)
	DECLARE @OrigPackage_Desc1 decimal(18,4)
	DECLARE @OrigQuantityAllocated  decimal(18,4)
	DECLARE @AllocationPercentage decimal(18,4)	 
	
	DECLARE @AvailableBOH decimal(18,4)

	DECLARE @AvailablePack decimal(18,4)
	
	DECLARE @QtyAlloc decimal(18,4)
	DECLARE @AdjPackChgQty decimal(18,4)
	
	DECLARE @BOH int
	DECLARE @SOO int
	DECLARE @TotalAllocated int 
	DECLARE @AllocDiff int 

	SET @TotalAllocated = 0 
	SET @AllocDiff = 0 
	
	
	
	--/////////////////////////////////   DO MULTIPLE PACK SIZES FIRST   //////////////////////////////////////////
	
	DECLARE @OrderItems TABLE	(
										Item_Key int,
										OrderHeader_ID int,
										Package_Desc1 decimal(9,4),
										QuantityOrdered decimal(18,4),
										QuantityAllocated decimal(18,4),
										OrigQuantityAllocated decimal(18,4),
										OrigPackage_Desc1 decimal (18,4),
										AllocationPercentage decimal (18,4)
									)

	DECLARE @FIFO TABLE	(
							Item_Key int,
							PackSize int,
							BOH decimal(9,4),
							FIFODateTime datetime
						)

	INSERT @OrderItems								
		SELECT
			Item_Key,	
			OrderHeader_ID,
			Package_Desc1,
			QuantityOrdered,
			QuantityAllocated,
			OrigQuantityAllocated,
			OrigPackage_Desc1,
			0
		FROM
			tmpOrdersAllocateOrderItems (nolock)
		WHERE
			Item_Key IN(SELECT
							Item_Key 
						FROM 
							tmpOrdersAllocateItems (NOLOCK)
						WHERE
							Store_No	=	@WarehouseNo
						AND SubTeam_No	=	@SubTeam_No
						AND UserName	=	@UserName
						AND Pre_Order	=	(CASE WHEN @PreOrder =	-1 THEN Pre_Order ELSE @PreOrder END)
						GROUP BY
							Item_Key
						HAVING 
							COUNT(PackSize) > 1)
		ORDER BY Item_Key, Package_Desc1, QuantityOrdered
		
	INSERT @FIFO
		SELECT DISTINCT
			Item_Key,
			PackSize,
			SUM(BOH),
			FIFODateTime
		FROM 
			tmpOrdersAllocateItems (NOLOCK)
		WHERE
			Item_Key IN(SELECT
							Item_Key 
						FROM 
							@OrderItems)
		GROUP BY
			Item_Key,
			PackSize,
			FIFODateTime
		ORDER BY
			Item_Key,
			FIFODateTime
				
	-- get a list of all the item keys we're working on
	DECLARE WorkItems CURSOR FOR
		SELECT
			Item_Key,	
			OrderHeader_ID,
			Package_Desc1,
			QuantityOrdered,
			QuantityAllocated,
			OrigQuantityAllocated,
			OrigPackage_Desc1,
			0
		FROM
			@OrderItems
			
	OPEN WorkItems
	
	FETCH NEXT FROM WorkItems
	INTO @Item_Key, @OrderHeader_ID, @Package_Desc1, @QuantityOrdered, @QuantityAllocated, @OrigQuantityAllocated, @OrigPackage_Desc1, @AllocationPercentage
	
	WHILE @@FETCH_STATUS = 0
	
		BEGIN
		
			SET @QtyAlloc = ISNULL(@QuantityAllocated, @QuantityOrdered)
					
			-- TRY TO FIND A PACK SIZE THAT CAN FULFILL THE WHOLE ORDER FIRST
			IF EXISTS(SELECT TOP 1
							BOH
						FROM
							@FIFO
						WHERE
							Item_Key	= @Item_Key
						AND BOH			>= ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0)
						ORDER BY
							FIFODateTime)
				BEGIN
				
					SELECT TOP 1
						@AvailablePack	= PackSize,
						@AdjPackChgQty	= ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0)	
					FROM
						@FIFO
					WHERE
						Item_Key		= @Item_Key
					AND BOH				>= ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0)
					ORDER BY
						FIFODateTime					
				
					UPDATE
						tmpOrdersAllocateOrderItems
					SET
						QuantityAllocated	=	@AdjPackChgQty,
						Package_Desc1		=	@AvailablePack
					WHERE
						Item_Key			=	@Item_Key
					AND OrderHeader_ID		=	@OrderHeader_ID
										
					UPDATE @FIFO SET BOH = BOH - @AdjPackChgQty WHERE Item_Key = @Item_Key AND PackSize = @AvailablePack
					
				END
								
			-- IF WE CAN'T FULFILL THE WHOLE ORDER WITH ONE PACK, HOW ABOUT PART OF THE ORDER? 	
			ELSE IF EXISTS (SELECT TOP 1
								BOH
							FROM
								@FIFO
							WHERE
								Item_Key	= @Item_Key
							AND BOH			>= ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0) - (ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0) - BOH)
							ORDER BY
								FIFODateTime)
				BEGIN
				
					SELECT TOP 1
						@AvailablePack	= PackSize,
						@AdjPackChgQty	= ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0) - (ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0) - BOH),
						@AllocDiff		= ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0) - BOH	
					FROM
						@FIFO
					WHERE
						Item_Key		= @Item_Key
					AND BOH				>= ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0) - (ROUND((@Package_Desc1 * @QtyAlloc) / PackSize, 0) - BOH)
					ORDER BY
						FIFODateTime
																						
					UPDATE
						tmpOrdersAllocateOrderItems
					SET
						QuantityAllocated	=	@AdjPackChgQty,
						Package_Desc1		=	@AvailablePack
					WHERE
						Item_Key			=	@Item_Key
					AND OrderHeader_ID		=	@OrderHeader_ID
										
					UPDATE @FIFO SET BOH = BOH - @AdjPackChgQty WHERE Item_Key = @Item_Key AND PackSize = @AvailablePack
					
				END
			
			ELSE
			
				BEGIN
					-- WE GOT NOTHING
					UPDATE
						tmpOrdersAllocateOrderItems
					SET
						QuantityAllocated	=	0
					WHERE
						Item_Key			=	@Item_Key
					AND OrderHeader_ID		=	@OrderHeader_ID
				END
			
			-- ON TO THE NEXT ITEM
			FETCH NEXT FROM WorkItems
			INTO @Item_Key, @OrderHeader_ID, @Package_Desc1, @QuantityOrdered, @QuantityAllocated, @OrigQuantityAllocated, @OrigPackage_Desc1, @AllocationPercentage
																					
		END
		
	CLOSE WorkItems
	DEALLOCATE WorkItems
	
	--///////////////////////////////////////////////////////////////////////////
	

	----/////////////////////////////////   NOW DO THE ORIGINAL LOGIC FOR SINGLE-PACK ITEMS   //////////////////////////////////////////
	
	-- get a list of all the item keys we're working on
	DECLARE AllocationItems CURSOR FOR
		SELECT DISTINCT
			Item_Key
		FROM 
			tmpOrdersAllocateItems (NOLOCK)
		WHERE
			Store_No		=	@WarehouseNo
			AND SubTeam_No	=	@SubTeam_No
			AND UserName	=	@UserName
			AND Pre_Order	=	(CASE WHEN @PreOrder =	-1 THEN Pre_Order ELSE @PreOrder END)
		GROUP BY
			Item_Key
		HAVING 
			COUNT(PackSize) = 1

	OPEN AllocationItems
	
	FETCH NEXT FROM AllocationItems
	INTO @Item_Key
	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		 -- clear the temp table
		DELETE FROM @OrderItems
		
		-- get the total BOH we can allocate
		SELECT @BOH = SUM(BOH) FROM tmpOrdersAllocateItems (nolock) WHERE Item_Key = @Item_Key 
		
		-- get the total Store On Order amount
		SELECT @SOO = SUM(QuantityOrdered) FROM tmpOrdersAllocateOrderItems (nolock) WHERE Item_Key = @Item_Key
		
		-- if we don't have any inventory, set allocation to 0 and move to the next item
		IF @BOH <= 0
		BEGIN
			UPDATE tmpOrdersAllocateOrderItems SET QuantityAllocated = 0 WHERE Item_Key = @Item_Key
			FETCH NEXT FROM AllocationItems
			INTO @Item_Key
			CONTINUE 
		END
			
		-- if the BOH is >= SOO, everyone gets what they ordered so set the allocation to the ordered qty and move to the next item
		IF @BOH >= @SOO
		BEGIN
			UPDATE tmpOrdersAllocateOrderItems SET QuantityAllocated = QuantityOrdered WHERE Item_Key = @Item_Key
			FETCH NEXT FROM AllocationItems
			INTO @Item_Key
			CONTINUE
		END

		-- now we have to allocate according to percentage of BOH ordered
		INSERT @OrderItems
			SELECT	
				Item_Key,
				OrderHeader_ID,
				Package_Desc1,
				QuantityOrdered,
				QuantityAllocated,
				OrigQuantityAllocated,
				OrigPackage_Desc1,
				0 -- allocation percentage
			FROM tmpOrdersAllocateOrderItems (nolock)
			WHERE Item_Key = @Item_Key
			ORDER BY QuantityOrdered Desc
				
		-- now we have to loop through and assign 1 case out to every order as long as the total alloc <= boh
		DECLARE PartialAllocationItems CURSOR FOR
				SELECT OrderHeader_ID, QuantityOrdered from @OrderItems WHERE Item_Key = @Item_Key ORDER BY QuantityOrdered Desc
				
		-- get the total allocation quantity across all orders
		SELECT @TotalAllocated = ISNULL(SUM(QuantityAllocated),0) FROM @OrderItems WHERE Item_Key = @Item_Key

		OPEN PartialAllocationItems

		FETCH NEXT FROM PartialAllocationItems
		INTO @OrderHeader_ID, @QuantityOrdered

		WHILE @@FETCH_STATUS = 0
		BEGIN
							
			IF @QuantityOrdered > 0 -- they ordered the item
			BEGIN
				IF @TotalAllocated <= @BOH -- the total qty alloc <=BOH
					-- give them 1
					UPDATE @OrderItems SET QuantityAllocated = ISNULL(QuantityAllocated, 0) + 1 WHERE OrderHeader_ID = @OrderHeader_ID and Item_Key = @Item_Key
			END

			-- get the total allocation quantity across all orders again
			SELECT @TotalAllocated = ISNULL(SUM(QuantityAllocated),0) FROM @OrderItems WHERE Item_Key = @Item_Key
		
			FETCH NEXT FROM PartialAllocationItems
			INTO @OrderHeader_ID, @QuantityOrdered
			
		END
		
		CLOSE PartialAllocationItems
		DEALLOCATE PartialAllocationItems
		
		-- get the total allocation quantity across all orders again
		SELECT @TotalAllocated = SUM(QuantityAllocated) FROM @OrderItems WHERE Item_Key = @Item_Key
		
		IF @TotalAllocated < @BOH -- the total qty alloc < BOH
			-- calculate a percentage allocation for each order
			IF (@SOO - @TotalAllocated) > 0 -- avoid divide by zero!!
			BEGIN
				UPDATE @OrderItems SET AllocationPercentage = (QuantityOrdered - ISNULL(QuantityAllocated,0)) / (@SOO - @TotalAllocated) WHERE Item_Key = @Item_Key
				-- do the allocation
				UPDATE @OrderItems SET QuantityAllocated = ISNULL(QuantityAllocated,0) + ROUND(AllocationPercentage * (@BOH - @TotalAllocated),0) WHERE Item_Key = @Item_Key
			END

		-- get the total allocation quantity across all orders again
		SELECT @TotalAllocated = ISNULL(SUM(QuantityAllocated),0) FROM @OrderItems WHERE Item_Key = @Item_Key
		
		-- now compare the allocated sum to the BOH.
		IF @TotalAllocated > @BOH
		BEGIN
			SET @AllocDiff = @TotalAllocated - @BOH
			-- we over allocated, let's shave the difference off the one that ordered the most
			UPDATE @OrderItems SET QuantityAllocated = ISNULL(QuantityAllocated,0) - @AllocDiff WHERE Item_Key = @Item_Key AND Orderheader_ID = (SELECT TOP 1 OrderHeader_ID FROM @OrderItems WHERE Item_Key = @Item_Key)
		END

		-- now update the temp table with the allocations
		UPDATE tmpOrdersAllocateOrderItems
		SET QuantityAllocated = (
									SELECT QuantityAllocated
									FROM @OrderItems 
									WHERE OrderHeader_ID = tmpOrdersAllocateOrderItems.OrderHeader_ID
									AND Item_Key = tmpOrdersAllocateOrderItems.Item_Key
								)
		WHERE Item_Key = @Item_Key
		
		-- on to the next item
		FETCH NEXT FROM AllocationItems
		INTO @Item_Key
		
	END
		
	CLOSE AllocationItems
	DEALLOCATE AllocationItems
	
END TRY

BEGIN CATCH

	CLOSE WorkItems;
	DEALLOCATE WorkItems;
	
	CLOSE AllocationItems;
	DEALLOCATE AllocationItems;
	
	SELECT	@ErrMsg = ERROR_MESSAGE() + ' Error on Line:' + CAST(ERROR_LINE() as varchar(10)),
			@ErrSeverity = ERROR_SEVERITY(),
			@ErrLine = ERROR_LINE() 
	RAISERROR(@ErrMsg , 18, 1)
	
END CATCH;
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DoFSAAutoAllocate] TO [IRMAClientRole]
    AS [dbo];

