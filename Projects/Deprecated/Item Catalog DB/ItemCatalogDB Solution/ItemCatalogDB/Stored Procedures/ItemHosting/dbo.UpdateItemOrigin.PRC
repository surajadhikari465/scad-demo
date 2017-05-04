SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateItemOrigin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateItemOrigin]
GO


CREATE PROCEDURE dbo.UpdateItemOrigin
@Item_Key int, 
@Origin_ID int
AS 

UPDATE Item
SET Origin_ID = @Origin_ID
WHERE Item_Key = @Item_Key
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


