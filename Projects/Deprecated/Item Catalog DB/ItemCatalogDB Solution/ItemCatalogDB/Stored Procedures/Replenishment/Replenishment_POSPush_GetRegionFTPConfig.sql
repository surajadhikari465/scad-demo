
/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetRegionFTPConfig]    Script Date: 01/25/2011 08:18:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Replenishment_POSPush_GetRegionFTPConfig]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Replenishment_POSPush_GetRegionFTPConfig]
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_POSPush_GetRegionFTPConfig]    Script Date: 01/25/2011 08:18:47 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[Replenishment_POSPush_GetRegionFTPConfig]
    @Description VARCHAR(50)

AS 
BEGIN
	DECLARE @Region VARCHAR(2)
	SELECT @Region = RegionCode FROM Region
	
	SELECT
		[FTPAddress],
		[Username],
		[Password],
		[ChangeDir],
		[Port],
		@Region as [Region]
	FROM RegionFTPConfig
	WHERE 
		[Description] = @Description

END
GO

-- GRANT EXECUTE ON dbo.Replenishment_POSPush_GetRegionFTPConfig TO IRSUser