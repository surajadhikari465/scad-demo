CREATE PROCEDURE dbo.EIM_LookUpHierarchyIds
    @SubTeamName varchar(200),
    @CategoryName varchar(200),
    @Level3Name varchar(200),
    @Level4Name varchar(200)
    ,
    @SubTeamId int OUTPUT,
    @CategoryId int OUTPUT,
    @Level3Id int OUTPUT,
    @Level4Id int OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    SET @CategoryId = -1
    SET @Level3Id = -1
    SET @Level4Id = -1

	SELECT @SubTeamId = Subteam_No FROM Subteam (NOLOCK)
	WHERE Lower(Subteam_Name) = Lower(@SubTeamName)
	
	SET @SubTeamId = IsNull(@SubTeamId, -1)
	
	IF @SubTeamId > -1
	BEGIN
		SELECT @CategoryId = IsNull(Category_ID, -1) FROM ItemCategory (NOLOCK)
		WHERE Lower(Category_Name) = Lower(@CategoryName) AND SubTeam_No = @SubTeamId
		
		IF @CategoryId > -1
		BEGIN
			SELECT @Level3Id = IsNull(ProdHierarchyLevel3_ID, -1) FROM ProdHierarchyLevel3 (NOLOCK)
			WHERE Lower(Description) = Lower(@Level3Name) AND Category_ID = @CategoryId
			
			IF @Level3Id > -1
			BEGIN
				SELECT @Level4Id = IsNull(ProdHierarchyLevel4_ID, -1) FROM ProdHierarchyLevel4 (NOLOCK)
				WHERE Lower(Description) = Lower(@Level4Name) AND ProdHierarchyLevel3_ID = @Level3Id
			END
		END
	END
		
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_LookUpHierarchyIds] TO [IRMAClientRole]
    AS [dbo];

