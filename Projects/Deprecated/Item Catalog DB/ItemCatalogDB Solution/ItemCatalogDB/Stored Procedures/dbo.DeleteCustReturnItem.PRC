IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'DeleteCustReturnItem' 
	   AND 	  type = 'P')
    DROP PROCEDURE DeleteCustReturnItem
GO

CREATE PROCEDURE dbo.DeleteCustReturnItem 
	@ReturnItemID int
AS
BEGIN
    SET NOCOUNT ON
    
    DELETE CustomerReturnItem
    WHERE ReturnItemID = @ReturnItemID

    SET NOCOUNT OFF
END

GO
