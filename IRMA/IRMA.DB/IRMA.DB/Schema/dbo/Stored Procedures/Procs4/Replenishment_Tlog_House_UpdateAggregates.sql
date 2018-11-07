
CREATE PROCEDURE dbo.Replenishment_Tlog_House_UpdateAggregates
	@Region as varchar(5),
	@UseModifiedSubteamNo as bit = 0
AS

-- **************************************************************************
-- Procedure: Replenishment_Tlog_House_UpdateAggregates(Region, UseModifiedSubteamNo)
--    Author: n/a
--      Date: n/a
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 
-- 12/29/2014	DN				6239	Modified INSERT INTO Sales_SumBySubteam... 
--										to remove the dependency for the Subteam
--										table. Subteam_No from the Item table will
--										be used instead.
-- **************************************************************************

BEGIN  
 SET NOCOUNT ON  
   
 /*  
  @UseModifiedSubteamNo has been added because SO has 4 digit subteam_no in ItemCatalog while their tlog data only has 3 digits and these must match for the join.  
  Other reigons use 3 digits in ItemCatalog already. It will be controlled by a config key from the calling code.   
  If @UseModifiedSubteamNo = 1 we will divide the Subteam.Subteam_No by 10 which should produce the needed 3 digit subteam_no.  
    
  using subteam.dept_no instead of subteam_no and making sure the proper 3 digit value was populated there was discussed. Becase this came about so late in the game  
  and there was a risk of breaking functionality for other regions it was decided that the config value route would be safer.   
 */  
   
 BEGIN TRY  
   
   
  DECLARE @StoreAbbr AS VARCHAR(10)  
  DECLARE @PosType INT, @POSSystemType AS VARCHAR(20)  
  DECLARE @AggregateDate DATETIME  
  
  SET @StoreAbbr = (SELECT TOP 1 Store_No FROM tlog_item)  
  SET @StoreAbbr = SUBSTRING(@StoreAbbr,3,3)  
  
  SELECT   
   @PosType = ISNULL((s.PosSystemId), -1),  @POSSystemType = ISNULL((p.POSSystemType), '')  
  FROM   
   dbo.Store (NOLOCK) s  
   JOIN dbo.POSSystemTypes p ON p.PosSystemId = s.PosSystemId  
  WHERE   
   StoreAbbr  = @StoreAbbr  
    
  IF @PosType = -1   
   BEGIN  
    DECLARE @msg VARCHAR(1024)  
    SET @msg = 'Could not determine POS System Type for ' + @StoreAbbr  
    RAISERROR(@msg,16,1)  
   END  
  
  ----------------------------------------------  
  -- Create a temp table of store numbers & abbr.  
  ----------------------------------------------  
  IF OBJECT_ID('tempdb..#Temp_Store') IS NOT NULL  
   DROP TABLE #Temp_Store  
     
  CREATE TABLE #Temp_Store  
  (  
   store VARCHAR(10) NULL,  
   store_no INT NULL  
  )  
    
  CREATE NONCLUSTERED INDEX IX_Store_Store_No_StoreAbbr ON #Temp_Store (Store_No, Store)  
  
  CREATE CLUSTERED INDEX CLIX_Store_StoreAbbr_StoreNo ON #Temp_Store (Store, Store_No)  
     
  INSERT INTO #Temp_Store  
  SELECT @region + storeabbr, Store_No   
  FROM Store (NOLOCK)  
    
  UPDATE TLOG_Item SET Base_Price =1 WHERE Base_Price = 0  
  
  DELETE FROM   
   Sales_SumByItem   
  WHERE   
   Date_Key IN (SELECT DISTINCT trans_date FROM tlog_item (NOLOCK)) AND  
   Store_No IN (SELECT Store_No FROM #Temp_Store WHERE Store IN (SELECT DISTINCT Store_No FROM TLOG_Item (NOLOCK)))  
  
  INSERT INTO Sales_SumByItem   
   SELECT   
    a.trans_date,   
    c.store_no,   
    b.Item_Key,   
    s.subteam_no,
    1 AS Price_Level,   
    SUM(CASE WHEN a.ITEM_TYPE IN (0)   
        THEN a.qty * (  
         CASE  WHEN a.item_void = 'Y'   
         THEN -1  
         ELSE 1 END)  
        ELSE 0 END) AS Sales_Quantity,   
    SUM(CASE WHEN a.ITEM_TYPE IN (2,3,8)  
        THEN a.qty * -1 * (  
         CASE WHEN a.item_void = 'Y'   
           THEN -1  
           ELSE 1 END)  
         ELSE 0 END) AS Return_Quantity,   
    SUM (CASE WHEN a.ITEM_TYPE IN (0)   
         THEN (CASE WHEN dbo.fn_isscaleitem(cast(cast(item_code as decimal(13,0)) as varchar(20))) =1  
           AND a.BASE_PRICE <> 0  
           THEN a.EXTENDED_PRICE / a.BASE_PRICE  
           ELSE a.WEIGHT * (  
          CASE WHEN a.item_void = 'Y'   
            THEN -1  
            ELSE 1 END)  
         END)  
         ELSE 0 END) AS Weight,   
    SUM(CASE WHEN a.ITEM_TYPE IN (0)   
        THEN a.EXTENDED_PRICE  
        ELSE 0 END) AS Sales_Amount,  
    SUM(CASE WHEN a.ITEM_TYPE IN (2,3,8)   
        THEN a.EXTENDED_PRICE  
        ELSE 0 END) AS Return_Amount,   
    ISNULL(SUM(CASE WHEN @POSSystemType <> 'NCR' THEN a.mrkdwn_amt ELSE 0 END), 0) AS Markdown_Amount,   
    0 AS Promotion_Amount,   
    0 AS Store_Coupon_Amount   
   FROM   
    tlog_item a  (NOLOCK)  
    JOIN ItemIdentifier b (NOLOCK) ON substring(a.item_code, PATINDEX('%[^0]%', a.item_code),12) = b.Identifier  
						and Deleted_Identifier = 0 
    JOIN #temp_store c ON a.store_no = c.store  
	JOIN Item s (NOLOCK) on b.Item_Key = s.Item_Key 
   WHERE   
    a.trans_void_indic='N'   
   GROUP BY   
    a.trans_date,   
    c.store_no,   
    b.Item_Key,   
    s.subteam_no 
 END TRY  
 BEGIN CATCH  
   
 ----------------------------------------------  
 -- Drop the temp table  
 ----------------------------------------------  
 IF OBJECT_ID('tempdb..#temp_store') IS NOT NULL  
  DROP TABLE #temp_store  
    
    
  Declare @error_No int  
        DECLARE @Severity smallint 
        declare @errormessage varchar(max) 
        SELECT @error_No = @@ERROR  
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_No), 16)  
        select @errormessage =  error_message()
        SET NOCOUNT OFF  
        RAISERROR ('Replenishment_Tlog_House_UpdateAggregates failed with @@ERROR: %d and @@ERROR_MESSAGE: %s ', @Severity, 1, @error_No, @errormessage)       
  
 END CATCH  
  
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_UpdateAggregates] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_Tlog_House_UpdateAggregates] TO [IRMAClientRole]
    AS [dbo];

