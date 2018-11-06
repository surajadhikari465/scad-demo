IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetCustReturnReasons' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetCustReturnReasons
GO

CREATE PROCEDURE dbo.GetCustReturnReasons 
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT CustReturnReasonID, CustReturnReason
    FROM CustomerReturnReason (NOLOCK)
    ORDER BY CustReturnReason

    SET NOCOUNT OFF
END

GO

