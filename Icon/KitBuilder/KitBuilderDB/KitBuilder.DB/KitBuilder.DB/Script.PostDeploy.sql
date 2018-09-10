
-- Populate Status Table.
SET IDENTITY_INSERT dbo.Status ON;
MERGE INTO Status AS Target
USING(VALUES(0, N'DIS', N'Disabled'),
			(1, N'ENA', N'Enabled')) 
AS Source(StatusID, StatusCode, StatusDescription)
ON Target.StatusID=Source.StatusID
--update matched rows
WHEN MATCHED THEN UPDATE SET
                      Target.StatusCode=Source.StatusCode,
                      Target.StatusDescription=Source.StatusDescription
--insert new rows
WHEN NOT MATCHED BY TARGET THEN INSERT(StatusID, StatusCode, StatusDescription)
                                VALUES(StatusID, StatusCode, StatusDescription);
SET IDENTITY_INSERT dbo.Status OFF;