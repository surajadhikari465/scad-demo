SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockItem]
GO


CREATE PROCEDURE dbo.UnlockItem 
@Item_Key int 
AS 

UPDATE Item 
SET User_ID = NULL, User_ID_Date = NULL
FROM Item (rowlock)
WHERE Item_Key = @Item_Key
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

