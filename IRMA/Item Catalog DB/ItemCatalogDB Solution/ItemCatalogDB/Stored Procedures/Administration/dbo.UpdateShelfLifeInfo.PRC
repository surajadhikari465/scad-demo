SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateShelfLifeInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateShelfLifeInfo]
GO


CREATE PROCEDURE dbo.UpdateShelfLifeInfo
@ShelfLife_ID int,
@ShelfLife_Name varchar(50)
AS 

UPDATE ItemShelfLife
SET User_ID = NULL,
    ShelfLife_Name = @ShelfLife_Name
WHERE ShelfLife_ID = @ShelfLife_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

