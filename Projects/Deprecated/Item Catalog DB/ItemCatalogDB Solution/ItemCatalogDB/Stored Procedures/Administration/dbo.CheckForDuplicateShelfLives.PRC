SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateShelfLives]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateShelfLives]
GO


CREATE PROCEDURE dbo.CheckForDuplicateShelfLives 
@ShelfLife_ID int, 
@ShelfLife_Name varchar(25) 
AS 

SELECT COUNT(*) AS ShelfLifeCount 
FROM ItemShelfLife 
WHERE ShelfLife_Name = @ShelfLife_Name AND ShelfLife_ID <> @ShelfLife_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


