CREATE PROCEDURE app.DeleteContactType @ids app.IntList READONLY
AS
BEGIN
	SET NOCOUNT ON;

	IF (object_id('tempdb..#ids') IS NOT NULL)
		DROP TABLE #ids;

	SELECT DISTINCT I AS Id
	INTO #ids
	FROM @ids;

	DELETE FROM dbo.Contact
	WHERE ContactTypeId IN(SELECT Id FROM #ids);

	DELETE FROM dbo.ContactType
	WHERE ContactTypeId IN(SELECT Id FROM #ids);

	IF (object_id('tempdb..#ids') IS NOT NULL)
		DROP TABLE #ids;
END
GO