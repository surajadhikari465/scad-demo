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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustReturnReasons] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustReturnReasons] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustReturnReasons] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCustReturnReasons] TO [IRMAReportsRole]
    AS [dbo];

