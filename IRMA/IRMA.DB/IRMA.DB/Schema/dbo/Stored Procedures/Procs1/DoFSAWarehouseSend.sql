CREATE PROCEDURE dbo.DoFSAWarehouseSend
	@PreOrder int,
	@Username varchar(35),
	@Subteam int,
	@Warehouse int
AS 
BEGIN TRY
	SET NOCOUNT ON

	DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int, @ErrLine int
	DECLARE @OrderHeader_ID int

	DECLARE OrderHeaders CURSOR FOR
	SELECT
		OrderHeader_ID 
	FROM
		tmpOrdersAllocateOrderItems
	WHERE
		OrderHeader_ID IN	(
								SELECT 
									OrderHeader_ID
								FROM
									tmpOrdersAllocateOrderItems		oi
								INNER JOIN 
									tmpOrdersAllocateItems			i	ON i.Item_Key	=	oi.Item_Key
								WHERE
									Pre_Order		=	(CASE WHEN @PreOrder =	-1 THEN Pre_Order ELSE @PreOrder END)
								AND	i.Store_No		=	@Warehouse
								AND	i.SubTeam_No	=	@Subteam
								AND i.UserName		=	@Username
							)

	OPEN OrderHeaders

	FETCH NEXT FROM OrderHeaders
	INTO @OrderHeader_ID
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		EXEC UpdateOrderWarehouseSend @OrderHeader_ID
		
		FETCH NEXT FROM OrderHeaders
		INTO @OrderHeader_ID
		
	END

	CLOSE OrderHeaders
	DEALLOCATE OrderHeaders
	
END TRY

BEGIN CATCH

	CLOSE OrderHeaders;
	DEALLOCATE OrderHeaders;
	
	SELECT	@ErrMsg = ERROR_MESSAGE() + ' Error on Line:' + CAST(ERROR_LINE() as varchar(10)),
			@ErrSeverity = ERROR_SEVERITY(),
			@ErrLine = ERROR_LINE() 
	RAISERROR(@ErrMsg , 18, 1)
	
END CATCH;
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DoFSAWarehouseSend] TO [IRMAClientRole]
    AS [dbo];

