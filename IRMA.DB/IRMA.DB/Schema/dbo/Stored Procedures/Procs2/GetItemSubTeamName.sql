CREATE PROCEDURE [dbo].[GetItemSubTeamName] 
	@ItemKey int,
	@SubTeamName varchar(100) OUTPUT
AS 
BEGIN

    SET @SubTeamName = 
		ISNULL((SELECT
			st.SubTeam_Name
		FROM
			SubTeam st
		JOIN
			Item i ON i.SubTeam_No = st.SubTeam_No
		WHERE
			i.Item_key = @ItemKey),'')

	SELECT @SubTeamName

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemSubTeamName] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemSubTeamName] TO [IRSUser]
    AS [dbo];
