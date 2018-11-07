CREATE PROCEDURE dbo.InsertConversion
@FromUnitID int,
@ToUnitID int,
@ConversionSymbol varchar(1),
@ConversionFactor decimal(9,4)
AS 

INSERT INTO ItemConversion (FromUnit_ID, ToUnit_ID, ConversionSymbol, ConversionFactor)
VALUES (@FromUnitID, @ToUnitID, @ConversionSymbol, @ConversionFactor)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertConversion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertConversion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertConversion] TO [IRMAReportsRole]
    AS [dbo];

