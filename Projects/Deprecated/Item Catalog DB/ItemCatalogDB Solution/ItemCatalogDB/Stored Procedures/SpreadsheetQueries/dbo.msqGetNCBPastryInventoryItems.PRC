SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.msqGetNCBPastryInventoryItems    Script Date: 9/20/2005 3:13:00 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[msqGetNCBPastryInventoryItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[msqGetNCBPastryInventoryItems]
GO

CREATE PROCEDURE dbo.msqGetNCBPastryInventoryItems

AS

DECLARE @StoreNo int, @VendorID int, @SubTeamNo int
SET @StoreNo = 101 
SET @VendorID = 5851 
SET @SubTeamNo = 4400

EXEC msqGetBakeryDistributionInventoryItems @StoreNo, @VendorID, @SubTeamNo

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


