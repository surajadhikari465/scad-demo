CREATE PROCEDURE dbo.GetConversionInfo
@FromUnitID int,
@ToUnitID int
AS 

SELECT FromUnit_ID, ToUnit_ID, ConversionSymbol, ConversionFactor 
FROM ItemConversion 
WHERE FromUnit_ID = @FromUnitID AND ToUnit_ID = @ToUnitID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfo] TO [IRMAReportsRole]
    AS [dbo];

