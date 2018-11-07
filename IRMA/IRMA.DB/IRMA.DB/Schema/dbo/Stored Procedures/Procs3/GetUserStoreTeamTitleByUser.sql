CREATE PROCEDURE dbo.GetUserStoreTeamTitleByUser
    @User_ID int
AS
-- **************************************************************************
-- Procedure: GetUserStoreTeamTitleByUser()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from the IRMA User Management interface
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 01/11/2011	BBB   	875		changed inner join to Store to left join; added
--								isnull and column aliases to Store_No and Store_Name
-- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
 	SELECT
 		Store_Name	=	ISNULL(Store.Store_Name, 'All Facility Teams'),
 		Team.Team_Name, 
 		Store_No	=	ISNULL(Store.Store_No, 0), 
 		Team.Team_No
	FROM         
		UserStoreTeamTitle 
		LEFT JOIN Store		ON UserStoreTeamTitle.Store_No	= Store.Store_No 
		INNER JOIN Team		ON UserStoreTeamTitle.Team_No	= Team.Team_No
	WHERE     
		(UserStoreTeamTitle.User_ID = @User_ID)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserStoreTeamTitleByUser] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUserStoreTeamTitleByUser] TO [IRMAClientRole]
    AS [dbo];

