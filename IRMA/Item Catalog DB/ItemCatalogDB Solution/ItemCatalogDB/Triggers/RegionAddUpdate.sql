IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'RegionAddUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER RegionAddUpdate
GO

CREATE TRIGGER RegionAddUpdate
ON Region
FOR INSERT, UPDATE 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	INSERT INTO PMOrganizationChg (HierLevel, ItemID, ItemDescription, ActionID)
    SELECT 'Region', CONVERT(varchar(255), Inserted.Region_ID), Inserted.RegionName,
           CASE WHEN Deleted.Region_ID IS NULL THEN 'ADD' ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Region_ID = Deleted.Region_ID
    WHERE ISNULL(Deleted.RegionName, '') <> ISNULL(Inserted.RegionName, '')

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('RegionAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

