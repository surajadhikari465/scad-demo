--PBI: 14897
DECLARE @scriptKey VARCHAR(128) = 'Truncate_IRMA_Price_Tables_14897';

IF(NOT EXISTS (SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @scriptKey))
BEGIN
  DECLARE @region VARCHAR(2),
          @sql NVARCHAR(MAX),
          @tableName VARCHAR(255);
  
  DECLARE @regions TABLE(Region varchar(2));
  
  INSERT INTO @regions(Region) VALUES
    ('FL')
    ,('MA')
    ,('MW')
    ,('NA')
    ,('NC')
    ,('NE')
    ,('PN')
    ,('RM')
    ,('SO')
    ,('SP')
    ,('SW')
    ,('TS')
    ,('UK')
  
  
  DECLARE cur CURSOR FAST_FORWARD FOR
  	SELECT Region from @regions ORDER BY Region;
  
  	OPEN cur
  
  	FETCH NEXT FROM cur INTO @region
  
  	WHILE @@FETCH_STATUS = 0
    BEGIN
      SET @tableName = 'dbo.Price_' + @region;
      
      IF(object_id(@tableName) IS NOT NULL)
      BEGIN
        PRINT 'Running script ' + @scriptKey;

        IF(@region = 'PN' OR @region = 'MW') --The regions include Canadian stores. Keep prices for Canadian stores. 
        BEGIN
          SET @sql = 'IF(object_id(''tempdb..#tmpPrice'') IS NOT NULL) DROP TABLE #tmpPrice;
                      SELECT A.* INTO #tmpPrice
                      FROM ' + @tableName + ' A
                      INNER JOIN dbo.StoreAddress B ON B.BusinessUnitID = A.BusinessUnitID
                      WHERE B.CountryAbbrev = ''CAN'';
  
                      TRUNCATE TABLE ' + @tableName + ';
  
                      INSERT INTO ' + @tableName + '(Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate)
                      SELECT Region, ItemID, BusinessUnitID, StartDate, EndDate, Price, PriceType, PriceUOM, CurrencyID, Multiple, AddedDate, ModifiedDate
                      FROM #tmpPrice
                      ORDER BY PriceID;'
  
          EXEC sp_executesql @sql;
        END
        ELSE
        BEGIN
  		    SET @sql = 'TRUNCATE TABLE ' + @tableName + ';'
  		    EXEC sp_executesql @sql;
        END
      END
  
  		FETCH NEXT FROM cur INTO @region
  	END
  
  	CLOSE cur
  	DEALLOCATE cur

    INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) VALUES (@scriptKey ,GETDATE());
END
ELSE
BEGIN
	PRINT '[' + convert(NVARCHAR, GetDate(), 121) + '] Script ' + @scriptKey + ' already applied.'
END