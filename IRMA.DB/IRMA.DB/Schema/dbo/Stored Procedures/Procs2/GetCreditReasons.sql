CREATE PROCEDURE dbo.GetCreditReasons
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT * 
    FROM  CreditReasons
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditReasons] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditReasons] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditReasons] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCreditReasons] TO [IRMAReportsRole]
    AS [dbo];

