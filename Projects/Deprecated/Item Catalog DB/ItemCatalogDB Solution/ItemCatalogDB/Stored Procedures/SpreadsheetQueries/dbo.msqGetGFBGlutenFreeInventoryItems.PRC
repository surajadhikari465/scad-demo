SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.msqGetGFBGlutenFreeInventoryItems    Script Date: 9/20/2005 3:12:59 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[msqGetGFBGlutenFreeInventoryItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[msqGetGFBGlutenFreeInventoryItems]
GO

CREATE PROCEDURE dbo.msqGetGFBGlutenFreeInventoryItems

AS

DECLARE @StoreNo int, @VendorID int, @SubTeamNo int
SET @StoreNo = 101 
SET @VendorID = 6389 
SET @SubTeamNo = 2600

EXEC msqGetBakeryDistributionInventoryItems @StoreNo, @VendorID, @SubTeamNo


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

