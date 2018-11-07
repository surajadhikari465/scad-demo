SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRegions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRegions]
GO

CREATE PROCEDURE dbo.GetRegions
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Region_ID, RegionName, RegionCode
    FROM Region
    ORDER BY RegionName
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


