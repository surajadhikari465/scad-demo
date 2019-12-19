CREATE PROCEDURE [dbo].[AddUpdateItemSubscription]
@items app.IRMAItemType READONLY
AS
BEGIN

	MERGE app.IRMAItemSubscription iis
	USING @items i
	ON iis.RegionCode = i.RegionCode
		AND iis.Identifier = i.Identifier
	WHEN MATCHED THEN
		UPDATE 
		SET insertDate = GETDATE(),
			deleteDate = NULL
	WHEN NOT MATCHED THEN
		INSERT (regioncode, identifier, insertDate, deleteDate)
		VALUES (RegionCode, Identifier, GETDATE(), NULL);

END