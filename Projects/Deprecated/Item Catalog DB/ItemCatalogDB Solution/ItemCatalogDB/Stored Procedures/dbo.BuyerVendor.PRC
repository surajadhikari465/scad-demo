SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[BuyerVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[BuyerVendor]
GO


CREATE PROCEDURE dbo.BuyerVendor
AS

SELECT Vendor_ID, Vendor_Key, CompanyName 
FROM Vendor

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


