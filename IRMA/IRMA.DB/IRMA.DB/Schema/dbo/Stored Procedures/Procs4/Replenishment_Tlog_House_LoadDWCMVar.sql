CREATE PROCEDURE dbo.Replenishment_Tlog_House_LoadDWCMVar
	@Path as varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @Sql as varchar(1024)
	
	set @Sql = 'BULK INSERT TLOG_CMVAR FROM ''' +  @Path + ''' WITH (FIELDTERMINATOR = '','')'
	exec(@Sql)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_LoadDWCMVar] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_LoadDWCMVar] TO [IRMAClientRole]
    AS [dbo];

