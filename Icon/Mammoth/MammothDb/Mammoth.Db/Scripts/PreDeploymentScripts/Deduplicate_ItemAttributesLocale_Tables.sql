SET NOCOUNT ON;
DECLARE @sql nvarchar(max),
        @region nvarchar(2),
        @name varchar(255),
        @msg varchar(255);

DECLARE cur CURSOR FOR
	SELECT DISTINCT Region FROM Regions
OPEN cur   
FETCH NEXT FROM cur INTO @region

WHILE @@FETCH_STATUS = 0  
BEGIN
  SET @name = 'ItemAttributes_Locale_' + @region;
  if(object_id(@name) is not null)
  BEGIN
    SET @msg = 'Processing ' + @name + ': Deduplicate ItemID-BusinessUnitID records...';
    RAISERROR(@msg, 0, 1) WITH NOWAIT;

    SET @sql = ';with cte as(select ItemAttributeLocaleID, Row_Number()over(partition by ItemID, BusinessUnitID order by ItemAttributeLocaleID) rowID FROM ' + @name +')
                DELETE A FROM ' + @name + ' A INNER JOIN cte B on B.ItemAttributeLocaleID = A.ItemAttributeLocaleID WHERE B.rowID > 1;';
    EXEC(@sql);
  END
  ELSE
  BEGIN
    SET @msg = 'Table ' + @name + ' does not exist!';
    RAISERROR(@msg, 0, 1) WITH NOWAIT;
  END

  FETCH NEXT FROM cur INTO @region;
END

DEALLOCATE cur;

PRINT 'Deduplication completed.';