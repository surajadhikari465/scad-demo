CREATE PROCEDURE dbo.[InsertPromoPlanner]
        @upcno  char(13),
        @salPrc decimal(8,4),
        @salCst decimal(8,4),
        @salPerc int,
        @cstPerc int,
        @com1 varchar(50),
        @vndr int,
        @bbk int,
        @strtDt smalldatetime,
        @endDt smalldatetime,
        @pm int,
        @com2 varchar(50),
        @qty int
AS
        --EDV items
BEGIN
    SET NOCOUNT ON

        If @salPrc = 0 And @salCst = 0 And @cstPerc = 0 And @salPerc = 0 And @vndr = 0

  BEGIN
            Insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple,Sale_Price,Sale_Cost,
                        Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId, @strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple, o.Price as Sale_Price,
                        vch.UnitCost as Sale_Cost, @endDt as Sale_End_Date, i.Identifier as Identifier, h.SubTeam_No as Dept_No,
                        c.Vendor_ID as Vendor_ID , @qty as ProjUnits,@com1 as comment1,@com2 as comment2, @bbk as billback
                        from StoreItemVendor c, Price o, Item h, ItemIdentifier i, PriceChgType p, VendorCostHistory vch
                        where (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0'+ @upcno)
                        and vch.StoreItemVendorID = c.StoreItemVendorID and
                        vch.vendorcosthistoryid = (select top 1 vendorcosthistoryid from vendorcosthistory where StoreItemVendorID = c.storeitemvendorid
and startdate < getdate() and enddate > getdate() order by vendorcosthistoryID desc)
                        and i.Item_Key = o.Item_Key and i.Item_Key = c.Item_Key and i.Item_Key = h.Item_Key and i.Item_Key = c.Item_Key
                        and p.PriceChgTypeDesc = 'EDV' and c.PrimaryVendor = 1 group by i.Item_Key ,o.Store_No,
                        p.PriceChgTypeId, o.Multiple , o.Price ,vch.UnitCost, i.Identifier, h.SubTeam_No, c.Vendor_ID;

            --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1,
                        --StartDate, EndDate, FromVendor, MSRP, InsertDate, InsertWorkStation)
                        --SELECT a.StoreItemVendorID, 1 AS Expr1, @salcst , 0 AS Expr8, i.Package_Desc1, @strtDt AS Expr2,
                        --@endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5, GETDATE() AS Expr6, 'O' AS Expr7 FROM
                        --dbo.VendorCostHistory AS b INNER JOIN dbo.StoreItemVendor AS a
                        --ON b.StoreItemVendorID = a.StoreItemVendorID CROSS JOIN dbo.Item AS i
                        --WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE()) and
                        --i.item_key = a.item_key and a.primaryvendor = 1 and
                        --i.Item_Key = (SELECT     Item_Key FROM dbo.ItemIdentifier WHERE
                        --(SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));
         END
                ELSE
        --actual price--actual cost--Vendor_ID
        If @salPrc = 0 And @salCst = 0 And @cstPerc = 0 And @salPerc = 0 And @vndr <> 0
