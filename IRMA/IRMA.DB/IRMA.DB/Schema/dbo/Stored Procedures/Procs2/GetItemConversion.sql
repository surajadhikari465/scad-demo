CREATE PROCEDURE dbo.GetItemConversion 
@FromUnit_ID int, 
@ToUnit_ID int 
AS 

SELECT FromUnit_ID, ToUnit_ID,  ConversionSymbol, ConversionFactor 
FROM ItemConversion 
WHERE FromUnit_ID = @FromUnit_ID AND ToUnit_ID = @ToUnit_ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversion] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversion] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversion] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversion] TO [IRMAReportsRole]
    AS [dbo];

