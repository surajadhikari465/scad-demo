CREATE PROCEDURE [app].[AddUpdateContactType]
	@contact app.ContactTypeInputType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	IF (object_id('tempdb..#contact') IS NOT NULL)
		DROP TABLE #contact;

	WITH cte AS (
		SELECT c.ContactTypeId,
			   c.ContactTypeName,
			   c.Archived,
			   Row_Number() OVER (PARTITION BY c.ContactTypeId, c.ContactTypeName ORDER BY c.ContactTypeId) rowId
		FROM @contact c)
	SELECT ContactTypeId, ContactTypeName, Archived
	INTO #contact
	FROM cte
	WHERE rowId = 1;

	MERGE dbo.ContactType WITH(UPDLOCK, ROWLOCK) c
	USING #contact c2 ON c2.ContactTypeId = c.ContactTypeId
	WHEN MATCHED THEN
		UPDATE SET ContactTypeName = c2.ContactTypeName,
				   Archived = c2.Archived
	WHEN NOT MATCHED THEN
		INSERT(ContactTypeName, Archived)
		VALUES(ContactTypeName, Archived);

	IF (object_id('tempdb..#contact') IS NOT NULL)
		DROP TABLE #contact;
END