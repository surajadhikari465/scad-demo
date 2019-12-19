


CREATE PROCEDURE [dbo].[spUpdateBrandName]
@LinkedServer nvarchar(50)
AS
BEGIN
	PRINT 'Updating Brand Name...'

	IF OBJECT_ID('dbo.Temp_Brand_Names', 'U') IS NOT NULL
	  DROP TABLE dbo.Temp_Brand_Names;
	CREATE TABLE dbo.Temp_Brand_Names
		(
		UPC varchar(25) null,
		BRAND varchar(50) null,
		BRAND_NAME varchar(50) null
		)
		

	IF OBJECT_ID('dbo.Temp_Upcs', 'U') IS NOT NULL
	  DROP TABLE dbo.Temp_Upcs;
	CREATE TABLE dbo.Temp_Upcs
		(
		UPC varchar(25) null
		)
		
	Insert into Temp_Upcs (UPC) (SELECT distinct UPC from REPORT_DETAIL WHERE UPC IS NOT NULL)
	
	PRINT 'Querying item_master for brand names.'
	DECLARE @upc varchar(25);
	DECLARE c CURSOR FOR SELECT UPC FROM Temp_Upcs;
	OPEN c
	FETCH c INTO @upc
	WHILE (@@FETCH_STATUS=0) BEGIN
		INSERT INTO Temp_Brand_Names (UPC, BRAND, BRAND_NAME) EXEC dbo.spFindProductInfoItemMaster @LinkedServer, @upc
		FETCH c INTO @upc
	END
	CLOSE c
	DEALLOCATE c
	
	DELETE FROM Temp_Brand_Names where BRAND_NAME is NULL
	
	DECLARE @n int;
	set @n=((select COUNT(*) from temp_upcs) - (select COUNT(*) from temp_brand_names))
	PRINT 'Querying item_region for ' +  CAST(@n as varchar(10)) + ' brand names not in item_master.'
	DECLARE p CURSOR FOR SELECT distinct(UPC) FROM Temp_Upcs WHERE UPC not in (SELECT UPC from Temp_Brand_Names)
	open p
	FETCH p into @upc
	WHILE (@@FETCH_STATUS=0) BEGIN
		INSERT INTO Temp_Brand_Names (UPC, BRAND, BRAND_NAME) EXEC dbo.spFindProductInfoItemRegion @LinkedServer, @upc
		FETCH p INTO @upc
	END
	CLOSE p
	DEALLOCATE p
	
	PRINT 'Updating BRAND_NAME in Report_Detail.'
	DECLARE s CURSOR FOR SELECT UPC FROM Temp_Upcs;
	OPEN s
	FETCH s INTO @upc
	WHILE (@@FETCH_STATUS=0) BEGIN
		Update Report_Detail set BRAND_NAME=(SELECT BRAND_NAME from Temp_Brand_Names where UPC=@upc) where UPC=@upc
		FETCH s INTO @upc
	END
	CLOSE s
	DEALLOCATE s

	PRINT 'Finished.'
END
