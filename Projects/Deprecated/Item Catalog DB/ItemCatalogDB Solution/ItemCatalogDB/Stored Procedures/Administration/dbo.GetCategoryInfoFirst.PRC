SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCategoryInfoFirst]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCategoryInfoFirst]
GO


CREATE PROCEDURE dbo.GetCategoryInfoFirst
AS 

SELECT Category_Name, Category_ID, SubTeam_No, User_ID 
FROM ItemCategory 
WHERE Category_ID = (SELECT MIN(Category_ID) FROM ItemCategory)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

