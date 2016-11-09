CREATE FUNCTION infor.[GetHierarchyClassIdFromInfor] 
(
	@hierarchyId INT,
	@inforHierarchyClassId NVARCHAR(255)
)
RETURNS INT
AS
BEGIN
	DECLARE @Result INT

	IF @hierarchyId = 3
	BEGIN 
	SELECT @Result = hierarchyClassID
		FROM HierarchyClass
		WHERE hierarchyid = 3
		AND LEFT(hierarchyClassName,7) LIKE @inforHierarchyClassId
	END
	ELSE IF @hierarchyId = 5	
		BEGIN 
		SELECT @Result = hierarchyClassID
		FROM HierarchyClass
		WHERE hierarchyID = 5
		AND hierarchyClassName LIKE CONCAT('%',@inforHierarchyClassId,')')
	END
	ELSE
	BEGIN 
		SELECT @Result = @inforHierarchyClassId
	END
	RETURN @Result
END