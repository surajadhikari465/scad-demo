/****** Object:  Stored Procedure dbo.SetFQSCusModifiedCreatedTime    Script Date: 4/25/99 10:35:03 PM ******/
CREATE PROCEDURE dbo.SetFQSCusModifiedCreatedTime
@CusNumber int
AS
UPDATE FQSCustomer SET
ModifiedCreatedTime = GetDate()
WHERE CusNumber = @CusNumber
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSCusModifiedCreatedTime] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSCusModifiedCreatedTime] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSCusModifiedCreatedTime] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSCusModifiedCreatedTime] TO [IRMAReportsRole]
    AS [dbo];

