CREATE PROCEDURE dbo.GetCustomerReturnHistory 
	@CustomerID int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT ReturnID, Store_No, User_ID, ReturnDate, Approver_ID,
           (SELECT COUNT(*) FROM CustomerReturnItem WHERE ReturnID = CustomerReturn.ReturnID) As ReturnItemCount,
           ISNULL((SELECT SUM((ISNULL(Quantity, 0) * Amount) + (ISNULL(Weight, 0) * Amount)) FROM CustomerReturnItem WHERE ReturnID = CustomerReturn.ReturnID), 0) As ReturnItemTotal
    FROM CustomerReturn (NOLOCK)
    WHERE CustomerID = @CustomerID
    ORDER BY ReturnDate DESC

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnHistory] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnHistory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnHistory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustomerReturnHistory] TO [IRMAReportsRole]
    AS [dbo];

