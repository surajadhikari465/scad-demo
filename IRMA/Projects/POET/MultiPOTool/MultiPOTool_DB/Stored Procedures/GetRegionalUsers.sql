IF exists (SELECT * FROM dbo.sysobjects where id = object_id(N'[dbo].[GetRegionalUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetRegionalUsers]
GO

--***********************************************************************************************************************************
-- Date			TFS		DEV		Comment
-- 07/29/2013	12064	DN		Stored procedure creation. Retrieve a list of users within same region based on the User ID of the 
--								currently logged in user.
--***********************************************************************************************************************************

CREATE PROCEDURE [dbo].[GetRegionalUsers]
	(
	@UserID		INT
	)
AS
	/* SET NOCOUNT ON */
BEGIN
	SELECT 
		u.UserName,
		u.UserID
	FROM Users u (NOLOCK)
	WHERE u.RegionID = 
		(SELECT u1.RegionID
		FROM Users u1 (NOLOCK)
		WHERE u1.UserID = @UserID) AND
		  u.Active = 1
	ORDER BY u.UserName
END
GO