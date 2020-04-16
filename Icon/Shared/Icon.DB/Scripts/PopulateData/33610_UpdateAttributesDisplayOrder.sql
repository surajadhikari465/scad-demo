DECLARE @updateAttributesDisplayOrderScriptKey VARCHAR(128) = '33610_UpdateAttributesDisplayOrder'

IF(NOT EXISTS(SELECT 1 FROM app.PostDeploymentScriptHistory WHERE ScriptKey = @updateAttributesDisplayOrderScriptKey))
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + @updateAttributesDisplayOrderScriptKey;

-- update Displayorder for first 16 attributes

UPDATE attributes
SET DisplayOrder = CASE 
		WHEN DisplayName = 'POS Scale Tare'
			THEN 1
		WHEN DisplayName = 'Inactive'
			THEN 2
		WHEN DisplayName = 'Request Number'
			THEN 3
		WHEN DisplayName = 'Product Description'
			THEN 4
		WHEN DisplayName = 'POS Description'
			THEN 5
		WHEN DisplayName = 'Customer Friendly Description'
			THEN 6
		WHEN DisplayName = 'Item Pack'
			THEN 7
		WHEN DisplayName = 'Retail Size'
			THEN 8
		WHEN DisplayName = 'UOM'
			THEN 9
		WHEN DisplayName = 'Food Stamp Eligible'
			THEN 10
		WHEN DisplayName = 'Data Source'
			THEN 11
		WHEN DisplayName = 'Notes'
			THEN 12
		WHEN DisplayName = 'Created By'
			THEN 13
		WHEN DisplayName = 'Created On'
			THEN 14
		WHEN DisplayName = 'Modified By'
			THEN 15
		WHEN DisplayName = 'Modified On'
			THEN 16
		END
WHERE DisplayName IN (
		'Request Number'
		,'Inactive'
		,'Product Description'
		,'POS Description'
		,'Customer Friendly Description'
		,'Item Pack'
		,'Retail Size'
		,'UOM'
		,'Food Stamp Eligible'
		,'Notes'
		,'Data Source'
		,'POS Scale Tare'
		,'Created By'
		,'Created On'
		,'Modified By'
		,'Modified On'
		);

-- update attributes displayorder for the remaining
WITH UpdateData
AS (
	SELECT DisplayName
		,ROW_NUMBER() OVER (
			ORDER BY DisplayName
			) + 16 AS RN
	FROM Attributes
	WHERE DisplayName NOT IN (
			'Request Number'
			,'Inactive'
			,'Product Description'
			,'POS Description'
			,'Customer Friendly Description'
			,'Item Pack'
			,'Retail Size'
			,'UOM'
			,'Food Stamp Eligible'
			,'Notes'
			,'Data Source'
			,'POS Scale Tare'
			,'Created By'
			,'Created On'
			,'Modified By'
			,'Modified On'
			)
		AND attributegroupid = (
			SELECT Attributegroupid
			FROM attributegroup
			WHERE AttributeGroupName = 'Global Item'
			)
	)
UPDATE Attributes
SET DisplayOrder = RN
FROM Attributes
INNER JOIN UpdateData ON Attributes.DisplayName = UpdateData.DisplayName
	
	INSERT INTO app.PostDeploymentScriptHistory (ScriptKey, RunTime) values (@updateAttributesDisplayOrderScriptKey, GETDATE())

END
ELSE
BEGIN
	PRINT '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Pop-data already applied: ' + @updateAttributesDisplayOrderScriptKey
END
GO