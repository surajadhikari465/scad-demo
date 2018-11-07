create PROCEDURE [dbo].[SubTeams_AddDiscountExceptions]
	-- Add the parameters for the stored procedure here
	@Store_no int,
	@SubTeam_no int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if not exists( select 'TRUE' from StoreSubteamDiscountException ssde where ssde.Store_No = @Store_no and ssde.Subteam_No = @SubTeam_no )
	begin
	    insert  into StoreSubteamDiscountException
	    (       Store_No, Subteam_No
	    )
	    values( @Store_no, @SubTeam_no )
	end
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_AddDiscountExceptions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_AddDiscountExceptions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_AddDiscountExceptions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_AddDiscountExceptions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SubTeams_AddDiscountExceptions] TO [IRMAReportsRole]
    AS [dbo];

