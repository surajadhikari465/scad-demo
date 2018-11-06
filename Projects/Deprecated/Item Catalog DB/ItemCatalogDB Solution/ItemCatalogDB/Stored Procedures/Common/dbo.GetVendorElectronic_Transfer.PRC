SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVendorElectronic_Transfer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVendorElectronic_Transfer]
GO


CREATE PROCEDURE dbo.GetVendorElectronic_Transfer 
@Vendor_ID int
AS 

SELECT Electronic_Transfer
FROM Vendor
WHERE Vendor_ID = @Vendor_ID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

