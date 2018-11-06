--exec rptLot_NoByIdentifier '222401', 523, '01/01/2005', '01/01/2006'

IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'rptLot_NoByIdentifier' 
	   AND 	  type = 'P')
    DROP PROCEDURE rptLot_NoByIdentifier
GO

CREATE PROCEDURE dbo.rptLot_NoByIdentifier 
	@Identifier varchar(13),
    @Vendor_ID int,
    @MinDate varchar(10),
    @MaxDate varchar(10)

AS
BEGIN
    SET NOCOUNT ON
    
    select OrderItem.Lot_no, OrderItem.OrderHeader_ID, store.Store_Name, Vendor.CompanyName, OrderHeader.CloseDate
    from OrderHeader (Nolock) 
        INNER JOIN
            orderitem (Nolock)
            on orderitem.orderheader_ID = orderheader.orderheader_ID
        INNER JOIN
            ItemIdentifier (Nolock)
            on OrderItem.Item_Key = ItemIdentifier.Item_Key and Default_Identifier = case when @Identifier is null then 1 else default_identifier end       
        INNER JOIN
            Vendor (Nolock)
            on Vendor.Vendor_Id = ORderHeader.Vendor_ID
        INNER JOIN
            vendor RcvLoc (Nolock)
            on orderheader.ReceiveLocation_ID = RcvLoc.Vendor_ID
        INNER JOIN
            store (Nolock)
            on Store.store_no = RcvLoc.Store_No
    WHERE  OrderItem.DateReceived > @MinDate and OrderItem.DateReceived < @MaxDate
           and OrderHeader.Vendor_ID = @Vendor_ID 
           and ItemIdentifier.Identifier = @Identifier and ItemIdentifier.Deleted_Identifier = 0 
           and not(lot_no is null)

    SET NOCOUNT OFF
END

GO


