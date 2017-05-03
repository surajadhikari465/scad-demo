IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ZoneAddUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER ZoneAddUpdate
GO

CREATE TRIGGER ZoneAddUpdate
ON Zone
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMOrganizationChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'Zone', Inserted.Zone_ID, Inserted.Zone_Name, Region.Region_ID, RegionName,
           CASE WHEN (Deleted.Zone_ID IS NULL) OR (Inserted.Region_ID IS NOT NULL AND Deleted.Region_ID IS NULL)
                     THEN 'ADD'
                ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Zone_ID = Deleted.Zone_ID
    LEFT JOIN
        Region
        ON ISNULL(Inserted.Region_ID, Deleted.Region_ID) = Region.Region_ID
    WHERE ((ISNULL(Deleted.Zone_Name, '') <> ISNULL(Inserted.Zone_Name, ''))
           OR (ISNULL(Deleted.Region_ID, 0) <> ISNULL(Inserted.Region_ID, 0)))
          AND (Deleted.Region_ID IS NOT NULL OR Inserted.Region_ID IS NOT NULL)

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('ZoneAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

