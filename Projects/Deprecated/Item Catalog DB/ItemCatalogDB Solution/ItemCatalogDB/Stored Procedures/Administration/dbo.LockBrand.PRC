SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LockBrand]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LockBrand]
GO


CREATE PROCEDURE dbo.LockBrand 
@Brand_ID int, 
@User_ID int 
AS 

UPDATE ItemBrand SET 
User_ID = @User_ID 

WHERE Brand_ID = @Brand_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


