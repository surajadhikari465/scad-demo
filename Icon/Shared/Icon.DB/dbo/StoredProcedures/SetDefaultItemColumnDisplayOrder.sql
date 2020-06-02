CREATE PROCEDURE dbo.SetDefaultItemColumnDisplayOrder
AS
BEGIN

	-- alphabetize everything first.
	;WITH cte
	AS (
		SELECT ColumnType
			,ReferenceId
			,ReferenceName
			,row_number() OVER (
				ORDER BY ReferenceName
				)  + 1000 AS DisplayOrder
		FROM (
			SELECT ColumnType
				,ReferenceId
				,h.HierarchyName ReferenceName
				,d.DisplayOrder
			FROM dbo.ItemColumnDisplayOrder d
			INNER JOIN dbo.Hierarchy h ON d.ReferenceId = h.HIERARCHYID
			WHERE d.ColumnType = 'Hierarchy' AND 
			h.hierarchyName not in ('Certification Agency Management', 'Browsing')
		
			UNION ALL
		
			SELECT ColumnType
				,ReferenceId
				,a.DisplayName ReferenceName
				,d.DisplayOrder
			FROM dbo.ItemColumnDisplayOrder d
			INNER JOIN dbo.Attributes a ON d.ReferenceId = a.AttributeId
			INNER JOIN dbo.AttributeGroup ag on a.AttributeGroupId = ag.AttributeGroupId
			WHERE d.ColumnType = 'Attribute' AND 
				  ag.AttributeGroupName = 'Global Item'
		
			UNION ALL
		
			SELECT ColumnType
				,ReferenceId
				,ReferenceName
				,d.DisplayOrder
			FROM dbo.ItemColumnDisplayOrder d
			WHERE d.ColumnType = 'Other'
			) p1
		)
	UPDATE icd
	SET DisplayOrder = cte.DisplayOrder
	FROM cte
	INNER JOIN ItemColumnDisplayOrder icd ON cte.referenceid = icd.ReferenceId
		AND cte.columntype = icd.ColumnType

	-- set defaults.
	UPDATE dbo.ItemColumnDisplayOrder
	SET displayorder = 1
	WHERE columntype = 'Other'
		AND ReferenceName = 'Item Id'

	UPDATE dbo.ItemColumnDisplayOrder
	SET displayorder = 2
	WHERE columntype = 'Other'
		AND ReferenceName = 'Item Type'

	UPDATE icd
	SET displayorder = 3
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'PosScaleTare'

	UPDATE icd
	SET displayorder = 4
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'Inactive'

	UPDATE dbo.ItemColumnDisplayOrder
	SET displayorder = 5
	WHERE columntype = 'Other'
		AND ReferenceName = 'Barcode Type'

	UPDATE icd
	SET displayorder = 6
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'RequestNumber'

	UPDATE dbo.ItemColumnDisplayOrder
	SET displayorder = 7
	WHERE columntype = 'Other'
		AND ReferenceName = 'Scancode'

	UPDATE icd
	SET displayorder = 8
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN Hierarchy h ON icd.ReferenceId = h.HIERARCHYID
	WHERE columntype = 'Hierarchy'
		AND h.HierarchyName = 'Brands'

	UPDATE icd
	SET displayorder = 9
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'ProductDescription'

	UPDATE icd
	SET displayorder = 10
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'POSDescription'

	UPDATE icd
	SET displayorder = 11
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'CustomerFriendlyDescription'

	UPDATE icd
	SET displayorder = 12
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'ItemPack'

	UPDATE icd
	SET displayorder = 13
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'RetailSize'

	UPDATE icd
	SET displayorder = 14
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'UOM'

	UPDATE icd
	SET displayorder = 15
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN Hierarchy h ON icd.ReferenceId = h.HIERARCHYID
	WHERE columntype = 'Hierarchy'
		AND h.HierarchyName = 'Financial'

	UPDATE icd
	SET displayorder = 16
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN Hierarchy h ON icd.ReferenceId = h.HIERARCHYID
	WHERE columntype = 'Hierarchy'
		AND h.HierarchyName = 'Merchandise'

	UPDATE icd
	SET displayorder = 17
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN Hierarchy h ON icd.ReferenceId = h.HIERARCHYID
	WHERE columntype = 'Hierarchy'
		AND h.HierarchyName = 'National'

	UPDATE icd
	SET displayorder = 18
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN Hierarchy h ON icd.ReferenceId = h.HIERARCHYID
	WHERE columntype = 'Hierarchy'
		AND h.HierarchyName = 'Tax'

	UPDATE icd
	SET displayorder = 19
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'FoodStampEligible'

	UPDATE icd
	SET displayorder = 20
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'DataSource'

	UPDATE icd
	SET displayorder = 21
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN Hierarchy h ON icd.ReferenceId = h.HIERARCHYID
	WHERE columntype = 'Hierarchy'
		AND h.HierarchyName = 'Manufacturer'

	UPDATE icd
	SET displayorder = 22
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'Notes'

	UPDATE icd
	SET displayorder = 23
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'CreatedBy'

	UPDATE icd
	SET displayorder = 24
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'CreatedDateTimeUtc'

	UPDATE icd
	SET displayorder = 25
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'ModifiedBy'

	UPDATE icd
	SET displayorder = 26
	FROM dbo.ItemColumnDisplayOrder icd
	INNER JOIN attributes a ON icd.ReferenceId = a.AttributeId
	WHERE columntype = 'Attribute'
		AND a.AttributeName = 'ModifiedDateTimeUtc'
END
GO