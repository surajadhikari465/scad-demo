SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetStoreFromOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetStoreFromOrder]
GO


CREATE PROCEDURE dbo.GetStoreFromOrder 
@OrderHeader_ID int 
AS 

SELECT Vendor.Store_No 
FROM Vendor INNER JOIN OrderHeader ON Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID 
WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

