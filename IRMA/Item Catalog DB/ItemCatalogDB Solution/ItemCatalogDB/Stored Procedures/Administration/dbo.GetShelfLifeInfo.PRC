SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetShelfLifeInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetShelfLifeInfo]
GO


CREATE PROCEDURE dbo.GetShelfLifeInfo
@ShelfLife_ID int
AS 

SELECT ShelfLife_Name, ShelfLife_ID, User_ID 
FROM ItemShelfLife
WHERE ShelfLife_ID = @ShelfLife_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



