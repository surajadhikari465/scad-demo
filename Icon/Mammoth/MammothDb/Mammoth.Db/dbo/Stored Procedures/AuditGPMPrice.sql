CREATE PROCEDURE [dbo].[AuditGPMPrice]
  @region varchar(2)
AS
BEGIN
  SET NOCOUNT ON;
 
  DECLARE @msg VARCHAR(500) = null,
          @query NVARCHAR(Max) = null;
 
  IF(NOT Exists(SELECT 1 FROM Regions WHERE Region = @region))
  BEGIN
    SET @msg = 'Invalid region specified ' + @region + '.';
    RAISERROR (@msg, 16, 1);
    RETURN;
  END
 
  SET @query = '
    SELECT p.Region,
               i.ScanCode,
               l.ItemID,
               l.BusinessUnitID,
               Cast(p.StartDate as Date) StartDate,
               p.EndDate,
               p.Price,
               p.PercentOff,
               p.PriceType,
               p.PriceTypeAttribute AS PriceReasonCode ,--price reason type modify
               p.CurrencyCode,
               p.Multiple,
               p.TagExpirationDate,
               p.InsertDateUtc,
               p.ModifiedDateUtc,
               l.Authorized,
               p.SellableUOM
      FROM [gpm].[Price_'+ @Region+ '] as p
      JOIN [dbo].[ItemAttributes_Locale_'+ @Region+'] l ON  p.ItemID = l.ItemID 
      JOIN [dbo].[Items] i ON    p.ItemID = i.ItemID' 
 
  EXEC sp_executesql @query;
  SET NOCOUNT OFF;
END