BEGIN
             insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple, Sale_Price,
                        Sale_Cost, Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId,
                        @strtDt as Start_Date, o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple,
                        o.Price as Sale_Price, vch.UnitCost as Sale_Cost, @endDt  as Sale_End_Date, i.Identifier as Identifier,
                        h.SubTeam_No as Dept_No, c.Vendor_ID as Vendor_ID , @qty as ProjUnits,@com1 as comment1,
                        @com2 as comment2,@bbk as billback from StoreItemVendor c, Price o, Item h, ItemIdentifier i,
                        PriceChgType p, VendorCostHistory vch where
                        (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and vch.StoreItemVendorID = c.StoreItemVendorID and
                        vch.vendorcosthistoryid = (select top 1 vendorcosthistoryid from vendorcosthistory where StoreItemVendorID = c.storeitemvendorid
and startdate < getdate() and enddate > getdate() order by vendorcosthistoryID desc)
                        and
                        i.Item_Key = o.Item_Key and i.Item_Key = h.Item_Key and p.PriceChgTypeDesc = 'EDV'
                        group by i.Item_Key ,o.Store_No, p.PriceChgTypeId, o.Multiple , o.Price ,vch.UnitCost, i.Identifier,
                        h.SubTeam_No,c.Vendor_ID;

            --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate,
                        --FromVendor, MSRP, InsertDate, InsertWorkStation) SELECT a.StoreItemVendorID, 1 AS Expr1,@salCst,
                        --0 AS Expr8, i.Package_Desc1, @strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5,
                        --GETDATE() AS Expr6, 'O' AS Expr7
                        --FROM dbo.VendorCostHistory AS b INNER JOIN dbo.StoreItemVendor AS a
                        --ON b.StoreItemVendorID = a.StoreItemVendorID CROSS JOIN dbo.Item AS i
                        --WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE()) and i.item_key = a.item_key
                        --and a.Vendor_ID = @vndr and  i.Item_Key =
                        --(SELECT     Item_Key FROM dbo.ItemIdentifier WHERE
                        --(SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));

            End
ELSE
        --go through combinations of fields populated on form and determine where to pull info and where to use
        --populated values when entering item into PriceBatchPromo
        --1 - %off retail--%off cost--no Vendor_ID
        If @salPrc = 0 And @salCst = 0 And @cstPerc <> 0 And @salPerc <> 0 And @vndr = 0
BEGIN
            insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple, Sale_Price,Sale_Cost,
                        Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId,@strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple,
                        o.Price-(o.Price*(0.01 *@salPerc)) as Sale_Price,
                        vch.UnitCost-(vch.UnitCost*(0.01*@cstPerc)) as Sale_Cost,
                        @endDt  as Sale_End_Date,
                        i.Identifier as Identifier, h.SubTeam_No as Dept_No, c.Vendor_ID as Vendor_ID ,
                        @qty as ProjUnits,@com1 as comment1,@com2 as comment2,@bbk as billback
                        from StoreItemVendor c, Price o, Item h, ItemIdentifier i, PriceChgType p, VendorCostHistory vch
                        where (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and c.StoreItemVendorID = vch.StoreItemVendorID and
                        vch.vendorcosthistoryid = (select top 1 vendorcosthistoryid from vendorcosthistory where StoreItemVendorID = c.storeitemvendorid
and startdate < getdate() and enddate > getdate() order by vendorcosthistoryID desc)
                         and
                         i.Item_Key = o.Item_Key and i.Item_Key = h.Item_Key and i.Item_Key = c.Item_Key
                        and p.PriceChgTypeDesc = 'SAL' and o.Store_no = c.Store_no and c.PrimaryVendor = 1 group by i.Item_Key ,
                        o.Store_No, p.PriceChgTypeId, o.Multiple , o.Price,vch.UnitCost , i.Identifier, h.SubTeam_No, c.Vendor_ID;

                        --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate,
                        --FromVendor, MSRP, InsertDate, InsertWorkStation) SELECT a.StoreItemVendorID, 1 AS Expr1,
                        --b.UnitCost-(b.UnitCost*(0.01*@cstPerc)), 0 AS Expr8, i.Package_Desc1,
                        --@strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5, GETDATE() AS Expr6,
                        --'O' AS Expr7 FROM dbo.VendorCostHistory AS b INNER JOIN dbo.StoreItemVendor AS a
                        --ON b.StoreItemVendorID = a.StoreItemVendorID CROSS JOIN dbo.Item AS i
                        --WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE()) and i.item_key = a.item_key
                        --and a.primaryvendor = 1 and  i.Item_Key = (
                        --SELECT     Item_Key FROM dbo.ItemIdentifier WHERE
                        --(SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));
                 End
ELSE

        --%off retail--%off cost--Vendor_ID
        If @salPrc = 0 And @salCst = 0 And @cstPerc <> 0 And @salPerc <> 0 And @vndr <> 0
