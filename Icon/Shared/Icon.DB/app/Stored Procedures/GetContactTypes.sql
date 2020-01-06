CREATE PROCEDURE [app].[GetContactTypes]
    @includeArchived bit = NULL
AS
    SELECT ContactTypeId, ContactTypeName, Archived
    FROM dbo.ContactType
    WHERE Archived = IsNull(@includeArchived, 0) OR @includeArchived = 1;