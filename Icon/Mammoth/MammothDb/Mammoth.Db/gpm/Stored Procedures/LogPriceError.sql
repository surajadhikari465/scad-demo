CREATE PROCEDURE [gpm].[LogPriceError] 
	@Region nvarchar(2),
	@GpmID uniqueidentifier,
	@ItemID int,
	@BusinessUnitID int,
	@Price decimal(19,4),
	@StartDate datetime2,
	@EndDate datetime2,
	@PriceType nvarchar(3),
	@PriceTypeAttribute nvarchar(3),
	@SellableUOM nvarchar(3),
	@CurrencyCode nvarchar(3),
	@Multiple int,
	@TagExpirationDate datetime2 = NULL,
	@ErrorMessage nvarchar(500) ,
	@ErrorCode nvarchar(100)
AS
BEGIN

	INSERT INTO [gpm].[PriceErrorLog]
           ([Region]
           ,[GpmID]
           ,[ItemID]
           ,[BusinessUnitID]
           ,[StartDate]
           ,[EndDate]
           ,[Price]
           ,[PriceType]
           ,[PriceTypeAttribute]
           ,[SellableUOM]
           ,[CurrencyCode]
           ,[Multiple]
           ,[TagExpirationDate]
           ,[InsertDateUtc] 
           ,[ErrorMessage]
           ,[ErrorCode])
     VALUES
			(
			  @Region,
			  @GpmID,
			  @ItemID,
			  @BusinessUnitID,		  
			  @StartDate,
			  @EndDate,
			  @Price,
			  @PriceType,
			  @PriceTypeAttribute,
			  @SellableUOM,
			  @CurrencyCode,
			  @Multiple,
			  @TagExpirationDate,
			  SYSUTCDATETIME(),
			  @ErrorMessage,
			  @ErrorCode 
			)

END
GO

GRANT EXEC ON [gpm].[LogPriceError] TO TibcoRole
GO