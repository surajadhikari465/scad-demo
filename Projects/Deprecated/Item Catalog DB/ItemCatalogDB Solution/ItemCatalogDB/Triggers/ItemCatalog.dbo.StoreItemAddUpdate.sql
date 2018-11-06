IF EXISTS (SELECT name FROM   sysobjects 
	   WHERE  name = N'StoreItemAddUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER StoreItemAddUpdate
GO

CREATE TRIGGER StoreItemAddUpdate
ON StoreItem
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    
    -- WHEN THE ITEM AUTHORIZATION FOR A STORE CHANGES FROM DE-AUTHORIZED TO AUTHORIZED:
    --		1. If the de-auth has already been communicated to the POS (StoreItem.POSDeAuth = 0),
    --				a 'New' item change type is inserted into PBD to send the auth as a new item record to the POS;
    --				if there is already an 'Item' item change type record in PBD, it is updated from 'Item' to 'New' change type
    --		   If the de-auth has not been communicated to the POS (StoreItem.POSDeAuth = 1),
    --				no changes are made to PBD
    --		2. For scale items, if the de-auth has already been communicated to the scale (StoreItem.ScaleDeAuth = 0),
    --				the StoreItem.ScaleAuth is set to 1 to send the auth to the scale
    --		   If the de-auth has not been communicated to the scale (StoreItem.ScaleDeAuth = 1),
    --				no changes are made to StoreItem.ScaleAuth
    --		3. The StoreItem.POSDeAuth and StoreItem.ScaleDeAuth flags are set to 0
    
    -- Communicate authorization to the POS if necessary (#1)
    IF @Error_No = 0
    BEGIN
		INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication,ReAuthFlag)
			SELECT Inserted.Store_No, Inserted.Item_Key, 1, 'StoreItemAddUpdate Trigger',Inserted.Authorized
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN Store (nolock) ON
				Store.Store_No = Inserted.Store_No
			WHERE (WFM_Store = 1 OR Mega_Store = 1)
			AND Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
			AND Inserted.Authorized = 1						-- the item is now authorized
			AND Deleted.POSDeAuth = 0						-- the de-auth was already communicated to the POS
			AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key,Inserted.Store_No) = 0
							
		SELECT @Error_No = @@ERROR
	END
 
    IF @Error_No = 0
    BEGIN
		UPDATE PriceBatchDetail 
			SET ItemChgTypeID = 1,ReAuthFlag = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN PriceBatchDetail PBD ON
				PBD.Item_Key = Inserted.Item_Key
				AND PBD.Store_No = Inserted.Store_No
				AND PBD.ItemChgTypeID = 2
				AND PriceBatchHeaderID IS NULL
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 1					-- the item is now authorized
				AND Deleted.POSDeAuth = 0					-- the de-auth was already communicated to the POS
							
		SELECT @Error_No = @@ERROR
	END
          
    -- Communicate authorization to the scale if necessary (#2)          
    IF @Error_No = 0
    BEGIN
		UPDATE StoreItem SET
				ScaleAuth = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN ItemIdentifier ON
				ItemIdentifier.Item_Key = Inserted.Item_Key
				AND ItemIdentifier.Scale_Identifier = 1		-- this is a scale item
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 1					-- the item is now authorized
				AND Deleted.ScaleDeAuth = 0					-- the de-auth was already communicated to the scale
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No
							
		SELECT @Error_No = @@ERROR
	END

	-- And add it to PLUMCorpChgQueue         
    IF @Error_No = 0
    BEGIN
    INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		SELECT 
			Inserted.Item_Key, 'A', Inserted.Store_No
		FROM 
			Inserted
		INNER JOIN Deleted ON
			Deleted.Item_Key = Inserted.Item_Key 
			AND Deleted.Store_No = Inserted.Store_No
		INNER JOIN ItemIdentifier ON
			ItemIdentifier.Item_Key = Inserted.Item_Key
			AND ItemIdentifier.Scale_Identifier = 1		-- this is a scale item
		INNER JOIN Store S ON
			Inserted.Store_No = S.Store_No
		WHERE 
			(S.WFM_Store = 1 OR S.Mega_Store = 1)
			AND Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
			AND Inserted.Authorized = 1						-- the item is now authorized
			AND Deleted.ScaleDeAuth = 0						
			AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND Store_No = Inserted.Store_No AND ActionCode = 'A')
			AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND Store_No = Inserted.Store_No AND ActionCode = 'A')
		SELECT @Error_No = @@ERROR
	END

	-- Clean up the de-auth flags (#3)
    IF @Error_No = 0
    BEGIN
		UPDATE StoreItem SET
				POSDeAuth = 0,
				ScaleDeAuth = 0
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			WHERE Inserted.Authorized = 1 -- the item is authorized
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No
							
		SELECT @Error_No = @@ERROR
	END
	
    -- WHEN THE ITEM AUTHORIZATION FOR A STORE CHANGES FROM AUTHORIZED TO DE-AUTHORIZED:
    --		1. Sets StoreItem.POSDeAuth = 1 to communicate the de-auth to the POS 
    --		2. For scale items, sets StoreItem.ScaleDeAuth = 1 to send the de-auth to the scale
    --		3. The StoreItem.ScaleAuth flags is set to 0

    -- Communicate de-authorization to the POS if necessary (#1)
    IF @Error_No = 0
    BEGIN
		UPDATE StoreItem SET
				POSDeAuth = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 0					-- the item is now de-authorized
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No
							
		SELECT @Error_No = @@ERROR
	END

    -- Communicate de-authorization to the scale if necessary (#2)
    IF @Error_No = 0
    BEGIN
		UPDATE StoreItem SET
				ScaleDeAuth = 1
			FROM Inserted
			INNER JOIN Deleted ON
				Deleted.Item_Key = Inserted.Item_Key 
				AND Deleted.Store_No = Inserted.Store_No
			INNER JOIN ItemIdentifier ON
				ItemIdentifier.Item_Key = Inserted.Item_Key
				AND ItemIdentifier.Scale_Identifier = 1		-- this is a scale item
			WHERE Inserted.Authorized <> Deleted.Authorized	-- the authorization changed
				AND Inserted.Authorized = 0					-- the item is now de-authorized
				AND Deleted.ScaleAuth = 0					-- the auth was already communicated to the scale
				AND StoreItem.Item_Key = Inserted.Item_Key
				AND StoreItem.Store_No = Inserted.Store_No
							
		SELECT @Error_No = @@ERROR
	END

	-- Clean up the auth flags (#3)
    IF @Error_No = 0
    BEGIN
		UPDATE StoreItem SET
				ScaleAuth = 0
			FROM StoreItem
			INNER JOIN Inserted ON
				StoreItem.Item_Key = Inserted.Item_Key 
				AND StoreItem.Store_No = Inserted.Store_No
			WHERE Inserted.Authorized = 0					-- the item is de-authorized
							
		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('StoreItemAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
 