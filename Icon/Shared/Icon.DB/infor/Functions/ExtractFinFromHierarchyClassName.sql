CREATE FUNCTION [dbo].[ExtractFinFromHierarchyClassName]
(
	@HierarchyClassName nvarchar(255)
)
RETURNS nvarchar(255)
AS
BEGIN
	RETURN Substring(@HierarchyClassName, Len(@HierarchyClassName)-4,4)
END
