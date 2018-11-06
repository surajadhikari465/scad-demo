IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.GetRegularPriceChgTypeData') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE dbo.GetRegularPriceChgTypeData
GO

CREATE PROCEDURE [dbo].[GetRegularPriceChgTypeData]
AS

BEGIN
	-- Return the PriceChgType values for the REGULAR price change type.
    SELECT PriceChgTypeId, PriceChgTypeDesc, Priority, On_Sale, MSRP_Required, LineDrive, Competitive, LastUpdateTimestamp
    FROM PriceChgType
    WHERE On_Sale = 0
END
GO

 