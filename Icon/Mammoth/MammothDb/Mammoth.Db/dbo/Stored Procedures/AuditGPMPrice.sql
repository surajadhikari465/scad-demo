--Formatted by Poor SQL
CREATE PROCEDURE dbo.AuditGPMPrice
  @action VARCHAR(25),
  @region VARCHAR(2),
	@groupSize INT = 250000,
	@groupId INT = 0
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

	DECLARE @query NVARCHAR(max);

	IF @action = 'Initilize'
	BEGIN
		SET @query = 'SELECT Count(*) FROM gpm.Price_' + @region;
		EXEC sp_executesql @query;
		RETURN;
	END

	IF @action = 'Get'
	BEGIN
		IF IsNull(@groupSize, 0) <= 0
			SET @groupSize = 250000;

		DECLARE @minId INT = (@groupId * @groupSize) + (CASE WHEN @groupID = 0 THEN 0 ELSE 1 END);

		SET @query = '
    IF (object_id(''tempdb..#group'') IS NOT NULL) DROP TABLE #group;
    IF (object_id(''tempdb..#items'') IS NOT NULL) DROP TABLE #items;

    CREATE TABLE #group (ItemID INT, BU INT, ScanCode varchar(255), Authorized bit, INDEX ix_ID NONCLUSTERED(ItemID, BU));

    ;WITH cte AS(SELECT TOP 100 PERCENT B.ItemID, B.BusinessUnitID, A.ScanCode, B.Authorized, Row_Number() OVER (ORDER BY B.ItemID ASC, B.BusinessUnitID ASC) rowID
                 FROM dbo.Items A
                 INNER JOIN dbo.ItemAttributes_Locale_' + @region + ' B ON B.ItemID = A.ItemID
                 ORDER BY B.ItemID ASC, B.BusinessUnitID ASC)
      INSERT INTO #group (ItemID, BU, ScanCode, Authorized)
      SELECT TOP(' + Cast(@groupSize AS VARCHAR(25)) + ') ItemID, BusinessUnitID, ScanCode, Authorized
	  	FROM cte
	  	WHERE rowID >= ' + Cast(@minId AS VARCHAR(25)) + ';

      SELECT B.ItemID,
           B.BU AS BusinessUnitID,
           B.ScanCode,
           Cast(A.StartDate as Date) StartDate,
           A.EndDate,
           A.Price,
           A.PercentOff,
           A.PriceType,
           A.PriceTypeAttribute AS PriceReasonCode, --price reason type modify
           A.CurrencyCode,
           A.Multiple,
           A.TagExpirationDate,
           A.InsertDateUtc,
           A.ModifiedDateUtc,
           B.Authorized,
           A.SellableUOM
      INTO #items
      FROM gpm.Price_' + @region + ' A
      JOIN #group B ON B.ItemID = A.ItemID AND B.BU = A.BusinessUnitID;

      SELECT ''' + @region + ''' AS Region, * from #items;'

		EXEC sp_executesql @query;
		RETURN;
	END

	SET NOCOUNT OFF;
	SET @msg = 'Unsupported action (' + @action + ').';
	RAISERROR(@msg, 16, 1);
END
GO

GRANT EXECUTE ON dbo.AuditGPMPrice TO [MammothRole];
GO