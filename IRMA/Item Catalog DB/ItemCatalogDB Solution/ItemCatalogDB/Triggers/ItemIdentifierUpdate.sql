IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'ItemIdentifierUpdate')
	BEGIN
		PRINT 'Dropping Trigger ItemIdentifierUpdate'
		DROP  Trigger ItemIdentifierUpdate
	END
GO


PRINT 'Creating Trigger ItemIdentifierUpdate'
GO
CREATE Trigger ItemIdentifierUpdate 
ON ItemIdentifier
FOR UPDATE
AS
BEGIN
    DECLARE @Error_No int

	DECLARE @Identifier varchar(13) 
	
	SELECT @Identifier = Identifier FROM Inserted

	DECLARE @newItemChgTypeID tinyint
	SELECT @newItemChgTypeID = itemchgtypeid FROM itemchgtype WHERE itemchgtypedesc like 'new'

	DECLARE @EnableUPCIRMAToIConFlow bit
	SELECT  @EnableUPCIRMAToIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = 'IRMA Client' AND
			ack.Name = 'EnableUPCIRMAToIConFlow' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
	
	DECLARE @EnablePLUIRMAIConFlow bit
	SELECT @EnablePLUIRMAIConFlow = acv.Value
			FROM AppConfigValue acv INNER JOIN AppConfigEnv ace
			ON acv.EnvironmentID = ace.EnvironmentID 
			INNER JOIN AppConfigApp aca
			ON acv.ApplicationID = aca.ApplicationID 
			INNER JOIN AppConfigKey ack
			ON acv.KeyID = ack.KeyID 
			WHERE aca.Name = 'IRMA Client' AND
			ack.Name = 'EnablePLUIRMAIConFlow' and
			SUBSTRING(ace.Name,1,1) = SUBSTRING((SELECT Environment FROM Version WHERE ApplicationName = 'IRMA CLIENT'),1,1)
    

    SELECT @Error_No = 0

    IF EXISTS (SELECT *
                FROM Inserted
                INNER JOIN
                    Item ON Item.Item_Key = Inserted.Item_Key
                INNER JOIN
                    Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
                INNER JOIN
                    (SELECT Supplier_Store_No, SubTeam_No
                     FROM ZoneSubTeam Z (nolock)
                     INNER JOIN Store (nolock) ON Store.Store_No = Z.Supplier_Store_No
                     WHERE EXEWarehouse IS NOT NULL
                     GROUP BY Supplier_Store_No, SubTeam_No) ZS
                    ON ZS.SubTeam_No = Item.SubTeam_No
                LEFT JOIN
                    WarehouseItemChange Q
                    ON ZS.Supplier_Store_No = Q.Store_No AND Inserted.Item_Key = Q.Item_Key AND Q.ChangeType = 'A'
                WHERE Inserted.Default_Identifier <> Deleted.Default_Identifier
                    AND Item.EXEDistributed = 1
                    AND Q.WarehouseItemChangeID IS NULL)
    BEGIN
        ROLLBACK TRAN
        RAISERROR('Default Identifier cannot be changed for an EXE Distributed Item', 16, 1)
        RETURN
    END

    -- Queue for Price Modeling if necessary
    INSERT INTO PMProductChg (HierLevel, Item_Key, ItemID, ItemDescription, ParentID, ParentDescription, ActionID, Status)
    SELECT 'Product', Inserted.Item_Key, Inserted.Identifier, Item_Description, 
           ISNULL(ItemCategory.Category_ID, CONVERT(varchar(255), Item.SubTeam_No) + '1'), ISNULL(Category_Name, 'NO CATEGORY'), 
           'CHANGE', CASE WHEN dbo.fn_GetDiscontinueStatus(Inserted.Item_Key, NULL, NULL) = 1 THEN 'DISCONTINUED' ELSE 'ACTIVE' END
    FROM Inserted
    INNER JOIN
        Deleted
        ON Deleted.Identifier_ID = Inserted.Identifier_ID
    INNER JOIN
        Item
        ON Inserted.Item_Key = Item.Item_Key
    INNER JOIN 
        PMSubTeamInclude SI 
        ON SI.SubTeam_No = Item.SubTeam_No
    LEFT JOIN
        ItemCategory
        ON Item.Category_ID = ItemCategory.Category_ID
    WHERE Retail_Sale = 1
          AND NOT EXISTS (SELECT * FROM PMExcludedItem WHERE Item_Key = Item.Item_Key)
          AND Inserted.Default_Identifier = 1 AND Deleted.Default_Identifier = 0

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
		--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS THAT ARE DELETED
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 2, 'ItemIdentifierUpdate trigger'
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
            (SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
        WHERE 
            (Inserted.Default_Identifier = 1 AND Deleted.Default_Identifier = 0)
		AND dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key, Store.Store_No) = 0
        
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
		--UPDATE SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE DELETED OR MARKED AS NON-SCALE
        DELETE PLUMCorpChgQueue
        FROM PLUMCorpChgQueue
        INNER JOIN 
            Inserted
            ON Inserted.Item_Key = PLUMCorpChgQueue.Item_Key
        INNER JOIN
            Deleted
            ON Deleted.Identifier_ID = Inserted.Identifier_ID
        WHERE Deleted.Scale_Identifier = 1 AND Deleted.Deleted_Identifier = 0
            AND (Inserted.Deleted_Identifier = 1 OR Inserted.Scale_Identifier = 0)
    
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
		--INSERT SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE DELETED OR MARKED AS NON-SCALE
        INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
        SELECT Inserted.Item_Key, 'D', s.Store_No
        FROM 
			Inserted
        INNER JOIN
            Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN 
			Store s
        JOIN 
			StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
        WHERE Deleted.Scale_Identifier = 1 AND Deleted.Deleted_Identifier = 0
              AND (Inserted.Deleted_Identifier = 1 OR Inserted.Scale_Identifier = 0)
              AND s.WFM_Store = 1 AND si.Authorized = 1 AND
	          NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'D') AND
			  NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'D')

    
        SELECT @Error_No = @@ERROR
    END
    
    IF @Error_No = 0
    BEGIN
		--UPDATE SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE UNDELETED OR MARKED AS SCALE
        DELETE PLUMCorpChgQueue
        FROM PLUMCorpChgQueue
        INNER JOIN 
            Inserted ON Inserted.Item_Key = PLUMCorpChgQueue.Item_Key
        INNER JOIN
            Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
        WHERE Inserted.Scale_Identifier = 1 AND Inserted.Deleted_Identifier = 0
            AND (Deleted.Deleted_Identifier = 1 OR Deleted.Scale_Identifier = 0)
    
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
		--INSERT SCALE PUSH QUEUE DATA FOR ANY IDENTIFIERS THAT ARE DELETED OR MARKED AS NON-SCALE
        INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
        SELECT Inserted.Item_Key, 'A', s.Store_No
        FROM Inserted
        INNER JOIN
            Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
			Store s
		JOIN 
			StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
        WHERE Inserted.Scale_Identifier = 1 AND Inserted.Deleted_Identifier = 0
            AND (Deleted.Deleted_Identifier = 1 OR Deleted.Scale_Identifier = 0)
            AND s.WFM_Store = 1 AND si.Authorized = 1 AND
			NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') AND
			NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')

    
        SELECT @Error_No = @@ERROR
    END

	IF @Error_No = 0
    BEGIN
		--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS TO BE ADDED (FOR NON-TYPE-2 IDENTIFIERS)
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 2, 'ItemIdentifierUpdate trigger'
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
            (SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
        WHERE 
            (Inserted.Scale_Identifier = 1  AND Deleted.Scale_Identifier = 0) AND
			(SUBSTRING(Inserted.Identifier,1,1) <> '2')
		
        
        SELECT @Error_No = @@ERROR
    END

	IF @Error_No = 0
    BEGIN
		--INSERT PRICE BATCH DATA FOR ANY IDENTIFIERS TO BE DELETED (FOR NON-TYPE-2 IDENTIFIERS)
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 3, 'ItemIdentifierUpdate trigger'
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
            (SELECT Store_No FROM Store (nolock) WHERE WFM_Store = 1 OR Mega_Store = 1) Store
        WHERE 
            (Inserted.Deleted_Identifier = 1  AND Deleted.Deleted_Identifier = 0) AND
			(SUBSTRING(Inserted.Identifier,1,1) <> '2')
		
        
        SELECT @Error_No = @@ERROR
    END

	-- WHEN AN ALTERNATE IDENTIFIER IS PROMOTED FROM ALTERNATE TO DEFAULT, THE ITEM WILL BE SENT TO ICON 
	-- AS A NEW ITEM
	IF (@Error_No = 0)
		BEGIN

			IF	(@EnableUPCIRMAToIConFlow = 1 AND @EnablePLUIRMAIConFlow = 1) OR
				(@EnableUPCIRMAToIConFlow = 1 AND NOT (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000')) OR
				(@EnablePLUIRMAIConFlow = 1 AND (LEN(@Identifier) < 7 OR @Identifier LIKE '2%00000'))
			BEGIN
			

				insert into iConItemChangeQueue (
					Item_Key, 
					Identifier, 
					ItemChgTypeID
					)
					SELECT DISTINCT
						Item_Key = Inserted.Item_Key,
						Identifier = Inserted.Identifier,
						ItemChgTypeID = @newItemChgTypeID
					FROM Inserted INNER JOIN Deleted
					ON Inserted.Identifier_ID = Deleted.Identifier_ID
					WHERE Inserted.Default_Identifier <> Deleted.Default_Identifier
					AND Inserted.Default_Identifier = 1 

			END
			SELECT @Error_No = @@ERROR
		END

    IF @error_no = 0
    BEGIN
		--INSERT A SCALE "CHANGE" (C) IF THE ItemIdentifier.NumPluDigitsSentToScale CHANGES
		--CURRENTLY NO OTHER FIELDS IN ItemIdentifier TRIGGERS A SCALE/POS BATCH
        INSERT INTO PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
        SELECT Inserted.Item_Key, 'C', s.Store_No
        FROM 
			Inserted
        INNER JOIN
            Deleted ON Deleted.Identifier_ID = Inserted.Identifier_ID
        CROSS JOIN
			Store s
		JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
        WHERE Inserted.Remove_Identifier = 0 AND Inserted.Deleted_Identifier = 0 
            AND ISNULL(Inserted.NumPluDigitsSentToScale, '') <> ISNULL(Deleted.NumPluDigitsSentToScale, '')
            AND Inserted.Scale_Identifier = 1 --ONLY INSERT SCALE IDENTIFIERS THAT ARE MEANT TO BE SENT TO SCALES
			AND s.WFM_Store = 1 AND si.Authorized = 1
			AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C')
			AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C')

            
        SELECT @error_no = @@ERROR
    END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ItemIdentifierUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO