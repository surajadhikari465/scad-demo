CREATE PROCEDURE dbo.SOG_GetCatalogs
	@CatalogID		int,
	@StoreID		int,
	@SubTeamID		int,
	@ZoneID			int,
	@Published		bit,
	@CatalogCode	varchar(20),
	@Order			bit
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetCatalogs()
--    Author: Billy Blackerby
--      Date: 3/13/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of catalogs based upon parameters
--
-- Modification History:
-- Date			Init	Comment
-- 03/13/2009	BBB		Creation
-- 03/17/2009	BBB		Added in variable treatment for all selections; treated
--						date values coming out of SQL
-- 03/19/2009	BBB		Added in return of ItemManager.Manager_ID; added GroupBy
-- 03/24/2009	BBB		Added CatalogCode parameter
-- 03/25/2009	BBB		Added Order parameter
-- 04/14/2009	BBB		Added SubTeam_Name to output
-- 05/07/2009	BBB		Added Details to output
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Treat Variables
	--**************************************************************************
	IF @CatalogID = 0
		SET @CatalogID = NULL
	ELSE
		SET @Published = NULL

	IF @SubTeamID = 0
		SET @SubTeamID = NULL

	IF @ZoneID = 0
		SET @ZoneID = NULL
		
	IF @CatalogCode = '0'
		SET @CatalogCode = NULL

	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @Order = 0
		IF @StoreID = 0
			BEGIN
				SET @StoreID = NULL

				SELECT
					[CatalogID]		= c.CatalogID,
					[ManagedBy]		= im.Value,
					[ManagedByID]	= im.Manager_ID,
					[CatalogCode]	= c.CatalogCode,
					[Description]	= c.Description,
					[Details]		= c.Details,
					[Published]		= c.Published,
					[ExpectedDate]	= c.ExpectedDate,
					[SubTeam]		= c.SubTeam,
					[SubTeamName]	= st.SubTeam_Name,
					[InsertDate]	= CONVERT(varchar(10), c.InsertDate, 110),
					[UpdateDate]	= CONVERT(varchar(10), c.UpdateDate, 110),
					[InsertUser]	= c.InsertUser,
					[UpdateUser]	= c.UpdateUser
				FROM
					[Catalog]				(nolock) c
					LEFT JOIN CatalogStore	(nolock) cs ON c.CatalogID		= cs.CatalogID
					LEFT JOIN ItemManager	(nolock) im ON c.ManagedByID	= im.Manager_ID
					LEFT JOIN Store			(nolock) s	ON cs.StoreNo		= s.Store_No
					LEFT JOIN Zone			(nolock) z	ON s.Zone_ID		= z.Zone_ID
					LEFT JOIN SubTeam		(nolock) st	ON c.SubTeam		= st.SubTeam_No
				WHERE
					c.Deleted			= 0
					AND c.CatalogID		= ISNULL(@CatalogID, c.CatalogID)
					AND c.Published		= ISNULL(@Published, c.Published)
					AND c.SubTeam		= ISNULL(@SubTeamID, c.SubTeam)
					AND c.CatalogCode	= ISNULL(@CatalogCode, c.CatalogCode)
					AND 
						(
						z.Zone_ID		= ISNULL(@ZoneID, z.Zone_ID)
						OR
						z.Zone_ID		IS NULL
						)
					AND 
						(
						cs.StoreNo		= ISNULL(@StoreID, cs.StoreNo)
						OR
						cs.StoreNo		IS NULL
						)
				GROUP BY
					c.CatalogID,
					im.Value,
					im.Manager_ID,
					c.CatalogCode,
					c.Description,
					c.Details,
					c.Published,
					c.ExpectedDate,
					c.SubTeam,
					st.SubTeam_Name,
					c.InsertDate,
					c.UpdateDate,
					c.InsertUser,
					c.UpdateUser
			END
		ELSE
			SELECT
				[CatalogID]		= c.CatalogID,
				[ManagedBy]		= im.Value,
				[ManagedByID]	= im.Manager_ID,
				[CatalogCode]	= c.CatalogCode,
				[Description]	= c.Description,
				[Details]		= c.Details,
				[Published]		= c.Published,
				[ExpectedDate]	= c.ExpectedDate,
				[SubTeam]		= c.SubTeam,
				[SubTeamName]	= st.SubTeam_Name,
				[InsertDate]	= CONVERT(varchar(10), c.InsertDate, 110),
				[UpdateDate]	= CONVERT(varchar(10), c.UpdateDate, 110),
				[InsertUser]	= c.InsertUser,
				[UpdateUser]	= c.UpdateUser
			FROM
				[Catalog]				(nolock) c
				LEFT JOIN CatalogStore	(nolock) cs ON c.CatalogID		= cs.CatalogID
				LEFT JOIN ItemManager	(nolock) im ON c.ManagedByID	= im.Manager_ID
				LEFT JOIN Store			(nolock) s	ON cs.StoreNo		= s.Store_No
				LEFT JOIN Zone			(nolock) z	ON s.Zone_ID		= z.Zone_ID
				LEFT JOIN SubTeam		(nolock) st	ON c.SubTeam		= st.SubTeam_No
			WHERE
				c.Deleted			= 0
				AND c.CatalogID		= ISNULL(@CatalogID, c.CatalogID)
				AND c.Published		= ISNULL(@Published, c.Published)
				AND c.SubTeam		= ISNULL(@SubTeamID, c.SubTeam)
				AND c.CatalogCode	= ISNULL(@CatalogCode, c.CatalogCode)
				AND z.Zone_ID		= ISNULL(@ZoneID, z.Zone_ID)
				AND cs.StoreNo		= ISNULL(@StoreID, cs.StoreNo)
			GROUP BY
				c.CatalogID,
				im.Value,
				im.Manager_ID,
				c.CatalogCode,
				c.Description,
				c.Details,
				c.Published,
				c.ExpectedDate,
				c.SubTeam,
				st.SubTeam_Name,
				c.InsertDate,
				c.UpdateDate,
				c.InsertUser,
				c.UpdateUser
	ELSE
		BEGIN 
			IF @StoreID = 0
				SET @StoreID = NULL
				
			SELECT
				[CatalogID]		= c.CatalogID,
				[ManagedBy]		= im.Value,
				[ManagedByID]	= im.Manager_ID,
				[CatalogCode]	= c.CatalogCode,
				[Description]	= c.Description,
				[Details]		= c.Details,
				[Published]		= c.Published,
				[ExpectedDate]	= c.ExpectedDate,
				[SubTeam]		= c.SubTeam,
				[SubTeamName]	= st.SubTeam_Name,
				[InsertDate]	= CONVERT(varchar(10), c.InsertDate, 110),
				[UpdateDate]	= CONVERT(varchar(10), c.UpdateDate, 110),
				[InsertUser]	= c.InsertUser,
				[UpdateUser]	= c.UpdateUser
			FROM
				[Catalog]				(nolock) c
				LEFT JOIN CatalogStore	(nolock) cs ON c.CatalogID		= cs.CatalogID
				LEFT JOIN ItemManager	(nolock) im ON c.ManagedByID	= im.Manager_ID
				LEFT JOIN Store			(nolock) s	ON cs.StoreNo		= s.Store_No
				LEFT JOIN Zone			(nolock) z	ON s.Zone_ID		= z.Zone_ID
				LEFT JOIN SubTeam		(nolock) st	ON c.SubTeam		= st.SubTeam_No
			WHERE
				c.Deleted			= 0
				AND c.CatalogID		= ISNULL(@CatalogID, c.CatalogID)
				AND c.Published		= ISNULL(@Published, c.Published)
				AND c.CatalogCode	= ISNULL(@CatalogCode, c.CatalogCode)
				AND cs.StoreNo		= ISNULL(@StoreID, cs.StoreNo)
			GROUP BY
				c.CatalogID,
				im.Value,
				im.Manager_ID,
				c.CatalogCode,
				c.Description,
				c.Details,
				c.Published,
				c.ExpectedDate,
				c.SubTeam,
				st.SubTeam_Name,
				c.InsertDate,
				c.UpdateDate,
				c.InsertUser,
				c.UpdateUser
		END
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SOG_GetCatalogs] TO [IRMASLIMRole]
    AS [dbo];

