create PROCEDURE [dbo].[SubTeams_RemoveDiscountExceptions]
	-- Add the parameters for the stored procedure here
	@Store_no int,
	@SubTeam_no int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if exists( select 'TRUE' from StoreSubteamDiscountException ssde where ssde.Store_No = @Store_no and ssde.Subteam_No = @SubTeam_no )
	begin
	    delete  StoreSubteamDiscountException
	    where   Store_No = @Store_no
	    and     Subteam_No = @SubTeam_no
	end
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveDiscountExceptions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveDiscountExceptions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveDiscountExceptions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveDiscountExceptions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_RemoveDiscountExceptions] TO [IRMAReportsRole]
    AS [dbo];

