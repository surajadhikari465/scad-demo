CREATE PROCEDURE [app].[PDX_MerchHierarchyFile]
AS 
BEGIN
SET NOCOUNT ON
set transaction isolation level read uncommitted

;WITH Segment
AS (
       select hierarchyClassID, hierarchyClassName from hierarchyClass
       where hierarchyID = 1
          and hierarchyLevel = 1
       ),
Family
AS (
       select hierarchyClassID, hierarchyClassName, hierarchyParentClassID from hierarchyClass
       where hierarchyID = 1
          and hierarchyLevel = 2
       ),
Class
AS (
       select hierarchyClassID, hierarchyClassName, hierarchyParentClassID from hierarchyClass
       where hierarchyID = 1
          and hierarchyLevel = 3
       ),
Brick
AS (
       select hierarchyClassID, hierarchyClassName, hierarchyParentClassID from hierarchyClass
       where hierarchyID = 1
          and hierarchyLevel = 4
       ),
Subbrick
AS (
       select hierarchyClassID, hierarchyClassName, hierarchyParentClassID from hierarchyClass
       where hierarchyID = 1
          and hierarchyLevel = 5
       )
select s.hierarchyClassID as SEGEMENT_HIERARCHYCLASSID,
              dbo.fn_RemoveSpecialChars(s.hierarchyClassName) as SEGEMENT_HIERARCHY_LABEL,
              f.hierarchyClassID as FAMILY_HIERARCHYCLASSID,
              dbo.fn_RemoveSpecialChars(f.hierarchyClassName) as FAMILY_HIERARCHYCLASS_LABEL,
              c.hierarchyClassID as CLASS_HIERARCHYCLASSID,
              dbo.fn_RemoveSpecialChars(c.hierarchyClassName) as CLASS_HIERARCHYCLASS_LABEL,
              b.hierarchyClassID as BRICK_HIERARCHYCLASSID,
              dbo.fn_RemoveSpecialChars(b.hierarchyClassName) as BRICK_HIERARCHYCLASS_LABEL,
              sb.hierarchyClassID as SUBBRICK_HIERARCHYCLASSID, 
              dbo.fn_RemoveSpecialChars(sb.hierarchyClassName) as SUBBRICK_HIERARCHYCLASS_LABEL,
              sb.hierarchyParentClassID as HIERARCHYCLASSPARENTID
  from Subbrick sb
  join Brick b on sb.hierarchyParentClassID = b.hierarchyClassID
  join Class c on b.hierarchyParentClassID = c.hierarchyClassID
  join Family f on c.hierarchyParentClassID = f.hierarchyClassID
  join Segment s on f.hierarchyParentClassID = s.hierarchyClassID
order by SEGEMENT_HIERARCHY_LABEL, FAMILY_HIERARCHYCLASS_LABEL, CLASS_HIERARCHYCLASS_LABEL, BRICK_HIERARCHYCLASS_LABEL, SUBBRICK_HIERARCHYCLASS_LABEL
END