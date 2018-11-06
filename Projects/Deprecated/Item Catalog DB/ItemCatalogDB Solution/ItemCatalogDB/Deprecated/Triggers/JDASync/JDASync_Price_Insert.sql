

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Price_Insert')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Price_Insert'
		DROP  Trigger JDASync_Price_Insert
	END
GO


PRINT 'Creating Trigger JDASync_Price_Insert'
GO
CREATE Trigger dbo.JDASync_Price_Insert 
ON Price
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
		INSERT INTO JDA_PriceSync
		(
			ApplyDate,
			Item_Key,
			Store_No,
			JDA_PricePriority,
			Multiple,
			Price,
			Sale_Multiple,
			Sale_Price,
			Sale_Start_Date,
			Sale_End_Date,
			IRMA_PriceChgType_ID
		)
		SELECT DISTINCT
			GetDate(),
			Inserted.Item_Key,
			Inserted.Store_No,
			jpcm.JDA_Priority,
			CASE WHEN PriceChgType.On_Sale = 0 THEN Inserted.Multiple ELSE NULL END,
			CASE WHEN PriceChgType.On_Sale = 0 THEN Inserted.Price ELSE NULL END,
			CASE WHEN PriceChgType.On_Sale = 1 THEN Inserted.Sale_Multiple ELSE NULL END,
			CASE WHEN PriceChgType.On_Sale = 1 THEN Inserted.Sale_Price ELSE NULL END,
			CASE WHEN PriceChgType.On_Sale = 1 THEN Inserted.Sale_Start_Date ELSE NULL END,
			CASE WHEN PriceChgType.On_Sale = 1 THEN Inserted.Sale_End_Date ELSE NULL END,
			Inserted.PriceChgTypeID
		FROM Inserted
			JOIN PriceChgType (NOLOCK)
				ON PriceChgType.PriceChgTypeID = Inserted.PriceChgTypeID
			LEFT JOIN JDA_PriceChgTypeMapping jpcm (NOLOCK)
				ON jpcm.PriceChgTypeID = Inserted.PriceChgTypeID
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_Price_Insert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END

GO
