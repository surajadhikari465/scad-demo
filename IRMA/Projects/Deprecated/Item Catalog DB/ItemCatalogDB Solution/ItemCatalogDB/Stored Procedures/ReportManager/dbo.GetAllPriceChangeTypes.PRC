IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].GetAllPriceChangeTypes') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure [dbo].GetAllPriceChangeTypes
	END
GO

CREATE Procedure dbo.GetAllPriceChangeTypes
AS
    SELECT   PriceChgTypeDesc, PriceChgTypeID
    FROM     PriceChgType
    ORDER BY PriceChgTypeDesc

GO

