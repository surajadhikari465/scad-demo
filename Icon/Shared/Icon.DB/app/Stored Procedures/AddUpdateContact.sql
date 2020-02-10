--Formatted with PoorSQL
CREATE PROCEDURE [app].[AddUpdateContact]
	@userName NVARCHAR(255) = NULL,
	@contact app.ContactInputType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @traitId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'HCU'),
			@dateNow DATETIME = GetDate();
	DECLARE @traitValue NVARCHAR(256) = 'Last updated by ' + IsNull(@userName, 'Unknown') + FORMAT (@dateNow, ' on MM-dd-yyyy'' at ''hh:mm:ss tt');

	IF (object_id('tempdb..#contact') IS NOT NULL) DROP TABLE #contact;

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
				,c.Email ORDER BY c.ContactId DESC) rowId
		FROM @contact c
		JOIN dbo.ContactType ct ON ct.ContactTypeId = c.ContactTypeId
		JOIN dbo.HierarchyClass hc ON hc.hierarchyClassID = c.HierarchyClassID)
	SELECT *
	INTO #contact
	FROM cte
	WHERE rowId = 1;

    --Find existing records to match new one
	UPDATE ct
	SET ContactId = c.ContactId
	FROM #contact ct
	JOIN dbo.Contact c ON c.HierarchyClassID = ct.HierarchyClassId
	  AND c.ContactTypeId = ct.ContactTypeId
	  AND c.Email = ct.Email
	WHERE ct.ContactId = 0; 

	UPDATE c
	SET ContactTypeId = c2.ContactTypeId
		,HierarchyClassID = c2.HierarchyClassID
		,ContactName = c2.ContactName
		,Email = c2.Email
		,Title = c2.Title
		,AddressLine1 = c2.AddressLine1
		,AddressLine2 = c2.AddressLine2
		,City = c2.City
		,State = c2.State
		,ZipCode = c2.ZipCode
		,Country = c2.Country
		,PhoneNumber1 = c2.PhoneNumber1
		,PhoneNumber2 = c2.PhoneNumber2
		,WebsiteURL = c2.WebsiteURL
		,ModifiedDate = @dateNow
	FROM dbo.Contact c
	JOIN #contact c2 ON c2.ContactId = c.ContactId;

	INSERT INTO dbo.Contact (
		ContactTypeId
		,HierarchyClassID
		,ContactName
		,Email
		,Title
		,AddressLine1
		,AddressLine2
		,City
		,State
		,ZipCode
		,Country
		,PhoneNumber1
		,PhoneNumber2
		,WebsiteURL
		)
	SELECT c2.ContactTypeId
		,c2.HierarchyClassID
		,c2.ContactName
		,c2.Email
		,c2.Title
		,c2.AddressLine1
		,c2.AddressLine2
		,c2.City
		,c2.State
		,c2.ZipCode
		,c2.Country
		,c2.PhoneNumber1
		,c2.PhoneNumber2
		,c2.WebsiteURL
	FROM #contact c2
	LEFT JOIN dbo.Contact c ON c.ContactId = c2.ContactId
	WHERE c2.ContactId = 0 AND c.ContactId IS NULL;

	UPDATE hct
	SET traitValue = @traitValue
	FROM dbo.HierarchyClassTrait hct
	JOIN #contact c ON c.HierarchyClassId = hct.HierarchyClassID AND hct.traitID = @traitId;

	INSERT INTO dbo.HierarchyClassTrait (
		c.HierarchyClassID
		,traitID
		,traitValue)
	SELECT c.HierarchyClassId
		,@traitId
		,@traitValue
	FROM #contact c
	LEFT JOIN dbo.HierarchyClassTrait hct ON hct.HierarchyClassID = c.HierarchyClassId
		AND hct.traitID = @traitId
	WHERE hct.HierarchyClassID IS NULL;

	IF (object_id('tempdb..#contact') IS NOT NULL) DROP TABLE #contact;
END