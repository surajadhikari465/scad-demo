SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemAdminUsers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemAdminUsers]
GO


CREATE PROCEDURE dbo.GetItemAdminUsers
AS

SELECT 
	[User_ID], UserName, FullName  
FROM 
	Users
WHERE 
	SuperUser = 1 OR Item_Administrator = 1	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

