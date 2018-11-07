CREATE PROCEDURE dbo.GetConversionInfoFirst
AS 

SELECT FromUnit_ID, ToUnit_ID, ConversionSymbol, ConversionFactor 
FROM ItemConversion
WHERE ToUnit_ID = (SELECT MIN(ToUnit_ID) FROM ItemConversion) AND 
      FromUnit_ID = (SELECT MIN(FromUnit_ID) FROM ItemConversion IC WHERE ToUnit_ID = ItemConversion.ToUnit_ID)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfoFirst] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfoFirst] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfoFirst] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetConversionInfoFirst] TO [IRMAReportsRole]
    AS [dbo];

