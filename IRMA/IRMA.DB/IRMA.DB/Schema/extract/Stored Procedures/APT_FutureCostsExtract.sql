CREATE PROCEDURE [extract].[APT_FutureCostsExtract]
	-- INPUT PARAMETERS
	@DeltaLoad bit = 1
AS
BEGIN
DECLARE @costtable TABLE(
	EFF_DATE VARCHAR(8) NULL,
	NAT_UPC  VARCHAR(13) NULL,
	STORE_NUMBER VARCHAR(12) NULL,
	COST VARCHAR(10) NULL,
	RowNumber INT NULL
)

INSERT INTO @costtable
SELECT 'EFF_DATE', 'NAT_UPC', 'STORE_NUMBER', 'COST', 1

IF @DeltaLoad = 1
    INSERT INTO @costtable
	EXEC [dbo].[PDX_FutureCostDeltaFile]
ELSE
	INSERT INTO @costtable
	EXEC [dbo].[PDX_FutureCostFullFile]

SELECT 
	EFF_DATE,
	NAT_UPC,
	STORE_NUMBER,
	COST
FROM @costtable
ORDER BY RowNumber DESC

END
GO

GRANT EXECUTE
    ON OBJECT::[extract].[APT_FutureCostsExtract] TO [IConInterface]
    AS [dbo];
GO