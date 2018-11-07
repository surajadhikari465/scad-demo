CREATE PROCEDURE dbo.GetItemUnits
AS 

SELECT
    Unit_ID ,
    Unit_Name ,
    Weight_Unit ,
    User_id, 
    Unit_Abbreviation ,
    ISNULL ( LOWER ( UnitSysCode ) , '' ) AS UnitSysCode ,
    IsPackageUnit,
    PlumUnitAbbr,
    EDISysCode
FROM
    ItemUnit ( nolock )
ORDER BY
    Unit_Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnits] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnits] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnits] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnits] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemUnits] TO [IRMAReportsRole]
    AS [dbo];

