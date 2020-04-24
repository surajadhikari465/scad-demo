CREATE PROCEDURE [extract].[APT_FutureCostsExtract]
	-- INPUT PARAMETERS
	@DeltaLoad bit = 1
AS
BEGIN
DECLARE @costtable TABLE(
	EFF_DATE VARCHAR(8) NULL,
	NAT_UPC  VARCHAR(13) NULL,
	STORE_NUMBER VARCHAR(12) NULL,
	COST VARCHAR(10) NULL
)

INSERT INTO @costtable
SELECT 'EFF_DATE', 'NAT_UPC', 'STORE_NUMBER', 'COST' 

IF @DeltaLoad = 1
    INSERT INTO @costtable
	EXEC [dbo].[PDX_FutureCostDeltaFile]
ELSE
	INSERT INTO @costtable
	EXEC [dbo].[PDX_FutureCostFullFile]

SELECT * FROM @costtable
END