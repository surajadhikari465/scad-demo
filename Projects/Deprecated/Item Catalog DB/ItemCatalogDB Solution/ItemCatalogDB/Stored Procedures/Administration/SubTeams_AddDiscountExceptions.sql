/****** Object:  StoredProcedure [dbo].[SubTeams_AddDiscountExceptions]    Script Date: 03/15/2012 13:35:03 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SubTeams_AddDiscountExceptions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SubTeams_AddDiscountExceptions]
GO

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
