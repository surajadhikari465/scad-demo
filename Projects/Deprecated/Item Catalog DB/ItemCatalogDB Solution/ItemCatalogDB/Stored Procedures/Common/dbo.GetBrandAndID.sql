SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBrandAndID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBrandAndID]
GO

CREATE PROCEDURE dbo.GetBrandAndID 
AS 

-- **************************************************************************************************
-- Procedure: [GetBrandAndID]
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
		Brand_ID, 
		Brand_Name, 
		IconBrandId 
	FROM 
		ItemBrand (nolock)					ib
		LEFT JOIN ValidatedBrand (nolock)	vb	ON	ib.Brand_ID = vb.IrmaBrandId
	ORDER BY 
		Brand_Name

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO