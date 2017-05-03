IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'TeamAddUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER TeamAddUpdate
GO

CREATE TRIGGER TeamAddUpdate
ON Team
FOR INSERT, UPDATE
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ActionID)
    SELECT 'Team', CONVERT(varchar(255), Inserted.Team_No), Inserted.Team_Name,
           CASE WHEN Deleted.Team_No IS NULL THEN 'ADD' ELSE 'CHANGE' END
    FROM Inserted
    LEFT JOIN
        Deleted
        ON Inserted.Team_No = Deleted.Team_No
    WHERE ISNULL(Deleted.Team_Name, '') <> ISNULL(Inserted.Team_Name, '')

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('TeamAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

