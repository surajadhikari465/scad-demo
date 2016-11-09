CREATE PROCEDURE dbo.GetItemConversionAll 

AS 

SELECT FromUnit_ID, ToUnit_ID,  ConversionSymbol, ConversionFactor 
FROM ItemConversion WITH(nolock)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversionAll] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversionAll] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversionAll] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemConversionAll] TO [IRMAReportsRole]
    AS [dbo];

