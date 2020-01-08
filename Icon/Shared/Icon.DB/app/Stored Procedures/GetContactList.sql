--Formatted with PoorSQL
CREATE PROCEDURE [app].[GetContactList]
    @hierarchyClassID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (IsNull(@hierarchyClassID, 0) <= 0)
		SET @hierarchyClassID = NULL;

	SELECT c.ContactId
		,c.ContactTypeId
		,c.ContactName
		,c.Email
		,c.Title
		,c.AddressLine1
		,c.AddressLine2
		,c.City
		,c.STATE
		,c.ZipCode
		,c.Country
		,c.PhoneNumber1
		,c.PhoneNumber2
		,c.WebsiteURL
		,ct.ContactTypeName
		,c.HierarchyClassId
		,hc.hierarchyClassName
		,h.hierarchyName
	FROM Contact c
	JOIN ContactType ct ON ct.ContactTypeId = c.ContactTypeId
	JOIN dbo.HierarchyClass hc ON hc.hierarchyClassID = c.HierarchyClassID
	JOIN dbo.Hierarchy h ON h.HIERARCHYID = hc.HIERARCHYID
	WHERE IsNull(ct.Archived, 0) = 0 AND c.HierarchyClassId = IsNull(@hierarchyClassID, c.HierarchyClassId);
END