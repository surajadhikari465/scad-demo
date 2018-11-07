/****** Object:  Stored Procedure dbo.SetFQSOrgModifiedCreatedTime    Script Date: 4/25/99 10:35:03 PM ******/
CREATE PROCEDURE dbo.SetFQSOrgModifiedCreatedTime
@OrgNumber int
AS
UPDATE FQSOrganization
SET ModifiedCreatedTime = GetDate()
WHERE OrgNumber = @OrgNumber
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSOrgModifiedCreatedTime] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSOrgModifiedCreatedTime] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSOrgModifiedCreatedTime] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SetFQSOrgModifiedCreatedTime] TO [IRMAReportsRole]
    AS [dbo];

