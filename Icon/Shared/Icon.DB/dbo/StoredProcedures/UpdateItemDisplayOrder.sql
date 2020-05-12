CREATE PROCEDURE dbo.UpdateItemColumnDisplayOrder @DisplayOrderData dbo.ItemColumnDisplayOrderType readonly
AS
BEGIN
	UPDATE ItemColumnDisplayOrder
	SET DisplayOrder = updateData.OrderId
	FROM @DisplayOrderData updateData
	WHERE updateData.ColumnType = ItemColumnDisplayOrder.ColumnType
		AND updatedata.ReferenceId = ItemColumnDisplayOrder.ReferenceId
END