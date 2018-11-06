
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetInventoryCountVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetInventoryCountVendors]
GO


CREATE PROCEDURE dbo.GetInventoryCountVendors
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT  ICVID, ICVABBRV
    FROM    CycleCountVendor
    ORDER BY ICVABBRV
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


