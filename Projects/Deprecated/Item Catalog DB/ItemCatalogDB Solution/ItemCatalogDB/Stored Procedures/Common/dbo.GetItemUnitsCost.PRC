SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemUnitsCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetItemUnitsCost]
GO

-- Updated to match SO functionality as of 5/16/2008
-- Related to bug 6095. Was originaly changed to match OLD SO code from 2 years ago. This was not correct.


--CREATE PROCEDURE dbo.GetItemUnitsCost
--    @WeightUnits bit,	-- If AllUnits = FALSE,
						--		Setting WeightUnits to TRUE will limit the results to Weight_Unit = TRUE values
						--		Setting WeightUnits to FALSE will limit the results to Weight_Unit = FALSE values
--    @AllUnits bit = 0	-- Returns all UOM values, regardless of Weight_Unit flag
--AS 

--IF @AllUnits = 1 
--BEGIN
--	SELECT Unit_ID, Weight_Unit, Unit_Name, Unit_Abbreviation
--	FROM ItemUnit (NOLOCK)
--	ORDER BY Unit_Name
--END
--ELSE
--BEGIN
--	SELECT Unit_ID, Weight_Unit, Unit_Name, Unit_Abbreviation
--	FROM ItemUnit (NOLOCK)
--	WHERE Weight_Unit = @WeightUnits
--	ORDER BY Unit_Name
--END
--
--GO
--SET QUOTED_IDENTIFIER OFF 
--GO
--SET ANSI_NULLS ON 
--GO

CREATE PROCEDURE dbo.GetItemUnitsCost
    @WeightUnits bit
AS 

SELECT Unit_ID, Weight_Unit, Unit_Name, Unit_Abbreviation
FROM ItemUnit (NOLOCK)
WHERE Weight_Unit = @WeightUnits OR IsPackageUnit = 1
ORDER BY Unit_Name



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


