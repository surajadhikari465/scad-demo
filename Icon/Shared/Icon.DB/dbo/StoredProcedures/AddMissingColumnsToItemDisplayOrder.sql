CREATE PROCEDURE dbo.AddMissingColumnsToItemColumnDisplayOrder
AS
BEGIN
	INSERT INTO ItemColumnDisplayOrder (
		 columntype
		,referenceid
		,displayorder
		)
	SELECT 'Hierarchy'
		,HIERARCHYID
		,9999
	FROM Hierarchy h
	LEFT JOIN ItemColumnDisplayOrder ido ON h.HIERARCHYID = ido.referenceid
		AND ido.columntype = 'hierarchy'
	WHERE referenceId IS NULL
	AND h.hierarchyName not in ('Certification Agency Management', 'Browsing')

	INSERT INTO ItemColumnDisplayOrder (
		columntype
		,referenceid
		,displayorder
		)
	SELECT 'Attribute'
		,attributeid
		,9999
	FROM Attributes a
	INNER JOIN AttributeGroup ag on a.AttributeGroupId = ag.AttributeGroupId
	LEFT JOIN ItemColumnDisplayOrder ido ON a.attributeid = ido.referenceid
		AND ido.columntype = 'attribute'
	WHERE referenceId IS NULL AND
	ag.AttributeGroupName = 'Global Item'

	if not exists (select 1 from dbo.ItemColumnDisplayOrder where ColumnType = 'Other' and ReferenceId = 1 )
	insert into dbo.ItemColumnDisplayOrder (ColumnType, ReferenceId, ReferenceName, DisplayOrder)
	values ('Other', 1, 'Item Id', 9999)

	if not exists (select 1 from dbo.ItemColumnDisplayOrder where ColumnType = 'Other' and ReferenceId = 2 )
	insert into dbo.ItemColumnDisplayOrder (ColumnType, ReferenceId, ReferenceName, DisplayOrder)
	values ('Other', 2, 'Item Type', 9999)

	if not exists (select 1 from dbo.ItemColumnDisplayOrder where ColumnType = 'Other' and ReferenceId = 3 )
	insert into dbo.ItemColumnDisplayOrder (ColumnType, ReferenceId, ReferenceName, DisplayOrder)
	values ('Other', 3, 'Barcode Type', 9999)

	if not exists (select 1 from dbo.ItemColumnDisplayOrder where ColumnType = 'Other' and ReferenceId = 4 )
	insert into dbo.ItemColumnDisplayOrder (ColumnType, ReferenceId, ReferenceName, DisplayOrder)
	values ('Other', 4, 'Scancode', 9999)

	if not exists (select 1 from dbo.ItemColumnDisplayOrder where ColumnType = 'Other' and ReferenceId = 5 )
	insert into dbo.ItemColumnDisplayOrder (ColumnType, ReferenceId, ReferenceName, DisplayOrder)
	values ('Other', 5, 'Modified On', 9999)

	if not exists (select 1 from dbo.ItemColumnDisplayOrder where ColumnType = 'Other' and ReferenceId = 6 )
	insert into dbo.ItemColumnDisplayOrder (ColumnType, ReferenceId, ReferenceName, DisplayOrder)
	values ('Other', 6, 'Created On', 9999)

END