SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBrandInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBrandInfo]
GO

CREATE PROCEDURE dbo.GetBrandInfo
	@Brand_ID int,
	@User_ID int
AS 

-- **************************************************************************************************
-- Procedure: [GetBrandInfo]
--    Author: unknown
--      Date: unknown
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2014/04/06	KM		14934	Add join to ValidatedBrand to check for brands that have an association in Icon;
--
-- **************************************************************************************************

BEGIN

    SELECT 
		Brand_Name, 
		Brand_ID, 
		Users.UserName, 
		ItemBrand.User_ID,
		vb.IconBrandId
    FROM 
		Users 
		RIGHT JOIN ItemBrand ON Users.User_ID = ItemBrand.User_ID
		LEFT JOIN ValidatedBrand vb ON ItemBrand.Brand_ID = vb.IrmaBrandId
    WHERE 
		Brand_ID = @Brand_ID

    UPDATE 
		ItemBrand 
	SET 
		User_ID = @User_ID 
    WHERE 
		Brand_ID = @Brand_ID AND User_ID IS NULL

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
