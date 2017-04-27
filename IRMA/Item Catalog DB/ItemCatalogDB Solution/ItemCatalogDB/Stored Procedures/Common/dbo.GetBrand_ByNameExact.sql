if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBrand_ByNameExact]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBrand_ByNameExact]
GO


CREATE PROCEDURE dbo.GetBrand_ByNameExact
	@Brand_Name varchar(25)
AS 

SELECT 
	Brand_ID,
	Brand_Name,
	User_ID
FROM 
	ItemBrand
WHERE 
	Brand_Name = @Brand_Name

GO