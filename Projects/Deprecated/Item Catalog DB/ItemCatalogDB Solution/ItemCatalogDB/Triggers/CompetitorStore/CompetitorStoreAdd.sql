IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'CompetitorStoreAdd' 
	   AND 	  type = 'TR')
    DROP TRIGGER [CompetitorStoreAdd]
GO

CREATE TRIGGER [CompetitorStoreAdd]
ON [CompetitorStore]
FOR INSERT 
AS 
BEGIN
    DECLARE @Error_No int
    SET @Error_No = 0

	INSERT INTO
		CompetitorStoreIdentifier
	SELECT
		I.CompetitorStoreID, 
		I.Name
	FROM
		Inserted I

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('CompetitorStoreAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
 