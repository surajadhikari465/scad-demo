SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockBrand]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockBrand]
GO


CREATE PROCEDURE dbo.UnlockBrand 
@Brand_ID int 
AS 

UPDATE ItemBrand 
SET User_ID = NULL 
WHERE Brand_ID = @Brand_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


