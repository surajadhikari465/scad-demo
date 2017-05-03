

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Price_Update')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Price_Update'
		DROP  Trigger JDASync_Price_Update
	END
GO


PRINT 'Creating Trigger JDASync_Price_Update'
GO
CREATE Trigger dbo.JDASync_Price_Update 
ON Price
FOR UPDATE
AS
BEGIN
	-- this is critical to the functioning of the audit
	-- it allows us to compare null to null
	SET ANSI_NULLS OFF

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
		FROM
			Inserted
			JOIN Deleted 
				ON Deleted.Item_Key = Inserted.Item_Key
				AND Deleted.Store_No = Inserted.Store_No
			JOIN PriceChgType (NOLOCK)
				ON PriceChgType.PriceChgTypeID = Inserted.PriceChgTypeID
			LEFT JOIN JDA_PriceChgTypeMapping jpcm (NOLOCK)
				ON jpcm.PriceChgTypeID = Inserted.PriceChgTypeID
		WHERE
			-- we care only if any of the columns we are tracking changes
			Inserted.Item_Key <> Deleted.Item_Key OR
			Inserted.Store_No <> Deleted.Store_No OR
			jpcm.PriceChgTypeId <> Deleted.PriceChgTypeId OR
			(
				PriceChgType.On_Sale = 0 AND
				(
					Inserted.Multiple <> Deleted.Multiple OR
					Inserted.Price <> Deleted.Price
				)
			) OR
			(
				PriceChgType.On_Sale = 1 AND
				(
					Inserted.Sale_Multiple <> Deleted.Sale_Multiple OR
					Inserted.Sale_Price <> Deleted.Sale_Price OR
					Inserted.Sale_Start_Date <> Deleted.Sale_Start_Date OR
					Inserted.Sale_End_Date <> Deleted.Sale_End_Date
				)
			) 

	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_Price_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
	
	-- reset it
	SET ANSI_NULLS ON

END

GO
