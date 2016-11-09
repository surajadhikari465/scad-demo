CREATE PROCEDURE dbo.GetOrderVendorConfig
@OrderHeader_ID int
AS 

select vdr.fax, vdr.electronic_transfer, vdr.file_type, vdr.ftp_addr, vdr.ftp_user, vdr.ftp_password, vdr.email, ord.fax_order, ord.email_order, ord.electronic_order
	from vendor vdr, orderheader ord
	where
		ord.orderheader_id = @OrderHeader_ID
		and vdr.vendor_id = ord.vendor_id
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderVendorConfig] TO [IRMAClientRole]
    AS [dbo];

