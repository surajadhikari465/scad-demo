CREATE PROCEDURE dbo.UpdateConversionInfo
@FromUnitID int,
@ToUnitID int,
@ConversionSymbol varchar(1),
@ConversionFactor decimal(9,4)
AS 

UPDATE ItemConversion 
SET ConversionSymbol = @ConversionSymbol,
    ConversionFactor = @ConversionFactor
WHERE FromUnit_ID = @FromUnitID AND ToUnit_ID = @ToUnitID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConversionInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConversionInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateConversionInfo] TO [IRMAReportsRole]
    AS [dbo];

