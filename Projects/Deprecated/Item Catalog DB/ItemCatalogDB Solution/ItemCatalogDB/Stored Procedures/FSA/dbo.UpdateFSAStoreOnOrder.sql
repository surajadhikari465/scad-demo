SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateFSAStoreOnOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateFSAStoreOnOrder]
GO

CREATE PROCEDURE dbo.UpdateFSAStoreOnOrder
	@WarehouseNo int,
	@SubTeam_No int,
	@UserName varchar(35),
	@PreOrder int
AS

BEGIN TRY
	SET NOCOUNT ON

	DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int, @ErrLine int
	DECLARE @OrderItem_ID int
	DECLARE @Item_Key int
	DECLARE	@QuantityOrdered decimal(18,4)
	DECLARE	@QuantityAllocated decimal(18,4)
	DECLARE	@OrigQuantityAllocated decimal(18,4)
	DECLARE	@Package_Desc1 decimal(9,4)
	DECLARE	@OrigPackage_Desc1 decimal (18,4)

	-- Gather up all the items we're working on in this session so we can save the allocations in the temp table to the OrderItem table
	DECLARE AllocationItems CURSOR FOR
		SELECT 
			OrderItem_ID,
			Item_Key,
			QuantityOrdered,
			QuantityAllocated,
			OrigQuantityAllocated,
			ISNULL(Package_Desc1, -1)		AS Package_Desc1,
			ISNULL(OrigPackage_Desc1, -1)	AS OrigPackage_Desc1
		FROM
			tmpOrdersAllocateOrderItems
		WHERE
			Item_Key IN(
							SELECT DISTINCT
								Item_Key
							FROM
								tmpOrdersAllocateItems
							WHERE
								Store_No	=	@WarehouseNo
							AND SubTeam_No	=	@SubTeam_No
							AND UserName	=	@UserName
							AND Pre_Order	=	(CASE WHEN @PreOrder =	-1 THEN Pre_Order ELSE @PreOrder END)
						)
		AND (
				ISNULL(QuantityAllocated, -1)	 <> ISNULL(OrigQuantityAllocated, -1)
				OR ISNULL(Package_Desc1, -1)     <> ISNULL(OrigPackage_Desc1, -1)
			)

	OPEN AllocationItems

	FETCH NEXT FROM AllocationItems
	INTO @OrderItem_ID, @Item_Key, @QuantityOrdered, @QuantityAllocated, @OrigQuantityAllocated, @Package_Desc1, @OrigPackage_Desc1

	WHILE @@FETCH_STATUS = 0
	BEGIN

		DECLARE @NewPackQty int
		
		-- if the pack size changed during allocation, recalculate the new quantity for the new packsize
		IF @Package_Desc1 <> @OrigPackage_Desc1
			BEGIN
				--The quantity ordered must change since the pack size is changing
				--(Round if the new pack size does not evenly divide into the original quantity ordered)
				SET @NewPackQty = ROUND((@OrigPackage_Desc1 * @QuantityOrdered) / @Package_Desc1, 0)

				IF @NewPackQty < 1
					BEGIN			
						SET @NewPackQty = 1
					END

				-- save the allocation to the OrderItem table
				EXEC UpdateOrderItemAlloc @OrderItem_ID, @NewPackQty, @QuantityAllocated, @Package_Desc1

				--Increate the new pack size SOO amount with the new pack qty
				UPDATE 
					tmpOrdersAllocateItems
				SET
					SOO = SOO + @NewPackQty
				WHERE
					Item_Key = @Item_Key
				AND	PackSize = @Package_Desc1
				
				-- reduce the old pack size SOO amount buy the Qty Ordered
				UPDATE
					tmpOrdersAllocateItems
				SET
					SOO = SOO - @QuantityOrdered
				WHERE
					Item_Key = @Item_Key
				AND PackSize = @OrigPackage_Desc1
				
				-- update the qty ordered on this order item temp record with the new pack qty
				UPDATE
					tmpOrdersAllocateOrderItems
				SET
					QuantityOrdered = @NewPackQty
				WHERE
					OrderItem_ID = @OrderItem_ID
			END
		ELSE
		-- otherwise just save the allocation, no recalculation needed
			BEGIN
				EXEC UpdateOrderItemAlloc @OrderItem_ID, @QuantityOrdered, @QuantityAllocated, @Package_Desc1
			END

			-- on to the next item
		FETCH NEXT FROM AllocationItems
		INTO @OrderItem_ID, @Item_Key, @QuantityOrdered, @QuantityAllocated, @OrigQuantityAllocated, @Package_Desc1, @OrigPackage_Desc1

	END

	CLOSE AllocationItems
	DEALLOCATE AllocationItems
	
	-- now update the original alloc qty and pack in the temp table with the allocations
	UPDATE
		tmpOrdersAllocateOrderItems 
	SET 
		OrigQuantityAllocated = QuantityAllocated,
		OrigPackage_Desc1 = Package_Desc1
	WHERE
		Item_Key IN(
						SELECT DISTINCT
							Item_Key
						FROM
							tmpOrdersAllocateItems
						WHERE
							Store_No	=	@WarehouseNo
						AND SubTeam_No	=	@SubTeam_No
						AND UserName	=	@UserName
						AND Pre_Order	=	(CASE WHEN @PreOrder =	-1 THEN Pre_Order ELSE @PreOrder END)
					)
	AND (
			ISNULL(QuantityAllocated, -1)	 <> ISNULL(OrigQuantityAllocated, -1)
			OR ISNULL(Package_Desc1, -1)     <> ISNULL(OrigPackage_Desc1, -1)
		)

    SET NOCOUNT OFF
END TRY

BEGIN CATCH

	CLOSE AllocationItems;
	DEALLOCATE AllocationItems;
	
	SELECT	@ErrMsg = ERROR_MESSAGE() + ' Error on Line:' + CAST(ERROR_LINE() as varchar(10)),
			@ErrSeverity = ERROR_SEVERITY(),
			@ErrLine = ERROR_LINE() 
	RAISERROR(@ErrMsg , 18, 1)
	
END CATCH;

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
