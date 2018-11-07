CREATE PROCEDURE [dbo].[InsertCompetitorPrice] 
	@Item_Key int,
	@CompetitorStoreID int OUTPUT,
	@FiscalYear smallint,
	@FiscalPeriod tinyint,
	@PeriodWeek tinyint,
	@UPCCode varchar(50),
	@Description varchar(250),
	@PriceMultiple tinyint,
	@Price smallmoney,
	@SaleMultiple tinyint,
	@Sale smallmoney,
	@Size decimal(9,4),
	@Unit_ID int,
	@UpdateUserID int,
	@UpdateDateTime smalldatetime,
	@CompetitorID int OUTPUT,
	@CompetitorLocationID int OUTPUT,
	@Competitor varchar(50),
	@Location varchar(50),
	@CompetitorStore varchar(50),
	@ItemIdentifier varchar(13)
AS
BEGIN

-- Competitor Store Matching
IF @CompetitorStoreID IS NULL
BEGIN

	IF @CompetitorID IS NULL
	BEGIN
		SELECT
			@CompetitorID = CompetitorID
		FROM 
			Competitor
		WHERE
			Name = @Competitor
		
		-- If the competitor was not found, create a new one
		IF @CompetitorID IS NULL
		BEGIN
			INSERT INTO Competitor
			(Name) VALUES (@Competitor)
			
			SET @CompetitorID = SCOPE_IDENTITY()
		END
	END

	IF @CompetitorLocationID IS NULL
	BEGIN
		SELECT
			@CompetitorLocationID = CompetitorLocationID
		FROM
			CompetitorLocation
		WHERE
			Name = @Location
		
		IF @CompetitorLocationID IS NULL
		BEGIN
			INSERT INTO CompetitorLocation
			(Name) VALUES (@Location)
			
			SET @CompetitorLocationID = SCOPE_IDENTITY()
		END
	END

	SELECT
		@CompetitorStoreID = CompetitorStoreID
	FROM
		CompetitorStore
	WHERE
		CompetitorID = @CompetitorID
		AND
		CompetitorLocationID = @CompetitorLocationID
		AND
		Name = @CompetitorStore
		
	IF @CompetitorStoreID IS NULL
	BEGIN
		INSERT INTO CompetitorStore
			(CompetitorID, CompetitorLocationID, Name, UpdateUserID, UpdateDateTime)
			VALUES
			(@CompetitorID, @CompetitorLocationID, @CompetitorStore, @UpdateUserID, @UpdateDateTime)
		
		SET @CompetitorStoreID = SCOPE_IDENTITY()
	END
END

IF @UPCCode <> @ItemIdentifier AND 
	(SELECT COUNT (*) 
		FROM CompetitorStoreItemIdentifier 
		WHERE 
			CompetitorStoreID = @CompetitorStoreID
			AND
			Identifier = @UPCCode
			AND
			Item_Key = @Item_Key) > 0
BEGIN
	INSERT INTO CompetitorStoreItemIdentifier
		(CompetitorStoreID, Identifier, Item_Key)
		VALUES
		(@CompetitorStoreID, @UPCCode, @Item_Key)
END

IF 0 = (SELECT COUNT(*) 
	FROM CompetitorPrice 
	WHERE
		Item_Key = @Item_Key
		AND
		CompetitorStoreID = @CompetitorStoreID
		AND
		FiscalYear = @FiscalYear
		AND
		FiscalPeriod = @FiscalPeriod
		AND
		PeriodWeek = @PeriodWeek)
BEGIN
	INSERT INTO CompetitorPrice
		(Item_Key,
		CompetitorStoreID,
		FiscalYear,
		FiscalPeriod,
		PeriodWeek,
		UPCCode,
		Description,
		PriceMultiple,
		Price,
		SaleMultiple,
		Sale,
		Size,
		Unit_ID,
		UpdateUserID,
		UpdateDateTime)		
	VALUES
		(@Item_Key,
		@CompetitorStoreID,
		@FiscalYear,
		@FiscalPeriod,
		@PeriodWeek,
		@UPCCode,
		@Description,
		@PriceMultiple,
		@Price,
		@SaleMultiple,
		@Sale, 
		@Size,
		@Unit_ID,
		@UpdateUserID,
		@UpdateDateTime)
END
ELSE
BEGIN
	-- There is an existing competitor price row with the given primary key
	EXEC UpdateCompetitorPrice
		@Item_Key = @Item_Key,
		@CompetitorStoreID = @CompetitorStoreID,
		@FiscalYear = @FiscalYear,
		@FiscalPeriod = @FiscalPeriod,
		@PeriodWeek = @PeriodWeek,
		@UPCCode = @UPCCode,
		@Description = @Description,
		@PriceMultiple = @PriceMultiple,
		@Price = @Price,
		@SaleMultiple = @SaleMultiple,
		@Sale = @Sale,
		@Size = @Size,
		@Unit_ID = @Unit_ID,
		@UpdateUserID = @UpdateUserID,
		@UpdateDateTime = @UpdateDateTime
END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertCompetitorPrice] TO [IRMAClientRole]
    AS [dbo];

