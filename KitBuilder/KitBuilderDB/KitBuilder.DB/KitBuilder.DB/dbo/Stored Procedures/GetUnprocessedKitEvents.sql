﻿CREATE PROCEDURE [dbo].[GetUnprocessedKitEvents]
AS
BEGIN
	SET NOCOUNT ON;

	WITH QkitLocaleId
	AS (
		SELECT TOP 1 kitLocaleId
		FROM dbo.KitQueue
		WHERE STATUS = 'U'
		)
	UPDATE kq
	SET STATUS = 'PR'
		,MessageTimestampUtc = GETUTCDATE()
	OUTPUT inserted.KitQueueId
		,inserted.KitID
		,inserted.StoreId
		,inserted.StoreName
		,inserted.VenueId
		,inserted.VenueName
		,inserted.kitLocaleId
		,inserted.Action
	FROM dbo.KitQueue kq WITH (
			UPDLOCK
			,READPAST
			)
	JOIN QkitLocaleId ki ON kq.kitLocaleId = ki.kitLocaleId
	WHERE STATUS = 'U'

	SET NOCOUNT OFF
END