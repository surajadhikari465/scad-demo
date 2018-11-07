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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsVendor] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsVendor] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsVendor] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsVendor] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnitsVendor] TO [IRMAExcelRole]
    AS [dbo];