BEGIN
            insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple, Sale_Price,Sale_Cost,
                        Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId,@strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple,
                        o.Price-(o.Price*(0.01 *@salPerc)) as Sale_Price,
                        vch.UnitCost-(vch.UnitCost*(0.01*@cstPerc)) as Sale_Cost,@endDt  as Sale_End_Date,
                        i.Identifier as Identifier, h.SubTeam_No as Dept_No, @vndr as Vendor_ID ,
                        @qty as ProjUnits,@com1 as comment1,@com2 as comment2,@bbk as billback
                        from Price o, Item h, ItemIdentifier i, PriceChgType p, VendorCostHistory vch , StoreItemVendor c
                        where (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and c.Vendor_ID = @vndr
                        and vch.StoreItemVendorID = c.StoreItemVendorID and
                        vch.vendorcosthistoryid = (select top 1 vendorcosthistoryid from vendorcosthistory where StoreItemVendorID = c.storeitemvendorid
and startdate < getdate() and enddate > getdate() order by vendorcosthistoryID desc)
                          and
                         i.Item_Key = o.Item_Key and i.Item_Key = c.Item_Key and i.Item_Key = h.Item_Key and p.PriceChgTypeDesc = 'SAL'
                        group by i.Item_Key ,o.Store_No, p.PriceChgTypeId,vch.UnitCost, o.Multiple , o.Price ,
                        i.Identifier, h.SubTeam_No;

            --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate,
                        --EndDate, FromVendor, MSRP, InsertDate, InsertWorkStation)
                        --SELECT a.StoreItemVendorID, 1 AS Expr1,b.UnitCost-(b.UnitCost*(0.01*@cstPerc)),
                        --0 AS Expr8, i.Package_Desc1, @strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5,
                        --GETDATE() AS Expr6, 'O' AS Expr7 FROM dbo.VendorCostHistory AS b
                        --INNER JOIN dbo.StoreItemVendor AS a ON b.StoreItemVendorID = a.StoreItemVendorID
                        --CROSS JOIN dbo.Item AS i WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE())
                        --and i.item_key = a.item_key and  a.Vendor_ID = @vndr  and  i.Item_Key =
                        --(SELECT     Item_Key FROM dbo.ItemIdentifier WHERE
                        --(SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));

        End
ELSE

        --2-%off retail--actual cost--no Vendor_ID
        If @salPrc = 0 And @salCst <> 0 And @vndr = 0
BEGIN
           insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple, Sale_Price,Sale_Cost,
                        Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId,@strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple,
                        o.Price-(o.Price*(0.01 *@salPerc)) as Sale_Price, @salCst as Sale_Cost, @endDt  as Sale_End_Date,
                        i.Identifier as Identifier, h.SubTeam_No as Dept_No, c.Vendor_ID as Vendor_ID ,
                        @qty as ProjUnits,@com1 as comment1,@com2 as comment2,@bbk as billback
                        from StoreItemVendor c, Price o, Item h, ItemIdentifier i, PriceChgType p, VendorCostHistory vch
                        where (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and vch.StoreItemVendorID = c.StoreItemVendorID and
                        vch.vendorcosthistoryid = (select top 1 vendorcosthistoryid from vendorcosthistory where StoreItemVendorID = c.storeitemvendorid
and startdate < getdate() and enddate > getdate() order by vendorcosthistoryID desc)
                         and
                         i.Item_Key = o.Item_Key and i.Item_Key = h.Item_Key and i.Item_Key = c.Item_Key
                        and p.PriceChgTypeDesc = 'SAL' and c.PrimaryVendor = 1
                        group by i.Item_Key ,o.Store_No, p.PriceChgTypeId, o.Multiple , o.Price ,
                        i.Identifier, h.SubTeam_No, c.Vendor_ID;

                        --INSERT INTO dbo.VendorCostHistory (StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate, FromVendor, MSRP, InsertDate, InsertWorkStation) SELECT a.StoreItemVendorID, 1 AS Expr1,@salCst, 0 AS Expr8, i.Package_Desc1, @strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5, GETDATE() AS Expr6, 'O' AS Expr7 FROM dbo.VendorCostHistory AS b INNER JOIN dbo.StoreItemVendor AS a ON b.StoreItemVendorID = a.StoreItemVendorID CROSS JOIN dbo.Item AS i WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE()) and i.item_key = a.item_key and a.primaryvendor = 1 and  i.Item_Key = (SELECT     Item_Key FROM dbo.ItemIdentifier WHERE      (SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));
     End
