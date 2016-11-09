CREATE PROCEDURE dbo.GetReturnOrderRecords (
    @Instance   INT,
    @User_ID    INT)
AS
BEGIN
    SELECT * 
    FROM  ReturnOrder 
    WHERE Instance = @Instance AND 
          User_ID  = @User_ID 
    ORDER BY OrderItem_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReturnOrderRecords] TO [IRMAClientRole]
    AS [dbo];

