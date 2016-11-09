CREATE  PROCEDURE [dbo].[UpdateItemHistoryFromSales]    
    @Date datetime  ,  
    @StoreNo int  
AS    
BEGIN
    SET nocount ON

    IF Object_id('tempdb..#ItemHistorySalesTemp') IS NOT NULL
      DROP TABLE #itemhistorysalestemp

    CREATE TABLE #itemhistorysalestemp
      (
         store_no                   INT,
         item_key                   INT,
         datestamp                  DATETIME,
         quantity                   NUMERIC(18, 4),
         weight                     NUMERIC(18, 4),
         cost                       MONEY,
         extcost                    MONEY,
         retail                     MONEY,
         adjustment_id              INT,
         adjustmentreason           VARCHAR(100),
         createdby                  INT,
         subteam_no                 INT,
         insert_date                DATETIME,
         orderitem_id               INT,
         inventoryadjustmentcode_id INT,
         correctionrecordflag       BIT
      )

    INSERT INTO #itemhistorysalestemp
                (store_no,
                 item_key,
                 datestamp,
                 quantity,
                 weight,
                 adjustment_id,
                 createdby,
                 subteam_no,
                 extcost,
                 retail)
    SELECT store_no,
           item_key,
           rundate,
           CASE
             WHEN Isnull(weight_unit, 0) = 0
                  AND costedbyweight = 0 THEN itemsalesqty
             ELSE 0
           END                   AS Quantity,
           weight,
           3                     Adjustment_ID,
           createdby,
           subteam_no,
           extcost,
           amount / itemsalesqty AS Retail
    FROM   (SELECT ssbi.store_no,
                   ssbi.item_key,
                   @Date                      runDate,
                   iu.weight_unit,
                   i.costedbyweight,
                   Sum(CASE
                         WHEN Isnull(iu.weight_unit, 0) > 0 THEN ssbi.weight
                         ELSE
                           CASE
                             WHEN i.costedbyweight = 1 THEN
                             ssbi.sales_quantity *
                             dbo.Fn_getaverageunitweightbyitemkey(i.item_key)
                             ELSE 0
                           END
                       END)                   AS Weight,
                   0                          AS CreatedBy,
                   ssbi.subteam_no,
                   Sum(ssbi.sales_amount - ssbi.return_amount -
                       ssbi.markdown_amount - ssbi.promotion_amount) Amount,
                   Sum(dbo.Fn_itemsalesqty2(i.item_key, iu.weight_unit,
                       ssbi.price_level,
                           ssbi.sales_quantity, ssbi.return_quantity, Cast(
                       dbo.Fn_getvendorpacksize(i.item_key, siv.vendor_id,
                       ssbi.store_no,
                       Getdate()
                       ) AS DECIMAL
                       (9, 4)), i.package_desc2, ssbi.weight, ssbi.salesamount,
                       ssbi.unitprice))
                                              ItemSalesQty,
                   Isnull(dbo.Fn_avgcosthistory(ssbi.item_key, ssbi.store_no,
                          ssbi.subteam_no,
                          Getdate()), 0)      AS ExtCost
            FROM   (SELECT ssbi1.*,
                           Isnull(dbo.Fn_pricehistoryprice(ssbi1.item_key,
                                  ssbi1.store_no,
                                  ssbi1.date_key),
                           dbo.Fn_price(dbo.Fn_onsale(p1.pricechgtypeid),
                           p1.multiple,
                           p1.price,
                           p1.pricingmethod_id,
                           p1.sale_multiple, p1.sale_price)) AS UnitPrice,
                           ssbi1.sales_amount - ssbi1.return_amount -
                           ssbi1.markdown_amount
                           -
                           ssbi1.promotion_amount            AS SalesAmount
                    FROM   sales_sumbyitem ssbi1(nolock)
                           LEFT OUTER JOIN price p1 (nolock)
                                        ON p1.item_key = ssbi1.item_key
                                           AND p1.store_no = ssbi1.store_no
                    WHERE  ssbi1.date_key >= @Date
                           AND ssbi1.date_key < Dateadd(day, 1, @Date)
                           AND ssbi1.store_no = @StoreNo) ssbi
                   INNER JOIN item i (nolock)
                           ON i.item_key = ssbi.item_key
                   INNER JOIN itemunit iu (nolock)
                           ON iu.unit_id = i.retail_unit_id
                   INNER JOIN itemidentifier ii (nolock)
                           ON ii.item_key = i.item_key
                              AND ii.default_identifier = 1
                   INNER JOIN price p (nolock)
                           ON p.item_key = ssbi.item_key
                              AND p.store_no = ssbi.store_no
                   INNER JOIN storeitemvendor siv (nolock)
                           ON siv.item_key = ssbi.item_key
                              AND siv.primaryvendor = 1
                              AND siv.store_no = ssbi.store_no
            WHERE  ssbi.store_no = @StoreNo
            GROUP  BY ssbi.item_key,
                      ssbi.subteam_no,
                      ssbi.store_no,
                      iu.weight_unit,
                      i.costedbyweight) subquery
    WHERE  itemsalesqty <> 0
   

    IF @@ERROR = 0
      BEGIN try
          BEGIN TRAN

          DELETE itemhistory
          FROM   itemhistory ih
          WHERE  adjustment_id IN ( 3, 9, 10 )
                 AND datestamp >= @Date
                 AND datestamp < Dateadd(day, 1, @Date)
                 AND ih.store_no = @StoreNo

          -- Record all as sales    
          INSERT INTO itemhistory
                      (store_no,
                       item_key,
                       datestamp,
                       quantity,
                       weight,
                       Adjustment_ID,
                       createdby,
                       subteam_no,
                       extcost,
                       retail)
          SELECT store_no,
                 item_key,
                 datestamp,
                 quantity,
                 weight,
                 Adjustment_ID,
                 createdby,
                 subteam_no,
                 extcost,
                 retail
          FROM   #itemhistorysalestemp

          IF Object_id('tempdb..#ItemHistorySalesTemp') IS NOT NULL
            DROP TABLE #itemhistorysalestemp

          COMMIT
      END try

    BEGIN catch
        ROLLBACK

        DECLARE @Severity SMALLINT

        SELECT @Severity = Isnull((SELECT severity
                                   FROM   master.dbo.sysmessages
                                   WHERE  error = @@ERROR), 16)

        SET nocount OFF

        RAISERROR ('UpdateItemHistoryFromSales failed with @@ERROR: %d',
                   @Severity,1,
                   @@ERROR)
    END catch
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemHistoryFromSales] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemHistoryFromSales] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemHistoryFromSales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemHistoryFromSales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemHistoryFromSales] TO [IRMAReportsRole]
    AS [dbo];

