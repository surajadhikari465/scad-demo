IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'StoreAddUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER StoreAddUpdate
GO

CREATE TRIGGER StoreAddUpdate
ON Store
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMOrganizationChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'Store', Inserted.Store_No, Inserted.Store_Name, Zone.Zone_ID, Zone_Name,
           CASE WHEN (Deleted.Store_No IS NULL) OR ((ISNULL(Deleted.Mega_Store, 0) = 0 AND ISNULL(Deleted.WFM_Store, 0) = 0) AND (Inserted.Mega_Store = 1 OR Inserted.WFM_Store = 1))
                     THEN 'ADD'
                ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Store_No = Deleted.Store_No
    LEFT JOIN
        Zone
        ON Inserted.Zone_ID = Zone.Zone_ID
    WHERE ((ISNULL(Deleted.Store_Name, '') <> ISNULL(Inserted.Store_Name, ''))
           OR (ISNULL(Deleted.Zone_ID, 0) <> ISNULL(Inserted.Zone_ID, 0)))
          AND (ISNULL(Deleted.Mega_Store, 0) = 1 OR ISNULL(Deleted.WFM_Store, 0) = 1 OR Inserted.Mega_Store = 1 OR Inserted.WFM_Store = 1)

    SELECT @Error_No = @@ERROR

    --IF @Error_No = 0
    --BEGIN
    --    INSERT INTO VendorExportQueue
    --    SELECT Vendor_ID
    --    FROM Vendor
    --	INNER JOIN
    --	    Inserted
    --        On Inserted.Store_No = Vendor.Store_No
    --	LEFT JOIN
    --	    Deleted
    --	    On Inserted.Store_No = Deleted.Store_No
    --    WHERE (Inserted.Store_Name <> ISNULL(Deleted.Store_Name, '') AND (Inserted.BusinessUnit_ID IS NOT NULL))
    --        OR (ISNULL(Inserted.BusinessUnit_ID, 0) <> ISNULL(Deleted.BusinessUnit_ID, 0))
    --
    --    SELECT @Error_No = @@ERROR
    --END 

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('StoreAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

