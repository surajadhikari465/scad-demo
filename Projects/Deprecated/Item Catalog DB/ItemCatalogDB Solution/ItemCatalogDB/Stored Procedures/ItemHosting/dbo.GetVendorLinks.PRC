SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorLinks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendorLinks]
GO


CREATE PROCEDURE dbo.GetVendorLinks 
@Vendor_ID int 
AS 

SELECT Vendor_ID FROM OrderHeader WHERE Vendor_ID = @Vendor_ID 
UNION 
SELECT PurchaseLocation_ID AS Vendor_ID FROM OrderHeader WHERE PurchaseLocation_ID = @Vendor_ID 
UNION 
SELECT ReceiveLocation_ID AS Vendor_ID FROM OrderHeader WHERE ReceiveLocation_ID = @Vendor_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