ELSE
        --%off retail--actual cost-- Vendor_ID
        If @salPrc = 0 And @salCst <> 0 And @vndr <> 0
BEGIN
           insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple, Sale_Price,Sale_Cost,
                        Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId,@strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple,
                        o.Price-(o.Price*(0.01 *@salPerc)) as Sale_Price,  @salCst as Sale_Cost,@endDt  as Sale_End_Date,
                        i.Identifier as Identifier, h.SubTeam_No as Dept_No, @vndr as Vendor_ID ,
                        @qty as ProjUnits,@com1 as comment1,@com2 as comment2,@bbk as billback
                        from Price o, Item h, ItemIdentifier i, PriceChgType p
                        where (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and i.Item_Key = o.Item_Key and i.Item_Key = h.Item_Key and p.PriceChgTypeDesc = 'SAL'
                        group by i.Item_Key ,o.Store_No, p.PriceChgTypeId, o.Multiple , o.Price , i.Identifier,
                        h.SubTeam_No;

                        --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate,
                        --EndDate, FromVendor, MSRP, InsertDate, InsertWorkStation)
                        --SELECT a.StoreItemVendorID, 1 AS Expr1,@salCst, 0 AS Expr8, i.Package_Desc1,
                        --@strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5, GETDATE() AS Expr6, 'O' AS Expr7
                        --FROM dbo.VendorCostHistory AS b INNER JOIN dbo.StoreItemVendor AS a
                        --ON b.StoreItemVendorID = a.StoreItemVendorID CROSS JOIN dbo.Item AS i
                        --WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE()) and i.item_key = a.item_key and
                        --a.Vendor_ID = @vndr and  i.Item_Key = (SELECT     Item_Key FROM dbo.ItemIdentifier
                        --WHERE      (SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));
        End
ELSE


        --3-actual retail--%off cost--no Vendor_ID
        If @salPrc <> 0 And @salCst = 0 And @vndr = 0
BEGIN
          insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple, Sale_Price,Sale_Cost,
                        Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId,@strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple, @salPrc as Sale_Price,
                        vch.UnitCost-(vch.UnitCost*(0.01*@cstPerc)) as Sale_Cost,
                        @endDt  as Sale_End_Date, i.Identifier as Identifier, h.SubTeam_No as Dept_No,
                        c.Vendor_ID as Vendor_ID , @qty as ProjUnits,@com1 as comment1,@com2 as comment2,
                        @bbk as billback from StoreItemVendor c, Price o, Item h, ItemIdentifier i, PriceChgType p, VendorCostHistory vch
                        where (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and vch.StoreItemVendorID = c.StoreItemVendorID and
                        vch.vendorcosthistoryid = (select top 1 vendorcosthistoryid from vendorcosthistory where StoreItemVendorID = c.storeitemvendorid
and startdate < getdate() and enddate > getdate() order by vendorcosthistoryID desc)
                         and
                        i.Item_Key = o.Item_Key and i.Item_Key = h.Item_Key and i.Item_Key = c.Item_Key and
                        p.PriceChgTypeDesc = 'SAL' and c.PrimaryVendor = 1 group by i.Item_Key , o.Store_No,
                        p.PriceChgTypeId, o.Multiple , o.Price , i.Identifier, vch.UnitCost, h.SubTeam_No, c.Vendor_ID;

            --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate,
                        --EndDate, FromVendor, MSRP, InsertDate, InsertWorkStation)
                        --SELECT a.StoreItemVendorID, 1 AS Expr1,b.UnitCost-(b.UnitCost*(0.01*@cstPerc)),
                        --0 AS Expr8, i.Package_Desc1, @strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5,
                        --GETDATE() AS Expr6, 'O' AS Expr7 FROM dbo.VendorCostHistory AS b
                        --INNER JOIN dbo.StoreItemVendor AS a ON b.StoreItemVendorID = a.StoreItemVendorID
                        --CROSS JOIN dbo.Item AS i WHERE (b.EndDate > GETDATE())
                        --AND (b.StartDate < GETDATE()) and i.item_key = a.item_key and a.primaryvendor = 1
                        --and  i.Item_Key = (SELECT     Item_Key FROM dbo.ItemIdentifier WHERE
                        --(SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));

            End

            --actual retail--%off cost-- Vendor_ID
        If @salPrc <> 0 And @salCst = 0 And @vndr <> 0
