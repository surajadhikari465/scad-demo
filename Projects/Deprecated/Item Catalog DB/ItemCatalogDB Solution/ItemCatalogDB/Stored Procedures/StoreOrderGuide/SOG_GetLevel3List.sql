SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SOG_GetLevel3List')
	BEGIN
		DROP Procedure [dbo].SOG_GetLevel3List
	END
GO

CREATE PROCEDURE dbo.SOG_GetLevel3List
	@Catalog	bit,
	@ClassID	int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: SOG_GetLevel3List
--    Author: Billy Blackerby
--      Date: 4/11/2009
--
-- Description:
-- Utilized by StoreOrderGuide to return a list of level3 hierarchies for filters
--
-- Modification History:
-- Date			Init	Comment
-- 04/11/2009	BBB		Creation of SP
-- 04/16/2009	BBB		Added ClassID parameter
-- **************************************************************************	
BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @Catalog = 1
		BEGIN
			SELECT DISTINCT
				[Level3ID]		= lvl3.ProdHierarchyLevel3_ID,
				[Level3Name]	= lvl3.Description
			FROM 
				ProdHierarchyLevel3	(nolock) lvl3
			WHERE
				lvl3.ProdHierarchyLevel3_ID > 0
				AND lvl3.Category_ID		= @ClassID
			ORDER BY 
				Level3ID, 
				Level3Name
		END
	ELSE
		BEGIN
			SELECT
				[Level3ID]		= 0,
				[Level3Name]	= 'All Level 3s'
				
			UNION
			
			SELECT DISTINCT
				[Level3ID]		= lvl3.ProdHierarchyLevel3_ID,
				[Level3Name]	= lvl3.Description
			FROM 
				ProdHierarchyLevel3	(nolock) lvl3
			WHERE
				lvl3.ProdHierarchyLevel3_ID > 0
				AND lvl3.Category_ID		= @ClassID
			ORDER BY 
				Level3ID, 
				Level3Name
		END

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO