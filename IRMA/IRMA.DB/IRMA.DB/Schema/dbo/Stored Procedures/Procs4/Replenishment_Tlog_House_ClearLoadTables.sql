CREATE PROCEDURE dbo.Replenishment_Tlog_House_ClearLoadTables
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @Sql as varchar(1024)
	
        DELETE FROM TLOG_DISCNT
        DELETE FROM TLOG_MRKDWN
        DELETE FROM TLOG_ITEM
        DELETE FROM TLOG_TAXREC
        DELETE FROM TLOG_TENDER
        DELETE FROM TLOG_CMCARD
        DELETE FROM TLOG_CMRESERVE
        DELETE FROM TLOG_CMREWARD
        --DELETE FROM TLOG_CMVAR

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_ClearLoadTables] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_ClearLoadTables] TO [IRMAClientRole]
    AS [dbo];

