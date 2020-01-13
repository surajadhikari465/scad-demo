CREATE PROCEDURE app.DeleteContact
	@userName NVARCHAR(255) = NULL,
	@ids app.IntList READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @traitId INT = (SELECT traitID FROM dbo.Trait WHERE traitCode = 'HCU'),
			@traitValue NVARCHAR(256) = 'Last updated by ' + IsNull(@userName, 'Unknown') + FORMAT (GetDate(), ' on MM-dd-yyyy'' at ''hh:mm:ss');

	IF (object_id('tempdb..#ids') IS NOT NULL) DROP TABLE #ids;
	IF (object_id('tempdb..#hcIds') IS NOT NULL)DROP TABLE #hcIds;

	SELECT DISTINCT I AS Id
	INTO #ids
	FROM @ids;

	SELECT HierarchyClassID
	INTO #hcIds
	FROM dbo.Contact c
	JOIN #ids i ON i.Id = c.ContactId;

	DELETE FROM dbo.Contact
	WHERE ContactId IN (SELECT Id FROM #ids);

	--Sync dbo.HierarchyClassTrait
	UPDATE hct
	SET traitValue = @traitValue
	FROM dbo.HierarchyClassTrait hct
	JOIN #hcIds hc ON hc.HierarchyClassId = hct.HierarchyClassID
		AND hct.traitID = @traitId;

	INSERT INTO dbo.HierarchyClassTrait (
		hc.HierarchyClassID
		,traitID
		,traitValue)
	SELECT hc.HierarchyClassId
		,@traitId
		,@traitValue
	FROM #hcIds hc
	LEFT JOIN dbo.HierarchyClassTrait hct ON hct.HierarchyClassID = hc.HierarchyClassId
		AND hct.traitID = @traitId
	WHERE hct.HierarchyClassID IS NULL;

	IF (object_id('tempdb..#ids') IS NOT NULL) DROP TABLE #ids;
	IF (object_id('tempdb..#hcIds') IS NOT NULL) DROP TABLE #hcIds;
END
GO