CREATE PROCEDURE [app].[GetContactTypes]
    @isIncludeArchived bit = NULL
AS
    SELECT ContactTypeId, ContactTypeName, Archived
    FROM dbo.ContactType
    WHERE Archived = IsNull(@isIncludeArchived, 0);