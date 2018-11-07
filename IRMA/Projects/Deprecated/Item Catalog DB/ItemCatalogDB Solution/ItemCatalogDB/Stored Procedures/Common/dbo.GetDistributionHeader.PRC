SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetDistributionHeader]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetDistributionHeader]
GO



CREATE PROCEDURE dbo.GetDistributionHeader 
@OrderHeader_ID int 
AS 

SELECT Vendor.CompanyName,
       OrderHeader.Expected_Date,
       OrderHeader.OrderDate
FROM OrderHeader (NOLOCK) INNER JOIN Vendor (NOLOCK) ON (OrderHeader.Vendor_ID = Vendor.Vendor_ID)
WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


