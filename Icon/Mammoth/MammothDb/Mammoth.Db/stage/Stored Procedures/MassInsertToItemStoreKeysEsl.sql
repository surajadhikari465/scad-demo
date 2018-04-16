CREATE PROCEDURE stage.MassInsertToItemStoreKeysEsl
	@Region nvarchar(2),
	@BusinessUnitIds gpm.BusinessUnitIdsType READONLY

AS

	/* =============================================
	Author:		  Min Zhao
	Create date:  2018-03-05
	Description:  Mass insert ESL records based on region and stores selection into the 
	              stage.ItemStoreKeysEsl table for initial load.
	CHANGE LOG
	DEV		DATE		TASK			Description
	
	=============================================*/

	IF OBJECT_ID('tempdb..#Stores') IS NOT NULL
		TRUNCATE TABLE #Stores
	ELSE
	BEGIN
		CREATE TABLE #Stores (BusinessUnitId int);
	END

	INSERT INTO #Stores SELECT BusinessUnitID FROM @BusinessUnitIds;

	DECLARE @sql nvarchar(max);
    SET @sql = N'
	INSERT INTO [stage].[ItemStoreKeysEsl]
           ([BusinessUnitID]
           ,[ItemID]
           ,[InsertDateUtc])
     SELECT s.BusinessUnitID
           ,il.ItemID
           ,GETDATE()		
	   FROM dbo.ItemAttributes_Locale_' + @Region + ' il
	   JOIN #Stores s on il.BusinessUnitID = s.BusinessUnitID
	  WHERE il.Authorized = 1'

	EXECUTE sp_executesql @sql
GO

GRANT EXECUTE on [stage].[MassInsertToItemStoreKeysEsl] to [MammothRole]
GO
