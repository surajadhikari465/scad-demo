SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemOnHandComparisonBetweenLocation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemOnHandComparisonBetweenLocation]
GO

CREATE PROCEDURE dbo.ItemOnHandComparisonBetweenLocation
    @Store1_No int,
    @Store2_No int,
    @SubTeam_No int
AS

-- **************************************************************************  
-- Procedure: ItemOnHandComparisonBetweenLocations()  
--  
-- Description:  
-- This procedure is called by the ItemOnHandComparison.vb
--  
-- Modification History:  
-- Date			Init		TFS		Comment  
-- 01.04.13		BS			8755	Coding Standards. Updated extension to .sql
--									Changed i.Discontinue_Item to scalar function
--									to account for schema change.	
-- **************************************************************************  

BEGIN
    SET NOCOUNT ON

	SELECT
		i.Item_Key,
		i.Item_Description,
		ii.Identifier, 
		i.Package_Desc1,
		i.Package_Desc2,
		i.Package_Unit_ID,
		(ohw.Quantity + ohw.Weight) AS On_Hand_With, 
		(ohwo.Quantity + ohwo.Weight) AS On_Hand_WithOut, 
		iu.Unit_Name,
		ohw.Store_No,
		ohwo.Store_No
	FROM
		Item						i		(nolock)
		INNER JOIN ItemIdentifier	ii		(nolock) ON ii.Item_Key					= i.Item_Key
														AND ii.Default_Identifier	= 1
		LEFT JOIN ItemUnit			iu		(nolock) ON iu.Unit_Id					= i.Package_Unit_ID
		INNER JOIN OnHand			ohw		(nolock) ON ohw.Item_Key				= i.Item_Key
														AND ohw.Store_No			= @Store1_No
														AND ohw.SubTeam_No			= @SubTeam_No
		LEFT JOIN OnHand			ohwo	(nolock) ON ohwo.Item_Key				= i.Item_Key
														AND ohwo.Store_No			= @Store2_No
														AND ohwo.SubTeam_No			= @SubTeam_No

	WHERE
		ISNULL(ohwo.Quantity + ohwo.Weight,0) = 0
		AND dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, NULL) <> 0

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


