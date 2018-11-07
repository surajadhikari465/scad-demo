SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCategoryAndID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCategoryAndID]
GO


CREATE PROCEDURE dbo.GetCategoryAndID 
AS 

SELECT Category_ID, Category_Name, SubTeam_No
FROM ItemCategory 
ORDER BY Category_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


