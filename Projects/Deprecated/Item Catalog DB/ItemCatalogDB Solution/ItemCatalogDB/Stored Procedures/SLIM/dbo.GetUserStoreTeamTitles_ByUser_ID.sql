if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUserStoreTeamTitles_ByUser_ID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUserStoreTeamTitles_ByUser_ID]
GO

CREATE PROCEDURE dbo.GetUserStoreTeamTitles_ByUser_ID 
	@User_ID int
AS 
	-- **************************************************************************
	-- Procedure: GetUserStoreTeamTitles_ByUser_ID()
	--    Author: 
	--      Date: 
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 2013-09-10   FA		Add transaction isolation level
	-- **************************************************************************
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	BEGIN TRAN

    select distinct 
		st.subteam_name,
		st.subteam_no 
	from 
		subteam st (NOLOCK)
        inner join userstoreteamtitle ust (NOLOCK) on st.team_no = ust.team_no
    where 
		ust.user_id = @User_ID
    order by 
		st.Subteam_Name
    
	COMMIT TRAN
END
GO
