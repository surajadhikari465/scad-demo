CREATE PROCEDURE dbo.AuditGPMPrice_NoGrouping
  @region VARCHAR(2)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	IF(NOT EXISTS(SELECT 1 FROM Regions WHERE Region = @region))
	BEGIN
		DECLARE @msg VARCHAR(100) = 'Invalid region specified ' + @region + '.';
		RAISERROR(@msg, 16, 1);
		RETURN;
	END

    declare @sql nvarchar(1024)

    set @sql = '
          SELECT B.ItemID,
               B.BusinessUnitID,
               A.ScanCode,
               Convert(varchar, P.StartDate, 23) StartDate,
               Convert(varchar, P.EndDate, 23) EndDate,
               P.Price,
               P.PercentOff,
               P.PriceType,
               P.PriceTypeAttribute AS PriceReasonCode, --price reason type modify
               P.CurrencyCode,
               P.Multiple,
               Convert(varchar, P.TagExpirationDate, 120) TagExpirationDate, 
               Convert(varchar, P.InsertDateUtc, 120) InsertDateUtc, 
               Convert(varchar, P.ModifiedDateUtc, 120) ModifiedDateUtc, 
               B.Authorized,
               P.SellableUOM
    from Items A
    inner join dbo.ItemAttributes_Locale_' + @region + ' B  on A.ItemID = B.ItemID
    inner join gpm.Price_' + @region + ' P on B.ItemId = P.ItemId  and B.BusinessUnitId = P.BusinessUnitId'

	EXEC sp_executesql @sql;
END
GO
