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
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetRegionFTPConfig] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_GetRegionFTPConfig] TO [IRMASchedJobsRole]
    AS [dbo];

