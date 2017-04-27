IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetItemUnitInfoStore]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetItemUnitInfoStore]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetItemUnitInfoStore]
    @Item_Key int,
    @Store_No int

AS

-- **************************************************************************
-- Procedure: GetItemUnitInfoStore()
--    Author: Faisal Ahmed
--      Date: 12.14.2012
--
-- Description:
-- This procedure is used to retrieve item info based on store jurisdicton
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 05.05.11		FA   	9251	Creation
-- 2013-04-01	KM		11750	Add ItemDescription to the selection;
-- **************************************************************************

BEGIN
    SET NOCOUNT ON

    SELECT
		ISNULL(ior.Item_Description, i.Item_Description)	AS ItemDescription,
		ISNULL(ior.Package_Desc1, i.Package_Desc1)			AS Package_Desc1,  	
		ISNULL(ior.Package_Desc2, i.Package_Desc2)			AS Package_Desc2,  		
		ISNULL(ior.Package_Unit_ID, i.Package_Unit_ID)		AS PU_Unit,  		
        pu.Unit_Name										AS PU_Name
		
    FROM 
		Item 						(nolock) i
		INNER JOIN	Store			(nolock) s		ON	s.Store_No				= @Store_No
		LEFT JOIN	ItemOverride	(nolock) ior	ON  ior.Item_Key			= i.Item_Key 
													AND ior.StoreJurisdictionID	= s.StoreJurisdictionID
		LEFT JOIN   ItemUnit		(nolock) pu		ON ISNULL(ior.Package_Unit_ID, i.Package_Unit_ID) = pu.Unit_ID  
    
    WHERE 
		i.Item_Key = @Item_Key

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO