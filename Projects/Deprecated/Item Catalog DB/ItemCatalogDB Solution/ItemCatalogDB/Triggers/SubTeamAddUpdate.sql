IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'SubTeamAddUpdate' 
	   AND 	  type = 'TR')
    DROP TRIGGER SubTeamAddUpdate
GO

CREATE TRIGGER SubTeamAddUpdate
ON SubTeam
FOR INSERT, UPDATE
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    INSERT INTO PMProductChg (HierLevel, ItemID, ItemDescription, ParentID, ParentDescription, ActionID)
    SELECT 'SubTeam', CONVERT(varchar(255), Inserted.SubTeam_No), Inserted.SubTeam_Name, CONVERT(varchar(255), Inserted.Team_No),
           Team.Team_Name, CASE WHEN Deleted.SubTeam_No IS NULL THEN 'ADD' ELSE 'CHANGE' END
    FROM Inserted
    INNER JOIN
        Team
        ON Team.Team_No = Inserted.Team_No
    LEFT JOIN
        Deleted
        ON Inserted.SubTeam_No = Deleted.SubTeam_No
    WHERE (ISNULL(Deleted.SubTeam_Name, '') <> ISNULL(Inserted.SubTeam_Name, ''))
          OR (ISNULL(Deleted.Team_No, 0) <> ISNULL(Inserted.Team_No, 0))

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('SubTeamAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

