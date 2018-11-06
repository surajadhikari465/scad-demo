SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCategoriesBySubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCategoriesBySubTeam]
GO


CREATE PROCEDURE dbo.GetCategoriesBySubTeam
@SubTeam_No as int
AS 
BEGIN
    SET NOCOUNT ON

    SELECT Category_ID, Category_Name
    FROM  ItemCategory (nolock) 
    WHERE SubTeam_No = @SubTeam_No
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




