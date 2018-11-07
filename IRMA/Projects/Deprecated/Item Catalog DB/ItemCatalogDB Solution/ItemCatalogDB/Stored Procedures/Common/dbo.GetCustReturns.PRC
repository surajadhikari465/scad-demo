IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'GetCustReturns' 
	   AND 	  type = 'P')
    DROP PROCEDURE GetCustReturns
GO

CREATE PROCEDURE dbo.GetCustReturns 
	@Store_No int,
    @FromDate datetime,
    @ToDate datetime,
    @ReturnTotal money
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT ReturnDate, LastName, FirstName, Phone, SubTeam_Name, Item_Description, CustReturnReason, Quantity, Weight, Amount
    FROM CustomerReturn
    INNER JOIN
        Customer (NOLOCK)
        ON Customer.CustomerID = CustomerReturn.CustomerID
    INNER JOIN 
        CustomerReturnItem (NOLOCK)
        ON CustomerReturnItem.ReturnID = CustomerReturn.ReturnID
    INNER JOIN
        CustomerReturnReason (NOLOCK)
        ON CustomerReturnReason.CustReturnReasonID = CustomerReturnItem.CustReturnReasonID
    INNER JOIN
        Item (NOLOCK)
        ON Item.Item_Key = CustomerReturnItem.Item_Key
    INNER JOIN
        SubTeam (NOLOCK)
        ON SubTeam.SubTeam_No = Item.SubTeam_No
    WHERE Store_No = @Store_No
    AND ReturnDate >= ISNULL(@FromDate, ReturnDate) AND DATEDIFF(day, ReturnDate, ISNULL(@ToDate, ReturnDate)) >= 0
    AND EXISTS (SELECT * FROM (SELECT SUM((Quantity * Amount) + (Weight * Amount)) As ReturnTotal
                               FROM CustomerReturnItem (NOLOCK)
                               WHERE CustomerReturnItem.ReturnID = CustomerReturn.ReturnID
                               GROUP BY ReturnID
                               HAVING SUM((Quantity * Amount) + (Weight * Amount)) >= ISNULL(@ReturnTotal, 0)) T1)

    SET NOCOUNT OFF
END

GO


