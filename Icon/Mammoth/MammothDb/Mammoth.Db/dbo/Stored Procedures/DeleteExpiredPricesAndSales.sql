CREATE PROCEDURE dbo.DeleteExpiredPricesAndSales
AS
BEGIN
	DECLARE @expiredPrices table
			(
				Region nchar(2),
				PriceID int,
				ItemID int,
				BusinessUnitID int,
				StartDate datetime,
				EndDate datetime,
				Price smallmoney,
				PriceType nvarchar(3),
				PriceUOM nvarchar(3),
				CurrencyID int,
				Multiple tinyint,
				AddedDate datetime,
				ModifiedDate datetime
			);		
	DECLARE	@expiredSales table
			(
				Region nchar(2),
				PriceID int,
				ItemID int,
				BusinessUnitID int,
				StartDate datetime,
				EndDate datetime,
				Price smallmoney,
				PriceType nvarchar(3),
				PriceUOM nvarchar(3),
				CurrencyID int,
				Multiple tinyint,
				AddedDate datetime,
				ModifiedDate datetime
			);

	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'FL'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'MA'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'MW'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'NA'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'NC'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'NE'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'PN'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'RM'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'SO'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'SP'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'SW'
	INSERT INTO @expiredPrices
	EXEC dbo.DeleteExpiredPrices @RegionCode = 'UK'

	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'FL'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'MA'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'MW'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'NA'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'NC'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'NE'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'PN'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'RM'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'SO'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'SP'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'SW'
	INSERT INTO @expiredSales
	EXEC dbo.DeleteExpiredSales @RegionCode = 'UK'

	SELECT * FROM @expiredPrices

	SELECT * FROM @expiredSales	
END