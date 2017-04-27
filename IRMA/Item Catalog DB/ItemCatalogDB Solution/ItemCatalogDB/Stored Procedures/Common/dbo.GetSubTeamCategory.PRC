SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeamCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeamCategory]
GO


CREATE PROCEDURE dbo.GetSubTeamCategory 
	@SubTeam_No int
AS 

SET NOCOUNT ON

SELECT DISTINCT 
	ItemCategory.Category_ID
	,ItemCategory.Category_Name 
FROM 
	ItemCategory (NOLOCK)
INNER JOIN 
	Item (NOLOCK) ON (Item.Category_ID = ItemCategory.Category_ID)
WHERE
	Item.SubTeam_No = @SubTeam_No

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


