SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnlockVendor]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnlockVendor]
GO


CREATE PROCEDURE dbo.UnlockVendor 
@Vendor_ID int 
AS 

UPDATE Vendor 
SET User_ID = NULL 
WHERE Vendor_ID = @Vendor_ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


