CREATE PROCEDURE dbo.InsertODBCError
@ODBCStart datetime,
@ODBCEnd datetime,
@ErrorNumber int, 
@ErrorDescription varchar(4096),
@ODBCCall varchar(4096)
AS 

INSERT INTO ODBCErrorLog (SystemTime, ODBCStart, ODBCEnd, ErrorNumber, ErrorDescription, ODBCCall, UserName, Computer)
VALUES (GetDate(), @ODBCStart, @ODBCEnd, @ErrorNumber, @ErrorDescription, @ODBCCall, SUSER_NAME(), HOST_NAME())
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMAAVCIRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IMHARole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMARSTRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [ExtractRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertODBCError] TO [IRMAPromoRole]
    AS [dbo];

