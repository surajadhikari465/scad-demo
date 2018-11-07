SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateBrandInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateBrandInfo]
GO


CREATE PROCEDURE dbo.UpdateBrandInfo
@Brand_ID int,
@Brand_Name varchar(50)
AS 

UPDATE ItemBrand
SET User_ID = NULL,
    Brand_Name = @Brand_Name
WHERE Brand_ID = @Brand_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


