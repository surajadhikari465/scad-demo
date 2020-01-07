CREATE PROCEDURE app.DeleteContact
    @ids app.IntList READONLY
AS
BEGIN
	SET NOCOUNT ON;

	IF (object_id('tempdb..#ids') IS NOT NULL)
		DROP TABLE #ids;

	SELECT DISTINCT I AS Id
	INTO #ids
	FROM @ids;

	DELETE FROM dbo.Contact
	WHERE ContactId IN(SELECT Id FROM #ids);

	IF (object_id('tempdb..#ids') IS NOT NULL)
		DROP TABLE #ids;
END
GO
