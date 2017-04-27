SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorAndID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendorAndID]
GO


CREATE PROCEDURE dbo.GetVendorAndID 
AS 

SELECT Vendor_ID, CompanyName 
FROM Vendor 
ORDER BY CompanyName
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

