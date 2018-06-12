CREATE PROCEDURE gpm.DequeueEmergencyPrices
	@emergencyPriceCount INT
AS
BEGIN
	DECLARE @businessUnitId INT = (SELECT TOP 1 BusinessUnitId FROM gpm.MessageQueueEmergencyPrice)

	DELETE TOP (@emergencyPriceCount)
	FROM gpm.MessageQueueEmergencyPrice
		OUTPUT deleted.ItemId, deleted.BusinessUnitId, deleted.MammothPriceXml
	WHERE BusinessUnitId = @businessUnitId
END
GO

GRANT EXEC ON [gpm].[DequeueEmergencyPrices] TO TibcoRole
GO