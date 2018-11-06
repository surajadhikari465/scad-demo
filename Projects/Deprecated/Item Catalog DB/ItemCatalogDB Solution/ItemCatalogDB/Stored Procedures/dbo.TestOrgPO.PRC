SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TestOrgPO]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[TestOrgPO]
GO


CREATE PROCEDURE dbo.TestOrgPO
@OrderHeader_ID int,
@Vendor_ID int
AS 

SELECT COUNT(*) 
FROM OrderHeader 
WHERE OrderHeader.Orderheader_Id =  @OrderHeader_ID
AND Vendor_ID = @Vendor_ID 
AND CloseDate IS NOT NULL
AND Return_Order = 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

