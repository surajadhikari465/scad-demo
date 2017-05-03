IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'StoreItemVendorUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER StoreItemVendorUpdate
GO

CREATE TRIGGER StoreItemVendorUpdate
    ON StoreItemVendor
FOR UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	-- Update the delete date for the inserted record, if necessary.
	IF @Error_No = 0
	BEGIN
		UPDATE StoreItemVendor
		SET DeleteWorkStation = CASE WHEN Inserted.DeleteDate IS NOT NULL THEN HOST_NAME() ELSE NULL END
		FROM StoreItemVendor SIV
		INNER JOIN 
		  Inserted 
		  ON Inserted.StoreItemVendorID = SIV.StoreItemVendorID
		INNER JOIN 
		  Deleted 
		  ON Deleted.StoreItemVendorID = Inserted.StoreItemVendorID
		WHERE ISNULL(Inserted.DeleteDate, 0) <> ISNULL(Deleted.DeleteDate, 0)

	    SELECT @Error_No = @@ERROR
	END
	
	-- Unset Primary status if Vendor is deleted.  
	IF @Error_No = 0
	BEGIN
		UPDATE StoreItemVendor
		SET PrimaryVendor = 0
		FROM StoreItemVendor SIV
		INNER JOIN 
		  Inserted 
		  ON Inserted.StoreItemVendorID = SIV.StoreItemVendorID
		WHERE Inserted.DeleteDate IS NOT NULL

	    SELECT @Error_No = @@ERROR
	END

    -- DETERMINE IF CURRENT REGION BATCHES VENDOR CHANGES --
    DECLARE @BatchVendorChanges bit
    SELECT @BatchVendorChanges = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'BatchVendorChanges'
    
    -- CREATE ITEM CHANGE TYPE PRICEBATCHDETAIL RECORD IF THIS REGION BATCHES VENDOR CHANGES
    IF @Error_No = 0 AND @BatchVendorChanges = 1
    BEGIN
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Inserted.Store_No, Inserted.Item_Key, 2, 'StoreItemVendorUpdate Trigger'
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Item_Key = Inserted.Item_Key	AND
               Deleted.Store_No = Inserted.Store_No AND
               Deleted.Vendor_ID = Inserted.Vendor_ID		
        WHERE Inserted.PrimaryVendor = 1	-- create records when the primary vendor changes
			AND (Inserted.PrimaryVendor <> Deleted.PrimaryVendor)
			AND (Inserted.DeleteDate IS NULL OR Inserted.DeleteDate > GetDate())
            AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Inserted.Store_No) = 0
            
        SELECT @Error_No = @@ERROR
    END
    
    	DECLARE @NewPrimaryVendor bit

	SELECT 
		@NewPrimaryVendor = CASE COUNT(*) WHEN 0 THEN 0 ELSE 1 END
	FROM Inserted
	INNER JOIN Deleted
		 ON Deleted.Item_Key = Inserted.Item_Key	AND
			   Deleted.Store_No = Inserted.Store_No AND
			   Deleted.Vendor_ID = Inserted.Vendor_ID
	WHERE
		Inserted.PrimaryVendor = 1	-- create records when the primary vendor changes
		AND (Inserted.PrimaryVendor <> Deleted.PrimaryVendor)
		AND (Inserted.DeleteDate IS NULL OR Inserted.DeleteDate > GetDate())
		
    
    IF @Error_No = 0 
    BEGIN
		IF @NewPrimaryVendor = 1
		BEGIN
			-- If there's a new primary vendor, disable the old one
			UPDATE StoreItemVendor
				SET PrimaryVendor = 0
				FROM 
					StoreItemVendor SIV
				INNER JOIN Inserted I ON 
					SIV.Store_No = I.Store_No
					AND
					SIV.Item_Key = I.Item_Key
					AND
					SIV.Vendor_ID <> I.Vendor_ID
					
			SELECT @Error_No = @@ERROR
		END
		ELSE
		BEGIN
			-- IF THERE IS NO LONGER A PRIMARY VENDOR FOR A STORE-ITEM DUE TO THIS CHANGE, THE ITEM SHOULD
			-- BE DE-AUTHORIZED FOR THE STORE
			UPDATE StoreItem
				SET	Authorized = 0
			FROM StoreItemVendor SIV
			INNER JOIN Inserted 
			  ON Inserted.StoreItemVendorID = SIV.StoreItemVendorID
			WHERE StoreItem.Item_Key = Inserted.Item_Key AND 
				  StoreItem.Store_No = Inserted.Store_No AND
				  dbo.fn_HasPrimaryVendor(Inserted.Item_Key, Inserted.Store_No) = 0

			SELECT @Error_No = @@ERROR
		END
	END

	-- Communicate discontinued items to PriceBatchDenorm 
	IF @error_no = 0
	BEGIN
		--INSERT INTO PriceBatchDenorm_Temp
	    INSERT INTO PriceBatchDenorm
	      (
	        Item_Key,
			Store_No,
			Check_Box_20,
			Text_10,
			DiscontinueItem
	      )
	    SELECT DISTINCT 
	           INSERTED.Item_Key,
	           INSERTED.Store_No,
			   1,
			   'DISCO',
			   INSERTED.DiscontinueItem
	    FROM   INSERTED
	           INNER JOIN DELETED
	                ON  DELETED.Item_Key = INSERTED.Item_Key
	    WHERE  (INSERTED.DiscontinueItem <> DELETED.DiscontinueItem )


		SELECT @Error_No = @@ERROR
	END
		
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('StoreItemVendorUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

