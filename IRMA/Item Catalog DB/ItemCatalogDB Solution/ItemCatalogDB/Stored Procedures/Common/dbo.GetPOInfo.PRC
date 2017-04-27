SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOInfo]
GO


CREATE PROCEDURE dbo.GetPOInfo
@OrderHeader_ID int
AS

SELECT OrderHeader_ID, OrderType_ID, Transfer_SubTeam, ISNULL(Store.Store_No,-1) AS Store_No, CloseDate, 
       ISNULL(Vendor.Vendor_Key, Vendor.CompanyName) AS CompanyName
FROM Vendor Store WITH (NOLOCK) INNER JOIN (
       Vendor WITH (NOLOCK) INNER JOIN OrderHeader ON (Vendor.Vendor_ID = OrderHeader.Vendor_ID)
     ) ON (Store.Vendor_ID = OrderHeader.ReceiveLocation_ID)
WHERE OrderHeader_ID = @OrderHeader_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


