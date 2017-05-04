SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCategoryLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCategoryLockStatus]
GO


CREATE PROCEDURE dbo.GetCategoryLockStatus 
@Category_ID int 
AS 

SELECT User_ID 
FROM ItemCategory 
WHERE Category_ID = @Category_ID 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


