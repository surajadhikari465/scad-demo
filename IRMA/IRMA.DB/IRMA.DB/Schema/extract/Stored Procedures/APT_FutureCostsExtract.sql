CREATE PROCEDURE [extract].[APT_FutureCostsExtract]
	-- INPUT PARAMETERS
	@DeltaLoad bit = 1
AS
BEGIN
IF @DeltaLoad = 1
	EXEC [dbo].[PDX_FutureCostDeltaFile]
ELSE
	EXEC [dbo].[PDX_FutureCostFullFile]
END