SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetBrandList')
	BEGIN
		DROP Procedure [dbo].SOG_GetBrandList
	END
GO

CREATE PROCEDURE dbo.SOG_GetBrandList
	@Catalog	bit
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetBrandList
--    Author: Billy Blackerby
--      Date: 4/11/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of brands for filters
--
-- Modification History:
-- Date			Init	Comment
-- 04/11/2009	BBB		Creation
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @Catalog = 1
		BEGIN
			SELECT DISTINCT
				[BrandID]	= ib.Brand_ID,
				[BrandName]	= ib.Brand_Name
			FROM 
				ItemBrand		(nolock) ib				
				INNER JOIN Item	(nolock) i	ON ib.Brand_ID = i.Brand_ID
			WHERE
				ib.Brand_ID > 0
			ORDER BY 
				BrandID, 
				BrandName
		END
	ELSE
		BEGIN
			SELECT
				[BrandID]	= 0,
				[BrandName]	= 'All Brands'
				
			UNION
			
			SELECT DISTINCT
				[BrandID]	= ib.Brand_ID,
				[BrandName]	= ib.Brand_Name
			FROM 
				ItemBrand		(nolock) ib				
				INNER JOIN Item	(nolock) i	ON ib.Brand_ID = i.Brand_ID
			WHERE
				ib.Brand_ID > 0
			ORDER BY 
				BrandID, 
				BrandName
		END

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO