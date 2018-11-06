 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_PrintCatalogs')
	BEGIN
		DROP Procedure [dbo].SOG_PrintCatalogs
	END
GO

CREATE PROCEDURE dbo.SOG_PrintCatalogs

WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_PrintCatalogs()
--    Author: Billy Blackerby
--      Date: 4/3/2009
--
-- Description:
-- Utilized by SSRS to return data consumed by SOG_PrintCatalogs.RDL
--
-- Modification History:
-- Date			Init	Comment
-- 04/03/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT
		[CatalogID]		= c.CatalogID,
		[ManagedBy]		= im.Value,
		[ManagedByID]	= im.Manager_ID,
		[CatalogCode]	= c.CatalogCode,
		[Description]	= c.Description,
		[Published]		= c.Published,
		[ExpectedDate]	= c.ExpectedDate,
		[SubTeam]		= c.SubTeam,
		[InsertDate]	= CONVERT(varchar(10), c.InsertDate, 110),
		[UpdateDate]	= CONVERT(varchar(10), c.UpdateDate, 110),
		[InsertUser]	= c.InsertUser,
		[UpdateUser]	= c.UpdateUser
	FROM
		[Catalog]				(nolock) c
		LEFT JOIN ItemManager	(nolock) im ON c.ManagedByID	= im.Manager_ID
	WHERE
		c.Deleted			= 0
	GROUP BY
		c.CatalogID,
		im.Value,
		im.Manager_ID,
		c.CatalogCode,
		c.Description,
		c.Published,
		c.ExpectedDate,
		c.SubTeam,
		c.InsertDate,
		c.UpdateDate,
		c.InsertUser,
		c.UpdateUser
    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 