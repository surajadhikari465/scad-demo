SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCategoryInfoLast]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCategoryInfoLast]
GO


CREATE PROCEDURE dbo.GetCategoryInfoLast
AS 

SELECT Category_Name, Category_ID, SubTeam_No, User_ID 
FROM ItemCategory 
WHERE Category_ID = (SELECT MAX(Category_ID) FROM ItemCategory)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


