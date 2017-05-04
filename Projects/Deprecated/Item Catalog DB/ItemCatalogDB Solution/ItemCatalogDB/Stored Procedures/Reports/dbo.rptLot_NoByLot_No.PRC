IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'rptLot_NoByLot_No' 
	   AND 	  type = 'P')
    DROP PROCEDURE rptLot_NoByLot_No
GO

CREATE PROCEDURE dbo.rptLot_NoByLot_No 
	@Lot_No varchar(12),
    @Vendor_ID int
AS
BEGIN
    SET NOCOUNT ON

    select OrderItem.OrderHeader_ID, ItemIdentifier.identifier, OrderItem.Lot_No, store.Store_Name,
           OrderHeader.CloseDate, Vendor.CompanyName
    from OrderHeader 
        INNER JOIN
            orderitem
            on orderitem.orderheader_ID = orderheader.orderheader_ID
        INNER JOIN
            ItemIdentifier
            on OrderItem.Item_Key = ItemIdentifier.Item_Key
        INNER JOIN
            Vendor
            on OrderHeader.Vendor_ID = Vendor.Vendor_ID
        INNER JOIN
            vendor RcvLoc
            on orderheader.ReceiveLocation_ID = RcvLoc.Vendor_ID
        INNER JOIN
            store
            on Store.store_no = RcvLoc.Store_No
    WHERE OrderItem.Lot_no = @Lot_No and OrderHeader.Vendor_ID = @Vendor_ID

    SET NOCOUNT OFF
END

GO

