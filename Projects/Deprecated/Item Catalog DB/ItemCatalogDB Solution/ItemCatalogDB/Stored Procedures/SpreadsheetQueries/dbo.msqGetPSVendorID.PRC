/****** Object:  Stored Procedure dbo.msqGetPSVendorID    Script Date: 1/27/2005 9:38:22 AM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[msqGetPSVendorID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[msqGetPSVendorID]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.msqGetPSVendorID    Script Date: 1/27/2005 9:38:22 AM ******/
CREATE PROCEDURE dbo.msqGetPSVendorID

AS

SELECT CompanyName, PS_Vendor_ID 
FROM Vendor
WHERE PS_Vendor_ID IS NOT NULL
ORDER BY CompanyName


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



