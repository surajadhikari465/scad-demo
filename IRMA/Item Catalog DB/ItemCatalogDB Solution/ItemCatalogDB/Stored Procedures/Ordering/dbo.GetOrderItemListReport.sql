SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

IF Exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[GetOrderItemListReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderItemListReport]
GO 

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetOrderItemListReport]
    @OrderHeader_ID INT ,
    @Item_ID INT ,
    @OptSort INT
AS 
    BEGIN  
	    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
        SET NOCOUNT ON  
 -- Drop the Temporary Table if already exists in the memory.  
        IF OBJECT_ID('tempdb..#tblOrderItemsListreport') IS NOT NULL 
            DROP TABLE #tblOrderItemsListreport  
  
 -- Creation of Temporary Table.  
        CREATE TABLE #tblOrderItemsListreport
            (
              LineItem VARCHAR(20) ,
              identifier VARCHAR(20) ,
              Item_Description VARCHAR(60) ,
              Package_Desc VARCHAR(50) ,
              QuantityOrdered DECIMAL(18, 4) ,
              QuantityUnit VARCHAR(50) ,
              caseprice DECIMAL(18, 4) ,
              TotalPrice DECIMAL(18, 4) ,
              Package_Desc1 VARCHAR(50) ,
              SubTeam_Name VARCHAR(50) ,
              TotalFreight VARCHAR(50) ,
              TotalHandling VARCHAR(50) ,
              UnitFreight VARCHAR(50) ,
              UnitsReceived VARCHAR(50) ,
              SubTeam_No VARCHAR(20) ,
              Team_No VARCHAR(20) ,
              Category_Name VARCHAR(55) ,
              Brand_Name VARCHAR(55) ,
              Origin_Name VARCHAR(55) ,
              Proc_Name VARCHAR(55) ,
              SustainabilityRankingAbbr VARCHAR(3) ,
              Lot_no VARCHAR(50) ,
              NextCasePrice DECIMAL(18, 4) ,
              NextLineItem VARCHAR(20) ,
              Stat_Code VARCHAR(4)
            )  
  
  -- Declare the variables to store the values returned by FETCH statement from the cursor.  
        DECLARE @OrderItem_ID VARCHAR(20)  
        DECLARE @identifier VARCHAR(20) ,
            @Item_Description VARCHAR(60) ,
            @QuantityReceived DECIMAL(18, 4)  
        DECLARE @QuantityDiscount DECIMAL(18, 4) ,
            @DiscountType INT ,
            @ReceivedItemCost DECIMAL(18, 4)  
        DECLARE @EstItemFreight VARCHAR(60) ,
            @ReceivedItemHandling VARCHAR(60) ,
            @Unit_Name VARCHAR(60)  
        DECLARE @Package_Desc1 VARCHAR(20) ,
            @Package_Desc2 VARCHAR(20) ,
            @Package_Unit VARCHAR(60)  
        DECLARE @Cost VARCHAR(60) ,
            @SubTeam_Name VARCHAR(60) ,
            @EstUnitFreight VARCHAR(60)  
        DECLARE @UnitsReceived VARCHAR(60) ,
            @SubTeam_No VARCHAR(20) ,
            @Team_No VARCHAR(20)  
        DECLARE @Category_Name VARCHAR(60) ,
            @Brand_Name VARCHAR(60) ,
            @Origin_Name VARCHAR(60)  
        DECLARE @Proc_Name VARCHAR(60) ,
            @SustainabilityRankingAbbr VARCHAR(3) ,
            @Lot_no VARCHAR(60)  
        DECLARE @LineItem INT ,
            @lLoop INT ,
            @Sum DECIMAL(18, 4) ,
            @Max DECIMAL(18, 4)  
        DECLARE @CategoryName VARCHAR(60) ,
            @Item_Desc VARCHAR(60)  
        DECLARE @caseprice DECIMAL(18, 4)  
        DECLARE @Stat_Code VARCHAR(4)  
  
  --Set variables to 0   
        SET @lLoop = 0  
  
  --  Declaration of the Cusrsor.  
  --  tfs 13389 using 3 unioned queries to get normal, noid, nord type items. since joining to the einvoicing_item table for noid/noids wasnt working right.
  
        DECLARE OrderItems_cursor CURSOR FOR  
  
        SELECT OrderItem.OrderItem_ID AS OrderItem_ID  
        , (CASE WHEN ISNULL(ItemVendor.Item_ID,'') > ''  
        THEN ItemVendor.Item_ID  
        ELSE Identifier  
        END) AS Identifier  
        , Item_Description  
        , OrderItem.QuantityReceived AS QuantityReceived  
        , OrderItem.QuantityDiscount AS QuantityDiscount  
        , OrderItem.DiscountType AS DiscountType  
        , OrderItem.ReceivedItemCost AS ReceivedItemCost  
        , ReceivedItemFreight AS EstItemFreight  -- kept this name because the report uses it - used to "estimated".   
        , OrderItem.ReceivedItemHandling AS ReceivedItemHandling  
        , ItemUnit.Unit_Name AS Unit_Name  
        , CONVERT(VARCHAR(5),CONVERT(INT, OrderItem.Package_Desc1)) AS Package_Desc1  
        , CONVERT(VARCHAR(5),CONVERT(INT, OrderItem.Package_Desc2)) AS Package_Desc2  
        , ISNULL(I.Unit_Abbreviation, 'Unit') AS Package_Unit     
        , OrderItem.Cost AS Cost  
        , SubTeam.SubTeam_Name AS SubTeam_Name  
        , ReceivedFreight AS EstUnitFreight  -- kept this name because the report uses it - used to "estimated".  
        --, OrderItem.UnitsReceived AS UnitsReceived  
        --Clean up UnitsReceived so that it fits on the report better and makes room for Sust Ranking  
        ,REPLACE(RTRIM(REPLACE(REPLACE(RTRIM(REPLACE(OrderItem.UnitsReceived,'0',' ')),' ','0'),'.',' ')),' ','.') AS UnitsReceived  
        , SubTeam.SubTeam_No AS SubTeam_No  
        , SubTeam.Team_No AS Team_No  
        , Category_Name  
        , Brand_Name  
        , ItemOrigin.Origin_Name AS Origin_Name  
        , ItemProc.Origin_Name AS Proc_Name  
        , SustainabilityRanking.RankingAbbr AS SustainabilityRankingAbbr  
        , OrderItem.Lot_no AS Lot_no  
        , NULL AS Stat_Code  
        FROM OrderHeader (NOLOCK)  
     
        INNER JOIN OrderItem (NOLOCK)  
        ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID  
        LEFT JOIN ItemUnit (NOLOCK)  
        ON OrderItem.QuantityUnit = ItemUnit.Unit_ID  
        LEFT JOIN ItemUnit I (NOLOCK)  
        ON OrderItem.Package_Unit_ID = I.Unit_ID  
        LEFT JOIN ItemOrigin (NOLOCK)  
        ON ItemOrigin.Origin_ID = OrderItem.Origin_ID  
        LEFT JOIN ItemOrigin ItemProc (NOLOCK)  
        ON ItemProc.Origin_ID = OrderItem.CountryProc_ID  
        LEFT OUTER JOIN SustainabilityRanking (NOLOCK)  
        ON SustainabilityRanking.ID = OrderItem.SustainabilityRankingID  
        LEFT JOIN ItemVendor (NOLOCK)  
        ON @Item_ID =1   
        AND ItemVendor.Item_Key = OrderItem.Item_Key  
        AND ItemVendor.Vendor_ID = OrderHeader.Vendor_ID  
        INNER JOIN Item (NOLOCK)  
        ON OrderItem.Item_Key = Item.Item_Key  
        INNER JOIN ItemIdentifier (NOLOCK)  
        ON ItemIdentifier.Item_Key = Item.Item_Key  
        AND ItemIdentifier.Default_Identifier = 1  
        LEFT JOIN ItemBrand (NOLOCK)  
        ON Item.Brand_ID = ItemBrand.Brand_ID  
        INNER JOIN ItemCategory (NOLOCK)  
        ON Item.Category_ID = ItemCategory.Category_ID  
        LEFT JOIN SubTeam (NOLOCK)  
        ON SubTeam.SubTeam_No = Transfer_To_SubTeam  
        WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID  
        --ORDER BY OrderItem.OrderItem_ID  
        UNION  
  
        --SELECT * FROM orderitem WHERE OrderHeader_ID = @OrderHeader_ID  
        SELECT NULL AS OrderItem_ID  
        , (CASE WHEN ISNULL(ItemVendor.Item_ID,'') > ''  
        THEN ItemVendor.Item_ID  
        ELSE Identifier  
        END) AS Identifier  
        , Item_Description  
        , NULL AS QuantityReceived  
        , NULL AS QuantityDiscount  
        , NULL AS DiscountType  
        , NULL AS ReceivedItemCost  
        , NULL AS EstItemFreight  -- kept this name because the report uses it - used to "estimated".   
        , NULL AS ReceivedItemHandling  
        , ItemUnit.Unit_Name AS Unit_Name  
        , CONVERT(VARCHAR(5),CONVERT(INT, item.Package_Desc1)) AS Package_Desc1  
        , CONVERT(VARCHAR(5),CONVERT(INT, item.Package_Desc2)) AS Package_Desc2  
        , ISNULL(I.Unit_Abbreviation, 'Unit') AS Package_Unit     
        , NULL AS Cost  
        , SubTeam.SubTeam_Name AS SubTeam_Name  
        , NULL AS EstUnitFreight  -- kept this name because the report uses it - used to "estimated".  
        --, OrderItem.UnitsReceived AS UnitsReceived  
        --Clean up UnitsReceived so that it fits on the report better and makes room for Sust Ranking  
        , NULL AS UnitsReceived  
        , SubTeam.SubTeam_No AS SubTeam_No  
        , SubTeam.Team_No AS Team_No  
        , Category_Name  
        , Brand_Name  
        , ItemOrigin.Origin_Name AS Origin_Name  
        , ItemProc.Origin_Name AS Proc_Name  
        , SustainabilityRanking.RankingAbbr AS SustainabilityRankingAbbr  
        , NULL AS Lot_no   
        , CASE WHEN ei.Item_Key IS NULL  
        THEN 'NOID'  
        ELSE 'NORD'  
        END AS StatCode  
        FROM einvoicing_item ei INNER JOIN   
        OrderHeader (NOLOCK) ON ei.EInvoice_id = orderheader.eInvoice_Id  
        INNER JOIN Item (NOLOCK)  
        ON ei.Item_Key = Item.Item_Key  
  
        LEFT JOIN ItemUnit (NOLOCK)  
        ON Item.retail_unit_id = ItemUnit.Unit_ID  
        LEFT JOIN ItemUnit I (NOLOCK)  
        ON Item.Package_Unit_ID = I.Unit_ID  
        LEFT JOIN ItemOrigin (NOLOCK)  
        ON ItemOrigin.Origin_ID = Item.Origin_ID  
        LEFT JOIN ItemOrigin ItemProc (NOLOCK)  
        ON ItemProc.Origin_ID = Item.CountryProc_ID  
        LEFT OUTER JOIN SustainabilityRanking (NOLOCK)  
        ON SustainabilityRanking.ID = Item.SustainabilityRankingID  
        LEFT JOIN ItemVendor (NOLOCK)  
        ON @Item_ID=1  
        AND ItemVendor.Item_Key = Item.Item_Key  
        AND ItemVendor.Vendor_ID = OrderHeader.Vendor_ID  
        INNER JOIN ItemIdentifier (NOLOCK)  
        ON ItemIdentifier.Item_Key = Item.Item_Key  
        AND ItemIdentifier.Default_Identifier = 1  
        LEFT JOIN ItemBrand (NOLOCK)  
        ON Item.Brand_ID = ItemBrand.Brand_ID  
        INNER JOIN ItemCategory (NOLOCK)  
        ON Item.Category_ID = ItemCategory.Category_ID  
        LEFT JOIN SubTeam (NOLOCK)  
        ON SubTeam.SubTeam_No = Transfer_To_SubTeam  
        WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID  
        AND (IsNotIdentifiable =1 OR IsNotOrdered=1)  
  
        UNION  
  
  
  
        SELECT NULL AS OrderItem_ID  
        , (CASE WHEN ISNULL(ItemVendor.Item_ID,'') > ''  
        THEN ItemVendor.Item_ID  
        ELSE Identifier  
        END) AS Identifier  
        , ISNULL(Item_Description, descrip) AS Item_Description  
        , NULL AS QuantityReceived  
        , NULL AS QuantityDiscount  
        , NULL AS DiscountType  
        , NULL AS ReceivedItemCost  
        , NULL AS EstItemFreight  -- kept this name because the report uses it - used to "estimated".   
        , NULL AS ReceivedItemHandling  
        , ItemUnit.Unit_Name AS Unit_Name  
        , CONVERT(VARCHAR(5),CONVERT(INT, item.Package_Desc1)) AS Package_Desc1  
        , CONVERT(VARCHAR(5),CONVERT(INT, item.Package_Desc2)) AS Package_Desc2  
        , ISNULL(I.Unit_Abbreviation, 'Unit') AS Package_Unit     
        , NULL AS Cost  
        , SubTeam.SubTeam_Name AS SubTeam_Name  
        , NULL AS EstUnitFreight  -- kept this name because the report uses it - used to "estimated".  
        --, OrderItem.UnitsReceived AS UnitsReceived  
        --Clean up UnitsReceived so that it fits on the report better and makes room for Sust Ranking  
        , NULL AS UnitsReceived  
        , SubTeam.SubTeam_No AS SubTeam_No  
        , SubTeam.Team_No AS Team_No  
        , Category_Name  
        , Brand_Name  
        , ItemOrigin.Origin_Name AS Origin_Name  
        , ItemProc.Origin_Name AS Proc_Name  
        , SustainabilityRanking.RankingAbbr AS SustainabilityRankingAbbr  
        , NULL AS Lot_no   
        , CASE WHEN ei.Item_Key IS NULL  
        THEN 'NOID'  
        ELSE 'NORD'  
        END AS StatCode  
        FROM einvoicing_item ei INNER JOIN   
        OrderHeader (NOLOCK) ON ei.EInvoice_id = orderheader.eInvoice_Id  
        LEFT JOIN Item (NOLOCK)  
        ON ei.Item_Key = Item.Item_Key  
  
        LEFT JOIN ItemUnit (NOLOCK)  
        ON Item.retail_unit_id = ItemUnit.Unit_ID  
        LEFT JOIN ItemUnit I (NOLOCK)  
        ON Item.Package_Unit_ID = I.Unit_ID  
        LEFT JOIN ItemOrigin (NOLOCK)  
        ON ItemOrigin.Origin_ID = Item.Origin_ID  
        LEFT JOIN ItemOrigin ItemProc (NOLOCK)  
        ON ItemProc.Origin_ID = Item.CountryProc_ID  
        LEFT OUTER JOIN SustainabilityRanking (NOLOCK)  
        ON SustainabilityRanking.ID = Item.SustainabilityRankingID  
        LEFT JOIN ItemVendor (NOLOCK)  
        ON @Item_ID=1  
        AND ItemVendor.Item_Key = Item.Item_Key  
        AND ItemVendor.Vendor_ID = OrderHeader.Vendor_ID  
        LEFT JOIN ItemIdentifier (NOLOCK)  
        ON ItemIdentifier.Item_Key = Item.Item_Key  
        AND ItemIdentifier.Default_Identifier = 1  
        LEFT JOIN ItemBrand (NOLOCK)  
        ON Item.Brand_ID = ItemBrand.Brand_ID  
        LEFT JOIN ItemCategory (NOLOCK)  
        ON Item.Category_ID = ItemCategory.Category_ID  
        LEFT JOIN SubTeam (NOLOCK)  
        ON SubTeam.SubTeam_No = Transfer_To_SubTeam  
        WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID  
        AND (IsNotIdentifiable =1 OR IsNotOrdered=1) AND ei.item_key IS NULL  
  
  
  
  -- Open the Cursor.  
        OPEN OrderItems_cursor  
  
  -- Fetch the Next record into corresponding Variables.  
        FETCH NEXT FROM OrderItems_cursor  
  INTO  @OrderItem_ID,@identifier,@Item_Description,@QuantityReceived,  
  @QuantityDiscount,  
  @DiscountType,@ReceivedItemCost,  
  @EstItemFreight,@ReceivedItemHandling,@Unit_Name,@Package_Desc1,  
  @Package_Desc2,@Package_Unit,@Cost,@SubTeam_Name,@EstUnitFreight,  
  @UnitsReceived,@SubTeam_No,@Team_No,  
  @Category_Name,@Brand_Name,@Origin_Name,@Proc_Name, @SustainabilityRankingAbbr, @Lot_no, @Stat_Code  
  
 -- Check the Fetch Status.  
        WHILE @@FETCH_STATUS = 0 
            BEGIN       
  
  
        -- Calculation the case Price value.  
                IF @QuantityReceived <> 0 
                    SET @caseprice = @ReceivedItemCost / @QuantityReceived  
                ELSE 
                    SET @caseprice = 0  
          
  -- Populating the data into Temporary Table with some business criteria.  
                INSERT  INTO #tblOrderItemsListreport
                        ( LineItem ,
                          identifier ,
                          Item_Description ,
                          Package_Desc ,
                          QuantityOrdered ,
                          QuantityUnit ,
                          caseprice ,
                          TotalPrice ,
                          Package_Desc1 ,
                          SubTeam_Name ,
                          TotalFreight ,
                          TotalHandling ,
                          UnitFreight ,
                          UnitsReceived ,
                          SubTeam_No ,
                          Team_No ,
                          Category_Name ,
                          Brand_Name ,
                          Origin_Name ,
                          Proc_Name ,
                          SustainabilityRankingAbbr ,
                          Lot_no ,
                          Stat_Code
                        )
                VALUES  ( @OrderItem_ID ,
                          @identifier ,
                          @Item_Description ,
                          @Package_Desc1 + '/' + @Package_Desc2 + ' '
                          + @Package_Unit ,
                          @QuantityReceived ,
                          @Unit_Name ,
                          @caseprice ,
                          @ReceivedItemCost ,
                          @Package_Desc1 ,
                          @SubTeam_Name ,
                          @EstItemFreight ,
                          0 ,
                          @EstUnitFreight ,
                          @UnitsReceived ,
                          @SubTeam_No ,
                          @Team_No ,
                          @Category_Name ,
                          @Brand_Name ,
                          @Origin_Name ,
                          @Proc_Name ,
                          @SustainabilityRankingAbbr ,
                          @Lot_no ,
                          @Stat_Code
                        )  
  
  -- Populating a new record into temporary table based on @DiscountType=3 and @QuantityDiscount>0  
                IF @DiscountType = 3
                    AND @QuantityDiscount > 0 
                    INSERT  INTO #tblOrderItemsListreport
                            ( LineItem ,
                              identifier ,
                              Item_Description ,
                              Package_Desc ,
                              QuantityOrdered ,
                              QuantityUnit ,
                              caseprice ,
                              TotalPrice ,
                              Package_Desc1 ,
                              SubTeam_Name ,
                              TotalFreight ,
                              TotalHandling ,
                              UnitFreight ,
                              UnitsReceived ,
                              SubTeam_No ,
                              Team_No ,
                              Category_Name ,
                              Brand_Name ,
                              Origin_Name ,
                              Proc_Name ,
                              SustainabilityRankingAbbr ,
                              Lot_no ,
                              Stat_Code
                            )
                    VALUES  ( @OrderItem_ID ,
                              @identifier ,
                              @Item_Description ,
                              @Package_Desc1 + '/' + @Package_Desc2 + ' '
                              + @Package_Unit ,
                              @QuantityReceived ,
                              @Unit_Name ,
                              0 ,
                              0 ,
                              @Package_Desc1 ,
                              @SubTeam_Name ,
                              0 ,
                              0 ,
                              0 ,
                              0 ,
                              @SubTeam_No ,
                              @Team_No ,
                              @Category_Name ,
                              @Brand_Name ,
                              @Origin_Name ,
                              @Proc_Name ,
                              @SustainabilityRankingAbbr ,
                              0 ,
                              @Stat_Code
                            )  
  
  -- Fetch the Next Record.  
  
                FETCH NEXT FROM OrderItems_cursor  
  INTO @OrderItem_ID,@identifier,@Item_Description,@QuantityReceived,@QuantityDiscount  
   ,@DiscountType,@ReceivedItemCost,@EstItemFreight,@ReceivedItemHandling  
   ,@Unit_Name,@Package_Desc1,@Package_Desc2,@Package_Unit,@Cost,@SubTeam_Name  
   ,@EstUnitFreight,@UnitsReceived,@SubTeam_No,@Team_No,@Category_Name  
   ,@Brand_Name,@Origin_Name,@Proc_Name,@SustainabilityRankingAbbr,@Lot_no,@Stat_Code  
   
            END  
  
  -- Closing and Deallocate the Cursor.  
        CLOSE OrderItems_cursor  
        DEALLOCATE OrderItems_cursor  
  -- End of populating into Temporary Table.  
    
  -- SortBy option Implementation.  
        IF @OptSort = 1   -- Sort By LineNumber Option.  
            BEGIN  
       --Returning the #tblOrderItemsListreport as output of the stored Procedure.  
                SELECT  LineItem ,
                        identifier ,
                        Item_Description ,
                        Package_Desc ,
                        QuantityOrdered ,
                        QuantityUnit ,
                        caseprice ,
                        TotalPrice ,
                        Package_Desc1 ,
                        SubTeam_Name ,
                        TotalFreight ,
                        TotalHandling ,
                        UnitFreight ,
                        UnitsReceived ,
                        SubTeam_No ,
                        Team_No ,
                        Category_Name ,
                        Brand_Name ,
                        Origin_Name ,
                        Proc_Name ,
                        SustainabilityRankingAbbr ,
                        Lot_no ,
                        NextCasePrice ,
                        NextLineItem ,
                        Stat_Code
                FROM    #tblOrderItemsListreport
                GROUP BY LineItem ,
                        identifier ,
                        Item_Description ,
                        Package_Desc ,
                        QuantityOrdered ,
                        QuantityUnit ,
                        caseprice ,
                        TotalPrice ,
                        Package_Desc1 ,
                        SubTeam_Name ,
                        TotalFreight ,
                        TotalHandling ,
                        UnitFreight ,
                        UnitsReceived ,
                        SubTeam_No ,
                        Team_No ,
                        Category_Name ,
                        Brand_Name ,
                        Origin_Name ,
                        Proc_Name ,
                        SustainabilityRankingAbbr ,
                        Lot_no ,
                        NextCasePrice ,
                        NextLineItem ,
                        Stat_Code  
       
            END  
        ELSE 
            BEGIN  
                IF @OptSort = 2   --Sort By LineCost.  
                    BEGIN  
                        DECLARE OrderItems_LineCost CURSOR FOR  
                        SELECT LineItem, SUM(TotalPrice)   
                        FROM #tblOrderItemsListreport   
                        GROUP BY LineItem   
                        ORDER BY SUM(TotalPrice) ASC, LineItem ASC  
  
                        OPEN OrderItems_LineCost  
                        FETCH NEXT FROM OrderItems_LineCost  
      INTO  @LineItem,@sum     
               
                        WHILE @@FETCH_STATUS = 0 
                            BEGIN  
                                SET @lLoop = @lLoop - 1   
                                UPDATE  #tblOrderItemsListreport
                                SET     LineItem = @lLoop
                                WHERE   LineItem = @LineItem  
                                FETCH NEXT FROM OrderItems_LineCost  
       INTO  @LineItem,@Sum  
                            END  
                        CLOSE OrderItems_LineCost  
                        DEALLOCATE OrderItems_LineCost  
                    END  
                ELSE 
                    IF @OptSort = 3   -- Sort By Identifier.  
                        BEGIN  
                            DECLARE OrderItems_Identifier CURSOR FOR  
                            SELECT LineItem, MAX(Identifier)   
                            FROM #tblOrderItemsListreport   
                            GROUP BY LineItem   
                            ORDER BY MAX(Identifier) DESC, LineItem ASC  
        
                            OPEN OrderItems_Identifier  
                            FETCH NEXT FROM OrderItems_Identifier  
      INTO  @LineItem,@Max     
               
                            WHILE @@FETCH_STATUS = 0 
                                BEGIN  
                                    SET @lLoop = @lLoop - 1   
                                    UPDATE  #tblOrderItemsListreport
                                    SET     LineItem = @lLoop
                                    WHERE   LineItem = @LineItem  
                                    FETCH NEXT FROM OrderItems_Identifier  
       INTO  @LineItem,@Max  
                                END  
                            CLOSE OrderItems_Identifier  
                            DEALLOCATE OrderItems_Identifier  
                        END  
                    ELSE 
                        IF @OptSort = 4  -- Sort by Category.  
                            BEGIN  
                                DECLARE OrderItems_Category CURSOR FOR  
                                SELECT LineItem, Category_Name, Item_Description   
                                FROM #tblOrderItemsListreport   
                                GROUP BY Category_Name, Item_Description, LineItem   
                                ORDER BY Category_Name DESC, Item_Description DESC, LineItem ASC  
        
                                OPEN OrderItems_Category  
                                FETCH NEXT FROM OrderItems_Category  
      INTO  @LineItem,@CategoryName,@Item_Desc     
               
                                WHILE @@FETCH_STATUS = 0 
                                    BEGIN  
                                        SET @lLoop = @lLoop - 1   
                                        UPDATE  #tblOrderItemsListreport
                                        SET     LineItem = @lLoop
                                        WHERE   LineItem = @LineItem  
                                        FETCH NEXT FROM OrderItems_Category  
       INTO  @LineItem,@CategoryName,@Item_Desc  
                                    END  
                                CLOSE OrderItems_Category  
                                DEALLOCATE OrderItems_Category  
                            END  
  
    --Returning the #tblOrderItemsListreport as output of the stored Procedure.  
                SELECT  LineItem ,
                        identifier ,
                        Item_Description ,
                        Package_Desc ,
                        QuantityOrdered ,
                        QuantityUnit ,
                        caseprice ,
                        TotalPrice ,
                        Package_Desc1 ,
                        SubTeam_Name ,
                        TotalFreight ,
                        TotalHandling ,
                        UnitFreight ,
                        UnitsReceived ,
                        SubTeam_No ,
                        Team_No ,
                        Category_Name ,
                        Brand_Name ,
                        Origin_Name ,
                        Proc_Name ,
                        SustainabilityRankingAbbr ,
                        Lot_no ,
                        NextCasePrice ,
                        NextLineItem ,
                        Stat_Code
                FROM    #tblOrderItemsListreport
                GROUP BY LineItem ,
                        identifier ,
                        Item_Description ,
                        Package_Desc ,
                        QuantityOrdered ,
                        QuantityUnit ,
                        caseprice ,
                        TotalPrice ,
                        Package_Desc1 ,
                        SubTeam_Name ,
                        TotalFreight ,
                        TotalHandling ,
                        UnitFreight ,
                        UnitsReceived ,
                        SubTeam_No ,
                        Team_No ,
                        Category_Name ,
                        Brand_Name ,
                        Origin_Name ,
                        Proc_Name ,
                        SustainabilityRankingAbbr ,
                        Lot_no ,
                        NextCasePrice ,
                        NextLineItem ,
                        Stat_Code   
       
            END  
    END  

--GRANT EXEC ON dbo.GetOrderItemListReport TO IRMAClientRole, IRMAReportsRole, IRMASchedJobsRole, IRMASupportRole
--GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO