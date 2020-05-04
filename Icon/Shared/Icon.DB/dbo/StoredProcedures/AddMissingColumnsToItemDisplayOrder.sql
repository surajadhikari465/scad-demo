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

	INSERT INTO ItemColumnDisplayOrder (
		columntype
		,referenceid
		,displayorder
		)
	SELECT 'Attribute'
		,attributeid
		,9999
	FROM Attributes a
	LEFT JOIN ItemColumnDisplayOrder ido ON a.attributeid = ido.referenceid
		AND ido.columntype = 'attribute'
	WHERE referenceId IS NULL

END