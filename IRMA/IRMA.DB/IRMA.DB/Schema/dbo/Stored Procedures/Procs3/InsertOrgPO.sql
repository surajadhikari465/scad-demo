CREATE PROCEDURE dbo.InsertOrgPO
@OrgOrderHeader_ID int,
@ReturnOrderHeader_ID int
AS 

BEGIN


DELETE ReturnOrderList WHERE ReturnOrderHeader_ID = @ReturnOrderHeader_ID

 INSERT INTO ReturnOrderList (OrderHeader_ID, ReturnOrderHeader_ID) VALUES (@OrgOrderHeader_ID, @ReturnOrderHeader_ID)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrgPO] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrgPO] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertOrgPO] TO [IRMAReportsRole]
    AS [dbo];

