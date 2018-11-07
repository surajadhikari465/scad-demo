
CREATE VIEW [dbo].[UploadTypeAttributeView]
AS
	SELECT
		UploadTypeAttribute.[UploadTypeAttribute_ID],
		UploadTypeAttribute.[UploadType_Code],
		UploadTypeAttribute.[UploadAttribute_ID],
		UploadTypeAttribute.[IsRequiredForUploadTypeForExistingItems],
		UploadTypeAttribute.[IsRequiredForUploadTypeForNewItems],
		UploadTypeAttribute.[IsReadOnlyForExistingItems],
		UploadTypeAttribute.[IsReadOnlyForNewItems],
		UploadTypeAttribute.[IsHidden],
		UploadTypeAttribute.[GridPosition],
		UploadTypeAttribute.[GroupName]
	FROM UploadTypeAttribute (NOLOCK)
	UNION
	SELECT
		AttributeIdentifier_ID + 1000 AS UploadTypeAttribute_ID,
		'ITEM_MAINTENANCE' AS UploadType_Code,
		AttributeIdentifier_ID + 10000 AS UploadAttribute_ID,
		CAST(0 AS bit) AS IsRequiredForUploadTypeForExistingItems,
		CAST(0 AS bit) AS IsRequiredForUploadTypeForNewItems,
		CAST(0 AS bit) AS IsReadOnlyForExistingItems,
		CAST(0 AS bit) AS IsReadOnlyForNewItems,
		CAST(0 AS bit) AS IsHidden,
		AttributeIdentifier_ID + 1000 AS GridPosition,
		'Flex Attributes' AS GroupName
	FROM AttributeIdentifier (NOLOCK)
	WHERE Screen_Text IS NOT NULL AND LEN(Screen_Text) > 0

GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeAttributeView] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UploadTypeAttributeView] TO [IRMAReportsRole]
    AS [dbo];

