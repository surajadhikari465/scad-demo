--PBI 19556: As the SCAD QA team we need a script that will clean up closed store FTP configs so that we do not have push failures after a refresh.
DECLARE @closedStore AS TABLE (Store_No INT);

INSERT INTO @closedStore
SELECT DISTINCT A.Store_No
FROM dbo.Store A
INNER JOIN dbo.StoreFTPConfig B ON B.Store_No = A.Store_No
WHERE A.Distribution_Center = 1
	OR A.Manufacturer = 1;

IF (EXISTS(SELECT 1	FROM @closedStore))
BEGIN
	DELETE
	FROM dbo.StorePOSConfig
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);

	DELETE
	FROM dbo.StoreScaleConfig
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);

	DELETE
	FROM dbo.StoreShelfTagConfig
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);

	DELETE
	FROM dbo.StoreElectronicShelfTagConfig
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);
END