SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckTransferDistributions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckTransferDistributions]
GO

CREATE PROCEDURE dbo.CheckTransferDistributions
@OrderHeader_ID int
AS 
SELECT Manifest_ID, Customer_ID
FROM Manifest
WHERE Customer_ID = (SELECT ReceiveLocation_ID FROM OrderHeader Where OrderHeader_ID = @OrderHeader_ID) AND
      DistLoc_ID = (SELECT Vendor_ID FROM OrderHeader Where OrderHeader_ID = @OrderHeader_ID) AND
      ShipDate IS NULL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

