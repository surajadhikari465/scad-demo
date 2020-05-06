CREATE  PROCEDURE dbo.UpdateItemColumnDisplayOrder
	@DisplayOrderData dbo.ItemColumnDisplayOrderType readonly
AS
BEGIN
	UPDATE ItemColumnDisplayOrder
	SET DisplayOrder = updateData.OrderId
	FROM (
		SELECT ColumnType
			,ReferenceId
			,HierarchyName AS ReferenceName
			,OrderId
		FROM @DisplayOrderData d
		INNER JOIN Hierarchy h ON d.referenceid = h.HIERARCHYID
		WHERE ColumnType = 'Hierarchy'
		and h.hierarchyName not in ('Certification Agency Management', 'Browsing')
	
		UNION ALL
	
		SELECT ColumnType
			,ReferenceId
			,AttributeName AS ReferenceName
			,OrderId
		FROM @DisplayOrderData d
		INNER JOIN Attributes a ON d.referenceid = a.AttributeId
		WHERE ColumnType = 'Attribute'
		) updateData
	WHERE updateData.ColumnType = ItemColumnDisplayOrder.ColumnType
		AND updatedata.ReferenceId = ItemColumnDisplayOrder.ReferenceId
END