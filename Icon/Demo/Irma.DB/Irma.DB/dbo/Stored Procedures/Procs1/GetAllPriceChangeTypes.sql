CREATE Procedure dbo.GetAllPriceChangeTypes
AS
    SELECT   PriceChgTypeDesc, PriceChgTypeID
    FROM     PriceChgType
    ORDER BY PriceChgTypeDesc