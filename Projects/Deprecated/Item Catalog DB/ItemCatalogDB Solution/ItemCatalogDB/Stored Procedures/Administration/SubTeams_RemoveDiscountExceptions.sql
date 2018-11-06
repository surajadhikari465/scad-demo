if exists( select 'TRUE' from sys.objects o where o.object_id = OBJECT_ID(N'SubTeams_RemoveDiscountExceptions') )
begin
    drop procedure dbo.SubTeams_RemoveDiscountExceptions
end
go

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
