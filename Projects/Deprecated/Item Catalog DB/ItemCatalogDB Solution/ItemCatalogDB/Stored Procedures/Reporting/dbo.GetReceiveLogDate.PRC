SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetReceiveLogDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetReceiveLogDate]
GO


CREATE PROCEDURE dbo.GetReceiveLogDate 
@Store_No int,
@Date datetime
AS 

SELECT RecvLogDate
FROM OrderHeader
INNER JOIN Vendor ON Vendor.Vendor_ID = OrderHeader.ReceiveLocation_ID
WHERE Vendor.Store_No = @Store_No
AND (RecvLogDate >= @Date AND RecvLogDate < DATEADD(d, 1, @Date))


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


