SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorLockStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendorLockStatus]
GO


CREATE PROCEDURE dbo.GetVendorLockStatus 
@Vendor_ID int 
AS 

SELECT User_ID 

FROM Vendor 

WHERE Vendor_ID = @Vendor_ID 


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


