create PROCEDURE dbo.SLIM_GetInStoreSpecials
 @requestId int
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

	SELECT	
		RequestId,
		Item_Key,
		Store_no,
		Price,
		Multiple,
		SalePrice,
		SaleMultiple,
		POSPrice,
		POSSalePrice,
		StartDate,
		EndDate,
		Status,
		RequestedBy,
		ProcessedBy,
		Comments  
	FROM SLIM_InstoreSpecials
	WHERE requestId=@requestId

	COMMIT TRAN
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_GetInStoreSpecials] TO [IRMASLIMRole]
    AS [dbo];

