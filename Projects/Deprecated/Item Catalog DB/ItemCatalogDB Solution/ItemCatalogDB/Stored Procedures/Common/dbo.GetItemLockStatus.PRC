SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemLockStatus]
GO


CREATE PROCEDURE dbo.GetItemLockStatus 
@Item_Key int 
AS 

SELECT User_ID 
FROM Item 
WHERE Item_Key = @Item_Key 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


