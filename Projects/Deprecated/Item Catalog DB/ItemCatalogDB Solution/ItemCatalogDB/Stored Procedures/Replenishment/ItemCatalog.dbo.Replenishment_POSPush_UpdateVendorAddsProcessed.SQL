/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_UpdateVendorAddsProcessed]    Script Date: 07/21/2006 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_POSPush_UpdateVendorAddsProcessed]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_POSPush_UpdateVendorAddsProcessed]
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_UpdateVendorAddsProcessed]    Script Date: 07/21/2006 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.Replenishment_POSPush_UpdateVendorAddsProcessed
    @Vendor_ID int    
AS

BEGIN
    UPDATE Vendor 
    SET AddVendor=0
    WHERE Vendor_ID = @Vendor_ID
END 
GO

