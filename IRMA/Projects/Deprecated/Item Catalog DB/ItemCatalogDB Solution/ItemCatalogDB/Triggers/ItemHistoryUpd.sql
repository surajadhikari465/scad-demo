IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ItemHistoryUpd' 
	   AND 	  type = 'TR')
    DROP TRIGGER ItemHistoryUpd
GO

CREATE TRIGGER ItemHistoryUpd
ON ItemHistory
FOR UPDATE
AS 
BEGIN
    RAISERROR('ItemHistory cannot be updated - insert and delete only', 16, 1)
    ROLLBACK TRAN
END
GO
