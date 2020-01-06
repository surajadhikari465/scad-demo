--Formatted with PoorSQL
CREATE PROCEDURE [app].[AddUpdateContact]
	@contact app.ContactInputType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	IF (object_id('tempdb..#contact') IS NOT NULL)
		DROP TABLE #contact;

	WITH cte AS (
		SELECT c.ContactId,
			c.HierarchyClassId,
			c.ContactTypeId,
			c.ContactName,
			c.Email,
			c.Title,
			c.AddressLine1,
			c.AddressLine2,
			c.City,
			c.State,
			c.ZipCode,
			c.Country,
			c.PhoneNumber1,
			c.PhoneNumber2,
			c.WebsiteURL,
			Row_Number() OVER (
				PARTITION BY c.ContactId
				,c.HierarchyClassID
				,c.ContactTypeId
				,c.Email ORDER BY c.ContactId DESC
				) rowId
		FROM @contact c
		JOIN ContactType ct ON ct.ContactTypeId = c.ContactTypeId
		JOIN HierarchyClass hc ON hc.hierarchyClassID = c.HierarchyClassID)
	SELECT *
	INTO #contact
	FROM cte
	WHERE rowId = 1;

	MERGE dbo.Contact WITH(UPDLOCK, ROWLOCK) c
	USING #contact c2 ON c2.ContactId = c.ContactId
	WHEN MATCHED THEN
		UPDATE
		SET ContactTypeId = c2.ContactTypeId
			,HierarchyClassID = c2.HierarchyClassID
			,ContactName = c2.ContactName
			,Email = c2.Email
			,Title = c2.Title
			,AddressLine1 = c2.AddressLine1
			,AddressLine2 = c2.AddressLine2
			,City = c2.City
			,STATE = c2.STATE
			,ZipCode = c2.ZipCode
			,Country = c2.Country
			,PhoneNumber1 = c2.PhoneNumber1
			,PhoneNumber2 = c2.PhoneNumber2
			,WebsiteURL = c2.WebsiteURL
			,ModifiedDate = GetDate()
	WHEN NOT MATCHED THEN
		INSERT (
			ContactTypeId
			,HierarchyClassID
			,ContactName
			,Email
			,Title
			,AddressLine1
			,AddressLine2
			,City
			,STATE
			,ZipCode
			,Country
			,PhoneNumber1
			,PhoneNumber2
			,WebsiteURL)
		VALUES (
			ContactTypeId
			,hierarchyClassID
			,ContactName
			,Email
			,Title
			,AddressLine1
			,AddressLine2
			,City
			,STATE
			,ZipCode
			,Country
			,PhoneNumber1
			,PhoneNumber2
			,WebsiteURL);

	IF (object_id('tempdb..#contact') IS NOT NULL)
		DROP TABLE #contact;
END
