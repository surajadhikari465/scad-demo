SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendorName]
GO


CREATE PROCEDURE dbo.GetVendorName 
@Vendor_ID int 
AS 

SELECT CompanyName 
FROM Vendor 
WHERE Vendor_ID = @Vendor_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


