IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'DeleteCustomerReturn' 
	   AND 	  type = 'P')
    DROP PROCEDURE DeleteCustomerReturn
GO

CREATE PROCEDURE dbo.DeleteCustomerReturn 
	@ReturnID int
AS
BEGIN
    SET NOCOUNT ON
    
    DELETE CustomerReturn
    WHERE ReturnID = @ReturnID

    SET NOCOUNT OFF
END

GO