BEGIN
            insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple,
                        Sale_Price,Sale_Cost, Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        Select i.Item_Key as Item_Key , o.Store_No, p.PriceChgTypeId,@strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple, @salPrc as Sale_Price,
                        vch.UnitCost-(vch.UnitCost*(0.01*@cstPerc)),@endDt  as Sale_End_Date, i.Identifier as Identifier, h.SubTeam_No as Dept_No, @vndr as Vendor_ID ,
                        @qty as ProjUnits,@com1 as comment1,@com2 as comment2,@bbk as billback
                        from Price o, Item h, ItemIdentifier i, PriceChgType p, VendorCostHistory vch, StoreItemVendor c
                        where (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and vch.StoreItemVendorID = c.StoreItemVendorID and
                        c.Vendor_ID = @vndr and
                        vch.vendorcosthistoryid = (select top 1 vendorcosthistoryid from vendorcosthistory where StoreItemVendorID = c.storeitemvendorid
and startdate < getdate() and enddate > getdate() order by vendorcosthistoryID desc)
                        and i.Item_Key = o.Item_Key and i.Item_Key = c.Item_Key and i.Item_Key = h.Item_Key and p.PriceChgTypeDesc = 'SAL'
                        group by i.Item_Key , o.Store_No,p.PriceChgTypeId, o.Multiple , o.Price , vch.UnitCost, i.Identifier,
                        h.SubTeam_No;

            --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate,
                        --EndDate, FromVendor, MSRP, InsertDate, InsertWorkStation)
                        --SELECT a.StoreItemVendorID, 1 AS Expr1,b.UnitCost-(b.UnitCost*(0.01*@cstPerc)),
                        --0 AS Expr8, i.Package_Desc1, @strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4,
                        --' ' AS Expr5, GETDATE() AS Expr6, 'O' AS Expr7 FROM dbo.VendorCostHistory AS b
                        --INNER JOIN dbo.StoreItemVendor AS a ON b.StoreItemVendorID = a.StoreItemVendorID
                        --CROSS JOIN dbo.Item AS i WHERE (b.EndDate > GETDATE())
                        --AND (b.StartDate < GETDATE()) and i.item_key = a.item_key and a.Vendor_ID = @vndr
                        --and  i.Item_Key = (SELECT     Item_Key FROM dbo.ItemIdentifier WHERE
                        --(SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));
            End

                    --4--actual price--actual cost--no Vendor_ID
            If @salPrc <> 0 And @salCst <> 0 And @vndr = 0
