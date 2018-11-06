if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SubTeams_GetDiscountExceptions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SubTeams_GetDiscountExceptions]
GO

create PROCEDURE [dbo].[SubTeams_GetDiscountExceptions]
	-- Add the parameters for the stored procedure here
	@Store_no int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	ss.SubTeam_No, s.SubTeam_Name, s.SubTeam_Abbreviation,
	        /* IsException will be 0 if there is no exception or 1 if there is an exception. */
	        SIGN(ISNULL(ssde.Subteam_No, 0)) IsException
	FROM StoreSubteam ss 
	INNER JOIN SubTeam s ON 
		ss.SubTeam_No = s.SubTeam_No
	left outer join StoreSubteamDiscountException ssde
	    on  ssde.Store_No = ss.Store_No
	    and ssde.Subteam_No = ss.SubTeam_No
	WHERE ss.Store_No = @Store_no
END
GO
