IF EXISTS ( SELECT  *
            FROM    SYSOBJECTS
            WHERE   NAME = 'fn_PeriodOrdersAvgCost' ) 
    DROP FUNCTION dbo.fn_PeriodOrdersAvgCost
    GO


CREATE FUNCTION dbo.fn_PeriodOrdersAvgCost
    (
      @VendorIDList VARCHAR(8000) ,
      @ListSeparator CHAR(1) ,
      @Store_No INT ,
      @Item_Key INT ,
      @Date DATETIME
    )
RETURNS MONEY
AS 
    BEGIN

        DECLARE @PeriodBeginDate DATETIME ,
            @PeriodEndDate DATETIME ,
            @AvgCost MONEY

    

    -- Get period begin date

        SELECT  @PeriodBeginDate = dbo.fn_PeriodBeginDate(@Date)



        SELECT  @PeriodEndDate = DATEADD(day, 28, @PeriodBeginDate)



        SELECT  @AvgCost = SUM(ReceivedItemCost + ReceivedItemFreight)
                / SUM(UnitsReceived)
        FROM    OrderHeader (NOLOCK)
                INNER JOIN dbo.fn_Parse_List(@VendorIDList, '|') VL ON VL.Key_Value = OrderHeader.Vendor_ID
                INNER JOIN Vendor VendStore ( NOLOCK ) ON VendStore.Vendor_ID = OrderHeader.ReceiveLocation_ID
                INNER JOIN OrderItem (NOLOCK) ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
        WHERE   VendStore.Store_No = @Store_No
                AND Return_Order = 0
                AND Item_Key = @Item_Key
                AND DateReceived >= @PeriodBeginDate
                AND DateReceived < @PeriodEndDate
                AND UnitsReceived > 0



        IF @AvgCost IS NULL 
            BEGIN

        -- Get last order cost for the item for any of the vendors in the list

                SELECT  @AvgCost = SUM(ReceivedItemCost + ReceivedItemFreight)
                        / SUM(UnitsReceived)
                FROM    OrderItem
                WHERE   OrderHeader_ID = ( SELECT TOP 1
                                                    OrderHeader.OrderHeader_ID
                                           FROM     OrderHeader (NOLOCK)
                                                    INNER JOIN dbo.fn_Parse_List(@VendorIDList,
                                                              @ListSeparator) VL ON VL.Key_Value = OrderHeader.Vendor_ID
                                                    INNER JOIN Vendor VendStore ( NOLOCK ) ON VendStore.Vendor_ID = OrderHeader.ReceiveLocation_ID
                                                    INNER JOIN OrderItem (NOLOCK) ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
                                           WHERE    VendStore.Store_No = @Store_No
                                                    AND Return_Order = 0
                                                    AND Item_Key = @Item_Key
                                                    AND UnitsReceived > 0
                                           ORDER BY DateReceived DESC
                                         )
                        AND Item_Key = @Item_Key



            END



        RETURN @AvgCost

    END
GO