BEGIN
                         insert into PriceBatchPromo
                        (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price, Sale_Multiple, Sale_Price,Sale_Cost,
                        Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1, Comment2, Billback)
                        SELECT i.Item_Key, o.Store_No, p.PriceChgTypeID, @strtDt AS Start_Date, o.Multiple, o.Price,
                        @pm AS Sale_Multiple, @salPrc AS Sale_Price,@salCst as Sale_Cost, @endDt AS Sale_End_Date, i.Identifier,
                        h.SubTeam_No AS Dept_No, c.Vendor_ID, @qty AS ProjUnits, @com1 AS comment1, @com2 AS comment2,
                        1 AS billback FROM ItemIdentifier AS i INNER JOIN Price AS o ON i.Item_Key = o.Item_Key
                        INNER JOIN Item AS h ON i.Item_Key = h.Item_Key INNER JOIN StoreItemVendor AS c
                        ON i.Item_Key = c.Item_Key and o.store_no = c.store_no CROSS JOIN PriceChgType AS p
                        WHERE (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        AND (p.PriceChgTypeDesc = 'SAL') AND (c.PrimaryVendor = 1)
                        GROUP BY i.Item_Key, o.Store_No, p.PriceChgTypeID, o.Multiple, o.Price, i.Identifier,
                        h.SubTeam_No, c.Vendor_ID;

            --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate,
                        --FromVendor, MSRP, InsertDate, InsertWorkStation)
                        --SELECT a.StoreItemVendorID, 1 AS Expr1,@salCst, 0 AS Expr8, i.Package_Desc1, @strtDt AS Expr2,
                        --@endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5, GETDATE() AS Expr6, 'O' AS Expr7
                        --FROM dbo.VendorCostHistory AS b INNER JOIN dbo.StoreItemVendor AS a
                        --ON b.StoreItemVendorID = a.StoreItemVendorID CROSS JOIN dbo.Item AS i
                        --WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE()) and i.item_key = a.item_key
                        --and a.primaryvendor = 1 and  i.Item_Key = (SELECT     Item_Key FROM dbo.ItemIdentifier
                        --WHERE      (SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));
            End
            --actual price--actual cost--Vendor_ID
            If @salPrc <> 0 And @salCst <> 0 And @vndr <> 0
            BEGIN
            insert into PriceBatchPromo (Item_Key, Store_No, PriceChgTypeID, Start_Date, Multiple, Price,
                        Sale_Multiple, Sale_Price,Sale_Cost, Sale_End_Date, Identifier, Dept_No, Vendor_ID, ProjUnits, Comment1,
                        Comment2, Billback)
                        select i.Item_Key as Item_Key ,o.Store_No, p.PriceChgTypeId,@strtDt as Start_Date,
                        o.Multiple as Multiple, o.price as Price, @pm as Sale_Multiple, @salPrc as Sale_Price, @salCst as Sale_Cost,
                        @endDt  as Sale_End_Date, i.Identifier as Identifier, h.SubTeam_No as Dept_No, @vndr as Vendor_ID ,
                        @qty as ProjUnits,@com1 as comment1,@com2 as comment2,@bbk as billback from Price o,
                        Item h, ItemIdentifier i, PriceChgType p where
                        (SUBSTRING('00000000000000', 1, 13 - LEN(i.Identifier)) + i.Identifier + '0' =  '0' + @upcno)
                        and
                        i.Item_Key = o.Item_Key and i.Item_Key = h.Item_Key and p.PriceChgTypeDesc = 'SAL'
                        group by i.Item_Key ,o.Store_No, p.PriceChgTypeId, o.Multiple , o.Price , i.Identifier,
                        h.SubTeam_No;

                        --INSERT INTO dbo.VendorCostHistory
                        --(StoreItemVendorID, Promotional, UnitCost, UnitFreight, Package_Desc1, StartDate, EndDate,
                        --FromVendor, MSRP, InsertDate, InsertWorkStation)
                        --SELECT a.StoreItemVendorID, 1 AS Expr1,@salCst, 0 AS Expr8, i.Package_Desc1,
                        --@strtDt AS Expr2, @endDt AS Expr3, 0 AS Expr4, ' ' AS Expr5, GETDATE() AS Expr6,
                        --'O' AS Expr7 FROM dbo.VendorCostHistory AS b INNER JOIN dbo.StoreItemVendor AS a
                        --ON b.StoreItemVendorID = a.StoreItemVendorID CROSS JOIN dbo.Item AS i
                        --WHERE (b.EndDate > GETDATE()) AND (b.StartDate < GETDATE()) and
                        --i.item_key = a.item_key and a.Vendor_ID = @vndr and  i.Item_Key =
                        --(SELECT     Item_Key FROM dbo.ItemIdentifier WHERE
                        --(SUBSTRING('0000000000000', 1, 12 - LEN(Identifier)) + Identifier + '0' = @upcno));

            End

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertPromoPlanner] TO [IRMAPromoRole]
    AS [dbo];

