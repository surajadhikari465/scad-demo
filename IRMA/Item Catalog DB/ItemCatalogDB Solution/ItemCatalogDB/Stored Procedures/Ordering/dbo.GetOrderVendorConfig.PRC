SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderVendorConfig]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderVendorConfig]
GO


CREATE PROCEDURE dbo.GetOrderVendorConfig
@OrderHeader_ID int
AS 

select vdr.fax, vdr.electronic_transfer, vdr.file_type, vdr.ftp_addr, vdr.ftp_user, vdr.ftp_password, vdr.email, ord.fax_order, ord.email_order, ord.electronic_order
	from vendor vdr, orderheader ord
	where
		ord.orderheader_id = @OrderHeader_ID
		and vdr.vendor_id = ord.vendor_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
