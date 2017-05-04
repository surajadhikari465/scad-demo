IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'UpdateCustReturnItem' 
	   AND 	  type = 'P')
    DROP PROCEDURE UpdateCustReturnItem
GO

CREATE PROCEDURE dbo.UpdateCustReturnItem 
	@ReturnItemID int,
    @Quantity decimal(18,4),
    @Weight decimal(18,4),
    @Amount decimal(18,4),
    @CustReturnReasonID int
AS
BEGIN
    SET NOCOUNT ON
    
    UPDATE CustomerReturnItem
    SET Quantity = @Quantity,
        Weight = @Weight,
        Amount = @Amount,
        CustReturnReasonID = @CustReturnReasonID
    WHERE ReturnItemID = @ReturnItemID

    SET NOCOUNT OFF
END

GO

