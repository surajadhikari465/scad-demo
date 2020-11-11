USE [ItemCatalog]
BEGIN
	DECLARE @TeamNo int = (SELECT Team_No FROM Team WHERE Team_Name = 'Grocery')

    -- Update statement here to modify Team_No for Coffee Subteam
	UPDATE Subteam
	SET Team_No = @TeamNo
	FROM SubTeam
	WHERE SubTeam_Name = 'Coffee'

	-- Update statement here to modify Team_No for Coffee Subteam in each store
	UPDATE sst
	SET sst.Team_No = st.Team_No
	FROM StoreSubTeam sst
	INNER JOIN SubTeam st ON sst.SubTeam_No = st.SubTeam_No
	WHERE st.SubTeam_Name = 'Coffee'
END
