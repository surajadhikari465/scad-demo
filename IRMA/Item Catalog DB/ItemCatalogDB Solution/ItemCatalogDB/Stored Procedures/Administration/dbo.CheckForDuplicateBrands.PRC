SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateBrands]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateBrands]
GO


CREATE PROCEDURE dbo.CheckForDuplicateBrands 
@Brand_ID int, 
@Brand_Name varchar(25) 
AS 

SELECT COUNT(*) AS BrandCount 
FROM ItemBrand 
WHERE Brand_Name = @Brand_Name AND Brand_ID <> @Brand_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


