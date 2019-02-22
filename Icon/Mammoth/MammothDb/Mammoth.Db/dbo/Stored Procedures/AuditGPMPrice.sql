CREATE PROCEDURE dbo.AuditGPMPrice
  @action VARCHAR(25),
  @region VARCHAR(2),
  @groupSize INT = 250000,
  @maxRows INT = 0,
  @groupId INT = NULL
AS
BEGIN
  SET NOCOUNT ON;
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

  IF(NOT Exists(SELECT 1 FROM Regions WHERE Region = @region))
  BEGIN
    DECLARE @msg varchar(100) = 'Invalid region specified ' + @region + '.';
    RAISERROR (@msg, 16, 1);
    RETURN;
  END

  

  IF @action = 'Initilize'
  BEGIN
    IF IsNull(@MaxRows, 0) <= 0 
	  	SET @MaxRows =  2147483647 --max int
    
    IF IsNull(@groupSize, 0) <= 0 
      SET @groupSize = 250000;
    
    TRUNCATE TABLE stage.GPMPriceExport;

    DECLARE @query nvarchar(max) = '
    INSERT INTO stage.GPMPriceExport
    SELECT TOP (' + cast(@maxRows AS NVARCHAR(10)) + ')
           p.Region,
           l.ItemID,
           l.BusinessUnitID,
           i.ScanCode,
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
           p.SellableUOM,
           0 GroupID
    FROM gpm.Price_' + @region + ' p
    JOIN dbo.ItemAttributes_Locale_' + @region + ' l ON p.ItemID = l.ItemID AND p.BusinessUnitID = l.BusinessUnitID
    JOIN dbo.Items i ON p.ItemID = i.ItemID;'
    
    EXEC sp_executesql @query;
    
    --Assign batches based on @GroupSize
    ;WITH cte AS(SELECT ItemId, GroupId,
	  		         (RANK() OVER (ORDER BY Itemid, BusinessUnitID) - 1) / @GroupSize [CalculatedGroupId]
	  	           FROM stage.GPMPriceExport)
	    UPDATE cte SET GroupId = cte.CalculatedGroupId;
    
    SELECT COUNT(*) AS [rowCount] FROM stage.GPMPriceExport;
    RETURN;
  END

  IF @action = 'Get'
  BEGIN
    SELECT A.* FROM stage.GPMPriceExport A WHERE A.GroupId = IsNull(@groupId, A.GroupId);
    RETURN;
  END

  SET NOCOUNT OFF;

  SET @msg = 'Unsupported action (' + @action + ').';
  RAISERROR (@msg, 16, 1);
  RETURN;
END
GO

GRANT EXECUTE ON dbo.AuditGPMPrice TO [MammothRole];
GO