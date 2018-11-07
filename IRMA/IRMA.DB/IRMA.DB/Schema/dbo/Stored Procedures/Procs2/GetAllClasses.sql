CREATE Procedure dbo.GetAllClasses
AS

    SELECT   ClassName, ClassID
    FROM     NatItemClass
    ORDER BY ClassName