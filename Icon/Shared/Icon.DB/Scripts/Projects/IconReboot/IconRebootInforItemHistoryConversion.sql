

TRUNCATE TABLE infor.ItemHistory


IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLES
		WHERE TABLE_SCHEMA = 'dbo'
			AND TABLE_NAME = 'temp_InforItemHistoryFile'
		)
BEGIN
	CREATE TABLE dbo.temp_InforItemHistoryFile (
	json nvarchar(max)
)
END

TRUNCATE TABLE dbo.temp_InforItemHistoryFile 

BULK INSERT dbo.temp_InforItemHistoryFile
FROM 'E:\sql_temp_01\IconRebootShare\InforItemHistory.json' -- needs to be added with file path and filename. Currently C:\TEMP\IconConversion\infor_im_prod_item_20190520.csv
WITH
(
	ROWTERMINATOR = '0x0a'
);
GO

INSERT INTO infor.ItemHistory
(
	ItemId,
	JsonObject
)
SELECT
	JSON_VALUE(ihf.json, '$."Item ID"') as ItemId,
	json
FROM
dbo.temp_InforItemHistoryFile ihf


DROP TABLE dbo.temp_InforItemHistoryFile 