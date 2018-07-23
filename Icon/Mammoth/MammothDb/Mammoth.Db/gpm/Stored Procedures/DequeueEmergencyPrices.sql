CREATE PROCEDURE gpm.DequeueEmergencyPrices
	@emergencyPriceCount INT
AS
BEGIN
	DECLARE @businessUnitId INT = (SELECT TOP 1 BusinessUnitId FROM gpm.MessageQueueEmergencyPrice);

	WITH EmergencyPriceTable
     AS
	 (  SELECT TOP (@emergencyPriceCount) Itemid, BusinessUnitId, PriceType,MammothPriceXml,InsertDateUtc 
	    FROM gpm.MessageQueueEmergencyPrice
		WHERE BusinessUnitId = @businessUnitId
		ORDER BY BusinessUnitId, ItemId, PriceType
	  )

	DELETE FROM EmergencyPriceTable
	OUTPUT deleted.ItemId, deleted.BusinessUnitId, deleted.MammothPriceXml
END
GO

GRANT EXEC ON [gpm].[DequeueEmergencyPrices] TO TibcoRole
GO