CREATE PROCEDURE [stage].[MassInsertToItemStoreKeysEPlum]
    @Region varchar(2),
	@BusinessUnitIds gpm.BusinessUnitIdsType READONLY
AS
BEGIN

	/* =============================================
	Author:		  Min Zhao
	Create date:  2018-03-02
	Description:  Mass insert eplum records based on region and stores selection into the 
	              stage.ItemStoreKeysEPlum table for initial load.
	CHANGE LOG
	DEV		DATE		TASK			Description
	
	=============================================*/

	IF OBJECT_ID('tempdb..#Stores') IS NOT NULL
		TRUNCATE TABLE #Stores
	ELSE
	BEGIN
		CREATE TABLE #Stores (BusinessUnitID int);
	END

	INSERT INTO #Stores SELECT BusinessUnitID FROM @BusinessUnitIds;

	DECLARE @sql nvarchar(max);
    SET @sql = N'

	INSERT INTO stage.ItemStoreKeysEPlum
           (BusinessUnitID
           ,ItemID
           ,InsertDateUtc)
     SELECT s.BusinessUnitID
           ,il.ItemID
           ,GETDATE()		
	   FROM dbo.ItemAttributes_Locale_' + @Region + ' il
	   JOIN #Stores s on il.BusinessUnitID = s.BusinessUnitID
	   JOIN dbo.Locales_' + @Region + ' l on il.BusinessUnitID = l.BusinessUnitID 
       JOIN dbo.ItemAttributes_Locale_' + @Region + '_Ext ie on il.ItemID = ie.ItemID and ie.LocaleID = l.LocaleID
       JOIN dbo.Attributes a on ie.AttributeID = a.AttributeID
      WHERE il.Authorized = 1
        AND a.AttributeDesc = ''CFS Send to Scale''
        AND ie.AttributeValue = ''True''
	  UNION
	 SELECT s.BusinessUnitID
           ,il.ItemID
           ,GETDATE()
       FROM dbo.ItemAttributes_Locale_' + @Region + ' il
       JOIN #Stores s on il.BusinessUnitID = s.BusinessUnitID
	  WHERE Authorized = 1
	    AND il.ScaleItem = 1'
	
	EXECUTE sp_executesql @sql
END
GO

GRANT EXECUTE on [stage].[MassInsertToItemStoreKeysEPlum] to [MammothRole]
GO