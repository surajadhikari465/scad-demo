USE ItemCatalog;

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
	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Closed-Store Cleanup: Deleting StorePOSConfig'
	DELETE
	FROM dbo.StorePOSConfig
	output deleted.*
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Closed-Store Cleanup: Deleting StoreScaleConfig'
	DELETE
	FROM dbo.StoreScaleConfig
	output deleted.*
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Closed-Store Cleanup: Deleting StoreShelfTagConfig'
	DELETE
	FROM dbo.StoreShelfTagConfig
	output deleted.*
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);

	print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Closed-Store Cleanup: Deleting StoreElectronicShelfTagConfig'
	DELETE
	FROM dbo.StoreElectronicShelfTagConfig
	output deleted.*
	WHERE Store_No IN (SELECT Store_No FROM @closedStore);
END
