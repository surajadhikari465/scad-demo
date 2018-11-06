SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeamBrand]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeamBrand]
GO


CREATE PROCEDURE dbo.GetSubTeamBrand 
	@SubTeam_No int
AS 

SET NOCOUNT ON

SELECT DISTINCT 
	ItemBrand.Brand_ID, ItemBrand.Brand_Name 
FROM 
	ItemBrand (NOLOCK)
INNER JOIN
	Item (NOLOCK) ON (Item.Brand_ID = ItemBrand.Brand_ID)
WHERE
	Item.SubTeam_No = @SubTeam_No
Order by ItemBrand.Brand_Name

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

