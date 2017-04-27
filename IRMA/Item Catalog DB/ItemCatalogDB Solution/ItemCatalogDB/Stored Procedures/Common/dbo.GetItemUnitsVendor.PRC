/****** Object:  StoredProcedure [dbo].[GetItemUnitsVendor]    Script Date: 06/02/2006 14:30:05 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetItemUnitsVendor]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetItemUnitsVendor]

/****** Object:  StoredProcedure [dbo].[GetItemUnitsVendor]    Script Date: 06/02/2006 14:30:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Updated to match SO functionality as of 5/16/2008
-- Related to bug 6095. Was originaly changed to match OLD SO code from 2 years ago. This was not correct.


CREATE PROCEDURE dbo.[GetItemUnitsVendor]
    @WeightUnits bit
AS 

SELECT Unit_ID, Weight_Unit, Unit_Name, Unit_Abbreviation
FROM ItemUnit (NOLOCK)
WHERE Weight_Unit = @WeightUnits OR IsPackageUnit = 1
ORDER BY Unit_Name

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO